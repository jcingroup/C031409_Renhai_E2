interface IOrders extends IScopeMD<server.Orders, server.Orders_Detail> {
    modalOpenEdit(member_detail_id: number): void;
}
module services {
    export class workOrdersService {
        getOrders: (orders_sn: string) => ng.IHttpPromise<IResultData<server.Orders>>;
        getOrderDetail: (orders_sn: string) => ng.IHttpPromise<server.Orders_Detail[]>;
    }
}

angular.module('angularApp', ['commfun', 'ui.bootstrap'])
    .service('workService', ['$http', function ($http: ng.IHttpService) {
        this.getOrders = function (orders_sn: string) {
            return $http.get(apiConnection, { params: { sn: orders_sn } });
        };

        this.getOrderDetail = function (orders_sn: string) {
            return $http.get(apiDConnection, { params: { orders_sn: orders_sn } });
        };
    }])
    .controller('ctrl', ['$scope', '$http', '$q', '$modal', 'gridpage', 'workService',
        function (
            $scope: IOrders,
            $http: ng.IHttpService,
            $q: ng.IQService,
            $modal: ng.ui.bootstrap.IModalService,
            gridpage,
            workService: services.workOrdersService
            ) {

            var id_name: string = "orders_id";
            $scope.Grid_Items = [];
            $scope.Detail_Items = [];
            $scope.TotalPage = 0;
            $scope.NowPage = 1;
            $scope.RecordCount = 0;
            $scope.StartCount = 0;
            $scope.EndCount = 0;

            $scope.firstpage = 1;
            $scope.lastpage = 0;
            $scope.nextpage = 0;
            $scope.prevpage = 0;

            $scope.show_master_edit = false;
            $scope.edit_type = IEditType.none; //ref 2
            var timer = false; //ref 3

            $scope.grid_new_show = true;
            $scope.grid_del_show = false;
            $scope.grid_nav_show = true;

            $scope.check_del_value = false;

            $scope.JumpPage = function (page: number) {
                $scope.NowPage = page;
                gridpage.CountPage($scope);
            };
            $scope.JumpPageKey = function () {
                gridpage.CountPage($scope);
            };
            $scope.Init_Query = function () {
                gridpage.CountPage($scope); //按下查詢
            };
            $scope.ExpandSub = function ($index) {
                $scope.Grid_Items[$index].expland_sub = !$scope.Grid_Items[$index].expland_sub;
            };

            $scope.Master_Grid_Delete = function () {
                var ids = [];
                for (var key in $scope.Grid_Items) {
                    if ($scope.Grid_Items[key].check_del) {
                        console.info("Select Delete Id:" + $scope.Grid_Items[key][id_name]);
                        ids.push($scope.Grid_Items[key][id_name]);
                    }
                }

                if (ids.length > 0) {
                    if (confirm(msg_Info_Is_SureDelete)) {
                        $http.delete(apiConnection, ids)
                            .success(function (data: IResultBase, status, headers, config) {
                                if (data.result) {
                                    $scope.Init_Query();
                                } else {
                                    alert(data.message);
                                }
                            });
                    }
                } else {
                    alert(msg_Warn_Select_Delete_Data);
                }
            };
            $scope.SelectAllCheckDel = function ($event) {
                for (var key in $scope.Grid_Items) {
                    $scope.Grid_Items[key].check_del = $event.target.checked;
                    console.log($scope.Grid_Items[key].check_del);
                }
            };
            $scope.Master_Submit = function () {

                if ($scope.edit_type == IEditType.insert) {
                    console.info("Insert Mode Start...");
                    $http.post(apiConnection, $scope.fd)
                        .success(function (data: IResultBase, status, headers, config: ng.IRequestConfig) {
                            if (data.result) {
                                $scope.edit_type = IEditType.update;
                                alert(Info_Insert_Success);
                                $scope.Init_Query();
                                console.info("Insert Finish", $scope.edit_type);
                            } else {
                                alert(data.message);
                            }
                        });
                }

                if ($scope.edit_type == IEditType.update) {
                    console.info("Update Mode Start...");
                    $http.put(apiConnection, $scope.fd)
                        .success(function (data: IResultBase, status, headers, config) {
                            if (data.result) {
                                alert(Info_Update_Success);
                                $scope.Init_Query();
                                console.info("Update Finish");
                            } else {
                                alert(data.message);
                            }
                        });
                }
            };
            $scope.Master_Edit_Close = function () {
                $scope.edit_type = IEditType.none;
                $scope.show_master_edit = false;
            };
            $scope.Master_Open_Modify = function ($index) {
                var get_order_sn: string = $scope.Grid_Items[$index].orders_sn;
                workService.getOrders(get_order_sn)
                    .success(function (data, status, headers, config) {
                        if (data.result) {
                            $scope.fd = data.data;
                            $scope.show_master_edit = true;
                            $scope.edit_type = IEditType.update;
                            $scope.Detail_Init();
                        } else {
                            alert(data.message);
                        }
                    });

                workService.getOrderDetail(get_order_sn)
                    .success(function (data, status, headers, config) {
                        $scope.fds = data;
                    });
            };
            $scope.Master_Open_New = function () {
                $scope.fd = <server.Orders>{ member_id: 0 };
                $http.post(aj_MGetId, {})
                    .success(function (data: number, status, headers, config) {
                        $scope.fd.member_id = data;
                        $scope.edit_type = IEditType.insert;
                        $scope.Detail_Items.length = 0;
                        console.info("Get New Id Value:", data);
                    });
                $scope.show_master_edit = true;
            };
            $scope.Detail_Init = function () {

            };

            $http.post(aj_Init, {})
                .success(function (data, status, headers, config) {
                    $scope.InitData = data;
                });

            $scope.Init_Query(); //第一次進入
            $scope.modalOpenEdit = function (member_detail_id) {
                console.log(member_detail_id);
                var modalInstance = $modal.open({
                    templateUrl: '/ScriptsCtrl/tmp/modalMemberEdit.html?tid=' + uniqid(),
                    controller: modalEditMember,
                    size: 'lg',
                    resolve: {
                        items: function () {

                            if (member_detail_id == 0) {
                                return {
                                    id: 0,
                                    member_id: $scope.fd.member_id,
                                    edit_type: IEditType.insert
                                };
                            } else {
                                return {
                                    id: member_detail_id,
                                    edit_type: IEditType.update
                                };
                            };
                        }
                    }
                });

                modalInstance.opened.then(function () {
                }, function () {
                        console.info('Modal dismissed at: ' + new Date());
                    });

                modalInstance.result.then(function (selectedItem) {
                }, function () {
                        console.info('Modal dismissed at: ' + new Date());
                    });
            };
            $scope.$on('refrshData', function () {
                //getMembetDetail($scope.fd.member_id);
            });

        }]);

