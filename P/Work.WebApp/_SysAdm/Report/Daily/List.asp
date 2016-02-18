<% Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/Power_Function.asp"-->
<%

Dim strScript,strPageNo
Dim strQS
Dim strWhere,strSql,oRs
Dim strRs
Dim aryDel,aryIns

Dim strDate1,strDate2
Dim strTime1,strTime2,strTime1Temp,strTime2Temp
Dim strDateTime1,strDateTime2
Dim intPeople
Dim strkey

	'-- UTF-8 偵錯時,要先定義網頁顯示編碼,才可正常顯示不會有亂碼
	Response.Write "<html><head><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""></head>"
	'=====================================================
	
	
	'-- 取得前頁轉送相關參數------------------------------
	strScript=Request.ServerVariables("SCRIPT_NAME")
	strPageNo=Trim(Request("pageno"))

	strDate1=Trim(Request("Date1"))
	strDate2=Trim(Request("Date2"))
	strTime1=Trim(Request("Time1"))
	strTime2=Trim(Request("Time2"))
	intPeople=Trim(Request("People"))
	
	
	strKey=Trim(Request("strKey"))
	
	'-- 預設日期 今天,目前只可查單天
	If strDate1="" Then strDate1=Date()
	If strDate2="" Then strDate2=Date()
	
	'-- 將 MM,DD 補0
	strDate1=Year(strDate1) & "-" & STRING(2-LEN(Month(strDate1)),"0") & Month(strDate1) & "-" & STRING(2-LEN(Day(strDate1)),"0") & Day(strDate1)
	strDate2=Year(strDate2) & "-" & STRING(2-LEN(Month(strDate2)),"0") & Month(strDate2) & "-" & STRING(2-LEN(Day(strDate2)),"0") & Day(strDate2)

	
	'-- 預設時間 早上8 下午16:00
	If strTime1="" Then strTime1="01"
	If strTime2="" Then strTime2="24"
	
	'-- 這兩個值有加工過,是要做查詢用
	If strTime1 <> "" Then strTime1Temp=strTime1 & ":00:00"
	If strTime2 <> "" Then strTime2Temp=strTime2 & ":00:00"
	
	'-- 組合日期+ " " +時間
	strDateTime1=strDate1 & " " & strTime1Temp
	strDateTime2=strDate2 & " " & strTime2Temp
	'RESPONSE.WRITE strDateTime1 & "<HR>" 
	'RESPONSE.WRITE strDateTime2 & "<HR>" 
	'================================
	
	'-- 如果第一次進來或不是數字 就帶Session("ID")
	If Not IsNumeric(intPeople) then intPeople=Session("ID") Else intPeople=CLNG(intPeople)
	'================================
	
	strQS="Date1="&strDate1&"&Date2="&strDate2&"&Time1="&strTime1&"&Time2="&strTime2&"&People="&intPeople&"&strkey="&strKey
	'RESPONSE.WRITE strQS & "<HR>"
	'RESPONSE.END
	'======================================================
	
	
	'-- 加入查詢條件---------------------------------------
	strWhere=""
	strWhere=MakeWhere(strWhere,"AND","A.新增人員","=",intPeople,-1,"N","","")
	
	'-- CONVERT(NVARCHAR(19),A.新增時間,120) YYYY-MM-DD HH:MM:SS
	strWhere=MakeWhere(strWhere,"AND","CONVERT(NVARCHAR(19),A.新增時間,120)",">=",strDateTime1,"","S","","")
	strWhere=MakeWhere(strWhere,"AND","CONVERT(NVARCHAR(19),A.新增時間,120)","<=",strDateTime2,"","S","","")

	'-- 關鍵字查詢---------------------------------------
	'strWhere=MakeWhereKW(strWhere,"AND",Array(""),"%LIKE%",strKey,"","S","","")
	'======================================================

	strWhere=MakeWhereEnd(strWhere)
	'RESPONSE.WRITE strWhere & "<HR>"
	'RESPONSE.END
	'======================================================

	'-- 組合查詢字串---------------------------------------
	StrSql= " Select "
	StrSql=StrSql & " A.產品編號, "
	StrSql=StrSql & " A.產品名稱, " 
	StrSql=StrSql & " SUM(A.數量金額) AS 數量金額 , "
	'StrSql=StrSql & " A.新增時間, "
	'StrSql=StrSql & " A.新增人員, "
	StrSql=StrSql & " A.新增人員姓名,"	
	StrSql=StrSql & " A.產品排序"
	StrSql=StrSql & " FROM VW_每日統計表 AS A"
	StrSql=StrSql & StrWhere
	StrSql=StrSql & " GROUP BY A.產品編號,A.產品名稱,A.產品排序,A.新增人員姓名 "
	StrSql=StrSql & " ORDER BY A.產品排序 "

    'RESPONSE.WRITE StrSql  & "<HR>"
	'RESPONSE.END
	'======================================================

	'-- 取得資料錄-----------------------------------------
	Set oRS=ExecSQL_RTN_RST(StrSql,3,1,2)
	'======================================================
	'-- 組合表單-------------------------------------------
	aryIns=NULL
	aryDel=NULL
	
	strRS=RsToTable_SelectAll(oRs,"del.asp",strQS,0,0,"print.asp?pageno="&request("pageno")&"&"&strQS&"&p0=","",30,"統計表查詢列印","99%", _
			Array("10%","9%","7%","8%","5%","7%","7%","10%","5%","10%","10%","9%"), _
			aryDel,0,"",0,"../../_Images/_pager/", _
			aryIns,"OpenWindow_No","HighLight_Yes","SelectAll_Yes")	
	'======================================================
	
	
	'-- 取得金額加總---------------------------------------
	Dim LngAmountTotal1,LngAmountTotal2,LngAmountTotal3
	Dim strAmountShow
	
	IF oRS.RecordCount > 0 Then oRS.MoveFirst		'-- 移到第一筆
	
	LngAmountTotal1=0	'-- 初始化
	LngAmountTotal2=0	'-- 初始化
	LngAmountTotal3=0	'-- 初始化
	While Not oRs.Eof
		SELECT CASE oRs("產品編號") 
		
			CASE 2,21,3,31,4,41,5,51,6,7,701,702,703,704,751,752,753,760,8,9,12,121,13,131,380,381,382,383,384,761,762,707,706,763,764,765,766'文昌燈,媽祖燈,觀音燈,關聖燈,安太歲,香油,入斗,保運,財神燈,姻緣燈
				LngAmountTotal1=LngAmountTotal1+oRs("數量金額") 
				
			CASE 10	'金牌
				LngAmountTotal2=LngAmountTotal2+oRs("數量金額") 
		
			CASE 11	'白米
				LngAmountTotal3=LngAmountTotal3+oRs("數量金額") 			
		End SELECT
		oRs.MoveNext
	Wend
	strAmountShow = "時間區間總金額：" & LngAmountTotal1 & "元　　"
	strAmountShow = strAmountShow & "金牌：" & LngAmountTotal2 & "面　　"
	strAmountShow = strAmountShow & "白米：" & LngAmountTotal3 & "斤　　"
	'=====================================================
	

	Dim strSelCboTime1 '時間1
	'-- 取得時間1-------------------------------
	strSelCboTime1=AryToOption(Array(01,02,03,04,05,06,07,08,09,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24),Array("01","02","03","04","05","06","07","08","09","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24"),1,1,Trim(strTime1),"","")
	''-- 取得一般分類代碼資料-------------------------------
	'Set oRs=ExecSQL_RTN_RST("SELECT 分類代碼,分類名稱 FROM 一般分類 Where 表格代碼 = 110 ORDER BY 排序",2,0,1)
	'strSelCboTime1=RsToOption(oRs,0,1,trim(Time1),"","")			
	'======================================================
	
	Dim strSelCboTime2 '時間2
	'-- 取得時間2-------------------------------
	strSelCboTime2=AryToOption(Array(01,02,03,04,05,06,07,08,09,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24),Array("01","02","03","04","05","06","07","08","09","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24"),1,1,Trim(strTime2),"","")
	''-- 取得一般分類代碼資料-------------------------------
	'Set oRs=ExecSQL_RTN_RST("SELECT 分類代碼,分類名稱 FROM 一般分類 Where 表格代碼 = 110 ORDER BY 排序",2,0,1)
	'strSelCboTime2=RsToOption(oRs,0,1,trim(Time1),"","")			
	'======================================================

	'-- 取得人員列表----------------------------
	Dim strSelCboPeople
	IF Session("UserKind") = 99 Then
		'-- 如果管理人員,可以看所有人(查詢其他人)
		StrSql="SELECT 人員代碼,姓名 FROM 人員 order by 帳號"
	ELSE
		'-- 如果是一般人員,可以看自己
		StrSql="SELECT 人員代碼,姓名 FROM 人員 WHERE 人員代碼 = " & intPeople
	End IF	
	Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	strSelCboPeople = RsToOption(oRs,0,1,Cstr(intPeople),"","")	
	'===========================================

	
	'-- 釋放物件-------------------------------------------
	ReleaseObj(Array(oRs))
	'======================================================

