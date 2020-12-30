angular
    .module('angularApp', ['commfun', 'siyfion.sfTypeahead'])
    .config(['$httpProvider', function ($httpProvider) {
        if (!$httpProvider.defaults.headers.get) {
            $httpProvider.defaults.headers.get = {};
        }
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
        $httpProvider.defaults.headers.get['If-Modified-Since'] = '0';
    }])
    .controller('ctrl', ['$scope', '$http', 'workService', 'gridpage', '$sce', function ($scope, $http, workService, gridpage, $sce) {
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
        $scope.SubmitOrder = function () {
            if ($scope.edit_type == IEditType.insert) {
                $http.post(gb_approot + 'Orders/AddOrders', $scope.fd)
                    .success(function (data, status, headers, config) {
                    if (data.result) {
                        $http.get(gb_approot + 'Cart/OrdersToSession', { params: { orders_sn: data.data } })
                            .success(function (data, status, headers, config) {
                            if (data.result) {
                                $scope.fd = data.data;
                                $scope.edit_type = IEditType.update;
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
            if ($scope.edit_type == IEditType.update) {
                $http.put(gb_approot + 'Orders/UpdateOrders', $scope.fd)
                    .success(function (data, status, headers, config) {
                    if (data.result) {
                        $scope.edit_type = IEditType.update;
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
        };
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
            $http.delete(gb_approot + 'Cart/RemoveCart', {
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
        $scope.DownLoadExcel_Thanks = function () {
            var url = gb_approot + 'ExcelReport/downloadExcel_Thanks?orders_sn=' + $scope.fd.orders_sn + '&t=' + uniqid();
            $scope.downloadExcel = $sce.getTrustedResourceUrl(url);
        };
        function GetOrder(order_sn) {
            workService.getOrderData(order_sn)
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
            $http.delete(gb_approot + 'Cart/ClearCart', { params: { t: uniqid() } })
                .success(function (data, status, headers, config) {
                if (!data.result) {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
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
        $scope.Master_Open_Modify = function (orders_sn) {
            $http.get(gb_approot + 'Orders/CheckOrderType', { params: { orders_sn: orders_sn } })
                .success(function (data, status, headers, config) {
                if (data.message != '') {
                    document.location.href = data.message;
                }
                else {
                    $http.delete(gb_approot + 'Cart/ClearCart', { params: {} })
                        .success(function (data, status, headers, config) {
                        if (data.result) {
                            $http.get(gb_approot + 'Orders/GetToSession', { params: { orders_sn: orders_sn } })
                                .success(function (data, status, headers, config) {
                                if (data.result) {
                                    $scope.fd = data.data;
                                    $scope.fd.is_light_serial = true;
                                    $scope.edit_type = IEditType.update;
                                    GetMemberAll(data.data.member_id);
                                    $scope.isShowEdit = true;
                                }
                                else {
                                    alert(data.message);
                                }
                            }).error(function (data) {
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
            });
        };
        if (p0 > 0) {
            GetOrder(p0);
        }
        else {
            if (member_id > 0) {
                $scope.edit_type = IEditType.insert;
                GetMemberByDetail(member_id);
                GetMemberAll(member_id);
                $scope.isShowEdit = true;
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
        GetUsers();
        GetFortune();
        $scope.Init_Query();
    }]);
