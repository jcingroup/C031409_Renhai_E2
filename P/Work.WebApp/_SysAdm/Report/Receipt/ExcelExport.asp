<%Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/Power_Function.asp"-->
<!--#include file="../../_Function/File_Function.asp"-->
<%
Dim strScript,strPageNo
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
Dim tempValue1,tempValue2
Dim p0
	
	'-- UTF-8 偵錯時,要先定義網頁顯示編碼,才可正常顯示不會有亂碼
	Response.Write "<html><head><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""></head>"
	'============================================================
	
	
	'-- 取得前頁轉送相關參數-------------------------------------
	strScript=Request.ServerVariables("SCRIPT_NAME")
	strPageNo=Trim(Request("pageno"))
	p0=Trim(Request("p0"))		'訂單序號
	'If Not IsNumeric(p0) then p0=-1 Else p0=Cint(p0)
	
	'strQS = "del="&intDel&"&sup="&intSup&"&month="&intMonth&"&StartDate="&strStartDate&"&EndDate="&strEndDate
	'RESPONSE.WRITE strQS & "<BR>"
	'RESPONSE.END
	'============================================================

	'-- 列印頁數預設為1,本參數如果沒有值,Sheet2會無法清空--
	intLoopPageCount=1
	'======================================================

	'-- 第0組資料錄----------------------------------------
	'-- 加入查詢條件-----------------------------
	strWhere0=" Where "
	strWhere0=strWhere0 & " 訂單編號 = '" & p0 & "'" 
	
	'============================================
	'-- 組合查詢字串-----------------------------
	StrSQL0 = ""
	StrSQL0 = StrSQL0 & " Select * "
	StrSQL0 = StrSQL0 & " From 訂單主檔 "
	StrSQL0 = StrSQL0 & StrWhere0
	StrSQL0 = StrSQL0 & " Order By 訂單序號  "

	'RESPONSE.WRITE StrSQL0 & "<BR>"
	'RESPONSE.END
	'======================================================

	'-- 取得資料錄0----------------------------------------
	Set oRs0=ExecSQL_RTN_RST(StrSQL0,3,0,1)
	intLoopPageCount=oRs0.RecordCount
	'======================================================

	If intLoopPageCount <= 0 Then
		oRs0.close
		Set oRs0 = Nothing
		RESPONSE.WRITE "無資料可供下載!!"
		RESPONSE.END
	End If

	'-- 取得範例檔,轉換檔名,另存新檔-----------------------------
    strSFileName = "感謝狀.xls"
	Randomize 'Jerry 2013/01/14 初始化Rnd
    strDFileName = Left(strSFileName,Len(strSFileName)-4) & "_" & Replace(Date, "/", "") & Hour(Time) & Minute(Time) & Second(Time) & "." & Int((1000 * Rnd) + 1) & "." & Session("ID") & ".XLS"
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
	'-- 加入查詢條件-----------------------------
	strWhere=" Where "
	strWhere=strWhere & " A.訂單編號 = '" & p0 & "'" 
	'============================================
	'-- 組合查詢字串-----------------------------
	StrSQL=""
	StrSQL= StrSQL & " SELECT  A.訂單序號, A.訂單編號, "
	StrSQL= StrSQL & " A.會員編號, A.申請人姓名, A.申請人電話, A.申請人地址, A.申請人手機, A.申請人性別, A.申請人生日, A.申請人EMAIL, "
	StrSQL= StrSQL & " dbo.JKFUN_NTC(A.總額,0) AS 總額, A.付款方式, A.訂單時間, A.付款時間, A.訂單狀態, "
	StrSQL= StrSQL & " A.查詢序號, A.付款方式名稱, A.訂單狀態名稱, A.銀行帳號, A.新增時間, "
	StrSQL= StrSQL & " A.新增人員, A.郵遞區號, A.戶長SN, "
	StrSQL= StrSQL & " B.姓名, "
	StrSQL= StrSQL & " ISNULL((SELECT SUM(X1.金牌) FROM 訂單明細檔 AS X1 WHERE X1.訂單編號=A.訂單編號 AND X1.產品編號 =10),0) AS 產品_捐金牌, "
	StrSQL= StrSQL & " ISNULL((SELECT SUM(X2.白米) FROM 訂單明細檔 AS X2 WHERE X2.訂單編號=A.訂單編號 AND X2.產品編號 =11),0) AS 產品_捐白米 "

	StrSQL= StrSQL & " From 訂單主檔 AS A "
	StrSQL= StrSQL & " LEFT JOIN 人員 AS B ON B.人員代碼=A.新增人員 "
	
	StrSQL=StrSQL & StrWhere
	StrSQL=StrSQL & " Order By A.訂單編號"
	'Response.Write StrSQL & "<BR>"
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
			'response.write intDataFlag
			Call SetRstToArray(oRs,aryFieldsData,intRecordNumber-1,intDataFlag)
			'response.write "HelloD"
			oRs.MoveNext
		Next'====================================
		Call objXLS.PutArrayDataToEXCEL(aryFieldsPosition, aryFieldsData,intPageNumber,0)
		'Response.End
	'End If
	'########################################
	
	'-- 把主檔移到第一筆
	oRs.MoveFirst
	'Response.Write oRs.RecordCount & "<HR>"
	'Response.Write oRs("訂單編號") & "<HR>"

	'-- 第2組資料錄----------------------------------------
	'-- 加入查詢條件----------------------------
	strWhere2=" Where "
	strWhere2=strWhere2 & " A.訂單編號='" & oRs("訂單編號") & "'"
	'============================================
	'-- 組合查詢字串-----------------------------
	StrSQL2=""
	StrSQL2= StrSQL2 & " SELECT TOP 10 A.申請人姓名,A.產品名稱,A.點燈位置, "
	StrSQL2= StrSQL2 & " CASE A.產品編號 "
	StrSQL2= StrSQL2 & " WHEN 10 Then A.金牌 "
	StrSQL2= StrSQL2 & " WHEN 11 Then A.白米 "
	StrSQL2= StrSQL2 & " ELSE A.價格 "
	StrSQL2= StrSQL2 & " END AS 數量金額 "
	StrSQL2= StrSQL2 & " FROM  訂單明細檔 AS A  "
	StrSQL2=StrSQL2 & StrWhere2
	StrSQL2=StrSQL2 & " Order By 訂單序號 "
	RESPONSE.WRITE StrSQL2 & "<BR>"
	'RESPONSE.END
	'======================================================
	'########################################
	'-- 取得資料錄2--------------------------
	Set oRs2=ExecSQL_RTN_RST(StrSQL2,3,0,1)
	intRecordCount=oRs2.RecordCount
	'========================================
	'-- 將資料2依序塞入 Excel----------------
	'If intRecordCount <=0 Then
		intDataFlag=2
		intPageCount=1
		intPageNumber=1
		'-- 呼叫 DLL 副程式,設定資料位置對應
		Call SetFieldsPositionMatch(intDataFlag)
		' ' '-- 將資料錄資料存入陣列-----------------
		' ' For intRecordNumber = 1 To intRecordCount
			' ' Call SetRstToArray(oRs2,aryFieldsData,intRecordNumber-1,intDataFlag)
			' ' oRs2.MoveNext
		' ' Next'====================================
		
		Dim NN
		'-- 依序將每一列標籤個數資料跑回圈
		While Not oRs2.Eof
			NN=NN+1
			aryFieldsData(NN-1,0)=oRs2("申請人姓名")
			aryFieldsData(NN-1,1)=oRs2("產品名稱")
			aryFieldsData(NN-1,2)=oRs2("數量金額")

			NN=NN+1
			aryFieldsData(NN-1,1)=oRs2("點燈位置")

			oRs2.MoveNext
		Wend
		
		Call objXLS.PutArrayDataToEXCEL_RC(aryFieldsData,2,15,intPageNumber,1)
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
	' ' ReleaseObj(Array(oRs,oRs0))
  	' ' oRs.close
	' ' Set oRs = Nothing
	' ' oRs0.close
	' ' Set oRs0 = Nothing
	' ' oRs2.close
	' ' Set oRs2 = Nothing
	' ' oRs3.close
	' ' Set oRs3 = Nothing
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
	strCopyEndRow=48	
	'-- 總列數
	intPageRow=48

