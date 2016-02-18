<%Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/Power_Function.asp"-->
<!--#include file="../../_Function/File_Function.asp"-->


<%
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
	
	
	'-- 列印頁數預設為1,本參數如果沒有值,Sheet2會無法清空--
	intLoopPageCount=1
	'======================================================
	


	'-- 取得範例檔,轉換檔名,另存新檔-----------------------------
    strSFileName = "太歲卡.xls"
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
	strCopyEndRow=31
	'-- 總列數
	intPageRow=31
	'===========================================================
	
	
	
	'-- 第1組資料錄----------------------------------------
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
	
	StrSql=StrSql & " REPLACE(A.申請人地址,'空白空白','') AS 申請人地址,"

	StrSql=StrSql & " (SELECT TOP 1 星君 FROM 年度生肖表 WHERE 民國年=DBO.JKFUN_ROCDate_Split(A.申請人生日,'YY')) AS 星君, " 
	StrSql=StrSql & " DBO.JKFUN_ROCDate_Split(A.申請人生日,'YY') AS 生日年,"
	StrSql=StrSql & " DBO.JKFUN_ROCDate_Split(A.申請人生日,'MM') AS 生日月,"
	StrSql=StrSql & " DBO.JKFUN_ROCDate_Split(A.申請人生日,'DD') AS 生日日,"
	StrSql=StrSql & " A.申請人時辰,"

	StrSql=StrSql & " (SELECT CASE isOnLeapMonth WHEN 1 THEN '閏' ELSE '' END FROM 會員資料表 WHERE 序號=A.會員編號) AS 閏月,"
	
	StrSql=StrSql & " CONVERT(NVARCHAR(10),A.新增時間,111) AS 新增時間 "
	StrSql=StrSql & " FROM 訂單明細檔 AS A"
	StrSql=StrSql & StrWhere
	StrSql=StrSql & " ORDER BY A.申請人年齡 DESC"

    'RESPONSE.WRITE StrSql  & "<HR>"
	'RESPONSE.END
	'======================================================

	'-- 取得資料錄1----------------------------------------
	Set oRs=ExecSQL_RTN_RST(StrSQL,3,0,1)
	'======================================================
	
	
	

	'------------------------------------------------------------
	Dim intLoopPageNumber			'外回圈使用
	Dim intLoopPageCount			'外回圈使用
	
	Dim intLabelColumns				'每列標籤個數
	Dim intLabelRows				'每頁標籤列數	
	Dim intSplitPageRecords			'每頁標籤總數(也是分頁的筆數)
	
	Dim intLabelFields		    	'每個標籤資料_欄位數 
	Dim intLabelFieldsArrayPos		'每個標籤陣列_位置
	Dim intLabelFieldsArrayPosTotal	'每個標籤陣列_位置加總
	'--------------------------------------------
	
	
	'-- 下列需要設定(每次)-----------------------
	intLabelColumns=3				'每列標籤數
	intLabelRows=2					'每頁標籤列數
	intLabelFields=10				'每個標籤資料_欄位數
	intLabelFieldsArrayPos=10		'每個標籤陣列_位置	   (這一個變數很特殊,其值通常會= [每個標籤資料_欄位數 intLabelFields] )
	intLabelFieldsArrayPosTotal=0	'每個標籤陣列_位置加總 (這一個變數很特殊)
	'============================================
	
	'-- 取得要輸出資料的總筆數-------------------
	intRecordCount=oRs.RecordCount
	'RESPONSE.WRITE intRecordCount& "<HR>" 
	'============================================
	
	'-- 每個頁面標籤數量總數---------------------
	'-- (也就是每頁分頁的資料筆數)
	intSplitPageRecords = intLabelColumns * intLabelRows
	'RESPONSE.WRITE intSplitPageRecords & "<HR>" 
	'============================================

	'-- 呼叫函數 取得分頁後總頁數----------------
	intLoopPageCount=GetPageCounts(intRecordCount,intSplitPageRecords)
	'RESPONSE.WRITE "資料總筆數:" & intRecordCount & "，　每頁筆數:" & intSplitPageRecords & "，　總共頁數:" & intLoopPageCount & "<HR>" 
	'============================================
	
	'-- 呼叫 DLL 副程式,設定資料位置對應---------
	intDataFlag=1
	Call SetFieldsPositionMatch_LabelStyle(intDataFlag,intLabelColumns * intLabelRows * intLabelFields - 1)
	'============================================
	
	
	
	'###############################################################################################
	'-- A.迴圈第一層----------------------------------------
	'-- 跑 總頁 回圈
	'-- 由 1 到 算出的總頁數
	'-- 標籤列印由左自右,由上而下
	For intLoopPageNumber = 1 To intLoopPageCount
		
		'-- 重新定義陣列大小(不可放到分頁回圈外,因為資料會殘留)
		'-- 每頁有資料的欄位
		ReDim aryFieldsData( intLabelColumns * intLabelRows * intLabelFields - 1)
			
		'-- 偵錯用
		'RESPONSE.WRITE "目前頁碼:" & intLoopPageNumber & ",　總共頁數:" & intLoopPageCount & "<HR>" 
		
		'-- 每個標籤資料跳欄數加總
		intLabelFieldsArrayPosTotal=0
		
		'-- B.迴圈第二層-----------------------------------
		'-- 跑 單頁_總標籤_列數 回圈
		'-- 依序將每頁標籤列數資料跑回圈
		For N= 1 TO intLabelRows

			'-- C.迴圈第三層-------------------------------
			'-- 跑 單頁_列數_欄數 回圈
			'-- 依序將每一列標籤個數資料跑回圈
			For NN= 1 TO intLabelColumns
				
				'-- 如果資料錄是空的就離開
				If oRs.Eof Then Exit For
				
				'-- 偵錯用
				'Response.Write oRs("申請人姓名") & "," & oRs("郵遞區號") & "&nbsp;&nbsp;&nbsp;&nbsp;"
				Response.Write intLabelFieldsArrayPosTotal & "," & intLabelFieldsArrayPosTotal + 1 & "," & intLabelFieldsArrayPosTotal + 2  & "&nbsp;&nbsp;&nbsp;&nbsp;"
				
				'-- D.第四層-------------------------------
				'-- 跑 單一標籤_欄位
				'-- intLabelFieldsArrayPosTotal 為特殊的變數
				aryFieldsData(intLabelFieldsArrayPosTotal+0)=oRs("星君")
				aryFieldsData(intLabelFieldsArrayPosTotal+1)=oRs("申請人姓名")
				aryFieldsData(intLabelFieldsArrayPosTotal+2)=oRs("申請人性別")
				aryFieldsData(intLabelFieldsArrayPosTotal+3)=oRs("申請人年齡")
				aryFieldsData(intLabelFieldsArrayPosTotal+4)=oRs("申請人地址")
				aryFieldsData(intLabelFieldsArrayPosTotal+5)=oRs("生日年")
				aryFieldsData(intLabelFieldsArrayPosTotal+6)=oRs("生日月")
				aryFieldsData(intLabelFieldsArrayPosTotal+7)=oRs("生日日")
				aryFieldsData(intLabelFieldsArrayPosTotal+8)=oRs("申請人時辰")
				aryFieldsData(intLabelFieldsArrayPosTotal+9)=oRs("閏月")


				'-- 每個標籤資料跳欄數加總-----
				'-- 這裡只用一維變數,所以每跑一標籤,就必須將標籤數加總
				intLabelFieldsArrayPosTotal=intLabelFieldsArrayPosTotal+intLabelFieldsArrayPos
				'==============================
				'==========================================
				
				
				'-- 移到下一筆
				oRs.MoveNext
				
			Next
			RESPONSE.WRITE "<HR>" 
			
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
	'###############################################################################################
	

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
'-- Jack 20111231 Added
'-- varTotalRecordCount 資料總筆數
'-- varSplitPageRecords 每頁幾筆資料
Function GetPageCounts(varTotalRecordCount,varSplitPageRecords)
Dim varTotalPageCount

	'-- 資料總筆數 MOD 資料分頁筆數 > 0  ==>(有餘數,沒有整除)
	If varTotalRecordCount Mod varSplitPageRecords > 0 Then
		'-- 資料總頁數 = INT(資料總筆數 / 資料分頁筆數 ) + 1  ==>(取商數,轉整數,再加1)
		varTotalPageCount = Int(varTotalRecordCount / varSplitPageRecords) + 1

	Else
		'-- 資料總頁數 = INT(資料總筆數 / 資料分頁筆數 )
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
'-- Jack 20111231 Added
'-- 報表為標籤型態,適用以下副程式
'-- 來設定每單一個標籤內容之欄位的對應位置
'-- varDataFlag 通常為 1
'-- ArrayItems : 陣列大小(標籤 ROW * COL * 單一標籤欄位資料數)
Sub SetFieldsPositionMatch_LabelStyle(varDataFlag,ArrayItems)
	Select Case varDataFlag
	Case 1
		'-- 重新宣告陣列
	   	ReDim aryFieldsPosition(ArrayItems)
		
		'-- 第一列------------------------
		aryFieldsPosition(0) = "C2"
		aryFieldsPosition(1) = "D4"
		aryFieldsPosition(2) = "E12"
		aryFieldsPosition(3) = "D12"
		aryFieldsPosition(4) = "C4"
		aryFieldsPosition(5) = "B4"
		aryFieldsPosition(6) = "B7"
		aryFieldsPosition(7) = "B10"
		aryFieldsPosition(8) = "B13"
		aryFieldsPosition(9) = "B5"

		aryFieldsPosition(10) = "H2"
		aryFieldsPosition(11) = "I4"
		aryFieldsPosition(12) = "J12"
		aryFieldsPosition(13) = "I12"
		aryFieldsPosition(14) = "H4"
		aryFieldsPosition(15) = "G4"
		aryFieldsPosition(16) = "G7"
		aryFieldsPosition(17) = "G10"
		aryFieldsPosition(18) = "G13"
		aryFieldsPosition(19) = "G5"

		aryFieldsPosition(20) = "M2"
		aryFieldsPosition(21) = "N4"
		aryFieldsPosition(22) = "O12"
		aryFieldsPosition(23) = "N12"
		aryFieldsPosition(24) = "M4"
		aryFieldsPosition(25) = "L4"
		aryFieldsPosition(26) = "L7"
		aryFieldsPosition(27) = "L10"
		aryFieldsPosition(28) = "L13"
		aryFieldsPosition(29) = "L5"

		'-- 第二列------------------------
		aryFieldsPosition(30) = "C17"
		aryFieldsPosition(31) = "D19"
		aryFieldsPosition(32) = "E27"
		aryFieldsPosition(33) = "D27"
		aryFieldsPosition(34) = "C19"
		aryFieldsPosition(35) = "B19"
		aryFieldsPosition(36) = "B22"
		aryFieldsPosition(37) = "B25"
		aryFieldsPosition(38) = "B28"
		aryFieldsPosition(39) = "B20"

		aryFieldsPosition(40) = "H17"
		aryFieldsPosition(41) = "I19"
		aryFieldsPosition(42) = "J27"
		aryFieldsPosition(43) = "I27"
		aryFieldsPosition(44) = "H19"
		aryFieldsPosition(45) = "G19"
		aryFieldsPosition(46) = "G22"
		aryFieldsPosition(47) = "G25"
		aryFieldsPosition(48) = "G28"
		aryFieldsPosition(49) = "G20"

		aryFieldsPosition(50) = "M17"
		aryFieldsPosition(51) = "N19"
		aryFieldsPosition(52) = "O27"
		aryFieldsPosition(53) = "N27"
		aryFieldsPosition(54) = "M19"
		aryFieldsPosition(55) = "L19"
		aryFieldsPosition(56) = "L22"
		aryFieldsPosition(57) = "L25"
		aryFieldsPosition(58) = "L28"
		aryFieldsPosition(59) = "L20"

	End Select

End Sub'===================================================



'###################################################################################################
%>