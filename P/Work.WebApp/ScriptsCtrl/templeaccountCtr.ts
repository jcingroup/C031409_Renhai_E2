interface Itemple_account extends IScopeM<server.TempleAccount> {
    DownLoadExcel_MemberPrint(): void;
    downloadExcel: string;
    godson_account_type_foreach: IKeyValueS[];//訂單類別
    godson_account_type: IKeyValueS[];//訂單類別
    users: server.Users[];//承辦人
    //繳款明細檢視
    openEditDetail(id: number): void;
    fds: server.TempleAccount;
    is_show_acct: boolean;
    closeAcct(): void;
    submitAcct(): void;
    print(): void;

    //日曆小幫手測試
    disabled(date: Date, mode: string): void;
    openStart($event): void;
    openEnd($event): void;

    openedStart: boolean;
    openedEnd: boolean;
    dateOptions: any;

    test(): void;
}

angular
    .module('angularApp', ['commfun', 'siyfion.sfTypeahead', 'ui.bootstrap'])
    .filter('godsonProductSn', function () {
    return function (input) {
        var r = input;
        var data: IKeyValueS[] = commData.godson_account_type;
        if (data) {
            for (var i in data) {
                if (input == data[i].value) {
                    r = data[i].label;
                    break;
                }
            }
        }
        return r;
    }
})
    .controller('ctrl', ['$scope', '$http', 'workService', 'gridpage', '$sce', function (
    $scope: Itemple_account,
    $http: ng.IHttpService,
    workService: services.workService,
    gridpage,
    $sce: ng.ISCEService
    ) {
    $scope.godson_account_type_foreach = commData.godson_account_type_forsearch;//契子產品編號下拉式選單
    $scope.godson_account_type = commData.godson_account_type;//顯示用
    var today: Date = new Date();
    var fday: Date = new Date(today.getFullYear() + "/1/1");
    $scope.sd = {//預設日期為今天
        startDate: setDateS(fday),
        endDate: setDateS(today),
        product_sn: '',
        InsertUserId: 0,
        account_sn: ''
    }
    $scope.JumpPage = function (page: number) {
        $scope.NowPage = page;
        gridpage.CountPage($scope);
    };
    $scope.JumpPageKey = function (page: number) {
        $scope.NowPage = page;
        gridpage.CountPage($scope);
    };
    $scope.Init_Query = function () {
        gridpage.CountPage($scope);
    };
    $scope.Init_Query();
    GetUsers();

    $scope.DownLoadExcel_MemberPrint = function () {
        var parm = [];
        parm.push('product_sn=' + $scope.sd.product_sn);
        parm.push('startDate=' + setDateS($scope.sd.startDate));
        parm.push('endDate=' + setDateS($scope.sd.endDate));
        parm.push('tid=' + uniqid());
        var url = gb_approot + 'ExcelReport/downloadExcel_MemberPrint?' + parm.join('&');
        console.log(url);
        $scope.downloadExcel = url;
    }

    $scope.test = function () {
        $scope.downloadExcel = gb_approot + 'ExcelReport/TEST?' + 't=' + uniqid();
    }

    function GetUsers() {
        workService.getUsers()
            .success(function (data: IResultData<server.Users[]>, status, headers, config) {
            if (data.result) {
                $scope.users = data.data;
            } else {
                alert(data.message);
            }
        })
            .error(function (data, status, headers, config) {
            showAjaxError(data);
        });
    }

    $scope.openEditDetail = function (id) {
        //繳款-修改編輯
        $http.get(gb_approot + 'api/TempleAccount', { params: { id: id } })
            .success(function (data: IResultData<server.TempleAccount>, status, headers, config) {
            if (data.result) {
                $scope.fds = obj_prop_date(data.data);
                $scope.is_show_acct = true;
                //$scope.detail_edit_type = IEditType.update;
            } else {
                alert(data.message);
            }
        })
            .error(function (data, status, headers, config) {
            showAjaxError(data);
        });
    }
    $scope.submitAcct = function () {//只有修改沒新增
        $http.put(gb_approot + 'api/TempleAccount', $scope.fds)
            .success(function (data: IResultBase, status, headers, config) {
            if (data.result) {
                $scope.Init_Query();
                alert('更新完成');
            } else {
                alert(data.message);
            }
        })
            .error(function (data, status, headers, config) {
            showAjaxError(data);
        });

    };
    $scope.closeAcct = function () {
        $scope.is_show_acct = false;
        $scope.fds = <server.TempleAccount>{};
        //$scope.detail_edit_type = IEditType.none;
    };
    $scope.print = function () {
        var url = gb_approot + 'ExcelReport/downloadExcel_GodSON?temple_account_id=' + $scope.fds.temple_account_id + '&t=' + uniqid();
        $scope.downloadExcel = $sce.getTrustedResourceUrl(url);
    }

    //日曆小幫手---start---
    // Disable weekend selection
    //$scope.disabled = function (date, mode) {
    //    return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
    //};

    $scope.openStart = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.openedStart = true;
    };
    $scope.openEnd = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.openedEnd = true;
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };
    //日曆小幫手---end-----

    function setDateS(date) {//將日期轉成字串
        if (date instanceof Date) {
            var dateS: String = date.getFullYear() + "/" + (date.getMonth() + 1) + "/" + date.getDate();
            return dateS
        } else {
            return date;
        }
    }
}]);

