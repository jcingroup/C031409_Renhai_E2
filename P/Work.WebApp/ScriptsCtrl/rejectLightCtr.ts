interface IRejectLight extends IScopeM<server.Reject> {
    SubmitOrder(): void;
    SubmitCart(): void;

    mb: server.Member;
    pds: server.Product[];
    mjs: server.Manjushri[];
    cart: server.cartMaster;
    born_sign: string[];
    born_time: IKeyValueS[];
}

angular
    .module('angularApp', ['commfun', 'siyfion.sfTypeahead'])
    .controller('ctrl', ['$scope', '$http', 'workService', function ($scope: IRejectLight, $http: ng.IHttpService, workService: services.workService) {

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
                        //$scope.fd = data.data;
                        GetMemberAll(data.data.member_detail_id);
                    } else {
                        alert(data.message);
                    }
                })
                .error(function (data, status, headers, config) {
                    alert('ajax error' + data);
                });
        };

        function GetMemberAll(member_id) {
            workService.getMemberAll(member_id)
                .success(function (data: IResultData<server.Member>, status, headers, config) {
                    if (data.result) {
                        $scope.mb = data.data;
                    } else {
                        alert(data.message);
                    }
                })
                .error(function (data, status, headers, config) {
                    alert('ajax error' + data);
                });
        };

        function GetMemberByDetail(member_id) {
            //取得戶長資料
            workService.getMemberByDetail(member_id)
                .success(function (data: IResultData<server.Member_Detail>, status, headers, config) {
                    if (data.result) {

                        if (member_id != data.data.member_id) {
                            alert('系統回傳member_id不一致');
                            return;
                        }

                    } else {
                        alert(data.message);
                    }
                })
                .error(function (data, status, headers, config) {
                    alert('ajax error' + data);
                });
        };

        function GetProductAll() {
            workService.getProductAll()
                .success(function (data: IResultData<server.Product[]>, status, headers, config) {
                    if (data.result) {
                        $scope.pds = data.data;
                    } else {
                        alert(data.message);
                    }
                })
                .error(function (data, status, headers, config) {
                    alert('ajax error' + data);
                });
        };

        function GetManjushri() {
            workService.getManjushri()
                .success(function (data: IResultData<server.Manjushri[]>, status, headers, config) {
                    if (data.result) {
                        $scope.mjs = data.data;
                    } else {
                        alert(data.message);
                    }
                })
                .error(function (data, status, headers, config) {
                    alert('ajax error' + data);
                });
        };

        function GetMemberDetail(member_detail_id: number) {
            workService.getMemberDetail(member_detail_id)
                .success(function (data: IResultData<server.Member_Detail>, status, headers, config) {
                    if (data.result) {

                    } else {
                        alert(data.message);
                    }
                })
                .error(function (data, status, headers, config) {
                    alert('ajax error' + data);
                });;
        }
    }]);
