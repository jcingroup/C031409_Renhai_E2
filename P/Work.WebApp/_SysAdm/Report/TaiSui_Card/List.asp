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
Dim intBoyGirl
Dim strkey


	'-- UTF-8 偵錯時,要先定義網頁顯示編碼,才可正常顯示不會有亂碼
	Response.Write "<html><head><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""></head>"
	'=====================================================
	
	
	'-- 取得前頁轉送相關參數------------------------------
	strScript=Request.ServerVariables("SCRIPT_NAME")
	strPageNo=Trim(Request("pageno"))
	
	strDate1=Trim(Request("Date1"))
	strDate2=Trim(Request("Date2"))
	intBoyGirl=Trim(Request("BoyGirl"))	
	strKey=Trim(Request("strKey"))
	
	'-- 預設日期 今天,目前只可查單天
	If strDate1="" Then strDate1=Year(Date()) & "/01/01"
	If strDate2="" Then strDate2=Date()
	
	If Not IsNumeric(intBoyGirl) then intBoyGirl=1 Else intBoyGirl=CINT(intBoyGirl)	
	
	strQS="Date1="&strDate1&"&Date2="&strDate2&"&BoyGirl="&intBoyGirl&"&strkey="&strKey
	'RESPONSE.WRITE strQS & "<HR>"
	'RESPONSE.END
	'======================================================
	
	
	'-- 加入查詢條件---------------------------------------
	strWhere=""
	strWhere=MakeWhere(strWhere,"AND","A.申請人性別","=",intBoyGirl,"","S","","")
	strWhere=MakeWhere(strWhere,"AND","A.產品編號","=",6,"","N","","")
	strWhere=MakeWhere(strWhere,"AND","CONVERT(NVARCHAR(10),A.新增時間,111)",">=",strDate1,"","S","","")
	strWhere=MakeWhere(strWhere,"AND","CONVERT(NVARCHAR(10),A.新增時間,111)","<=",strDate2,"","S","","")
	'-- 關鍵字查詢---------------------------------------
	'strWhere=MakeWhereKW(strWhere,"AND",Array(""),"%LIKE%",strKey,"","S","","")
	'======================================================
	strWhere=MakeWhereEnd(strWhere)
	'RESPONSE.WRITE strWhere & "<HR>"
	'RESPONSE.END
	'======================================================

	'-- 組合查詢字串---------------------------------------
	'StrSql="Select 太歲編號, 訂單編號, 申請人姓名 as 姓名, 申請人性別, 申請人地址 as 地址, 歲數 as 年齡, 申請人時辰 as 出生時辰, 新增時間, 星君"
	'StrSql=StrSql & " From 安太歲明細檔 " 
	'StrSql=StrSql & strWhere
	'StrSql=StrSql & " order by 訂單編號 desc"
	
	StrSql= " Select "
	StrSql=StrSql & " A.訂單序號, "
	StrSql=StrSql & " A.訂單編號, "
	StrSql=StrSql & " A.產品編號, "
	StrSql=StrSql & " A.產品名稱, " 
	StrSql=StrSql & " A.申請人姓名,"

	StrSql=StrSql & " CASE A.申請人性別 "
	StrSql=StrSql & " WHEN 1 THEN '男'"
	StrSql=StrSql & " WHEN 2 THEN '女'"
	StrSql=StrSql & " ELSE '' END AS 申請人性別,"
	StrSql=StrSql & " REPLACE((Year(GetDate()) - 1911 - CAST(DBO.JKFUN_ROCDate_Split(A.申請人生日,'YY') AS INT)) +1,Year(GetDate()) - 1911,0) AS  申請人年齡,"
	
	StrSql=StrSql & " A.申請人地址,"

	StrSql=StrSql & " (SELECT TOP 1 星君 FROM 年度生肖表 WHERE 民國年=DBO.JKFUN_ROCDate_Split(A.申請人生日,'YY')) AS 星君, " 
	StrSql=StrSql & " DBO.JKFUN_ROCDate_Split(A.申請人生日,'YY') AS 生日年,"
	StrSql=StrSql & " DBO.JKFUN_ROCDate_Split(A.申請人生日,'MM') AS 生日月,"
	StrSql=StrSql & " DBO.JKFUN_ROCDate_Split(A.申請人生日,'DD') AS 生日日,"
	StrSql=StrSql & " A.申請人時辰,"
	
	StrSql=StrSql & " CONVERT(NVARCHAR(10),A.新增時間,111) AS 新增時間 "
	StrSql=StrSql & " FROM 訂單明細檔 AS A"
	StrSql=StrSql & StrWhere
	StrSql=StrSql & " ORDER BY A.生日年  DESC"

    'RESPONSE.WRITE StrSql  & "<HR>"
	'RESPONSE.END
	'======================================================

	'-- 取得資料錄-----------------------------------------
	Set oRS=ExecSQL_RTN_RST(StrSql,3,1,2)
	'======================================================
	'-- 組合表單-------------------------------------------
	aryIns=NULL
	aryDel=NULL
	
	strRS=RsToTable_SelectAll(oRs,"del.asp",strQS,0,0,"print.asp?pageno="&request("pageno")&"&"&strQS&"&p0=","",10,"太歲卡-依歲數做排序","99%", _
			Array("3%","3%","5%","5%","3%","3%","13%","3%","3%","3%","3%","3%","3%"), _
			aryDel,0,"",1,"../../_Images/_pager/", _
			aryIns,"OpenWindow_No","HighLight_Yes","SelectAll_Yes")	
	'======================================================
	Dim strSelCboBoyGirl
	'-- 取得男女類別-----------------------------------
	strSelCboBoyGirl=AryToOption(Array(1,2),Array("男","女"),0,1,Trim(intBoyGirl),"","")
	'======================================================
	
	Dim strSelCboTime1 '時間1
	'-- 取得時間1------------------------------------------
	'strSelCboTime1=AryToOption(Array(01,02,03,04,05,06,07,08,09,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24),Array("01","02","03","04","05","06","07","08","09","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24"),1,1,Trim(strTime1),"","")
	'======================================================
	
	Dim strSelCboTime2 '時間2
	'-- 取得時間2------------------------------------------
	'strSelCboTime2=AryToOption(Array(01,02,03,04,05,06,07,08,09,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24),Array("01","02","03","04","05","06","07","08","09","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24"),1,1,Trim(strTime2),"","")
	'======================================================

	'-- 取得人員列表---------------------------------------
	Dim strSelCboPeople
	'IF Session("UserKind") = 99 Then
	'	'-- 如果管理人員,可以看所有人(查詢其他人)
	'	StrSql="SELECT 人員代碼,姓名 FROM 人員 "
	'ELSE
	'	'-- 如果是一般人員,可以看自己
	'	StrSql="SELECT 人員代碼,姓名 FROM 人員 WHERE 人員代碼 = " & intPeople
	'End IF	
	'Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	'strSelCboPeople = RsToOption(oRs,0,1,Cstr(intPeople),"","")	
	'======================================================

	'-- 取得產品基本資料-----------------------------------
	Dim strSelCboProductSN
	'Set oRs=ExecSQL_RTN_RST("SELECT 產品編號,產品名稱 FROM 產品資料表 Where 產品編號 IN ('2','3','4','5') ORDER BY 產品編號",2,0,1)
	'strSelCboProductSN=RsToOption(oRs,0,1,trim(intProductSN),"","")			
	'======================================================
	
	'-- 農曆生日拆解年月日-----------------------------------
	Function GetBirthday_Split(varDate,varPart)
	Dim AryD1,AryD2
	Dim IntPos
	Dim varTemp
		
		'-- 列外處裡
		IF NOT ISNULL(varDate)  Then
		
			'-- 將傳入的農曆生日拆解年月日
			AryD1=Split(varDate,"/")
			IF UBOUND(AryD1) = 2 Then
				SELECT CASE UCASE(varPart)
					CASE "YY"
						varTemp=AryD1(0)
						
					CASE "MM"
						varTemp=AryD1(1)
					CASE "DD"
						varTemp=AryD1(2)
				End Select
			End IF
		ELSE
			varTemp=""
		End IF
		'-- 偵錯用
		'RESPONSE.WRITE varDate & "--" & varPart & "--" & varTemp & "<HR>" 
		
		GetBirthday_Split=varTemp
		

	End Function'==============================================	
	
	'-- 釋放物件-------------------------------------------
	ReleaseObj(Array(oRs))
	'======================================================

