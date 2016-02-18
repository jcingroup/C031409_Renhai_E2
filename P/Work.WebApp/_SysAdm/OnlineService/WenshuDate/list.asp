<% Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/Power_Function.asp"-->
<%
On Error Resume Next
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
Dim s1,s2,s3,s4,s5,s6,s7,strsel,intsel
Dim strSelCbo

Dim WhereStr

	strScript=Request.ServerVariables("SCRIPT_NAME")
	strPageNo=Trim(Request("pageno"))
	
	s1=Trim(Request("s1"))	'
	s2=Trim(Request("s2"))	'
	s3=Trim(Request("s3"))	'
	
	If Not IsNumeric(strSel) then intSel="" Else intSel=Cint(strSel)
	'strQS="s1="&s1&"&s2="&s2&"&s3="&s3
	strQS = ""
	'-- 加入查詢條件---------------------------------------
	Set WhereStr = new WhereSet
'	If s1<> "" Then	WhereStr.Add "A.姓名","LikeLeft",s1,"'"

				
	strwhere = WhereStr.ToWhereString
	If strwhere <> "" Then strwhere = " Where " & strwhere
		
	'-- 組合查詢字串---------------------------------------
	StrSql =	"SELECT  序號, 梯次, 時間, 農曆年, 農曆月, 農曆日  " _
		&	"FROM 文疏梯次時間表 " _
		&	"Order By 序號 " 
		'response.write strsql
		
	'======================================================
	'-- 取得資料錄-----------------------------------------
	Set oRS=ExecSQL_RTN_RST(StrSql,3,1,2)
	AspErrCheck "SQL Select Err " & StrSql
	'======================================================
	'-- 組合表單-------------------------------------------
	'aryIns=Array()
	'aryDel=Array("刪除")
	strRS=RsToTable_SelectAll(oRs,"del.asp",strQS,0,1,"upt.asp?p0=","",30,"<div align=left>文殊梯次表管理</div>","99%", _
			Array("15%","15%","20%","15%","15%","15%"), _
			aryDel,0,"",1,"../../_Images/_pager/", _
			aryIns,"OpenWindow_No","HighLight_Yes","SelectAll_Yes")
			
	AspErrCheck "RsToTable Err"
	'======================================================
	'-- 取得付款方式
	'StrSql="SELECT 序號,分類名稱 FROM 訂單分類檔 "
	'Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	's2 = RsToOption(oRs,0,1,Cstr(s2),"","")
%>
<html>
<head>
	<title></title>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<link rel=stylesheet href=../../_CSS/default.css>
	<script  type="text/javascript" src="../../_JScript/List.js"></script>
</head>
<body>
	
<form action="<%=strScript%>" method="post" name=ins id="form1">
    <table style="width:98%; border:0px; padding:0px; margin:0px">
	    <tr>
		    <td> 
		    </td>
	    </tr>
    </table>	
</form>
<%=strRs%>
</body>
</html>