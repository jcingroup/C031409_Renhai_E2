<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/File_Function.asp"-->
<%
On Error Resume Next
Dim MasterID,MemberID
Dim p0,p0SN	'訂單編號
Dim p1,p1ReadOnly'
Dim p2,p2RadioA,p2RadioB	'
Dim p3	'
Dim p4	'
Dim p5	'
Dim p6	'
Dim p7	'
Dim p8  '
Dim p9,p9Option
Dim p10,p10Option
Dim p11
Dim p12
Dim p13,today '新增時間

Dim SF_EPD_p1_Disabled
Dim SF_EPD_p2_ReadOnly
Dim SF_EPD_p3_Disabled

Dim SF_EPD_p13Option

Dim CF_EditProductDetail_p2Option
Dim CF_EditProductDetail_p18Option

Dim EditType
	'-- 取得 ID
	p0SN = Trim(Request("p0")) '序號

	If p0SN = "" Then
		EditType = "Insert"	'新增模式
		MasterID = Request("MasterID")
	Else
		EditType = "Modify"	'修改模式
	End If

	'-- 取得產品資料
	StrSql="SELECT 產品編號,產品名稱 FROM 產品資料表 Where 隱藏 = '0' and 產品分類='禮斗' and 選擇 = 0 Order By 排序"
	Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	CF_EditProductDetail_p2Option = RsToOption(oRs,0,1,"","","")

	StrSql="SELECT 序號,梯次 + ' ' + convert(varchar(20),時間,111) as 梯次 FROM 文疏梯次時間表 Order By 時間"
	Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	CF_EditProductDetail_p18Option = RsToOption(oRs,0,1,"","","")

%>
<html>
<head>
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <link rel="stylesheet" href="../../_Css/Set.css">
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

	<script type="text/javascript">

        function WO(URL) {
            window.open(URL, "WO", "Left=0,Top=0,width=1180,height=750,center=yes,status=no,toolbar=no,scrollbars=yes");
        }

    </script>
	
	</head>
