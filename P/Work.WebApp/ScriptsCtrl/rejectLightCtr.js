angular
    .module('angularApp', ['commfun', 'siyfion.sfTypeahead'])
    .controller('ctrl', ['$scope', '$http', 'workService', function ($scope, $http, workService) {
        $scope.born_sign = commData.born_sign;
        $scope.born_time = commData.born_time;
        $scope.SubmitOrder = function () {
        };
        $scope.SubmitCart = function () {
            $http.post(gb_approot + 'Cart/Post', $scope.cart);
        };
        function GetOrder(order_sn) {
            workService.getOrderData(order_sn)
                .success(function (data, status, headers, config) {
                if (data.result) {
                    GetMemberAll(data.data.member_detail_id);
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                alert('ajax error' + data);
            });
        }
        ;
        function GetMemberAll(member_id) {
            workService.getMemberAll(member_id)
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.mb = data.data;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                alert('ajax error' + data);
            });
        }
        ;
        function GetMemberByDetail(member_id) {
            workService.getMemberByDetail(member_id)
                .success(function (data, status, headers, config) {
                if (data.result) {
                    if (member_id != data.data.member_id) {
                        alert('系統回傳member_id不一致');
                        return;
                    }
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                alert('ajax error' + data);
            });
        }
        ;
        function GetProductAll() {
            workService.getProductAll()
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.pds = data.data;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                alert('ajax error' + data);
            });
        }
        ;
        function GetManjushri() {
            workService.getManjushri()
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.mjs = data.data;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                alert('ajax error' + data);
            });
        }
        ;
        function GetMemberDetail(member_detail_id) {
            workService.getMemberDetail(member_detail_id)
                .success(function (data, status, headers, config) {
                if (data.result) {
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                alert('ajax error' + data);
            });
            ;
        }
    }]);
