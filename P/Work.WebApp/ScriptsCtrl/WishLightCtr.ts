interface IWishLight extends IScopeM<server.Orders> {
    sd: any;
    masterDelete($index): void //主檔單一筆刪除
    year_list: number[]

    //日曆小幫手測試
    disabled(date: Date, mode: string): void;
    openStart($event): void;
    openEnd($event): void;

    openedStart: boolean;
    openedEnd: boolean;
    dateOptions: any;
    //日曆小幫手測試

    //download excel
    downloadExcel: string;
    DownLoadExcel_WishLight(): void;//訂單芳名錄
    //download excel

    //新增今年度燈位
    AddSite($index): void;
}

interface IWishLightEdit extends IScopeM<server.cartDetail[]> {
    orders_sn: string;
    goGrid(): void;
    born_sign: string[];
    born_time: IKeyValueS[];
    wishs: server.Wish[];
    wishlen: number;

    pindex: number;

    checkWishList(index: number): void;
    changeWishText(id: number, val: any): void;
    ShowEditWish(parent:number): void;
    CloseEditWish(): void;
    isShowEditWish: boolean;

    Submit(): void;
}


angular
    .module('angularApp', ['commfun', 'siyfion.sfTypeahead', 'ngDialog', 'ui.router', 'ui.bootstrap'])
    .config(['$httpProvider', '$stateProvider', '$urlRouterProvider', function ($httpProvider,
    $stateProvider: ng.ui.IStateProvider,
    $urlRouterProvider: ng.ui.IUrlRouterProvider) {
    if (!$httpProvider.defaults.headers.get) {
        $httpProvider.defaults.headers.get = {};
    }
    $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    $httpProvider.defaults.headers.get['If-Modified-Since'] = '0';
    $urlRouterProvider.otherwise("/grid");
    $stateProvider
        .state('grid', {
        url: '/grid',
        templateUrl: 'dataGrid',
        controller: 'ctrl'
    })
        .state('edit', {
        url: '/edit?orders_sn',
        templateUrl: 'dataEdit',
        controller: 'ctrl_edit'
    })
}]);
var apiPath: string = gb_approot + 'api/GetAction/GetWishOrderList';
var apiGetEditPath: string = gb_approot + 'api/GetAction/GetWishOrderItem';
angular
    .module('angularApp')
    .controller('ctrl', ['$scope', '$http', 'gridpage', 'workService', '$state',
    function (
        $scope: IWishLight,
        $http: ng.IHttpService,
        gridpage,
        workService: services.workService,
        $state: ng.ui.IStateService
        ) {
        $scope.apiPath = apiPath;
        var today: Date = new Date();
        var fday: Date = new Date(today.getFullYear() + "/" + (today.getMonth()+1)+"/1");
        $scope.year_list = workService.setApplyYearRange();

        $scope.sd = {//預設日期為今天
            year: allowyear,
            startDate: setDateS(fday),
            endDate: setDateS(today)
        }
        $scope.show_master_edit = false;
        $scope.edit_type = 0;// IEditType.none; //ref 2
        var timer = false; //ref 3

        $scope.grid_new_show = true;
        $scope.grid_del_show = true;
        $scope.grid_nav_show = true;

        $scope.check_del_value = true;

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
        $scope.ExpandSub = function ($index) {
            //$scope.Grid_Items[$index].expland_sub = !$scope.Grid_Items[$index].expland_sub;
        };

        $scope.Init_Query(); //第一次進入


        $scope.DownLoadExcel_WishLight = function () {
            if ($scope.sd.year == null || $scope.sd.year == undefined) {
                alert("請選擇「年度」後再列印名冊！")
                return;
            }
            var parm = [];
            parm.push('year=' + $scope.sd.year);
            parm.push('startDate=' + setDateS($scope.sd.startDate));
            parm.push('endDate=' + setDateS($scope.sd.endDate));
            parm.push('tid=' + uniqid());
            var url = gb_approot + 'ExcelReport/WishRoll?' + parm.join('&');
            $scope.downloadExcel = url;
        }
        $scope.AddSite = function (row) {
            console.log(row);
            $http.get(gb_approot + 'WishLight/addPlace', { params: { row: row } })
                .success(function (data: IResultBase, status, headers, config) {
                if (data.result) {
                    alert('新增燈位第' + row + '排燈位');
                    //$scope.Init_Query();
                } else {
                    alert(data.message);
                }
            });
        };

        function setDateS(date) {//將日期轉成字串
            if (date instanceof Date) {
                var dateS: String = date.getFullYear() + "/" + (date.getMonth() + 1) + "/" + date.getDate();
                return dateS
            } else {
                return $.extend({}, date);
            }
        }

        $scope.Master_Open_Modify = function ($index) {
            var get_id = $scope.Grid_Items[$index].orders_sn;
            $state.go('edit', { 'orders_sn': get_id });
        };

        //日曆小幫手---start---
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
    }]);