<body>
    <!--#include file="../../_include/top.asp"-->
    <input type="hidden" id="MasterID" name="MasterID" value="<%=MasterID%>">
    <input type="hidden" id="MemberID" name="MemberID" value="">
    <form id="myform" name="myform" action="../../../Logic/ajax_UpdateOrderHandle" method="post">
    <input type="hidden" id="EditType" name="EditType" value="<%=EditType%>">
    <input type="hidden" id="p0SN" name="p0SN" value="<%=p0SN%>">
    <input type="hidden" name="MasterID" value="<%=MasterID%>">
    <input type="hidden" name="MemberID" value="">
    <table class="gridtable" style="width: 99%; height: 450px; border: 1px">
        <caption id="CaptionText"></caption>
        <tr>
            <td style="vertical-align: top">
                <table style="width: 100%; margin: 1px; padding: 1px; border: 0px">
                    <tr>
                        <td style="width: 90px">
                        </td>
                        <td style="width: 180px">
                        </td>
                        <td style="width: 90px">
                        </td>
                        <td style="width: 280px">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="TDLine">
                            基本資料填寫
                        </td>
                    </tr>
                    <tr>
                        <th style="text-align: right">
                            (請填戶長資料)申請人姓名：
                        </th>
                        <td>
                            <input type="text" name="p1" id="p1" value="<%=p1%>" maxlength="20" size="20" <%=p1ReadOnly%> />
                            <input type="button" id="btn_OpenMasterSearch" value="查詢" />
                        </td>
                        <th style="text-align: right">
                            申請人性別：
                        </th>
                        <td>
                            <input type="radio" name="p2" id="p2A" value="男" />先生
                            <input type="radio" name="p2" id="p2B" value="女" />小姐
                        </td>
                    </tr>
                    <tr>
                        <th style="text-align: right">
                            申請人地址：
                        </th>
                        <td colspan="3">
                            <input type="text" name="p3" id="p3" size="5" value="<%=p3%>">
                            <input type="text" name="p4" id="p4" value="<%=p4%>" maxlength="128" size="64" />
                        </td>
                    </tr>
                    <tr>
                        <th style="text-align: right">
                            申請人手機：
                        </th>
                        <td>
                            <input type="text" name="p5" id="p5" value="<%=p5%>" maxlength="16" size="16" />
                        </td>
                        <th style="text-align: right">
                            申請人電話：
                        </th>
                        <td>
                            <input type="text" name="p6" id="p6" value="<%=p6%>" maxlength="16" size="16" />
                        </td>
                    </tr>
                    <tr>
                        <th style="text-align: right">
                        </th>
                        <td colspan="3">
                            <input type="text" name="p7" id="p7" value="<%=p7%>" maxlength="64" size="64" style="display: none" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="TDLine">
                            訂單明細資料填寫
                            <%If p13>=today Then%>
                            <input type="button" name="AddNewDetail" id="AddNewDetail" value=" 新 增 明 細 ">
                            <%End If%>
                        </td>
                    </tr>
                    <!--Order Detail Start-->
                    <tr>
                        <td colspan="4">
                            <table id="TableProductDetailView" style="width: 100%; margin: 1px; padding: 1px;
                                border: 1px">
                                <thead>
                                    <tr class="TableHeadTR">
                                        <td class="TableHeadTD">
                                            會員
                                        </td>
                                        <td class="TableHeadTD">
                                            產品
                                        </td>
                                        <td class="TableHeadTD">
                                            單價
                                        </td>
                                        <td class="TableHeadTD">
                                            檢視
                                        </td>
                                        <%'If Session("UserKind")=99 Then%>
                                        <td class="TableHeadTD">
                                            刪除
                                        </td>
                                        <%'End If%>
                                    </tr>
                                </thead>
                                <tbody>
                                    <%=ProductDetailList%></tbody>
                            </table>
                        </td>
                    </tr>
                    <!--Order Detail End-->
                    <tr>
                        <td class="gridtdtool" colspan="4" style="text-align: center">
                            <span class="errmsg">
                                <%=strMsg%></span><br />
                            <input id="btn_Print" type="button" class="button" onclick="WO('../../Report/Receipt/ExcelExport.asp?p0='+document.getElementById('p0SN').value)"
                                value="列印感謝狀" style="margin-right: 10px;">
                            <!--'控制確認儲存的按鈕顯示或除藏-->
                            <%If p13>=today Then%>
                            <input id="btn_submit" name="btn_submit" class="gridsubmit" type="button" value="確　　認"
                                style="cursor: Hand" /><span style="color: red">(有任何修改異動請務必按此鍵存檔)</span>
                            <%End If%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
    <!-- 子板面 產品明細維護-->
    <form id="SubFrm_EditProductDetail" method="post" action="../../../Logic/btn_ChildFrm_EditProductDetail">
    <input type="hidden" id="SF_EPD_MasterID" value="<%=MasterID%>">
    <input type="hidden" name="MasterID" value="<%=MasterID%>">
    <input type="hidden" name="MemberID" value="">
    <div id="SubDiv_EPD" style="display: none; width: 650px; height: 320px; background-color: #333333">
        <br />
        <div style="width: 100%; border-style: solid; border-width: 1px; border-color: green green green green;
            background-color: #FFFFFF">
            <table style="width: 100%; margin: 1px; padding: 1px; border: 0px">
                <tr>
                    <th style="width: 120px">
                    </th>
                    <td style="width: 200px">
                    </td>
                    <th style="width: 200px">
                    </th>
                    <td style="width: 200px">
                    </td>
                </tr>
                <tr>
                    <th style="text-align: center">
                        祈福姓名：
                    </th>
                    <td>
                        <select name="SF_EPD_p1" id="SF_EPD_p1">
                            <%=r_Option_Members%>
                        </select>
                        <input type="text" name="SF_EPD_p1_Name" id="SF_EPD_p1_Name" size="20" maxlength="20">
                    </td>
                    <th style="text-align: center">
                        祈福種類：
                    </th>
                    <td>
                        <select name="SF_EPD_p2" id="SF_EPD_p2">
                            <option value=""></option>
                            <%=CF_EditProductDetail_p2Option%>
                        </select><span id="SF_EPD_p2_Name"></span>
                        <input id="btn_ChildFrm_EditProductDetail2" name="btn_ChildFrm_EditProductDetail2"
                            class="gridsubmit" type="button" value="新 增" style="cursor: Hand" />
                        <input type="button" value="關　閉" id="btn_close_ChildFrm_EditProductDetail" />
                    </td>
                </tr>
                <tr>
                    <th style="text-align: center">
                        祈福性別：
                    </th>
                    <td>
                        男<input type="radio" id="SF_EPD_p5A" name="SF_EPD_p5" value="1" />
                        女<input type="radio" id="SF_EPD_p5B" name="SF_EPD_p5" value="2" />
                    </td>
                    <th style="text-align: center">
                        單價：
                    </th>
                    <td>
                        <input type="text" id="SF_EPD_p16" name="SF_EPD_p16" size="7" maxlength="7" />
                    </td>
                </tr>
                <tr>
                    <th style="text-align: center">
                        農曆生日：
                    </th>
                    <td>
                        <input type="text" id="SF_EPD_p6" name="SF_EPD_p6" size="4" maxlength="3" value="" />年
                        <input type="text" id="SF_EPD_p8" name="SF_EPD_p8" size="2" maxlength="2" value="" />月
                        <input type="text" id="SF_EPD_p17" name="SF_EPD_p17" size="2" maxlength="2" value="" />日
                    </td>
                    <th style="text-align: center">
                        出生時辰：
                    </th>
                    <td>
                        <select id="SF_EPD_p3" name="SF_EPD_p3">
                            <option value="子">23:00~01:00 子時</option>
                            <option value="丑">01:00~03:00 丑時</option>
                            <option value="寅">03:00~05:00 寅時</option>
                            <option value="卯">05:00~07:00 卯時</option>
                            <option value="辰">07:00~09:00 辰時</option>
                            <option value="巳">09:00~11:00 巳時</option>
                            <option value="午">11:00~13:00 午時</option>
                            <option value="未">13:00~15:00 未時</option>
                            <option value="申">15:00~17:00 申時</option>
                            <option value="酉">17:00~19:00 酉時</option>
                            <option value="戌">19:00~21:00 戌時</option>
                            <option value="亥">21:00~23:00 亥時</option>
                            <option value="吉">00:00~23:59 吉時</option>
                        </select>
                </tr>
                <tr>
                    <th style="text-align: center">
                        祈福地址：
                    </th>
                    <td colspan="3">
                        <input type="text" id="SF_EPD_p7" name="SF_EPD_p7" size="10" style="width: 100%" />
                    </td>
                </tr>
                <tr>
                    <th style="text-align: center">
                        電話：
                    </th>
                    <td>
                        <input type="hidden" id="SF_EPD_p9" name="SF_EPD_p9" />
                        <input type="text" id="SF_EPD_p10" name="SF_EPD_p10" size="8" />
                    </td>
                    <th style="text-align: center">
                        行動電話：
                    </th>
                    <td>
                        <input type="text" id="SF_EPD_p11" name="SF_EPD_p11" size="10" value="" style="width: 100%" />
                    </td>
                </tr>
                <tr style="display: none">
                    <th style="text-align: center">
                        捐白米：
                    </th>
                    <td>
                        <input type="text" id="SF_EPD_p14" name="SF_EPD_p14" size="3" maxlength="20" />(斤)
                    </td>
                    <th style="text-align: center">
                        捐金牌：
                    </th>
                    <td>
                        <input type="text" id="SF_EPD_p15" name="SF_EPD_p15" size="3" maxlength="20" />(面)
                    </td>
                </tr>
                <tr>
                    <th style="text-align: center">
                        點燈位置：
                    </th>
                    <td>
                        <div id="SF_EPD_p13">
                        </div>
                    </td>
                    <th style="text-align: center">
                    </th>
                    <td>
                        <select id="SF_EPD_p18" name="SF_EPD_p18" style="display: none">
                            <option value=""></option>
                            <%=CF_EditProductDetail_p18Option%>
                        </select>
                    </td>
                </tr>
                <tr>
                    <th style="text-align: center">
                        祈福事項：
                    </th>
                    <td colspan="3">
                        <textarea name="SF_EPD_p12" id="SF_EPD_p12" style="width: 100%" rows="5" cols="15"
                            maxlength="400"></textarea>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center">
                        <input id="btn_ChildFrm_EditProductDetail" name="btn_ChildFrm_EditProductDetail"
                            class="gridsubmit" type="button" value="新 增" style="cursor: Hand" />
                        <input type="button" value="關　閉" id="btn_close_ChildFrm_EditProductDetail2" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
    <!-- 子板面 產品明細維護-->
    <!--#include file="../../_include/bottom.asp"-->
