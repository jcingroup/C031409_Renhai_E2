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
Dim s1,s2,s3,s4,s5,s6,s7,strsel,intsel
Dim strSelCbo
Dim intPeople

	'-- UTF-8 偵錯時,要先定義網頁顯示編碼,才可正常顯示不會有亂碼
	Response.Write "<html><head><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""></head>"
	'=====================================================
	
	strScript=Request.ServerVariables("SCRIPT_NAME")
	strPageNo=Trim(Request("pageno"))
	
	s1=Trim(Request("s1"))	'訂單編號	
	s2=Trim(Request("s2"))	'付款方式	
	s3=Trim(request("s3"))	'訂單金額最小值
	s4=Trim(request("s4"))	'訂單金額最大值
	s5=Trim(Request("s5"))	'狀態		
	s6=Trim(Request("s6"))	'銀行帳號
	s7=Trim(Request("s7"))	'經手人
	strSel=Trim(Request("Sel"))	'產品編號
	
	intPeople=Trim(Request("People"))
	
	
	
	If Not IsNumeric(strSel) then intSel="" Else intSel=Cint(strSel)
	strQS="s1="&s1&"&s2="&s2&"&s3="&s3&"&s4="&s4&"&s5="&s5&"&s6="&s6&"&s7="&s7&"&sel="&intSel&"&People=" & intPeople

	'-- 如果第一次進來或不是數字 就帶Session("ID")
	If Not IsNumeric(intPeople) then intPeople=Session("ID") Else intPeople=CLNG(intPeople)
	
	'================================
	'-- 加入查詢條件---------------------------------------
	strWhere=""
	strWhere=MakeWhere(strWhere,"AND","A.付款方式","=",s2,"","N","","")
	'strWhere=MakeWhere(strWhere,"AND","A.狀態","=",s5,"","N","","")
	strWhere=MakeWhere(strWhere,"AND","B.姓名","=",s7,"","S","","")
	strWhere=MakeWhere(strWhere,"AND","A.新增人員","=",intPeople,-1,"N","","")	
	'strWhere=MakeWhere(strWhere,"AND","產品編號","=",intSel,"","N","","")
	
	'-- 關鍵字查詢---------------------------------------
	strWhere=MakeWhereKW(strWhere,"AND",Array("A.訂單編號"),"%LIKE%",s1,"","S","","")
	'strWhere=MakeWhereKW(strWhere,"AND",Array("A.銀行帳號"),"%LIKE%",s6,"","S","","")
	'-- 金額區間查詢--------------------------------------
	'strWhere=MakeWhereBeTween(strWhere,"AND","A.總額","",s3,s4,1)
	strWhere=MakeWhereEnd(strWhere)
	
	
	'-- 組合查詢字串---------------------------------------
	
	'StrSql=" Select A.訂單序號, A.訂單編號, B.姓名,"
	'StrSql=StrSql & "(B.電話區碼+B.電話尾碼) as 電話, B.手機, A.總額, "
	'StrSql=StrSql & " A.訂單狀態名稱 as 狀態, A.付款方式名稱 as 付費方式, A.銀行帳號 " 
	'StrSql=StrSql & "From 訂單主檔 as A "
	'StrSql=StrSql & "left outer join 會員資料表 as B "
	'StrSql=StrSql & "ON A.會員編號 = B.序號 "
	'StrSql=StrSql & strwhere 
	'StrSql=StrSql & "Order By A.訂單時間 desc, A.訂單序號 desc "
	
	'StrSql=	" Select Top 100 訂單序號, 訂單編號 as 編號, 申請人姓名 as 姓名, 申請人電話 as 電話, 總額, 新增時間, 新增人員 " _
	'	& " From  訂單主檔 A "
	StrSql=	" Select 訂單編號, 訂單編號 as 編號, 申請人姓名 as 戶長姓名, 申請人電話 as 電話, 總額, 新增時間, 姓名 "
	StrSql=StrSql & " From 訂單主檔 as A "	
	StrSql=StrSql & " left outer join 人員 as B  ON A.新增人員=B.人員代碼 "
	StrSql=StrSql & strwhere 
	StrSql=StrSql & " Order By A.訂單序號 desc "
	
	'======================================================
	'-- 取得資料錄-----------------------------------------
	Set oRS=ExecSQL_RTN_RST(StrSql,3,1,2)
	'======================================================
	'-- 組合表單-------------------------------------------
	'aryIns=Array("新增訂單","window.location.href='upt.asp'")
	'aryDel=Array("刪除")
	strRS=RsToTable_SelectAll(oRs,"del.asp",strQS,0,2,"upt.asp?pageno="&request("pageno")&"&"&strQS&"&p0=","",10,"<div align=left>訂單管理</div>","99%", _
			Array("10%","17%","10%","17%","20%","17%","10%","10%"), _
			aryDel,0,"",1,"../../_Images/_pager/", _
			aryIns,"OpenWindow_No","HighLight_Yes","SelectAll_Yes")
	'======================================================

	'-- 取得付款方式
	StrSql="SELECT 序號,分類名稱 FROM 訂單分類檔 "
	Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	s2 = RsToOption(oRs,0,1,Cstr(s2),"","")
	
	'-- 取得訂單狀態
	StrSql="SELECT 序號,狀態名稱 FROM 訂單狀態檔 "
	Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	s5 = RsToOption(oRs,0,1,Cstr(s5),"","")
	
	'-- 取得人員列表----------------------------
	Dim strSelCboPeople
	IF Session("UserKind") = 99 Then
		'-- 如果管理人員,可以看所有人(查詢其他人)
		StrSql="SELECT 人員代碼,姓名 FROM 人員  order by 帳號"
	ELSE
		'-- 如果是一般人員,可以看自己
		StrSql="SELECT 人員代碼,姓名 FROM 人員 WHERE 人員代碼 = " & intPeople
	End IF	
	Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	strSelCboPeople = RsToOption(oRs,0,1,Cstr(intPeople),"","")	
	
	'-- 取得產品分類代碼資料-------------------------------
	Set oRs=ExecSQL_RTN_RST("SELECT 產品編號,產品名稱 FROM 產品資料表 where 隱藏 = '0' ",3,0,1)
	strSelCbo=RsToOption(oRs,0,1,trim(intSel),"","")
	
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
			<table width="100%" border=0  cellspacing="0" cellpadding="1">
			
				
				<tr> 
					<!--<td align="Right">訂單編號:</td><td><input type="text" size=10 name="s1" value="<%=s1%>" ></td>			-->
					<td align="Left">經手人員:<select name="People" style="width:20%;Cursor:Hand;"><%=strSelCboPeople%></select> </select>
					<input type="submit" value="查　詢" class=button style="margin-right:10px;">
					</td>		
				</tr>
				
				
			</table>
		</td>
	</tr>
</table>	
</form>
<%=strRs%>
</body>
</html>