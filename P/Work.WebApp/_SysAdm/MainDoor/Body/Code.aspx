<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Code.aspx.cs" Inherits="DotWeb._SysAdm.MainDoor.Body.Code" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
      <style type="text/css">
        .GridTable
        {
            margin-top: 10px;
            border: 5 double #778899;
        }
        TH
        {
            background: #F5F5DC;
        }
        
        .TDLine
        {
            background-color: #FF7573;
            color: #FFFFFF;
            text-align: center;
            font-size: 11pt;
        }
        
        .TableHeadTR
        {
            background-color: #1F6CBD;
            color: #FFFFFF;
            text-align: center;
            font-size: 12pt;
            padding-top: 3px;
            padding-bottom: 3px;
        }
        .TableHeadTD
        {
            border-style: solid;
            border-width: 1px;
            border-bottom-color: #333333;
            border-right-color: #CCCCCC;
        }
        
        .TableBodyTR
        {
            background-color: #EEF5FC;
            padding-top: 1px;
            padding-bottom: 1px;
            padding-left: 5px;
            padding-right: 5px;
        }
        
        .TableBodyTD
        {
            border-style: solid;
            border-width: 1px;
            border-bottom-color: #AAAAAA;
            border-right-color: #EEEEEE;
        }
        
        .TableBodyTdNum
        {
            text-align: right;
            border-style: solid;
            border-width: 1px;
            border-bottom-color: #AAAAAA;
            border-right-color: #EEEEEE;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div><a href="<%=ResolveUrl("~/Orders#/edit/gorders?member_id=10000001")%>">新增一般訂單(學勤)</a></div>
            <div><a href="<%=ResolveUrl("~/Orders#/edit/forders?member_id=10000001")%>">新增福燈訂單(學勤)</a></div>
            <div><a href="<%=ResolveUrl("~/Orders#/edit/wishorders?member_id=10000001")%>">新增祈福許願訂單(學勤)</a></div>
            <div><a href="<%=ResolveUrl("~/Orders")%>">訂單管理</a></div>
            <div><a href="<%=ResolveUrl("~/MemberMark")%>">會員重複標記</a></div>
            <div><a href="<%=ResolveUrl("~/TempleMember")%>">契子會會員管理</a></div>
            <div><a href="<%=ResolveUrl("~/ExcelReport/downloadExcel_PostMember?year=2015")%>">會員郵寄標籤列印</a></div>
            <div><a href="<%=ResolveUrl("~/ExcelReport/ajax_MakeExcel?Date1=2017-01-01&Date2=2017-12-07&Time1=01&Time2=24&People=1000001")%>">統計表列印</a></div>
            <div><a href="<%=ResolveUrl("~/ExcelReport/LiDoRoll?year=2017")%>">禮斗名冊列印</a></div>
        </div>
        <table id="TableProductDetailView" style="width: 100%; margin: 1px; padding: 1px; border: 1px">
            <thead>
                <tr class="TableHeadTR">
                    <td class="TableHeadTD">禮斗名稱</td>
                    <td class="TableHeadTD">燈位名稱</td>
                    <td class="TableHeadTD">姓名</td>
                    <td class="TableHeadTD">住址</td>
                    <td class="TableHeadTD">電話</td>
                </tr>
            </thead>
            <tbody></tbody>
        </table>
    </form>
</body>
</html>
<script type="text/javascript">
    $(document).ready(function () {
        $.ReFreshDetailHTML();
    })
</script>
<script type="text/javascript">
    (function ($) {

        //取得統計資料
        $.ReFreshDetailHTML = function () {
            $.ajax({
                url: '../../../Logic/ajax_getLiDo',
                type: "POST",
                datatype: "json",
                contentType: "application/x-www-form-urlencoded; charset=utf-8",
                data: {},
                cache: false,
                global: true,
            }).done(function (response) {
                var stai = jQuery.parseJSON(response);
                if (stai.result) {
                    $('tbody', $('#TableProductDetailView')).html(''); //first clear
                    for (var i = 0; i < stai.Module.length; i++) {

                        $('tbody', $('#TableProductDetailView'))
                        .append
                        (
                                $('<tr class="TableBodyTR">')
                                    .append($('<td class="TableBodyTD">').html(stai.Module[i].產品名稱))
                                    .append($('<td class="TableBodyTD">').html(stai.Module[i].燈位名稱))
                                    .append($('<td class="TableBodyTdNum">').html(stai.Module[i].姓名))
                                    .append($('<td class="TableBodyTdNum">').html(stai.Module[i].地址))
                                    .append($('<td class="TableBodyTdNum">').html(stai.Module[i].電話))
                        )
                    }
                } else {

                    alert(stai.message);
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                //alert("Request failed: " + textStatus);
                alert(errorThrown);
            });
        };
    })(jQuery);
</script>