</body>
</html>
<!--Start Init-->
<script type="text/javascript">
    $(document).ready(function () {

        $.StatusInit(); //初始化介面狀態
        $.ajax_ClearSessionOrderItems(); //進入即先清空Session

        //載入訂單資料
        if ($('#p0SN').val() != '') { //如果Key有傳值進來為修改狀態
            //call server 將database data load to session car

            $.when.apply($, ajaxRequest).done(function () {
                $.ajax_LoadOrderToSession();

                $.when.apply($, ajaxRequest).done(function () {
                    $.ajax_GetSessionOrderHead();
                    $.ReFreshDetailHTML();

                });
            });

        } else {
            //新增訂單狀態，載入戶長資料即可。
            $.when.apply($, ajaxRequest).done(function () {
                $.ajax_LoadMasterData();
            });
        }

        $('.RemoveSessionItem').live('click', function () {
            $.ajax_RemoveSessionOrderItem($(this).attr('memberId'), $(this).attr('prodId'));
        });

        $('.ViewSessionItem').live('click', function () {
            $.ajax_ViewSessionOrderItems($(this).attr('memberId'), $(this).attr('prodId'));
        });

        $('#AddNewDetail').click(function () { //開啟產品明細編輯

            if ($('#p1').val() != '') {

                if ($("prod").size() <= 12) {
                    $('#SubDiv_EPD').center();
                    $('#SubDiv_EPD').show();

                    $('#SF_EPD_p1').show();
                    $('#SF_EPD_p2').show();
                    $('#SF_EPD_p2_Name').text('');
                    $('#btn_ChildFrm_EditProductDetail').show();
                    $('#btn_ChildFrm_EditProductDetail2').show();
                } else
                    alert('每張訂單最多只能設定12筆產品資料!');
            }
            else
                alert('尚未選擇戶長!需選擇戶長後方可進行產品設定');
        })

        $("#p8").datepicker(); $("#p11").datepicker();
    })
