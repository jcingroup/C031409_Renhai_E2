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
Dim intStepSN			'文疏梯次
Dim strkey



	'-- UTF-8 偵錯時,要先定義網頁顯示編碼,才可正常顯示不會有亂碼
	Response.Write "<html><head><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""></head>"
	'=====================================================
	
	
	'-- 取得前頁轉送相關參數------------------------------
	strScript=Request.ServerVariables("SCRIPT_NAME")
	strPageNo=Trim(Request("pageno"))
	
	strDate1=Trim(Request("Date1"))
	strDate2=Trim(Request("Date2"))
	intStepSN=Trim(Request("StepSN"))
	
	
	strKey=Trim(Request("strKey"))
	
	'-- 預設日期 今天,目前只可查單天
	'If strDate1="" Then strDate1=Year(Date()) & "/01/01"
	'If strDate2="" Then strDate2=Date()
	'================================
	
	If Not IsNumeric(intStepSN) then intStepSN=-1 Else intStepSN=CLNG(intStepSN)

	strQS="Date1="&strDate1&"&Date2="&strDate2&"&StepSN="&intStepSN&"&strkey="&strKey
	'RESPONSE.WRITE strQS & "<HR>"
	'RESPONSE.END
	'======================================================
	
	
	'-- 加入查詢條件---------------------------------------
	strWhere=""
	strWhere=MakeWhere(strWhere,"AND","A.序號","=",intStepSN,-1,"N","","")
	'strWhere=MakeWhere(strWhere,"AND","B.新增人員","=",intPeople,-1,"N","","")
	'strWhere=MakeWhere(strWhere,"AND","CONVERT(NVARCHAR(10),B.新增時間,111)",">=",strDate1,"","S","","")
	'strWhere=MakeWhere(strWhere,"AND","CONVERT(NVARCHAR(10),B.新增時間,111)","<=",strDate2,"","S","","")

	strWhere=MakeWhereEnd(strWhere)
	'RESPONSE.WRITE strWhere & "<HR>"
	'RESPONSE.END
	'======================================================

	'-- 組合查詢字串---------------------------------------
	StrSql= " Select DISTINCT "
	StrSql=StrSql & " A.序號, "
	StrSql=StrSql & " A.梯次, "
	StrSql=StrSql & " CONVERT(NVARCHAR(10),A.時間,111) AS 梯次時間_國, "
	StrSql=StrSql & " A.農曆年 + '/' + A.農曆月 + '/' + A.農曆日  AS 梯次時間_農, "
	StrSql=StrSql & " B.訂單編號, "
	StrSql=StrSql & " B.戶長姓名, "
	StrSql=StrSql & " B.戶長地址, "
	'StrSql=StrSql & " (Select Count(*) FROM 訂單主檔 WHERE 訂單編號=B.訂單編號 ) AS 戶數, "
	StrSql=StrSql & " (Select Count(*) FROM 訂單明細檔 WHERE 訂單編號=B.訂單編號 AND 文疏梯次 > 0) AS 每戶人數 "
	'StrSql=StrSql & " CONVERT(NVARCHAR(10),B.新增時間,111) AS 新增時間, "
	'StrSql=StrSql & " (Select Count(*) FROM 訂單明細檔 WHERE 產品編號 = 9 AND 訂單編號=B.訂單編號) AS 保運人數      "
	StrSql=StrSql & " FROM 文疏梯次時間表 AS A "
	StrSql=StrSql & " Left Join ("
	
	StrSql=StrSql & " Select DISTINCT "
	StrSql=StrSql & " 	X1.訂單編號,X1.申請人姓名 AS 戶長姓名 ,X1.申請人地址 AS 戶長地址,"
	'StrSql=StrSql & "	X2.申請人姓名 , "
	StrSql=StrSql & "	X2.文疏梯次 "
	StrSql=StrSql & " 	FROM 訂單主檔 AS X1 "
	StrSql=StrSql & " 		Left Join 訂單明細檔 AS X2 ON X2.訂單編號=X1.訂單編號 "
	StrSql=StrSql & " 	WHERE X2.文疏梯次 > 0 and 年度=(SELECT TOP 1 apply_year FROM ApplyParam) "
	
	StrSql=StrSql & " 	) AS B ON B.文疏梯次=A.序號 "
	StrSql=StrSql & StrWhere
	StrSql=StrSql & " ORDER BY A.序號 "
	
    'RESPONSE.WRITE StrSql  & "<HR>"
	'RESPONSE.END
	'======================================================

	'-- 取得資料錄-----------------------------------------
	Set oRS=ExecSQL_RTN_RST(StrSql,3,1,2)
	'======================================================
	'-- 組合表單-------------------------------------------
	aryIns=NULL
	aryDel=NULL
	
	strRS=RsToTable_SelectAll(oRs,"del.asp",strQS,0,0,"print.asp?pageno="&request("pageno")&"&"&strQS&"&p0=","",10,"保運文疏","99%", _
			Array("10%","10%","10%","10%","10%","30%","10%","10%"), _
			aryDel,0,"",1,"../../_Images/_pager/", _
			aryIns,"OpenWindow_No","HighLight_Yes","SelectAll_Yes")	
	'======================================================

	'-- 取得文疏梯次時間表基本資料-----------------------------------
	Dim strSelCboStepSN
	StrSql= "SELECT A.序號,"
	StrSql=StrSql & " (A.梯次 + '__' + CAST( (Select Count(*) FROM 訂單明細檔 AS X WHERE X.文疏梯次=A.序號 and 年度=(SELECT TOP 1 apply_year FROM ApplyParam)) AS NVARCHAR(10))) AS 梯次 "
	StrSql=StrSql & " FROM 文疏梯次時間表 AS A "
	StrSql=StrSql & " ORDER BY A.序號 "
	'RESPONSE.WRITE StrSql & "<BR>"
	
	Set oRs=ExecSQL_RTN_RST(StrSql,2,0,1)
	strSelCboStepSN=RsToOption(oRs,0,1,trim(intStepSN),"","")			
	'======================================================
	
	'-- 釋放物件-------------------------------------------
	ReleaseObj(Array(oRs))
	'======================================================

