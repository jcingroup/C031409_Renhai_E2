<% Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/Power_Function.asp"-->

<%	
Dim strScript,strPageNo
Dim strQS,strSql,oRs,strRS,oRs1

Dim strCon,intCon
Dim strHot,intHot
Dim strSel,strSel1,intSel,intSel1,strSelCbo,strSelCbo1

Dim strKey,StrPay,intPay
Dim aryIns,aryDel
Dim strWhere
Dim strOrderNumber,user
Dim now,Ynow,Mnow,Dnow
	

	strScript=Request.ServerVariables("SCRIPT_NAME")
	strPageNo=Trim(Request("pageno"))
	
	strOrderNumber=""
	strOrderNumber=Trim(Request("OrderNumber"))
	'strCon=Trim(Request("con"))
	'strHot=Trim(Request("hot"))
	strSel=Trim(Request("Sel"))
	'strSel1=Trim(Request("Sel1"))
	strKey=Trim(Request("key"))
	user=Trim(Request("user"))
	strPay = Trim(Request("pay"))
	'response.write strPay
	'response.end
	
	now = date()
	Ynow=Cstr(year(now)-1911)
	Mnow=CStr(Month(now))
	Dnow=CStr(day(now))
	
	'now = Ynow & "年" & Mnow & "月" & Dnow & "日"
	
	If Not IsNumeric(strSel) then intSel=0 Else intSel=Cint(strSel)
	If Not IsNumeric(strPay) then intPay=0 Else intPay=Cint(strPay)
	'If Not IsNumeric(strSel1) then intSel1=2 Else intSel1=Cint(strSel1)
	'If Not IsNumeric(strCon) then intCon=1 Else intCon=Cint(strCon)

	strQS="sel="&intSel&"&sel1="&intSel1&"&con="&intCon&"&key="&strKey&"&pay="&intPay
	'RESPONSE.WRITE strQS


	
Dim strReportName,strReportCaption 
Dim iLen,Path,strPath 
Dim DataBase,Tables,Table
Dim strRPTWhere
'====================================================================================
' 設定物件初值

'	設定報表                                        
	If Not IsObject (session("oApp")) Then
		Set session("oApp") = Server.CreateObject("CrystalRuntime.Application.10")
	End If
                                                               
'	檢查報表物是否存在，如有則清除
	If IsObject(session("oRpt")) then
		Set session("oRpt") = nothing
	End if
'====================================================================================
' 設定報表路徑
	'If Request("RptName") = 1 Then
		strReportName = "捐獻感謝狀.rpt"
		'strReportCaption="menu"
	'Else
		'strReportName = "會員清冊2.rpt"
		'strReportCaption="會員清冊2"
	'End If	
	
	Path = Request.ServerVariables("PATH_TRANSLATED")   
	strScript=Request.Servervariables("SCRIPT_NAME")	'程式實體路徑
	strPath=Server.MapPath(Left(strScript,InstrRev(strScript,"/")) & strReportName)	'報表路徑
	
	'將path變數中的*.asp拿掉                 
	While (Right(Path, 1) <> "\" And Len(Path) <> 0)                      
		iLen = Len(Path) - 1                                                  
		Path = Left(Path, iLen)                                               
	Wend	

	Set session("oRpt") = session("oApp").OpenReport(strPath,1)
'====================================================================================
' 設定DataBase

	set Database = session("oRpt").Database	
	set Tables = Database.Tables

	for each table in Tables
		table.SetLogOnInfo SQL_ServerName,SQL_DBName,SQL_UserId,SQL_Pwd
	Next	
	
'	設定SubReport 的dataBase
'	set SubDatabase = CRSubreports.Database	
'	set SubTables = SubDatabase.Tables	
'	for each table in SubTables
'		table.SetLogOnInfo SQL_ServerName,SQL_DBName,SQL_UserId,SQL_Pwd
'	Next

'	設定SubReport2 的dataBase
'	set SubDatabase = CRSubreports2.Database	
'	set SubTables = SubDatabase.Tables	
'	for each table in SubTables
'		table.SetLogOnInfo SQL_ServerName,SQL_DBName,SQL_UserId,SQL_Pwd
'	Next

	'session("oRpt").DiscardSavedData
'====================================================================================
' 設定ParameterFields
	'Session("oRpt").ParameterFields.GetItemByName("ReportCaption").SetCurrentValue(strReportCaption)
	'Session("oRpt").ParameterFields.GetItemByName("PKID").SetCurrentValue(p100)
	'Session("oRpt").ParameterFields.GetItemByName("@tmp_syear").SetCurrentValue(date1)
	'Session("oRpt").ParameterFields.GetItemByName("@tmp_smonth").SetCurrentValue(date2)
	Session("oRpt").ParameterFields.GetItemByName("@tmp_user").SetCurrentValue(user)
	Session("oRpt").ParameterFields.GetItemByName("@tmp_day").SetCurrentValue(Dnow)
	Session("oRpt").ParameterFields.GetItemByName("@tmp_month").SetCurrentValue(Mnow)
	Session("oRpt").ParameterFields.GetItemByName("@tmp_year").SetCurrentValue(Ynow)
	'Session("oRpt").ParameterFields.GetItemByName("@tmp_now").SetCurrentValue(now)
'====================================================================================
' 設定篩選條件
	strRPTWhere=""
	IF intSel <> 0 Then 
		strRPTWhere ="{vw_訂單明細檔.產品編號}='"& intSel &"'"
	End IF	
	
	IF strOrderNumber <> "" Then 
		IF strRPTWhere = "" Then
			strRPTWhere ="{vw_訂單明細檔.訂單編號}='"& strOrderNumber &"'"
		Else
			strRPTWhere =strRPTWhere & " AND {vw_訂單明細檔.訂單編號}='"& strOrderNumber &"'"
		End IF
	End IF	

	IF strKey <> "" Then 
		IF strRPTWhere = "" Then
			strRPTWhere ="{vw_訂單明細檔.申請人姓名} LIKE '%"& strKey &"%'"
		Else
			strRPTWhere =strRPTWhere & " AND {vw_訂單明細檔.申請人姓名}='"& strKey &"'"
		End IF
	End IF	
		
	IF intPay <> "" Then 
		'IF strRPTWhere = "" Then
		'	strRPTWhere ="{vw_訂單明細檔.申請人姓名} LIKE '%"& strKey &"%'"
		'Else
			strRPTWhere = strRPTWhere & " AND {vw_訂單明細檔.付款方式}='"& intPay&"'"
		'End IF
	End IF		
		
	session("oRpt").RecordSelectionFormula = strRPTWhere
'====================================================================================

' 取得資料
	On Error Resume Next
	Session("oRpt").ReadRecords
	If Err.Number <> 0 Then
		Response.Write "An Error has occured on the server in attempting to access the data source"
	Else
		If IsObject(session("oPageEngine")) Then
			set session("oPageEngine") = nothing
		End If
		set session("oPageEngine") = session("oRpt").PageEngine
	End If
'====================================================================================
%>

<!-- #include file="../../_Function/Smartvieweractivex.asp" --> 