</script>

<!--主表單-->
<script type="text/javascript">
    $(document).ready(function () {
        //ajax主要表單設定
        //表單填寫規則設定
        $("#myform").validate({
            rules:
				{
				    p1: { required: true },
				    p3: { required: true },
				    p4: { required: true },
				    p7: { email: true },
				    p8: { required: true, date: true }
				},
            messages:
				{
				}
        });

        $('#btn_submit').click(function () {
            if ($('#myform').valid()) {
                $('#myform').submit();
            }
        });

        $('#myform').submit(function () {
            $(this).ajaxSubmit(options);
            return false;
        });

        var options = { target: '#outputmessagefromserver',
            beforeSubmit: showRequest,
            success: showResponse,
            dataType: 'json'
        };

        function showRequest(formData, jqForm, options) {
            var queryString = $.param(formData);
            return true;
        }

        function showResponse(jsonobj, statusText, xhr, $form) {

            if (jsonobj.result) {
                if (jsonobj.id != 0) {
                    $('#p0').val(jsonobj.id);
                    $('#p0SN').val(jsonobj.IdStr);
                    $('#EditType').val("Modify");
                    $.StatusInit();
                }
                alert('更新完成');
            }

            if (jsonobj.message != '')
                alert(jsonobj.message);
        }
        //ajax主要表單結束

        $('#SF_EPD_p1').change(function () { //選擇家成員的事件
            $('#SF_EPD_p1_Name').val($('#SF_EPD_p1').find("option:selected").text());
            //取得該會員資料
            var data_jsonstyle = { MemberID: escape($('#SF_EPD_p1').val()) };
            $.EventAjaxHandle(data_jsonstyle, "ajax.sle_CF_EPD_p1.change.asp")
            .done(function (data, textStatus, jqXHR) {

                var jsonObject = jQuery.parseJSON(data);

                if (jsonObject.result) {
                    $('#SF_EPD_p9').val(jsonObject.電話區碼);
                    $('#SF_EPD_p10').val(jsonObject.電話尾碼);
                    $('#SF_EPD_p7').val(jsonObject.地址);
                    $('#SF_EPD_p11').val(jsonObject.手機);
                    $('#SF_EPD_p3').val(jsonObject.時辰);

                    if (jsonObject.IsBirthday) {

                        $('#SF_EPD_p6').val(jsonObject.年);
                        $('#SF_EPD_p8').val(jsonObject.月);
                        $('#SF_EPD_p17').val(jsonObject.日);
                    }
                    else {
                        $('#SF_EPD_p6').val('');
                        $('#SF_EPD_p8').val('');
                    }

                    if (jsonObject.性別 == '1') {
                        $('#SF_EPD_p5A').attr('checked', true);
                        $('#SF_EPD_p5B').prop('checked', false);
                    }

                    if (jsonObject.性別 == '2') {
                        $('#SF_EPD_p5A').prop('checked', false);
                        $('#SF_EPD_p5B').attr('checked', true);
                    }
                }
                else alert(jsonObject.message);
            });
        })
    })
