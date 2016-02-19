angular.module('angularApp', ['commfun', 'siyfion.sfTypeahead', 'ui.bootstrap']).filter('godsonProductSn', function () {
    return function (input) {
        var r = input;
        var data = commData.godson_account_type;
        if (data) {
            for (var i in data) {
                if (input == data[i].value) {
                    r = data[i].label;
                    break;
                }
            }
        }
        return r;
    };
}).controller('ctrl', ['$scope', '$http', 'workService', 'gridpage', '$sce', function ($scope, $http, workService, gridpage, $sce) {
    $scope.godson_account_type = commData.godson_account_type_forsearch;
    var today = new Date();
    var fday = new Date(today.getFullYear() + "/1/1");
    $scope.sd = {
        startDate: setDateS(fday),
        endDate: setDateS(today),
        product_sn: '753'
    };
    $scope.JumpPage = function (page) {
        $scope.NowPage = page;
        gridpage.CountPage($scope);
    };
    $scope.JumpPageKey = function (page) {
        $scope.NowPage = page;
        gridpage.CountPage($scope);
    };
    $scope.Init_Query = function () {
        gridpage.CountPage($scope);
    };
    $scope.Init_Query();
    $scope.DownLoadExcel_MemberPrint = function () {
        var parm = [];
        parm.push('product_sn=' + $scope.sd.product_sn);
        parm.push('startDate=' + setDateS($scope.sd.startDate));
        parm.push('endDate=' + setDateS($scope.sd.endDate));
        parm.push('tid=' + uniqid());
        var url = gb_approot + 'ExcelReport/downloadExcel_MemberPrint?' + parm.join('&');
        console.log(url);
        $scope.downloadExcel = url;
    };
    $scope.test = function () {
        $scope.downloadExcel = gb_approot + 'ExcelReport/TEST?' + 't=' + uniqid();
    };
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
    function setDateS(date) {
        if (date instanceof Date) {
            var dateS = date.getFullYear() + "/" + (date.getMonth() + 1) + "/" + date.getDate();
            return dateS;
        }
        else {
            return date;
        }
    }
}]);
