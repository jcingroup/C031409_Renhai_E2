<%Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/Power_Function.asp"-->
<!--#include file="../../_Function/File_Function.asp"-->


<%
	Server.ScriptTimeOut = 3600


Dim strScript,strPageNo,strQS
Dim oRs,StrSql,strWhere
Dim N,NN
'-- ExcelExport -------------------------------------------
Dim aryFieldsPosition
Dim aryFieldsData
Dim intDataFlag
Dim strSFileName,strDFileName,strTemp
Dim strSPathFile,strDPathFile,strTempPathFile
Dim strSSheetName,strDSheetName

Dim intPageNumber,intPageCount
Dim intRecordNumber,intRecordCount
Dim strCopyStartRow,strCopyEndRow,intPageRow
Dim objXLS
'==========================================================
Dim oRs0,StrSql0,strWhere0
Dim oRs2,StrSql2,strWhere2
Dim oRs3,StrSql3,strWhere3

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
	
	'-- 列印頁數預設為1,本參數如果沒有值,Sheet2會無法清空--
	intLoopPageCount=1
	'======================================================
	


	'-- 取得範例檔,轉換檔名,另存新檔-----------------------------
    strSFileName = "郵寄標籤.xls"
    strDFileName = Left(strSFileName,Len(strSFileName)-4) & "_" & Replace(Date, "/", "") & Hour(Time) & Minute(Time) & Second(Time) & Int((1000 * Rnd) + 1) & ".XLS"
    strSPathFile = Server.MapPath(strSFileName)
    strTempPathFile="../../_DLoadTmp/ExcelSample/" & strDFileName
    strDPathFile = Server.MapPath(strTempPathFile )
	Call CopyFile(strSPathFile,strDPathFile)
	'On Error Resume Next
	'============================================================

	'-- 建立 EXCEL 物件------------------------------------------
	Set objXLS =Server.CreateObject("ExcelObj2003.ExcelDLL")
	'-- 呼叫 DLL 副程式
    Call objXLS.CreateAP (strDPathFile)

	'-- 設定 EXCEL 頁面資料--------------------------------------
	'-- 範例檔工作表名稱
	strSSheetName="Sheet2"
	'-- 資料檔工作表名稱
	strDSheetName="Sheet1"

	'-- 複製起始列
	strCopyStartRow=1
	'-- 複製結束列
	strCopyEndRow=6	
	'-- 總列數
	intPageRow=6
	'===========================================================
	
	
	
	'-- 第1組資料錄----------------------------------------
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

	
	strWhere = ""
	If 	strZipCode ="Other" Then
		strWhere=MakeWhere(strWhere,"AND","A.郵遞區號"," NOT IN ","('320','324','326')","","N","","")
	Else
		strWhere=MakeWhere(strWhere,"AND","A.郵遞區號","=",strZipCode,"","S","","")
	End IF
	
	strWhere=MakeWhereEnd(strWhere)
	
	StrSql = "SELECT 戶長SN,姓名 as 申請人姓名, 郵遞區號, 地址 as 申請人地址 FROM  會員戶長資料 As A " & StrWhere &  " Order By 姓名"
	
    'RESPONSE.WRITE StrSql  & "<HR>"
	'RESPONSE.END
	'======================================================
	'########################################
	'-- 取得資料錄1--------------------------
	Set oRs=ExecSQL_RTN_RST(StrSQL,3,0,1)
	intRecordCount=oRs.RecordCount
	'RESPONSE.WRITE intRecordCount& "<HR>" 
	'========================================
	'------------------------------------------------------------
	Dim intLoopPageNumber		'外回圈使用
	Dim intLoopPageCount		'外回圈使用
	
	Dim intLabelColumns			'每列標籤個數
	Dim intLabelRows			'每頁標籤列數	
	Dim intSplitPageRecords		'每頁標籤總數(也是分頁的筆數)
	
	Dim intLabelFields		    	'每個標籤資料_欄位數 
	Dim intLabelFieldsArrayPos		'每個標籤陣列_位置
	Dim intLabelFieldsArrayPosTotal	'每個標籤陣列_位置加總
	'--------------------------------------------
	
	intLabelColumns=11				'每列標籤數
	intLabelRows=2					'每頁標籤列數
	
	intLabelFields=3				'每個標籤資料_欄位數
	intLabelFieldsArrayPos=3		'每個標籤陣列_位置(此值通常為 intLabelFields-1)
	intLabelFieldsArrayPosTotal=0	'每個標籤陣列_位置加總
	
	'-- 每頁標籤總數(也是分頁的筆數)
	intSplitPageRecords = intLabelColumns * intLabelRows

	'-- 呼叫函數 取得分頁後總頁數----------------
	intLoopPageCount=GetPageCounts(intRecordCount,intSplitPageRecords)
	'RESPONSE.WRITE "資料總筆數:" & intRecordCount & "，　每頁筆數:" & intSplitPageRecords & "，　總共頁數:" & intLoopPageCount & "<HR>" 
	'============================================
	
	
	'-- 呼叫 DLL 副程式,設定資料位置對應---------
	intDataFlag=1
	Call SetFieldsPositionMatch_LabelStyle(intDataFlag,intLabelColumns * intLabelRows * intLabelFields - 1)
	'============================================
	
	'-- 跑回圈分頁---------------------------------------
	'-- 標籤列印由左自右,由上而下
	For intLoopPageNumber = 1 To intLoopPageCount
		
		'-- 重新定義陣列大小(不可放到分頁回圈外,因為資料會殘留)
		ReDim aryFieldsData( intLabelColumns * intLabelRows * intLabelFields - 1)
			
		'-- 偵錯用
		'RESPONSE.WRITE "目前頁碼:" & intLoopPageNumber & ",　總共頁數:" & intLoopPageCount & "<HR>" 
		
		'-- 每個標籤資料跳欄數加總
		intLabelFieldsArrayPosTotal=0
		
		'-- 依序將每頁標籤列數資料跑回圈
		For N= 1 TO intLabelRows

			'-- 依序將每一列標籤個數資料跑回圈
			For NN= 1 TO intLabelColumns
				
				'-- 如果資料錄是空的就離開
				If oRs.Eof Then Exit For
				
				'-- 偵錯用
				'Response.Write oRs("申請人姓名") & "," & oRs("郵遞區號") & "&nbsp;&nbsp;&nbsp;&nbsp;"
				'Response.Write intLabelFieldsArrayPosTotal & "," & intLabelFieldsArrayPosTotal + 1 & "," & intLabelFieldsArrayPosTotal + 2  & "&nbsp;&nbsp;&nbsp;&nbsp;"
				
				aryFieldsData(intLabelFieldsArrayPosTotal)=oRs("申請人姓名")
				aryFieldsData(intLabelFieldsArrayPosTotal+1)=oRs("郵遞區號")
				aryFieldsData(intLabelFieldsArrayPosTotal+2)=oRs("申請人地址")

				'-- 每個標籤資料跳欄數加總 
				intLabelFieldsArrayPosTotal=intLabelFieldsArrayPosTotal+intLabelFieldsArrayPos
				
				'-- 移到下一筆
				oRs.MoveNext
			Next
			'RESPONSE.WRITE "<HR>" 
			
			'-- 如果資料錄是空的就離開
			If oRs.Eof Then Exit For
		Next

		'RESPONSE.WRITE UBound(aryFieldsPosition)& "<HR>" 
		'RESPONSE.WRITE UBOUND(aryFieldsData)& "<HR>" 
		
		'-- 呼叫 DLL 副程式,把資料指定給 EXCEL
		intPageNumber=1
		Call objXLS.PutArrayDataToEXCEL(aryFieldsPosition, aryFieldsData,intPageNumber,0)						
	
		
		'-- 呼叫 DLL 副程式,EXCEL 分頁
		Call objXLS.SplitPage( strDSheetName, strSSheetName, strCopyStartRow, strCopyEndRow,intPageRow ,intLoopPageNumber,intLoopPageCount)

	Next'==================================================


	'-- 關閉DLL物件,釋放DLL物件----------------------------
    objXLS.CloseAP
    Set objXLS = Nothing
    '======================================================

	'-- 關閉物件,釋放物件----------------------------------
	ReleaseObj(Array(oRs))
    '======================================================
	
	'RESPONSE.END
	
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



