interface IRejectReport extends IScopeM<server.Orders_Detail> {

    DownLoadExcel_RejectReport(): void;
    downloadExcel: string;

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
    .controller('ctrl', ['$scope', '$http', 'workService', 'gridpage', '$sce', function (
    $scope: IRejectReport,
    $http: ng.IHttpService,
    workService: services.workService,
    gridpage,
    $sce: ng.ISCEService
    ) {


    var today: Date = new Date();
    var fday: Date = new Date(today.getFullYear() + "/1/1");//預設今年一月一日
    $scope.sd = {//預設日期為今天
        startDate: setDateS(fday),
        endDate: setDateS(today),
        order_sn: '',
        member_name:''
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

    $scope.DownLoadExcel_RejectReport = function () {
        var parm = [];
        parm.push('startDate=' + setDateS($scope.sd.startDate));
        parm.push('endDate=' + setDateS($scope.sd.endDate));
        parm.push('order_sn=' + setDateS($scope.sd.order_sn));
        parm.push('member_name=' + setDateS($scope.sd.member_name));
        parm.push('tid=' + uniqid());
        var url = gb_approot + 'ExcelReport/downloadExcel_RejectReport?' + parm.join('&');
        console.log(url);
        $scope.downloadExcel = url;
    }

    $scope.test = function () {
        $scope.downloadExcel = gb_approot + 'ExcelReport/TEST?' + 't=' + uniqid();
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

    function setDateS(date) {
        if (date instanceof Date) {
            var dateS: String = date.getFullYear() + "/" + (date.getMonth() + 1) + "/" + date.getDate();
            return dateS
        } else {
            return date;
        }
    }
}]);

