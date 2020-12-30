angular
    .module('angularApp', ['commfun', 'siyfion.sfTypeahead'])
    .controller('ctrl', ['$scope', '$http', 'workService', 'gridpage', function ($scope, $http, workService, gridpage) {
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
        $scope.openFamilyData = function (member_id) {
            workService.getMemberDetailAll(member_id)
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.isShow = true;
                    $scope.member_details = data.data;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        $scope.closeFamileData = function () {
            $scope.isShow = false;
            $scope.member_details = [];
        };
        $scope.isShow = false;
        $scope.upCheck = function (index) {
            var m = $scope.Grid_Items[index];
            $http.put(gb_approot + 'api/PutAction/PutMemberCheck', { member_id: m.member_id, repeat_mark: m.repeat_mark })
                .success(function (data, status, headers, config) {
                if (!data.result) {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
    }]);
