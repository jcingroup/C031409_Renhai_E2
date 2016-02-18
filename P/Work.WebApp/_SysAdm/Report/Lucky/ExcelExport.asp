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
Dim aryFieldsPosition,aryFieldsPosition01,aryFieldsPosition02
Dim aryFieldsData,aryFieldsData01,aryFieldsData02
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
Dim intStepSN
Dim strkey

	'-- 取得前頁轉送相關參數------------------------------
	strScript=Request.ServerVariables("SCRIPT_NAME")
	strPageNo=Trim(Request("pageno"))
	
	strDate1=Trim(Request("Date1"))
	strDate2=Trim(Request("Date2"))
	intStepSN=Trim(Request("StepSN"))
	
	
	strKey=Trim(Request("strKey"))
	
	'-- 預設日期 今天,目前只可查單天
	'If strDate1="" Then strDate1=Year(Date()) & "/01/01"
	'If strDate2="" Then strDate2=Date()
	'================================
	
	If Not IsNumeric(intStepSN) then intStepSN=-1 Else intStepSN=CLNG(intStepSN)

	strQS="Date1="&strDate1&"&Date2="&strDate2&"&StepSN="&intStepSN&"&strkey="&strKey
	'RESPONSE.WRITE strQS & "<HR>"
	'RESPONSE.END
	'======================================================
	
	
	If intStepSN=-1 Then 
		Response.Write "請選擇 文疏梯次 !!"
		Response.End 
	End IF 	
	
	'-- 列印頁數預設為1,本參數如果沒有值,Sheet2會無法清空--
	intLoopPageCount=1
	'======================================================


	'-- 取得範例檔,轉換檔名,另存新檔-----------------------------
    strSFileName = "保運文疏名冊.xls"
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
	strCopyEndRow=13
	'-- 總列數
	intPageRow=13
	'===========================================================
	
	
	
	
	
	
	'-- 第1組資料錄----------------------------------------
	'-- 加入查詢條件---------------------------------------
	strWhere=""
	strWhere=MakeWhere(strWhere,"AND","A.序號","=",intStepSN,-1,"N","","")
	'strWhere=MakeWhere(strWhere,"AND","B.新增人員","=",intPeople,-1,"N","","")
	'strWhere=MakeWhere(strWhere,"AND","CONVERT(NVARCHAR(10),B.新增時間,111)",">=",strDate1,"","S","","")
	'strWhere=MakeWhere(strWhere,"AND","CONVERT(NVARCHAR(10),B.新增時間,111)","<=",strDate2,"","S","","")

	strWhere=MakeWhereEnd(strWhere)
	'RESPONSE.WRITE strWhere & "<HR>"
	'RESPONSE.END
	'======================================================

	'-- 組合查詢字串---------------------------------------
	StrSql= " Select DISTINCT "
	StrSql=StrSql & " A.序號, "
	StrSql=StrSql & " A.梯次, "
	StrSql=StrSql & " CONVERT(NVARCHAR(10),A.時間,111) AS 梯次時間, "
	StrSql=StrSql & " B.訂單編號, "
	StrSql=StrSql & " B.戶長姓名, "
	StrSql=StrSql & " B.戶長地址, "
	StrSQL=StrSQL & " (SELECT  歲次年 FROM 年度生肖表 WHERE 民國年 =A.農曆年) AS 歲次年 , "
	StrSQL=StrSQL & " A.農曆月 AS 歲次月, "
	StrSQL=StrSQL & " A.農曆日 AS 歲次日 "		
	
	StrSql=StrSql & " FROM 文疏梯次時間表 AS A "
	StrSql=StrSql & " Left Join ("
	
	StrSql=StrSql & " Select DISTINCT "
	StrSql=StrSql & " 	X1.訂單編號,X1.申請人姓名 AS 戶長姓名 ,X1.申請人地址 AS 戶長地址,"
	'StrSql=StrSql & "	X2.申請人姓名 , "
	StrSql=StrSql & "	X2.文疏梯次 "
	StrSql=StrSql & " 	FROM 訂單主檔 AS X1 "
	StrSql=StrSql & " 		Left Join 訂單明細檔 AS X2 ON X2.訂單編號=X1.訂單編號 "
	StrSql=StrSql & " 	WHERE X2.文疏梯次 > 0 and year(X1.新增時間)=year(getdate()) "
	
	StrSql=StrSql & " 	) AS B ON B.文疏梯次=A.序號 "
	StrSql=StrSql & StrWhere
	StrSql=StrSql & " ORDER BY A.序號 "
	
    'RESPONSE.WRITE StrSql  & "<HR>"
	'RESPONSE.END
	'======================================================
	
	'########################################
	'-- 取得資料錄1--------------------------
	Set oRs=ExecSQL_RTN_RST(StrSQL,3,0,1)
	intLoopPageCount=oRs.RecordCount
	'========================================
	
	While Not oRs.Eof
		'-- 將資料1依序塞入 Excel----------------
		intDataFlag=1
		intPageCount=1
		intPageNumber=1
		'-- 呼叫 DLL 副程式,設定資料位置對應
		Call SetFieldsPositionMatch(intDataFlag)

		'-- 將資料錄資料存入陣列-----------------
		Call SetRstToArray(oRs,aryFieldsData,0,intDataFlag)
			
		'-- 將資料丟給EXCEL----------------------
		Call objXLS.PutArrayDataToEXCEL(aryFieldsPosition, aryFieldsData,intPageNumber,1)
		'========================================

		'##########################################################################################
		'-- 第2組資料錄-----------------------------
		'-- 加入查詢條件----------------------------
		strWhere2=""
		strWhere2=MakeWhere(strWhere2,"AND","A.訂單編號","=",oRs("訂單編號"),"","S","","")
		strWhere2=MakeWhere(strWhere2,"AND","A.文疏梯次",">",0,"","N","","")
		strWhere2=MakeWhereEnd(strWhere2)
		RESPONSE.WRITE strWhere2 & "<HR>"
		'RESPONSE.END
		'============================================	
		'-- 組合查詢字串-----------------------------
		StrSQL2=""
		StrSQL2= StrSQL2 & " SELECT Top 6 LEFT(A.申請人姓名,4) AS 申請人姓名, "
		StrSQL2= StrSQL2 & " A.申請人地址 ,"
		StrSQL2= StrSQL2 & " ISNULL(A.申請人生日,0) AS 申請人生日 ,"
		StrSQL2= StrSQL2 & " ISNULL(A.申請人年齡,0) AS 申請人年齡,"
		StrSQL2= StrSQL2 & " ISNULL(A.申請人時辰,'吉') AS 申請人時辰 ,"
		StrSQL2= StrSQL2 & "  CASE A.申請人性別 WHEN 1 THEN '建' ELSE '瑞' END AS 申請人性別 ,"
		StrSQL2= StrSQL2 & " ISNULL((SELECT Top 1 星運 From 星運表 WHERE 生肖 = A.申請人生肖 ),'') AS 星運 "
		StrSQL2= StrSQL2 & " FROM  訂單明細檔 AS A  "
		StrSQL2=StrSQL2 & StrWhere2
		StrSQL2=StrSQL2 & " Order By 申請人生日 "
		'RESPONSE.WRITE StrSQL2 & "<HR>" 
		'RESPONSE.END
		'============================================
		'-- 取得資料錄2------------------------------
		Set oRs2=ExecSQL_RTN_RST(StrSQL2,3,0,1)
		intRecordCount=oRs2.RecordCount
		'============================================
		
		
		'-- 將資料2依序塞入 Excel--------------------
		If intRecordCount >=0 Then
			intDataFlag=2
			intPageCount=1
			intPageNumber=1
			'-- 1.呼叫 DLL 副程式,設定資料位置對應
			Call SetFieldsPositionMatch(intDataFlag)
			
			aryFieldsData01(0)=""
			aryFieldsData02(0)=""
			'-- 2.將資料錄資料存入陣列---------------
			For intRecordNumber = 1 To intRecordCount
				
				'-- 2-1.將資料錄資料存入陣列
				'Call SetRstToArray(oRs,aryFieldsData,intRecordNumber-1,intDataFlag)
				strTemp = oRs2("申請人姓名") & "生於" 
				strTemp = strTemp & GetBirthday_Split(oRs2("申請人生日"),"YY") & "年" 
				strTemp = strTemp &	GetBirthday_Split(oRs2("申請人生日"),"MM") & "月" 
				strTemp = strTemp & GetBirthday_Split(oRs2("申請人生日"),"DD") & "日" 
				strTemp = strTemp & oRs2("申請人時辰") & "時"
				strTemp = strTemp & oRs2("申請人性別") & "生" 
				
				strTemp = strTemp & "年庚" & Year(Date) - 1911 - GetBirthday_Split(oRs2("申請人生日"),"YY") + 1 & "歲" 
				strTemp = strTemp & oRs2("星運")
				aryFieldsData(intRecordNumber-1) = strTemp
				'RESPONSE.WRITE strTemp & "<HR>"
				
			
				'-- 2-2.將資料錄資料存入陣列
				aryFieldsData01(0) = aryFieldsData01(0) & oRs2("申請人姓名") & " " 
				
				'-- 2-3.將資料錄資料存入陣列
				aryFieldsData02(0) = aryFieldsData02(0) & oRs2("申請人姓名") & " "

				'RESPONSE.WRITE intRecordNumber-1 & "-" & aryFieldsPosition(intRecordNumber-1) & "__" &aryFieldsData(intRecordNumber-1) & "<HR>" 
				oRs2.MoveNext
			Next'====================================
			'RESPONSE.WRITE  "<HR>" 
			
			
			'-- 3-1.將資料丟給EXCEL----------------------
			Call objXLS.PutArrayDataToEXCEL(aryFieldsPosition, aryFieldsData,intPageNumber,0)
			
			'-- 3-2.將資料丟給EXCEL----------------------
			Call objXLS.PutArrayDataToEXCEL(aryFieldsPosition01, aryFieldsData01,intPageNumber,0)
			
			'-- 3-3.將資料丟給EXCEL----------------------
			Call objXLS.PutArrayDataToEXCEL(aryFieldsPosition02, aryFieldsData02,intPageNumber,0)
		End If'======================================
		'###########################################################################################

		intLoopPageNumber=intLoopPageNumber+1
		'-- 呼叫 DLL 副程式,EXCEL 分頁
		Call objXLS.SplitPage( strDSheetName, strSSheetName, strCopyStartRow, strCopyEndRow,intPageRow ,intLoopPageNumber,intLoopPageCount)

		oRs.MoveNext
	Wend


	'-- 關閉DLL物件,釋放DLL物件----------------------------
    objXLS.CloseAP
    Set objXLS = Nothing
    '======================================================

	'-- 關閉物件,釋放物件----------------------------------
	' ' ReleaseObj(Array(oRs,oRs0))
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


