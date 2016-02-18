<% Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/Power_Function.asp"-->
<%
Dim p0	' 代碼
Dim p1	' 
Dim p2	' 
Dim strWhere
Dim strScript
Dim oRs
Dim ary
Dim strMsg
Dim strSql
Dim strSql1
	'-- UTF-8 偵錯時,要先定義網頁顯示編碼,才可正常顯示不會有亂碼
	Response.Write "<html><head><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""></head>"
	'=====================================================
	
	'-- 取得編號
	p0=Trim(Request("p0"))
	p2=Trim(Request("p2"))
	'strScript = Request.ServerVariables("SCRIPT_NAME")
	
	'-- 加入查詢條件---------------------------------------
	strWhere=""
	'strWhere=MakeWhere(strWhere,"AND","序號","=",p0,-1,"N","","")	
	strWhere=MakeWhereEnd(strWhere)
	
	StrSql=" Select 序號,生肖,星運 From 星運表 where 序號= " & p0
	'StrSql=StrSql & " 
	'StrSql=StrSql & strwhere 
	'StrSql=StrSql & " Order By 序號 "
	'response.write strsql
	'response.end
	Set oRs = ExecSQL_RTN_RST(StrSql,3,0,1)
	If oRS.RecordCount > 0 Then
		p0 = oRs("序號")
		p1 = oRs("生肖")
		p2 = oRs("星運")
	End If

	'-- 修改-------------------------------------------
	strMsg = ""
	If Trim(Request.Form("cmd")) = "set" Then	
			p2=Trim(Request.Form("p2"))		'	
		If Len(strMsg) = 0 Then
				strSql1="Update 星運表 SET 星運='"&p2&"' WHERE 序號 ='"&p0&"'"
				ExecSql strSql1
				Response.Redirect "list.asp?p0= "
		End If
	End If


%>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<link rel=stylesheet href=../../_CSS/set.css>
	<script language=javascript src=set.js></script>
	<script language=javascript src=../../_JScript/subwin.js></script>
	<title>修改星運</title>	
</head>
<body>
<!--#include file="../../_include/top.asp"-->

	<form action="<%=strScript%>" method=post>
		<input type=hidden name="cmd" value=set>
		<table class=gridtable border=0 width=60% cellspacing=0 cellpadding=3>
			<caption class=gridcaption>修改星運-每年印製保運(益壽延年文疏)前需調整成新的)</caption>
			<tr><th class=gridth><font color=red>＊</font>生肖:<td><input type=text name=p1 size=20 maxlength=20 value="<%=p1%>" readonly></tr>
			<tr><th class=gridth><font color=red>＊</font>星運:<td><input type=text name=p2 size=20 maxlength=20 value="<%=p2%>" ></tr>
		
			<tr>
				<td class=gridtdtool colspan=4>
					<span class=errmsg><%=strMsg%></span>
					<input class=gridsubmit type="button" value="取 消" onclick="history.go(-1);">
					<input class=gridsubmit type=submit value="修 改">
				</td>
			</tr>
		</table>
	</form>

<!--#include file="../../_include/bottom.asp"-->
</body>
</html>