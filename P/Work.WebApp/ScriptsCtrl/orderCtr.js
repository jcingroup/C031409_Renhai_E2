var callOnName;
(function (callOnName) {
    callOnName.updateCartMaster_is_light_serial = 'updateCartMaster_is_light_serial';
})(callOnName || (callOnName = {}));
angular
    .module('angularApp', ['commfun', 'siyfion.sfTypeahead', 'ngDialog', 'ui.router', 'ngCookies'])
    .config(['$httpProvider', '$stateProvider', '$urlRouterProvider', function ($httpProvider, $stateProvider, $urlRouterProvider) {
        if (!$httpProvider.defaults.headers.get) {
            $httpProvider.defaults.headers.get = {};
        }
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
        $httpProvider.defaults.headers.get['If-Modified-Since'] = '0';
        $urlRouterProvider.otherwise("/grid");
        $stateProvider
            .state('grid', {
            url: '/grid',
            templateUrl: 'dataGrid'
        })
            .state('edit', {
            url: '/edit?member_id',
            templateUrl: 'dataEdit',
            controller: 'ctrl_master'
        })
            .state('edit.general_orders', {
            url: "/gorders?orders_sn&member_id",
            templateUrl: "dataEditGeneralOrders",
            controller: 'ctrl_general'
        })
            .state('edit.fortune_orders', {
            url: "/forders?orders_sn&member_id",
            templateUrl: "dataEditFortuneOrders",
            controller: 'ctrl_fortune'
        })
            .state('edit.sdlight_orders', {
            url: "/sdorders?orders_sn&member_id",
            templateUrl: "dataEditSDLightOrders",
            controller: 'ctrl_sdlight'
        })
            .state('edit.mdlight_orders', {
            url: "/mdorders?orders_sn&member_id",
            templateUrl: "dataEditMDLightOrders",
            controller: 'ctrl_mdlight'
        })
            .state('edit.wishlight_orders', {
            url: "/wishorders?orders_sn&member_id",
            templateUrl: "dataEditWishLightOrders",
            controller: 'ctrl_wishlight'
        }).state('edit.doulight_orders', {
            url: "/doulightorders?orders_sn&member_id",
            templateUrl: "dataEditDouLightOrders",
            controller: 'ctrl_doulight'
        });
    }]);
