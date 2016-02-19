interface IOrder extends IScopeM<server.cartMaster> {
    mb: server.Member;
    pds: server.Product[];
    lights: server.Light_Site[];
    pds_fortune: server.Product[];//福燈類
    mjs: server.Manjushri[];
    cart: server.cartDetail;
    born_sign: string[];
    downloadExcel: string;
    born_time: IKeyValueS[];
    orders_type: IKeyValue[];//訂單分類
    cart_price_disable: boolean;
    cart_race_disable: boolean;
    cart_gold_disable: boolean;
    isShowEditProduct: boolean;
    isShowEdit: boolean;
    isViewWorking: boolean;
    isTodayOrder: boolean;
    users: server.Users[];
    order_detail: server.Orders_Detail[];

    ShowEdit(): void;
    CloseEdit(): void;
    GoGrid(): void;

    ShowEditAddProduct(): void;
    ShowEditViewProduct(member_detail_id: number, product_sn: string): void;
    CloseEditProduct(): void;
    ShowOrderDetail(): void;

    SubmitOrder(): void;
    SubmitCart(): void;
    RemoveCart(member_detail_id: number, product_sn: string): void;
    CalcLunar(): void;
    DownLoadExcel_Thanks(): void;
    ProductChange(product_sn: string): void;
    DeleteOrderFourtune(): void;

    openOrders(orders_sn, orders_type): void;
    checkFortuneNum(): void;

    qMember: ng.IHttpPromise<IResultData<server.Member_Detail>>;
    qMasterView: ng.IPromise<{}>;

    $parent: IParent;
    UnitId: number;
    UserId: number;
    allowReject: number;

    //ng-sortable
    openSortData(): void;
    closeSortData(): void;
    isShow: boolean;
    sort_member: server.Sort_Member[];
}
interface IOrderCookie extends ng.cookies.ICookiesService {
    unit_id: number;
    user_id: number;
    allowReject: number;
}
interface IParent extends ng.IScope {
    GoGrid(): void;
    NowPage: number;
    Init_Query(): void;
}

angular
    .module('angularApp', ['commfun', 'siyfion.sfTypeahead', 'ngDialog', 'ui.router', 'ngCookies'])
    .config(['$httpProvider', '$stateProvider', '$urlRouterProvider', function ($httpProvider,
    $stateProvider: ng.ui.IStateProvider,
    $urlRouterProvider: ng.ui.IUrlRouterProvider) {
    // 防止IE對Ajax Get做Cache
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

}]);