'###################################################################################################
'-- 取得分頁總頁數-----------------------------------------
'-- varTotalRecordCount 資料總筆數
'-- varSplitPageRecords 每頁幾筆資料
Function GetPageCounts(varTotalRecordCount,varSplitPageRecords)
Dim varTotalPageCount

	'-- 資料總筆數 MOD 資料分頁筆數 > 0  
	If varTotalRecordCount Mod varSplitPageRecords > 0 Then
		varTotalPageCount = Int(varTotalRecordCount / varSplitPageRecords) + 1

	Else
		varTotalPageCount = Int(varTotalRecordCount / varSplitPageRecords)
		'-- 頁數一定要大於0
		If varTotalPageCount = 0 Then varTotalPageCount = 1

	End If'======================================
	
	'-- 將總頁數回傳
	GetPageCounts=varTotalPageCount
End Function'==============================================





'-- 陣列與EXCEL CELL 對應----------------------------------
Sub SetFieldsPositionMatch(varDataFlag)
	Select Case varDataFlag
	Case 1
		'-- 重新宣告陣列
	   	'ReDim aryFieldsPosition(1)
		'ReDim aryFieldsData(1)

		'aryFieldsPosition(0) = "B2"	
		'aryFieldsPosition(1) = "B3"
	
	Case 2
	   	'ReDim aryFieldsPosition(0)
		'ReDim aryFieldsData(38,11)
	End Select

