<% Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/Power_Function.asp"-->
<%
On Error Resume Next
Dim StrSql,oRs
Dim WhereObj,strwhere
Dim strQS,strScript,strPageNo,Uptmode
Dim strReturn,strRS
Dim aryIns,aryDel
Dim p0,p1,p2,p3,p4,p5,p6

	p0 = Request.QueryString("p0")
	Uptmode = "Update"	'修改模式

'=========================
	StrSql="SELECT * FROM 文疏梯次時間表 Where 序號=" & p0
	Set oRs = ExecSQL_RTN_RST(StrSql,3,0,1)

	p1 = oRs("梯次")
	p2 = Year(oRs.Fields("時間").Value) & "/" & Month(oRs.Fields("時間").Value) & "/" & Day(oRs.Fields("時間").Value):AspErrCheck "oRs.時間"
	p3 = oRs("農曆年")	
	p4 = oRs("農曆月")	
	p5 = oRs("農曆日")
	'-- 回上一頁
	strQS="p0=" & p0 & "&pageno=" & strPageNo
	strReturn="<a href=""list.asp?"&strQS&"&pageno="&strPageNo&""">→ 回上一頁</a>"
%>
<html>
<head>
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <link rel="stylesheet" href="../../_CSS/default.css">
    <script type="text/javascript" src="lunar.js"></script>
    <style>
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
    <!-- 這一段要放在HEAD裡面 -->
</head>
<body>
    <%=strReturn %>
    
    <!--#include file="../../_include/top.asp"-->

    <form id="myform" name="myform" action="ajax.myform.submit.asp" method="post">
    <input type="hidden" id="Uptmode" name="Uptmode" value="">
    <input type="hidden" name="cmd" value="set">
    <input type="hidden" name="p0" value="<%=p0%>">
    <table id="EditDataTable" class="gridtable" width="99%" border="1" cellspacing="0"
        cellpadding="0">
        <caption class="gridcaption style:margin:0">
            <table width="100%">
                <tr>
                    <td class="gridtdcaption" id="CaptionText">文殊梯次設定</td>
                    <td align="right"></td>
                </tr>
            </table>
        </caption>
        <tr>
            <td valign="top">
                <table width="100%" border="0" cellspacing="1" cellpadding="1">
                    <tr>
                        <th width="90">
                        </th>
                        <td width="210">
                        </td>
                        <th width="120">
                        </th>
                        <td width="180">
                        </td>
                    </tr>
                    <!--<tr><td colspan="4" class="TDLine">基本資料填寫</td></tr>-->
                    <tr>
                        <th align="right">序號：</th><td><%=p0%></td><th align="right">梯次：</th><td><%=p1%></td>
                    </tr>
                    <tr>
                        <th align="right">時間：</th><td><input type="text" name="p2" id="p2" value="<%=p2%>" maxlength="10" size="10" /></td>
                        <th align="right">農曆：</th>
                        <td>
							<input type="text" name="p3" id="p3" value="<%=p3%>" maxlength="3" size="3" />年
							<input type="text" name="p4" id="p4" value="<%=p4%>" maxlength="2" size="2" />月
							<input type="text" name="p5" id="p5" value="<%=p5%>" maxlength="2" size="2" />日
                        </td>
					</tr>
                    <tr>

                    <tr>
                        <td class="gridtdtool" colspan="4" align="Center">
                            <span class="errmsg">
                                <%=strMsg%></span><br />
                            <input id="btn_submit" name="btn_submit" class="gridsubmit" type="button" value="確認" style="cursor: Hand" /><%=strReturn %>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
<script type="text/javascript">
        //$("#p11").datepicker();
        //ajax主要表單設定
        //表單填寫規則設定
	$(document).ready(function () {
		
		$("#p2").datepicker({
			numberOfMonths: [2,2],showButtonPanel: true
		});
		
        $("#myform").validate({
            rules:
			{
			    p2: { required: true },
			    p3: { required: true },
				p4: { required: true },
				p5: { required: true }
			},
            messages:
			{
			}
        });

        $('#btn_submit').click(function () {
            //待所有的ajax完成 才可做儲檔的動作
            ajaxHasDone = $.when.apply($, ajaxRequest);
            ajaxHasDone.done(function () {
				if ($('#myform').valid()) {
                    var FormFlag = false;

					FormFlag = true;

                    if (FormFlag)
                        $('#myform').submit();
                }
            });
        })

        $('#myform').submit(function () {
            $(this).ajaxSubmit(options);
            return false;
        });

        var options = { 
            beforeSubmit: showRequest,
            success: showResponse,
            dataType: 'json'
        };

        function showRequest(formData, jqForm, options) {
            var queryString = $.param(formData);
            return true;
        }

        //表單在Server完成 傳送Data回來，由此function處理
        function showResponse(jsonobj, statusText, xhr, $form) {
            if (jsonobj.result)
                alert('更新完成');
			
			if(jsonobj.message!='')
				alert(jsonobj.message);
        }
		
		$('#p2').change(function(){
			var gDate = $(this).val().split('/');

            Lunar(0, gDate[0], gDate[1], gDate[2]);
			$('#p3').val(lunar.y - 1911);	
			$('#p4').val(lunar.m);
			$('#p5').val(lunar.d);
		});
		
	})

</script>
<script type="text/javascript" language="javascript">
    //顯示讀取資料中..... Show and Hide
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