angular.module('angularApp')
    .controller('ctrl', ['$scope', '$http', 'workService', 'gridpage', '$sce', 'ngDialog', '$state', '$cookies',
    function ($scope: IOrder,
        $http: ng.IHttpService,
        workService: services.workService,
        gridpage,
        $sce: ng.ISCEService,
        ngDialog,
        $state: ng.ui.IStateService,
        $cookies: IOrderCookie
        ) {
        console.log('ctrl');
        $scope.orders_type = commData.Orders_type;
        var p0: number = 0;
        $scope.sd = {
            InsertUserId: 0,
            order_sn: ''
        };

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
        $scope.GoGrid = function () {
            $state.go('grid');
        }

        $scope.openOrders = function (orders_sn, orders_type) {
            if (orders_type == e_orders_type.general) {
                $state.go('edit.general_orders', { 'orders_sn': orders_sn });
            }
            if (orders_type == e_orders_type.fortune_order) {
                $state.go('edit.fortune_orders', { 'orders_sn': orders_sn });
            }
            if (orders_type == e_orders_type.sdlight) {
                $state.go('edit.sdlight_orders', { 'orders_sn': orders_sn });
            }
            if (orders_type == e_orders_type.mdlight) {
                $state.go('edit.mdlight_orders', { 'orders_sn': orders_sn });
            }
        };

        function GetUsers() {
            workService.getUsers()
                .success(function (data: IResultData<server.Users[]>, status, headers, config) {
                if (data.result) {
                    $scope.users = data.data;
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        $scope.allowReject = $cookies.allowReject;

        if (p0 > 0) { // 進入為修改模式

        } else { //進入為新增模式
            if (member_id > 0) {
                $scope.edit_type = IEditType.insert;
            }
        }
        $scope.Init_Query();
        GetUsers();
    }]);

angular.module('angularApp')
    .controller('ctrl_master', ['$scope', '$http', 'workService', '$sce', 'ngDialog', '$state', '$q',
    function (
        $scope: IOrder,
        $http: ng.IHttpService,
        workService: services.workService,
        $sce: ng.ISCEService,
        ngDialog,
        $state: ng.ui.IStateService,
        $q: ng.IQService
        ) {
        console.log('ctrl_master');
        var p0: number = 0;

        var allowSetPrice: string[] = ['香油'];
        var allowSetRace: string[] = ['白米'];
        var allowSetGold: string[] = ['金牌'];
        var allowSetGodSon: string[] = ['契子'];

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
        };
        function SendGeneralOrder() {

            if ($scope.edit_type == IEditType.insert) {
                $http.post(gb_approot + 'Orders/AddOrders', $scope.fd)
                    .success(function (data: IResultData<string>, status, headers, config) {
                    if (data.result) {
                        //回傳訂單編號
                        $http.get(gb_approot + 'Cart/OrdersToSession', { params: { orders_sn: data.data } })
                            .success(function (data: IResultData<server.cartMaster>, status, headers, config) {
                            if (data.result) {
                                $scope.fd = data.data;
                                $scope.edit_type = IEditType.update;
                                $scope.$parent.Init_Query();
                                alert('訂單新增完成');
                            } else {
                                alert(data.message);
                            }
                        })
                            .error(function (data, status, headers, config) {
                            showAjaxError(data);
                        });

                    } else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            };

            if ($scope.edit_type == IEditType.update) {
                $http.put(gb_approot + 'Orders/UpdateOrders', $scope.fd)
                    .success(function (data: IResultData<server.Orders>, status, headers, config) {
                    if (data.result) {
                        $scope.edit_type = IEditType.update;
                        $scope.$parent.Init_Query();
                        alert('訂單修改完成');
                    } else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            };

        };
        function SendFortuneOrder() {
            console.log($scope.fd.Item);
            var item = $scope.fd.Item[0];

            if ($scope.edit_type == IEditType.insert) {
                var m: number[] = [];
                for (var i in $scope.mb.getMember_Detail) {
                    var n = $scope.mb.getMember_Detail[i];
                    if (n.fortune_value > 0) {
                        m.push(n.fortune_value);
                    }
                }
                $http.post(gb_approot + 'Orders/AddOrdersFortune',
                    {
                        'master': $scope.fd,
                        'product_sn': item.product_sn,
                        'member_detail_ids': m
                    })
                    .success(function (data: IResultData<string>, status, headers, config) {
                    if (data.result) {
                        //回傳訂單編號
                        $http.get(gb_approot + 'Cart/OrdersToSession', { params: { orders_sn: data.data } })
                            .success(function (data: IResultData<server.cartMaster>, status, headers, config) {
                            if (data.result) {
                                $scope.fd = data.data;
                                $scope.edit_type = IEditType.update;
                                $scope.$parent.Init_Query();
                                alert('訂單新增完成');
                            } else {
                                alert(data.message);
                            }
                        })
                            .error(function (data, status, headers, config) {
                            showAjaxError(data);
                        });

                    } else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            };

            if ($scope.edit_type == IEditType.update) {

                var m: number[] = [];
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
                    .success(function (data: IResultData<server.Orders>, status, headers, config) {
                    if (data.result) {
                        $scope.edit_type = IEditType.update;
                        $scope.$parent.Init_Query();
                        alert('訂單修改完成');
                    } else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            };
        };
        function SendSDLightOrder() {

            if ($scope.edit_type == IEditType.insert) {
                $http.post(gb_approot + 'Orders/AddSDLight', $scope.fd)
                    .success(function (data: IResultData<string>, status, headers, config) {
                    if (data.result) {
                        //回傳訂單編號
                        $http.get(gb_approot + 'Cart/OrdersToSession', { params: { orders_sn: data.data } })
                            .success(function (data: IResultData<server.cartMaster>, status, headers, config) {
                            if (data.result) {
                                $scope.fd = data.data;
                                $scope.edit_type = IEditType.update;
                                $scope.$parent.Init_Query();
                                alert('訂單新增完成');
                            } else {
                                alert(data.message);
                            }
                        })
                            .error(function (data, status, headers, config) {
                            showAjaxError(data);
                        });

                    } else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            };

            if ($scope.edit_type == IEditType.update) {
                $http.put(gb_approot + 'Orders/UpdateSDLight', $scope.fd)
                    .success(function (data: IResultData<server.Orders>, status, headers, config) {
                    if (data.result) {
                        $scope.edit_type = IEditType.update;
                        $scope.$parent.Init_Query();
                        alert('訂單修改完成');
                    } else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            };

        };
        function SendMDLightOrder() {

            if ($scope.edit_type == IEditType.insert) {
                $http.post(gb_approot + 'Orders/AddMDLight', $scope.fd)
                    .success(function (data: IResultData<string>, status, headers, config) {
                    if (data.result) {
                        //回傳訂單編號
                        $http.get(gb_approot + 'Cart/OrdersToSession', { params: { orders_sn: data.data } })
                            .success(function (data: IResultData<server.cartMaster>, status, headers, config) {
                            if (data.result) {
                                $scope.fd = data.data;
                                $scope.edit_type = IEditType.update;
                                $scope.$parent.Init_Query();
                                alert('訂單新增完成');
                            } else {
                                alert(data.message);
                            }
                        })
                            .error(function (data, status, headers, config) {
                            showAjaxError(data);
                        });

                    } else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            };

            if ($scope.edit_type == IEditType.update) {
                $http.put(gb_approot + 'Orders/UpdateMDLight', $scope.fd)
                    .success(function (data: IResultData<server.Orders>, status, headers, config) {
                    if (data.result) {
                        $scope.edit_type = IEditType.update;
                        $scope.$parent.Init_Query();
                        alert('訂單修改完成');
                    } else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            };

        };

        $scope.CalcLunar = function () {
            workService.getCalcLunar($scope.cart.SY, $scope.cart.SM, $scope.cart.SD)
                .success(function (data: server.LuniInfo, status, headers, config) {
                $scope.cart.LY = data.LY;
                $scope.cart.LM = data.M;
                $scope.cart.LD = data.D;
                $scope.cart.isOnLeapMonth = data.IsLeap;
            }).error(function (data) {
                showAjaxError(data);
            })
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

                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        function GetMemberAll(member_id) {
            return workService.getMemberAll(member_id)
                .success(function (data: IResultData<server.Member>, status, headers, config) {
                if (data.result) {
                    $scope.mb = data.data;
                    //ng-sortable
                    //var tmp: server.Sort_Member[] = [];
                    //for (var i in $scope.mb.getMember_Detail) {
                    //    var n = $scope.mb.getMember_Detail[i];
                    //        tmp.push({ member_detail_id: n.member_detail_id, member_name: n.member_name });
                    //}
                    //$scope.sort_member = tmp;
                    //----------
                } else {
                    alert(data.message);
                }

            })
                .error(function (data, status, headers, config) {

                showAjaxError(data);

            });
        };
        function GetMemberByDetail(member_id) {
            //取得戶長資料
            var r = workService.getMemberByDetail(member_id)
                .success(function (data: IResultData<server.Member_Detail>, status, headers, config) {
                if (data.result) {
                    if (member_id != data.data.member_id) {
                        alert('系統回傳member_id不一致');
                        return;
                    }
                    ClearCart();//先清楚之前的Sesion
                    $scope.fd = <server.cartMaster>
                    {
                        member_id: member_id,
                        is_light_serial: true, //燈位產品預設採連續排位
                        member_name: data.data.member_name,
                        member_detail_id: data.data.member_detail_id,
                        address: data.data.city + data.data.country + data.data.address,
                        zip: data.data.zip,
                        gender: data.data.gender == '1' ? '男' : '女',
                        tel: data.data.tel,
                        mobile: data.data.mobile,
                        Item: []
                    };
                    SetMemberToCart(); //將資料回傳Server設定至Session儲存[只有表頭]

                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });

            return r;
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
                showAjaxError(data);
            });
        };
        function GetFortune() {
            $http.get(gb_approot + apiGetAction + '/GetFortune')
                .success(function (data: IResultData<server.Product[]>, status, headers, config) {
                if (data.result) {
                    $scope.pds_fortune = data.data;
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
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
                showAjaxError(data);
            });;
        }
        function GetNowCartList() {
            $http.get(gb_approot + 'Cart/ListCartItems', { params: {} })
                .success(function (data: IResultData<server.cartMaster>, status, headers, config) {
                if (data.result) {
                    $scope.fd.Item = data.data.Item;
                    $scope.fd.total = data.data.total;
                    $scope.fd.race = data.data.race;
                    $scope.fd.gold = data.data.gold;
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        function SetMemberToCart() {
            $http.post(gb_approot + 'Cart/SetCartMaster', $scope.fd)
                .success(function (data: IResultBase, status, headers, config) {
                if (data.result) {
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        function ClearCart() {
            //清除Session
            $http.get(gb_approot + 'Cart/ClearCart', { params: { t: uniqid() } })
                .success(function (data: IResultBase, status, headers, config) {
                if (!data.result) {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        function GetLight(product_sn: string) {
            workService.getLightByMD(product_sn)
                .success(function (data: IResultData<server.Light_Site[]>, status, headers, config) {
                if (data.result) {
                    $scope.lights = data.data;
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        function MasterDataView(orders_sn) {
            var q = $q.defer<{}>();

            $http.get(gb_approot + 'Orders/CheckOrderType', { params: { orders_sn: orders_sn } })
                .success(function (data: IResultBase, status, headers, config) {
                if (data.message != '') {
                    document.location.href = data.message;
                } else {
                    //清除Session
                    $http.get(gb_approot + 'Cart/ClearCart', { params: {} })
                        .success(function (data: IResultBase, status, headers, config) {
                        if (data.result) {
                            $http.get(gb_approot + 'Orders/GetToSession', { params: { orders_sn: orders_sn } })
                                .success(function (data: IResultData<server.cartMaster>, status, headers, config) {
                                if (data.result) {
                                    $scope.fd = data.data;
                                    $scope.fd.transation_date = new Date(data.data.transation_date.toString());
                                    $scope.fd.is_light_serial = true;
                                    $scope.edit_type = IEditType.update;

                                    var LocalDate = new Date(
                                        $scope.fd.transation_date.getFullYear(),
                                        $scope.fd.transation_date.getMonth(),
                                        $scope.fd.transation_date.getDate(),
                                        $scope.fd.transation_date.getHours() - 8
                                        );
                                    var nowToday = new Date();
                                    if (LocalDate.getFullYear() == nowToday.getFullYear() &&
                                        LocalDate.getMonth() == nowToday.getMonth() &&
                                        LocalDate.getDate() == nowToday.getDate()
                                        ) {
                                        $scope.isTodayOrder = true;
                                    } else {
                                        $scope.isTodayOrder = false;
                                    }
                                    console.log($scope.isTodayOrder, 'Local Date', LocalDate);
                                    GetMemberAll(data.data.member_id).success(function (data) {
                                        q.resolve();
                                    });
                                } else {
                                    alert(data.message);
                                }
                            }).error(function (data) {
                                q.reject();
                                showAjaxError(data);
                            });
                        } else {
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
        };

        if ($state.params.orders_sn != undefined) { // 進入為修改模式
            $scope.edit_type = IEditType.update;
            $scope.qMasterView = MasterDataView($state.params.orders_sn);
        } else { //進入為新增模式
            $scope.edit_type = IEditType.insert;
            if ($state.params.member_id != undefined) {
                $scope.edit_type = IEditType.insert;
                $scope.qMember = GetMemberByDetail($state.params.member_id);
                GetMemberAll($state.params.member_id);
                $scope.isTodayOrder = true;
            }
        }
    }]);

angular.module('angularApp')
    .controller('ctrl_general', ['$scope', '$http', 'workService', '$sce', 'ngDialog', '$state',
    function (
        $scope: IOrder,
        $http: ng.IHttpService,
        workService: services.workService,
        $sce: ng.ISCEService,
        ngDialog,
        $state: ng.ui.IStateService
        ) {
        console.log('ctrl_general');
        var p0: number = 0;

        var allowSetPrice: string[] = ['香油'];
        var allowSetRace: string[] = ['白米'];
        var allowSetGold: string[] = ['金牌'];
        var allowSetGodSon: string[] = ['契子'];

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
                .success(function (data: IResultBase, status, headers, config) {
                if (data.result) {
                    GetNowCartList();
                } else {
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
                .success(function (data: IResultBase, status, headers, config) {
                if (data.result) {
                    GetNowCartList();
                } else {
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
            $scope.cart = <server.cartDetail>{ member_detail_id: -1 };
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
                .success(function (data: IResultData<server.cartDetail>, status, headers, config) {
                if (data.result) {
                    $scope.cart = data.data;
                    $scope.isShowEditProduct = true;
                    $scope.isViewWorking = true;
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        $scope.CloseEditProduct = function () {
            $scope.isShowEditProduct = false;
        };
        $scope.ShowOrderDetail = function () {
            if ($scope.cart.member_detail_id > 0) {
                $http.get(gb_approot + apiGetAction + '/GetOrderDetail', {
                    params: {
                        member_detail_id: $scope.cart.member_detail_id,
                    }
                })
                    .success(function (data: IResultData<server.Orders_Detail[]>, status, headers, config) {
                    if (data.result) {
                        $scope.order_detail = data.data;
                        if (data.data.length <= 0) {
                            alert("去年無購買紀錄!");
                        }
                    } else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            } else {
                alert("請選擇好會員後再查詢!!");
            }
        };
        $scope.CalcLunar = function () {
            workService.getCalcLunar($scope.cart.SY, $scope.cart.SM, $scope.cart.SD)
                .success(function (data: server.LuniInfo, status, headers, config) {
                $scope.cart.LY = data.LY;
                $scope.cart.LM = data.M;
                $scope.cart.LD = data.D;
                $scope.cart.isOnLeapMonth = data.IsLeap;
            }).error(function (data) {
                showAjaxError(data);
            })
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

                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
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
                showAjaxError(data);
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
                    ClearCart();//先清楚之前的Sesion
                    $scope.fd = <server.cartMaster>
                    {
                        member_id: member_id,
                        is_light_serial: true, //燈位產品預設採連續排位
                        member_name: data.data.member_name,
                        member_detail_id: data.data.member_detail_id,
                        address: data.data.city + data.data.country + data.data.address,
                        zip: data.data.zip,
                        gender: data.data.gender == '1' ? '男' : '女',
                        tel: data.data.tel,
                        mobile: data.data.mobile
                    };
                    SetMemberToCart(); //將資料回傳Server設定至Session儲存[只有表頭]

                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
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
                showAjaxError(data);
            });
        };
        function GetFortune() {
            $http.get(gb_approot + apiGetAction + '/GetFortune')
                .success(function (data: IResultData<server.Product[]>, status, headers, config) {
                if (data.result) {
                    $scope.pds_fortune = data.data;
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
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
                showAjaxError(data);
            });;
        }
        function GetNowCartList() {
            $http.get(gb_approot + 'Cart/ListCartItems', { params: {} })
                .success(function (data: IResultData<server.cartMaster>, status, headers, config) {
                if (data.result) {
                    $scope.fd.Item = data.data.Item;
                    $scope.fd.total = data.data.total;
                    $scope.fd.gold = data.data.gold;
                    $scope.fd.race = data.data.race;
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        function SetMemberToCart() {
            $http.post(gb_approot + 'Cart/SetCartMaster', $scope.fd)
                .success(function (data: IResultBase, status, headers, config) {
                if (data.result) {
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        function ClearCart() {
            //清除Session
            $http.get(gb_approot + 'Cart/ClearCart', { params: { t: uniqid() } })
                .success(function (data: IResultBase, status, headers, config) {
                if (!data.result) {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }

        if ($scope.edit_type == IEditType.update) { // 進入為修改模式
            //GetMemberAll($scope.fd.member_id);
        }

        if ($scope.edit_type == IEditType.insert) { //進入為新增模式
            if ($state.params.member_id != undefined) {
                GetMemberByDetail($state.params.member_id);
                GetMemberAll($state.params.member_id);
            }
        }

        $scope.$watch('cart.product_sn', function (newValue: string, oldValue) {
            if (newValue != undefined) {
                for (var i in $scope.pds) {
                    var n = $scope.pds[i];
                    if (n.product_sn == newValue) {
                        $scope.cart.price = n.price;
                        $scope.cart.product_name = n.product_name;
                        $scope.cart.product_sn = n.product_sn;
                        $scope.cart.race = 0;
                        $scope.cart.gold = 0;
                        if (allowSetPrice.indexOf(n.category) >= 0) { //香油類...等產品可設定單價
                            $scope.cart_price_disable = false;
                            $scope.cart_race_disable = true;
                            $scope.cart_gold_disable = true;
                        }
                        else if (allowSetRace.indexOf(n.category) >= 0) {
                            $scope.cart_price_disable = true;
                            $scope.cart_race_disable = false;
                            $scope.cart_gold_disable = true;
                        } else if (allowSetGold.indexOf(n.category) >= 0) {
                            $scope.cart_price_disable = true;
                            $scope.cart_race_disable = true;
                            $scope.cart_gold_disable = false;
                        } else if (allowSetGodSon.indexOf(n.category) >= 0) {//契子觀摩可設定單價
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
        $scope.$watch('cart.member_detail_id', function (newValue: number, oldValue) {
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

                        var lbirthday: string[] = $scope.cart.l_birthday.split("/");
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
    .controller('ctrl_fortune', ['$scope', '$http', 'workService', '$sce', 'ngDialog', '$state',
    function (
        $scope: IOrder,
        $http: ng.IHttpService,
        workService: services.workService,
        $sce: ng.ISCEService,
        ngDialog,
        $state: ng.ui.IStateService
        ) {
        console.log('ctrl_fortune');
        var p0: number = 0;

        $scope.born_sign = commData.born_sign;
        $scope.born_time = commData.born_time;
        $scope.isShowEdit = false;
        $scope.isShowEditProduct = false;
        $scope.cart_price_disable = true;
        $scope.cart_race_disable = true;
        $scope.cart_gold_disable = true;

        $scope.ShowEditAddProduct = function () {
            $scope.cart = <server.cartDetail>{ member_detail_id: -1 };
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
                .success(function (data: IResultData<server.cartDetail>, status, headers, config) {
                if (data.result) {
                    $scope.cart = data.data;
                    $scope.isShowEditProduct = true;
                    $scope.isViewWorking = true;
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        $scope.CloseEditProduct = function () {
            $scope.isShowEditProduct = false;
        };

        $scope.CalcLunar = function () {
            workService.getCalcLunar($scope.cart.SY, $scope.cart.SM, $scope.cart.SD)
                .success(function (data: server.LuniInfo, status, headers, config) {
                $scope.cart.LY = data.LY;
                $scope.cart.LM = data.M;
                $scope.cart.LD = data.D;
                $scope.cart.isOnLeapMonth = data.IsLeap;
            }).error(function (data) {
                showAjaxError(data);
            })
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

                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        function GetProductAll() {
            workService.getProductFortune()
                .success(function (data: IResultData<server.Product[]>, status, headers, config) {
                if (data.result) {
                    $scope.pds = data.data;
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        function GetFortune() {
            $http.get(gb_approot + apiGetAction + '/GetFortune')
                .success(function (data: IResultData<server.Product[]>, status, headers, config) {
                if (data.result) {
                    $scope.pds_fortune = data.data;
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
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
                showAjaxError(data);
            });;
        }
        function GetNowCartList() {
            $http.get(gb_approot + 'Cart/ListCartItems', { params: {} })
                .success(function (data: IResultData<server.cartDetail[]>, status, headers, config) {
                if (data.result) {
                    $scope.fd.Item = data.data;
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        function SetMemberToCart() {
            $http.post(gb_approot + 'Cart/SetCartMaster', $scope.fd)
                .success(function (data: IResultBase, status, headers, config) {
                if (data.result) {
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        function ClearCart() {
            //清除Session
            $http.get(gb_approot + 'Cart/ClearCart', { params: { t: uniqid() } })
                .success(function (data: IResultBase, status, headers, config) {
                if (!data.result) {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }

        if ($scope.edit_type == IEditType.update) { // 進入為修改模式
            //Master_Open_Modify($state.params.orders_sn);
            workService.getFortune($state.params.orders_sn)
                .success(function (fdata: IResultData<number[]>, status, headers, config) {

                $scope.qMasterView.then(function () {
                    for (var i in $scope.mb.getMember_Detail) {
                        var item = $scope.mb.getMember_Detail[i];
                        if (fdata.data.indexOf(item.member_detail_id) >= 0) {
                            item.fortune_value = item.member_detail_id;
                        }
                    }
                })
            });
        }

        if ($scope.edit_type == IEditType.insert) { //進入為新增模式
            if ($state.params.member_id != undefined) {
                $scope.qMember.success(function (data: IResultData<server.Member_Detail>, status, headers, config) {
                    var detail: server.cartDetail;
                    detail = <server.cartDetail>{
                        member_detail_id: $state.params.member_id,
                        member_name: $scope.fd.member_name,
                        product_sn: ''
                    };
                    $scope.fd.Item.push(detail);
                    //GetMemberAll($state.params.member_id);
                });
            }
        }
        $scope.$watch('cart.member_detail_id', function (newValue: number, oldValue) {
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

                        var lbirthday: string[] = $scope.cart.l_birthday.split("/");
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
        }
        $scope.DeleteOrderFourtune = function () {
            if (confirm('確定刪除訂單?')) {
                $http.get(gb_approot + 'Orders/DeleteOrdersFortune', { params: { orders_sn: $scope.fd.orders_sn } })
                    .success(function (data: IResultBase, status, headers, config) {
                    if (data.result) {
                        $scope.Init_Query();
                        $scope.$parent.GoGrid();
                    } else {
                        alert(data.message);
                    }
                });
            } else {
                return;
            }
        }
        $scope.checkFortuneNum = function () {
            var items = $scope.mb.getMember_Detail;

            for (var i in items) {
                var item = items[i];
            }
        }

        function GetLight(product_sn: string) {
            workService.getLightByMD(product_sn)
                .success(function (data: IResultData<server.Light_Site[]>, status, headers, config) {
                if (data.result) {
                    $scope.lights = data.data;
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };

        //(ng-sortable)排序設定---start
        $scope.openSortData = function () {
            //var tmp: server.Sort_Member[] = [];
            //for (var i in $scope.mb.getMember_Detail) {
            //    var n = $scope.mb.getMember_Detail[i];
            //    if (n.fortune_value > 0) {
            //        tmp.push({ member_detail_id: n.fortune_value, member_name:n.member_name,sort:0});
            //    }
            //}
            //$scope.sort_member = tmp;
            $scope.isShow = true;
            console.log($scope.mb);
        };
        $scope.closeSortData = function () {
            $scope.isShow = false;
        };
        //(ng-sortable)排序設定---end
        GetProductAll();
    }]);

angular.module('angularApp')
    .controller('ctrl_sdlight', ['$scope', '$http', 'workService', '$sce', 'ngDialog', '$state',
    function (
        $scope: IOrder,
        $http: ng.IHttpService,
        workService: services.workService,
        $sce: ng.ISCEService,
        ngDialog,
        $state: ng.ui.IStateService
        ) {
        console.log('ctrl_general');
        var p0: number = 0;

        var allowSetPrice: string[] = ['香油'];
        var allowSetRace: string[] = ['白米'];
        var allowSetGold: string[] = ['金牌'];

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
                .success(function (data: IResultBase, status, headers, config) {
                if (data.result) {
                    GetNowCartList();
                } else {
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
                .success(function (data: IResultBase, status, headers, config) {
                if (data.result) {
                    GetNowCartList();
                } else {
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
            $scope.cart = <server.cartDetail>{ member_detail_id: -1 };
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
                .success(function (data: IResultData<server.cartDetail>, status, headers, config) {
                if (data.result) {
                    $scope.cart = data.data;
                    $scope.isShowEditProduct = true;
                    $scope.isViewWorking = true;
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        $scope.CloseEditProduct = function () {
            $scope.isShowEditProduct = false;
        };
        $scope.ShowOrderDetail = function () {
            if ($scope.cart.member_detail_id > 0) {
                $http.get(gb_approot + apiGetAction + '/GetOrderDetail', {
                    params: {
                        member_detail_id: $scope.cart.member_detail_id,
                    }
                })
                    .success(function (data: IResultData<server.Orders_Detail[]>, status, headers, config) {
                    if (data.result) {
                        $scope.order_detail = data.data;
                        if (data.data.length <= 0) {
                            alert("去年無購買紀錄!");
                        }
                    } else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            } else {
                alert("請選擇好會員後再查詢!!");
            }
        };
        $scope.CalcLunar = function () {
            workService.getCalcLunar($scope.cart.SY, $scope.cart.SM, $scope.cart.SD)
                .success(function (data: server.LuniInfo, status, headers, config) {
                $scope.cart.LY = data.LY;
                $scope.cart.LM = data.M;
                $scope.cart.LD = data.D;
                $scope.cart.isOnLeapMonth = data.IsLeap;
            }).error(function (data) {
                showAjaxError(data);
            })
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

                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
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
                showAjaxError(data);
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
                    ClearCart();//先清楚之前的Sesion
                    $scope.fd = <server.cartMaster>
                    {
                        member_id: member_id,
                        is_light_serial: true, //燈位產品預設採連續排位
                        member_name: data.data.member_name,
                        member_detail_id: data.data.member_detail_id,
                        address: data.data.city + data.data.country + data.data.address,
                        zip: data.data.zip,
                        gender: data.data.gender == '1' ? '男' : '女',
                        tel: data.data.tel,
                        mobile: data.data.mobile
                    };
                    SetMemberToCart(); //將資料回傳Server設定至Session儲存[只有表頭]

                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        function GetProductAll() {
            workService.getProductSDLight()
                .success(function (data: IResultData<server.Product[]>, status, headers, config) {
                if (data.result) {
                    $scope.pds = data.data;
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
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
                showAjaxError(data);
            });;
        }
        function GetNowCartList() {
            $http.get(gb_approot + 'Cart/ListCartItems', { params: {} })
                .success(function (data: IResultData<server.cartMaster>, status, headers, config) {
                if (data.result) {
                    $scope.fd.Item = data.data.Item;
                    $scope.fd.total = data.data.total;
                    $scope.fd.gold = data.data.gold;
                    $scope.fd.race = data.data.race;
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        function SetMemberToCart() {
            $http.post(gb_approot + 'Cart/SetCartMaster', $scope.fd)
                .success(function (data: IResultBase, status, headers, config) {
                if (data.result) {
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        function ClearCart() {
            //清除Session
            $http.get(gb_approot + 'Cart/ClearCart', { params: { t: uniqid() } })
                .success(function (data: IResultBase, status, headers, config) {
                if (!data.result) {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }

        if ($scope.edit_type == IEditType.update) { // 進入為修改模式
            //GetMemberAll($scope.fd.member_id);
        }

        if ($scope.edit_type == IEditType.insert) { //進入為新增模式
            if ($state.params.member_id != undefined) {
                GetMemberByDetail($state.params.member_id);
                GetMemberAll($state.params.member_id);
            }
        }

        $scope.$watch('cart.product_sn', function (newValue: string, oldValue) {
            if (newValue != undefined) {
                for (var i in $scope.pds) {
                    var n = $scope.pds[i];
                    if (n.product_sn == newValue) {
                        $scope.cart.price = n.price;
                        $scope.cart.product_name = n.product_name;
                        $scope.cart.product_sn = n.product_sn;
                        $scope.cart.race = 0;
                        $scope.cart.gold = 0;
                        if (allowSetPrice.indexOf(n.category) >= 0) { //香油類...等產品可設定單價
                            $scope.cart_price_disable = false;
                            $scope.cart_race_disable = true;
                            $scope.cart_gold_disable = true;
                        }
                        else if (allowSetRace.indexOf(n.category) >= 0) {
                            $scope.cart_price_disable = true;
                            $scope.cart_race_disable = false;
                            $scope.cart_gold_disable = true;
                        } else if (allowSetGold.indexOf(n.category) >= 0) {
                            $scope.cart_price_disable = true;
                            $scope.cart_race_disable = true;
                            $scope.cart_gold_disable = false;
                        } else {
                            $scope.cart_price_disable = true;
                            $scope.cart_race_disable = true;
                            $scope.cart_gold_disable = true;
                        }
                    }
                }
            }
        });
        $scope.$watch('cart.member_detail_id', function (newValue: number, oldValue) {
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
                        var lbirthday: string[] = $scope.cart.l_birthday.split("/");
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
    .controller('ctrl_mdlight', ['$scope', '$http', 'workService', '$sce', 'ngDialog', '$state',
    function (
        $scope: IOrder,
        $http: ng.IHttpService,
        workService: services.workService,
        $sce: ng.ISCEService,
        ngDialog,
        $state: ng.ui.IStateService
        ) {
        //console.log('ctrl_general');
        var p0: number = 0;

        var allowSetPrice: string[] = ['香油'];
        var allowSetRace: string[] = ['白米'];
        var allowSetGold: string[] = ['金牌'];
        var allowSetGodSon: string[] = ['契子'];

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
                .success(function (data: IResultBase, status, headers, config) {
                if (data.result) {
                    GetNowCartList();
                } else {
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
                .success(function (data: IResultBase, status, headers, config) {
                if (data.result) {
                    GetNowCartList();
                } else {
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
            $scope.cart = <server.cartDetail>{ member_detail_id: -1 };
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
                .success(function (data: IResultData<server.cartDetail>, status, headers, config) {
                if (data.result) {
                    $scope.cart = data.data;
                    $scope.isShowEditProduct = true;
                    $scope.isViewWorking = true;
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        $scope.CloseEditProduct = function () {
            $scope.isShowEditProduct = false;
        };
        $scope.ShowOrderDetail = function () {
            if ($scope.cart.member_detail_id > 0) {
                $http.get(gb_approot + apiGetAction + '/GetOrderDetail', {
                    params: {
                        member_detail_id: $scope.cart.member_detail_id,
                    }
                })
                    .success(function (data: IResultData<server.Orders_Detail[]>, status, headers, config) {
                    if (data.result) {
                        $scope.order_detail = data.data;
                        if (data.data.length <= 0) {
                            alert("去年無購買紀錄!");
                        }
                    } else {
                        alert(data.message);
                    }
                })
                    .error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            } else {
                alert("請選擇好會員後再查詢!!");
            }
        };
        $scope.CalcLunar = function () {
            workService.getCalcLunar($scope.cart.SY, $scope.cart.SM, $scope.cart.SD)
                .success(function (data: server.LuniInfo, status, headers, config) {
                $scope.cart.LY = data.LY;
                $scope.cart.LM = data.M;
                $scope.cart.LD = data.D;
                $scope.cart.isOnLeapMonth = data.IsLeap;
            }).error(function (data) {
                showAjaxError(data);
            })
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

                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
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
                showAjaxError(data);
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
                    ClearCart();//先清楚之前的Sesion
                    $scope.fd = <server.cartMaster>
                    {
                        member_id: member_id,
                        is_light_serial: true, //燈位產品預設採連續排位
                        member_name: data.data.member_name,
                        member_detail_id: data.data.member_detail_id,
                        address: data.data.city + data.data.country + data.data.address,
                        zip: data.data.zip,
                        gender: data.data.gender == '1' ? '男' : '女',
                        tel: data.data.tel,
                        mobile: data.data.mobile
                    };
                    SetMemberToCart(); //將資料回傳Server設定至Session儲存[只有表頭]

                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        function GetProductAll() {
            workService.getProductMDLight()
                .success(function (data: IResultData<server.Product[]>, status, headers, config) {
                if (data.result) {
                    $scope.pds = data.data;
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        function GetLight(product_sn: string) {
            workService.getLightByMD(product_sn)
                .success(function (data: IResultData<server.Light_Site[]>, status, headers, config) {
                if (data.result) {
                    $scope.lights = data.data;
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
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
                showAjaxError(data);
            });;
        }
        function GetNowCartList() {
            $http.get(gb_approot + 'Cart/ListCartItems', { params: {} })
                .success(function (data: IResultData<server.cartMaster>, status, headers, config) {
                if (data.result) {
                    $scope.fd.Item = data.data.Item;
                    $scope.fd.total = data.data.total;
                    $scope.fd.gold = data.data.gold;
                    $scope.fd.race = data.data.race;
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        function SetMemberToCart() {
            $http.post(gb_approot + 'Cart/SetCartMaster', $scope.fd)
                .success(function (data: IResultBase, status, headers, config) {
                if (data.result) {
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        function ClearCart() {
            //清除Session
            $http.get(gb_approot + 'Cart/ClearCart', { params: { t: uniqid() } })
                .success(function (data: IResultBase, status, headers, config) {
                if (!data.result) {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }

        if ($scope.edit_type == IEditType.update) { // 進入為修改模式
            //GetMemberAll($scope.fd.member_id);
        }

        if ($scope.edit_type == IEditType.insert) { //進入為新增模式
            if ($state.params.member_id != undefined) {
                GetMemberByDetail($state.params.member_id);
                GetMemberAll($state.params.member_id);
            }
        }

        $scope.$watch('cart.product_sn', function (newValue: string, oldValue) {
            if (newValue != undefined) {
                for (var i in $scope.pds) {
                    var n = $scope.pds[i];
                    if (n.product_sn == newValue) {
                        $scope.cart.price = n.price;
                        $scope.cart.product_name = n.product_name;
                        $scope.cart.product_sn = n.product_sn;
                        $scope.cart.race = 0;
                        $scope.cart.gold = 0;
                        if (allowSetPrice.indexOf(n.category) >= 0) { //香油類...等產品可設定單價
                            $scope.cart_price_disable = false;
                            $scope.cart_race_disable = true;
                            $scope.cart_gold_disable = true;
                        }
                        else if (allowSetRace.indexOf(n.category) >= 0) {
                            $scope.cart_price_disable = true;
                            $scope.cart_race_disable = false;
                            $scope.cart_gold_disable = true;
                        } else if (allowSetGold.indexOf(n.category) >= 0) {
                            $scope.cart_price_disable = true;
                            $scope.cart_race_disable = true;
                            $scope.cart_gold_disable = false;
                        } else {
                            $scope.cart_price_disable = true;
                            $scope.cart_race_disable = true;
                            $scope.cart_gold_disable = true;
                        }
                    }
                }
            }
        });
        $scope.$watch('cart.member_detail_id', function (newValue: number, oldValue) {
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
                        var lbirthday: string[] = $scope.cart.l_birthday.split("/");
                        if (lbirthday.length == 3) {
                            $scope.cart.LY = parseInt(lbirthday[0]) - 1911;
                            $scope.cart.LM = parseInt(lbirthday[1]);
                            $scope.cart.LD = parseInt(lbirthday[2]);
                        }
                    }
                }
            }
        });
        $scope.$watch('cart.product_sn', function (newValue: string, oldValue) {
            if (newValue != undefined) {
                GetLight(newValue);
            }
        });

        GetProductAll();
    }]);