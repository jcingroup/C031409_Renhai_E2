interface IBatchChg extends IScopeM<server.AssemblyBatchChglog> {
    sd: any;
    year_list: number[]
    bath_list: server.AssemblyBatch[];
    timeperiod_list: IKeyValueS[];
    batch_prod: IKeyValueS[];
    batch_slist: IKeyValue[];

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
    DownLoadExcel_BatchChgPrint(): void;//梯次替換紀錄清單
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
        templateUrl: 'dataGrid',
        controller: 'ctrl'
    });
}]);
var apiPath: string = gb_approot + 'api/GetAction/GetBatchChgList';
angular
    .module('angularApp')
    .controller('ctrl', ['$scope', '$http', 'gridpage', 'workService', '$state',
    function (
        $scope: IBatchChg,
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
        $scope.batch_slist = [];

        $scope.sd = {//預設日期為今天
            year: allowyear,
            product_sn: null,
            startDate: setDateS(today),
            endDate: setDateS(today),
            orders_sn: null
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

        function GetBathList(year) {
            workService.getQueryBatchList(year)
                .success(function (data: IResultData<server.AssemblyBatch[]>, status, headers, config) {
                if (data.result) {
                    $scope.bath_list = data.data;
                    $scope.batch_slist = data.data.map(x=> <IKeyValue>{ value: x.batch_sn, label: x.batch_title });
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

        $scope.DownLoadExcel_BatchChgPrint = function () {
            var parm = [];
            parm.push('year=' + $scope.sd.year);
            parm.push('product_sn=' + $scope.sd.product_sn);
            parm.push('startDate=' + setDateS($scope.sd.startDate));
            parm.push('endDate=' + setDateS($scope.sd.endDate));
            parm.push('orders_sn=' + $scope.sd.orders_sn);
            parm.push('tid=' + uniqid());
            var url = gb_approot + 'ExcelReport/BatchChg?' + parm.join('&');
            $scope.downloadExcel = url;
        }
        //日曆小幫手---start---
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
        function setDateS(date): string {//將日期轉成字串
            if (date instanceof Date) {
                var dateS: string = date.getFullYear() + "/" + (date.getMonth() + 1) + "/" + date.getDate();
                return dateS;
            } else {
                return date;
            }
        }
    }]);
