interface IAssemblyBatch extends IScopeM<server.AssemblyBatch> {
    sd: any;
    masterDelete($index): void //主檔單一筆刪除
    year_list: number[];
    timeperiod_list: IKeyValueS[];

    matchOptions(): string;//filter

    copy(): void;
    copySite($index): void;

    //日曆小幫手測試
    disabled(date: Date, mode: string): void;
    openStart($event): void;
    openEnd($event): void;

    openedStart: boolean;
    openedEnd: boolean;
    dateOptions: any;
    //日曆小幫手測試
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
        $scope.copySite = function ($index) {
            var get_id = $scope.Grid_Items[$index].batch_sn;
            $http.get(gb_approot + 'AssemblyBatch/addPlace', { params: { batch_sn: get_id } })
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