%>


<html>
<head>
	<title>太歲卡</title>
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
		.gridtd6{color:black;text-align:Left}
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
<a href="SinGiy_font.msi" target="new">***下載安裝星君名稱補丁，潘、毛，適用WinXP作業系統優先***</a>
<form action="<%=strScript%>" method="post" name=traQuery id="form1">
<input name="cmd" type="hidden" id="cmd" value="set">
<table width=100% border=0  cellspacing="0" cellpadding="0">
	<tr>
	<td Align=Left> 
	
		<table wwidth=800 border=0  cellspacing="0" cellpadding="0">
			<tr> 
				<td width="80" Align=Right>列印日期：</td>
				<td width="20"  Align=Right>起</td>
				<td width="80" Align=Right><input name="Date1" type="text" id="Date1" value="<%=strDate1%>" class="datepicker" /></td>
				<td width="20"  Align=Right>迄</td>
				<td width="80" Align=Right><input name="Date2" type="text" id="Date2" value="<%=strDate2%>" class="datepicker" /></td>
				
				<td width="100" Align=Right>姓別：</td>
				<td width="80" Align=Left><select name="BoyGirl" style="width:100%;Cursor:Hand;"><%=strSelCboBoyGirl%></select> </td>				
				<!--
				<td width="80" Align=Right>列印時間：</td>
				<td width="20"  Align=Right>起</td>
				<td width="50" Align=Left><select name="Time1" style="width:100%;Cursor:Hand;"><%=strSelCboTime1%></select> </td>
				<td width="20"  Align=Right>迄</td>
				<td width="50" Align=Left><select name="Time2" style="width:100%;Cursor:Hand;"><%=strSelCboTime2%></select> </td>

				<td width="80" Align=Right>經手人員：</td>
				<td width="80" Align=Left><select name="People" style="width:100%;Cursor:Hand;"><%=strSelCboPeople%></select> </td>
				
				
				<td width="80" Align=Right>點燈類別：</td>
				<td width="80" Align=Left><select name="ProductSN" style="width:100%;Cursor:Hand;"><option></option><%=strSelCboProductSN%></select> </td>
				-->

				<!--
				<td>關鍵字:</td>
				<td><input type="text" size=10 name="key" value="<%=strKey%>"></td>
				-->
				<td width=80 Align=Right><input type="submit" value="查　詢" class=button></td>
				<td width=110 Align=Right><input type="button" class="button" OnClick="WO('ExcelExport.asp?<%=strQS%>')" value="產生太歲卡檔" style="margin-right:10px;"></td>
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