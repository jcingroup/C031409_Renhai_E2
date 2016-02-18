interface ITempleMarkClose extends IScopeM<server.TempleMember> {
    mb: server.Member;
    openFamilyData(member_id: number): void;
    closeFamileData(): void;
    isShow: boolean;
    member_details: server.Member_Detail[];
    godson_markclose_forsearch: IKeyValueS[];
    upCheck($index: number): void;
    gd: any;
    masterDelete($index): void //主檔單一筆刪除
}

angular
    .module('angularApp', ['commfun', 'siyfion.sfTypeahead'])
    .controller('ctrl', ['$scope', '$http', 'workService', 'gridpage', function (
    $scope: ITempleMarkClose,
    $http: ng.IHttpService,
    workService: services.workService,
    gridpage
    ) {

    $scope.godson_markclose_forsearch = commData.godson_markclose_forsearch;//關閉分類下拉式選單
    $scope.sd = {//預設日期為今天
        is_close: ''
    }
    $scope.JumpPage = function (page: number) {
        $scope.NowPage = page;
        gridpage.CountPage($scope);
    };
    $scope.JumpPageKey = function () {
        gridpage.CountPage($scope);
    };
    $scope.Init_Query = function () {
        gridpage.CountPage($scope);
    };
    $scope.Init_Query();
    $scope.openFamilyData = function (member_id) {
        workService.getMemberDetailAll(member_id)
            .success(function (data: IResultData<server.Member_Detail[]>, status, headers, config) {
            if (data.result) {
                $scope.isShow = true;
                $scope.member_details = data.data;
            } else {
                alert(data.message);
            }
        })
            .error(function (data, status, headers, config) {
            showAjaxError(data);
        });
    };
    $scope.closeFamileData = function () {
        $scope.isShow = false;
        $scope.member_details = [];
    };
    $scope.isShow = false;
    $scope.masterDelete = function ($index) {

        if (confirm('確定刪除?刪除後資料無法復原，需重新加入會員及繳費．')) {

            var get_id = $scope.Grid_Items[$index].temple_member_id;

            $http.delete(gb_approot + 'api/TempleMember', { params: { id: get_id } })
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
    $scope.upCheck = function (index) {

        var m = $scope.Grid_Items[index];
        $http.put(gb_approot + 'api/PutAction/PutTempleMemberCheck',
            { member_id: m.temple_member_id, repeat_mark: m.is_close })
            .success(function (data: IResultBase, status, headers, config) {
            if (!data.result) {
                alert(data.message);
            }
        })
            .error(function (data, status, headers, config) {
            showAjaxError(data);
        });
    }
}]);

