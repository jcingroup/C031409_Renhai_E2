﻿<% Option Explicit%>
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
Dim intPeople
Dim intProductSN
Dim intDocType
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
	intProductSN=Trim(Request("ProductSN"))
	intDocType=Trim(Request("DocType"))
	
	
	strKey=Trim(Request("strKey"))
	
	'-- 預設日期 今天,目前只可查單天
	If strDate1="" Then strDate1=Year(Date())&"/01/01"
	If strDate2="" Then strDate2=Date()
	
	''-- 預設時間 早上8 下午16:00
	'If strTime1="" Then strTime1="01"
	'If strTime2="" Then strTime2="24"
	
	''-- 這兩個值有加工過,是要做查詢用
	'If strTime1 <> "" Then strTime1Temp=strTime1 & "-00-00"
	'If strTime1 <> "" Then strTime2Temp=strTime2 & "-00-00"
	'================================
	
	''-- 如果第一次進來或不是數字 就帶Session("ID")
	'If Not IsNumeric(intPeople) then intPeople=Session("ID") Else intPeople=CLNG(intPeople)
	''================================
	
	'If Not IsNumeric(intProductSN) then intProductSN=-1 Else intProductSN=CLNG(intProductSN)
	
	If Not IsNumeric(intDocType) then intDocType=1 Else intDocType=CINT(intDocType)
	
	strQS="Date1="&strDate1&"&Date2="&strDate2&"&DocType="&intDocType&"&strkey="&strKey
	'strQS="Date1="&strDate1&"&Date2="&strDate2&"&Time1="&strTime1&"&Time2="&strTime2&"&Time1Temp="&strTime1Temp&"&Time2Temp="&strTime2Temp&"&People="&intPeople&"&ProductSN="&intProductSN&"&strkey="&strKey
	'RESPONSE.WRITE strQS & "<HR>"
	'RESPONSE.END
	'======================================================
	
	
	'-- 加入查詢條件---------------------------------------
	strWhere=""
	
	Select Case intDocType 
		Case 1	'點燈
			strWhere=MakeWhere(strWhere,"AND","A.產品編號","IN","('2','3','4','5','12','13','21','31','41','51','121','131','16','161','17','171')","","N","","")
		Case 2	'入斗
			strWhere=MakeWhere(strWhere,"AND","A.產品編號","IN","('8')","","N","","")
		Case 3	'太歲
			strWhere=MakeWhere(strWhere,"AND","A.產品編號","IN","('6')","","N","","")
		Case 4  '福燈
			strWhere=MakeWhere(strWhere,"AND","A.產品編號","IN","('390','391','392','393','394','395','396')","","N","","")
		Case 5	'祈福許願燈
			strWhere=MakeWhere(strWhere,"AND","A.產品編號","IN","('1501')","","N","","")
    	Case 6	'沉香媽祖/藥師佛斗燈
			strWhere=MakeWhere(strWhere,"AND","A.產品編號","IN","('1801','1802')","","N","","")
    End Select	
	
	'strWhere=MakeWhere(strWhere,"AND","A.產品編號","=",intProductSN,-1,"N","","")
	'strWhere=MakeWhere(strWhere,"AND","A.產品編號","IN","('2','3','4','5')","","N","","")
	'strWhere=MakeWhere(strWhere,"AND","A.新增人員","=",intPeople,-1,"N","","")
	
	strWhere=MakeWhere(strWhere,"AND","CONVERT(NVARCHAR(10),A.新增時間,111)",">=",strDate1,"","S","","")
	strWhere=MakeWhere(strWhere,"AND","CONVERT(NVARCHAR(10),A.新增時間,111)","<=",strDate2,"","S","","")
	
	'-- Add by Taka 更改DB_function------------------------
	'strWhere=MakeWhereBeTween(strWhere,"AND","CONVERT(NVARCHAR(20),A.新增時間,108)","",strTime1Temp,strTime2Temp,0)
	'======================================================

	'-- 關鍵字查詢---------------------------------------
	'strWhere=MakeWhereKW(strWhere,"AND",Array(""),"%LIKE%",strKey,"","S","","")
	'======================================================

	strWhere=MakeWhereEnd(strWhere)
	'RESPONSE.WRITE strWhere & "<HR>"
	'RESPONSE.END
	'======================================================

	'-- 組合查詢字串---------------------------------------
	StrSql= " Select "
	StrSql=StrSql & " A.訂單序號, "
	StrSql=StrSql & " A.訂單編號, "
	StrSql=StrSql & " A.產品編號, "
	StrSql=StrSql & " A.產品名稱, " 
	StrSql=StrSql & " A.數量, "
	StrSql=StrSql & " A.價格, "
	StrSql=StrSql & " A.申請人姓名,"
	StrSql=StrSql & " A.點燈位置,"
	StrSql=StrSql & " A.新增時間 "
	'StrSql=StrSql & " A.新增人員, "
	'StrSql=StrSql & " A.新增人員姓名,"	
	'StrSql=StrSql & " A.產品排序"
	StrSql=StrSql & " FROM 訂單明細檔 AS A"
	StrSql=StrSql & StrWhere
	StrSql=StrSql & " ORDER BY A.新增時間 "

    'RESPONSE.WRITE StrSql  & "<HR>"
	'RESPONSE.END
	'======================================================

	'-- 取得資料錄-----------------------------------------
	Set oRS=ExecSQL_RTN_RST(StrSql,3,1,2)
	'======================================================
	'-- 組合表單-------------------------------------------
	aryIns=NULL
	aryDel=NULL
	
	strRS=RsToTable_SelectAll(oRs,"del.asp",strQS,0,0,"print.asp?pageno="&request("pageno")&"&"&strQS&"&p0=","",10,"疏文名冊","99%", _
			Array("10%","9%","7%","8%","5%","7%","7%","10%","5%","10%","10%","9%"), _
			aryDel,0,"",1,"../../_Images/_pager/", _
			aryIns,"OpenWindow_No","HighLight_Yes","SelectAll_Yes")	
	'======================================================

	Dim strSelCboDocType
	'-- 取得疏文名冊類別-----------------------------------
	strSelCboDocType=AryToOption(Array(1,2,3,4,5,6),Array("點燈","入斗","太歲","福燈","祈福許願燈","沉香媽祖/藥師佛斗燈"),0,1,Trim(intDocType),"","")
	'======================================================
	
	
	Dim strSelCboTime1 '時間1'
	'''-- 取得時間1------------------------------------------
	''strSelCboTime1=AryToOption(Array(01,02,03,04,05,06,07,08,09,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24),Array("01","02","03","04","05","06","07","08","09","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24"),1,1,Trim(strTime1),"","")
	''======================================================
	
	Dim strSelCboTime2 '時間2
	'''-- 取得時間2------------------------------------------
	''strSelCboTime2=AryToOption(Array(01,02,03,04,05,06,07,08,09,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24),Array("01","02","03","04","05","06","07","08","09","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24"),1,1,Trim(strTime2),"","")
	'======================================================

	'''-- 取得人員列表---------------------------------------
	Dim strSelCboPeople
	''IF Session("UserKind") = 99 Then
	''	'-- 如果管理人員,可以看所有人(查詢其他人)
	''	StrSql="SELECT 人員代碼,姓名 FROM 人員 "
	''ELSE
	''	'-- 如果是一般人員,可以看自己
	''	StrSql="SELECT 人員代碼,姓名 FROM 人員 WHERE 人員代碼 = " & intPeople
	''End IF	
	''Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	''strSelCboPeople = RsToOption(oRs,0,1,Cstr(intPeople),"","")	
	'======================================================

	'''-- 取得產品基本資料-----------------------------------
	Dim strSelCboProductSN
	''Set oRs=ExecSQL_RTN_RST("SELECT 產品編號,產品名稱 FROM 產品資料表 Where 產品編號 IN ('2','3','4','5') ORDER BY 產品編號",2,0,1)
	''strSelCboProductSN=RsToOption(oRs,0,1,trim(intProductSN),"","")			
	'======================================================
	
	'-- 釋放物件-------------------------------------------
	ReleaseObj(Array(oRs))
	'======================================================

