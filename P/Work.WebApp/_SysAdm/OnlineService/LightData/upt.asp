<% Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/Power_Function.asp"-->
<%
On Error Resume Next
Dim StrSql,oRs
Dim WhereObj,strwhere
Dim strQS,strScript,strPageNo,EditType
Dim strReturn,strRS
Dim aryIns,aryDel
Dim p0,p1,p2,p3,p4
Dim p1Option
Dim MasterId

	MasterId = Request.QueryString("p0")

	If MasterId = "" Then
		EditType = "Insert"	'新增模式
		MasterId = GetSysID("產品基本資料")
	Else
		EditType = "Modify"	'修改模式

        StrSql="SELECT * FROM 產品資料表 Where 產品編號=" & MasterId
	    Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	    AspErrCheck "產品資料表 取得發生錯誤"

	    p1 = oRs("產品名稱")
	    p2 = oRs("標籤")
	    p3 = oRs("價格")
	    p4 = oRs("位置")
	End If
'=========================

	'StrSql="SELECT 鄉鎮,鄉鎮 FROM 地址鄉鎮 Where 縣市='" & p13 & "' order by 排序 "
	'Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	'p14Option = RsToOption(oRs,0,1,Cstr(p14),"","")
	'AspErrCheck "p14Option"	
	
	'-- 回上一頁
	strQS="MasterId=" & MasterId & "&pageno=" & strPageNo
	strReturn="<a href=""list.asp?"&strQS&"&pageno="&strPageNo&""">←回上一頁</a>"	
%>
<html>
<head>
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <link rel="stylesheet" href="../../_CSS/default.css">
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
    </style>
</head>
<body>
    <input type="button" value="回上一頁" onclick="history.back()">
    <!--#include file="../../_include/top2.asp"-->
    <form id="frmData" name="frmData" action="../../../Logic/ajax_LightData_date" method="post">
    <input type="hidden" id="EditType" name="EditType" value="<%=EditType %>">
    <input type="hidden" name="cmd" value="set">
    <input type="hidden" id="p0" name="p0" value="<%=MasterId%>">
    <table id="EditDataTable" class="gridtable" border="0">
        <tr>
            <td class="gridcaption style:margin:0">
                <table style="width: 100%">
                    <tr>
                        <td class="gridtdcaption" id="CaptionText">
                            點燈資料管理
                        </td>
                        <td style="text-align: center">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top">
                <table style="width: 100%" border="0">
                    <tr>
                        <th style="width: 120px">
                        </th>
                        <td style="width: 120px">
                        </td>
                        <th style="width: 120px">
                        </th>
                        <td style="width: 210px">
                        </td>
                    </tr>
                    <tr>
                        <th style="text-align: right">
                            項目編號：
                        </th>
                        <td>
                            <input type="text" name="產品編號" id="MasterId" value="<%=MasterId%>" maxlength="20"
                                size="8" />
                        </td>
                        <th style="text-align: right">
                            項目名稱：
                        </th>
                        <td>
                            <input type="text" name="產品名稱" id="p1" value="<%=p1%>" maxlength="16" size="16" />
                            <span style="color: Red">*例：媽祖壁燈(甲)。</span>
                        </td>
                    </tr>
                    <tr>
                        <th style="text-align: right">
                            點燈標籤：
                        </th>
                        <td>
                            <input type="text" name="標籤" id="p2" value="<%=p2%>" maxlength="16" size="16" />
                            <span style="color: Red">*例：媽壁甲。</span>
                        </td>
                        <th style="text-align: right">
                            價格：
                        </th>
                        <td>
                            <input type="text" name="價格" id="p3" value="<%=p3%>" maxlength="5" size="5" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <input type="button" id="btn_planA_show" style="width: 100%" value="產生燈座資料">
                        </td>
                    </tr>
                    <tr>
                        <td class="gridtdtool" colspan="4" style="text-align: center">
                            <span class="errmsg">
                                <%=strMsg%></span><br />
                            <input id="btn_submit" name="btn_submit" class="gridsubmit" type="button" value="確認"
                                style="cursor: Hand" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
    <div id="div_planA" style="display: none; width: 650px; height: 320px; background-color: #333333">
        <input type="button" value="關　閉" id="btn_planA_close" />
    </div>
    <div id="dialog" title="" style="display: none;">
    </div>
    <div id="wait" style="display: none; width: 200px; height: 25px; line-height: 25px;
        text-align: center; color: #009900; font-size: 12px; border: 1px solid #ccc;
        background: #f8f8f8; position: absolute; padding: 2px;">
        資料讀取中，請稍侯．．．
    </div>
</body>
</html>
<script type="text/javascript">
    $(document).ready(function () {

        $('#btn_submit').click(function () {
            ajaxHasDone = $.when.apply($, ajaxRequest);
            ajaxHasDone.done(function () {
                if ($('#frmData').valid()) {  // for ( instance in CKEDITOR.instances ) CKEDITOR.instances[instance].updateElement();
                    $('#frmData').submit();
                }
            });
        });

        $('#btn_return_list').click(function () { document.location.href = 'ListGrid?' + $('#returnQueryString').val(); });

        var options = {
            beforeSubmit: AjaxFormBeforeSubmit,
            success: AjaxFormSuccess,
            dataType: 'json',
            contentType: 'application/x-www-form-urlencoded; charset=utf-8'
        };
        function AjaxFormBeforeSubmit(formData, jqForm, options) { var queryString = $.param(formData); return true; };

        function AjaxFormSuccess(jsonobj, statusText, xhr, $form) {
            if (jsonobj.result) {
                //$('#id').val(jsonobj.id);
                $('#EditType').val('Modify');
            }

            $('#dialog').html(jsonobj.message);
            $('#dialog').dialog({ title: jsonobj.title });
            $('#dialog').dialog();
        };

        $('#frmData').validate({
            errorClass: 'error',
            validClass: 'valid',
            errorElement: 'label',
            groups: {},
            rules: {
                MasterId: { required: true },
                p1: { required: true },
                p2: { required: true },
                p3: { required: true, digits: true }
            },
            messages: {
                MasterId: {},
                p1: {},
                p2: {},
                p3: {}
            }
        });

        $('#frmData').submit(function () {
            $(this).ajaxSubmit(options); return false;
        });
    })
</script>
<script type="text/javascript">    //開啟 div_planA
    $(document).ready(function () {
        $('#btn_planA_show').click(function () {
            $('#div_planA').show();
        })

        $('#btn_planA_close').click(function () {
            $('#div_planA').hide();
        })
    })
</script>
<script type="text/javascript">    //顯示讀取資料中..... Show and Hide

    $('#div_planA').center();

    $("#wait").center();

    $("#wait").ajaxStart(function () {
        $("#wait").css("display", "block");
    });

    $("#wait").ajaxComplete(function () {
        $("#wait").css("display", "none");
    });
    //讀取資料 Show and Hide
    $("#wait").center();

    $("#wait").ajaxStart(function () {
        $("#wait").css("display", "block");
    });

    $("#wait").ajaxComplete(function () {
        $("#wait").css("display", "none");
    });

</script>
