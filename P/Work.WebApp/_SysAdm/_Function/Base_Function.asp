<%

Session.timeout			= 120								'Session 淤期時間
Server.ScriptTimeout	= 90								'程式執行最長時間,單位為秒 預設值為90秒
Session.codepage		= 65001                        		'網頁國別  unicode:65001
															'codepage=65001 unicode UFT-8
															'codepage=950 繁體中文BIG5
															'codepage=936 簡體中文GBK
															'codepage=437 美國/加拿大英語
															'codepage=932 日文
															'codepage=949 韓文
															'codepage=866 俄文




'常數定義
Const ReadOnlyStr = " ReadOnly=""ReadOnly"""
Const CheckedStr = " CHECKED=""CHECKED"""
Const DisabledStr = " disabled "


'-- 基本函式-----------------------------------------------
Dim strTitleName
' strTitleName="21世紀"
' session("UID")="1"
' session("ID")="1000001"

'=======================
'設定網頁防止存取功能
'=======================
Function SetBodyAntiCopy(i)
Dim strTmp
	'-- i=0 防拷功能取消
	'i = 0
	If i = 0 Then 
		strTmp=""
	ElseIf i = 1 Then
		strTmp=" onselectstart=""return false"" oncontextmenu=""return false"" "
	End If
	SetBodyAntiCopy=strTmp	
	
	'-- 說明
	'onDRAGSTART="window.event.returnValue=false" 	//鎖住滑鼠拖曳功能
	'onCONTEXTMENU="window.event.returnValue=false" //鎖住右鍵選單功能
	'onSelectStart="event.returnValue=false" 		//鎖住滑鼠選取功能
	
End Function

'=======================
'設定網頁防止列印功能，使其輸出資料為空白
'=======================
Function SetBodyPrintBlank(i)
Dim strTmp
	'-- i=0 防列印功能取消
	'i = 0
	If i = 0 Then 
		strTmp=""
	ElseIf i = 1 Then
		strTmp="<style type=""text/css"">@Media Print{Body{display:none;}}</style>"
	End If
	SetBodyPrintBlank=strTmp
End Function

'=======================
'設定網頁另存新檔功能
'=======================
Function SetBodyAntiSaveAs(i)
Dim strTmp
	'-- i=0 網頁另存新檔功能取消
	'i = 0
	If i = 0 Then 
		strTmp=""
	ElseIf i = 1 Then
		strTmp="<noscript><iframe src=*.html></iframe></noscript>"
	End If
	SetBodyAntiSaveAs=strTmp
End Function




'=======================
'取得URL 中間路徑: _admin/member
'i:第幾層
'=======================
Function getScriptPath(i)
	Dim strPath
	Dim intLstSlash
	Dim intFstSlash
	Dim intI
	strPath=Request.ServerVariables("SCRIPT_NAME")
	intLstSlash=InStrRev(strPath,"/")
	If IntLstSlash>1 and i>=1 Then
		strPath=Left(strPath,intLstSlash-1)
		intFstSlash=intLstSlash
		for intI=1 to i
			if intFstSlash>1 Then
				intFstSlash=InStrRev(strPath,"/",intFstSlash-1)
			Else
				Exit For
			End if
		next
		strPath=right(strPath,len(strPath)-intFstSlash)
	End If
	getScriptPath=strPath
End Function


