<%@codepage="65001"%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/File_Function.asp"-->
<!--#include file="../../_Function/JSON_2.0.4.asp"-->
<%
On Error Resume Next

'===變數定訂===
Dim cnn,rs,sql
Dim message
Dim id,ids
Dim ReturnSYSJson

Dim p4Birthday,p4Array '資料庫存 農曆西元
'===變數初始化
	message = ""
'===程新開始
	
	id  = Request.Form("MemberID")
	
	sql = "Select * From 會員資料表 Where 序號=" & id
	
	Set rs =ExecSQL_RTN_RST(sql,3,0,1)
	
	If rs.Eof Then LogicErrCheck "無法取得ID:" & sql

	Set ReturnSYSJson = MakeJson()
	
	'序號, 會員編號, 戶長SN, Is戶長, 姓名, 電話區碼, 電話尾碼, 郵遞區號, 地址, 手機, 性別, 生日, 時辰, EMAIL, 祈福事項, 生肖, 縣市, 鄉鎮
	ReturnSYSJson("序號")	= rs.fields("序號").value
	ReturnSYSJson("會員編號")	= rs.fields("會員編號").value
	ReturnSYSJson("戶長SN")	= rs.fields("戶長SN").value	
	ReturnSYSJson("姓名")	= rs.fields("姓名").value	
	ReturnSYSJson("Is戶長")	= rs.fields("Is戶長").value
	ReturnSYSJson("電話尾碼")	= rs.fields("電話尾碼").value	
	ReturnSYSJson("郵遞區號")	= rs.fields("郵遞區號").value	
	ReturnSYSJson("地址")	= rs.fields("地址").value
	ReturnSYSJson("手機")	= rs.fields("手機").value	
	ReturnSYSJson("性別")	= rs.fields("性別").value	
	'ReturnSYSJson("生日")	= rs.fields("生日").value
	ReturnSYSJson("時辰")	= rs.fields("時辰").value	
	ReturnSYSJson("生肖")	= rs.fields("生肖").value	
	ReturnSYSJson("縣市")	= rs.fields("縣市").value
	ReturnSYSJson("鄉鎮")	= rs.fields("鄉鎮").value
	ReturnSYSJson("EMAIL")	= rs.fields("EMAIL").value	
	
	p4Birthday = NullValue(rs.fields("生日").value,"")
	
	If p4Birthday <> "" Then
		p4Array = Split(p4Birthday,"/")
		If UBound(p4Array) = 2 Then
			If IsNumeric( p4Array(0)) And IsNumeric( p4Array(1)) And IsNumeric( p4Array(2)) Then
				ErrCheck "p4Array" 
				ReturnSYSJson("生日") = (CInt(p4Array(0))-1911) & "/" & p4Array(1) & "/" & p4Array(2)
				ErrCheck "ReturnSYSJson(生日)"
			Else
				ReturnSYSJson("生日") = ""
			End If
		Else
			ReturnSYSJson("生日") = ""
		End If
		
	Else
		ReturnSYSJson("生日") = ""
	End If
	
	ReturnSYSJson("result")	= true
	ReturnSYSJson("message")= message
	ReturnSYSJson.Flush
%>