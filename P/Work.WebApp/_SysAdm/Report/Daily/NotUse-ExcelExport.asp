<%Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/Power_Function.asp"-->
<!--#include file="../../_Function/File_Function.asp"-->


<%
Dim strScript,strPageNo,strQS
Dim oRs,StrSql,strWhere
Dim N
'-- ExcelExport -------------------------------------------
Dim aryFieldsPosition
Dim aryFieldsData
Dim intDataFlag
Dim strSFileName,strDFileName,strTemp
Dim strSPathFile,strDPathFile,strTempPathFile
Dim strSSheetName,strDSheetName
Dim intLoopPageNumber,intLoopPageCount			'-- 外回圈使用
Dim intPageNumber,intPageCount
Dim intRecordNumber,intRecordCount
Dim strCopyStartRow,strCopyEndRow,intPageRow
Dim objXLS
'==========================================================
Dim oRs0,StrSql0,strWhere0
Dim oRs2,StrSql2,strWhere2
Dim oRs3,StrSql3,strWhere3

Dim strDate1,strDate2
Dim strTime1,strTime2,strTime1Temp,strTime2Temp
Dim strDateTime1,strDateTime2
Dim intPeople
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
	
	
	strKey=Trim(Request("strKey"))
	
	'-- 預設日期 今天,目前只可查單天
	If strDate1="" Then strDate1=Date()
	If strDate2="" Then strDate2=Date()
	strDate1=Year(strDate1) & "-" & STRING(2-LEN(Month(strDate1)),"0") & Month(strDate1) & "-" & STRING(2-LEN(Day(strDate1)),"0") & Day(strDate1)
	strDate2=Year(strDate2) & "-" & STRING(2-LEN(Month(strDate2)),"0") & Month(strDate2) & "-" & STRING(2-LEN(Day(strDate2)),"0") & Day(strDate2)
	
	'-- 預設時間 早上8 下午16:00
	If strTime1="" Then strTime1="01"
	If strTime2="" Then strTime2="24"
	
	'-- 這兩個值有加工過,是要做查詢用
	If strTime1 <> "" Then strTime1Temp=strTime1 & ":00:00"
	If strTime2 <> "" Then strTime2Temp=strTime2 & ":00:00"
	
	'-- 日期+時間
	strDateTime1=strDate1 & " " & strTime1Temp
	strDateTime2=strDate2 & " " & strTime2Temp
	'RESPONSE.WRITE strDateTime1 & "<HR>" 
	'RESPONSE.WRITE strDateTime2 & "<HR>" 
	'================================
	
	'-- 如果第一次進來或不是數字 就帶Session("ID")
	If Not IsNumeric(intPeople) then intPeople=Session("ID") Else intPeople=CLNG(intPeople)
	'================================
	
	strQS="Date1="&strDate1&"&Date2="&strDate2&"&Time1="&strTime1&"&Time2="&strTime2&"&People="&intPeople&"&strkey="&strKey
	'RESPONSE.WRITE strQS & "<HR>"
	'RESPONSE.END
	'======================================================
	
	'If strStartDate="" OR strStartDate="" Then 
	'	Response.Write "請選擇 起始日期 或 結束日期 !!"
	'	Response.End 
	'End IF 	
	
	'-- 列印頁數預設為1,本參數如果沒有值,Sheet2會無法清空--
	intLoopPageCount=1
	'======================================================

	' ' '-- 第0組資料錄----------------------------------------
	' ' '-- 加入查詢條件-----------------------------
	' ' strWhere0=" Where "
	' ' strWhere0=strWhere0 & " 訂單序號 = " & p0 
	
	' ' '============================================
	' ' '-- 組合查詢字串-----------------------------
	' ' StrSQL0 = ""
	' ' StrSQL0 = StrSQL0 & " Select * "
	' ' StrSQL0 = StrSQL0 & " From 訂單主檔 "
	' ' StrSQL0 = StrSQL0 & StrWhere0
	' ' StrSQL0=StrSQL0 & " Order By 訂單序號  "

	' ' 'RESPONSE.WRITE StrSQL0 & "<BR>"
	' ' 'RESPONSE.END
	' ' '======================================================

	' ' '-- 取得資料錄0----------------------------------------
	' ' Set oRs0=ExecSQL_RTN_RST(StrSQL0,3,0,1)
	' ' intLoopPageCount=oRs0.RecordCount
	' ' '======================================================

	' ' If intLoopPageCount <= 0 Then
		' ' oRs0.close
		' ' Set oRs0 = Nothing
		' ' RESPONSE.WRITE "無資料可供下載!!"
		' ' RESPONSE.END
	' ' End If

	'-- 取得範例檔,轉換檔名,另存新檔-----------------------------
    strSFileName = "統計表.xls"
	'strSFileName = "111.xls"
	strDFileName = Left(strSFileName,Len(strSFileName)-4) & "_" & Replace(Date, "/", "") & Hour(Time) & Minute(Time) & Second(Time) & Int((1000 * Rnd) + 1) & ".XLS"
    'strDFileName = "001" & "_" & Replace(Date, "/", "") & Hour(Time) & Minute(Time) & Second(Time) & Int((1000 * Rnd) + 1) & ".XLS"
    strSPathFile = Server.MapPath(strSFileName)
    strTempPathFile="../../_DLoadTmp/ExcelSample/" & strDFileName
    strDPathFile = Server.MapPath(strTempPathFile )
	Call CopyFile(strSPathFile,strDPathFile)
	'On Error Resume Next
	'============================================================
	

	'-- 建立 EXCEL 物件----------------------
	Set objXLS =Server.CreateObject("ExcelObj2003.ExcelDLL")
	'-- 呼叫 DLL 副程式
    Call objXLS.CreateAP (strDPathFile)
    '-- 呼叫 DLL 副程式,設定 EXCEL 頁面資料
    Call SetPageData()

	
	
	
	
	
	
	'-- 第1組資料錄----------------------------------------
	
	'-- 加入查詢條件---------------------------------------
	strWhere=""
	strWhere=MakeWhere(strWhere,"AND","新增人員","=",intPeople,-1,"N","","")
	
	'-- CONVERT(NVARCHAR(19),新增時間,120) YYYY-MM-DD HH:MM:SS
	strWhere=MakeWhere(strWhere,"AND","CONVERT(NVARCHAR(19),新增時間,120)",">=",strDateTime1,"","S","","")
	strWhere=MakeWhere(strWhere,"AND","CONVERT(NVARCHAR(19),新增時間,120)","<=",strDateTime2,"","S","","")

	'-- 關鍵字查詢---------------------------------------
	'strWhere=MakeWhereKW(strWhere,"AND",Array(""),"%LIKE%",strKey,"","S","","")
	'======================================================

	strWhere=MakeWhereEnd(strWhere)
	'RESPONSE.WRITE strWhere & "<HR>"
	'RESPONSE.END
	'======================================================

	'-- 組合查詢字串---------------------------------------
	StrSql= " Select "
	StrSql=StrSql & "'" & strDate1 & "' AS 新增日期1 ,"
	StrSql=StrSql & "'" & strDate2 & "' AS 新增日期2 ,"
	StrSql=StrSql & "'" & strTime1 & "' AS 新增時間1 ,"
	StrSql=StrSql & "'" & strTime2 & "' AS 新增時間2 ,"
	StrSql=StrSql & " ISNULL((Select SUM(數量金額) AS 數量金額 FROM VW_每日統計表 " & StrWhere & " AND 產品排序=1 ),0)AS P1 ,"
	StrSql=StrSql & " ISNULL((Select SUM(數量金額) AS 數量金額 FROM VW_每日統計表 " & StrWhere & " AND 產品排序=2 ),0)AS P2 ,"
	StrSql=StrSql & " ISNULL((Select SUM(數量金額) AS 數量金額 FROM VW_每日統計表 " & StrWhere & " AND 產品排序=3 ),0)AS P3 ,"
	StrSql=StrSql & " ISNULL((Select SUM(數量金額) AS 數量金額 FROM VW_每日統計表 " & StrWhere & " AND 產品排序=4 ),0)AS P4 ,"
	StrSql=StrSql & " ISNULL((Select SUM(數量金額) AS 數量金額 FROM VW_每日統計表 " & StrWhere & " AND 產品排序=5 ),0)AS P5 ,"
	StrSql=StrSql & " ISNULL((Select SUM(數量金額) AS 數量金額 FROM VW_每日統計表 " & StrWhere & " AND 產品排序=6 ),0)AS P6 ,"
	StrSql=StrSql & " ISNULL((Select SUM(數量金額) AS 數量金額 FROM VW_每日統計表 " & StrWhere & " AND 產品排序=7 ),0)AS P7 ,"
	StrSql=StrSql & " ISNULL((Select SUM(數量金額) AS 數量金額 FROM VW_每日統計表 " & StrWhere & " AND 產品排序=8 ),0)AS P8 ,"
	StrSql=StrSql & " ISNULL((Select SUM(數量金額) AS 數量金額 FROM VW_每日統計表 " & StrWhere & " AND 產品排序=9 ),0)AS P9 ,"
	StrSql=StrSql & " ISNULL((Select SUM(數量金額) AS 數量金額 FROM VW_每日統計表 " & StrWhere & " AND 產品排序=10 ),0)AS P10 ,"
	StrSql=StrSql & " ISNULL((Select SUM(數量金額) AS 數量金額 FROM VW_每日統計表 " & StrWhere & " AND 產品排序=11 ),0)AS P11 ,"
	StrSql=StrSql & " ISNULL((Select SUM(數量金額) AS 數量金額 FROM VW_每日統計表 " & StrWhere & " AND 產品排序=12 ),0)AS P12 ,"
	StrSql=StrSql & " ISNULL((Select TOP 1 新增人員姓名 FROM VW_每日統計表 " & StrWhere & " ),0)AS 新增人員姓名 ,"
	StrSql=StrSql & "'" &  Date() & "' AS 列印日期 "

	
    'RESPONSE.WRITE StrSql  & "<HR>"
	'RESPONSE.END
	'======================================================

	'########################################
	'-- 取得資料錄1--------------------------
	Set oRs=ExecSQL_RTN_RST(StrSQL,3,0,1)
	intRecordCount=oRs.RecordCount
	'========================================

	'-- 將資料1依序塞入 Excel----------------
	'If intRecordCount <=0 Then
		intDataFlag=1
		intPageCount=1
		intPageNumber=1
		'-- 呼叫 DLL 副程式,設定資料位置對應
		Call SetFieldsPositionMatch(intDataFlag)

		'-- 將資料錄資料存入陣列-----------------
		For intRecordNumber = 1 To intRecordCount
			Call SetRstToArray(oRs,aryFieldsData,intRecordNumber-1,intDataFlag)
			oRs.MoveNext
		Next'====================================
		
		Call objXLS.PutArrayDataToEXCEL(aryFieldsPosition, aryFieldsData,intPageNumber,0)
	'End If
	'########################################

	intLoopPageNumber=intLoopPageNumber+1
	'-- 呼叫 DLL 副程式,EXCEL 分頁
	Call objXLS.SplitPage( strDSheetName, strSSheetName, strCopyStartRow, strCopyEndRow,intPageRow ,intLoopPageNumber,intLoopPageCount)

	'oRs0.MoveNext
	'Wend


	'-- 關閉DLL物件,釋放DLL物件----------------------------
    objXLS.CloseAP
    Set objXLS = Nothing
    '======================================================

	'-- 關閉物件,釋放物件----------------------------------
	'ReleaseObj(Array(oRs))
    '======================================================

	'-- 把網頁轉入其他頁面處裡-----------------------------
	Dim strURL
	'strURL = "../_ExcelOutput/EEDirectDL.asp?Title=" & strDFileName & "&PathFile=" & strTempPathFile & "&FileName=" & strDFileName & "&TTL=" & Timer()
	strURL = "../_ExcelOutput/EEDownLoad.asp?Title="&Server.URLEncode(strDFileName)
	strURL = strURL & "&PathFile="&Server.URLEncode(strTempPathFile)
	strURL = strURL & "&FileName="&Server.URLEncode(strDFileName )
	strURL = strURL & "&TTL=" & Timer()
	'Response.WRITE strURL 
	Response.Redirect strURL
	RESPONSE.END
	'======================================================