End Sub'===================================================


'-- 陣列與EXCEL CELL 對應----------------------------------
Sub SetFieldsPositionMatch(varDataFlag)
	Select Case varDataFlag
	Case 1

		'-- 重新宣告陣列
	   	ReDim aryFieldsPosition(8)
		ReDim aryFieldsData(8)
		aryFieldsPosition(0) = "J9"
		aryFieldsPosition(1) = "B14"
		aryFieldsPosition(2) = "D16"
		aryFieldsPosition(3) = "D18"
		aryFieldsPosition(4) = "C21"
		aryFieldsPosition(5) = "F21"
		aryFieldsPosition(6) = "H21"
		aryFieldsPosition(7) = "L21"
		aryFieldsPosition(8) = "I23"
		
	Case 2

		ReDim aryFieldsPosition(0)
		ReDim aryFieldsData(19,2)
	End Select

End Sub'===================================================

'-- 指定資料錄資料給陣列-----------------------------------
Sub SetRstToArray(ByRef varORS,ByRef aryFieldsData, varIntRecordNumber,varDataFlag)

	Select Case varDataFlag
	Case 1

		aryFieldsData(0) = varORS("總額")
		aryFieldsData(1) = varORS("申請人姓名")
		aryFieldsData(2) = Replace(varORS("申請人地址"),"空白","")

		tempValue1 = varORS("產品_捐金牌")
		IF tempValue1 > 0 Then
			tempValue2 = "捐金牌 " & tempValue1 & " 面   "
		End IF
		
		tempValue1 = varORS("產品_捐白米")
		IF tempValue1 > 0 Then
			tempValue2 = tempValue2 & "捐白米 " & tempValue1 & " 斤   "
		End IF
		aryFieldsData(3) = tempValue2
		
		aryFieldsData(4) = Year(Date())-1911
		aryFieldsData(5) = Month(Date())
		aryFieldsData(6) = Day(Date())
		aryFieldsData(7) = varORS("姓名")
		   
		tempValue1	=	LEN(varORS("訂單序號"))
		'tempValue2	=	"1" & String(9-tempValue1,"0") & varORS("訂單序號")
		tempValue2 = varORS("訂單編號")
		'response.write tempValue1 & "<br />"
		'response.write tempValue2
		
		aryFieldsData(8) = tempValue2

	Case 2
		' ' aryFieldsData(varIntRecordNumber,0) = varORS("申請人姓名")
		' ' aryFieldsData(varIntRecordNumber,1) = varORS("產品名稱")
		' ' aryFieldsData(varIntRecordNumber,2) = varORS("數量金額")
		' ' aryFieldsData(varIntRecordNumber,3) = varORS("點燈位置")
	End Select

End Sub'===================================================

%>