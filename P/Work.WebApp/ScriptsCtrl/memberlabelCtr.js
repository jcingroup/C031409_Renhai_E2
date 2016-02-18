angular.module('angularApp', ['commfun', 'siyfion.sfTypeahead', 'ui.bootstrap']).controller('ctrl', ['$scope', '$http', 'workService', 'gridpage', '$sce', function ($scope, $http, workService, gridpage, $sce) {
    $scope.selectZipcode = commData.member_label_print;
    $scope.sd = {
        zipcode: '320',
        year: 2015
    };
    $scope.JumpPage = function (page) {
        $scope.NowPage = page;
        gridpage.CountPage($scope);
    };
    $scope.JumpPageKey = function () {
        gridpage.CountPage($scope);
    };
    $scope.Init_Query = function () {
        gridpage.CountPage($scope);
    };
    $scope.Init_Query();
    $scope.DownLoadExcel_PostPrint = function () {
        var parm = [];
        parm.push('zipcode=' + ($scope.sd.zipcode));
        parm.push('year=' + ($scope.sd.year));
        parm.push('tid=' + uniqid());
        var url = gb_approot + 'ExcelReport/downloadExcel_PostMember?' + parm.join('&');
        console.log(url);
        $scope.downloadExcel = url;
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
