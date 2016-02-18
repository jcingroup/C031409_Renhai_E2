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


angular.module('commfun', [])
    .directive('capitalize', function () {
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
    })
    .factory('gridpage', ['$http', function ($http: ng.IHttpService) {
        var struc = {
            CountPage: function ($scope: IMakeScopeBase) {

                var s = { page: $scope.NowPage };
                if ($scope.sd != null) {
                    for (var key in $scope.sd) {
                        s[key] = $scope.sd[key];
                    }
                }

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
        }
    return struc;
    }])
    .filter('codelang', function () {
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
})
    .filter('coin', function () {
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
})
    .filter('left', function () {
    return function (input, string_length) {
            if (input.length > string_length) {
                //console.log(input, input.length);
                return input.substring(0, string_length) + '...';
            } else {
            return input
        }
        }
})