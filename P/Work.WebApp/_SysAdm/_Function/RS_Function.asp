<%
'**************************************
'取得某一筆資料下一筆資料的某一欄位值
'oRs			'資料錄
'sIndex			'用來尋找的欄位
'sValue			'尋找的欄位值
'dIndex			'欲取得的欄位Index
'======================================
Function getNextF(oRs,sIndex,sValue,dIndex)
Dim dValue
Dim strFilter
	strFilter=oRs.Fields(sIndex).Name & "=" & Cstr(sValue)

	If oRs.RecordCount>0 Then
		oRs.Find strFilter
		If Not oRs.Eof Then
			oRs.MoveNext
			If Not oRs.Eof then
				dValue=oRs(dIndex)
			Else
				dValue=Null
			End If
		End if
	Else
		dValue=Null
	End if
	getNextF=dValue
End Function
'======================================


'**************************************
'取得某一筆資料上一筆的某一欄位值
'======================================
Function getPreF(oRs,sIndex,sValue,dIndex)
Dim dValue
Dim strFilter
	strFilter=oRs.Fields(sIndex).Name & "=" & Cstr(sValue)
	If oRs.RecordCount>0 Then
		oRs.Find strFilter
		If Not oRs.Bof Then
			oRs.MovePrevious
			If Not oRs.Bof then
				dValue=oRs(dIndex)
			Else
				dValue=Null
			End If
		End if
	Else
		dValue=Null
	End if
	getPreF=dValue

End Function
'======================================



'**************************************
'RecordSet 轉換成 HTML Table
'======================================
'oRs:RecordSet
'strScriptName:Form目標程式名稱
'strQS:其他QueryString 參數
'strTbWidth:Table 寬度
'aryThWidth:欄位寬度
'arySubmit:Submit 按鈕要出現的文字
'NName:流水號欄要出現的文字
'PFlag:是否要Pager出現
'strImagePath:Pager 的影像位置
'insURL:新增按鈕
'insName:按鈕名稱
'====================================