'-- 設定 EXCEL 頁面資料------------------------------------
Sub SetPageData()

	'-- 範例檔工作表名稱
	strSSheetName="Sheet2"
	'-- 資料檔工作表名稱
	strDSheetName="Sheet1"

	'-- 複製起始列
	strCopyStartRow=1
	'-- 複製結束列
	strCopyEndRow=24	
	'-- 總列數
	intPageRow=24

End Sub'===================================================


'-- 陣列與EXCEL CELL 對應----------------------------------
Sub SetFieldsPositionMatch(varDataFlag)
	Select Case varDataFlag
	Case 1
		'-- 重新宣告陣列
	   	ReDim aryFieldsPosition(15)
		ReDim aryFieldsData(15)

		aryFieldsPosition(0) = "C5"	
		aryFieldsPosition(1) = "C6"	
		aryFieldsPosition(2) = "D8"
		aryFieldsPosition(3) = "D9"
		aryFieldsPosition(4) = "D10"
		aryFieldsPosition(5) = "D11"
		aryFieldsPosition(6) = "D12"
		aryFieldsPosition(7) = "D13"
		aryFieldsPosition(8) = "D14"
		aryFieldsPosition(9) = "D15"
		aryFieldsPosition(10) = "D16"
		aryFieldsPosition(11) = "D17"		
		aryFieldsPosition(12) = "D18"		
		aryFieldsPosition(13) = "D19"		
		aryFieldsPosition(14) = "D21"	
		aryFieldsPosition(15) = "D23"	
		
		
	Case 2
	   	ReDim aryFieldsPosition(0)
		ReDim aryFieldsData(11,2)
		

	End Select

