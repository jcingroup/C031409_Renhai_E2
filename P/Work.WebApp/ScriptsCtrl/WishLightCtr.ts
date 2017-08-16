interface IWishLight extends IScopeM<server.AssemblyBatch> {
    sd: any;
    masterDelete($index): void //主檔單一筆刪除
    year_list: number[]

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
    DownLoadExcel_WishLight(): void;//訂單芳名錄
    //download excel

    //新增今年度燈位
    AddSite($index): void;
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
    //    .state('edit', {
    //    url: '/edit?batch_id',
    //    templateUrl: 'dataEdit',
    //    controller: 'ctrl_edit'
    //})
}]);
var apiPath: string = gb_approot + 'api/GetAction/GetWishOrderList';
angular
    .module('angularApp')
    .controller('ctrl', ['$scope', '$http', 'gridpage', 'workService', '$state',
    function (
        $scope: IWishLight,
        $http: ng.IHttpService,
        gridpage,
        workService: services.workService,
        $state: ng.ui.IStateService
        ) {
        $scope.apiPath = apiPath;
        var today: Date = new Date();
        $scope.year_list = workService.setApplyYearRange();

        $scope.sd = {//預設日期為今天
            year: today.getFullYear(),
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

        $scope.Init_Query(); //第一次進入


        $scope.DownLoadExcel_WishLight = function () {
            if ($scope.sd.year == null || $scope.sd.year==undefined) {
                alert("請選擇「年度」後再列印名冊！")
                return;
            }
            var parm = [];
            parm.push('year=' + $scope.sd.year);
            parm.push('startDate=' + setDateS($scope.sd.startDate));
            parm.push('endDate=' + setDateS($scope.sd.endDate));
            parm.push('tid=' + uniqid());
            var url = gb_approot + 'ExcelReport/WishRoll?' + parm.join('&');
            $scope.downloadExcel = url;
        }
        $scope.AddSite = function (row) {
            console.log(row);
            $http.get(gb_approot + 'WishLight/addPlace', { params: { row: row } })
                .success(function (data: IResultBase, status, headers, config) {
                if (data.result) {
                    alert('新增燈位第' + row + '排燈位');
                    //$scope.Init_Query();
                } else {
                    alert(data.message);
                }
            });
        };

        function setDateS(date) {//將日期轉成字串
            if (date instanceof Date) {
                var dateS: String = date.getFullYear() + "/" + (date.getMonth() + 1) + "/" + date.getDate();
                return dateS
            } else {
                return $.extend({}, date);
            }
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
    }]);