%>


<html>
<head>
	<title>疏文名冊</title>
	<meta http-equiv="Content-Type" content="text/html; charset=big5">
	<link rel=stylesheet href=../../_CSS/default.css>
	<script language=javascript src="../../_JScript/List.js"></script>
	<script language=javascript src="../../_JScript/Subwin.js"></script>
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
		.gridtd6{color:black;text-align:center}
		.gridtd7{color:black;text-align:center}

	</style>
</head>
<body>
<!--#include file="../../_include/top.asp"-->
<script>
	$(document).ready(function(){
		$('.datepicker').datepicker();
	});
</script>
<a href="./" >***說明：產生的疏文名冊檔已過濾掉重覆的姓名***</a>
<form action="<%=strScript%>" method="post" name=traQuery id="form1">
<input name="cmd" type="hidden" id="cmd" value="set">
<table width=100% border=0  cellspacing="0" cellpadding="0">
	<tr>
	<td Align=Left> 
	
		<table wwidth=800 border=0  cellspacing="0" cellpadding="0">
			<tr> 
				<td width="80" Align=Right>列印日期：</td>
				<td width="20"  Align=Right>起</td>
				<td width="80" Align=Right><input name="Date1" type="text" id="Date1" value="<%=strDate1%>" class="datepicker"></td>
				<td width="20"  Align=Right>迄</td>
				<td width="80" Align=Right><input name="Date2" type="text" id="Date2" value="<%=strDate2%>" class="datepicker"></td>
				
				<!--
				<td width="80" Align=Right>列印時間：</td>
				<td width="20"  Align=Right>起</td>
				<td width="50" Align=Left><select name="Time1" style="width:100%;Cursor:Hand;"><%=strSelCboTime1%></select> </td>
				<td width="20"  Align=Right>迄</td>
				<td width="50" Align=Left><select name="Time2" style="width:100%;Cursor:Hand;"><%=strSelCboTime2%></select> </td>

				<td width="80" Align=Right>經手人員：</td>
				<td width="80" Align=Left><select name="People" style="width:100%;Cursor:Hand;"><%=strSelCboPeople%></select> </td>
				-->

				<td width="100" Align=Right>疏文名冊類別：</td>
				<td width="80" Align=Left><select name="DocType" style="width:100%;Cursor:Hand;"><%=strSelCboDocType%></select> </td>

				<!--			
				<td width="80" Align=Right>點燈類別：</td>
				<td width="80" Align=Left><select name="ProductSN" style="width:100%;Cursor:Hand;"><option></option><%=strSelCboProductSN%></select> </td>
				-->

				<!--
				<td>關鍵字:</td>
				<td><input type="text" size=10 name="key" value="<%=strKey%>"></td>
				-->
				<td width=80 Align=Right><input type="submit" value="查　詢" class=button></td>
				<td width=170 Align=Right><input type="button" class="button" OnClick="WO('ExcelExport.asp?<%=strQS%>')" value="產生疏文名冊檔" style="margin-right:10px;"></td>
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