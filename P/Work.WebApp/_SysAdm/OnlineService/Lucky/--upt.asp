<%@ codepage=65001%>
<%Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<%
Dim p0	'流水號
Dim p1	'
Dim p2	'

Dim strScript,strPageNo
Dim strQS,strSql,oRs,strRS
Dim strPrev,strNext,strNew
Dim strSql1,oRs1,strSql2,oRs2,strSql3,oRs3,strSql4,oRs4,strSql5,oRs5,strSql6,oRs6,strSql7,oRs7

Dim strCon,intCon
Dim strHot,intHot
Dim strSel,intSel,strSelCbo
Dim strKey
Dim strMsg
Dim strReturn
Dim strWhere
Dim intSel1

	'------------------------------------
	'-- 取得編號
	p0=Trim(Request("p0"))

	strScript=Request.ServerVariables("SCRIPT_NAME")
	strPageNo=Trim(Request("pageno"))

	'-- 加入查詢條件---------------------------------------
	strWhere=""
	strWhere=MakeWhereEnd(strWhere)
	
	'-- 組合查詢字串---------------------------------------
	StrSql=" Select 序號,序號,生肖,星運 "
	StrSql=StrSql & " From 星運表 "
	StrSql=StrSql & strwhere 
	StrSql=StrSql & " Order By 序號 "

	'-- 取得資料錄-----------------------------------------
	Set oRs=ExecSQL_RTN_RST(StrSql,3,1,2)
	If oRs.RecordCount>0 Then
		p0=Trim(Request("p0"))
		strNext=Trim(getNextF(oRs,0,p0,0))
		oRs.MoveFirst
		strPrev=Trim(getPreF(oRs,0,p0,0))
		If Not IsNull(strNext) Then
			strNext="<a href="""&strScript & "?"&strQS&"&p0="&strNext&"""><img border=0 src=../../_images/forward.gif></a>"
		End If
		If Not IsNull(strPrev) Then
			strPrev="<a href="""&strScript & "?"&strQS&"&p0="&strPrev&"""><img border=0 src=../../_images/back.gif></a>"
		End If
	End if
	'======================================================
	'-- 回上一頁
	strReturn="<a href=""list.asp?"&strQS&"&pageno="&strPageNo&"""><img border=0 src=../../_images/return.gif></a>"
	'======================================================

	'-- 修改資料-------------------------------------------
	If Trim(Request.Form("cmd"))="set" Then

		p1=Trim(Request.Form("p1"))		'	
		p2=Trim(Request.Form("p2"))		'			
		
			StrSql="Select Top 1 * From 星運表 WHERE 序號='"&p0&"'"
			Set oRs=GetRST(StrSql,3,1,2)
			oRs("星運")=p2
			oRs.Update			

			Response.Redirect "List.asp?"&strQs&"&pageno="&strPageNo&"&p0="&p0
	End If
	'======================================================

	StrSql=StrSql & " From 訂單主檔 " & strWhere & " ORDER BY 訂單序號 DESC "

	'======================================================
	
	StrSql1="Select 序號,*"
	StrSql1=StrSql1 & " From 星運表 WHERE 序號="&p0&""
	Set oRs1=ExecSQL_RTN_RST(StrSql1,3,1,2)
		p1=oRs1("生肖")
		p2=oRs1("星運")
	'======================================================
	'-- 關閉物件-------------------------------------------
	oRs.Close
	Set oRs=Nothing
	'======================================================

%>

<html>
<head>
<title>星運表管理</title>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
<link rel=stylesheet href=../../_Css/Set.css>
<script language=javascript src=../../_JScript/subwin.js></script>
<script language=javascript src=set.js></script>
<style>
	.GridTable{
		margin-top:10px;
		border:5 double #778899;
		}
	TH{
		background:#F5F5DC;
		}
</style>
</head>
<body >
<form  Name=myform action="<%=strScript%>?<%=strQS&"&pageno="&strPageNo%>" id="form1"  method="post" onSubmit="return check(this)">
<Input type=hidden name=cmd value=set>
<Input type=hidden name="p0"  value="<%=p0%>">
<table class=gridtable width=99% Height=450 border=1 cellspacing="0" cellpadding="0">
	<caption class=gridcaption style:margin:0>
		<table width=100%>
			<tr>
				<td class=gridtdcaption>星運表管理:修改
				<td align=right><%=strPrev%><%=strNext%><%=strReturn%>
			</tr>
		</table>
	</caption>
	<tr>
	<td  valign=top>
		<table width=100% border=0 cellspacing="1" cellpadding="1">
			<tr>
				<th width="160" align="center">生肖：</th>
				<td width="220" ><input type=text name="p1" size=10 value="<%=p1%>"  style="width:100%" readonly></td>
				<th width="160" align="center">星運：</th>
				<td width="220" ><input type=text name="p2" size=10 value="<%=p2%>"  style="width:100%"></td>
			</tr>
			<tr>
				<td class=gridtdtool colspan=4 align=Center><span class=errmsg><%=strMsg%></span><br>			
					<input class=gridsubmit type="submit" value="確認修改" Style="Cursor:Hand">	
				</td>
			</tr>
		</table>
	</td>


	</tr>
</table>
</form>
</body>
</html>