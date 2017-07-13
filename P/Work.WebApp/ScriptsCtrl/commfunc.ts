function uniqid() {
    /*
        Autohr:Jerry
        Date:2014/2/23
        Description:取得唯一值
    */
    var newDate: Date = new Date(); return newDate.getTime();
}
function obj_prop_list(obj) {
    /*
    Autohr:Jerry
    Date:2014/2/23
    Description:列出物件屬性
    */
    for (var prop in obj) {
        if (obj.hasOwnProperty(prop)) {
            console.log(prop + " :" + obj[prop]);
        }
    }
}
function stand_date(getDateStr: string) {
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
    var d = new Date(); return d.toUTCString() + '.' + d.getMilliseconds().toString();
}
function pad(str: string, len: number, pad: string, dir: number) {

    var padlen: number;
    if (typeof (len) == "undefined") { var len = 0; }
    if (typeof (pad) == "undefined") { var pad = ' '; }
    if (typeof (dir) == "undefined") { var dir = 3; }

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
function showAjaxError(data: any): void {
    alert('Ajax error,check console info!');
    console.log(data);
}
function isValidJSONDate(value: string, userFormat) {
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
            if (/m/.test(format[i])) m = date[i];
            if (/d/.test(format[i])) d = date[i];
            if (/y/.test(format[i])) y = date[i];
        };
        return (
            m > 0 && m < 13 &&
            y && y.length === 4 &&
            d > 0 && d <= (new Date(y, m, 0)).getDate()
            )
    }

    return isDate(theDate, theFormat)
}
function obj_prop_date(obj: any) {

    for (var prop in obj) {
        if (obj.hasOwnProperty(prop)) {
            var getUTCStr = obj[prop];
            if (typeof getUTCStr == 'string') {
                var isValid: boolean = isValidJSONDate(getUTCStr, null);
                if (isValid) {
                    obj[prop] = new Date(getUTCStr);
                }
            }
        }
    }

    return obj;
}


declare enum emToasterType {
    success = 0,
    error = 1,
    wait = 2,
    warning = 3,
    note = 4
}

module services {
    export class workService {
        getOrderData: (order_sn: string) => ng.IHttpPromise<IResultData<server.Orders>>;
        getMemberAll: (member_id: number) => ng.IHttpPromise<IResultData<server.Member>>;
        getMemberDetailAll: (member_id: number) => ng.IHttpPromise<IResultData<server.Member_Detail[]>>;
        getMember: (member_id: number) => ng.IHttpPromise<IResultData<server.Member>>;
        getMemberDetail: (member_detail_id: number) => ng.IHttpPromise<IResultData<server.Member_Detail>>;
        getMemberByDetail: (member_id: number) => ng.IHttpPromise<IResultData<server.Member_Detail>>;
        getProductAll: () => ng.IHttpPromise<IResultData<server.Product[]>>;
        getProductSDLight: () => ng.IHttpPromise<IResultData<server.Product[]>>;
        getProductMDLight: () => ng.IHttpPromise<IResultData<server.Product[]>>;
        getLightByMD: (product_sn: string) => ng.IHttpPromise<IResultData<server.Light_Site[]>>;
        getProductFortune: () => ng.IHttpPromise<IResultData<server.Product[]>>;
        getManjushri: () => ng.IHttpPromise<IResultData<server.Manjushri[]>>;
        getCalcLunar: (Y: number, M: number, D: number) => ng.IHttpPromise<server.LuniInfo>;
        getUsers: () => ng.IHttpPromise<IResultData<server.Users[]>>;
        getFortune: (orders_sn: string) => ng.IHttpPromise<IResultData<number[]>>;
        showToaster: (type: emToasterType, title: string, message: string) => void;
        setApplyYearRange: () => number[];
    }
};

