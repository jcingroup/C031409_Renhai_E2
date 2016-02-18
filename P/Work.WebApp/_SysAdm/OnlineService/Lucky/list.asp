<% Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/Power_Function.asp"-->
<%
Dim oRs,strRs
Dim strScript,strPageNo,strQS
Dim strCon,intCon
Dim strUID,intUID,strUnit
Dim strCID,intCID,strCat
Dim strSDate,strEDate
Dim strKey
Dim aryDel,aryIns
Dim strSql,strWhere,strWhere2
Dim p0
Dim s1,s2,s3,s4,s5,s6
	
	strScript=Request.ServerVariables("SCRIPT_NAME")
	strPageNo=Trim(Request("pageno"))
	
	'-- 加入查詢條件---------------------------------------
	strWhere=""
	strWhere=MakeWhereEnd(strWhere)
	
	'-- 組合查詢字串---------------------------------------
	StrSql=" Select 序號,序號,生肖,星運 "
	StrSql=StrSql & " From 星運表 "
	StrSql=StrSql & strwhere 
	StrSql=StrSql & "Order By 序號"

	'======================================================
	'-- 取得資料錄-----------------------------------------
	Set oRS=ExecSQL_RTN_RST(StrSql,3,1,2)
	'======================================================
	'-- 組合表單-------------------------------------------
	'aryIns=Array("新增訂單","window.location.href='ins.asp'")
	'aryDel=Array("刪除")
	strRS=RsToTable_SelectAll(oRs,"del.asp",strQS,0,2,"upt.asp?pageno="&request("pageno")&"&"&strQS&"p0=","",12,"<div align=left>星運表管理-每年印製保運(益壽延年文疏)前需調整成新的)</div>","99%", _
			Array("10%","20%","40%"), _
			aryDel,0,"",1,"../../_Images/_pager/", _
			aryIns,"OpenWindow_No","HighLight_Yes","SelectAll_Yes")
	'======================================================
	
	'-- 釋放物件-------------------------------------------
	oRs.Close
	Set oRs=Nothing
	'======================================================
%>

<html>
<head>
	<title></title>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<link rel=stylesheet href=../../_CSS/default.css>
	<script language=javascript src="../../_JScript/Subwin.js"></script>
	<script language=javascript src="../../_JScript/List.js"></script>
	<style type="text/css">
		.gridtd1{color:black;text-align:center}
		.gridtd2{color:black;text-align:center}	
		.gridtd3{color:black;text-align:center}	
		.gridtd4{color:black;text-align:center}
		.gridtd5{color:black;text-align:center}
		.gridtd6{color:black;text-align:center}
		.gridtd7{color:black;text-align:center}
		.gridtd8{color:black;text-align:center}
		.gridtd9{color:black;text-align:center}
		.gridtd10{color:black;text-align:center}
	</style>
</head>
<body>

	
<form action="<%=strScript%>" method="post" name=ins id="form1">
<table width=98% border=0  cellspacing="0" cellpadding="0">
	<tr>
		<td> 
			<table width="100%" border=0  cellspacing="0" cellpadding="1">

			</table>
		</td>
	</tr>
</table>	
</form>
<%=strRs%>
</body>
</html>