'-- 組合錯誤的訊息-----------------------------------------
Function MakeErrMsg(strErrMsg,strVarValue,strValueType,strComparision1,strVaule1,strComparision2,strVaule2,strVarCName,strLead)
Dim strTmp
Dim CheckResult1,CheckResult2,CheckResult3
	strTmp=""
	CheckResult1=0
	CheckResult2=0
	CheckResult3=0

	'-- 依型態來組合---------
	Select Case strValueType
	Case "N"
		If Trim(strVarValue) = "" Then
			strTmp = "「" & strVarCName & "」欄位,不可空白" & strLead & " !!"
		Else
			strTmp = "「" & strVarCName & "」欄位," & strLead & " !!"
		End If

		If IsNumeric(strVarValue) Then
			strVarValue=CDbl(strVarValue)

			If strComparision1 <> "" And strVaule1 <> "" Then
				strVaule1=CDbl(strVaule1)
				CheckResult1 = 1
				Select Case strComparision1
				Case "="
					If strVarValue = strVaule1 Then CheckResult1 = 0
				Case ">"
					If strVarValue > strVaule1 Then CheckResult1 = 0
				Case "<"
					If strVarValue < strVaule1 Then CheckResult1 = 0
				Case "<>"
					If strVarValue <> strVaule1 Then CheckResult1 = 0
				Case ">=" ,"=>"
					If strVarValue >= strVaule1 Then CheckResult1 = 0
				Case "<=" ,"=<"
					If strVarValue <= strVaule1 Then CheckResult1 = 0
				End Select
			End If


			If strComparision2 <> "" And strVaule2 <> "" Then
				strVaule2=CDbl(strVaule2)
				CheckResult2 = 1
				Select Case strComparision2
				Case "="
					If strVarValue = strVaule2 Then CheckResult2 = 0
				Case ">"
					If strVarValue > strVaule2 Then CheckResult2 = 0
				Case "<"
					If strVarValue < strVaule2 Then CheckResult2 = 0
				Case "<>"
					If strVarValue <> strVaule2 Then CheckResult2 = 0
				Case ">=" ,"=>"
					If strVarValue >= strVaule2 Then CheckResult2 = 0
				Case "<=" ,"=<"
					If strVarValue <= strVaule2 Then CheckResult2 = 0


				End Select
			End If
		Else
			CheckResult3=1
		End If

	Case "S"
		strTmp = "「" & strVarCName & "」欄位,不可空白 !!"
		If Len(strVarValue) > 0 Then
			CheckResult1 = 0
		Else
			CheckResult3 = 1
		End If

	Case "D"
		strTmp = "「" & strVarCName & "」欄位,請輸入日期 !!"
		If IsDate(strVarValue) Then
			CheckResult1 = 0
		Else
			CheckResult3 = 1
		End If

	End Select'==============
'	RESPONSE.WRITE CheckResult1 & "<BR>"
'	RESPONSE.WRITE CheckResult2 & "<BR>"
'	RESPONSE.WRITE CheckResult3 & "<BR>"
'	RESPONSE.WRITE strVarValue & "<BR>"
'
'	RESPONSE.WRITE strComparision2 & "<BR>"
'	RESPONSE.WRITE strVaule2 & "<BR>"


	If (CheckResult1+CheckResult2+CheckResult3) = 0 Then strTmp=""


	MakeErrMsg=strErrMsg &  strTmp
End Function'==============================================


'-- 組合錯誤的訊息-----------------------------------------
'-- intDebugType 偵錯的種類,目前只有一種
'-- intStatus 偵錯的方式 0:不偵錯,1:偵錯後繼續執行,2:偵錯後停止
Sub Debug(intDebugType,intStatus)
	If intStatus = 0 Then Exit Sub

	Select Case intDebugType
	Case 1
		Dim Item
		For Each Item in Request.Form
			RESPONSE.WRITE Item  & "--" & Request.Form(Item) & "<BR>"
		Next'==========================
	Case 2


	Case 3


	End Select

	If intStatus = 2 Then RESPONSE.END


End Sub'==============================================

'取小數點後兩位 Add By geeter 
Function FormatNum(Num)
	Num=FormatNumber(Num,2,,,0)
	FormatNum=Num
End Function

'Jerry Add Function
'======================================================================================================================
' Debug Depend on "On Error Rsume Next"
Function NullValue(src,value)
	If IsNull(src) Then
		NullValue = value
	Else
		NullValue = src
	End If
End Function

Sub AjaxStopCheck(message)
	
 		Set ReturnSYSJson = MakeJson() 
		ReturnSYSJson("result") = false
		ReturnSYSJson("message") = message
		ReturnSYSJson.Flush
		Response.End 
End Sub


Sub AspLogicErrCheck(message)

	Response.Write  message
	Response.End

End Sub

Sub AspLogicErrCheckTran(message ,cnn)

	cnn.RollbackTrans
	cnn.Close
	cnn = Nothing	
	Response.Write  message
	Response.End

End Sub

Sub AspErrCheck(Label)
	Dim message 
	If Err.Number <> 0 then   	
		message = Label & "<ERR:NO=>" & Err.Number & ":Desc:" & Err.Description & ">"
		Response.Write  message
		Response.End
	End If
End Sub

Sub AspErrCheckTran(Label,cnn)
	Dim message 
	If Err.Number <> 0 then 
		cnn.RollbackTrans
		cnn.Close
		Set cnn = Nothing	
		message = Label & "<ERR:NO=>" & Err.Number & ":Desc:" & Err.Description & ">"
		Response.Write  message
		Response.End
	End If