%>
<html>
<head>
    <title>統計表查詢</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="../../_CSS/default.css" />
    <link rel="Stylesheet" href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.23/themes/redmond/jquery-ui.css"
        type="text/css" />
    <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.1.min.js"></script>
    <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.23/jquery-ui.min.js"></script>
    <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.9/jquery.validate.min.js"></script>

    <script type="text/javascript" src="../../../_Code/JQScript/jquery.ui.datepicker-zh-TW.js"></script>
    <script type="text/javascript" src="http://malsup.github.com/jquery.form.js"></script>
    <script type="text/javascript" src="../../../_Code/JQScript/CommFunc.js"></script>
    <script type="text/ecmascript">

        $(document).ready(function () {

            var submitflag = 0;

            $('#btn_ExportExcel').click(function () {
                //submitflag = 0;
                //var org_action = $('#form1').attr("action");
                //$('#form1').attr("action", "../../../ExcelReport/ajax_MakeExcel");
                //$('#form1').submit();
                //$('#form1').attr("action", org_action);
                var parms = [];
                parms.push('Date1=' + $('#Date1').val());
                parms.push('Date2=' + $('#Date2').val());
                parms.push('Time1=' + $('#Time1').val());
                parms.push('Time2=' + $('#Time2').val());
                parms.push('People=' + $('#People').val());

                $("#ifm_exceldownload").attr("src", "../../../ExcelReport/ajax_MakeExcel?" + parms.join('&'));
            });

            $('#btn_submit').click(function () {
                submitflag = 1;
                $('#form1').submit();
            })

            $('#form1').submit(function () {
                if (submitflag == 0) {
                    $("#form1").ajaxSubmit(options);
                    return false;
                } else {
                    return true;
                }
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

            function showResponse(jsonobj, statusText, xhr, $form) {

                if (jsonobj.result) {
                    $("#ifm_exceldownload").attr("src",
                    "../../../ExcelReport/ajax_GetFile?FileName=" + jsonobj.filesObject[0].FileName + "&WebPath=" + jsonobj.filesObject[0].OriginFilePath);
                }

                if (jsonobj.message != '')
                    alert(jsonobj.message);
            }

            $("#Date1").datepicker();
            $("#Date2").datepicker();
        })
    </script>
    <style type="text/css">
        .gridtd1 {
            color: black;
            text-align: center;
        }

        .gridtd2 {
            color: black;
            text-align: center;
        }

        .gridtd3 {
            color: black;
            text-align: center;
        }

        .gridtd4 {
            color: black;
            text-align: center;
        }

        .gridtd5 {
            color: black;
            text-align: center;
        }

        .gridtd6 {
            color: black;
            text-align: center;
        }

        .gridtd7 {
            color: black;
            text-align: center;
        }
    </style>
</head>
<body>
    <!--#include file="../../_include/top.asp"-->
    <form action="<%=strScript%>" method="post" name="traQuery" id="form1">
        <input name="cmd" type="hidden" id="cmd" value="set" />
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td align="left">
                    <table width="800" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="240" align="left">訂單日期：起：<input name="Date1" type="text" id="Date1" value="<%=strDate1%>" style="width: 30%"
                                readonly="readonly" />
                                <select name="Time1" id="Time1" style="width: 20%; cursor: pointer;">
                                    <%=strSelCboTime1%>
                                </select>~
                            </td>
                            <td width="180" align="Left">迄：<input name="Date2" type="text" id="Date2" value="<%=strDate2%>" style="width: 40%"
                                readonly="readonly" />
                                <select name="Time2" id="Time2" style="width: 30%; cursor:pointer;">
                                    <%=strSelCboTime2%>
                                </select>
                            </td>
                            <td width="80" align="Right">經手人員：
                            </td>
                            <td width="80" align="Left">
                                <select name="People" id="People" style="width: 100%; cursor: pointer;">
                                    <%=strSelCboPeople%>
                                </select>
                            </td>
                            <!--
				<td>關鍵字:</td>
				<td><input type="text" size=10 name="key" value="<%=strKey%>"></td>
				-->
                            <td style="width: 50px; text-align: center">
                                <input type="button" id="btn_submit" value="查　詢" class="button" />
                            </td>
                            <td style="width: 80px; text-align: center">
                                <input type="button" id="btn_ExportExcel" class="button" value="產生統計表" style="margin-right: 10px;" />
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td align="Left" colspan="5">
                                <font color="RED"><%=strAmountShow%></font><span style="color: red"><a href="../../onlineservice/order/信任網站.doc" target="new">(統計表打不開?)</a></span>
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
    <%=strRs%>
    <iframe id="ifm_exceldownload" src="" style="border: 0px; width: 0px; height: 0px"></iframe>
    <!--#include file="../../_include/bottom.asp"-->
</body>

</html>
