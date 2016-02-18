<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/File_Function.asp"-->
<%
Dim p0	'訂單編號
    p0 = Request("p0")
%>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="../../_Css/Set.css" />
    <style type="text/css">
        .GridTable {
            margin-top: 10px;
            border: 5px double #778899;
        }

        TH {
            background: #F5F5DC;
        }

        .TDLine {
            background-color: #FF7573;
            color: #FFFFFF;
            text-align: center;
            font-size: 11pt;
        }

        .TableHeadTR {
            background-color: #1F6CBD;
            color: #FFFFFF;
            text-align: center;
            font-size: 12pt;
            padding-top: 3px;
            padding-bottom: 3px;
        }

        .TableHeadTD {
            border-style: solid;
            border-width: 1px;
            border-bottom-color: #333333;
            border-right-color: #CCCCCC;
        }

        .TableBodyTR {
            background-color: #EEF5FC;
            padding-top: 1px;
            padding-bottom: 1px;
            padding-left: 5px;
            padding-right: 5px;
        }

        .TableBodyTD {
            border-style: solid;
            border-width: 1px;
            border-bottom-color: #AAAAAA;
            border-left-color: #AAAAAA;
            border-right-color: #EEEEEE;
            border-top-color: #EEEEEE;
        }

        .TableBodyTdNum {
            text-align: right;
            border-style: solid;
            border-width: 1px;
            border-bottom-color: #AAAAAA;
            border-left-color: #AAAAAA;
            border-right-color: #EEEEEE;
            border-top-color: #EEEEEE;
        }
    </style>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.2.7/angular.min.js"></script>
    <script type="text/javascript">
        var id = '<%=p0%>';
        var gb_approot = '/Renhai2012/';
    </script>
    <script type="text/javascript">

        ajax_MasterUpdataName = '';

        var agApp = angular.module('angularApp', []);

        agApp.controller('ctr_edit', function ($scope, $http) {
            $scope.submit = function () {
                var obj = { orders_id: id, order_data: $scope.Items };
                $http.post(gb_approot + 'Logic/ajax_modOrderMemberItem', obj)
                .success(function (data, status, headers, config) {
                    if (data.result) {
                        alert('更新完成');
                    } else {
                        alert('更新失敗:' + data.message);
                    }
                });
            }

            $scope.options_born_sign = [
            { 'id': '鼠', 'text': '鼠' },
            { 'id': '牛', 'text': '牛' },
            { 'id': '虎', 'text': '虎' },
            { 'id': '兔', 'text': '兔' },
            { 'id': '龍', 'text': '龍' },
            { 'id': '蛇', 'text': '蛇' },
            { 'id': '馬', 'text': '馬' },
            { 'id': '羊', 'text': '羊' },
            { 'id': '猴', 'text': '猴' },
            { 'id': '雞', 'text': '雞' },
            { 'id': '狗', 'text': '狗' },
            { 'id': '豬', 'text': '豬' }
            ];

            $scope.options_born_time = [
            { 'id': '吉', 'text': '00:00~23:59 吉時' },
            { 'id': '子', 'text': '23:00~01:00 子時' },
            { 'id': '丑', 'text': '01:00~03:00 丑時' },
            { 'id': '寅', 'text': '03:00~05:00 寅時' },
            { 'id': '卯', 'text': '05:00~07:00 卯時' },
            { 'id': '辰', 'text': '07:00~09:00 辰時' },
            { 'id': '巳', 'text': '09:00~11:00 巳時' },
            { 'id': '午', 'text': '11:00~13:00 午時' },
            { 'id': '未', 'text': '13:00~15:00 未時' },
            { 'id': '申', 'text': '15:00~17:00 申時' },
            { 'id': '酉', 'text': '17:00~19:00 酉時' },
            { 'id': '戌', 'text': '19:00~21:00 戌時' },
            { 'id': '亥', 'text': '21:00~23:00 亥時' }
            ];

            //取得本筆訂單資料的會員成員
            $http.post(gb_approot + 'Logic/ajax_getOrderMemberItem', { id: id })
            .success(function (data, status, headers, config) {
                $scope.Items = data;
            })
        });
    </script>
    <title></title>
</head>
<body ng-app="angularApp">
    <form ng-controller="ctr_edit" ng-submit="submit()">
        <table class="gridtable" style="width: 99%; border: 1px">
            <tr>
                <td class="gridcaption" style="margin: 0px">
                    <table style="width: 100%">
                        <tr>
                            <td class="gridtdcaption">修正訂單會員資料 訂單編號<%=p0%></td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%; margin: 1px; padding: 1px; border: 1px">
                                    <tr class="TableHeadTR">
                                        <td class="TableHeadTD">產品</td>
                                        <td class="TableHeadTD">燈位</td>
                                        <td class="TableHeadTD">姓名</td>
                                        <td class="TableHeadTD">性別</td>
                                        <td class="TableHeadTD">農曆生日</td>
                                        <td class="TableHeadTD">生辰</td>
                                        <td class="TableHeadTD">生肖</td>
                                        <td class="TableHeadTD">地址</td>
                                    </tr>
                                    <tbody ng-repeat="gd in Items">
                                        <tr>
                                            <td class="TableBodyTD">{{gd.product_name}}</td>
                                            <td class="TableBodyTD">{{gd.light_name}}</td>
                                            <td class="TableBodyTD">
                                                <input type="text" ng-model="gd.member_name" style="width: 64px" required /></td>
                                            <td class="TableBodyTD">
                                                <input type="radio" ng-model="gd.gender" value="2" />女
                                                <input type="radio" ng-model="gd.gender" value="1" />男
                                            </td>
                                            <td class="TableBodyTD">
                                                <input type="text" ng-model="gd.l_birthday" style="width: 64px" /></td>
                                            <td class="TableBodyTD">
                                                <select ng-model="gd.born_time" ng-options="m.id as m.text for m in options_born_time"></select></td>
                                            <td class="TableBodyTD">
                                                <select ng-model="gd.born_sign" ng-options="m.id as m.text for m in options_born_sign"></select></td>
                                            <td class="TableBodyTD">
                                                <input type="text" ng-model="gd.address" style="width: 210px" /></td>
                                        </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <button type="submit">修正</button>
    </form>
</body>
</html>
