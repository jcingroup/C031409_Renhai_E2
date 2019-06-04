interface IBatchCount extends IScopeM<server.AssemblyBatch> {
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
    DownLoadExcel_BatchRollPrint(): void;//法會名冊
    DownLoadExcel_BatchAllRollPrint(): void;//法會總名冊
    DownLoadExcel_PaperMoneyShuWenPrint(): void;//金紙疏文
    DownLoadExcel_DieWenPrint(): void;//蝶文
    DownLoadExcel_ShuWenPrint(): void;//個別、歷代祖先 疏文
    //download excel

    //超渡法會換梯次功能
    Master_Open_ChgBatch($index: number): void;
}
interface IChgBatch extends IScopeM<server.Orders> {
    orders_sn: string;
    order_data: { main: server.Orders; dtl: server.Orders_Detail }
    bath_list: server.AssemblyBatch[];
    timeperiod_list: IKeyValueS[];
    submitData(): void;
    goGrid(): void;
    allowYear: number;
    upData: { orders_sn?: string; orders_detail_id?: number; up_batch_sn?: number }//更新後資料
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
    })
        .state('chgbatch', {
        url: '/chgbatch?orders_sn&orders_detail_id',
        templateUrl: 'dataChgBatch',
        controller: 'ctrl_edit'
    });
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
            product_sn: null,
            startDate: setDateS(today),
            endDate: setDateS(today)
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
            if ($scope.sd.product_sn == null && userid != '1000001') {
                alert("請選擇「產品種類」後再列印名冊！")
                return;
            }
            var parm = [];
            parm.push('year=' + $scope.sd.year);
            parm.push('batch_sn=' + $scope.sd.assembly_batch_sn);
            parm.push('product_sn=' + $scope.sd.product_sn);
            parm.push('startDate=' + setDateS($scope.sd.startDate));
            parm.push('endDate=' + setDateS($scope.sd.endDate));
            parm.push('tid=' + uniqid());
            var url = gb_approot + 'ExcelReport/BatchRoll?' + parm.join('&');
            $scope.downloadExcel = url;
        }
        $scope.DownLoadExcel_BatchAllRollPrint = function () {
            var parm = [];
            parm.push('year=' + $scope.sd.year);
            parm.push('batch_sn=' + $scope.sd.assembly_batch_sn);
            parm.push('product_sn=' + $scope.sd.product_sn);
            parm.push('startDate=' + setDateS($scope.sd.startDate));
            parm.push('endDate=' + setDateS($scope.sd.endDate));
            parm.push('tid=' + uniqid());
            var url = gb_approot + 'ExcelReport/BatchAllRoll?' + parm.join('&');
            $scope.downloadExcel = url;
        }
        $scope.DownLoadExcel_PaperMoneyShuWenPrint = function () {
            var parm = [];
            parm.push('year=' + $scope.sd.year);
            parm.push('batch_sn=' + $scope.sd.assembly_batch_sn);
            parm.push('product_sn=' + $scope.sd.product_sn);
            parm.push('startDate=' + setDateS($scope.sd.startDate));
            parm.push('endDate=' + setDateS($scope.sd.endDate));
            parm.push('tid=' + uniqid());
            var url = gb_approot + 'ExcelReport/PaperMoneyShuWen?' + parm.join('&');
            $scope.downloadExcel = url;
        }
        $scope.DownLoadExcel_DieWenPrint = function () {
            if ($scope.sd.product_sn == null && userid != '1000001') {
                alert("請選擇「產品種類」後再列印牒文！")
                return;
            }
            var parm = [];
            parm.push('year=' + $scope.sd.year);
            parm.push('batch_sn=' + $scope.sd.assembly_batch_sn);
            parm.push('product_sn=' + $scope.sd.product_sn);
            parm.push('startDate=' + setDateS($scope.sd.startDate));
            parm.push('endDate=' + setDateS($scope.sd.endDate));
            parm.push('tid=' + uniqid());
            var url = gb_approot + 'ExcelReport/DieWen?' + parm.join('&');
            $scope.downloadExcel = url;
        }
        $scope.DownLoadExcel_ShuWenPrint = function () {
            if (!($scope.sd.product_sn == 1401 || $scope.sd.product_sn == 1402)) {
                alert("請選擇正確的「產品種類」後再列印疏文！")
                return;
            }
            var parm = [];
            parm.push('year=' + $scope.sd.year);
            parm.push('batch_sn=' + $scope.sd.assembly_batch_sn);
            parm.push('product_sn=' + $scope.sd.product_sn);
            parm.push('startDate=' + setDateS($scope.sd.startDate));
            parm.push('endDate=' + setDateS($scope.sd.endDate));
            parm.push('tid=' + uniqid());
            var url = gb_approot + 'ExcelReport/ShuWen?' + parm.join('&');
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
        function setDateS(date) {//將日期轉成字串
            if (date instanceof Date) {
                var dateS: String = date.getFullYear() + "/" + (date.getMonth() + 1) + "/" + date.getDate();
                return dateS
            } else {
                return $.extend({}, date);
            }
        }
        //超渡法會 換梯次功能---start---
        $scope.Master_Open_ChgBatch = function ($index) {
            var orders_sn = $scope.Grid_Items[$index]["orders_sn"];
            var orders_detail_id = $scope.Grid_Items[$index]["orders_detail_id"];
            $state.go('chgbatch', { 'orders_sn': orders_sn, 'orders_detail_id': orders_detail_id });
        };
        //超渡法會 換梯次功能---end---
    }]);