Function RsToTable (ByRef oRs,strScriptName,strQS,intPK,intLink,strHrefA,strHrefB,intPageSize,strCaption,strTbWidth,_
						  ByRef aryThWidth(),_
						  arySubmit,intHidden,NName,PFlag,strImagePath,_
						  aryIns)

	Dim strRs						'資料錄轉HTML
	Dim strPager					'分頁內容
	Dim i,j,k						'迴圈用變數
	Dim strHd						'Table標題
	Dim strThWidth					'標題欄位Width
	Dim strTRBK						'Row BackGround Color
	Dim strLink						'strHref+parameter

	Dim intPageNo					'分頁頁碼
	Dim strFt
	Dim strAction					'Action URL
	Dim intRowCountDown
	Dim intColspan
	Dim Index

	intColspan=oRs.Fields.Count-1-intHidden

	if IsArray(arySubmit) Then intColspan=intColspan+UBound(arySubmit)+1
	if Len(NName)>0 Then intColspan=intColspan+1

	j=0
	i=0
	k=0
	intRowCountDown=intPageSize

	intPageNo=Trim(Request("pageno"))
	If (Len(intPageNo)=0 or Not IsNumeric(intPageNo))Then intPageNo=1
	strAction=strScriptName & "?pageno="&intPageNO&"&"&strQS

	'---包含Form
	If IsArray(arySubmit) Then strRS=strRs & "<form name=""frmGrid"" action=""" & strAction & """ method=""post"">" & vbCrLf

	strRs=strRs & "<div class=gridiv ><table class=""gridtable"" border=""0"" cellspacing=""0"" cellpadding=""3"" width=""" & strTbWidth & """ >" & vbCRLf
	strRs=strRs & "<caption class=""gridcaption"">"

	If IsArray(aryIns) Then

		strRs=strRs & "<table border=""0"" width=""100%"" cellspacing=""0"" cellpadding=""0"" ><tr><td class=""gridtdcaption"">"&strCaption&"<td align=""right"">"
		For i=LBound(aryIns) to Ubound(aryIns)
			strRs=strRs & "<input class=""gridins"" type=""button"" value="""&aryIns(i) & """ onclick="""&aryIns(i+1)&""">&nbsp;"
			i=i+1
		Next
		strRs=strRs & "</tr></table></caption>"
	Else

		strRs=strRs & strCaption

	End If
	strRS=strRs & "</caption>"
	'==================================
	'產生標題
	strHd=strHd & "<tr class=gridtr>"
	If Len(NName)>0 Then strHd=strHd & "<th class=gridth >" & NName & "&nbsp;</th>" & vbCrLf

	'---自動從RecordSet取得欄位標題
	For i=0 to oRs.Fields.Count-1-intHidden

		Select Case i
		Case intPK
		Case Else			
			If IsArray(aryThWidth) Then strThWidth=aryThWidth(j)
			strHd=strHd & "<th class=""gridth"" width=" & strThWidth & ">" & oRs(i).Name & "</th>" & vbCrLf
			j=j+1
		End Select
	Next

	If IsArray(arySubmit) Then
		For i=LBound(arySubmit) to UBound(arySubmit)
			strHd=strHd & "<th class=gridth ><Input  onclick=check(this) name=cmd type=button value="""&arySubmit(i)&""">" & vbCrLf
		Next
		strFt=strHd & "</tr>"
	End If

	strHd=strHd & "</tr>" & vbCrLf
	strRs=strRs & strHd
	'==================================


	strPager=RsToPager(oRs,intPageSize,strQS,strImagePath,strTbWidth)

	If Len(NName)>1 Then
		If intPageNo>1 Then
			k=(intPageNo-1)*intPageSize
		Else
			k=0
		End If
	End If

	Dim q
	q=k
	'==================================
	'產生內容
	Dim strValue		'欄位值
	Dim strTrClass		'TRClass
	strTrClass="GridTrERow"
	While Not oRs.Eof and intRowCountDown>0				'-- 計算分頁--------------------


		If strTrClass="GridTrORow" Then
			strTrClass="GridTrERow"
		Else
			strTrClass="GridTrORow"
		End If



		strRS=strRS & "<Tr class="&strTrClass&">" & vbCrLf
		If Len(NName)>0 Then
			q=q+1
			strRs=strRs & "<td class=gridtd align=center >" & q & "</td>" & vbCrLf
		End If

		For i=0 to oRs.Fields.Count-1-intHidden

			strValue=oRs(i).Value
			If Not IsNull(strValue) Then strValue=Trim(Cstr(strValue))
			Select Case i
			Case intPK
			Case intLink
	  			strLink=strHrefA & oRs(intPK) & strHrefB
				strRS=strRS & "<Td class=gridtd><a class=grida href=""" & strLink & """>" & strValue & "</a>&nbsp;</Td>" & vbCrLf
			Case Else
				strRS=strRS & "<Td class=gridtd><div class=gridtd" &  i & ">" & strValue & "&nbsp;</div></Td>" & vbCrLf
			End Select
		Next

		If IsArray(arySubmit)  Then
			For i=Lbound(arySubmit) to UBound(arySubmit)

				If intHidden-i>0 Then
					k=oRs.Fields.Count+i-1					
					If oRs(k)=0 Then
						strRS=strRs & "<Td align=center valign=top class=gridtdsubmit >&nbsp;</Td>" & vbCrLf
					Else
						strRS=strRs & "<Td align=center valign=top class=gridtdsubmit ><Input class=gridcheck name=""did" & i & """ type=checkbox value=""" & oRs(intPK) & """></Td>" & vbCrLf
					End if
				Else
					strRS=strRs & "<Td align=center valign=top class=gridtdsubmit ><Input class=gridcheck name=""did" & i & """ type=checkbox value=""" & oRs(intPK) & """></Td>" & vbCrLf
				End If

			Next
		End If
		strRs=strRs & "</Tr>" & vbCrLf
		oRs.MoveNext
		intRowCountDown=intRowCountDown-1					'*****計算分頁*****
	Wend

	'==================================

	If intRowCountDown>0 Then

		For k=0 to (intRowcountDown-1)
			strRS=strRs & "<tr class=gridtrspace><Td class=gridspace colspan="& intColspan & ">&nbsp;</Tr>"
		Next
	End If


	strRS=strRS & "</table></div>"


	If IsArray(arySubmit) Then strRs=strRs & "</form>" & vbCrLf

	If PFlag=1 Then strRs=strRS & strPager


	RsToTable=strRs

End Function
'============================================

'********************************************
'RecordSet 分頁
'============================================
'intPageSize		'每頁筆數
'strPageNo			'目前要求頁數
'strQString			'Request String
'strImagePath		'相對於執行檔案的影像目錄位址
'*intRowCountDown=intPageSize
'============================================
Function RsToPager(ByRef oRs,intPageSize,strQString,strImagePath,strTbWidth)
	Dim intRsCount				'全部筆數
	Dim intTotalPage			'全部頁數
	Dim strPageNo				'目前要求頁數(STR)
	Dim intPageNo				'目前要求頁數(INT)
	Dim intRowCountDown
	Dim strScriptName
	strScriptName=Request.ServerVariables("SCRIPT_NAME")
	intRsCount=oRs.RecordCount												'全部筆數
	strPageNo=Trim(Request("PageNo"))									'取得目前要求頁數
	If intRsCount>0 Then														'若全部筆數大於零
		oRs.PageSize=intPageSize											'設定RecorSet PageSize
		intTotalPage=int(intRsCount/intPageSize*-1)*-1				'計算全部頁數
		If Len(strPageNo)=0 or (Not IsNumeric(strPageNo)) Then	'格式不合，則指定為第一頁
				intPageNo=1
		Else
				intPageNo=cInt(strPageNo)									'指定頁數
		End If
		If intPageNo > intTotalPage Then	intPageNo=intTotalPage	'若指定頁數，超過全部頁數則為最後一頁
		oRs.AbsolutePage=intPageNo											'資料錄移動至指定的頁數
	End If

	Dim strNextPage			'下一頁
	Dim strPrevPage			'上一頁
	Dim strPageSelect			'第?頁
	Dim QQStr					'Request參數字串
	Dim strAction
	Dim strURL
	Dim strPager
	Dim i

	'===Page Select Option===
	strPageSelect=IntToOption(1,intTotalPage,strPageNO)

	If Len(strQstring)=0 Then
		strAction=strScriptName & "?"
	Else
		strAction=strScriptName & "?" & strQstring & "&"
	End If
	strURL=strAction & "pageno="
	strNextPage=""
	strPrevPage=""
	'資料筆數大於每頁筆數時
	'If intRsCount>intPageSize Then
		If intPageNo<intTotalPage Then
			strNextPage="<a href=" & strURL & intPageNo+1 & ">" _
			& "<img border=0 src="&strImagePath&"next.gif></a>"
		Else
			strNextPage="<img border=0 src="&strImagePath&"next.gif>"
		End If

		If intPageNo>1 Then
			strPrevPage="<a href=" & strURL & intPageNo-1 & ">" _
			& "<img border=0 src="&strImagePath&"pre.gif></a>"
		Else
			strPrevPage="<img border=0 src="&strImagePath&"pre.gif>"
		End If

		strPager="<Form  name=rspager action=" & strAction & " method=post>" _
				 & "<Table width="&strTbWidth&"  cellspacing=0 cellpadding=0 border=0  style=border:0 valign=bottom><Tr><Td>" _
				 & "<Table  cellspacing=0 cellpadding=0 border=0 style=border:0>" _
				  & "<Tr>" _
				  & "<td align=left valign=top width=30px><img src=../../_images/_pager/n-left.gif >" _
				 & "<Td background=../../_images/_pager/n-line.gif >&nbsp;<span style=margin-right:20px;font-size:9pt;color:#666699>" _
				 & intPageNo & " 之 " & intTotalPage & " 頁/ " & intRsCount & " 筆 " & " </span></Td>" _
				& "<Td valign=top><a href=" & strURL & "1>" _
				 & "<img border=0 src="&strImagePath&"first.gif></a>" _
				 & strPrevPage & strNextPage _
				 & "<a href=" & strURL  & intTotalPage & ">" _
				 & "<img border=0 src="&strImagePath&"last.gif></a>" _
				 & "<td valign=top background=../../_images/_pager/n-line.gif >" _
				 & "<td width=20px>" _
				 & "<td ><span style=margin-right:5px;font-size:9pt;color:#666699>到第</span>" _
				 & "<td width=10px>" _
				 & "</Td><Td>" & vbCrLf _
				 & "<select Name=PageNo onChange=""document.forms['rspager'].submit()"">" & strPageSelect & "</select>" & vbCrLf _
				 & "<td width=10px>" _
				 & "</Td><Td width=20><span style=margin-right:10px;font-size:8pt;color:#666699>頁</span>" _
				 & "</Tr></Table></Tr></Table></Form>"
	'Else
	'	strPager=""
	'End if
	RsToPager=strPager
End Function


Function RsToPagerAJAX(ByRef oRs,intPageSize,strQString,strImagePath,strTbWidth)
	Dim intRsCount				'全部筆數
	Dim intTotalPage			'全部頁數
	Dim strPageNo				'目前要求頁數(STR)
	Dim intPageNo				'目前要求頁數(INT)
	Dim intRowCountDown
	Dim strScriptName
	strScriptName=Request.ServerVariables("SCRIPT_NAME")
	intRsCount=oRs.RecordCount												'全部筆數
	strPageNo=Trim(Request("PageNo"))									'取得目前要求頁數
	If intRsCount>0 Then														'若全部筆數大於零
		oRs.PageSize=intPageSize											'設定RecorSet PageSize
		intTotalPage=int(intRsCount/intPageSize*-1)*-1				'計算全部頁數
		If Len(strPageNo)=0 or (Not IsNumeric(strPageNo)) Then	'格式不合，則指定為第一頁
				intPageNo=1
		Else
				intPageNo=cInt(strPageNo)									'指定頁數
		End If
		If intPageNo > intTotalPage Then	intPageNo=intTotalPage	'若指定頁數，超過全部頁數則為最後一頁
		oRs.AbsolutePage=intPageNo											'資料錄移動至指定的頁數
	End If

	Dim strNextPage			'下一頁
	Dim strPrevPage			'上一頁
	Dim strPageSelect			'第?頁
	Dim QQStr					'Request參數字串
	Dim strAction
	Dim strURL
	Dim strPager
	Dim i

	'===Page Select Option===
	strPageSelect=IntToOption(1,intTotalPage,strPageNO)

	If Len(strQstring)=0 Then
		strAction=strScriptName & "?"
	Else
		strAction=strScriptName & "?" & strQstring & "&"
	End If
	strURL=strAction & "pageno="
	strNextPage=""
	strPrevPage=""
	'資料筆數大於每頁筆數時
	'If intRsCount>intPageSize Then
		If intPageNo<intTotalPage Then
			strNextPage="<a href=""javascript:AjaxChangePage(" & intPageNo+1 & ")"">" _
			& "<img border=0 src="&strImagePath&"next.gif></a>"
		Else
			strNextPage="<img border=0 src="&strImagePath&"next.gif>"
		End If

		If intPageNo>1 Then
			strPrevPage="<a href=""javascript:AjaxChangePage(" & intPageNo-1 & ")"">" _
			& "<img border=0 src="&strImagePath&"pre.gif></a>"
		Else
			strPrevPage="<img border=0 src="&strImagePath&"pre.gif>"
		End If
		
		'strPager="<Form id=""rspager""  name=""rspager"" action=""" & strAction & """ method=""post"">" _
		strPager="" _
				& "<Table width="&strTbWidth&"  cellspacing=0 cellpadding=0 border=0  style=border:0 valign=bottom><Tr><Td>" _
				& "<Table  cellspacing=0 cellpadding=0 border=0 style=border:0>" _
				& "<Tr>" _
				& "<td align=left valign=top width=30px><img src=../../_images/_pager/n-left.gif >" _
				& "<Td background=../../_images/_pager/n-line.gif >&nbsp;<span style=margin-right:20px;font-size:9pt;color:#666699>" _
				& intPageNo & " 之 " & intTotalPage & " 頁/ " & intRsCount & " 筆 " & " </span></Td>" _
				& "<Td valign=top><a href=""javascript:AjaxChangePage(1)"">" _
				 & "<img border=0 src="&strImagePath&"first.gif></a>" _
				 & strPrevPage & strNextPage _
				 & "<a href=""javascript:AjaxChangePage(" & intTotalPage & ")"">" _
				 & "<img border=0 src="&strImagePath&"last.gif></a>" _
				 & "<td valign=top background=../../_images/_pager/n-line.gif >" _
				 & "<td width=20px>" _
				 & "<td ><span style=margin-right:5px;font-size:9pt;color:#666699>到第</span>" _
				 & "<td width=10px>" _
				 & "</Td><Td>" & vbCrLf _
				 & "<select Name=""PageNo"" id=""PageNo"">" & strPageSelect & "</select>" & vbCrLf _
				 & "<td width=10px>" _
				 & "</Td><Td width=20><span style=margin-right:10px;font-size:8pt;color:#666699>頁</span>" _
				 & "</Tr></Table></Tr></Table>"
	'Else
	'	strPager=""
	'End if
	RsToPagerAJAX = strPager
End Function

'==============================================


'**************************************
'數列轉換成 HTML Option Element
'======================================
Function IntToOption(intFst,intLst,strPK)
Dim strHTML
Dim i
Dim j
	For i=intFst to intLst
		if cstr(i)=strPK then
			j=" selected "
		else
			j=""
		end if
		strHTML=strHTML & "<option " & j & "value=""" & i & """>" & i & vbCrLf
	Next
	IntToOption=strHTML
End Function
'======================================

'**************************************
'陣列 轉換成 HTML Option Element
'======================================
Function AryToOption(ArrayIntKey,ArrayStrData,intKey,intText,strSel,strOColor,strGColor)
Dim strOption
Dim strKey
Dim strText
Dim strSelected
Dim strFix
Dim strBGColor
Dim i
	If IsNull(strSel) Then strSel=""
	If Len(strOcolor)=0 Then strOColor="white"
	If Len(strGColor)=0 Then strGColor="#E6E6FA"
	If UBound(ArrayIntKey) <> UBound(ArrayStrData) Then	RESPONSE.WRITE "錯誤!!AryToOption 陣列數不對等!!"
	strBGColor=strOColor
	For i=0 to Ubound(ArrayStrData)
		If strBGColor=strOColor Then strBGColor=strGColor Else strBGColor=strOColor
		If intKey=0 Then strKey=ArrayIntKey(i) Else strKey=ArrayStrData(i)
		If intText=0 Then strText=ArrayIntKey(i) Else strText=ArrayStrData(i)
		If trim(CStr(strKey))=trim(CStr(strSel)) Then strFix=" selected " Else strFix=""
		strOption=strOption & "<option style=""background-color:" & strBGColor & """" & strFix & " value=""" & strKey & """>" & strText
	Next
	AryToOption=strOption