End Sub


Sub ErrCheck(Label)
	If Err.Number <> 0 then
 		Set ReturnSYSJson = MakeJson() 
		ReturnSYSJson("result") = false
		ReturnSYSJson("message") = "[" & Label & "]ERR NO[" & Err.Number & "]Desc:[" & Err.Description & "]" 
		ReturnSYSJson.Flush
		Response.End 
	End If
End Sub

Sub ErrCheckTran(Label,cnn)
	If Err.Number <> 0 then
		cnn.RollbackTrans
		cnn.Close
		Set cnn = Nothing		
 		Set ReturnSYSJson = MakeJson() 
		ReturnSYSJson("result") = false
		ReturnSYSJson("message") = "[" & Label & "]ERR NO[" & Err.Number & "]Desc:[" & Err.Description & "]" 
		ReturnSYSJson.Flush
		Response.End 
	End If
End Sub

Sub LogicErrCheck(message)
	Set ReturnSYSJson = MakeJson() 
	ReturnSYSJson("result") = false
	ReturnSYSJson("message") = message
	ReturnSYSJson.Flush	
	Response.End
End Sub

Sub LogicErrCheckTran(message,cnn)
	cnn.RollbackTrans
	cnn.Close
	Set cnn = Nothing

	Set ReturnSYSJson = MakeJson() 
	ReturnSYSJson("result") = false
	ReturnSYSJson("message") = message
	ReturnSYSJson.Flush	
	Response.End
End Sub

'Ajax Return Master Page Some Message Object
Function MakeJson()
		Set MakeJson = jsObject()
End Function 

Function Pad0Left(rsIn, rlCount)
	Pad0Left = PadLeft(rsIn, rlCount, "0")
End Function

Function PadLeft(rsIn, rlCount, rvntChar)
	'Dim s: s = String(rlCount, rvntChar)
	If Len(rsIn) >= rlCount Then
		PadLeft = rsIn
	Else
		PadLeft = String(rlCount - Len(rsIn), rvntChar) & rsIn
	End If
End Function


Sub DicKeyValue(dic,Key,Value)
	If dic.Exists(Key) Then
		If IsObject(dic.Item(Key)) Then
			Set dic.Item(Key) = Value
		Else
			dic.Item(Key) = Value
		End If 	
	Else
		dic.Add key,Value
	End If
End Sub


Function DicGetValue(dic,Key)
	If dic.Exists(Key) Then
		If IsObject(dic.Item(Key)) Then
			Set DicGetValue = dic.Item(Key)
		Else
			DicGetValue = dic.Item(Key)
		End If
	Else
		DicGetValue = ""
	End If
End Function


Function DicGetNumValue(dic,Key)
	Dim GetNum
	If dic.Exists(Key) Then
		GetNum = Trim(dic.Item(Key))
		If GetNum = "" Then 
			DicGetNumValue = 0
		Else
			DicGetNumValue = dic.Item(Key)
		End If

	Else
		DicGetNumValue = 0
	End If
End Function


'Handle Where Condition
Class WhereSet

	Private dic

	Private Sub Class_Initialize()	'初始化
		Set dic	= Server.CreateObject("Scripting.Dictionary")
	End Sub

	Private Sub Class_Terminate()	'結束程序
	End Sub

	Public Property Get ID
		ID = vID
	End  Property

	Public Property Let ID(Value)
        	vID = Value
  	End Property

	Public Function Add(FieldName,EquelStyle,Value,dot)		
		'EquelStyle = > < Between like
		Dim EquelTpl
		If EquelStyle="Like" Then
			EquelTpl = FieldName & " Like " &  dot & "%" & Value & "%" & dot
		ElseIf EquelStyle="LikeLeft" Then
			EquelTpl = FieldName & " Like " &  dot & "" & Value & "%" & dot
		ElseIf EquelStyle="LikeRight" Then
			EquelTpl = FieldName & " Like " &  dot & "%" & Value & "" & dot			
		Else
			EquelTpl = FieldName & EquelStyle & dot & Value & dot
		End If

		
		dic.Add FieldName,EquelTpl
	End Function
	
	Public Function ToWhereString()		
		Dim s,r
		s = dic.Items
		r = Join(s," and ")
		ToWhereString = r		
	End Function
	
	Public Function ToWhereByDEF()		
		Dim s,r
		s = dic.Items
		r = Join(s,"  ")
		ToWhereString = r		
	End Function
	
End Class
%>