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
	strQS="s1="&s1&"&s2="&s2&"&s3="&s3
	
	'-- 加入查詢條件---------------------------------------
	Set WhereStr = new WhereSet
	If s1<> "" and len(s1) >=2 Then	WhereStr.Add "A.姓名","Like",s1,"'"
	If s2<> "" Then	WhereStr.Add "A.電話尾碼","LikeLeft",s2,"'"
	If s3<> "" Then	WhereStr.Add "A.手機","LikeLeft",s3,"'"
				
	strwhere = WhereStr.ToWhereString
	If strwhere <> "" Then strwhere = " Where A.is_delete=0 AND " & strwhere
    If strwhere = "" Then strwhere = " Where A.is_delete=0 AND 1=0"
		
	'-- 組合查詢字串---------------------------------------
	StrSql =	"SELECT  Top 40  B.戶長SN, B.姓名 as 戶長姓名,A.姓名,B.電話,a.手機,B.戶長SN " _
		&	"FROM         會員資料表 AS A INNER JOIN " _
		&	"會員戶長資料 AS B ON A.戶長SN = B.戶長SN INNER JOIN " _
		&	"性別表 AS C ON A.性別 = C.序號 INNER JOIN " _
		&	"TF表 AS D ON A.Is戶長 = D.Value " & strwhere 
		'response.write strsql
		
	'======================================================
	'-- 取得資料錄-----------------------------------------
	Set oRS=ExecSQL_RTN_RST(StrSql,3,1,2)
	AspErrCheck "SQL Select Err " & StrSql
	'======================================================
	'-- 組合表單-------------------------------------------
	aryIns=Array("新增一戶","window.location.href='upt.asp'")
	'aryDel=Array("刪除")
	strRS=RsToTable_SelectAll(oRs,"del.asp",strQS,0,2,"upt.asp?pageno="&request("pageno")&"&"&strQS&"&p0=","",40,"<div align=left>會員管理</div>","99%", _
			Array("15%","15%","20%","15%","15%"), _
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
			    <table style="border:0px; padding:1px; margin:0px">
				    <tr><td colspan="4"><span style="color:Red">使用說明：1.請輸入姓名或手機或電話後查詢出戶長 2.若無來過點右方「新增一戶」。</span></td></tr>
				    <tr> 
					    <td style="text-align:right">姓名:</td><td><input type="text" size="10" name="s1" value="<%=s1%>" /></td>
					    <td style="text-align:right">電話:</td><td><input type="text" size="15"  name="s2" value="<%=s2%>" /></td>
					    <td style="text-align:right">手機:</td><td><input type="text" size="15"  name="s3" value="<%=s3%>" /></td>
					    <td style="text-align:right"><input type="submit" value="查　詢" class="button" style="margin-right:10px;" /></td>
				    </tr>
			    </table>
		    </td>
	    </tr>
    </table>	
</form>
<%=strRs%>
</body>
</html>