</script>

<!--產品處理-->
<script type="text/javascript">
    //子表單 客戶搜尋 設定
    $(document).ready(function () {
        //子表單 產品明細 設定
        var options = {
            target: '#outputmessagefromserver',
            beforeSubmit: showRequest,
            success: showResponse,
            dataType: 'json'
        };

        $("#SubFrm_EditProductDetail").validate({
            rules:
			{
			    SF_EPD_p1_Name: { required: true },
			    SF_EPD_p14: { digits: true },
			    SF_EPD_p15: { digits: true },
			    SF_EPD_p6: { required: true, digits: true },
			    SF_EPD_p8: { required: true, digits: true },
			    SF_EPD_p17: { required: true, digits: true }
			},
            messages:{}
        });

        $('#btn_ChildFrm_EditProductDetail').click(function () {

            if ($('#SubFrm_EditProductDetail').valid()) {
                //if ($.CheckProdDetailEdit()) {
                $('#SubFrm_EditProductDetail').submit();
                //}
            }
        });

        $('#btn_ChildFrm_EditProductDetail2').click(function () {
            if ($('#SubFrm_EditProductDetail').valid()) {
                //if ($.CheckProdDetailEdit()) {
                $('#SubFrm_EditProductDetail').submit();
                //}
            }
        });

        $('#SubFrm_EditProductDetail').submit(function () {
            $(this).ajaxSubmit(options);
            return false;
        });

        function showRequest(formData, jqForm, options) {
            var queryString = $.param(formData);
            return true;
        }

        function showResponse(jsonobj, statusText, xhr, $form) {
            if (jsonobj.result)
                $.ReFreshDetailHTML();

            if (jsonobj.message != "")
                alert(jsonobj.message);
        }

        //選擇產品時切換價錢
        $('#SF_EPD_p2').change(function () {

            $.ProdStatusSet($(this).val());

            var data_jsonstyle = { ProdcutID: escape($('#SF_EPD_p2').val()) };
            $.EventAjaxHandle(data_jsonstyle, "ajax.sle_CF_EPD_p2.change.asp")
            .done(function (data, textStatus, jqXHR) {
                var jsonObject = jQuery.parseJSON(data);
                if (jsonObject.result)
                    $('#SF_EPD_p16').val(jsonObject.價格);
                else
                    alert(jsonObject.message);
            })
        })

        //子表單 結束

        $('#btn_close_ChildFrm_EditProductDetail').click(function () {
            $('#SubDiv_EPD').hide();
        })

        $('#btn_close_ChildFrm_EditProductDetail2').click(function () {
            $('#SubDiv_EPD').hide();
        })
    })