interface IOrdersDetailEdit extends IScopeM<server.Orders_Detail> {
    ok(): void;
    cancel(): void;
    born_sign: string[];
    born_time: CBornTime[];
    getTW_Adr(val: string): void;
    edit_type: IEditType;
    submit(): void;
}

var modalEditMember = ['$scope', '$http', '$modalInstance', 'items', 'notifyCtrl',
    function ($scope: IOrdersDetailEdit, $http: ng.IHttpService, $modalInstance, items, notifyCtrl) {
        console.log($scope.$parent);
        $scope.edit_type = IEditType.none;
        $scope.born_sign = commData.born_sign;
        $scope.born_time = commData.born_time;

        $scope.getTW_Adr = function (val) {

            return $http.get(gb_approot + 'Api/Addr', {
                params: {                       //回傳值
                    keyword: val
                }
            }).then(function (res: ng.IHttpPromiseCallbackArg<IJSONBase>) {
                    if (res.data.result) {
                        return res.data.json;
                    }
                    else {
                        alert(res.data.message);
                        return [];
                    };
                });
        };
        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

        $scope.submit = function () {
            console.log('submit', $scope.edit_type);
            if ($scope.edit_type == IEditType.insert) {
                console.info("Insert Mode Start...");
                $http.post(gb_approot + 'Api/MberDetail', $scope.fd)
                    .success(function (data: IResultBase, status, headers, config) {
                        if (data.result) {
                            $scope.edit_type = IEditType.update;
                            notifyCtrl.workRefresh();
                            alert('新增完成');
                            //toaster.pop('success', js_Info_Toast_Return_Title, Info_Insert_Success);
                            //$scope.Init_Query();
                            console.info("Insert Finish", $scope.edit_type);
                        } else {
                            alert(data.message);
                            //toaster.pop('error', msg_Err_System_Failure, data.message, 5000);
                        }
                    });
            }

            if ($scope.edit_type == IEditType.update) {
                console.info("Update Mode Start...");
                $http.put(gb_approot + 'Api/MberDetail', $scope.fd)
                    .success(function (data: IResultBase, status, headers, config) {
                        if (data.result) {
                            //toaster.pop('success', js_Info_Toast_Return_Title, Info_Update_Success);
                            //$scope.Init_Query();
                            notifyCtrl.workRefresh();
                            alert('更新完成');
                        } else {
                            alert(data.message);
                            //toaster.pop('error', msg_Err_System_Failure, data.message, 5000);
                        }
                    });
            }
        };

        if (items.edit_type == IEditType.update) {

            $http.get(apiDConnection, { params: { id: items.id } })
                .success(function (data: IResultData<server.Orders_Detail>, status, headers, config) {
                    $scope.fd = data.data;
                    $scope.edit_type = IEditType.update;
                });
        };

        if (items.edit_type == IEditType.insert) {
            $scope.edit_type = IEditType.insert;

            $http.get(gb_allpath + 'ajax_GetDetailNewId')
                .success(function (data: number, status, headers, config) {
                    $scope.fd = <server.Orders_Detail>{
                        orders_detail_id: data,
                        orders_sn: items.order_sn
                    };
                });
        }
    }];