End Sub'===================================================

'-- 指定資料錄資料給陣列-----------------------------------
Sub SetRstToArray(ByRef varORS,ByRef aryFieldsData, varIntRecordNumber,varDataFlag)
Dim tempValue1,tempValue2
	Select Case varDataFlag
	Case 1
		'Year(varORS("新增日期1"))-1911 & " 年 " & Month(varORS("新增日期1")) & " 月 " & Day(varORS("新增日期1")) & " 日"
		tempValue1=Year(varORS("新增日期1"))-1911 & "/" & Month(varORS("新增日期1")) & "/" & Day(varORS("新增日期1")) 
		tempValue2=Year(varORS("新增日期2"))-1911 & "/" & Month(varORS("新增日期2")) & "/" & Day(varORS("新增日期2")) 
		aryFieldsData(0) = tempValue1 & " ~ " & tempValue2
		aryFieldsData(1) = varORS("新增時間1") & ":00"& " ~ " & varORS("新增時間2") & ":00"
		aryFieldsData(2) = varORS("P1")
		aryFieldsData(3) = varORS("P2")
		aryFieldsData(4) = varORS("P3")
		aryFieldsData(5) = varORS("P4")
		aryFieldsData(6) = varORS("P5")
		aryFieldsData(7) = varORS("P6")
		aryFieldsData(8) = varORS("P7")
		aryFieldsData(9) = varORS("P8")
		aryFieldsData(10) = varORS("P9")
		aryFieldsData(11) = varORS("P10")
		aryFieldsData(12) = varORS("P11") & "面"
		aryFieldsData(13) = varORS("P12") & "斤"
		aryFieldsData(14) = varORS("新增人員姓名")
		aryFieldsData(15) = varORS("列印日期")

	Case 2
		aryFieldsData(varIntRecordNumber,0) = varORS("申請人姓名")
		aryFieldsData(varIntRecordNumber,1) = varORS("產品名稱")
		aryFieldsData(varIntRecordNumber,2) = varORS("數量金額")
	
	End Select

End Sub'===================================================

%>