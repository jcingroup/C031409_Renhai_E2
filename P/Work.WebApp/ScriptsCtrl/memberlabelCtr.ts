interface memberlabel_postprint extends IScopeM<server.Member_Detail> {
    DownLoadExcel_PostPrint(print_type: number): void;
    downloadExcel: string;

    //日曆小幫手測試
    disabled(date: Date, mode: string): void;
    openStart($event): void;
    openEnd($event): void;

    openedStart: boolean;
    openedEnd: boolean;
    dateOptions: any;
    //日曆小幫手測試

    test(): void;
    selectZipcode: IKeyValueS[];
}

angular
    .module('angularApp', ['commfun', 'siyfion.sfTypeahead', 'ui.bootstrap'])
    .controller('ctrl', ['$scope', '$http', 'workService', 'gridpage', '$sce', function (
    $scope: memberlabel_postprint,
    $http: ng.IHttpService,
    workService: services.workService,
    gridpage,
    $sce: ng.ISCEService
    ) {
    $scope.selectZipcode = commData.member_label_print;
    var today: Date = new Date();
    var fday: Date = new Date((today.getFullYear() - 1) + "/1/1");//預設前年一月一日
    var eday: Date = new Date((today.getFullYear() - 1) + "/12/31");//預設前年十二月三十一日
    $scope.sd = {
        zipcode: '320',
        startDate: setDateS(fday),
        endDate: setDateS(eday)
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

    $scope.DownLoadExcel_PostPrint = function (print_type) {
        if ($scope.sd.startDate == null || $scope.sd.startDate == undefined || $scope.sd.endDate == null || $scope.sd.endDate == undefined) {
            alert("訂單日期起迄日未填寫完整無法列印報表!");
            return;
        }
        var parm = [];
        parm.push('startDate=' + setDateS($scope.sd.startDate));
        parm.push('endDate=' + setDateS($scope.sd.endDate));
        parm.push('zipcode=' + ($scope.sd.zipcode));
        //parm.push('year=' + ($scope.sd.year)); 年拿掉改為列印日期區間
        if (print_type != null)
            parm.push('type=' + print_type);
        parm.push('tid=' + uniqid());
        var url = gb_approot + 'ExcelReport/downloadExcel_PostMember?' + parm.join('&');
        console.log(url);
        $scope.downloadExcel = url;
    }

    function setDateS(date) {//將日期轉成字串
        if (date instanceof Date) {
            var dateS: String = date.getFullYear() + "/" + (date.getMonth() + 1) + "/" + date.getDate();
            return dateS
        } else {
            return date;
        }
    }
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