'-- 陣列與EXCEL CELL 對應----------------------------------
Sub SetFieldsPositionMatch(varDataFlag)
	Select Case varDataFlag
	Case 1
		'-- 重新宣告陣列
	   	ReDim aryFieldsPosition(5)
		aryFieldsPosition(0) = "Q3"
		aryFieldsPosition(1) = "P3"
		aryFieldsPosition(2) = "G5"
		aryFieldsPosition(3) = "B5"
		aryFieldsPosition(4) = "B8"
		aryFieldsPosition(5) = "B10"		
		ReDim aryFieldsData(5)

	Case 2
		'-- 重新宣告陣列
	   	ReDim aryFieldsPosition(5)
	    aryFieldsPosition(0) = "N4"
	    aryFieldsPosition(1) = "M4"
	    aryFieldsPosition(2) = "L4"
	    aryFieldsPosition(3) = "K4"
	    aryFieldsPosition(4) = "J4"
	    aryFieldsPosition(5) = "I4"		
		ReDim aryFieldsData(5)	

		
		
	   	ReDim aryFieldsPosition01(0)
	    aryFieldsPosition01(0) = "P3"
		ReDim aryFieldsData01(0)	
		
		

		ReDim aryFieldsPosition02(0)
	    aryFieldsPosition02(0) = "D6"
		ReDim aryFieldsData02(0)	
		


	End Select

