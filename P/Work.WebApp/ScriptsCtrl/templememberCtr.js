angular.module('angularApp', ['commfun', 'siyfion.sfTypeahead', 'ngDialog', 'ui.router']).config(['$httpProvider', '$stateProvider', '$urlRouterProvider', function ($httpProvider, $stateProvider, $urlRouterProvider) {
    if (!$httpProvider.defaults.headers.get) {
        $httpProvider.defaults.headers.get = {};
    }
    $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    $httpProvider.defaults.headers.get['If-Modified-Since'] = '0';
    $urlRouterProvider.otherwise("/grid");
    $stateProvider.state('grid', {
        url: '/grid',
        templateUrl: 'dataGrid'
    }).state('edit', {
        url: '/edit?temple_member_id',
        templateUrl: 'dataEdit',
        controller: 'ctrl_edit'
    });
}]);
angular.module('angularApp').controller('ctrl', ['$scope', '$http', 'gridpage', 'workService', '$state', function ($scope, $http, gridpage, workService, $state) {
    $scope.sd = {};
    $scope.show_master_edit = false;
    $scope.edit_type = 0;
    var timer = false;
    $scope.grid_new_show = true;
    $scope.grid_del_show = true;
    $scope.grid_nav_show = true;
    $scope.check_del_value = true;
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
    $scope.ExpandSub = function ($index) {
    };
    $scope.Master_Grid_Delete = function () {
        var ids = [];
        for (var key in $scope.Grid_Items) {
            if ($scope.Grid_Items[key].check_del) {
                ids.push($scope.Grid_Items[key].temple_member_id);
            }
        }
        if (ids.length > 0) {
            if (confirm(msg_Info_Is_SureDelete)) {
                $http['delete'](apiConnection + '?ids=' + ids).success(function (data, status, headers, config) {
                    if (data.result) {
                        $scope.Init_Query();
                    }
                    else {
                        workService.showToaster(1 /* error */, msg_Err_System_Failure, data.message);
                    }
                });
            }
        }
        else {
            alert(msg_Warn_Select_Delete_Data);
        }
    };
    $scope.SelectAllCheckDel = function ($event) {
        for (var key in $scope.Grid_Items) {
            $scope.Grid_Items[key].check_del = $event.target.checked;
        }
    };
    $scope.Master_Submit = function () {
        if ($scope.edit_type == 1 /* insert */) {
            $http.post(apiConnection, $scope.fd).success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.edit_type = 2 /* update */;
                    workService.showToaster(0 /* success */, js_Info_Toast_Return_Title, Info_Update_Success);
                    $scope.Init_Query();
                }
                else {
                    workService.showToaster(1 /* error */, msg_Err_System_Failure, data.message);
                }
            });
        }
        if ($scope.edit_type == 2 /* update */) {
            $http.put(apiConnection, $scope.fd).success(function (data, status, headers, config) {
                if (data.result) {
                    workService.showToaster(0 /* success */, js_Info_Toast_Return_Title, Info_Update_Success);
                    $scope.Init_Query();
                }
                else {
                    workService.showToaster(1 /* error */, msg_Err_System_Failure, data.message);
                }
            });
        }
    };
    $scope.Master_Edit_Close = function () {
        $scope.edit_type = 0 /* none */;
        $scope.show_master_edit = false;
        $scope.fd = {};
    };
    $scope.Master_Open_Modify = function ($index) {
        var get_id = $scope.Grid_Items[$index].temple_member_id;
        $state.go('edit', { 'temple_member_id': get_id });
    };
    $scope.Master_Open_New = function () {
        $state.go('edit', {});
    };
    $scope.masterDelete = function ($index) {
        if (confirm('確定刪除?刪除後資料無法復原，需重新加入會員及繳費．')) {
            var get_id = $scope.Grid_Items[$index].temple_member_id;
            $http.delete(gb_approot + 'api/TempleMember', { params: { id: get_id } }).success(function (data, status, headers, config) {
                if (data.result) {
                    $scope.Init_Query();
                    alert('刪除完成');
                }
                else {
                    alert(data.message);
                }
            });
        }
        return;
    };
    $scope.Detail_Init = function () {
    };
    $http.post(aj_Init, {}).success(function (data, status, headers, config) {
        $scope.InitData = data;
    });
    $scope.Init_Query();
}]);
angular.module('angularApp').controller('ctrl_edit', ['$scope', '$http', 'workService', '$sce', '$state', '$q', function ($scope, $http, workService, $sce, $state, $q) {
    $scope.godson_account_type = commData.godson_account_type;
    $scope.closeEdit = function () {
        $scope.fd = null;
    };
    $scope.submitData = function () {
        var submitCheck = false;
        var exMassage = '';
        if (checkTwID($scope.fd.sno)) {
            submitCheck = true;
        }
        else {
            exMassage = "身分證字號格式錯誤!";
        }
        if (submitCheck && !checkDate($scope.fd.birthday)) {
            exMassage = "生日日期格式錯誤!";
            submitCheck = false;
        }
        if (submitCheck) {
            if ($scope.edit_type == 1 /* insert */) {
                $http.post(gb_approot + 'api/TempleMember', $scope.fd).success(function (data, status, headers, config) {
                    if (data.result) {
                        $scope.fd.temple_member_id = data.id;
                        getDetailListData($scope.fd.temple_member_id);
                        $scope.edit_type = 2 /* update */;
                        alert('增新完成');
                    }
                    else {
                        alert(data.message);
                    }
                }).error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            }
            if ($scope.edit_type == 2 /* update */) {
                $http.put(gb_approot + 'api/TempleMember', $scope.fd).success(function (data, status, headers, config) {
                    if (data.result) {
                        getDetailListData($scope.fd.temple_member_id);
                        alert('更新完成');
                    }
                    else {
                        alert(data.message);
                    }
                }).error(function (data, status, headers, config) {
                    showAjaxError(data);
                });
            }
        }
        else {
            alert(exMassage);
        }
    };
    $scope.goGrid = function () {
        $state.go('grid');
        $scope.Init_Query();
    };
    $scope.closeAcct = function () {
        $scope.is_show_acct = false;
        $scope.fds = {};
        $scope.detail_edit_type = 0 /* none */;
    };
    $scope.submitAcct = function () {
        if ($scope.detail_edit_type == 1 /* insert */) {
            $http.post(gb_approot + 'api/TempleAccount', $scope.fds).success(function (data, status, headers, config) {
                if (data.result) {
                    getMasterData($scope.fds.temple_member_id);
                    getDetailListData($scope.fds.temple_member_id);
                    $scope.fds.temple_account_id = data.id;
                    alert('增新完成');
                }
                else {
                    alert(data.message);
                }
            }).error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
        if ($scope.detail_edit_type == 2 /* update */) {
            $http.put(gb_approot + 'api/TempleAccount', $scope.fds).success(function (data, status, headers, config) {
                if (data.result) {
                    getDetailListData($scope.fds.temple_member_id);
                    alert('更新完成');
                }
                else {
                    alert(data.message);
                }
            }).error(function (data, status, headers, config) {
                showAjaxError(data);
            });
        }
    };
    $scope.openEditDetail = function (id) {
        $http.get(gb_approot + 'api/TempleAccount', { params: { id: id } }).success(function (data, status, headers, config) {
            if (data.result) {
                $scope.fds = obj_prop_date(data.data);
                $scope.is_show_acct = true;
                $scope.detail_edit_type = 2 /* update */;
            }
            else {
                alert(data.message);
            }
        }).error(function (data, status, headers, config) {
            showAjaxError(data);
        });
    };
    $scope.openNewDetail = function (master_id) {
        $scope.fds = {
            temple_member_id: master_id,
            tran_date: new Date()
        };
        $scope.is_show_acct = true;
        $scope.detail_edit_type = 1 /* insert */;
    };
    $scope.deleteDetail = function (id) {
        if (confirm('確定刪除訂單?')) {
            $http.delete(gb_approot + 'api/TempleAccount', { params: { ids: id } }).success(function (data, status, headers, config) {
                if (data.result) {
                    getDetailListData($state.params.temple_member_id);
                }
                else {
                    alert(data.message);
                }
            });
        }
        else {
            return;
        }
    };
    $scope.print = function () {
        var url = gb_approot + 'ExcelReport/downloadExcel_GodSON?temple_account_id=' + $scope.fds.temple_account_id + '&t=' + uniqid();
        $scope.downloadExcel = $sce.getTrustedResourceUrl(url);
    };
    $scope.$watch('fds.product_sn', function (newValue, oldValue) {
        if (newValue != undefined) {
            for (var i in $scope.pds) {
                var n = $scope.pds[i];
                if (n.product_sn == newValue) {
                    $scope.fds.price = n.price;
                }
            }
        }
    });
    function getMasterData(id) {
        $http.get(gb_approot + 'api/TempleMember', { params: { id: id } }).success(function (data, status, headers, config) {
            if (data.result) {
                $scope.fd = obj_prop_date(data.data);
            }
            else {
                alert(data.message);
            }
        }).error(function (data, status, headers, config) {
            showAjaxError(data);
        });
    }
    function getDetailListData(id) {
        $http.get(gb_approot + 'api/TempleAccount', { params: { temple_member_id: id } }).success(function (data, status, headers, config) {
            $scope.gds = data.rows;
        }).error(function (data, status, headers, config) {
            showAjaxError(data);
        });
    }
    function GetProductTempMember() {
        workService.getProductTempleMember().success(function (data, status, headers, config) {
            if (data.result) {
                $scope.pds = data.data;
            }
            else {
                alert(data.message);
            }
        }).error(function (data, status, headers, config) {
            showAjaxError(data);
        });
    }
    ;
    function checkTwID(id) {
        if (id != null && id != "") {
            var city = new Array(1, 10, 19, 28, 37, 46, 55, 64, 39, 73, 82, 2, 11, 20, 48, 29, 38, 47, 56, 65, 74, 83, 21, 3, 12, 30);
            id = id.toUpperCase();
            if (id.search(/^[A-Z](1|2)\d{8}$/i) == -1) {
                return false;
            }
            else {
                id = id.split('');
                var total = city[id[0].charCodeAt(0) - 65];
                for (var i = 1; i <= 8; i++) {
                    total += eval(id[i]) * (9 - i);
                }
                total += eval(id[9]);
                return ((total % 10 == 0));
            }
        }
        else {
            return true;
        }
    }
    function checkDate(date) {
        if (date != null && date != "") {
            var day = date.split('/');
            var month = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
            if (checkLeapYear(parseInt(day[0]) + 1911)) {
                month[1] = 29;
            }
            if (day[1] > 12 || day[1] <= 0) {
                return false;
            }
            else if (day[2] > month[(day[1] - 1)] || day[2] <= 0) {
                return false;
            }
            else {
                return true;
            }
        }
        else {
            return true;
        }
    }
    function checkLeapYear(Year) {
        if (Year % 4 == 0) {
            if (Year % 100 == 0) {
                if (Year % 400 == 0) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return true;
            }
        }
        else {
            return false;
        }
    }
    GetProductTempMember();
    if ($state.params.temple_member_id != undefined) {
        var get_id = $state.params.temple_member_id;
        $scope.edit_type = 2 /* update */;
        getMasterData(get_id);
        getDetailListData(get_id);
    }
    else {
        $scope.edit_type = 1 /* insert */;
        $scope.fd = {
            zip: '320',
            join_datetime: new Date()
        };
    }
}]);