angular.module('commfun', []).service('workService', ['$http', function ($http: ng.IHttpService) {
    this.getOrderData = function (order_sn: string) { //取得訂單主檔及明細檔全部資料
        return $http.get(gb_approot + 'Orders/GetOrders', { params: { order_sn: order_sn } });
    };

    this.getMemberDetailAll = function (member_id: number) {//取得家庭成員全部資料 不含主檔
        return $http.get(gb_approot + 'api/Member_Detail', { params: { member_id: member_id } });
    };

    this.getMemberAll = function (member_id: number) { //取得家庭全部資料
        return $http.get(gb_approot + 'Member/getMemberAll', { params: { member_id: member_id } });
    };

    this.getMember = function (member_id: number) { //取得戶長資料 單筆
        return $http.get(gb_approot + 'Member/GetMember', { params: { member_id: member_id } });
    };

    this.getMemberDetail = function (member_detail_id: number) { //取得成員資料 單筆
        return $http.get(gb_approot + 'Member/GetMemberDetail', { params: { member_detail_id: member_detail_id } });
    };

    this.getMemberByDetail = function (member_id: number) {//取得從明細檔取得戶長完整資料 單筆
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
    this.getLightByMD = function (product_sn: string) {
        return $http.get(gb_approot + apiGetAction + '/GetLightByMD', { params: { product_sn: product_sn } });
    };

    this.getProductFortune = function () {
        return $http.get(gb_approot + apiGetAction + '/GetProductFortune', { params: {} });
    };


    this.getManjushri = function () {
        return $http.get(gb_approot + 'Manjushri/GetManjushri', { params: { t: uniqid() } });
    };

    this.getCalcLunar = function (Y, M, D) {
        //Y:是民國值 不是西元
        var setDate = new Date(Y + 1911, M, D);
        return $http.get(gb_approot + apiGetAction + '/GetLunisolar', { params: { t: uniqid(), dt: setDate } });
    };

    this.getUsers = function () {
        return $http.get(gb_approot + apiGetAction + '/GetUsers', { params: {} });
    };

    this.getFortune = function (order_sn: string) { //取得訂單主檔及明細檔全部資料
        return $http.get(gb_approot + apiGetAction + '/GetFortuneLight', { params: { orders_sn: order_sn } });
    };

    this.showToaster = function (type: emToasterType, title: string, message: string) {
        //if (type == emToasterType.success)
        //    toaster.pop('success', title, message);

        //if (type == emToasterType.error)
        //    toaster.pop('error', title, message);

        //if (type == emToasterType.wait)
        //    toaster.pop('wait', title, message);

        //if (type == emToasterType.warning)
        //    toaster.pop('warning', title, message);

        //if (type == emToasterType.note)
        //    toaster.pop('note', title, message);
    }
    this.setApplyYearRange = function (): number[] {
        var y: number[] = [];
        var getNowYear: number = (new Date()).getFullYear();

        //for (var i = getNowYear - 1; i <= getNowYear + 1; i++) {
        //    y.push(i);
        //}
        for (var i = 2016; i <= getNowYear; i++) {
            y.push(i);
        }
        return y;
    };
}]);
angular.module('commfun').directive('capitalize', function () {
    return {
        require: 'ngModel',
        link: function (scope: ng.IScope, element, attrs, modelCtrl) {
            var capitalize = function (inputValue) {

                if (angular.isUndefined(inputValue))
                    return;

                var capitalized = inputValue.toUpperCase();
                if (capitalized !== inputValue) {
                    modelCtrl.$setViewValue(capitalized);
                    modelCtrl.$render();
                }
                return capitalized;
            }
            modelCtrl.$parsers.push(capitalize);
            capitalize(scope[attrs.ngModel]);
        }
    };
});
angular.module('commfun').factory('gridpage', ['$http', function ($http: ng.IHttpService) {
    var struc = {
        CountPage: function ($scope: IMakeScopeBase) {

            $scope.NowPage = $scope.NowPage == undefined ? 1 : $scope.NowPage;

            var s = { page: $scope.NowPage };
            if ($scope.sd != null) {
                for (var key in $scope.sd) {
                    s[key] = $scope.sd[key];
                }
            }
            var path = $scope.apiPath ? $scope.apiPath : apiConnection;
            //console.log('path',path);

            if ($scope.NowPage >= 1) {
                $http.get(apiConnection, { params: s })
                    .success(function (data: widegt.GridInfo2, status, headers, config) {
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
    }
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
    }
});
angular.module('commfun').filter('left', function () {
    return function (input, string_length) {
        if (input.length > string_length) {
            return input.substring(0, string_length) + '...';
        } else {
            return input
        }
    }
});

angular.module('commfun').filter('getTaiwanCalendarDate', function () {
    return function (input) {
        var d = new Date(Date.parse(input));
        var s = (d.getFullYear() - 1911) + "/" + (d.getMonth() + 1) + "/" + d.getDate();
        return s;
    }
});
angular.module('commfun').filter('godsonProductSn', function () {
    return function (input) {
        var s: string = "";
        for (var key in commData.godson_print) {
            if (input == commData.godson_print[key].value) {
                s = commData.godson_print[key].label;
            }
        }
        return s;
    }
});