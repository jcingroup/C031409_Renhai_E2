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
	If s1<> "" Then	WhereStr.Add "A.姓名","LikeLeft",s1,"'"
	If s2<> "" Then	WhereStr.Add "A.電話尾碼","LikeLeft",s2,"'"
	If s3<> "" Then	WhereStr.Add "A.手機","LikeLeft",s3,"'"
				
	strwhere = WhereStr.ToWhereString
	If strwhere <> "" Then strwhere = " Where " & strwhere
		
	'-- 組合查詢字串---------------------------------------
	StrSql =	"SELECT 產品編號,產品名稱,產品分類,標籤,價格 FROM 產品資料表" & strwhere & " Order by 產品編號 desc"
	'response.write strsql
		
	'======================================================
	'-- 取得資料錄-----------------------------------------
	Set oRS=ExecSQL_RTN_RST(StrSql,3,1,2)
	AspErrCheck "SQL Select Err " & StrSql
	'======================================================
	'-- 組合表單-------------------------------------------
	aryIns=Array("新增點燈項目","window.location.href='upt.asp'")
	'aryDel=Array("刪除")
	strRS=RsToTable_SelectAll(oRs,"del.asp",strQS,0,1,"upt.asp?pageno="&request("pageno")&"&"&strQS&"&p0=","",10,"<div align=left>點燈資料管理</div>","99%", _
			Array("30%","20%","20%","15%","15%"), _
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
		.gridtd11{color:black;text-align:center}
	</style>
	
		<!-- 這一段要放在HEAD裡面 -->
		<%=SetBodyPrintBlank(0)%>
</head>
<body <%=SetBodyAntiCopy(0)%>>
	<%=SetBodyAntiSaveAs(0)%>
	
<form action="<%=strScript%>" method="post" name=ins id="form1">
<table width=98% border=0  cellspacing="0" cellpadding="0">
	<tr>
		<td> 
			<table  border=0  cellspacing="0" cellpadding="1">
				<tr><font color=red>使用說明：1.請輸入姓名或手機或電話後查詢出戶長 2.若無來過點右方「新增一戶」。</font></tr>
				<tr> 
					<td align="Right">姓名:</td><td><input type="text" size="10" name="s1" value="<%=s1%>" /></td>
					<td width="15"></td>	
					<td align="Right">電話:</td><td><input type="text" size="15"  name="s2" value="<%=s2%>" /></td>
					<td align="Right">手機:</td><td><input type="text" size="15"  name="s3" value="<%=s3%>" /></td>
					<td align="Right"><input type="submit" value="查　詢" class="button" style="margin-right:10px;" /></td>
				</tr>
			
			</table>
		</td>
	</tr>
</table>	
</form>
<%=strRs%>
</body>
</html>