angular.module('angularApp')
    .controller('ctrl', ['$scope', '$http', 'workService', 'gridpage', '$sce', 'ngDialog', '$state', '$cookies', function ($scope, $http, workService, gridpage, $sce, ngDialog, $state, $cookies) {
        console.log('ctrl');
        $scope.orders_type = commData.Orders_type;
        var p0 = 0;
        $scope.sd = {
            InsertUserId: 0,
            order_sn: ''
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
        $scope.GoGrid = function () {
            $state.go('grid');
        };
        $scope.openOrders = function (orders_sn, orders_type) {
            if (orders_type == 0) {
                $state.go('edit.general_orders', { 'orders_sn': orders_sn });
            }
            if (orders_type == 3) {
                $state.go('edit.fortune_orders', { 'orders_sn': orders_sn });
            }
            if (orders_type == 2) {
                $state.go('edit.sdlight_orders', { 'orders_sn': orders_sn });
            }
            if (orders_type == 1) {
                $state.go('edit.mdlight_orders', { 'orders_sn': orders_sn });
            }
            if (orders_type == 4) {
                $state.go('edit.wishlight_orders', { 'orders_sn': orders_sn });
            }
            if (orders_type == 5) {
                $state.go('edit.doulight_orders', { 'orders_sn': orders_sn });
            }
        };
        function GetUsers() {
            workService.getUsers()
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.users = data.data;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        $scope.allowReject = $cookies.allowReject;
        if (p0 > 0) {
        }
        else {
            if (member_id > 0) {
                $scope.edit_type = 1;
            }
        }
        $scope.Init_Query();
        GetUsers();
    }]);
angular.module('angularApp')
    .controller('ctrl_master', ['$scope', '$http', 'workService', '$sce', 'ngDialog', '$state', '$q', function ($scope, $http, workService, $sce, ngDialog, $state, $q) {
        console.log('ctrl_master');
        var p0 = 0;
        var allowSetPrice = ['香油'];
        var allowSetRace = ['白米'];
        var allowSetGold = ['金牌'];
        var allowSetGodSon = ['契子'];
        $scope.born_sign = commData.born_sign;
        $scope.born_time = commData.born_time;
        $scope.isShowEdit = false;
        $scope.isShowEditProduct = false;
        $scope.cart_price_disable = true;
        $scope.cart_race_disable = true;
        $scope.cart_gold_disable = true;
        $scope.CloseEdit = function () {
            $scope.fd = null;
            $scope.isShowEdit = false;
        };
        $scope.SubmitOrder = function () {
            if ($state.current.name == 'edit.general_orders') {
                SendGeneralOrder();
            }
            if ($state.current.name == 'edit.fortune_orders') {
                SendFortuneOrder();
            }
            if ($state.current.name == 'edit.sdlight_orders') {
                SendSDLightOrder();
            }
            if ($state.current.name == 'edit.mdlight_orders') {
                SendMDLightOrder();
            }
            if ($state.current.name == 'edit.wishlight_orders') {
                SendWishLightOrder();
            }
            if ($state.current.name == 'edit.doulight_orders') {
                SendDouLightOrder();
            }
        };
        function SendGeneralOrder() {
            if ($scope.edit_type == 1) {
                $http.post(gb_approot + 'Orders/AddOrders', $scope.fd)
                    .success(function (data, status, headers, config) {
                    if (data.result) {
                        $http.get(gb_approot + 'Cart/OrdersToSession', { params: { orders_sn: data.data } })
                            .success(function (data, status, headers, config) {
                            if (data.result) {
                                $scope.fd = data.data;
                                $scope.edit_type = 2;
                                $scope.$parent.Init_Query();
                                alert('訂單新增完成');
                            }
                            else {
                                alert(data.message);
                            }
                        })
                            .error(function (data, status, headers, config) {
                            showAjaxError(data);
                        });
                    }
                    else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            }
            ;
            if ($scope.edit_type == 2) {
                $http.put(gb_approot + 'Orders/UpdateOrders', $scope.fd)
                    .success(function (data, status, headers, config) {
                    if (data.result) {
                        $scope.edit_type = 2;
                        $scope.$parent.Init_Query();
                        alert('訂單修改完成');
                    }
                    else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            }
            ;
        }
        ;
        function SendFortuneOrder() {
            console.log($scope.fd.Item);
            var item = $scope.fd.Item[0];
            if ($scope.edit_type == 1) {
                var m = [];
                for (var i in $scope.mb.getMember_Detail) {
                    var n = $scope.mb.getMember_Detail[i];
                    if (n.fortune_value > 0) {
                        m.push(n.fortune_value);
                    }
                }
                $http.post(gb_approot + 'Orders/AddOrdersFortune', {
                    'master': $scope.fd,
                    'product_sn': item.product_sn,
                    'member_detail_ids': m
                })
                    .success(function (data, status, headers, config) {
                    if (data.result) {
                        $http.get(gb_approot + 'Cart/OrdersToSession', { params: { orders_sn: data.data } })
                            .success(function (data, status, headers, config) {
                            if (data.result) {
                                $scope.fd = data.data;
                                $scope.edit_type = 2;
                                $scope.$parent.Init_Query();
                                alert('訂單新增完成');
                            }
                            else {
                                alert(data.message);
                            }
                        })
                            .error(function (data, status, headers, config) {
                            showAjaxError(data);
                        });
                    }
                    else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            }
            ;
            if ($scope.edit_type == 2) {
                var m = [];
                for (var i in $scope.mb.getMember_Detail) {
                    var n = $scope.mb.getMember_Detail[i];
                    if (n.fortune_value > 0) {
                        m.push(n.fortune_value);
                    }
                }
                $http.put(gb_approot + 'Orders/UpdateOrdersFortune', {
                    'master': $scope.fd,
                    'member_detail_ids': m
                })
                    .success(function (data, status, headers, config) {
                    if (data.result) {
                        $scope.edit_type = 2;
                        $scope.$parent.Init_Query();
                        alert('訂單修改完成');
                    }
                    else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            }
            ;
        }
        ;
        function SendSDLightOrder() {
            if ($scope.edit_type == 1) {
                $http.post(gb_approot + 'Orders/AddSDLight', $scope.fd)
                    .success(function (data, status, headers, config) {
                    if (data.result) {
                        $http.get(gb_approot + 'Cart/OrdersToSession', { params: { orders_sn: data.data } })
                            .success(function (data, status, headers, config) {
                            if (data.result) {
                                $scope.fd = data.data;
                                $scope.edit_type = 2;
                                $scope.$parent.Init_Query();
                                alert('訂單新增完成');
                            }
                            else {
                                alert(data.message);
                            }
                        })
                            .error(function (data, status, headers, config) {
                            showAjaxError(data);
                        });
                    }
                    else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            }
            ;
            if ($scope.edit_type == 2) {
                $http.put(gb_approot + 'Orders/UpdateSDLight', $scope.fd)
                    .success(function (data, status, headers, config) {
                    if (data.result) {
                        $scope.edit_type = 2;
                        $scope.$parent.Init_Query();
                        alert('訂單修改完成');
                    }
                    else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            }
            ;
        }
        ;
        function SendMDLightOrder() {
            if ($scope.edit_type == 1) {
                $http.post(gb_approot + 'Orders/AddMDLight', $scope.fd)
                    .success(function (data, status, headers, config) {
                    if (data.result) {
                        $http.get(gb_approot + 'Cart/OrdersToSession', { params: { orders_sn: data.data } })
                            .success(function (data, status, headers, config) {
                            if (data.result) {
                                $scope.fd = data.data;
                                $scope.edit_type = 2;
                                $scope.$parent.Init_Query();
                                alert('訂單新增完成');
                            }
                            else {
                                alert(data.message);
                            }
                        })
                            .error(function (data, status, headers, config) {
                            showAjaxError(data);
                        });
                    }
                    else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            }
            ;
            if ($scope.edit_type == 2) {
                $http.put(gb_approot + 'Orders/UpdateMDLight', $scope.fd)
                    .success(function (data, status, headers, config) {
                    if (data.result) {
                        $scope.edit_type = 2;
                        $scope.$parent.Init_Query();
                        alert('訂單修改完成');
                    }
                    else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            }
            ;
        }
        ;
        function SendWishLightOrder() {
            if ($scope.edit_type == 1) {
                $http.post(gb_approot + 'Orders/AddWishLight', $scope.fd)
                    .success(function (data, status, headers, config) {
                    if (data.result) {
                        $http.get(gb_approot + 'Cart/OrdersToSession', { params: { orders_sn: data.data } })
                            .success(function (data, status, headers, config) {
                            if (data.result) {
                                $scope.fd = data.data;
                                $scope.edit_type = 2;
                                $scope.$parent.Init_Query();
                                alert('訂單新增完成');
                            }
                            else {
                                alert(data.message);
                            }
                        })
                            .error(function (data, status, headers, config) {
                            showAjaxError(data);
                        });
                    }
                    else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            }
            ;
            if ($scope.edit_type == 2) {
                $http.put(gb_approot + 'Orders/UpdateWishLight', $scope.fd)
                    .success(function (data, status, headers, config) {
                    if (data.result) {
                        $scope.edit_type = 2;
                        $scope.$parent.Init_Query();
                        alert('訂單修改完成');
                    }
                    else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            }
            ;
        }
        ;
        function SendDouLightOrder() {
            console.log($scope.fd.Item);
            var item = $scope.fd.Item[0];
            if ($scope.edit_type == 1) {
                var m = [];
                for (var i in $scope.mb.getMember_Detail) {
                    var n = $scope.mb.getMember_Detail[i];
                    if (n.fortune_value > 0) {
                        m.push(n.fortune_value);
                    }
                }
                $http.post(gb_approot + 'Orders/AddOrdersDouLight', {
                    'master': $scope.fd,
                    'product_sn': item.product_sn,
                    'member_detail_ids': m
                })
                    .success(function (data, status, headers, config) {
                    if (data.result) {
                        $http.get(gb_approot + 'Cart/OrdersToSession', { params: { orders_sn: data.data } })
                            .success(function (data, status, headers, config) {
                            if (data.result) {
                                $scope.fd = data.data;
                                $scope.edit_type = 2;
                                $scope.$parent.Init_Query();
                                alert('訂單新增完成');
                            }
                            else {
                                alert(data.message);
                            }
                        })
                            .error(function (data, status, headers, config) {
                            showAjaxError(data);
                        });
                    }
                    else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            }
            ;
            if ($scope.edit_type == 2) {
                var m = [];
                for (var i in $scope.mb.getMember_Detail) {
                    var n = $scope.mb.getMember_Detail[i];
                    if (n.fortune_value > 0) {
                        m.push(n.fortune_value);
                    }
                }
                $http.put(gb_approot + 'Orders/UpdateOrdersDouLight', {
                    'master': $scope.fd,
                    'member_detail_ids': m
                })
                    .success(function (data, status, headers, config) {
                    if (data.result) {
                        $scope.edit_type = 2;
                        $scope.$parent.Init_Query();
                        alert('訂單修改完成');
                    }
                    else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            }
            ;
        }
        ;
        $scope.$on(callOnName.updateCartMaster_is_light_serial, function (event, args) {
            $scope.fd.is_light_serial = args;
        });
        $scope.CalcLunar = function () {
            workService.getCalcLunar($scope.cart.SY, $scope.cart.SM, $scope.cart.SD)
                .success(function (data, status, headers, config) {
                $scope.cart.LY = data.LY;
                $scope.cart.LM = data.M;
                $scope.cart.LD = data.D;
                $scope.cart.isOnLeapMonth = data.IsLeap;
            }).error(function (data) {
                showAjaxError(data);
            });
        };
        $scope.DownLoadExcel_Thanks = function () {
            var url = gb_approot + 'ExcelReport/downloadExcel_Thanks?orders_sn=' + $scope.fd.orders_sn + '&t=' + uniqid();
            $scope.downloadExcel = $sce.getTrustedResourceUrl(url);
        };
        function GetOrder(orders_sn) {
            workService.getOrderData(orders_sn)
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.fd.member_id = data.data.member_id;
                    $scope.fd.member_name = data.data.member_name;
                    $scope.fd.member_detail_id = data.data.member_detail_id;
                    $scope.fd.tel = data.data.tel;
                    $scope.fd.zip = data.data.zip;
                    $scope.fd.address = data.data.address;
                    $scope.fd.gender = data.data.gender;
                    $scope.fd.mobile = data.data.mobile;
                    for (var i in data.data.getOrders_Detail) {
                        var order_detail = data.data.getOrders_Detail[i];
                    }
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        ;
        function GetMemberAll(member_id) {
            return workService.getMemberAll(member_id)
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.mb = data.data;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        ;
        function GetMemberByDetail(member_id) {
            var r = workService.getMemberByDetail(member_id)
                .success(function (data, status, headers, config) {
                if (data.result) {
                    if (member_id != data.data.member_id) {
                        alert('系統回傳member_id不一致');
                        return;
                    }
                    ClearCart();
                    $scope.fd = {
                        member_id: member_id,
                        is_light_serial: true,
                        member_name: data.data.member_name,
                        member_detail_id: data.data.member_detail_id,
                        address: data.data.city + data.data.country + data.data.address,
                        zip: data.data.zip,
                        gender: data.data.gender == '1' ? '男' : '女',
                        tel: data.data.tel,
                        mobile: data.data.mobile,
                        Item: []
                    };
                    SetMemberToCart();
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
            return r;
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
                showAjaxError(data);
            });
        }
        ;
        function GetFortune() {
            $http.get(gb_approot + apiGetAction + '/GetFortune')
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.pds_fortune = data.data;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
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
                showAjaxError(data);
            });
            ;
        }
        function GetNowCartList() {
            $http.get(gb_approot + 'Cart/ListCartItems', { params: {} })
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.fd.Item = data.data.Item;
                    $scope.fd.total = data.data.total;
                    $scope.fd.race = data.data.race;
                    $scope.fd.gold = data.data.gold;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        ;
        function SetMemberToCart() {
            $http.post(gb_approot + 'Cart/SetCartMaster', $scope.fd)
                .success(function (data, status, headers, config) {
                if (data.result) {
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        function ClearCart() {
            $http.get(gb_approot + 'Cart/ClearCart', { params: { t: uniqid() } })
                .success(function (data, status, headers, config) {
                if (!data.result) {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        function GetLight(product_sn) {
            workService.getLightByMD(product_sn)
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.lights = data.data;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        ;
        function MasterDataView(orders_sn) {
            var q = $q.defer();
            $http.get(gb_approot + 'Orders/CheckOrderType', { params: { orders_sn: orders_sn } })
                .success(function (data, status, headers, config) {
                if (data.message != '') {
                    document.location.href = data.message;
                }
                else {
                    $http.get(gb_approot + 'Cart/ClearCart', { params: {} })
                        .success(function (data, status, headers, config) {
                        if (data.result) {
                            $http.get(gb_approot + 'Orders/GetToSession', { params: { orders_sn: orders_sn } })
                                .success(function (data, status, headers, config) {
                                if (data.result) {
                                    $scope.fd = data.data;
                                    $scope.fd.transation_date = new Date(data.data.transation_date.toString());
                                    $scope.fd.is_light_serial = true;
                                    $scope.edit_type = 2;
                                    var LocalDate = new Date($scope.fd.transation_date.getFullYear(), $scope.fd.transation_date.getMonth(), $scope.fd.transation_date.getDate(), $scope.fd.transation_date.getHours() - 8);
                                    var nowToday = new Date();
                                    if (LocalDate.getFullYear() == nowToday.getFullYear() &&
                                        LocalDate.getMonth() == nowToday.getMonth() &&
                                        LocalDate.getDate() == nowToday.getDate()) {
                                        $scope.isTodayOrder = true;
                                    }
                                    else {
                                        $scope.isTodayOrder = false;
                                    }
                                    console.log($scope.isTodayOrder, 'Local Date', LocalDate);
                                    GetMemberAll(data.data.member_id).success(function (data) {
                                        q.resolve();
                                    });
                                }
                                else {
                                    alert(data.message);
                                }
                            }).error(function (data) {
                                q.reject();
                                showAjaxError(data);
                            });
                        }
                        else {
                            alert(data.message);
                        }
                    })
                        .error(function (data, status, headers, config) {
                        q.reject();
                        showAjaxError(data);
                    });
                }
            });
            return q.promise;
        }
        ;
        if ($state.params.orders_sn != undefined) {
            $scope.edit_type = 2;
            $scope.qMasterView = MasterDataView($state.params.orders_sn);
        }
        else {
            $scope.edit_type = 1;
            if ($state.params.member_id != undefined) {
                $scope.edit_type = 1;
                $scope.qMember = GetMemberByDetail($state.params.member_id);
                GetMemberAll($state.params.member_id);
                $scope.isTodayOrder = true;
            }
        }
    }]);
angular.module('angularApp')
    .controller('ctrl_general', ['$scope', '$http', 'workService', '$sce', 'ngDialog', '$state', function ($scope, $http, workService, $sce, ngDialog, $state) {
        console.log('ctrl_general');
        var p0 = 0;
        var allowSetPrice = ['香油'];
        var allowSetRace = ['白米'];
        var allowSetGold = ['金牌'];
        var allowSetGodSon = ['契子'];
        $scope.timeperiod_list = commData.batch_timeperiod;
        $scope.allowYear = allowyear;
        $scope.allowSetAssembly = ['超渡法會'];
        $scope.born_sign = commData.born_sign;
        $scope.born_time = commData.born_time;
        $scope.isShowEdit = false;
        $scope.isShowEditProduct = false;
        $scope.cart_price_disable = true;
        $scope.cart_race_disable = true;
        $scope.cart_gold_disable = true;
        $scope.SubmitCart = function () {
            $scope.cart.isOnOrder = false;
            $http.post(gb_approot + 'Cart/AddCart', $scope.cart)
                .success(function (data, status, headers, config) {
                if (data.result) {
                    GetNowCartList();
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        $scope.RemoveCart = function (member_detail_id, product_sn) {
            if (!confirm('確認是否刪除?'))
                return;
            $http.get(gb_approot + 'Cart/RemoveCart', {
                params: {
                    member_detail_id: member_detail_id,
                    product_sn: product_sn
                }
            })
                .success(function (data, status, headers, config) {
                if (data.result) {
                    GetNowCartList();
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        $scope.CloseEdit = function () {
            $scope.fd = null;
            $scope.isShowEdit = false;
        };
        $scope.ShowEditAddProduct = function () {
            $scope.cart = { member_detail_id: -1, y: allowyear };
            GetAssemblyBatch(null);
            $scope.isShowEditProduct = true;
            $scope.isViewWorking = false;
        };
        $scope.ShowEditViewProduct = function (member_detail_id, product_sn) {
            $http.get(gb_approot + 'Cart/ViewCart', {
                params: {
                    member_detail_id: member_detail_id,
                    product_sn: product_sn
                }
            })
                .success(function (data, status, headers, config) {
                if (data.result) {
                    GetAssemblyBatch(data.data.y);
                    $scope.cart = data.data;
                    $scope.isShowEditProduct = true;
                    $scope.isViewWorking = true;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        $scope.CloseEditProduct = function () {
            $scope.isShowEditProduct = false;
            $scope.is_godson = false;
        };
        $scope.ShowOrderDetail = function () {
            if ($scope.cart.member_detail_id > 0) {
                $http.get(gb_approot + apiGetAction + '/GetOrderDetail', {
                    params: {
                        member_detail_id: $scope.cart.member_detail_id,
                    }
                })
                    .success(function (data, status, headers, config) {
                    if (data.result) {
                        $scope.order_detail = data.data;
                        if (data.data.length <= 0) {
                            alert("去年無購買紀錄!");
                        }
                    }
                    else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            }
            else {
                alert("請選擇好會員後再查詢!!");
            }
        };
        $scope.CalcLunar = function () {
            workService.getCalcLunar($scope.cart.SY, $scope.cart.SM, $scope.cart.SD)
                .success(function (data, status, headers, config) {
                $scope.cart.LY = data.LY;
                $scope.cart.LM = data.M;
                $scope.cart.LD = data.D;
                $scope.cart.isOnLeapMonth = data.IsLeap;
            }).error(function (data) {
                showAjaxError(data);
            });
        };
        function GetOrder(orders_sn) {
            workService.getOrderData(orders_sn)
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.fd.member_id = data.data.member_id;
                    $scope.fd.member_name = data.data.member_name;
                    $scope.fd.member_detail_id = data.data.member_detail_id;
                    $scope.fd.tel = data.data.tel;
                    $scope.fd.zip = data.data.zip;
                    $scope.fd.address = data.data.address;
                    $scope.fd.gender = data.data.gender;
                    $scope.fd.mobile = data.data.mobile;
                    for (var i in data.data.getOrders_Detail) {
                        var order_detail = data.data.getOrders_Detail[i];
                    }
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
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
                showAjaxError(data);
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
                    ClearCart();
                    $scope.fd = {
                        member_id: member_id,
                        is_light_serial: true,
                        member_name: data.data.member_name,
                        member_detail_id: data.data.member_detail_id,
                        address: data.data.city + data.data.country + data.data.address,
                        zip: data.data.zip,
                        gender: data.data.gender == '1' ? '男' : '女',
                        tel: data.data.tel,
                        mobile: data.data.mobile
                    };
                    SetMemberToCart();
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
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
                showAjaxError(data);
            });
        }
        ;
        function GetFortune() {
            $http.get(gb_approot + apiGetAction + '/GetFortune')
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.pds_fortune = data.data;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
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
        function GetAssemblyBatch(year) {
            workService.getAssemblyBatch(year)
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.Abath_List = data.data;
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
                showAjaxError(data);
            });
            ;
        }
        function GetNowCartList() {
            $http.get(gb_approot + 'Cart/ListCartItems', { params: {} })
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.fd.Item = data.data.Item;
                    $scope.fd.total = data.data.total;
                    $scope.fd.gold = data.data.gold;
                    $scope.fd.race = data.data.race;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        ;
        function SetMemberToCart() {
            $http.post(gb_approot + 'Cart/SetCartMaster', $scope.fd)
                .success(function (data, status, headers, config) {
                if (data.result) {
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        function ClearCart() {
            $http.get(gb_approot + 'Cart/ClearCart', { params: { t: uniqid() } })
                .success(function (data, status, headers, config) {
                if (!data.result) {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        if ($scope.edit_type == 2) {
        }
        if ($scope.edit_type == 1) {
            if ($state.params.member_id != undefined) {
                GetMemberByDetail($state.params.member_id);
                GetMemberAll($state.params.member_id);
            }
        }
        $scope.$watch('cart.product_sn', function (newValue, oldValue) {
            if (newValue != undefined) {
                for (var i in $scope.pds) {
                    var n = $scope.pds[i];
                    if (n.product_sn == newValue) {
                        $scope.cart.price = n.price;
                        $scope.cart.product_name = n.product_name;
                        $scope.cart.product_sn = n.product_sn;
                        $scope.cart.race = 0;
                        $scope.cart.gold = 0;
                        $scope.product_category = n.category;
                        if (allowSetPrice.indexOf(n.category) >= 0) {
                            $scope.cart_price_disable = false;
                            $scope.cart_race_disable = true;
                            $scope.cart_gold_disable = true;
                        }
                        else if (allowSetRace.indexOf(n.category) >= 0) {
                            $scope.cart_price_disable = true;
                            $scope.cart_race_disable = false;
                            $scope.cart_gold_disable = true;
                        }
                        else if (allowSetGold.indexOf(n.category) >= 0) {
                            $scope.cart_price_disable = true;
                            $scope.cart_race_disable = true;
                            $scope.cart_gold_disable = false;
                        }
                        else if (allowSetGodSon.indexOf(n.category) >= 0) {
                            $scope.cart_price_disable = false;
                            $scope.cart_race_disable = true;
                            $scope.cart_gold_disable = true;
                        }
                        else {
                            $scope.cart_price_disable = true;
                            $scope.cart_race_disable = true;
                            $scope.cart_gold_disable = true;
                        }
                    }
                }
            }
        });
        $scope.$watch('cart.member_detail_id', function (newValue, oldValue) {
            if (newValue != undefined) {
                for (var i in $scope.mb.getMember_Detail) {
                    var n = $scope.mb.getMember_Detail[i];
                    if (n.member_detail_id == newValue) {
                        $scope.cart.born_time = n.born_time;
                        $scope.cart.born_sign = n.born_sign;
                        $scope.cart.member_name = n.member_name;
                        $scope.cart.l_birthday = n.lbirthday;
                        $scope.cart.address = n.city + n.country + n.address;
                        $scope.cart.gender = n.gender;
                        $scope.cart.tel = n.tel;
                        $scope.cart.mobile = n.mobile;
                        $scope.cart.join_date = n.join_date;
                        $scope.order_detail = null;
                        $scope.is_godson = n.is_godson;
                        var lbirthday = $scope.cart.l_birthday.split("/");
                        if (lbirthday.length == 3) {
                            $scope.cart.LY = parseInt(lbirthday[0]) - 1911;
                            $scope.cart.LM = parseInt(lbirthday[1]);
                            $scope.cart.LD = parseInt(lbirthday[2]);
                        }
                    }
                }
            }
        });
        GetProductAll();
        GetManjushri();
        GetFortune();
    }]);
angular.module('angularApp')
    .controller('ctrl_fortune', ['$scope', '$http', 'workService', '$sce', 'ngDialog', '$state', function ($scope, $http, workService, $sce, ngDialog, $state) {
        console.log('ctrl_fortune');
        var p0 = 0;
        $scope.born_sign = commData.born_sign;
        $scope.born_time = commData.born_time;
        $scope.isShowEdit = false;
        $scope.isShowEditProduct = false;
        $scope.cart_price_disable = true;
        $scope.cart_race_disable = true;
        $scope.cart_gold_disable = true;
        $scope.ShowEditAddProduct = function () {
            $scope.cart = { member_detail_id: -1 };
            $scope.isShowEditProduct = true;
            $scope.isViewWorking = false;
        };
        $scope.ShowEditViewProduct = function (member_detail_id, product_sn) {
            $http.get(gb_approot + 'Cart/ViewCart', {
                params: {
                    member_detail_id: member_detail_id,
                    product_sn: product_sn
                }
            })
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.cart = data.data;
                    $scope.isShowEditProduct = true;
                    $scope.isViewWorking = true;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        $scope.CloseEditProduct = function () {
            $scope.isShowEditProduct = false;
            $scope.is_godson = false;
        };
        $scope.CalcLunar = function () {
            workService.getCalcLunar($scope.cart.SY, $scope.cart.SM, $scope.cart.SD)
                .success(function (data, status, headers, config) {
                $scope.cart.LY = data.LY;
                $scope.cart.LM = data.M;
                $scope.cart.LD = data.D;
                $scope.cart.isOnLeapMonth = data.IsLeap;
            }).error(function (data) {
                showAjaxError(data);
            });
        };
        function GetOrder(orders_sn) {
            workService.getOrderData(orders_sn)
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.fd.member_id = data.data.member_id;
                    $scope.fd.member_name = data.data.member_name;
                    $scope.fd.member_detail_id = data.data.member_detail_id;
                    $scope.fd.tel = data.data.tel;
                    $scope.fd.zip = data.data.zip;
                    $scope.fd.address = data.data.address;
                    $scope.fd.gender = data.data.gender;
                    $scope.fd.mobile = data.data.mobile;
                    for (var i in data.data.getOrders_Detail) {
                        var order_detail = data.data.getOrders_Detail[i];
                    }
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        ;
        function GetProductAll() {
            workService.getProductFortune()
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.pds = data.data;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        ;
        function GetFortune() {
            $http.get(gb_approot + apiGetAction + '/GetFortune')
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.pds_fortune = data.data;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
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
                showAjaxError(data);
            });
            ;
        }
        function GetNowCartList() {
            $http.get(gb_approot + 'Cart/ListCartItems', { params: {} })
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.fd.Item = data.data;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        ;
        function SetMemberToCart() {
            $http.post(gb_approot + 'Cart/SetCartMaster', $scope.fd)
                .success(function (data, status, headers, config) {
                if (data.result) {
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        function ClearCart() {
            $http.get(gb_approot + 'Cart/ClearCart', { params: { t: uniqid() } })
                .success(function (data, status, headers, config) {
                if (!data.result) {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        if ($scope.edit_type == 2) {
            workService.getFortune($state.params.orders_sn)
                .success(function (fdata, status, headers, config) {
                $scope.qMasterView.then(function () {
                    for (var i in $scope.mb.getMember_Detail) {
                        var item = $scope.mb.getMember_Detail[i];
                        if (fdata.data.indexOf(item.member_detail_id) >= 0) {
                            item.fortune_value = item.member_detail_id;
                        }
                    }
                });
            });
        }
        if ($scope.edit_type == 1) {
            if ($state.params.member_id != undefined) {
                $scope.qMember.success(function (data, status, headers, config) {
                    var detail;
                    detail = {
                        member_detail_id: $state.params.member_id,
                        member_name: $scope.fd.member_name,
                        product_sn: ''
                    };
                    $scope.fd.Item.push(detail);
                });
            }
        }
        $scope.$watch('cart.member_detail_id', function (newValue, oldValue) {
            if (newValue != undefined) {
                for (var i in $scope.mb.getMember_Detail) {
                    var n = $scope.mb.getMember_Detail[i];
                    if (n.member_detail_id == newValue) {
                        $scope.cart.born_time = n.born_time;
                        $scope.cart.born_sign = n.born_sign;
                        $scope.cart.member_name = n.member_name;
                        $scope.cart.l_birthday = n.lbirthday;
                        $scope.cart.address = n.city + n.country + n.address;
                        $scope.cart.gender = n.gender;
                        $scope.cart.tel = n.tel;
                        $scope.cart.mobile = n.mobile;
                        $scope.is_godson = n.is_godson;
                        var lbirthday = $scope.cart.l_birthday.split("/");
                        if (lbirthday.length == 3) {
                            $scope.cart.LY = parseInt(lbirthday[0]) - 1911;
                            $scope.cart.LM = parseInt(lbirthday[1]);
                            $scope.cart.LD = parseInt(lbirthday[2]);
                        }
                    }
                }
            }
        });
        $scope.ProductChange = function (product_sn) {
            for (var i in $scope.pds) {
                var n = $scope.pds[i];
                if (n.product_sn == product_sn) {
                    $scope.fd.Item[0].price = n.price;
                    $scope.fd.Item[0].product_name = n.product_name;
                    $scope.fd.Item[0].race = 0;
                    $scope.fd.Item[0].gold = 0;
                }
            }
            GetLight(product_sn);
        };
        $scope.DeleteOrderFourtune = function () {
            if (confirm('確定刪除訂單?')) {
                $http.get(gb_approot + 'Orders/DeleteOrdersFortune', { params: { orders_sn: $scope.fd.orders_sn } })
                    .success(function (data, status, headers, config) {
                    if (data.result) {
                        $scope.Init_Query();
                        $scope.$parent.GoGrid();
                    }
                    else {
                        alert(data.message);
                    }
                });
            }
            else {
                return;
            }
        };
        $scope.checkFortuneNum = function () {
            var items = $scope.mb.getMember_Detail;
            for (var i in items) {
                var item = items[i];
            }
        };
        function GetLight(product_sn) {
            workService.getLightByMD(product_sn)
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.lights = data.data;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        ;
        $scope.openSortData = function () {
            $scope.isShow = true;
            console.log($scope.mb);
        };
        $scope.closeSortData = function () {
            $scope.isShow = false;
        };
        GetProductAll();
    }]);
angular.module('angularApp')
    .controller('ctrl_sdlight', ['$scope', '$http', 'workService', '$sce', 'ngDialog', '$state', function ($scope, $http, workService, $sce, ngDialog, $state) {
        console.log('ctrl_general');
        var p0 = 0;
        var allowSetPrice = ['香油'];
        var allowSetRace = ['白米'];
        var allowSetGold = ['金牌'];
        $scope.born_sign = commData.born_sign;
        $scope.born_time = commData.born_time;
        $scope.isShowEdit = false;
        $scope.isShowEditProduct = false;
        $scope.cart_price_disable = true;
        $scope.cart_race_disable = true;
        $scope.cart_gold_disable = true;
        $scope.SubmitCart = function () {
            $scope.cart.isOnOrder = false;
            $http.post(gb_approot + 'Cart/AddCart', $scope.cart)
                .success(function (data, status, headers, config) {
                if (data.result) {
                    GetNowCartList();
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        $scope.RemoveCart = function (member_detail_id, product_sn) {
            if (!confirm('確認是否刪除?'))
                return;
            $http.get(gb_approot + 'Cart/RemoveCart', {
                params: {
                    member_detail_id: member_detail_id,
                    product_sn: product_sn
                }
            })
                .success(function (data, status, headers, config) {
                if (data.result) {
                    GetNowCartList();
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        $scope.CloseEdit = function () {
            $scope.fd = null;
            $scope.isShowEdit = false;
        };
        $scope.ShowEditAddProduct = function () {
            $scope.cart = { member_detail_id: -1 };
            $scope.isShowEditProduct = true;
            $scope.isViewWorking = false;
        };
        $scope.ShowEditViewProduct = function (member_detail_id, product_sn) {
            $http.get(gb_approot + 'Cart/ViewCart', {
                params: {
                    member_detail_id: member_detail_id,
                    product_sn: product_sn
                }
            })
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.cart = data.data;
                    $scope.isShowEditProduct = true;
                    $scope.isViewWorking = true;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        $scope.CloseEditProduct = function () {
            $scope.isShowEditProduct = false;
            $scope.is_godson = false;
        };
        $scope.ShowOrderDetail = function () {
            if ($scope.cart.member_detail_id > 0) {
                $http.get(gb_approot + apiGetAction + '/GetOrderDetail', {
                    params: {
                        member_detail_id: $scope.cart.member_detail_id,
                    }
                })
                    .success(function (data, status, headers, config) {
                    if (data.result) {
                        $scope.order_detail = data.data;
                        if (data.data.length <= 0) {
                            alert("去年無購買紀錄!");
                        }
                    }
                    else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            }
            else {
                alert("請選擇好會員後再查詢!!");
            }
        };
        $scope.CalcLunar = function () {
            workService.getCalcLunar($scope.cart.SY, $scope.cart.SM, $scope.cart.SD)
                .success(function (data, status, headers, config) {
                $scope.cart.LY = data.LY;
                $scope.cart.LM = data.M;
                $scope.cart.LD = data.D;
                $scope.cart.isOnLeapMonth = data.IsLeap;
            }).error(function (data) {
                showAjaxError(data);
            });
        };
        function GetOrder(orders_sn) {
            workService.getOrderData(orders_sn)
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.fd.member_id = data.data.member_id;
                    $scope.fd.member_name = data.data.member_name;
                    $scope.fd.member_detail_id = data.data.member_detail_id;
                    $scope.fd.tel = data.data.tel;
                    $scope.fd.zip = data.data.zip;
                    $scope.fd.address = data.data.address;
                    $scope.fd.gender = data.data.gender;
                    $scope.fd.mobile = data.data.mobile;
                    for (var i in data.data.getOrders_Detail) {
                        var order_detail = data.data.getOrders_Detail[i];
                    }
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
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
                showAjaxError(data);
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
                    ClearCart();
                    $scope.fd = {
                        member_id: member_id,
                        is_light_serial: true,
                        member_name: data.data.member_name,
                        member_detail_id: data.data.member_detail_id,
                        address: data.data.city + data.data.country + data.data.address,
                        zip: data.data.zip,
                        gender: data.data.gender == '1' ? '男' : '女',
                        tel: data.data.tel,
                        mobile: data.data.mobile
                    };
                    SetMemberToCart();
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        ;
        function GetProductAll() {
            workService.getProductSDLight()
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.pds = data.data;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
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
                showAjaxError(data);
            });
            ;
        }
        function GetNowCartList() {
            $http.get(gb_approot + 'Cart/ListCartItems', { params: {} })
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.fd.Item = data.data.Item;
                    $scope.fd.total = data.data.total;
                    $scope.fd.gold = data.data.gold;
                    $scope.fd.race = data.data.race;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        ;
        function SetMemberToCart() {
            $http.post(gb_approot + 'Cart/SetCartMaster', $scope.fd)
                .success(function (data, status, headers, config) {
                if (data.result) {
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        function ClearCart() {
            $http.get(gb_approot + 'Cart/ClearCart', { params: { t: uniqid() } })
                .success(function (data, status, headers, config) {
                if (!data.result) {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        if ($scope.edit_type == 2) {
        }
        if ($scope.edit_type == 1) {
            if ($state.params.member_id != undefined) {
                GetMemberByDetail($state.params.member_id);
                GetMemberAll($state.params.member_id);
            }
        }
        $scope.$watch('cart.product_sn', function (newValue, oldValue) {
            if (newValue != undefined) {
                for (var i in $scope.pds) {
                    var n = $scope.pds[i];
                    if (n.product_sn == newValue) {
                        $scope.cart.price = n.price;
                        $scope.cart.product_name = n.product_name;
                        $scope.cart.product_sn = n.product_sn;
                        $scope.cart.race = 0;
                        $scope.cart.gold = 0;
                        if (allowSetPrice.indexOf(n.category) >= 0) {
                            $scope.cart_price_disable = false;
                            $scope.cart_race_disable = true;
                            $scope.cart_gold_disable = true;
                        }
                        else if (allowSetRace.indexOf(n.category) >= 0) {
                            $scope.cart_price_disable = true;
                            $scope.cart_race_disable = false;
                            $scope.cart_gold_disable = true;
                        }
                        else if (allowSetGold.indexOf(n.category) >= 0) {
                            $scope.cart_price_disable = true;
                            $scope.cart_race_disable = true;
                            $scope.cart_gold_disable = false;
                        }
                        else {
                            $scope.cart_price_disable = true;
                            $scope.cart_race_disable = true;
                            $scope.cart_gold_disable = true;
                        }
                    }
                }
            }
        });
        $scope.$watch('cart.member_detail_id', function (newValue, oldValue) {
            if (newValue != undefined) {
                for (var i in $scope.mb.getMember_Detail) {
                    var n = $scope.mb.getMember_Detail[i];
                    if (n.member_detail_id == newValue) {
                        $scope.cart.born_time = n.born_time;
                        $scope.cart.born_sign = n.born_sign;
                        $scope.cart.member_name = n.member_name;
                        $scope.cart.l_birthday = n.lbirthday;
                        $scope.cart.address = n.city + n.country + n.address;
                        $scope.cart.gender = n.gender;
                        $scope.cart.tel = n.tel;
                        $scope.cart.mobile = n.mobile;
                        $scope.order_detail = null;
                        $scope.is_godson = n.is_godson;
                        var lbirthday = $scope.cart.l_birthday.split("/");
                        if (lbirthday.length == 3) {
                            $scope.cart.LY = parseInt(lbirthday[0]) - 1911;
                            $scope.cart.LM = parseInt(lbirthday[1]);
                            $scope.cart.LD = parseInt(lbirthday[2]);
                        }
                    }
                }
            }
        });
        GetProductAll();
    }]);
angular.module('angularApp')
    .controller('ctrl_mdlight', ['$scope', '$http', 'workService', '$sce', 'ngDialog', '$state', function ($scope, $http, workService, $sce, ngDialog, $state) {
        var p0 = 0;
        var allowSetPrice = ['香油'];
        var allowSetRace = ['白米'];
        var allowSetGold = ['金牌'];
        var allowSetGodSon = ['契子'];
        $scope.born_sign = commData.born_sign;
        $scope.born_time = commData.born_time;
        $scope.isShowEdit = false;
        $scope.isShowEditProduct = false;
        $scope.cart_price_disable = true;
        $scope.cart_race_disable = true;
        $scope.cart_gold_disable = true;
        $scope.SubmitCart = function () {
            $scope.cart.isOnOrder = false;
            $http.post(gb_approot + 'Cart/AddLightCart', $scope.cart)
                .success(function (data, status, headers, config) {
                if (data.result) {
                    GetNowCartList();
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        $scope.RemoveCart = function (member_detail_id, product_sn) {
            if (!confirm('確認是否刪除?'))
                return;
            $http.get(gb_approot + 'Cart/RemoveCart', {
                params: {
                    member_detail_id: member_detail_id,
                    product_sn: product_sn
                }
            })
                .success(function (data, status, headers, config) {
                if (data.result) {
                    GetNowCartList();
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        $scope.CloseEdit = function () {
            $scope.fd = null;
            $scope.isShowEdit = false;
        };
        $scope.ShowEditAddProduct = function () {
            $scope.cart = { member_detail_id: -1 };
            $scope.isShowEditProduct = true;
            $scope.isViewWorking = false;
        };
        $scope.ShowEditViewProduct = function (member_detail_id, product_sn) {
            $http.get(gb_approot + 'Cart/ViewCart', {
                params: {
                    member_detail_id: member_detail_id,
                    product_sn: product_sn
                }
            })
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.cart = data.data;
                    $scope.isShowEditProduct = true;
                    $scope.isViewWorking = true;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        $scope.CloseEditProduct = function () {
            $scope.isShowEditProduct = false;
            $scope.is_godson = false;
        };
        $scope.ShowOrderDetail = function () {
            if ($scope.cart.member_detail_id > 0) {
                $http.get(gb_approot + apiGetAction + '/GetOrderDetail', {
                    params: {
                        member_detail_id: $scope.cart.member_detail_id,
                    }
                })
                    .success(function (data, status, headers, config) {
                    if (data.result) {
                        $scope.order_detail = data.data;
                        if (data.data.length <= 0) {
                            alert("去年無購買紀錄!");
                        }
                    }
                    else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            }
            else {
                alert("請選擇好會員後再查詢!!");
            }
        };
        $scope.CalcLunar = function () {
            workService.getCalcLunar($scope.cart.SY, $scope.cart.SM, $scope.cart.SD)
                .success(function (data, status, headers, config) {
                $scope.cart.LY = data.LY;
                $scope.cart.LM = data.M;
                $scope.cart.LD = data.D;
                $scope.cart.isOnLeapMonth = data.IsLeap;
            }).error(function (data) {
                showAjaxError(data);
            });
        };
        function GetOrder(orders_sn) {
            workService.getOrderData(orders_sn)
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.fd.member_id = data.data.member_id;
                    $scope.fd.member_name = data.data.member_name;
                    $scope.fd.member_detail_id = data.data.member_detail_id;
                    $scope.fd.tel = data.data.tel;
                    $scope.fd.zip = data.data.zip;
                    $scope.fd.address = data.data.address;
                    $scope.fd.gender = data.data.gender;
                    $scope.fd.mobile = data.data.mobile;
                    for (var i in data.data.getOrders_Detail) {
                        var order_detail = data.data.getOrders_Detail[i];
                    }
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
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
                showAjaxError(data);
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
                    ClearCart();
                    $scope.fd = {
                        member_id: member_id,
                        is_light_serial: true,
                        member_name: data.data.member_name,
                        member_detail_id: data.data.member_detail_id,
                        address: data.data.city + data.data.country + data.data.address,
                        zip: data.data.zip,
                        gender: data.data.gender == '1' ? '男' : '女',
                        tel: data.data.tel,
                        mobile: data.data.mobile
                    };
                    SetMemberToCart();
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        ;
        function GetProductAll() {
            workService.getProductMDLight()
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.pds = data.data;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        ;
        function GetLight(product_sn) {
            workService.getLightByMD(product_sn)
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.lights = data.data;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
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
                showAjaxError(data);
            });
            ;
        }
        function GetNowCartList() {
            $http.get(gb_approot + 'Cart/ListCartItems', { params: {} })
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.fd.Item = data.data.Item;
                    $scope.fd.total = data.data.total;
                    $scope.fd.gold = data.data.gold;
                    $scope.fd.race = data.data.race;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        ;
        function SetMemberToCart() {
            $http.post(gb_approot + 'Cart/SetCartMaster', $scope.fd)
                .success(function (data, status, headers, config) {
                if (data.result) {
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        function ClearCart() {
            $http.get(gb_approot + 'Cart/ClearCart', { params: { t: uniqid() } })
                .success(function (data, status, headers, config) {
                if (!data.result) {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        if ($scope.edit_type == 2) {
        }
        if ($scope.edit_type == 1) {
            if ($state.params.member_id != undefined) {
                GetMemberByDetail($state.params.member_id);
                GetMemberAll($state.params.member_id);
            }
        }
        $scope.$watch('cart.product_sn', function (newValue, oldValue) {
            if (newValue != undefined) {
                for (var i in $scope.pds) {
                    var n = $scope.pds[i];
                    if (n.product_sn == newValue) {
                        $scope.cart.price = n.price;
                        $scope.cart.product_name = n.product_name;
                        $scope.cart.product_sn = n.product_sn;
                        $scope.cart.race = 0;
                        $scope.cart.gold = 0;
                        if (allowSetPrice.indexOf(n.category) >= 0) {
                            $scope.cart_price_disable = false;
                            $scope.cart_race_disable = true;
                            $scope.cart_gold_disable = true;
                        }
                        else if (allowSetRace.indexOf(n.category) >= 0) {
                            $scope.cart_price_disable = true;
                            $scope.cart_race_disable = false;
                            $scope.cart_gold_disable = true;
                        }
                        else if (allowSetGold.indexOf(n.category) >= 0) {
                            $scope.cart_price_disable = true;
                            $scope.cart_race_disable = true;
                            $scope.cart_gold_disable = false;
                        }
                        else {
                            $scope.cart_price_disable = true;
                            $scope.cart_race_disable = true;
                            $scope.cart_gold_disable = true;
                        }
                    }
                }
            }
        });
        $scope.$watch('cart.member_detail_id', function (newValue, oldValue) {
            if (newValue != undefined) {
                for (var i in $scope.mb.getMember_Detail) {
                    var n = $scope.mb.getMember_Detail[i];
                    if (n.member_detail_id == newValue) {
                        $scope.cart.born_time = n.born_time;
                        $scope.cart.born_sign = n.born_sign;
                        $scope.cart.member_name = n.member_name;
                        $scope.cart.l_birthday = n.lbirthday;
                        $scope.cart.address = n.city + n.country + n.address;
                        $scope.cart.gender = n.gender;
                        $scope.cart.tel = n.tel;
                        $scope.cart.mobile = n.mobile;
                        $scope.order_detail = null;
                        $scope.is_godson = n.is_godson;
                        var lbirthday = $scope.cart.l_birthday.split("/");
                        if (lbirthday.length == 3) {
                            $scope.cart.LY = parseInt(lbirthday[0]) - 1911;
                            $scope.cart.LM = parseInt(lbirthday[1]);
                            $scope.cart.LD = parseInt(lbirthday[2]);
                        }
                    }
                }
            }
        });
        $scope.$watch('cart.product_sn', function (newValue, oldValue) {
            if (newValue != undefined) {
                GetLight(newValue);
            }
        });
        GetProductAll();
    }]);
angular.module('angularApp')
    .controller('ctrl_wishlight', ['$scope', '$http', 'workService', '$sce', 'ngDialog', '$state', function ($scope, $http, workService, $sce, ngDialog, $state) {
        console.log('ctrl_wishlight');
        var p0 = 0;
        var allowSetPrice = ['香油'];
        var allowSetRace = ['白米'];
        var allowSetGold = ['金牌'];
        $scope.wishmemo_list = commData.wishmemo_list;
        $scope.born_sign = commData.born_sign;
        $scope.born_time = commData.born_time;
        $scope.isShowEdit = false;
        $scope.isShowEditProduct = false;
        $scope.cart_price_disable = true;
        $scope.cart_race_disable = true;
        $scope.cart_gold_disable = true;
        $scope.chgWishMemo = function () {
            $scope.cart.wish_memo = $scope.wishmemo_no;
        };
        $scope.SubmitCart = function () {
            $scope.cart.isOnOrder = false;
            $http.post(gb_approot + 'Cart/AddWishCart', $scope.cart)
                .success(function (data, status, headers, config) {
                if (data.result) {
                    GetNowCartList();
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        $scope.RemoveCart = function (member_detail_id, product_sn) {
            if (!confirm('確認是否刪除?'))
                return;
            $http.get(gb_approot + 'Cart/RemoveCart', {
                params: {
                    member_detail_id: member_detail_id,
                    product_sn: product_sn
                }
            })
                .success(function (data, status, headers, config) {
                if (data.result) {
                    GetNowCartList();
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        $scope.CloseEdit = function () {
            $scope.fd = null;
            $scope.isShowEdit = false;
        };
        $scope.ShowEditAddProduct = function () {
            SetWishList([]);
            $scope.cart = { member_detail_id: -1, wishs: [] };
            $scope.isShowEditProduct = true;
            $scope.isViewWorking = false;
        };
        $scope.ShowEditViewProduct = function (member_detail_id, product_sn) {
            $http.get(gb_approot + 'Cart/ViewCart', {
                params: {
                    member_detail_id: member_detail_id,
                    product_sn: product_sn
                }
            })
                .success(function (data, status, headers, config) {
                if (data.result) {
                    SetWishList(data.data.wishs);
                    $scope.cart = data.data;
                    $scope.isShowEditProduct = true;
                    $scope.isViewWorking = true;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        $scope.CloseEditProduct = function () {
            $scope.isShowEditProduct = false;
            $scope.is_godson = false;
        };
        $scope.ShowOrderDetail = function () {
            if ($scope.cart.member_detail_id > 0) {
                $http.get(gb_approot + apiGetAction + '/GetOrderDetail', {
                    params: {
                        member_detail_id: $scope.cart.member_detail_id,
                    }
                })
                    .success(function (data, status, headers, config) {
                    if (data.result) {
                        $scope.order_detail = data.data;
                        if (data.data.length <= 0) {
                            alert("去年無購買紀錄!");
                        }
                    }
                    else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            }
            else {
                alert("請選擇好會員後再查詢!!");
            }
        };
        $scope.CalcLunar = function () {
            workService.getCalcLunar($scope.cart.SY, $scope.cart.SM, $scope.cart.SD)
                .success(function (data, status, headers, config) {
                $scope.cart.LY = data.LY;
                $scope.cart.LM = data.M;
                $scope.cart.LD = data.D;
                $scope.cart.isOnLeapMonth = data.IsLeap;
            }).error(function (data) {
                showAjaxError(data);
            });
        };
        function GetOrder(orders_sn) {
            workService.getOrderData(orders_sn)
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.fd.member_id = data.data.member_id;
                    $scope.fd.member_name = data.data.member_name;
                    $scope.fd.member_detail_id = data.data.member_detail_id;
                    $scope.fd.tel = data.data.tel;
                    $scope.fd.zip = data.data.zip;
                    $scope.fd.address = data.data.address;
                    $scope.fd.gender = data.data.gender;
                    $scope.fd.mobile = data.data.mobile;
                    for (var i in data.data.getOrders_Detail) {
                        var order_detail = data.data.getOrders_Detail[i];
                    }
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
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
                showAjaxError(data);
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
                    ClearCart();
                    $scope.fd = {
                        member_id: member_id,
                        is_light_serial: true,
                        member_name: data.data.member_name,
                        member_detail_id: data.data.member_detail_id,
                        address: data.data.city + data.data.country + data.data.address,
                        zip: data.data.zip,
                        gender: data.data.gender == '1' ? '男' : '女',
                        tel: data.data.tel,
                        mobile: data.data.mobile
                    };
                    SetMemberToCart();
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        ;
        function GetProductAll() {
            workService.getProductWishLight()
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.pds = data.data;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        ;
        function GetWishList() {
            workService.getWishList()
                .success(function (data, status, headers, config) {
                if (data.result) {
                    data.data.forEach(function (o) {
                        if (!o.can_text)
                            o.wish_text = o.wish_name;
                    });
                    $scope.wishs = data.data;
                    $scope.wishlen = 0;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        ;
        function SetWishList(wish) {
            $scope.wishlen = wish.length;
            $scope.wishs.forEach(function (i) {
                var obj = wish.filter(function (x) { return x.wish_id == i.wish_id; });
                if (obj.length > 0) {
                    i.wish_checked = 1;
                    i.wish_text = obj[0].wish_text;
                }
                else {
                    i.wish_checked = 0;
                    if (i.can_text)
                        i.wish_text = null;
                }
            });
        }
        $scope.checkWishList = function ($index) {
            var items = $scope.wishs;
            var item = items[$index];
            var select = items.filter(function (x) { return x.wish_checked; });
            $scope.wishlen = select.length;
            if (item.wish_checked) {
                var obj = {
                    wish_id: item.wish_id,
                    wish_text: item.wish_text,
                    can_text: item.can_text,
                    edit_type: 1
                };
                $scope.cart.wishs.push(obj);
            }
            else if (!item.wish_checked) {
                var i = findIndex($scope.cart.wishs, "wish_id", item.wish_id);
                $scope.cart.wishs.splice(i, 1);
                if (item.can_text)
                    item.wish_text = null;
            }
        };
        $scope.changeWishText = function (id, text) {
            var i = findIndex($scope.cart.wishs, "wish_id", id);
            $scope.cart.wishs[i].wish_text = text;
        };
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
                showAjaxError(data);
            });
            ;
        }
        function GetNowCartList() {
            $http.get(gb_approot + 'Cart/ListCartItems', { params: {} })
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.fd.Item = data.data.Item;
                    $scope.fd.total = data.data.total;
                    $scope.fd.gold = data.data.gold;
                    $scope.fd.race = data.data.race;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        ;
        function SetMemberToCart() {
            $http.post(gb_approot + 'Cart/SetCartMaster', $scope.fd)
                .success(function (data, status, headers, config) {
                if (data.result) {
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        function ClearCart() {
            $http.get(gb_approot + 'Cart/ClearCart', { params: { t: uniqid() } })
                .success(function (data, status, headers, config) {
                if (!data.result) {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        if ($scope.edit_type == 2) {
        }
        if ($scope.edit_type == 1) {
            if ($state.params.member_id != undefined) {
                GetMemberByDetail($state.params.member_id);
                GetMemberAll($state.params.member_id);
            }
        }
        $scope.$watch('cart.product_sn', function (newValue, oldValue) {
            if (newValue != undefined) {
                for (var i in $scope.pds) {
                    var n = $scope.pds[i];
                    if (n.product_sn == newValue) {
                        $scope.cart.price = n.price;
                        $scope.cart.product_name = n.product_name;
                        $scope.cart.product_sn = n.product_sn;
                        $scope.cart.race = 0;
                        $scope.cart.gold = 0;
                        if (allowSetPrice.indexOf(n.category) >= 0) {
                            $scope.cart_price_disable = false;
                            $scope.cart_race_disable = true;
                            $scope.cart_gold_disable = true;
                        }
                        else if (allowSetRace.indexOf(n.category) >= 0) {
                            $scope.cart_price_disable = true;
                            $scope.cart_race_disable = false;
                            $scope.cart_gold_disable = true;
                        }
                        else if (allowSetGold.indexOf(n.category) >= 0) {
                            $scope.cart_price_disable = true;
                            $scope.cart_race_disable = true;
                            $scope.cart_gold_disable = false;
                        }
                        else {
                            $scope.cart_price_disable = true;
                            $scope.cart_race_disable = true;
                            $scope.cart_gold_disable = true;
                        }
                    }
                }
            }
        });
        $scope.$watch('cart.member_detail_id', function (newValue, oldValue) {
            if (newValue != undefined) {
                for (var i in $scope.mb.getMember_Detail) {
                    var n = $scope.mb.getMember_Detail[i];
                    if (n.member_detail_id == newValue) {
                        $scope.cart.born_time = n.born_time;
                        $scope.cart.born_sign = n.born_sign;
                        $scope.cart.member_name = n.member_name;
                        $scope.cart.l_birthday = n.lbirthday;
                        $scope.cart.address = n.city + n.country + n.address;
                        $scope.cart.gender = n.gender;
                        $scope.cart.tel = n.tel;
                        $scope.cart.mobile = n.mobile;
                        $scope.order_detail = null;
                        $scope.is_godson = n.is_godson;
                        var lbirthday = $scope.cart.l_birthday.split("/");
                        if (lbirthday.length == 3) {
                            $scope.cart.LY = parseInt(lbirthday[0]) - 1911;
                            $scope.cart.LM = parseInt(lbirthday[1]);
                            $scope.cart.LD = parseInt(lbirthday[2]);
                        }
                    }
                }
            }
        });
        $scope.$watch('fd.is_light_serial', function (newValue, oldValue) {
            if (newValue != undefined) {
                $scope.$emit(callOnName.updateCartMaster_is_light_serial, newValue);
            }
        });
        GetProductAll();
        GetWishList();
    }]);
angular.module('angularApp')
    .controller('ctrl_doulight', ['$scope', '$http', 'workService', '$sce', 'ngDialog', '$state', function ($scope, $http, workService, $sce, ngDialog, $state) {
        console.log('ctrl_doulight');
        var p0 = 0;
        var p0 = 0;
        $scope.born_sign = commData.born_sign;
        $scope.born_time = commData.born_time;
        $scope.isShowEdit = false;
        $scope.isShowEditProduct = false;
        $scope.cart_price_disable = true;
        $scope.cart_race_disable = true;
        $scope.cart_gold_disable = true;
        $scope.ShowEditAddProduct = function () {
            $scope.cart = { member_detail_id: -1 };
            $scope.isShowEditProduct = true;
            $scope.isViewWorking = false;
        };
        $scope.ShowEditViewProduct = function (member_detail_id, product_sn) {
            $http.get(gb_approot + 'Cart/ViewCart', {
                params: {
                    member_detail_id: member_detail_id,
                    product_sn: product_sn
                }
            })
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.cart = data.data;
                    $scope.isShowEditProduct = true;
                    $scope.isViewWorking = true;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        $scope.CloseEditProduct = function () {
            $scope.isShowEditProduct = false;
            $scope.is_godson = false;
        };
        $scope.CalcLunar = function () {
            workService.getCalcLunar($scope.cart.SY, $scope.cart.SM, $scope.cart.SD)
                .success(function (data, status, headers, config) {
                $scope.cart.LY = data.LY;
                $scope.cart.LM = data.M;
                $scope.cart.LD = data.D;
                $scope.cart.isOnLeapMonth = data.IsLeap;
            }).error(function (data) {
                showAjaxError(data);
            });
        };
        function GetOrder(orders_sn) {
            workService.getOrderData(orders_sn)
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.fd.member_id = data.data.member_id;
                    $scope.fd.member_name = data.data.member_name;
                    $scope.fd.member_detail_id = data.data.member_detail_id;
                    $scope.fd.tel = data.data.tel;
                    $scope.fd.zip = data.data.zip;
                    $scope.fd.address = data.data.address;
                    $scope.fd.gender = data.data.gender;
                    $scope.fd.mobile = data.data.mobile;
                    for (var i in data.data.getOrders_Detail) {
                        var order_detail = data.data.getOrders_Detail[i];
                    }
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        ;
        function GetProductAll() {
            workService.getProductDouLight()
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.pds = data.data;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        ;
        function GetFortune() {
            $http.get(gb_approot + apiGetAction + '/GetFortune')
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.pds_fortune = data.data;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
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
                showAjaxError(data);
            });
            ;
        }
        function GetNowCartList() {
            $http.get(gb_approot + 'Cart/ListCartItems', { params: {} })
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.fd.Item = data.data;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        ;
        function SetMemberToCart() {
            $http.post(gb_approot + 'Cart/SetCartMaster', $scope.fd)
                .success(function (data, status, headers, config) {
                if (data.result) {
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        function ClearCart() {
            $http.get(gb_approot + 'Cart/ClearCart', { params: { t: uniqid() } })
                .success(function (data, status, headers, config) {
                if (!data.result) {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        if ($scope.edit_type == 2) {
            workService.getFortune($state.params.orders_sn)
                .success(function (fdata, status, headers, config) {
                $scope.qMasterView.then(function () {
                    for (var i in $scope.mb.getMember_Detail) {
                        var item = $scope.mb.getMember_Detail[i];
                        if (fdata.data.indexOf(item.member_detail_id) >= 0) {
                            item.fortune_value = item.member_detail_id;
                        }
                    }
                });
            });
        }
        if ($scope.edit_type == 1) {
            if ($state.params.member_id != undefined) {
                $scope.qMember.success(function (data, status, headers, config) {
                    var detail;
                    detail = {
                        member_detail_id: $state.params.member_id,
                        member_name: $scope.fd.member_name,
                        product_sn: ''
                    };
                    $scope.fd.Item.push(detail);
                });
            }
        }
        $scope.$watch('cart.member_detail_id', function (newValue, oldValue) {
            if (newValue != undefined) {
                for (var i in $scope.mb.getMember_Detail) {
                    var n = $scope.mb.getMember_Detail[i];
                    if (n.member_detail_id == newValue) {
                        $scope.cart.born_time = n.born_time;
                        $scope.cart.born_sign = n.born_sign;
                        $scope.cart.member_name = n.member_name;
                        $scope.cart.l_birthday = n.lbirthday;
                        $scope.cart.address = n.city + n.country + n.address;
                        $scope.cart.gender = n.gender;
                        $scope.cart.tel = n.tel;
                        $scope.cart.mobile = n.mobile;
                        $scope.is_godson = n.is_godson;
                        var lbirthday = $scope.cart.l_birthday.split("/");
                        if (lbirthday.length == 3) {
                            $scope.cart.LY = parseInt(lbirthday[0]) - 1911;
                            $scope.cart.LM = parseInt(lbirthday[1]);
                            $scope.cart.LD = parseInt(lbirthday[2]);
                        }
                    }
                }
            }
        });
        $scope.ProductChange = function (product_sn) {
            for (var i in $scope.pds) {
                var n = $scope.pds[i];
                if (n.product_sn == product_sn) {
                    $scope.fd.Item[0].price = n.price;
                    $scope.fd.Item[0].product_name = n.product_name;
                    $scope.fd.Item[0].race = 0;
                    $scope.fd.Item[0].gold = 0;
                }
            }
            GetLight(product_sn);
        };
        $scope.DeleteOrderFourtune = function () {
            if (confirm('確定刪除訂單?')) {
                $http.get(gb_approot + 'Orders/DeleteOrdersDouLight', { params: { orders_sn: $scope.fd.orders_sn } })
                    .success(function (data, status, headers, config) {
                    if (data.result) {
                        $scope.Init_Query();
                        $scope.$parent.GoGrid();
                    }
                    else {
                        alert(data.message);
                    }
                });
            }
            else {
                return;
            }
        };
        $scope.checkFortuneNum = function () {
            var items = $scope.mb.getMember_Detail;
            for (var i in items) {
                var item = items[i];
            }
        };
        function GetLight(product_sn) {
            workService.getLightByMD(product_sn)
                .success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.lights = data.data;
                }
                else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        ;
        $scope.openSortData = function () {
            $scope.isShow = true;
            console.log($scope.mb);
        };
        $scope.closeSortData = function () {
            $scope.isShow = false;
        };
        GetProductAll();
    }]);
