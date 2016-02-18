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
Dim s4CheckedA,s4CheckedB
Dim s5A,s5B

Dim strSelCbo

Dim WhereStr

	strScript=Request.ServerVariables("SCRIPT_NAME")
	strPageNo=Trim(Request("pageno"))
	
	s1=Trim(Request("s1"))	''
	s2=Trim(Request("s2"))	'
	s3=Trim(Request("s3"))	'
	s4=Trim(Request("s4"))	'
	s5A=Trim(Request("s5A"))	'
	s5B=Trim(Request("s5B"))	'
	
	If s4 = "1" Then  s4CheckedA = "Checked"
	If s4 = "2" Then  s4CheckedB = "Checked"
		
'	If Not IsNumeric(strSel) then intSel="" Else intSel=Cint(strSel)
	strQS="s1="&s1&"&s2="&s2&"&s3="&s3&"&s4="&s4&"&s5A="&s5A&"&s5B="&s5B	
	'-- 加入查詢條件---------------------------------------
	Set WhereStr = new WhereSet

	WhereStr.Add "A.產品編號","=","6","'"
	If s1<> "" Then	WhereStr.Add "A.申請人姓名","LikeLeft",s1,"'"
	If s2<> "" Then	WhereStr.Add "A.電話尾碼","LikeLeft",s2,"'"
	If s3<> "" Then	WhereStr.Add "A.手機","LikeLeft",s3,"'"
	If s4<> "" Then	WhereStr.Add "A.申請人性別","=",s4,"'"
	If s5A<> "" Then	WhereStr.Add "A.新增時間",">=",s5A,"'"
	If s5B<> "" Then	WhereStr.Add "新增時間","<=",s5B,"'"
				
	strwhere = WhereStr.ToWhereString
	If strwhere <> "" Then strwhere = " Where " & strwhere
		
	'-- 組合查詢字串---------------------------------------
	StrSql = "" _	
&"SELECT " _
&"A.訂單序號, " _
&"A.申請人姓名 as 姓名, " _
&"B.電話尾碼 as 電話, " _
&"B.手機, " _
&"A.申請人地址 as 地址, " _
&"Case A.申請人性別 When '1' Then '男' When '2' Then '女' End as 性別, " _
&"A.申請人生日 as 生日, " _
&"C.姓名 as 戶長, " _
&"C.電話 as 戶長電話, " _
&"A.新增時間 " _
&"FROM         會員戶長資料 AS C INNER JOIN " _
&"             會員資料表 AS B ON C.戶長SN = B.戶長SN INNER JOIN " _
&"             訂單明細檔 AS A ON B.序號 = A.會員編號 " & strwhere & " Order By B.生日 desc, 新增時間 desc"

		'response.write strsql
		
	'======================================================
	'-- 取得資料錄-----------------------------------------
	Set oRS=ExecSQL_RTN_RST(StrSql,3,1,2)
	AspErrCheck "SQL Select Err " & StrSql
	'======================================================
	'-- 組合表單-------------------------------------------
	'aryIns=Array("新增一戶","window.location.href='upt.asp'")
	aryIns=Null
	'aryDel=Array("刪除")
	strRS=RsToTable_SelectAll(oRs,"del.asp",strQS,0,1,"upt.asp?pageno="&request("pageno")&"&"&strQS&"&p0=","",10,"<div align=left>安太歲性別生日修改-按生日由小至大排序(無生日者在最後面)</div>","99%", _
			Array("10%","10%","10%","30%","5%","5%","10%","5%","15%"), _
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
<!--#include file="../../_include/top2.asp"-->
<form action="<%=strScript%>" method="post" name=ins id="form1">
<table width=98% border=0  cellspacing="0" cellpadding="0">
	<tr>
		<td> 
			<table  border=0  cellspacing="0" cellpadding="1">
				<tr><font color=red></font></tr>
				<tr> 
					<td align="Right">姓名:</td><td><input type="text" size="10" name="s1" value="<%=s1%>" /></td>
<!--	
					<td align="Right">電話:</td><td><input type="text" size="15"  name="s2" value="<%=s2%>" /></td>
					<td align="Right">手機:</td><td><input type="text" size="15"  name="s3" value="<%=s3%>" /></td>
-->					
					<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
					<td align="Right">性別:</td><td>
							<input type="radio" name="s4" value="1" <%=s4CheckedA%> />男
							<input type="radio" name="s4" value="2" <%=s4CheckedB%> />女
					</td>
					<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
					<td align="Right">新增時間:</td><td>
							<input type="text" size="10" id="s5A" name="s5A" value="<%=s5A%>" readonly/>~
							<input type="text" size="10" id="s5B" name="s5B" value="<%=s5B%>" readonly/>
					</td>			
					<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
					<td align="Right"><input type="submit" value="查　詢" class="button" style="margin-right:10px;" /></td>
				</tr>
			</table>
		</td>
	</tr>
</table>	
</form>
<%=strRs%>
 說明1:點選姓名進入修改頁 說明2:更新後一併更新會員基本資料
</body>
</html>
<script type="text/javascript" language="javascript">
	$(document).ready(function () {
		$("#s5A").datepicker();	
		$("#s5B").datepicker();			
	})
	</Script>