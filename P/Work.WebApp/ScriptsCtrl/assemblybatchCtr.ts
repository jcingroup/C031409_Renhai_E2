interface IAssemblyBatch extends IScopeM<server.AssemblyBatch> {
    sd: any;
    masterDelete($index): void //主檔單一筆刪除
    year_list: number[];
    timeperiod_list: IKeyValueS[];

    matchOptions(): string;//filter

    copy(): void;
    copySite(): void;

    //日曆小幫手測試
    disabled(date: Date, mode: string): void;
    openStart($event): void;
    openEnd($event): void;

    openedStart: boolean;
    openedEnd: boolean;
    dateOptions: any;
    //日曆小幫手測試
}
interface IAssemblyBatchEdit extends IScopeM<server.AssemblyBatch> {
    submitData(): void;
    goGrid(): void;

    print(): void;
    godson_account_type: IKeyValueS[];
    downloadExcel: string;

}
interface JQuery {
    modal(options?: string): JQuery;
}

angular
    .module('angularApp', ['commfun', 'siyfion.sfTypeahead', 'ngDialog', 'ui.router', 'ui.bootstrap'])
    .config(['$httpProvider', '$stateProvider', '$urlRouterProvider', function ($httpProvider,
    $stateProvider: ng.ui.IStateProvider,
    $urlRouterProvider: ng.ui.IUrlRouterProvider) {
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
        url: '/edit?batch_id',
        templateUrl: 'dataEdit',
        controller: 'ctrl_edit'
    })

}]);
var apiPath: string = gb_approot + 'api/AssemblyBatch';
angular
    .module('angularApp')
    .controller('ctrl', ['$scope', '$http', 'gridpage', 'workService', '$state',
    function (
        $scope: IAssemblyBatch,
        $http: ng.IHttpService,
        gridpage,
        workService: services.workService,
        $state: ng.ui.IStateService
        ) {
        var today: Date = new Date();
        var fday: Date = new Date(today.getFullYear() + "/1/1");
        $scope.year_list = workService.setApplyYearRange();
        $scope.timeperiod_list = commData.batch_timeperiod;
        $scope.sd = {//預設日期為今天
            year: today.getFullYear()
        }




        $scope.show_master_edit = false;
        $scope.edit_type = 0;// IEditType.none; //ref 2
        var timer = false; //ref 3

        $scope.grid_new_show = true;
        $scope.grid_del_show = true;
        $scope.grid_nav_show = true;

        $scope.check_del_value = true;

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
        $scope.ExpandSub = function ($index) {

            //$scope.Grid_Items[$index].expland_sub = !$scope.Grid_Items[$index].expland_sub;
        };

        $scope.Master_Grid_Delete = function () {
            var ids = [];
            for (var key in $scope.Grid_Items) {
                if ($scope.Grid_Items[key].check_del) {
                    ids.push($scope.Grid_Items[key].batch_sn);
                }
            }

            if (ids.length > 0) {
                if (confirm(msg_Info_Is_SureDelete)) {
                    $http['delete'](apiConnection + '?ids=' + ids) //IE8 問題須採用此方式
                        .success(function (data: IResultBase, status, headers, config) {
                        if (data.result) {
                            $scope.Init_Query();
                        } else {
                            workService.showToaster(emToasterType.error, msg_Err_System_Failure, data.message);
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

            }
        };
        $scope.Master_Submit = function () {
            if ($scope.edit_type == IEditType.insert) {
                $http.post(apiConnection, $scope.fd)
                    .success(function (data: IResultBase, status, headers, config) {
                    if (data.result) {
                        $scope.edit_type = IEditType.update;
                        workService.showToaster(emToasterType.success, js_Info_Toast_Return_Title, Info_Update_Success);
                        $scope.Init_Query();
                    } else {
                        workService.showToaster(emToasterType.error, msg_Err_System_Failure, data.message);
                    }
                });
            }

            if ($scope.edit_type == IEditType.update) {
                $http.put(apiConnection, $scope.fd)
                    .success(function (data: IResultBase, status, headers, config) {
                    if (data.result) {
                        workService.showToaster(emToasterType.success, js_Info_Toast_Return_Title, Info_Update_Success);
                        //$('#Edit').modal('hide');
                        $scope.Init_Query();
                    } else {
                        workService.showToaster(emToasterType.error, msg_Err_System_Failure, data.message);
                    }
                });
            }
        };
        $scope.Master_Edit_Close = function () {
            $scope.edit_type = IEditType.none;
            $scope.show_master_edit = false;
            $scope.fd = <server.AssemblyBatch>{};
        };
        $scope.Master_Open_Modify = function ($index) {
            var get_id = $scope.Grid_Items[$index].batch_sn;
            $state.go('edit', { 'batch_id': get_id });
        };
        $scope.Master_Open_New = function () {
            $state.go('edit', {});
        };
        $scope.masterDelete = function ($index) {

            if (confirm('確定刪除?')) {

                var get_id = $scope.Grid_Items[$index].batch_sn;

                $http.delete(apiPath, { params: { id: get_id } })
                    .success(function (data: IResultBase, status, headers, config) {
                    if (data.result) {
                        $scope.Init_Query();
                        alert('刪除完成');
                    } else {
                        alert(data.message);
                    }
                });
            }
            return;
        };

        $scope.Detail_Init = function () {

        };

        $http.post(aj_Init, {})
            .success(function (data, status, headers, config) {
            $scope.InitData = data;
        });

        $scope.Init_Query(); //第一次進入

        $scope.copy = function () {
            $http.get(gb_approot + 'AssemblyBatch/CopyLastYear')
                .success(function (data: IResultBase, status, headers, config) {
                if (data.result) {
                    alert('複製完成');
                    $scope.Init_Query();
                } else {
                    alert(data.message);
                }
            });
        };
        $scope.copySite = function () {
            $http.get(gb_approot + 'AssemblyBatch/addPlace')
                .success(function (data: IResultBase, status, headers, config) {
                if (data.result) {
                    alert('新增燈位');
                    $scope.Init_Query();
                } else {
                    alert(data.message);
                }
            });
        };


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
    }]);

angular.module('angularApp')
    .controller('ctrl_edit', ['$scope', '$http', 'workService', '$sce', '$state', '$q',
    function (
        $scope: IAssemblyBatchEdit,
        $http: ng.IHttpService,
        workService: services.workService,
        $sce: ng.ISCEService,
        $state: ng.ui.IStateService,
        $q: ng.IQService
        ) {

        $scope.submitData = function () {
            var submitCheck: boolean = false;
            var exMassage: string = '';
            if (checkDate($scope.fd.lunar_y + '/' + $scope.fd.lunar_m + '/' + $scope.fd.lunar_d)) {
                submitCheck = true;
            } else {
                exMassage = "[法會時間(農曆)]格式錯誤!";
            }


            if (submitCheck) {
                var md = jQuery.extend({}, $scope.fd);
                md.batch_date = setDateS(md.batch_date);

                if ($scope.edit_type == IEditType.insert) {
                    $http.post(apiPath, md)
                        .success(function (data: IResultBase, status, headers, config) {
                        if (data.result) {
                            $scope.fd.batch_sn = data.id;
                            $scope.edit_type = IEditType.update;
                            alert('增新完成');
                            $scope.goGrid();
                        } else {
                            alert(data.message);
                        }
                    })
                        .error(function (data, status, headers, config) {
                        showAjaxError(data);
                    });
                }
                if ($scope.edit_type == IEditType.update) {

                    $http.put(apiPath, md)
                        .success(function (data: IResultBase, status, headers, config) {
                        if (data.result) {
                            alert('更新完成');
                            $scope.goGrid();
                        } else {
                            alert(data.message);
                        }
                    })
                        .error(function (data, status, headers, config) {
                        showAjaxError(data);
                    });
                }
            } else {
                alert(exMassage);
            }
        };
        $scope.goGrid = function () {
            $state.go('grid');
            $scope.Init_Query();
        }
        $scope.print = function () {
            //var url = gb_approot + 'ExcelReport/downloadExcel_GodSON?temple_account_id=' + $scope.fds.temple_account_id + '&t=' + uniqid();
            //$scope.downloadExcel = $sce.getTrustedResourceUrl(url);
        }
        function getMasterData(id: number) {
            $http.get(apiPath, { params: { id: id } })
                .success(function (data: IResultData<server.AssemblyBatch>, status, headers, config) {
                if (data.result) {
                    $scope.fd = obj_prop_date(data.data);
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }

        if ($state.params.batch_id != undefined) { // 進入為修改模式
            var get_id: number = $state.params.batch_id;
            $scope.edit_type = IEditType.update;
            getMasterData(get_id);
        } else { //進入為新增模式
            $scope.edit_type = IEditType.insert;
            $scope.fd = <server.AssemblyBatch>{ batch_qty: 500, batch_timeperiod: 'All' };
        }
    }]);

function setDateS(date) {
    if (date instanceof Date) {
        var dateS: String = date.getFullYear() + "/" + (date.getMonth() + 1) + "/" + date.getDate();
        return dateS
    } else {
        return date;
    }
}
function checkDate(date) {//檢查日期
    if (date != null && date != "") {
        var day = date.split('/');
        var month: number[] = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

        if (checkLeapYear(parseInt(day[0]) + 1911)) {//如果閏年2月變29天
            month[1] = 29;
        }

        if (day[1] > 12 || day[1] <= 0) {//
            return false;
        } else if (day[2] > month[(day[1] - 1)] || day[2] <= 0) {
            return false;
        } else {
            return true
        }
    } else {
        return true;
    }

}

function checkLeapYear(Year) {//檢查閏年true:是閏年
    if (Year % 4 == 0) {
        if (Year % 100 == 0) {
            if (Year % 400 == 0) {
                return true;//可以被100整除又可以被400整除
            } else {
                return false;//可以被100整除但無法被400整除
            }
        } else {
            return true;//被4整除但無法被100整除
        }
    } else {//無法被4整除
        return false;
    }
}