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
Dim strkey
Dim strZipCode


	'-- UTF-8 偵錯時,要先定義網頁顯示編碼,才可正常顯示不會有亂碼
	Response.Write "<html><head><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""></head>"
	'=====================================================
	
	
	'-- 取得前頁轉送相關參數------------------------------
	strScript=Request.ServerVariables("SCRIPT_NAME")
	strPageNo=Trim(Request("pageno"))
	
	strDate1=Trim(Request("Date1"))
	strDate2=Trim(Request("Date2"))
	strZipCode=Trim(Request("ZipCode"))	
	strKey=Trim(Request("strKey"))
	
	'-- 預設日期 今天,目前只可查單天
	If strDate1="" Then strDate1=Year(Date()) & "/01/01"
	If strDate2="" Then strDate2=Date()
	'================================
	
	strQS="Date1="&strDate1&"&Date2="&strDate2&"&ZipCode="&strZipCode&"&strkey="&strKey
	'RESPONSE.WRITE strQS & "<HR>"
	'RESPONSE.END
	'======================================================
	
	
	'-- 加入查詢條件---------------------------------------
	strWhere=""
	'strWhere=MakeWhere(strWhere,"AND","CONVERT(NVARCHAR(10),A.新增時間,111)",">=",strDate1,"","S","","")
	'strWhere=MakeWhere(strWhere,"AND","CONVERT(NVARCHAR(10),A.新增時間,111)","<=",strDate2,"","S","","")
	
	If 	strZipCode ="Other" Then
		strWhere=MakeWhere(strWhere,"AND","A.郵遞區號"," NOT IN ","('320','324','326')","","N","","")
	Else
		strWhere=MakeWhere(strWhere,"AND","A.郵遞區號","=",strZipCode,"","S","","")
	End IF
	
	'-- 關鍵字查詢---------------------------------------
	'strWhere=MakeWhereKW(strWhere,"AND",Array(""),"%LIKE%",strKey,"","S","","")
	'======================================================

	strWhere=MakeWhereEnd(strWhere)
	'RESPONSE.WRITE strWhere & "<HR>"
	'RESPONSE.END
	'======================================================

	'-- 組合查詢字串---------------------------------------
	StrSql= " Select "
	'StrSql=StrSql & " distinct A.郵遞區號,"
	StrSql=StrSql & " distinct A.申請人地址,"	
	'StrSql=StrSql & " A.訂單序號, "
	'StrSql=StrSql & " A.訂單編號, "
	'StrSql=StrSql & " A.會員編號, "
	StrSql=StrSql & " A.申請人姓名,"
	StrSql=StrSql & " A.郵遞區號,"
	StrSql=StrSql & " A.申請人地址"
	'StrSql=StrSql & " A.新增時間 "
	'StrSql=StrSql & " A.新增人員, "
	'StrSql=StrSql & " A.新增人員姓名,"	
	StrSql=StrSql & " FROM 訂單主檔 AS A"
	StrSql=StrSql & StrWhere
	StrSql=StrSql & " ORDER BY 申請人姓名 " 'ORDER BY不能用申請人地址，會出錯

    'RESPONSE.WRITE StrSql  & "<HR>"
	'RESPONSE.END
	'======================================================

	'-- 取得資料錄-----------------------------------------
	Set oRS=ExecSQL_RTN_RST(StrSql,3,1,2)
	'======================================================
	'-- 組合表單-------------------------------------------
	aryIns=NULL
	aryDel=NULL
	
	strRS=RsToTable_SelectAll(oRs,"del.asp",strQS,0,0,"print.asp?pageno="&request("pageno")&"&"&strQS&"&p0=","",22,"郵寄標籤","99%", _
			Array("10%","9%","7%","8%","25%","17%","7%","10%","5%"), _
			aryDel,0,"",1,"../../_Images/_pager/", _
			aryIns,"OpenWindow_No","HighLight_Yes","SelectAll_Yes")	
	'======================================================

	Dim strSelCboZipCode '郵遞區號
	'-- 郵遞區號------------------------------------------
	strSelCboZipCode=AryToOption(Array("320","324","326","Other"),Array("中壢320","平鎮324","楊梅326","其他未分"),0,1,Trim(strZipCode),"","")
	'======================================================

	'-- 釋放物件-------------------------------------------
	ReleaseObj(Array(oRs))
	'======================================================

%>


<html>
<head>
	<title>郵寄標籤</title>
	<meta http-equiv="Content-Type" content="text/html; charset=big5">
	<link rel=stylesheet href=../../_CSS/default.css>
	<script language=javascript src="../../_JScript/List.js"></script>
	<script language=javascript src="../../_JScript/Subwin.js"></script>
	<script language=javascript src="../../_JScript/Calndr.js"></script>
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
<form action="<%=strScript%>" method="post" name=traQuery id="form1">
<input name="cmd" type="hidden" id="cmd" value="set">
<table width=100% border=0  cellspacing="0" cellpadding="0">
	<tr>
	<td Align=Left> 
	
		<table wwidth=800 border=0  cellspacing="0" cellpadding="0">
			<tr> 
			<!--
				<td width="80" Align=Right>列印日期：</td>
				<td width="20"  Align=Right>起</td>
				<td width="80" Align=Right><input name="Date1" type="text" id="Date1" value="<%=strDate1%>" onclick="ShowCalendar();" style="width:100%;Cursor:Hand;" ReadOnly ></td>
				<td width="20"  Align=Right>迄</td>
				<td width="80" Align=Right><input name="Date2" type="text" id="Date2" value="<%=strDate2%>" onclick="ShowCalendar();" style="width:100%;Cursor:Hand;"></td>
				-->
				<td width="80"  Align=Right>郵遞區號：</td>
				<td width="100" Align=Right><select name="zipcode" style="width:100%;Cursor:Hand;"><%=strSelCboZipCode%></select></td>	


				<!--
				<td>關鍵字:</td>
				<td><input type="text" size=10 name="key" value="<%=strKey%>"></td>
				-->
				<td width=80 Align=Right><input type="submit" value="查　詢" class=button></td>
				<td width=130 Align=Right><input type="button" class="button" OnClick="WO('ExcelExport.asp?<%=strQS%>')" value="列印郵寄標籤" style="margin-right:10px;"></td>
				<td ><font color="red">備註：列出所有歷史訂單資料的戶長的地址，重覆會篩選掉。</font></td>

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