var apiItemPath: string = gb_approot + 'api/GetAction/GetBatchItem';
var apiUpPath: string = gb_approot + 'AssemblyBatch/chgBatch';
angular.module('angularApp')
    .controller('ctrl_edit', ['$scope', '$http', 'workService', '$sce', '$state', '$q',
    function (
        $scope: IChgBatch,
        $http: ng.IHttpService,
        workService: services.workService,
        $sce: ng.ISCEService,
        $state: ng.ui.IStateService,
        $q: ng.IQService
        ) {
        $scope.allowYear = allowyear;
        $scope.timeperiod_list = commData.batch_timeperiod;
        $scope.upData = { assembly_batch_sn: null };
        $scope.submitData = function () {
            $http.post(apiUpPath, $scope.upData)
                .success(function (data: IResultBase, status, headers, config) {
                if (data.result) {
                    var orders_sn: string = $state.params.orders_sn;
                    var orders_detail_id: number = $state.params.orders_detail_id;
                    getMasterData(orders_sn, orders_detail_id);//重新整理
                    alert("更新成功!");
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        };
        $scope.goGrid = function () {
            $state.go('grid');
            $scope.Init_Query();
        }
        function getMasterData(orders_sn: string, orders_detail_id: number) {
            $http.get(apiItemPath, { params: { orders_sn: orders_sn, orders_detail_id: orders_detail_id } })
                .success(function (data: IResultData<{ y: number; main: server.Orders; dtl: server.Orders_Detail }>, status, headers, config) {
                if (data.result) {
                    GetBathList(data.data.y);//一年度取得法會梯次
                    $scope.upData.up_batch_sn = data.data.dtl.assembly_batch_sn;
                    $scope.order_data = obj_prop_date(data.data);
                } else {
                    alert(data.message);
                }
            })
                .error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
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
        if ($state.params.orders_sn != undefined && $state.params.orders_detail_id) { // 進入為修改模式
            var orders_sn: string = $state.params.orders_sn;
            var orders_detail_id: number = $state.params.orders_detail_id;
            $scope.orders_sn = orders_sn;
            $scope.edit_type = IEditType.update;
            $scope.upData.orders_sn = orders_sn;
            $scope.upData.orders_detail_id = orders_detail_id;
            getMasterData(orders_sn, orders_detail_id);
        } else {
            //錯誤
        }
    }]);