interface Itemple_postprint extends IScopeM<server.TempleMember> {
    DownLoadExcel_PostPrint(): void;
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
    $scope: Itemple_postprint,
    $http: ng.IHttpService,
    workService: services.workService,
    gridpage,
    $sce: ng.ISCEService
    ) {

    var today: Date = new Date();
    //var fday: Date = new Date(today.getFullYear() + "/1/1");
    var fday: Date = new Date("1988/1/1");//最小入會日期
    $scope.sd = {//預設日期為今天
        startDate: setDateS(fday),
        endDate: setDateS(today),
        product_sn: '',
        pageclass: 'postprint'
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

    $scope.DownLoadExcel_PostPrint = function () {
        var parm = [];
        parm.push('startDate=' + setDateS($scope.sd.startDate));
        parm.push('endDate=' + setDateS($scope.sd.endDate));
        parm.push('tid=' + uniqid());
        var url = gb_approot + 'ExcelReport/downloadExcel_PostPrint?' + parm.join('&');
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

    function setDateS(date) {//將日期轉成字串
        if (date instanceof Date) {
            var dateS: String = date.getFullYear() + "/" + (date.getMonth() + 1) + "/" + date.getDate();
            return dateS
        } else {
            return date;
        }
    }
}]);

