<SCRIPT RUNAT=Server LANGUAGE="VBScript">

'----------------------------------------------------------
'-- 設定資料庫IP位址
Function SQL_ServerName
	SQL_ServerName="Localhost"
End Function
'==========================================================
'----------------------------------------------------------
'-- 設定資料庫名稱
Function SQL_DBName
	SQL_DBName="RenHai2012"
End Function
'==========================================================
'----------------------------------------------------------
'-- 設定資料庫登入帳號
Function SQL_UserId
	SQL_UserId="sa"
End Function
'==========================================================
'----------------------------------------------------------
'-- 設定資料庫登入密碼
Function SQL_Pwd
	SQL_Pwd="jcin@4257386~"
End Function
'==========================================================
'----------------------------------------------------------
'-- 取得連線字串
Function getConnStr(i)
Dim strConn,strConnShape
	'-- 連線字串0
	strConn="Provider=SQLOLEDB;Server=" & SQL_ServerName & ";UID=" & SQL_UserId & ";PWD=" & SQL_Pwd & ";DataBase=" &  SQL_DBName
	'
	'strConn="Provider=SQLNCLI10;Server=localhost;Database=RenHai;Uid=sa; Pwd=earn1000;"
	
	'-- 連線字串1
	strConnShape="Provider=MSDataShape;Data " & strConn

	If i=0 Then
		getConnStr=strConn
	Else
		getConnStr=strConnShape
	End If
End Function
'==========================================================

Function GetTranConnection()
	Dim oConn
	Set oConn = Server.CreateObject("ADODB.Connection")
	oConn.CursorLocation=3
	oConn.Open GetConnStr(0)
	oConn.BeginTrans
	Set GetTranConnection = oConn
End Function

Function GetTranRS(cnn,sql)
	Dim rs
	Set rs = Server.CreateObject("ADODB.Recordset")

	rs.CursorType = 3
	rs.LockType = 3
	rs.Open sql,cnn
	Set GetTranRS = rs
End Function

Function ExecTranSQL(conn,sql)
Dim oCmd
	Set oCmd=Server.CreateObject("ADODB.Command")
	oCmd.CommandType=&H0001
	oCmd.CommandText=sql
	oCmd.ActiveConnection=conn
	oCmd.Execute ,,&H00000080
	'adExecuteNoRecords = &H00000080
	'-- 釋放物件
	Set oCmd.ActiveConnection=Nothing
	Set oCmd=Nothing
End Function

'----------------------------------------------------------
'-- 使用 RECORDSET.OPEN 取得資料錄
Function GetRst(strSQL,intCursorLocation,intCursorType,intLockType)
Dim oConn
Dim oRST

	'-- CursorLocationEnum Values
	Const adUseServer = 2
	Const adUseClient = 3

	'-- CursorTypeEnum Values
	Const adOpenForwardOnly = 0
	Const adOpenKeyset = 1
	Const adOpenDynamic = 2
	Const adOpenStatic = 3

	'-- LockTypeEnum Values
	Const adLockReadOnly = 1
	Const adLockPessimistic = 2
	Const adLockOptimistic = 3
	Const adLockBatchOptimistic = 4

	Set oConn = Server.CreateObject("ADODB.Connection")
	oConn.CursorLocation=intCursorLocation
	oConn.Open GetConnStr(0)

	Set oRST=Server.CreateObject("ADODB.Recordset")

	oRST.CursorType = intCursorType
	oRST.LockType = intLockType
	oRST.Open strSQL,oConn
	Set GetRst=oRST

	'-- 釋放物件
	Set oRST=Nothing
	Set oConn=Nothing
End Function '===============================

