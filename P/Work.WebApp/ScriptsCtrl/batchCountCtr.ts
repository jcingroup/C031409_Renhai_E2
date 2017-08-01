﻿interface IBatchCount extends IScopeM<server.AssemblyBatch> {
    sd: any;
    masterDelete($index): void //主檔單一筆刪除
    year_list: number[]
    bath_list: server.AssemblyBatch[];
    timeperiod_list: IKeyValueS[];
    batch_prod: IKeyValueS[];

    //日曆小幫手測試
    disabled(date: Date, mode: string): void;
    openStart($event): void;
    openEnd($event): void;

    openedStart: boolean;
    openedEnd: boolean;
    dateOptions: any;
    //日曆小幫手測試

    //download excel
    downloadExcel: string;
    DownLoadExcel_BatchRollPrint(): void;
    //download excel
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
    //    .state('edit', {
    //    url: '/edit?batch_id',
    //    templateUrl: 'dataEdit',
    //    controller: 'ctrl_edit'
    //})
}]);
var apiPath: string = gb_approot + 'api/GetAction/GetAssemblyList';
angular
    .module('angularApp')
    .controller('ctrl', ['$scope', '$http', 'gridpage', 'workService', '$state',
    function (
        $scope: IBatchCount,
        $http: ng.IHttpService,
        gridpage,
        workService: services.workService,
        $state: ng.ui.IStateService
        ) {
        $scope.apiPath = apiPath;
        var today: Date = new Date();
        var fday: Date = new Date(today.getFullYear() + "/1/1");
        $scope.year_list = workService.setApplyYearRange();
        $scope.timeperiod_list = commData.batch_timeperiod;
        $scope.batch_prod = commData.batch_prod;

        $scope.sd = {//預設日期為今天
            year: today.getFullYear(),
            product_sn: null
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

        //$http.post(aj_Init, {})
        //    .success(function (data, status, headers, config) {
        //    $scope.InitData = data;
        //});
        function GetBathList(year) {
            workService.getQueryBatchList(year)
                .success(function (data: IResultData<server.AssemblyBatch[]>, status, headers, config) {
                if (data.result) {
                    $scope.bath_list = data.data;
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                alert('ajax error' + data);
            });
        };

        $scope.Init_Query(); //第一次進入
        GetBathList($scope.sd.year);

        $scope.$watch('sd.year', function (newValue: string, oldValue) {
            if (newValue != undefined && newValue != oldValue) {
                GetBathList(newValue);
            }
        });

        $scope.DownLoadExcel_BatchRollPrint = function () {
            if ($scope.sd.product_sn == null) {
                alert("請選擇「產品種類」後再列印名冊！")
                return;
            }
            var parm = [];
            parm.push('year=' + $scope.sd.year);
            parm.push('batch_sn=' + $scope.sd.assembly_batch_sn);
            parm.push('product_sn=' + $scope.sd.product_sn);
            parm.push('tid=' + uniqid());
            var url = gb_approot + 'ExcelReport/BatchRoll?' + parm.join('&');
            $scope.downloadExcel = url;
        }
    }]);