</script>

<!--Ajax-->
<script type="text/javascript">
    (function ($) {
        $.fn.ShowiFrame = function () {
            $('#ChildFrm_SearchHouseMaster_Result').html("搜尋結果");
            $('#ChildDiv_SearchHouseMaster').center();
            $('#ChildDiv_SearchHouseMaster').show();
        };

        //出始化預設狀態
        $.StatusInit = function () {
            if ($('#EditType').val() == "Insert") {
                $('#btn_submit').val("確認");
                $('#CaptionText').text("訂單管理：新增");
                $.ProdStatusSet('');

                var myDate = new Date();
                var displayDate = myDate.getFullYear() + '/' + (myDate.getMonth() + 1) + '/' + (myDate.getDate());
                $('#p8').val(displayDate);
                $('#p11').val(displayDate);

                $('#btn_Print').hide();
            }

            if ($('#EditType').val() == "Modify") {
                $('#btn_submit').val("確認");
                $('#CaptionText').text("訂單管理：修改");
                $('#btn_Print').show();
            }

            //$.EventAjaxHandle();
        };

        //依據選擇的產品 設定產品表單
        $.ProdStatusSet = function (ProdValue) {
            $('#SF_EPD_p16').attr("readonly", true);
            $('#SF_EPD_p14').attr("disabled", true);
            $('#SF_EPD_p15').attr("disabled", true);
            $('#SF_EPD_p18').attr("disabled", true);

            $.EventAjaxHandle({ ProdId: ProdValue }, '../../../Logic/ajax_SetupProductState')
            .done(function (data, textStatus, jqXHR) {
                var jsonObject = jQuery.parseJSON(data);
                if (typeof (jsonObject.length) != 'undefined') {
                    for (var i = 0; i < jsonObject.length; i++) {
                        if (jsonObject[i].AttrValue != 'undefined')
                            $(jsonObject[i].ElementId).attr(jsonObject[i].AttrName, jsonObject[i].AttrValue);

                        if (jsonObject[i].Value != null)
                            $(jsonObject[i].ElementId).val(jsonObject[i].Value);
                    }
                }
            });
        }

        //清除購物車項目Session
        $.ajax_ClearSessionOrderItems = function () {
            $.EventAjaxHandle({}, '../../../Logic/ajax_ClearSessionOrderItems')
            .done(function (data, textStatus, jqXHR) {
                var jsonObject = jQuery.parseJSON(data);
                if (jsonObject.message != '')
                    alert(jsonObject.message)
            });
        }

        //清空購物車Session再載入指定的訂單資料至Session
        $.ajax_LoadOrderToSession = function () {
            $.EventAjaxHandle({ OrderSerial: $('#p0SN').val() }, '../../../Logic/ajax_LoadOrderToSession')
            .done(function (data, textStatus, jqXHR) {
                var jsonObject = jQuery.parseJSON(data);
                if (!jsonObject.Module) {
                    $("#AddNewDetail").hide();
                    $("#btn_ChildFrm_EditProductDetail").hide();
                }
                if (jsonObject.message != '')
                    alert(jsonObject.message)
            });
        }

        //移除購物車項目單項Session
        $.ajax_RemoveSessionOrderItem = function (p_MemberId, p_ProdId) {

            $.EventAjaxHandle({ MemberId: p_MemberId, ProdId: p_ProdId }, '../../../Logic/ajax_RemoveSessionOrderItem')
            .done(function (data, textStatus, jqXHR) {
                var jsonObject = jQuery.parseJSON(data);
                if (jsonObject.message != '')
                    alert(jsonObject.message)

                $.when.apply($, ajaxRequest).done(function () {
                    $.ReFreshDetailHTML();
                })
            })
        }

        //檢視購物車項目單項Session
        $.ajax_ViewSessionOrderItems = function (p_MemberId, p_ProdId) {

            $.EventAjaxHandle({ MemberId: p_MemberId, ProdId: p_ProdId }, '../../../Logic/ajax_ViewSessionOrderItem')
            .done(function (data, textStatus, jqXHR) {
                var jsonObject = jQuery.parseJSON(data);

                $('#SubDiv_EPD').center();
                $('#SubDiv_EPD').show();

                $('#SF_EPD_p1').val(jsonObject.會員編號);
                $('#SF_EPD_p1_Name').val($('#SF_EPD_p1').find("option:selected").text());
                $('#SF_EPD_p2').val(jsonObject.產品編號);
                $('#SF_EPD_p7').val(jsonObject.申請人地址);
                $('#SF_EPD_p14').val(jsonObject.白米);
                $('#SF_EPD_p15').val(jsonObject.金牌);
                $('#SF_EPD_p13').html(jsonObject.點燈位置);
                $('#SF_EPD_p16').val(jsonObject.價格);
                $('#SF_EPD_p18').val(jsonObject.文疏);

                $('#SF_EPD_p6').val(jsonObject.申請人生日.split('/')[0]);
                $('#SF_EPD_p8').val(jsonObject.申請人生日.split('/')[1]);
                $('#SF_EPD_p17').val(jsonObject.申請人生日.split('/')[2]);

                if (jsonObject.申請人性別 == '1') { $('#SF_EPD_p5A').attr('checked', true); }
                if (jsonObject.申請人性別 == '2') { $('#SF_EPD_p5B').attr('checked', true); }

                $('#SF_EPD_p1').hide();
                $('#SF_EPD_p1_Name').attr("readonly", true);
                $('#SF_EPD_p2').hide();
                $('#SF_EPD_p2_Name').text($('#SF_EPD_p2').find("option:selected").text());

                //檢視狀態不做修改 把修改新增Button Hide
                $('#btn_ChildFrm_EditProductDetail').hide();
                $('#btn_ChildFrm_EditProductDetail2').hide();
            })
        }

        //清空購物車Session再載入指定的訂單資料至Session
        $.ajax_LoadMasterData = function () {

            $.EventAjaxHandle({ MasterID: $('#MasterID').val() }, '../../../Logic/ajax_LoadMasterId')
            .done(function (data, textStatus, jqXHR) {
                var jsonObject = jQuery.parseJSON(data);

                if (jsonObject.result) {
                    var ModuleData = jsonObject.Module;

                    $('#p1').val(ModuleData.姓名);
                    if (ModuleData.性別 == '1') $('#p2A').attr('checked', true);
                    if (ModuleData.性別 == '2') $('#p2B').attr('checked', true);

                    $('#p3').val(ModuleData.Zip);
                    $('#p4').val(ModuleData.縣市 + ModuleData.鄉鎮 + ModuleData.地址);
                    $('#p5').val(ModuleData.手機);
                    $('#p6').val(ModuleData.電話);

                    $('input[name="MasterID"]').val(ModuleData.戶長SN);
                    $('input[name="MemberID"]').val(ModuleData.會員編號);
                    //重設 Select Box Family Members
                    $('#SF_EPD_p1').find('option').remove();
                    $('#SF_EPD_p1').append($('<option value=""></option>'));
                    $.each(ModuleData.OptionMember, function (index, items) {
                        $('#SF_EPD_p1').append(
										$('<option value="' + items.SN + '">' + items.NAME + '</option>')
									);
                    });
                }

                if (jsonObject.message != '')
                    alert(jsonObject.message)

            })
        }

        //
        $.ajax_GetSessionOrderHead = function () {

            $.EventAjaxHandle({}, '../../../Logic/ajax_GetSessionOrderHead')
            .done(function (data, textStatus, jqXHR) {
                var jsonObject = jQuery.parseJSON(data);

                if (jsonObject.result) {
                    var ModuleData = jsonObject.Module;

                    $('#p1').val(ModuleData.姓名);
                    if (ModuleData.性別 == '男') $('#p2A').attr('checked', true);
                    if (ModuleData.性別 == '女') $('#p2B').attr('checked', true);

                    $('#p3').val(ModuleData.Zip);
                    $('#p4').val(ModuleData.縣市 + ModuleData.鄉鎮 + ModuleData.地址);
                    $('#p5').val(ModuleData.手機);
                    $('#p6').val(ModuleData.電話);

                    $('input[name="MasterID"]').val(ModuleData.戶長SN);
                    $('input[name="MemberID"]').val(ModuleData.會員編號);
                    //重設 Select Box Family Members
                    $('#SF_EPD_p1').find('option').remove();
                    $('#SF_EPD_p1').append($('<option value=""></option>'));
                    $.each(ModuleData.OptionMember, function (index, items) {
                        $('#SF_EPD_p1').append(
										$('<option value="' + items.SN + '">' + items.NAME + '</option>')
									);
                    });
                }

                if (jsonObject.message != '')
                    alert(jsonObject.message)
            })
        }
    })(jQuery);