'''----------------------------------------------------------
'''-- 取得資料庫SchemaRecordSet
''Function getSchemaRs(QueryType,Criteria)
''	Dim oCnn
''	Dim oRs
''	Set oCnn=Server.CreateObject("ADODB.Connection")
''	oCnn.CursorLocation=3
''	oCnn.Open getConnStr(1)
''	Set oRS=oCnn.OpenSchema(QueryType,Criteria)
''	Set getSchemaRs=oRs
''End Function
'''==========================================================


'----------------------------------------------------------
'-- 執行DataShape並傳回RecoredSet
'-- strSQL:SQL Statement
'-- intCursorLocation:Connection 物件之CursorLocation
'-- intCursorType:傳回RecoredSet的CursorType屬性
'-- intLockType:傳回RecordSet的LockType屬性   原名：ExecShapeRs
Function ExecShape_RTN_RST(strSQL,intCursorLocation,intCursorType,intLockType)
	Dim oConn
	Dim oRs
	Dim oCmd
	Set oConn=Server.CreateObject("ADODB.Connection")
	Set oRs=Server.CreateObject("ADODB.RecordSet")
	Set oCmd=Server.CreateObject("ADODB.Command")
	oCmd.CommandType=&H0001
	oCmd.CommandText=strSQL
	oConn.CursorLocation=intCursorLocation
	oConn.Open getConnStr(1)
	oCmd.ActiveConnection=oConn
	oRs.CursorType=intCursorType
	oRs.LockType=intLockType
	Set oRs=oCmd.Execute
	Set oConn=Nothing
	Set oCmd=Nothing
	Set ExecShape_RTN_RST=oRs
	Set oRs=Nothing
End Function
'==========================================================


'----------------------------------------------------------
'-- 執行SQL 字串,不回傳值
Function ExecSQL(strSQL)
Dim oCmd
	Set oCmd=Server.CreateObject("ADODB.Command")
	oCmd.CommandType=&H0001
	oCmd.CommandText=strSQL
	oCmd.ActiveConnection=getConnStr(0)
	oCmd.Execute ,,&H00000080
	'adExecuteNoRecords = &H00000080
	'-- 釋放物件
	Set oCmd.ActiveConnection=Nothing
	Set oCmd=Nothing
End Function
'==========================================================
'----------------------------------------------------------
'-- 執行SQL字串,並傳回RecordSet 原名 ExecSQLRs
Function ExecSQL_RTN_RST(strSQL,intCursorLocation,intCursorType,intLockType)
Dim oCmd
Dim oCnn
Dim oRs
	Set oCnn=Server.CreateObject("ADODB.Connection")
	Set oCmd=Server.CreateObject("ADODB.Command")
	Set oRs=Server.CreateObject("ADODB.RecordSet")
	oCmd.CommandType=&H0001
	oCmd.CommandText=strSQL
	oCnn.CursorLocation=intCursorLocation
	oCnn.Open getConnStr(0)
	oCmd.ActiveConnection=oCnn
	oRs.CursorType=intCursorType
	oRs.LockType=intLockType
	Set oRS=oCmd.Execute
	'-- 回傳值
	Set ExecSQL_RTN_RST=oRs
	'-- 釋放物件
	Set oCmd.ActiveConnection=Nothing
	Set oCmd=Nothing
	Set oCnn=Nothing
End Function

'==========================================================
'----------------------------------------------------------
'-- 執行Stored Procedure 並傳回Array陣列 (Stored Procedure Outp參數)
'-- strSPName:Stored Procedure 名稱
'-- aryInput:Array 型態;傳入Stored Procedure Input 參數 原名：ExecSP
Function ExecSP_RTN_ARY(strSPName,ByRef aryInput)
Dim oCmd
Dim i,j
Dim intParamCount
	i=0
	j=0
	Set oCmd=Server.CreateObject("ADODB.Command")
	oCmd.CommandType=&H0004
	oCmd.CommandText=strSPName
	oCmd.ActiveConnection=getConnStr(0)

	oCmd.Parameters.Refresh()
	intParamCount=oCmd.Parameters.Count
	For i=0 to intParamCount-1
		If oCmd.Parameters(i).Direction=&H0001 Then
			oCmd.Parameters(i).Value=aryInput(j)
			j=j+1
		End If
	Next

	oCmd.Execute ,,128

	Dim aryOut()
	Dim p
	ReDim aryOut(0)
	aryOut(0)=oCmd.Parameters(0)
	If intParamCount>j+1 Then
		ReDim Preserve aryOut(intParamCount-1-j)
		j=1
		For i=1 to intParamCount-1
			If oCmd.Parameters(i).Direction=&H0003 Then
			'response.write oCmd.Parameters(i).Name & "=" & oCmd.Parameters(i).Type & "<br>"
				Select Case oCmd.Parameters(i).Type
				Case 129,200,201,130,202,203
					p=oCmd.Parameters(i).Value
					If Not IsNull(p) Then
						aryOut(j)=Trim(p)
					Else
						aryOut(j)=""
					End if
				Case Else
					aryOut(j)=oCmd.Parameters(i).Value
				End Select
				j=j+1
			End If
		Next
	End If
	'-- 回傳值
	ExecSP_RTN_ARY=aryOut
	'-- 釋放物件
	Set oCmd.ActiveConnection=Nothing
	Set oCmd=Nothing
End Function
'==========================================================
'----------------------------------------------------------
'-- 執行Stored Procedure 並傳回RecordSet   原名：ExecSPRs
'-- 若aryInput為Null則無參數傳入
Function ExecSP_RTN_RST(strSPName,intCursorLocation,intCursorType,intLockType,ByRef aryInput)
Dim oConn
Dim oRs
Dim oCmd
Dim i,j
Dim intParamCount
	i=0
	j=0
	Set oConn=Server.CreateObject("ADODB.Connection")
	Set oRs=Server.CreateObject("ADODB.RecordSet")
	Set oCmd=Server.CreateObject("ADODB.Command")
	oCmd.CommandType=&H0004
	oCmd.CommandText=strSPName
	oConn.CursorLocation=intCursorLocation
	oConn.Open getConnStr(0)
	oCmd.ActiveConnection=oConn
	If Not IsNull(aryInput) Then
		oCmd.Parameters.Refresh()
		intParamCount=oCmd.Parameters.Count
		For i=0 to intParamCount-1
			If oCmd.Parameters(i).Direction=&H0001 Then
				oCmd.Parameters(i).Value=aryInput(j)
				j=j+1
			End If
		Next
	End If
	oRs.CursorType=intCursorType
	oRs.LockType=intLockType
	Set oRs=oCmd.Execute
	'-- 回傳值
	Set ExecSP_RTN_RST=oRs
	'-- 釋放物件
	Set oConn=Nothing
	Set oCmd=Nothing
	Set oRs=Nothing
End Function
'==========================================================

'----------------------------------------------------------
'-- 取得最大流水號
Function GstMaxID(strTBName)
	Dim oRST
	Dim lngMaxID
	Set oRST=GetRst("Select Max(號數)AS 最大號數 FROM 流水號資料表 Where 資料表名稱='" & strTBName & "'")
	
lngMaxID=oRST("最大號數")
	ExecSQL "Update 流水號資料表 Set 號數=號數+1 Where 資料表名稱='" & strTBName & "'"

    GstMaxID=lngMaxID

	'-- 釋放物件
	Set oRST=Nothing
End Function
'==========================================================

'----------------------------------------------------------
'-- 組合 SQL WHERE 子句1
Function MakeWhere(strWhere,strConnectFlag,strFieldName,strComparision,NewVaule,DefaultValue,strValueType,strLeftFlag,strRightFlag)
Dim strTmp
	strTmp=""
	strConnectFlag=" " & strConnectFlag & " "
	strFieldName=" " & strFieldName & " "
	strComparision=" " & strComparision & " "

	'-- 依型態來組合---------
	Select Case strValueType
	Case "N"
		strTmp=strTmp & strFieldName & strComparision & NewVaule
	Case "S"
		strTmp=strTmp & strFieldName & strComparision & "'" & NewVaule & "'"
	CASE "D"
		strTmp=strTmp & strFieldName & strComparision & "'" & NewVaule & "'"
	End Select'==============

	'-- 如果變數值與預設值一樣就清空(不做查詢)
	IF NewVaule=DefaultValue Then strTmp = ""

	'-- 加入串接的值( AND,OR)
	If strWhere="" Or strTmp="" Then
		IF strTmp="" THEN
			strTmp= strWhere &  strTmp
		ELSEIF strWhere="" THEN
			strTmp= strWhere & strLeftFlag & strTmp & strRightFlag
		END IF
	Else
		strTmp=strWhere & strConnectFlag & strLeftFlag & strTmp & strRightFlag
	End IF'==================
	MakeWhere=strTmp
End Function
'==========================================================

'----------------------------------------------------------
'-- 組合 SQL WHERE 子句2
Function MakeWhereKW(strWhere,strConnectFlag,strFieldName,strComparision,NewVaule,DefaultValue,strValueType,strLeftFlag,strRightFlag)
Dim strTmp
Dim i
Dim TmpNewValue
	strTmp=""
	strConnectFlag=" " & strConnectFlag & " "

	'-- 判斷%LIKE%,並與值合併
	SELECT CASE UCASE(strComparision)
		CASE "%LIKE"
			TmpNewValue= " LIKE '%" & NewVaule & "'"
		CASE "LIKE%"
			TmpNewValue= " LIKE '"  & NewVaule & "%'"
		CASE "%LIKE%"
			TmpNewValue= " LIKE '%" & NewVaule & "%'"
		CASE "LIKE"
			TmpNewValue= " LIKE '" & NewVaule & "'"
	END SELECT


	'-- 如果有值,就把字串兜起來
	If IsArray(strFieldName) Then
		For i = 0 To UBound(strFieldName)
			strTmp=strTmp & strFieldName(i) & TmpNewValue
			IF i < UBound(strFieldName) Then strTmp = strTmp & " OR "
		Next
		strTmp = "(" & strTmp & " )"

	End IF'==================

	'-- 如果變數值與預設值一樣就清空(不做查詢)
	IF NewVaule=DefaultValue  Then strTmp = ""

	'-- 加入串接的值( AND,OR)
	If strWhere="" Or strTmp="" Then
		IF strTmp="" THEN
			strTmp= strWhere &  strTmp
		ELSEIF strWhere="" THEN
			strTmp= strWhere & strLeftFlag & strTmp & strRightFlag
		END IF
	Else
		strTmp=strWhere & strConnectFlag & strLeftFlag & strTmp & strRightFlag
	End IF'==================
	MakeWhereKW=strTmp
End Function
'==========================================================

'----------------------------------------------------------
'-- 組合 SQL WHERE 子句3
Function MakeWhereBeTween(strWhere,strConnectFlag,strFieldName1,strFieldName2,NewVaule1,NewVaule2,intFieldFlag)
Dim strTmp
Dim Ydate1,Mdate1,Ddate1,Ydate2,Mdate2,Ddate2
	strTmp=""
	strConnectFlag=" " & strConnectFlag & " "
	
	'-- Add By Taka 20110107 新增日期判別 intFieldFlag = 0 時========
	'-- Jack Fixed 
	If intFieldFlag = 0 Then
		'-- 依造輸入值是否為空來做判斷
		IF NewVaule1 <> "" AND NewVaule2 <> "" Then 
			strTmp="(" & strFieldName1 & " BeTween '"&NewVaule1&"' AND '"&NewVaule2&"') "
		
		ELSEIF NewVaule1 <> ""  AND NewVaule2 = "" Then 
			strTmp="(" & strFieldName1 & " >= '"&NewVaule1&"' ) "
				
		ELSEIF NewVaule1 = ""  AND NewVaule2 <> "" Then 
			strTmp="(" & strFieldName1 & " <= '"&NewVaule2&"' ) "
	
		ELSE
			strTmp = ""
		End If
	'=========================Add End=================================
	
	ElseIf intFieldFlag = 1 Then
		'-- 依造輸入值是否為空來做判斷
		IF NewVaule1 <> ""  AND NewVaule2 <> "" Then 
			strTmp="(" & strFieldName1 & " BeTween " & NewVaule1 & " AND " & NewVaule2 & ") "
		
		ELSEIF NewVaule1 <> ""  AND NewVaule2 = "" Then 
			strTmp="(" & strFieldName1 & " >= " & NewVaule1 & " ) "
				
		ELSEIF NewVaule1 = ""  AND NewVaule2 <> "" Then 
			strTmp="(" & strFieldName1 & " <= " & NewVaule2 & " ) "
	
		ELSE
			strTmp = ""
		End If
	Elseif intFieldFlag = 2 Then
		
		'-- 依造輸入值是否為空來做判斷
		IF NewVaule1 <> ""  AND NewVaule2 <> "" Then 
			strTmp="((" & strFieldName1 & " BeTween " & NewVaule1 & " AND " & NewVaule2 & ") OR (" & strFieldName2 & " BeTween " & NewVaule1 & " AND " & NewVaule2 & " )) "
		
		ELSEIF NewVaule1 <> ""  AND NewVaule2 = "" Then 
			strTmp="(" & strFieldName1 & " >= " & NewVaule1 & " OR " & strFieldName2 & " >= " & NewVaule1 & ") "
				
		ELSEIF NewVaule1 = ""  AND NewVaule2 <> "" Then 
			strTmp="(" & strFieldName1 & " <= " & NewVaule2 & " OR " & strFieldName2 & " <= " & NewVaule2 & ") "
	
		ELSE
			strTmp = ""
		End If		
		
	End IF

	'-- 加入串接的值( AND,OR)
	If strWhere="" Or strTmp="" Then
		IF strTmp="" THEN
			strTmp= strWhere &  strTmp
		ELSEIF strWhere="" THEN
			strTmp= strWhere  & strTmp 
		END IF
	Else
		strTmp=strWhere & strConnectFlag & strTmp 
	End IF'==================

	MakeWhereBeTween=strTmp
End Function
'==========================================================



'----------------------------------------------------------
'-- 判斷查詢條件是否要加入[WHERE]關鍵字
Function MakeWhereEnd(strWhere)
Dim strTmp
	strTmp=Trim(strWhere)
	If Len(strTmp) = 0 Then
		strTmp=""
	Else
		strTmp=" Where " & strTmp
	End If

	MakeWhereEnd=strTmp
End Function
'==========================================================

'----------------------------------------------------------
'-- 儲存時檢查資料是否為[""]或[NULL]
Function SaveDataCheck(Value,ReplaceValue)
    If Value = "" Or IsNULL(Value) Then
        SaveDataCheck = ReplaceValue
    Else
        SaveDataCheck = Value
    End If
End Function
'==========================================================

'----------------------------------------------------------
'-- 顯示時檢查資料是否為[""]或[NULL]
Function ShowDataCheck(Value,ReplaceValue)
    If Value="" Or IsNull(Value)  Then
        ShowDataCheck = ReplaceValue
    Else
        ShowDataCheck = Value
    End If
End Function
'==========================================================

'----------------------------------------------------------
'-- 顯示時檢查資料是否為[""]或[NULL]
Function ReplaceDataCheck(Value,QueryValue,ReplaceValue)
    If Value="" Or IsNull(Value)  Then
        ReplaceDataCheck = Value
    Else
        ReplaceDataCheck = Replace(Value,QueryValue,ReplaceValue)
    End If
End Function
'==========================================================



'----------------------------------------------------------
'-- Design By Jack Hsieh
Function SP_Spliter(SPNAmeStr,ByRef ArrayA,Flag)
Dim N
Dim Counts
Dim ValueStr

	RESPONSE.WRITE "<HR>"
	RESPONSE.WRITE "DECLARE @流水號 INT <BR>"
	RESPONSE.WRITE "EXEC " & SPNAmeStr  & " "
	Counts = UBound(ArrayA)
	For N = 0 TO Counts
		If  Len(Trim(ArrayA(N))) = 0 OR IsNull(ArrayA(N)) OR IsEmpty(ArrayA(N)) Then
			ValueStr="NULL"
		ElseIf IsDate(ArrayA(N)) Then
			ValueStr="'" & ArrayA(N) & "'"
		ElseIf IsNumeric(ArrayA(N)) Then
			ValueStr= ArrayA(N)
		Else
			ValueStr="'" & ArrayA(N) & "'"
		End If
		If N <> Counts Then
			RESPONSE.WRITE ValueStr & ","
		Else
			RESPONSE.WRITE ValueStr
		End If
	Next
	If Flag = 1 Then  RESPONSE.WRITE ",@流水號"
	SP_Spliter=0
End Function
'==========================================================
'----------------------------------------------------------
'-- 關閉物件,清空變數
Function ReleaseObj(ByRef ArrObj)
Dim N
Dim Counts
	Counts = UBound(ArrObj)
	For N = 0 TO Counts
		ArrObj(N).Close
		Set ArrObj(N) = Nothing
	Next
End Function
'==========================================================
Function GetSysID(TagName)

	Dim cnn,sql,rs
	Dim getSerialID
	
	Set cnn = GetTranConnection
	sql = "select 號數 From 流水號資料表 Where 資料表名稱='" & TagName & "'"
	Set rs	=	GetTranRS(cnn,sql)
	
	If rs.Eof Then
		sql = "Insert Into 流水號資料表(資料表名稱,號數) Values('" & TagName & "',10000000)"
	Else
		getSerialID = CLng(rs.Fields("號數").Value) + 1
		sql = "Update 流水號資料表 Set 號數=" & getSerialID & " Where 資料表名稱='" & TagName & "'"
	End If 
	ExecTranSQL cnn,sql
	
	rs.Close
	Set rs = Nothing
			
	cnn.CommitTrans
	cnn.Close
	Set cnn = Nothing
	
	GetSysID = getSerialID
	
End Function

</SCRIPT>            