End Function
'======================================


'**************************************
'RecordSet 轉換成 HTML Option Element
'======================================
Function RsToOption(Byref oRs,intKey,intText,strSel,strOColor,strGColor)
Dim strOption
Dim strKey
Dim strText
Dim strSelected
Dim strFix
Dim strBGColor
	If Len(strOcolor)=0 Then strOColor="white"
	If Len(strGColor)=0 Then strGColor="#E6E6FA"

	strBGColor=strOColor
	While Not oRs.eof

		If strBGColor=strOColor Then
			strBGColor=strGColor
		Else
			strBGColor=strOColor
		End iF
		strKey=oRs(intKey)

		strText=oRs(intText)
		If trim(cStr(strKey))=strSel then
			strFix=" selected "
		else
			strFix=""
		End if
		strOption=strOption & "<option style=""background-color:" & strBGColor & """" & strFix & " value=""" & strKey & """>" & strText
		oRs.MoveNext
	Wend
	RsToOption=strOption
End Function
'======================================

'-- 數字的取代-------------------------------------------
Function Number(Num)
Dim StrNum,StrNumber
Dim i

	for i=1 to Len(Cstr(Num))
	StrNumber=Mid(Cstr(Num),i,1)

		Select Case Trim(StrNumber)
			Case "1"
				StrNum=StrNum & "壹"
			Case "2"
				StrNum=StrNum & "貳"
			Case "3"
				StrNum=StrNum & "參"
			Case "4"
				StrNum=StrNum & "肆"
			Case "5"
				StrNum=StrNum & "伍"
			Case "6"
				StrNum=StrNum & "陸"
			Case "7"
				StrNum=StrNum & "柒"
			Case "8"
				StrNum=StrNum & "捌"
			Case "9"
				StrNum=StrNum & "玖"
		End Select
	Next
	StrNum=StrNum &"、"
	Number=StrNum

End Function'============================================

'-- 數字的取代-------------------------------------------
Function NumberY(Num)
Dim StrNum,StrNumber
Dim i

	for i=1 to Len(Cstr(Num))
	StrNumber=Mid(Cstr(Num),i,1)

		Select Case Trim(StrNumber)
		Case 1
			StrNum=StrNum & "一"
		Case 2
			StrNum=StrNum & "二"
		Case 3
			StrNum=StrNum & "三"
		Case 4
			StrNum=StrNum & "四"
		Case 5
			StrNum=StrNum & "五"
		Case 6
			StrNum=StrNum & "六"
		Case 7
			StrNum=StrNum & "七"
		Case 8
			StrNum=StrNum & "八"
		Case 9
			StrNum=StrNum & "九"
	End Select
	Next
	StrNum=StrNum &"、"
	NumberY=StrNum

End Function
'=========================================

'**************************************
'RecordSet 轉換成 HTML text Element
'======================================
Function RsToText(Byref oRs,intKey,intText,intValue,strName,strName2,strOColor,strGColor)
Dim strOption
Dim strKey
Dim strText
Dim strSelected
Dim strFix
Dim strBGColor
Dim strValue
Dim i
i=1
	If Len(strOcolor)=0 Then strOColor="white"
	If Len(strGColor)=0 Then strGColor="#E6E6FA"


	strOption="<table width=100% cellspacing=0 cellpadding=0 border=1>"
	strBGColor=strOColor
	While Not oRs.eof
		If strBGColor=strOColor Then
			strBGColor=strGColor
		Else
			strBGColor=strOColor
		End iF

		strKey=oRs(intKey)
		strText=oRs(intText)
		strValue=oRs(intValue)

		strOption=strOption & "<tr bgcolor=" & strBGColor & "><td align=right><input style=width:25; name=" & strName &"  type=text value="& i &" maxlength=2></td><td>" & strText & "<input type=hidden name=" & strName2 & " value= "& strKey & " ></tr>" & vbcrlf

		i=i+1
		oRs.MoveNext
	Wend
	strOption=strOPtion & "</table>"
	RsToText=strOption
End Function
'======================================







Function RsToPower(oRs,inputName)
Dim strPower
Dim strIndex
Dim strName
Dim strCheck
	strPower="<table border=0>"
	While Not oRs.Eof
		strCheck=""
		strIndex=oRs(0)
		strName=oRs(1)
		If oRs(2)=1 Then strCheck=" checked "
		strPower=strPower & "<tr><td><input type=""checkbox"" name="""&inputName&""" " _
								& " value="""&strIndex&""" "&strCheck&">" & vbCrLf _
								& "<td>"&strName&"</tr>" & vbCrLf
		oRs.MoveNext
	Wend
	strPower=strPower & "</table>"
	RsToPower=strPower
End Function


'**************************************
'RecordSet 轉換成 HTML Table
'======================================
'oRs:RecordSet
'strScriptName:Form目標程式名稱
'strQS:其他QueryString 參數
'strTbWidth:Table 寬度
'aryThWidth:欄位寬度
'arySubmit:Submit 按鈕要出現的文字
'NName:流水號欄要出現的文字
'PFlag:是否要Pager出現
'strImagePath:Pager 的影像位置
'insURL:新增按鈕
'insName:按鈕名稱
'====================================
Function RsToTable_SelectAll (ByRef oRs,strScriptName,strQS,intPK,intLink,strHrefA,strHrefB,intPageSize,strCaption,strTbWidth,_
						  ByRef aryThWidth(),_
						  arySubmit,intHidden,NName,PFlag,strImagePath, _
						  aryIns,OpenWindowStatus,HighLightStatus,SelectAllStatus)

	Dim strRs					'資料錄轉HTML
	Dim strPager					'分頁內容
	Dim i,j,k					'迴圈用變數
	Dim strHd					'Table標題
	Dim strThWidth					'標題欄位Width
	Dim strTRBK					'Row BackGround Color
	Dim strLink					'strHref+parameter

	Dim intPageNo					'分頁頁碼
	Dim strFt
	Dim strAction					'Action URL
	Dim intRowCountDown
	Dim intColspan
	Dim Index
	Dim strMouseOverRowColor,strMouseOver,strMouseOut

	'-- 顯示光棒---------------------------------
	If HighLightStatus = "HighLight_Yes" Then
		strMouseOverRowColor="#FFD29E"
	ElseIf HighLightStatus = "HighLight_No" OR HighLightStatus = "" Then
		strMouseOverRowColor=""
	End If

	strRS=strRS & "<input type=hidden name=strBgColor>"

	intColspan=oRs.Fields.Count-1-intHidden

	if IsArray(arySubmit) Then intColspan=intColspan+UBound(arySubmit)+1
	if Len(NName)>0 Then intColspan=intColspan+1

	j=0
	i=0
	k=0
	intRowCountDown=intPageSize

	intPageNo=Trim(Request("pageno"))
	If (Len(intPageNo)=0 or Not IsNumeric(intPageNo))Then intPageNo=1
	strAction=strScriptName & "?pageno="&intPageNO&"&"&strQS

	'-- 包含Form
	If IsArray(arySubmit) Then strRS=strRs & "<Form name=frmGrid action=" & strAction & " method=post>" & vbCrLf

	strRs=strRs & "<Div class=gridiv ><Table class=gridtable border=0 cellspacing=0 cellpadding=3 width=""" & strTbWidth & """ >" & vbCRLf
	strRs=strRs & "<caption class=gridcaption>"

	If IsArray(aryIns) Then

		strRs=strRs & "<table border=0 width=100% cellspacing=0 cellpadding=0 ><tr><td class=gridtdcaption>"&strCaption&"<td align=right>"
		For i=LBound(aryIns) to Ubound(aryIns)
			strRs=strRs & "<input class=gridins type=button value="""&aryIns(i) & """ onclick="""&aryIns(i+1)&""">&nbsp;"
			i=i+1
		Next
		strRs=strRs & "</tr></table></caption>"
	Else

		strRs=strRs & strCaption

	End If
	strRS=strRs & "</caption>"
	'==================================
	'產生標題
	strHd=strHd & "<Tr class=gridtr>"
	If Len(NName)>0 Then strHd=strHd & "<Th class=gridth >"&NName&"&nbsp;</th>" & vbCrLf

	'---自動從RecordSet取得欄位標題
	For i=0 to oRs.Fields.Count-1-intHidden

		Select Case i
		Case intPK
		Case Else
			If IsArray(aryThWidth) Then strThWidth=aryThWidth(j)
			strHd=strHd & "<Th class=gridth width=" & strThWidth & ">" & oRs(i).Name & "</Th>" & vbCrLf
			j=j+1
		End Select

	Next

	If IsArray(arySubmit) Then
		For i=LBound(arySubmit) to UBound(arySubmit)
			'strHd=strHd & "<Th class=gridth ><Input class=gridSubmit onclick=check(this) name=cmd type=button value="""&arySubmit(i)&""">" & vbCrLf
			'strHd=strHd & "<Th class=gridth ><Input  onclick=check(this) name=cmd type=button value="""&arySubmit(i)&""">" & vbCrLf
			strHd=strHd & "<Th class=gridth >"
			strHd=strHd & "<Input onclick=check(this) name=cmd type=button value="""&arySubmit(i)&""">"
			'-- 這裡判斷 SelectAllStatus
			'//先把刪除的select all拿掉
			'If SelectAllStatus = "SelectAll_Yes" Then
				'strHd=strHd & "<Input onclick=CheckSelect(this) name=did"&i&" type=checkbox value=0 class=gridcheck>"& vbCrLf
			'ElseIf SelectAllStatus = "SelectAll_No" Then
				'strHd=strHd & vbCrLf
			'End If
		Next
		strFt=strHd & "</tr>"
	End If

	strHd=strHd & "</Tr>" & vbCrLf
	strRs=strRs & strHd
	'==================================


	strPager=RsToPager(oRs,intPageSize,strQS,strImagePath,strTbWidth)

	If Len(NName)>1 Then
		If intPageNo>1 Then
			k=(intPageNo-1)*intPageSize
		Else
			k=0
		End If
	End If

	Dim q
	q=k
	'==================================
	'產生內容
	Dim strValue		'欄位值
	Dim strTrClass		'TRClass
	strTrClass="bgcolor=#E6E6FA"

	if not strMouseOverRowColor="" then
		'strMouseOver="strBgColor.value=this.bgColor;this.bgColor='"& strMouseOverRowColor &"';"
		'strMouseOut="this.bgColor=strBgColor.value"

    	strMouseOver=""
		strMouseOut=""
	end if

	'-- 計算分頁--------------------
	While Not oRs.Eof and intRowCountDown>0


		If strTrClass="bgcolor=#E6E6FA" Then
			strTrClass="bgcolor=white"
		Else
			strTrClass="bgcolor=#E6E6FA"
		End If


		strRS=strRS & "<Tr height=30px "&strTrClass&" onmouseover="""& strMouseOver &""" onmouseout="""& strMouseOut &""">" & vbCrLf


		If Len(NName)>0 Then
			q=q+1
			strRs=strRs & "<td class=gridtd align=center >" & q & "</td>" & vbCrLf
		End If

		For i=0 to oRs.Fields.Count-1-intHidden
			strValue=oRs(i).Value
			If Not IsNull(strValue) Then strValue=Trim(Cstr(strValue))
			Select Case i
			Case intPK
			Case intLink
	  			'strLink=strHrefA & oRs(intPK) & strHrefB
				'strRS=strRS & "<Td class=gridtd><a class=grida href=""" & strLink & """>" & strValue & "</a>&nbsp;</Td>" & vbCrLf

				'-- FIX BY JACK AT 20040708----------------
				IF strHrefB <>"" THEN
					Dim strHrefB_Ary
					Dim strHrefB_Tmp
					Dim N
					strHrefB_Ary=SPLIT(strHrefB,"|")
					strHrefB_Tmp=""
					FOR N = 0 To UBOUND(strHrefB_Ary)
						'-- 因為本陣列必須要兩個兩個一組(第一個是要LINK名稱,第二個是要LINK的欄位
						'-- 將陣列子加一後 MOD 2 取得1 或 0
						IF (N + 1) MOD 2 = 1 THEN
							strHrefB_Tmp=strHrefB_Tmp & "&" & strHrefB_Ary(N) & "="
						ELSE
							strHrefB_Tmp=strHrefB_Tmp & oRs(Cint(strHrefB_Ary(N)))
						End If
					NEXT
					'Response.Write strHrefB_Tmp
					'Response.End

	  				strLink=strHrefA & oRs(intPK) & strHrefB_Tmp
				ELSE
		  			strLink=strHrefA & oRs(intPK)
				End IF
				'===========================================
				'-- FIX BY JACK AT 20040921-----------------
				'-- 超連結時,是否要另外開視窗
				If OpenWindowStatus = "OpenWindow_Yes" Then
					strRS=strRS & "<Td class=gridtd><a href=#  onclick=""window.open('" & strLink & "',null,'width=800px,height=600px,location=no,menubar=no,status=no,resizable=yes,scrollbars=yes')"" class=grida>" & strValue & "</a>&nbsp;</Td>" & vbCrLf
				ElseIf OpenWindowStatus = "OpenWindow_No" OR OpenWindowStatus= "" Then
					strRS=strRS & "<Td class=gridtd><a class=grida href=""" & strLink & """  >" & strValue & "</a>&nbsp;</Td>" & vbCrLf
				End If
				'RESPONSE.WRITE strLink
				'RESPONSE.END
				'===========================================

			Case Else
				strRS=strRS & "<Td class=gridtd><div class=gridtd" &  i & ">" & strValue & "&nbsp;</div></Td>" & vbCrLf
			End Select
		Next



		If IsArray(arySubmit)  Then
			Dim M
			M=UBound(arySubmit)+1
			For i=Lbound(arySubmit) to UBound(arySubmit)
				k=oRs.Fields.Count-M + i
				If intHidden = 0 And IsNumeric(oRs(k)) Then
					If oRs(k)=0 Then
						strRS=strRs & "<Td align=center valign=top class=gridtdsubmit >&nbsp;</Td>" & vbCrLf
					Else
						strRS=strRs & "<Td align=center valign=top class=gridtdsubmit ><Input class=gridcheck name=""did" & i & """ type=checkbox value=""" & oRs(intPK) & """></Td>" & vbCrLf
					End if
				Else
					strRS=strRs & "<Td align=center valign=top class=gridtdsubmit ><Input class=gridcheck name=""did" & i & """ type=checkbox value=""" & oRs(intPK) & """></Td>" & vbCrLf
				End If

			Next
		End If



		strRs=strRs & "</Tr>" & vbCrLf
		oRs.MoveNext
		intRowCountDown=intRowCountDown-1					'*****計算分頁*****
	Wend

	'==================================

	If intRowCountDown>0 Then

		For k=0 to (intRowcountDown-1)
			strRS=strRs & "<Tr class=gridtrspace><Td class=gridspace colspan="& intColspan & ">&nbsp;</Tr>"
		Next
	End If


	strRS=strRS & "</Table></div>"


	If IsArray(arySubmit) Then strRs=strRs & "</Form>" & vbCrLf

	If PFlag=1 Then strRs=strRS & strPager


	RsToTable_SelectAll=strRs

End Function

'-- FIX BY JACK HESIH 20060814---------
'-- 以下項目把 CHECKBOX 改成 RADIOBOX
Function RsToTable_SelectItem (ByRef oRs,strScriptName,strQS,intPK,intLink,strHrefA,strHrefB,intPageSize,strCaption,strTbWidth,_
						ByRef aryThWidth(),_
						arySubmit,intHidden,NName,PFlag,strImagePath,_
						aryIns,OpenWindowStatus,HighLightStatus,SelectAllStatus)

	Dim strRs						'資料錄轉HTML
	Dim strPager					'分頁內容
	Dim i,j,k						'迴圈用變數
	Dim strHd						'Table標題
	Dim strThWidth					'標題欄位Width
	Dim strTRBK						'Row BackGround Color
	Dim strLink						'strHref+parameter

	Dim intPageNo					'分頁頁碼
	Dim strFt
	Dim strAction					'Action URL
	Dim intRowCountDown
	Dim intColspan
	Dim Index
	Dim strMouseOverRowColor,strMouseOver,strMouseOut

	'-- 顯示光棒---------------------------------
	If HighLightStatus = "HighLight_Yes" Then
		strMouseOverRowColor="#FFD29E"
	ElseIf HighLightStatus = "HighLight_No" OR HighLightStatus = "" Then
		strMouseOverRowColor=""
	End If
	strRS=strRS & "<input type=hidden name=strBgColor>"
	intColspan=oRs.Fields.Count-1-intHidden
	If IsArray(arySubmit) Then intColspan=intColspan+UBound(arySubmit)+1
	if Len(NName)>0 Then intColspan=intColspan+1

	j=0
	i=0
	k=0
	intRowCountDown=intPageSize

	intPageNo=Trim(Request("pageno"))
	If (Len(intPageNo)=0 or Not IsNumeric(intPageNo))Then intPageNo=1
	strAction=strScriptName & "?pageno="&intPageNO&"&"&strQS

	'---包含Form
	If IsArray(arySubmit) Then strRS=strRs & "<Form name=frmGrid action=" & strAction & " method=post>" & vbCrLf

	strRs=strRs & "<Div class=gridiv ><Table class=gridtable border=0 cellspacing=0 cellpadding=3 width=""" & strTbWidth & """ >" & vbCRLf
	strRs=strRs & "<caption class=gridcaption>"

	If IsArray(aryIns) Then

		strRs=strRs & "<table border=0 width=100% cellspacing=0 cellpadding=0 ><tr><td class=gridtdcaption>"&strCaption&"<td align=right>"
		For i=LBound(aryIns) to Ubound(aryIns)
			strRs=strRs & "<input class=gridins type=button value="""&aryIns(i) & """ onclick="""&aryIns(i+1)&""">&nbsp;"
			i=i+1
		Next
		strRs=strRs & "</tr></table></caption>"
	Else

		strRs=strRs & strCaption

	End If
	strRS=strRs & "</caption>"
	'==================================
	'產生標題
	strHd=strHd & "<Tr class=gridtr>"
	If Len(NName)>0 Then strHd=strHd & "<Th class=gridth >"&NName&"&nbsp;</th>" & vbCrLf

	'---自動從RecordSet取得欄位標題
	For i=0 to oRs.Fields.Count-1-intHidden

		Select Case i
		Case intPK
		Case Else
			If IsArray(aryThWidth) Then strThWidth=aryThWidth(j)
			strHd=strHd & "<Th class=gridth width=" & strThWidth & ">" & oRs(i).Name & "</Th>" & vbCrLf
			j=j+1
		End Select

	Next

	If IsArray(arySubmit) Then
		For i=LBound(arySubmit) to UBound(arySubmit)
			'strHd=strHd & "<Th class=gridth ><Input class=gridSubmit onclick=check(this) name=cmd type=button value="""&arySubmit(i)&""">" & vbCrLf
			strHd=strHd & "<Th class=gridth ><Input  onclick=check(this) name=cmd type=button value="""&arySubmit(i)&""">" & vbCrLf
		Next
		strFt=strHd & "</tr>"
	End If

	strHd=strHd & "</Tr>" & vbCrLf
	strRs=strRs & strHd
	'==================================


	strPager=RsToPager(oRs,intPageSize,strQS,strImagePath,strTbWidth)

	If Len(NName)>1 Then
		If intPageNo>1 Then
			k=(intPageNo-1)*intPageSize
		Else
			k=0
		End If
	End If

	Dim q
	q=k
	'==================================
	'產生內容
	Dim strValue		'欄位值
	Dim strTrClass		'TRClass
	strTrClass="GridTrERow"

	If not strMouseOverRowColor="" Then
		'strMouseOver="strBgColor.value=this.bgColor;this.bgColor='"& strMouseOverRowColor &"';"
		'strMouseOut="this.bgColor=strBgColor.value"

    	strMouseOver    =   ""
		strMouseOut     =   ""

	End If

	'-- 計算分頁--------------------
	While Not oRs.Eof and intRowCountDown>0


		If strTrClass="bgcolor=#E6E6FA" Then
			strTrClass="bgcolor=white"
		Else
			strTrClass="bgcolor=#E6E6FA"
		End If
		strRS=strRS & "<Tr height=30px "&strTrClass&" onmouseover="""& strMouseOver &""" onmouseout="""& strMouseOut &""">" & vbCrLf

		If Len(NName)>0 Then
			q=q+1
			strRs=strRs & "<td class=gridtd align=center >" & q & "</td>" & vbCrLf
		End If

		For i=0 to oRs.Fields.Count-1-intHidden

			strValue=oRs(i).Value
			If Not IsNull(strValue) Then strValue=Trim(Cstr(strValue))
			Select Case i
			Case intPK
			Case intLink
	  			strLink=strHrefA & oRs(intPK) & strHrefB
				strRS=strRS & "<Td class=gridtd><a class=grida href=""" & strLink & """>" & strValue & "</a>&nbsp;</Td>" & vbCrLf
			Case Else
				strRS=strRS & "<Td class=gridtd><div class=gridtd" &  i & ">" & strValue & "&nbsp;</div></Td>" & vbCrLf
			End Select
		Next

		If IsArray(arySubmit)  Then
			For i=Lbound(arySubmit) to UBound(arySubmit)

				If intHidden-i>0 Then
					k=oRs.Fields.Count+i-1
					If oRs(k)=0 Then
						strRS=strRs & "<Td align=center valign=top class=gridtdsubmit >&nbsp;</Td>" & vbCrLf
					Else
							strRS=strRs & "<Td align=center valign=top class=gridtdsubmit ><Input class=gridcheck name=""did" & i & """ type=radio value=""" & oRs(intPK) & """></Td>" & vbCrLf
					End if
				Else
					strRS=strRs & "<Td align=center valign=top class=gridtdsubmit ><Input class=gridcheck name=""did" & i & """ type=radio value=""" & oRs(intPK) & """></Td>" & vbCrLf
				End If

			Next
		End If
		strRs=strRs & "</Tr>" & vbCrLf
		oRs.MoveNext
		intRowCountDown=intRowCountDown-1					'*****計算分頁*****
	Wend

	'==================================

	If intRowCountDown>0 Then

		For k=0 to (intRowcountDown-1)
			strRS=strRs & "<Tr class=gridtrspace><Td class=gridspace colspan="& intColspan & ">&nbsp;</Tr>"
		Next
	End If


	strRS=strRS & "</Table></div>"


	If IsArray(arySubmit) Then strRs=strRs & "</Form>" & vbCrLf

	If PFlag=1 Then strRs=strRS & strPager
	RsToTable_SelectItem=strRs

End Function
'============================================


Function RsToTable_SelectAll_dialog (ByRef oRs,strScriptName,strQS,intPK,intLink,strHrefA,strHrefB,intPageSize,strCaption,strTbWidth,_
						  ByRef aryThWidth(),_
						  arySubmit,intHidden,NName,PFlag,strImagePath, _
						  aryIns,OpenWindowStatus,HighLightStatus,SelectAllStatus)

	Dim strRs						'資料錄轉HTML
	Dim strPager					'分頁內容
	Dim i,j,k						'迴圈用變數
	Dim strHd						'Table標題
	Dim strThWidth					'標題欄位Width
	Dim strTRBK						'Row BackGround Color
	Dim strLink						'strHref+parameter

	Dim intPageNo					'分頁頁碼
	Dim strFt
	Dim strAction					'Action URL
	Dim intRowCountDown
	Dim intColspan
	Dim Index
	Dim strMouseOverRowColor,strMouseOver,strMouseOut

	'-- 顯示光棒---------------------------------
	If HighLightStatus = "HighLight_Yes" Then
		strMouseOverRowColor="#FFD29E"
	ElseIf HighLightStatus = "HighLight_No" OR HighLightStatus = "" Then
		strMouseOverRowColor=""
	End If

	strRS=strRS & "<input type=""hidden"" name=""strBgColor"">"

	intColspan=oRs.Fields.Count-1-intHidden

	if IsArray(arySubmit) Then intColspan=intColspan+UBound(arySubmit)+1
	if Len(NName)>0 Then intColspan=intColspan+1

	j=0
	i=0
	k=0
	intRowCountDown=intPageSize

	intPageNo=Trim(Request("pageno"))
	If (Len(intPageNo)=0 or Not IsNumeric(intPageNo))Then intPageNo=1
	strAction=strScriptName & "?pageno="&intPageNO&"&"&strQS

	'-- 包含Form
	If IsArray(arySubmit) Then strRS=strRs & "<Form name=frmGrid action=" & strAction & " method=post>" & vbCrLf

	strRs=strRs & "<Div class=gridiv ><Table class=gridtable border=0 cellspacing=0 cellpadding=3 width=""" & strTbWidth & """ >" & vbCRLf
	strRs=strRs & "<caption class=gridcaption>"

	If IsArray(aryIns) Then

		strRs=strRs & "<table border=0 width=100% cellspacing=0 cellpadding=0 ><tr><td class=gridtdcaption>"&strCaption&"<td align=right>"
		For i=LBound(aryIns) to Ubound(aryIns)
			strRs=strRs & "<input class=gridins type=button value="""&aryIns(i) & """ onclick="""&aryIns(i+1)&""">&nbsp;"
			i=i+1
		Next
		strRs=strRs & "</tr></table></caption>"
	Else

		strRs=strRs & strCaption

	End If
	strRS=strRs & "</caption>"
	'==================================
	'產生標題
	strHd=strHd & "<Tr class=gridtr>"
	If Len(NName)>0 Then strHd=strHd & "<Th class=gridth >"&NName&"&nbsp;</th>" & vbCrLf

	'---自動從RecordSet取得欄位標題
	For i=0 to oRs.Fields.Count-1-intHidden

		Select Case i
		Case intPK
		Case Else
			If IsArray(aryThWidth) Then strThWidth=aryThWidth(j)
			strHd=strHd & "<Th class=gridth width=" & strThWidth & ">" & oRs(i).Name & "</Th>" & vbCrLf
			j=j+1
		End Select

	Next

	If IsArray(arySubmit) Then
		For i=LBound(arySubmit) to UBound(arySubmit)
			'strHd=strHd & "<Th class=gridth ><Input class=gridSubmit onclick=check(this) name=cmd type=button value="""&arySubmit(i)&""">" & vbCrLf
			'strHd=strHd & "<Th class=gridth ><Input  onclick=check(this) name=cmd type=button value="""&arySubmit(i)&""">" & vbCrLf
			strHd=strHd & "<Th class=gridth >"
			strHd=strHd & "<Input onclick=check(this) name=cmd type=button value="""&arySubmit(i)&""">"
			'-- 這裡判斷 SelectAllStatus
			'//先把刪除的select all拿掉
			'If SelectAllStatus = "SelectAll_Yes" Then
				'strHd=strHd & "<Input onclick=CheckSelect(this) name=did"&i&" type=checkbox value=0 class=gridcheck>"& vbCrLf
			'ElseIf SelectAllStatus = "SelectAll_No" Then
				'strHd=strHd & vbCrLf
			'End If
		Next
		strFt=strHd & "</tr>"
	End If

	strHd=strHd & "</Tr>" & vbCrLf
	strRs=strRs & strHd
	'==================================


	strPager=RsToPager(oRs,intPageSize,strQS,strImagePath,strTbWidth)

	If Len(NName)>1 Then
		If intPageNo>1 Then
			k=(intPageNo-1)*intPageSize
		Else
			k=0
		End If
	End If

	Dim q
	q=k
	'==================================
	'產生內容
	Dim strValue		'欄位值
	Dim strTrClass		'TRClass
	strTrClass="bgcolor=#E6E6FA"

	if not strMouseOverRowColor="" then
		'strMouseOver="strBgColor.value=this.bgColor;this.bgColor='"& strMouseOverRowColor &"';"
		'strMouseOut="this.bgColor=strBgColor.value"

    	strMouseOver=""
		strMouseOut=""
	end if

	'-- 計算分頁--------------------
	While Not oRs.Eof and intRowCountDown>0


		If strTrClass="bgcolor=#E6E6FA" Then
			strTrClass="bgcolor=white"
		Else
			strTrClass="bgcolor=#E6E6FA"
		End If


		strRS=strRS & "<Tr height=30px "&strTrClass&" onmouseover="""& strMouseOver &""" onmouseout="""& strMouseOut &""">" & vbCrLf


		If Len(NName)>0 Then
			q=q+1
			strRs=strRs & "<td class=gridtd align=center >" & q & "</td>" & vbCrLf
		End If

		For i=0 to oRs.Fields.Count-1-intHidden
			strValue=oRs(i).Value
			If Not IsNull(strValue) Then strValue=Trim(Cstr(strValue))
			Select Case i
			Case intPK
			Case intLink
	  			'strLink=strHrefA & oRs(intPK) & strHrefB
				'strRS=strRS & "<Td class=gridtd><a class=grida href=""" & strLink & """>" & strValue & "</a>&nbsp;</Td>" & vbCrLf

				'-- FIX BY JACK AT 20040708----------------
				IF strHrefB <>"" THEN
					Dim strHrefB_Ary
					Dim strHrefB_Tmp
					Dim N
					strHrefB_Ary=SPLIT(strHrefB,"|")
					strHrefB_Tmp=""
					FOR N = 0 To UBOUND(strHrefB_Ary)
						'-- 因為本陣列必須要兩個兩個一組(第一個是要LINK名稱,第二個是要LINK的欄位
						'-- 將陣列子加一後 MOD 2 取得1 或 0
						IF (N + 1) MOD 2 = 1 THEN
							strHrefB_Tmp=strHrefB_Tmp & "&" & strHrefB_Ary(N) & "="
						ELSE
							strHrefB_Tmp=strHrefB_Tmp & oRs(Cint(strHrefB_Ary(N)))
						End If
					NEXT
					'Response.Write strHrefB_Tmp
					'Response.End

	  				strLink=strHrefA & oRs(intPK) & strHrefB_Tmp
				ELSE
		  			strLink=strHrefA & oRs(intPK)
				End IF
				'===========================================
				'-- FIX BY JACK AT 20040921-----------------
				'-- 超連結時,是否要另外開視窗
				If OpenWindowStatus = "dialog" Then
					strRS=strRS & "<Td class=gridtd><a href=#  onClick=""content("&session("ID")&","&ors(0)&");"" >" & strValue & "</a>&nbsp;</Td>" & vbCrLf
				End If
				'RESPONSE.WRITE strLink
				'RESPONSE.END
				'===========================================

			Case Else
				strRS=strRS & "<Td class=gridtd><div class=gridtd" &  i & ">" & strValue & "&nbsp;</div></Td>" & vbCrLf
			End Select
		Next



		If IsArray(arySubmit)  Then
			Dim M
			M=UBound(arySubmit)+1
			For i=Lbound(arySubmit) to UBound(arySubmit)
				k=oRs.Fields.Count-M + i
				If intHidden = 0 And IsNumeric(oRs(k)) Then
					If oRs(k)=0 Then
						strRS=strRs & "<Td align=center valign=top class=gridtdsubmit >&nbsp;</Td>" & vbCrLf
					Else
						strRS=strRs & "<Td align=center valign=top class=gridtdsubmit ><Input class=gridcheck name=""did" & i & """ type=checkbox value=""" & oRs(intPK) & """></Td>" & vbCrLf
					End if
				Else
					strRS=strRs & "<Td align=center valign=top class=gridtdsubmit ><Input class=gridcheck name=""did" & i & """ type=checkbox value=""" & oRs(intPK) & """></Td>" & vbCrLf
				End If

			Next
		End If



		strRs=strRs & "</Tr>" & vbCrLf
		oRs.MoveNext
		intRowCountDown=intRowCountDown-1					'*****計算分頁*****
	Wend

	'==================================

	If intRowCountDown>0 Then

		For k=0 to (intRowcountDown-1)
			strRS=strRs & "<Tr class=gridtrspace><Td class=gridspace colspan="& intColspan & ">&nbsp;</Tr>"
		Next
	End If


	strRS=strRS & "</Table></div>"


	If IsArray(arySubmit) Then strRs=strRs & "</Form>" & vbCrLf

	If PFlag=1 Then strRs=strRS & strPager


	RsToTable_SelectAll_dialog=strRs

End Function




'-- 視窗大小可控制,視窗出現位置置中 JACK20080617 Fixed
Function RsToTable_SelectAllV2 (ByRef oRs,strScriptName,strQS,intPK,intLink,strHrefA,strHrefB,intPageSize,strCaption,strTbWidth,_
						  ByRef aryThWidth(),_
						  arySubmit,intHidden,NName,PFlag,strImagePath, _
						  aryIns,OpenWindowStatus,HighLightStatus,SelectAllStatus, _
						  OpenWindowWidth,OpenWindowHeight)

	Dim strRs						'資料錄轉HTML
	Dim strPager					'分頁內容
	Dim i,j,k						'迴圈用變數
	Dim strHd						'Table標題
	Dim strThWidth					'標題欄位Width
	Dim strTRBK						'Row BackGround Color
	Dim strLink						'strHref+parameter

	Dim intPageNo					'分頁頁碼
	Dim strFt
	Dim strAction					'Action URL
	Dim intRowCountDown
	Dim intColspan
	Dim Index
	Dim strMouseOverRowColor,strMouseOver,strMouseOut

	'-- 顯示光棒---------------------------------
	If HighLightStatus = "HighLight_Yes" Then
		strMouseOverRowColor="#FFD29E"
	ElseIf HighLightStatus = "HighLight_No" OR HighLightStatus = "" Then
		strMouseOverRowColor=""
	End If

	strRS=strRS & "<input type=hidden name=strBgColor>"

	intColspan=oRs.Fields.Count-1-intHidden

	if IsArray(arySubmit) Then intColspan=intColspan+UBound(arySubmit)+1
	if Len(NName)>0 Then intColspan=intColspan+1

	j=0
	i=0
	k=0
	intRowCountDown=intPageSize

	intPageNo=Trim(Request("pageno"))
	If (Len(intPageNo)=0 or Not IsNumeric(intPageNo))Then intPageNo=1
	strAction=strScriptName & "?pageno="&intPageNO&"&"&strQS

	'-- 包含Form
	If IsArray(arySubmit) Then strRS=strRs & "<Form name=frmGrid action=" & strAction & " method=post>" & vbCrLf

	strRs=strRs & "<Div class=gridiv ><Table class=gridtable border=0 cellspacing=0 cellpadding=3 width=""" & strTbWidth & """ >" & vbCRLf
	strRs=strRs & "<caption class=gridcaption>"

	If IsArray(aryIns) Then

		strRs=strRs & "<table border=0 width=100% cellspacing=0 cellpadding=0 ><tr><td class=gridtdcaption>"&strCaption&"<td align=right>"
		For i=LBound(aryIns) to Ubound(aryIns)
			strRs=strRs & "<input class=gridins type=button value="""&aryIns(i) & """ onclick="""&aryIns(i+1)&""">&nbsp;"
			i=i+1
		Next
		strRs=strRs & "</tr></table></caption>"
	Else

		strRs=strRs & strCaption

	End If
	strRS=strRs & "</caption>"
	'==================================
	'產生標題
	strHd=strHd & "<Tr class=gridtr>"
	If Len(NName)>0 Then strHd=strHd & "<Th class=gridth >"&NName&"&nbsp;</th>" & vbCrLf

	'---自動從RecordSet取得欄位標題
	For i=0 to oRs.Fields.Count-1-intHidden

		Select Case i
		Case intPK
		Case Else
			If IsArray(aryThWidth) Then strThWidth=aryThWidth(j)
			strHd=strHd & "<Th class=gridth width=" & strThWidth & ">" & oRs(i).Name & "</Th>" & vbCrLf
			j=j+1
		End Select

	Next

	If IsArray(arySubmit) Then
		For i=LBound(arySubmit) to UBound(arySubmit)
			'strHd=strHd & "<Th class=gridth ><Input class=gridSubmit onclick=check(this) name=cmd type=button value="""&arySubmit(i)&""">" & vbCrLf
			'strHd=strHd & "<Th class=gridth ><Input  onclick=check(this) name=cmd type=button value="""&arySubmit(i)&""">" & vbCrLf
			strHd=strHd & "<Th class=gridth >"
			strHd=strHd & "<Input onclick=check(this) name=cmd type=button value="""&arySubmit(i)&""">"
			'-- 這裡判斷 SelectAllStatus
			If SelectAllStatus = "SelectAll_Yes" Then
				strHd=strHd & "<Input onclick=CheckSelect(this) name=did"&i&" type=checkbox value=0 class=gridcheck>"& vbCrLf
			ElseIf SelectAllStatus = "SelectAll_No" Then
				strHd=strHd & vbCrLf
			End If
		Next
		strFt=strHd & "</tr>"
	End If

	strHd=strHd & "</Tr>" & vbCrLf
	strRs=strRs & strHd
	'==================================


	strPager=RsToPager(oRs,intPageSize,strQS,strImagePath,strTbWidth)

	If Len(NName)>1 Then
		If intPageNo>1 Then
			k=(intPageNo-1)*intPageSize
		Else
			k=0
		End If
	End If

	Dim q
	q=k
	'==================================
	'產生內容
	Dim strValue		'欄位值
	Dim strTrClass		'TRClass
	strTrClass="bgcolor=#E6E6FA"

	if not strMouseOverRowColor="" then
		'strMouseOver="strBgColor.value=this.bgColor;this.bgColor='"& strMouseOverRowColor &"';"
		'strMouseOut="this.bgColor=strBgColor.value"

    	strMouseOver=""
		strMouseOut=""
	end if

	'-- 計算分頁--------------------
	While Not oRs.Eof and intRowCountDown>0


		If strTrClass="bgcolor=#E6E6FA" Then
			strTrClass="bgcolor=white"
		Else
			strTrClass="bgcolor=#E6E6FA"
		End If


		strRS=strRS & "<Tr height=30px "&strTrClass&" onmouseover="""& strMouseOver &""" onmouseout="""& strMouseOut &""">" & vbCrLf


		If Len(NName)>0 Then
			q=q+1
			strRs=strRs & "<td class=gridtd align=center >" & q & "</td>" & vbCrLf
		End If

		For i=0 to oRs.Fields.Count-1-intHidden
			strValue=oRs(i).Value
			If Not IsNull(strValue) Then strValue=Trim(Cstr(strValue))
			Select Case i
			Case intPK
			Case intLink
	  			'strLink=strHrefA & oRs(intPK) & strHrefB
				'strRS=strRS & "<Td class=gridtd><a class=grida href=""" & strLink & """>" & strValue & "</a>&nbsp;</Td>" & vbCrLf

				'-- FIX BY JACK AT 20040708----------------
				IF strHrefB <>"" THEN
					Dim strHrefB_Ary
					Dim strHrefB_Tmp
					Dim N
					strHrefB_Ary=SPLIT(strHrefB,"|")
					strHrefB_Tmp=""
					FOR N = 0 To UBOUND(strHrefB_Ary)
						'-- 因為本陣列必須要兩個兩個一組(第一個是要LINK名稱,第二個是要LINK的欄位
						'-- 將陣列子加一後 MOD 2 取得1 或 0
						IF (N + 1) MOD 2 = 1 THEN
							strHrefB_Tmp=strHrefB_Tmp & "&" & strHrefB_Ary(N) & "="
						ELSE
							strHrefB_Tmp=strHrefB_Tmp & oRs(Cint(strHrefB_Ary(N)))
						End If
					NEXT
					'Response.Write strHrefB_Tmp
					'Response.End

	  				strLink=strHrefA & oRs(intPK) & strHrefB_Tmp
				ELSE
		  			strLink=strHrefA & oRs(intPK)
				End IF
				'===========================================
				'-- FIX BY JACK AT 20080617-----------------
				'-- 超連結時,是否要另外開視窗,視窗大小可控制,視窗出現位置置中
				If OpenWindowStatus = "OpenWindow_Yes" Then
					'strRS=strRS & "<Td class=gridtd><a href=#  onclick=""window.open('" & strLink & "',null,'top="&intTop&",left="&intLeft&",width="&OpenWindowWidth&"px,height="&OpenWindowHeight&"px,location=no,menubar=no,status=no,resizable=yes,scrollbars=yes')"" class=grida>" & strValue & "</a>&nbsp;</Td>" & vbCrLf
					strRS=strRS & "<Td class=gridtd><a href=#  onclick=""openWin('" & strLink & "',null,'"&OpenWindowWidth&"','"&OpenWindowHeight&"')"" class=grida>" & strValue & "</a>&nbsp;</Td>" & vbCrLf
				ElseIf OpenWindowStatus = "OpenWindow_No" OR OpenWindowStatus= "" Then
					strRS=strRS & "<Td class=gridtd><a class=grida href=""" & strLink & """  >" & strValue & "</a>&nbsp;</Td>" & vbCrLf
				End If
				'RESPONSE.WRITE strLink
				'RESPONSE.END
				'===========================================

			Case Else
				strRS=strRS & "<Td class=gridtd><div class=gridtd" &  i & ">" & strValue & "&nbsp;</div></Td>" & vbCrLf
			End Select
		Next


		'-- FIX BY JACK AT 200800812-----------------------
		'-- 列表紀錄中，刪除，審核，發送是否可勾選
		If IsArray(arySubmit)  Then
			Dim M
			M=UBound(arySubmit)+1
			For i=Lbound(arySubmit) to UBound(arySubmit)
				k=oRs.Fields.Count-M + i
				If intHidden = 0 And IsNumeric(oRs(k)) Then
					If oRs(k)=0 Then
						strRS=strRs & "<Td align=center valign=top class=gridtdsubmit >&nbsp;</Td>" & vbCrLf
					Else
						strRS=strRs & "<Td align=center valign=top class=gridtdsubmit ><Input class=gridcheck name=""did" & i & """ type=checkbox value=""" & oRs(intPK) & """></Td>" & vbCrLf
					End if
				Else
					strRS=strRs & "<Td align=center valign=top class=gridtdsubmit ><Input class=gridcheck name=""did" & i & """ type=checkbox value=""" & oRs(intPK) & """></Td>" & vbCrLf
				End If

			Next
		End If
		'==================================================


		strRs=strRs & "</Tr>" & vbCrLf
		oRs.MoveNext
		intRowCountDown=intRowCountDown-1					'*****計算分頁*****
	Wend

	'==================================

	If intRowCountDown>0 Then

		For k=0 to (intRowcountDown-1)
			strRS=strRs & "<Tr class=gridtrspace><Td class=gridspace colspan="& intColspan & ">&nbsp;</Tr>"
		Next
	End If


	strRS=strRS & "</Table></div>"


	If IsArray(arySubmit) Then strRs=strRs & "</Form>" & vbCrLf

	If PFlag=1 Then strRs=strRS & strPager


	RsToTable_SelectAllV2=strRs

End Function

'------Ajax Ver Jerry 2011/11/17
Function RsToTableAJAX (ByRef oRs,strScriptName,strQS,intPK,intLink,strHrefA,strHrefB,intPageSize,strCaption,strTbWidth,_
						  ByRef aryThWidth(),_
						  arySubmit,intHidden,NName,PFlag,strImagePath, _
						  aryIns,OpenWindowStatus,HighLightStatus,SelectAllStatus)

	Dim strRs					'資料錄轉HTML
	Dim strPager					'分頁內容
	Dim i,j,k					'迴圈用變數
	Dim strHd					'Table標題
	Dim strThWidth					'標題欄位Width
	Dim strTRBK					'Row BackGround Color
	Dim strLink					'strHref+parameter

	Dim intPageNo					'分頁頁碼
	Dim strFt
	Dim strAction					'Action URL
	Dim intRowCountDown
	Dim intColspan
	Dim Index
	Dim strMouseOverRowColor,strMouseOver,strMouseOut

	'-- 顯示光棒---------------------------------
	If HighLightStatus = "HighLight_Yes" Then
		strMouseOverRowColor="#FFD29E"
	ElseIf HighLightStatus = "HighLight_No" OR HighLightStatus = "" Then
		strMouseOverRowColor=""
	End If

	strRS=strRS & "<input type=""hidden"" name=""strBgColor"" id=""strBgColor"">"

	intColspan=oRs.Fields.Count-1-intHidden

	if IsArray(arySubmit) Then intColspan=intColspan+UBound(arySubmit)+1
	if Len(NName)>0 Then intColspan=intColspan+1

	j=0
	i=0
	k=0
	intRowCountDown=intPageSize

	intPageNo=Trim(Request("pageno"))
	If (Len(intPageNo)=0 or Not IsNumeric(intPageNo))Then intPageNo=1
	strAction=strScriptName & "?pageno="&intPageNO&"&"&strQS

	'-- 包含Form
	'If IsArray(arySubmit) Then strRS=strRs & "<Form name=""frmGrid"" action=""" & strAction & """ method=""post"">" & vbCrLf

	strRs=strRs & "<Div class=""gridiv""><Table class=""gridtable"" border=""0"" cellspacing=""0"" cellpadding=""3"" width=""" & strTbWidth & """ >" & vbCRLf
	strRs=strRs & "<caption class=""gridcaption"">"

	If IsArray(aryIns) Then

		strRs=strRs & "<table border=""0"" width=""100%"" cellspacing=""0"" cellpadding=""0""><tr><td class=""gridtdcaption"">"&strCaption&"<td align=""right"">"
		For i=LBound(aryIns) to Ubound(aryIns)
			strRs=strRs & "<input id=""btnins" & i & """ class=""gridins"" type=""button"" value="""&aryIns(i) & """ onclick="""&aryIns(i+1)&"""/>&nbsp;"
			i=i+1
		Next
		strRs=strRs & "</tr></table></caption>"
	Else

		strRs=strRs & strCaption

	End If
	strRS=strRs & "</caption>"
	'==================================
	'產生標題
	strHd=strHd & "<Tr class=""gridtr"">"
	If Len(NName)>0 Then strHd=strHd & "<Th class=""gridth"">"&NName&"&nbsp;</th>" & vbCrLf

	'---自動從RecordSet取得欄位標題
	For i=0 to oRs.Fields.Count-1-intHidden

		Select Case i
		Case intPK
		Case Else
			If IsArray(aryThWidth) Then strThWidth=aryThWidth(j)
			strHd=strHd & "<Th class=""gridth"" width=""" & strThWidth & """>" & oRs(i).Name & "</Th>" & vbCrLf
			j=j+1
		End Select

	Next

	If IsArray(arySubmit) Then
		For i=LBound(arySubmit) to UBound(arySubmit)
			strHd=strHd & "<Th class=""gridth"">"
			strHd=strHd & "<input id=""cmd"" name=""cmd"" type=""button"" value="""&arySubmit(i)&""" />"
		Next
		strFt=strHd & "</tr>"
	End If

	strHd=strHd & "</Tr>" & vbCrLf
	strRs=strRs & strHd
	'==================================


	strPager=RsToPagerAJAX(oRs,intPageSize,strQS,strImagePath,strTbWidth)

	If Len(NName)>1 Then
		If intPageNo>1 Then
			k=(intPageNo-1)*intPageSize
		Else
			k=0
		End If
	End If

	Dim q
	q=k
	'==================================
	'產生內容
	Dim strValue		'欄位值
	Dim strTrClass		'TRClass
	strTrClass="bgcolor=#E6E6FA"

	if not strMouseOverRowColor="" then
		'strMouseOver="strBgColor.value=this.bgColor;this.bgColor='"& strMouseOverRowColor &"';"
		'strMouseOut="this.bgColor=strBgColor.value"

    	strMouseOver=""
		strMouseOut=""
	end if

	'-- 計算分頁--------------------
	While Not oRs.Eof and intRowCountDown > 0

		If strTrClass="bgcolor=#E6E6FA" Then
			strTrClass="bgcolor=white"
		Else
			strTrClass="bgcolor=#E6E6FA"
		End If


		strRS=strRS & "<Tr height=""30px"" "&strTrClass&" onmouseover="""& strMouseOver &""" onmouseout="""& strMouseOut &""">" & vbCrLf


		If Len(NName)>0 Then
			q=q+1
			strRs=strRs & "<td class=""gridtd"" align=""center"">" & q & "</td>" & vbCrLf
		End If

		For i=0 to oRs.Fields.Count-1-intHidden
			strValue=oRs(i).Value
			If Not IsNull(strValue) Then strValue=Trim(Cstr(strValue))
			Select Case i
			Case intPK
			Case intLink

				'-- FIX BY JACK AT 20040708----------------
				IF strHrefB <>"" THEN
					Dim strHrefB_Ary
					Dim strHrefB_Tmp
					Dim N
					strHrefB_Ary=SPLIT(strHrefB,"|")
					strHrefB_Tmp=""
					FOR N = 0 To UBOUND(strHrefB_Ary)
						'-- 因為本陣列必須要兩個兩個一組(第一個是要LINK名稱,第二個是要LINK的欄位
						'-- 將陣列子加一後 MOD 2 取得1 或 0
						IF (N + 1) MOD 2 = 1 THEN
							strHrefB_Tmp=strHrefB_Tmp & "&" & strHrefB_Ary(N) & "="
						ELSE
							strHrefB_Tmp=strHrefB_Tmp & oRs(Cint(strHrefB_Ary(N)))
						End If
					NEXT
					'Response.Write strHrefB_Tmp
					'Response.End
					
					'Jerry 可用Function	2011/12/13 下午 05:15:19
					If Left(strHrefA,8)="function" Then
						strLink= "javascript:" & Right(strHrefA,Len(strHrefA)-9) & "("  & oRs(intPK) & ")"
					Else
	  					strLink=strHrefA & oRs(intPK) & strHrefB_Tmp
	  				End If
				ELSE
					'Jerry 可用Function	2011/12/13 下午 05:15:19
					If Left(strHrefA,8)="function" Then
						strLink= "javascript:" & Right(strHrefA,Len(strHrefA)-9) & "("  & oRs(intPK) & ")"
					Else
	  					strLink=strHrefA & oRs(intPK) & strHrefB_Tmp
	  				End If
				End IF
				'===========================================
				'-- FIX BY JACK AT 20040921-----------------
				'-- 超連結時,是否要另外開視窗
				If OpenWindowStatus = "OpenWindow_Yes" Then
					strRS=strRS & "<Td class=""gridtd""><a href=#  onclick=""window.open('" & strLink & "',null,'width=800px,height=600px,location=no,menubar=no,status=no,resizable=yes,scrollbars=yes')"" class=grida>" & strValue & "</a>&nbsp;</Td>" & vbCrLf
				ElseIf OpenWindowStatus = "OpenWindow_No" OR OpenWindowStatus= "" Then
					strRS=strRS & "<Td class=""gridtd""><a class=grida href=""" & strLink & """  >" & strValue & "</a>&nbsp;</Td>" & vbCrLf
				End If
				'RESPONSE.WRITE strLink
				'RESPONSE.END
				'===========================================

			Case Else
				strRS=strRS & "<Td class=""gridtd""><div class=""gridtd" &  i & """>" & strValue & "&nbsp;</div></Td>" & vbCrLf
			End Select
		Next



		If IsArray(arySubmit)  Then
			Dim M
			M=UBound(arySubmit)+1
			For i=Lbound(arySubmit) to UBound(arySubmit)
				k=oRs.Fields.Count-M + i
				If intHidden = 0 And IsNumeric(oRs(k)) Then
					If oRs(k)=0 Then
						strRS=strRs & "<Td align=""center"" valign=""top"" class=""gridtdsubmit"">&nbsp;</Td>" & vbCrLf
					Else
						strRS=strRs & "<Td align=""center"" valign=""top"" class=""gridtdsubmit""><Input class=""gridcheck"" id=""did" & oRs(intPK) & """ name=""did" & i & """ type=""checkbox"" value=""" & oRs(intPK) & """></Td>" & vbCrLf
					End if
				Else
					strRS=strRs & "<Td align=""center"" valign=""top"" class=""gridtdsubmit""><Input class=""gridcheck"" id=""did" & oRs(intPK) & """ name=""did" & i & """ type=""checkbox"" value=""" & oRs(intPK) & """></Td>" & vbCrLf
				End If

			Next
		End If

		strRs=strRs & "</Tr>" & vbCrLf
		oRs.MoveNext
		intRowCountDown=intRowCountDown-1					'*****計算分頁*****
	Wend

	'==================================

	If intRowCountDown>0 Then

		For k=0 to (intRowcountDown-1)
			strRS=strRs & "<Tr class=""gridtrspace""><Td class=""gridspace"" colspan="& intColspan & ">&nbsp;</Tr>"
		Next
	End If


	strRS=strRS & "</Table></div>"


	'If IsArray(arySubmit) Then strRs=strRs & "</Form>" & vbCrLf

	If PFlag=1 Then strRs=strRS & strPager


	RsToTableAJAX = strRs

End Function
%>