End Sub'===================================================





'-- 指定資料錄資料給陣列-----------------------------------
Sub SetRstToArray(ByRef varORS,ByRef aryFieldsData, varIntRecordNumber,varDataFlag)
Dim tempValue1,tempValue2
	Select Case varDataFlag
	Case 1
	
		'aryFieldsData(0) = varORS("")
		'aryFieldsData(1) = varORS("") 
		

	Case 2
		'aryFieldsData(0,varIntRecordNumber) = varORS("")
		'aryFieldsData(1,varIntRecordNumber) = varORS("")

	
	End Select

End Sub'===================================================



'-- 陣列與EXCEL CELL 對應----------------------------------
Sub SetFieldsPositionMatch_LabelStyle(varDataFlag,ArrayItems)
	Select Case varDataFlag
	Case 1
		'-- 重新宣告陣列
	   	ReDim aryFieldsPosition(ArrayItems)

		aryFieldsPosition(0) = "B2"
		aryFieldsPosition(1) = "C2"
		aryFieldsPosition(2) = "C3"

		aryFieldsPosition(3) = "D2"
		aryFieldsPosition(4) = "E2"
		aryFieldsPosition(5) = "E3"

		aryFieldsPosition(6) = "F2"
		aryFieldsPosition(7) = "G2"
		aryFieldsPosition(8) = "G3"

		aryFieldsPosition(9) = "H2"
		aryFieldsPosition(10) = "I2"
		aryFieldsPosition(11) = "I3"

		aryFieldsPosition(12) = "J2"
		aryFieldsPosition(13) = "K2"
		aryFieldsPosition(14) = "K3"

		aryFieldsPosition(15) = "L2"
		aryFieldsPosition(16) = "M2"
		aryFieldsPosition(17) = "M3"

		aryFieldsPosition(18) = "N2"
		aryFieldsPosition(19) = "O2"
		aryFieldsPosition(20) = "O3"

		aryFieldsPosition(21) = "P2"
		aryFieldsPosition(22) = "Q2"
		aryFieldsPosition(23) = "Q3"

		aryFieldsPosition(24) = "R2"
		aryFieldsPosition(25) = "S2"
		aryFieldsPosition(26) = "S3"

		aryFieldsPosition(27) = "T2"
		aryFieldsPosition(28) = "U2"
		aryFieldsPosition(29) = "U3"

		aryFieldsPosition(30) = "V2"
		aryFieldsPosition(31) = "W2"
		aryFieldsPosition(32) = "W3"

		
		'-- 第二列
		aryFieldsPosition(33) = "B5"
		aryFieldsPosition(34) = "C5"
		aryFieldsPosition(35) = "C6"

		aryFieldsPosition(36) = "D5"
		aryFieldsPosition(37) = "E5"
		aryFieldsPosition(38) = "E6"

		aryFieldsPosition(39) = "F5"
		aryFieldsPosition(40) = "G5"
		aryFieldsPosition(41) = "G6"

		aryFieldsPosition(42) = "H5"
		aryFieldsPosition(43) = "I5"
		aryFieldsPosition(44) = "I6"

		aryFieldsPosition(45) = "J5"
		aryFieldsPosition(46) = "K5"
		aryFieldsPosition(47) = "K6"

		aryFieldsPosition(48) = "L5"
		aryFieldsPosition(49) = "M5"
		aryFieldsPosition(50) = "M6"

		aryFieldsPosition(51) = "N5"
		aryFieldsPosition(52) = "O5"
		aryFieldsPosition(53) = "O6"

		aryFieldsPosition(54) = "P5"
		aryFieldsPosition(55) = "Q5"
		aryFieldsPosition(56) = "Q6"

		aryFieldsPosition(57) = "R5"
		aryFieldsPosition(58) = "S5"
		aryFieldsPosition(59) = "S6"

		aryFieldsPosition(60) = "T5"
		aryFieldsPosition(61) = "U5"
		aryFieldsPosition(62) = "U6"

		aryFieldsPosition(63) = "V5"
		aryFieldsPosition(64) = "W5"
		aryFieldsPosition(65) = "W6"		
	End Select

End Sub'===================================================



'###################################################################################################
%>