End Sub'===================================================

'-- 指定資料錄資料給陣列-----------------------------------
Sub SetRstToArray(ByRef varORS,ByRef aryFieldsData, varIntRecordNumber,varDataFlag)
Dim tempValue1,tempValue2
	Select Case varDataFlag
	Case 1
		aryFieldsData(0) = varORS("戶長地址")
		aryFieldsData(1) = varORS("戶長姓名")
		aryFieldsData(2) = GetDate_Convert(varORS("歲次日"))
		aryFieldsData(3) = varORS("歲次年")
		aryFieldsData(4) = GetDate_Convert(varORS("歲次月"))
		aryFieldsData(5) = GetDate_Convert(varORS("歲次日"))

	' ' Case 2
		' ' aryFieldsData(varIntRecordNumber) = varORS("申請人姓名") & varORS("申請人地址")
		
	' ' Case 21
		' ' aryFieldsData(varIntRecordNumber) = varORS("申請人姓名") 
	' ' Case 22
		' ' aryFieldsData(varIntRecordNumber) = varORS("申請人姓名") 

	End Select

End Sub'===================================================

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

'-- 日期數字轉成日期國字-----------------------------------
Function GetDate_Convert(varDate)
Dim strD1,strD2
Dim AryD1,AryD2
Dim IntPos
Dim varTemp
	'-- 將傳入的數字日期長度為1的前面捕0
	varDate=String(2-Len(varDate),"0") & varDate

	strD1="01,02,03,04,05,06,07,08,09,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31"
	strD2="一,二,三,四,五,六,七,八,九,十,十一,十二,十三,十四,十五,十六,十七,十八,十九,二十,二十一,二十二,二十三,二十四,二十五,二十六,二十七,二十八,二十九,三十,三十一"
	AryD1=Split(strD1,",")
	AryD2=Split(strD2,",")

	'-- 取得 字串的位置
	'-- 1,4,7,10,13,16,19,22,25,28,   31,34,37,40,43,46,49,52,55,58,   61,64,67,70,73,76,79,82,85,88,91
	IntPos=INSTR(1,strD1,varDate)
	'-- 列外處裡
	IF IntPos >=1 AND IntPos <= 91 Then
		IntPos=(IntPos-1) / 3
		varTemp=AryD2(IntPos)
	ELSE
		varTemp=""
	End IF
	'-- 偵錯用
	'RESPONSE.WRITE varDate & "--" & IntPos & "--" & varTemp & "<HR>" 
	
	GetDate_Convert=varTemp
	

End Function'==============================================

%>