angular
    .module('angularApp')
    .controller('ctrl_edit', ['$scope', '$http', 'gridpage', 'workService', '$state',
    function (
        $scope: IWishLightEdit,
        $http: ng.IHttpService,
        gridpage,
        workService: services.workService,
        $state: ng.ui.IStateService
        ) {

        $scope.born_sign = commData.born_sign;
        $scope.born_time = commData.born_time;

        $scope.goGrid = function () {
            $state.go('grid');
            $scope.Init_Query();
        }
        $scope.ShowEditWish = function (pindex) {
            $scope.pindex = pindex;
            SetWishList($scope.fd[pindex].wishs);
            $scope.isShowEditWish = true;
        };
        $scope.CloseEditWish = function () {
            $scope.pindex = null;
            $scope.isShowEditWish = false;
        };
        function getMasterData(id: string) {
            $http.get(apiGetEditPath, { params: { orders_sn: id } })
                .success(function (data: IResultData<server.cartDetail[]>, status, headers, config) {
                if (data.result) {
                    $scope.fd = obj_prop_date(data.data);
                    setWishUpdate($scope.fd);
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        function setWishUpdate(fd: server.cartDetail[]) {
            fd.forEach(function(x){
                x.wishs.forEach(function(y){
                    y.edit_type = IEditType.update;
                });
            });
        }

        function GetWishList() {
            workService.getWishList()
                .success(function (data: IResultData<server.Wish[]>, status, headers, config) {
                if (data.result) {
                    data.data.forEach((o) => {
                        if (!o.can_text)
                            o.wish_text = o.wish_name;
                    });
                    $scope.wishs = data.data;
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        function SetWishList(wish: server.WishText[]) {
            $scope.wishlen = wish.length;
            $scope.wishs.forEach((i) => {
                var obj = wish.filter(x=> x.wish_id == i.wish_id);
                if (obj.length > 0) {
                    i.wish_checked = 1;
                    i.wish_text = obj[0].wish_text;
                } else {
                    i.wish_checked = 0;
                    if (i.can_text)
                        i.wish_text = null;
                }
            });
        }
        $scope.checkWishList = function ($index) {
            var items = $scope.wishs;
            var item = items[$index];
            var select = items.filter(x=> x.wish_checked);
            $scope.wishlen = select.length;

            if (item.wish_checked) {
                var obj: server.WishText = {
                    wish_id: item.wish_id,
                    wish_text: item.wish_text,
                    can_text: item.can_text,
                    edit_type:IEditType.insert
                };
                $scope.fd[$scope.pindex].wishs.push(obj);
            } else if (!item.wish_checked){
                var i = findIndex($scope.fd[$scope.pindex].wishs, "wish_id", item.wish_id);

                $scope.fd[$scope.pindex].wishs.splice(i,1);
                
                if (item.can_text)
                    item.wish_text = null;
            }
        }
        $scope.changeWishText = function (id, text) {
            var i = findIndex($scope.fd[$scope.pindex].wishs, "wish_id", id);
            $scope.fd[$scope.pindex].wishs[i].wish_text = text;
        }


        $scope.Submit = function () {
            var pm = { orders_id: $scope.orders_sn , order_data:$scope.fd};
            $http.post(gb_approot + 'WishLight/updateWishOrder', pm)
                .success(function (data: IResultBase, status, headers, config) {
                if (data.result) {
                    alert('更新完成');
                } else {
                    alert('更新失敗:' + data.message);
                }
            });
        };


        if ($state.params.orders_sn != undefined) { // 進入為修改模式
            var get_id: any = $state.params.orders_sn;
            $scope.orders_sn = get_id;
            $scope.edit_type = IEditType.update;
            getMasterData(get_id);
        } else { //進入為新增模式
            //$scope.edit_type = IEditType.insert;
            //$scope.fd = <server.Orders_Detail[]>[];
        }
        GetWishList();
    }]);


