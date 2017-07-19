function uniqid() {
    var newDate = new Date();
    return newDate.getTime();
}
function obj_prop_list(obj) {
    for (var prop in obj) {
        if (obj.hasOwnProperty(prop)) {
            console.log(prop + " :" + obj[prop]);
        }
    }
}
function stand_date(getDateStr) {
    var d = new Date(getDateStr);
    var r = d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate;
    return r;
}
function month_first_day() {
    var d = new Date();
    var r = new Date(d.getFullYear(), d.getMonth(), 1);
    console.log(r);
    return r;
}
function month_last_day() {
    var d = new Date();
    var r = new Date(d.getFullYear(), d.getMonth() + 1, 0);
    console.log(r);
    return r;
}
function tim() {
    var d = new Date();
    return d.toUTCString() + '.' + d.getMilliseconds().toString();
}
function pad(str, len, pad, dir) {
    var padlen;
    if (typeof (len) == "undefined") {
        var len = 0;
    }
    if (typeof (pad) == "undefined") {
        var pad = ' ';
    }
    if (typeof (dir) == "undefined") {
        var dir = 3;
    }
    if (len + 1 >= str.length) {
        switch (dir) {
            case 1:
                str = Array(len + 1 - str.length).join(pad) + str;
                break;
            case 2:
                str = str + Array(len + 1 - str.length).join(pad);
                break;
            case 3:
                var right = Math.ceil((padlen = len - str.length) / 2);
                var left = padlen - right;
                str = Array(left + 1).join(pad) + str + Array(right + 1).join(pad);
                break;
        }
    }
    return str;
}
function showAjaxError(data) {
    alert('Ajax error,check console info!');
    console.log(data);
}
function isValidJSONDate(value, userFormat) {
    var userFormat = userFormat || 'yyyy-mm-dd';
    var delimiter = /[^mdy]/.exec(userFormat)[0];
    var theFormat = userFormat.split(delimiter);
    var splitDatePart = value.split('T');
    if (splitDatePart.length == 1)
        return false;
    var theDate = splitDatePart[0].split(delimiter);
    var isDate = function (date, format) {
        var m, d, y;
        for (var i = 0, len = format.length; i < len; i++) {
            if (/m/.test(format[i]))
                m = date[i];
            if (/d/.test(format[i]))
                d = date[i];
            if (/y/.test(format[i]))
                y = date[i];
        }
        ;
        return (m > 0 && m < 13 && y && y.length === 4 && d > 0 && d <= (new Date(y, m, 0)).getDate());
    };
    return isDate(theDate, theFormat);
}
function obj_prop_date(obj) {
    for (var prop in obj) {
        if (obj.hasOwnProperty(prop)) {
            var getUTCStr = obj[prop];
            if (typeof getUTCStr == 'string') {
                var isValid = isValidJSONDate(getUTCStr, null);
                if (isValid) {
                    obj[prop] = new Date(getUTCStr);
                }
            }
        }
    }
    return obj;
}
var services;
(function (services) {
    var workService = (function () {
        function workService() {
        }
        return workService;
    })();
    services.workService = workService;
})(services || (services = {}));
;
angular.module('commfun', []).service('workService', ['$http', function ($http) {
    this.getOrderData = function (order_sn) {
        return $http.get(gb_approot + 'Orders/GetOrders', { params: { order_sn: order_sn } });
    };
    this.getMemberDetailAll = function (member_id) {
        return $http.get(gb_approot + 'api/Member_Detail', { params: { member_id: member_id } });
    };
    this.getMemberAll = function (member_id) {
        return $http.get(gb_approot + 'Member/getMemberAll', { params: { member_id: member_id } });
    };
    this.getMember = function (member_id) {
        return $http.get(gb_approot + 'Member/GetMember', { params: { member_id: member_id } });
    };
    this.getMemberDetail = function (member_detail_id) {
        return $http.get(gb_approot + 'Member/GetMemberDetail', { params: { member_detail_id: member_detail_id } });
    };
    this.getMemberByDetail = function (member_id) {
        return $http.get(gb_approot + 'Member/GetMemberByDetail', { params: { member_id: member_id } });
    };
    this.getProductAll = function () {
        return $http.get(gb_approot + apiGetAction + '/GetProductAll', { params: {} });
    };
    this.getProductSDLight = function () {
        return $http.get(gb_approot + apiGetAction + '/GetProductSDLight', { params: {} });
    };
    this.getProductMDLight = function () {
        return $http.get(gb_approot + apiGetAction + '/GetProductMDLight', { params: {} });
    };
    this.getLightByMD = function (product_sn) {
        return $http.get(gb_approot + apiGetAction + '/GetLightByMD', { params: { product_sn: product_sn } });
    };
    this.getProductFortune = function () {
        return $http.get(gb_approot + apiGetAction + '/GetProductFortune', { params: {} });
    };
    this.getManjushri = function () {
        return $http.get(gb_approot + 'Manjushri/GetManjushri', { params: { t: uniqid() } });
    };
    this.getAssemblyBatch = function (year) {
        return $http.get(gb_approot + 'AssemblyBatch/GetAssemblyBatch', { params: { year: year, t: uniqid() } });
    };
    this.getQueryBatchList = function (year) {
        return $http.get(gb_approot + apiGetAction + '/GetBatchList', { params: { year: year, t: uniqid() } });
    };
    this.getCalcLunar = function (Y, M, D) {
        var setDate = new Date(Y + 1911, M, D);
        return $http.get(gb_approot + apiGetAction + '/GetLunisolar', { params: { t: uniqid(), dt: setDate } });
    };
    this.getUsers = function () {
        return $http.get(gb_approot + apiGetAction + '/GetUsers', { params: {} });
    };
    this.getFortune = function (order_sn) {
        return $http.get(gb_approot + apiGetAction + '/GetFortuneLight', { params: { orders_sn: order_sn } });
    };
    this.showToaster = function (type, title, message) {
    };
    this.setApplyYearRange = function () {
        var y = [];
        var getNowYear = (new Date()).getFullYear();
        for (var i = 2016; i <= getNowYear; i++) {
            y.push(i);
        }
        return y;
    };
}]);
angular.module('commfun').directive('capitalize', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, modelCtrl) {
            var capitalize = function (inputValue) {
                if (angular.isUndefined(inputValue))
                    return;
                var capitalized = inputValue.toUpperCase();
                if (capitalized !== inputValue) {
                    modelCtrl.$setViewValue(capitalized);
                    modelCtrl.$render();
                }
                return capitalized;
            };
            modelCtrl.$parsers.push(capitalize);
            capitalize(scope[attrs.ngModel]);
        }
    };
});
angular.module('commfun').factory('gridpage', ['$http', function ($http) {
    var struc = {
        CountPage: function ($scope) {
            $scope.NowPage = $scope.NowPage == undefined ? 1 : $scope.NowPage;
            var s = { page: $scope.NowPage };
            if ($scope.sd != null) {
                for (var key in $scope.sd) {
                    s[key] = $scope.sd[key];
                }
            }
            var path = $scope.apiPath ? $scope.apiPath : apiConnection;
            if ($scope.NowPage >= 1) {
                $http.get(path, { params: s }).success(function (data, status, headers, config) {
                    $scope.Grid_Items = data.rows;
                    $scope.TotalPage = data.total;
                    $scope.NowPage = data.page;
                    $scope.RecordCount = data.records;
                    $scope.StartCount = data.startcount;
                    $scope.EndCount = data.endcount;
                    $scope.firstpage = 1;
                    $scope.prevpage = $scope.NowPage <= 1 ? 1 : data.page - 1;
                    $scope.lastpage = data.total;
                    $scope.nextpage = $scope.NowPage >= $scope.TotalPage ? $scope.TotalPage : data.page + 1;
                });
            }
        }
    };
    return struc;
}]);
angular.module('commfun').filter('codelang', function () {
    return function (input, data) {
        var r = input;
        if (data) {
            for (var i = 0; i < data.length; i++) {
                if (input == data[i].value) {
                    r = data[i].label;
                    break;
                }
            }
        }
        return r;
    };
});
angular.module('commfun').filter('coin', function () {
    return function (input, data) {
        var r = input;
        if (data) {
            for (var i = 0; i < data.length; i++) {
                if (input == data[i].code) {
                    r = data[i].sign;
                    break;
                }
            }
        }
        return r;
    };
});
angular.module('commfun').filter('left', function () {
    return function (input, string_length) {
        if (input.length > string_length) {
            return input.substring(0, string_length) + '...';
        }
        else {
            return input;
        }
    };
});
angular.module('commfun').filter('getTaiwanCalendarDate', function () {
    return function (input) {
        var d = new Date(Date.parse(input));
        var s = (d.getFullYear() - 1911) + "/" + (d.getMonth() + 1) + "/" + d.getDate();
        return s;
    };
});
angular.module('commfun').filter('godsonProductSn', function () {
    return function (input) {
        var s = "";
        for (var key in commData.godson_print) {
            if (input == commData.godson_print[key].value) {
                s = commData.godson_print[key].label;
            }
        }
        return s;
    };
});
angular.module('commfun').filter('OptionStr', function () {
    return function (input, data) {
        var res = "";
        var item = data.filter(function (x) {
            return x.value == input;
        });
        if (item.length > 0) {
            res = item[0].label;
        }
        return res;
    };
});