%>


<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<title>保運文疏</title>
	<link rel=stylesheet href=../../_CSS/default.css>
	<script language=javascript src="../../_JScript/List.js"></script>
	<script language=javascript src="../../_JScript/Subwin.js"></script>
	<script language=javascript src="../../_JScript/Calndr.js"></script>
	<script Language=javascript>
	
	//打開視窗-------------
	function WO(URL){
		window.open(URL,"WO","Left=0,Top=0,width=1180,height=750,center=yes,status=no,toolbar=no,scrollbars=yes");
	}//================================
	</script>
	
	<style type="text/css">
		.gridtd1{color:black;text-align:center}
		.gridtd2{color:black;text-align:center}	
		.gridtd3{color:black;text-align:center}	
		.gridtd4{color:black;text-align:center}
		.gridtd5{color:black;text-align:center}
		.gridtd6{color:black;text-align:Left}
		.gridtd7{color:black;text-align:center}
		.gridtd8{color:black;text-align:center}

	</style>
</head>
<body>
<!--#include file="../../_include/top.asp"-->
<form action="<%=strScript%>" method="post" name=traQuery id="form1">
<input name="cmd" type="hidden" id="cmd" value="set">
<table width=100% border=0  cellspacing="0" cellpadding="0">
	<tr>
	<td Align=Left> 
	
		<table wwidth=800 border=0  cellspacing="0" cellpadding="0">
			<tr> 
				<!--
				<td width="80" Align=Right>列印日期：</td>
				<td width="20"  Align=Right>起</td>
				<td width="80" Align=Right><input name="Date1" type="text" id="Date1" value="<%=strDate1%>" onclick="ShowCalendar();" style="width:100%;Cursor:Hand;" ReadOnly ></td>
				<td width="20"  Align=Right>迄</td>
				<td width="80" Align=Right><input name="Date2" type="text" id="Date2" value="<%=strDate2%>" onclick="ShowCalendar();" style="width:100%;Cursor:Hand;"></td>
				-->
				
				<td width="80" Align=Right>文疏梯次：</td>
				<td width="150" Align=Left><select name="StepSN" style="width:100%;Cursor:Hand;"><option></option><%=strSelCboStepSN%></select> </td>
				

				<!--
				<td>關鍵字:</td>
				<td><input type="text" size=10 name="key" value="<%=strKey%>"></td>
				-->
				<td width=80 Align=Right><input type="submit" value="查　詢" class=button></td>
				
				<% IF intStepSN > 0 Then %>
				<td width=150 Align=Right><input type="button" class="button" OnClick="WO('ExcelExport.asp?<%=strQS%>')" value="列印保運文疏" style="margin-right:10px;"></td>
				<%End IF%>
				<td >&nbsp;</td>

			</tr>
			
			
        </table>
	</td>
	</tr>
</table>	
</form>

<%=strRs%>
<!--#include file="../../_include/bottom.asp"-->
</body>
</html>