</script>

<script type="text/javascript">
    //取得訂購項目清單
    $.ReFreshDetailHTML = function () {
        ajaxRequest.push
            (
		        $.ajax
		        (
			        {
			            url: '../../../Logic/ajax_GetSessionOrderItems',
			            type: "POST",
			            datatype: "json",
			            contentType: "application/x-www-form-urlencoded; charset=utf-8",
			            data: {},
			            cache: "false",
			            global: true,
			            error: function (xhr) { alert('非同步理發生錯誤，請聯絡系統管理員。'); },
			            success: function (response) {

			                var car = jQuery.parseJSON(response);

			                $('tbody', $('#TableProductDetailView')).html(''); //first clear

			                for (var i = 0; i < car.Item.length; i++) {

			                    $('tbody', $('#TableProductDetailView'))
                                .append
                                (


                                        $('<tr class="TableBodyTR">')
											.append($('<td class="TableBodyTD">').html(car.Item[i].姓名))
                                            .append($('<td class="TableBodyTD">').html(car.Item[i].產品名稱))
                                            .append($('<td class="TableBodyTdNum">').html(car.Item[i].價格))
                                            .append($('<td class="TableBodyTD">').append(
																					$('<input>').attr(
																					{
																					    type: 'button', value: '檢視',
																					    memberId: car.Item[i].會員編號,
																					    prodId: car.Item[i].產品編號
																					}).addClass('ViewSessionItem'))
											 )
                                            .append($('<td class="TableBodyTD">').append(
																					$('<input>').attr(
																					{
																					    type: 'button', value: '刪除',
																					    memberId: car.Item[i].會員編號,
																					    prodId: car.Item[i].產品編號
																					}).addClass('RemoveSessionItem'))
											 )
			                    )
			                }

			                $('tbody', $('#TableProductDetailView')).append(
                                $('<tr class="TableBodyTR">')
                                            .append($('<td colspan="2" class="TableBodyTD" align="right">').html('總計：'))
                                            .append($('<td class="TableBodyTdNum" align="right">').html(car.總額))
                                            .append($('<td class="TableBodyTD" colspan="3">').html('　'))
                            );
			            }
			        }
		        )
	        )
    };
</script>
