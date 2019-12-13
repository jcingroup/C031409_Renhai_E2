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
Dim intPeople
Dim intProductSN
Dim strkey



	'-- UTF-8 偵錯時,要先定義網頁顯示編碼,才可正常顯示不會有亂碼
	Response.Write "<html><head><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""></head>"
	'=====================================================
	
	
	'-- 取得前頁轉送相關參數------------------------------
	strScript=Request.ServerVariables("SCRIPT_NAME")
	strPageNo=Trim(Request("pageno"))
	
	'intPayType=Trim(Request("PayType"))	'付款方式

	strDate1=Trim(Request("Date1"))
	strDate2=Trim(Request("Date2"))
	strTime1=Trim(Request("Time1"))
	strTime2=Trim(Request("Time2"))
	intPeople=Trim(Request("People"))
	intProductSN=Trim(Request("ProductSN"))
	
	
	strKey=Trim(Request("strKey"))
	
	'-- 預設日期 今天,目前只可查單天
	If strDate1="" Then strDate1=Date()
	If strDate2="" Then strDate2=Date()
	
	''-- 預設時間 早上8 下午16:00
	'If strTime1="" Then strTime1="01"
	'If strTime2="" Then strTime2="24"
	
	''-- 這兩個值有加工過,是要做查詢用
	'If strTime1 <> "" Then strTime1Temp=strTime1 & "-00-00"
	'If strTime1 <> "" Then strTime2Temp=strTime2 & "-00-00"
	'================================
	
	'-- 如果第一次進來或不是數字 就帶Session("ID")
	If Not IsNumeric(intPeople) then intPeople=Session("ID") Else intPeople=CLNG(intPeople)
	'================================
	
	If Not IsNumeric(intProductSN) then intProductSN=-1 Else intProductSN=CLNG(intProductSN)
	
	
	strQS="Date1="&strDate1&"&Date2="&strDate2&"&Time1="&strTime1&"&Time2="&strTime2&"&Time1Temp="&strTime1Temp&"&Time2Temp="&strTime2Temp&"&People="&intPeople&"&ProductSN="&intProductSN&"&strkey="&strKey
	'RESPONSE.WRITE strQS & "<HR>"
	'RESPONSE.END
	'======================================================

	
	'-- 列印頁數預設為1,本參數如果沒有值,Sheet2會無法清空--
	intLoopPageCount=1
	'======================================================
	


	'-- 取得範例檔,轉換檔名,另存新檔-----------------------------
    strSFileName = "點燈標籤.xls"
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
	strCopyEndRow=33	
	'-- 總列數
	intPageRow=33
	'===========================================================
	
	
	
	'-- 第1組資料錄----------------------------------------
	'-- 加入查詢條件---------------------------------------
	strWhere=""
	strWhere=MakeWhere(strWhere,"AND","A.產品編號","=",intProductSN,-1,"N","","")
	strWhere=MakeWhere(strWhere,"AND","A.產品編號","IN","('2','21','3','31','4','41','5','51','12','121','13','131','16','161','17','171')","","N","","")
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
	StrSql=StrSql & " ISNULL(A.點燈位置,'無') AS 點燈位置,"
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

	'########################################
	'-- 取得資料錄1--------------------------
	Set oRs=ExecSQL_RTN_RST(StrSQL,3,0,1)
	intRecordCount=oRs.RecordCount
	'RESPONSE.WRITE intRecordCount& "<HR>" 
	'========================================
	
	
	

	'------------------------------------------------------------
	Dim intLabelColumns			'每列標籤個數(也是分頁的筆數)
	Dim intLabelRows			'每頁標籤列數	
	Dim intSplitPageRecords		'每頁標籤總數
	
	Dim intLabelDataRows		'每個標籤資料所佔的ROW數 (橫向標籤為ROW)
	Dim intLabelJumpRows		'每個標籤資料跳欄數
	Dim intLabelJumpRowsTotal	'每個標籤資料跳欄數加總
	
	intLabelColumns=6			'每列標籤數
	intLabelRows=16				'每頁標籤列數
	
	intLabelDataRows=2			'每個標籤資料所佔的ROW數 (橫向標籤為ROW)
	intLabelJumpRows=1			'每個標籤資料跳欄數(此值通常為 intLabelDataRows-1)
	intLabelJumpRowsTotal=0		'每個標籤資料跳欄數加總
	
	'-- 每列標籤數(也是分頁的筆數)
	intSplitPageRecords = intLabelColumns * intLabelRows

	'-- 呼叫函數 取得分頁後總頁數----------------
	intLoopPageCount=GetPageCounts(intRecordCount,intSplitPageRecords)
	RESPONSE.WRITE "資料總筆數:" & intRecordCount & "，　每頁筆數:" & intSplitPageRecords & "，　總共頁數:" & intLoopPageCount & "<HR>" 
	'============================================
	

	'-- 跑回圈分頁---------------------------------------
	'-- 標籤列印由左自右,由上而下
	For intLoopPageNumber = 1 To intLoopPageCount
		
		'-- 重新定義陣列大小(不可放到分頁回圈外,因為資料會殘留)
		ReDim aryFieldsData(intLabelRows * intLabelDataRows - 1,intLabelColumns-1)
			
		'-- 偵錯用
		RESPONSE.WRITE "目前頁碼:" & intLoopPageNumber & ",　總共頁數:" & intLoopPageCount & "<HR>" 
		
		'-- 每個標籤資料跳欄數加總
		intLabelJumpRowsTotal=0
		
		'-- 依序將每頁標籤列數資料跑回圈
		For N= 1 TO intLabelRows

			'-- 依序將每一列標籤個數資料跑回圈
			For NN= 1 TO intLabelColumns
				
				'-- 如果資料錄是空的就離開
				If oRs.Eof Then Exit For
				
				'-- 偵錯用
				Response.Write oRs("申請人姓名") & "," & oRs("點燈位置") & "&nbsp;&nbsp;&nbsp;&nbsp;"
				
				aryFieldsData(N-1+intLabelJumpRowsTotal,NN-1)=oRs("申請人姓名")
				aryFieldsData(N  +intLabelJumpRowsTotal,NN-1)=oRs("點燈位置")		
				
				oRs.MoveNext
			Next
			
			'-- 每個標籤資料跳欄數加總 
			intLabelJumpRowsTotal=intLabelJumpRowsTotal+intLabelJumpRows
			'-- 偵錯用
			RESPONSE.WRITE "___intRowStep=" & intLabelJumpRowsTotal & "__" & N & "<HR>" 
			
			'-- 如果資料錄是空的就離開
			If oRs.Eof Then Exit For

		Next

		
		
		'-- 呼叫 DLL 副程式,把資料指定給 EXCEL
		Call objXLS.PutArrayDataToEXCEL_RC(aryFieldsData,2,2,intPageNumber,1)						
	
		
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





'###################################################################################################
%>