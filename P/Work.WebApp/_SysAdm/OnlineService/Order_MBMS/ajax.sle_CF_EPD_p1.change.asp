<%@codepage="65001"%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/File_Function.asp"-->
<!--#include file="../../_Function/JSON_2.0.4.asp"-->
<!--#include file="pCar.asp"-->
<%
On Error Resume Next

Dim MemberID
Dim rs,sql
Dim message,rturnHTML
Dim ReturnSYSJson,WhereStr,GetWhereString	
Dim BirthdayHandle,BirthdayArray

	message = ""
	MemberID = Trim(Request.Form("MemberID"))	'
	
	Set ReturnSYSJson = MakeJson()
	
	If MemberID = "" Then LogicErrCheck "系統錯誤:無法取得會員ID"

	sql = "Select * from 會員資料表 Where 序號=" & MemberID
	Set rs = ExecSQL_RTN_RST(sql,3,1,2)
	ErrCheck "SQL ERR " & sql
		
	If rs.eof Then LogicErrCheck "系統錯誤:無法取得會員資料，ID=" & P0

	BirthdayHandle = rs.Fields("生日").Value

	BirthdayArray = Split(BirthdayHandle,"/")

	If UBound(BirthdayArray)=2 Then
		If IsNumeric( BirthdayArray(0)) And IsNumeric( BirthdayArray(1)) And IsNumeric( BirthdayArray(2)) Then
			ReturnSYSJson("IsBirthday") = True
			ReturnSYSJson("年") = CInt(BirthdayArray(0)) - 1911
			ReturnSYSJson("月") = BirthdayArray(1)
			ReturnSYSJson("日") = BirthdayArray(2)
		Else
			ReturnSYSJson("IsBirthday") = False
		End If		
	Else
		ReturnSYSJson("IsBirthday") = False
	End If

	ReturnSYSJson("姓名") 		= rs.Fields("姓名").Value
	ReturnSYSJson("電話區碼")	= rs.Fields("電話區碼").Value			
	ReturnSYSJson("電話尾碼")	= rs.Fields("電話尾碼").Value
	
	'ReturnSYSJson("地址")		= rs.Fields("縣市").Value & rs.Fields("鄉鎮").Value & rs.Fields("地址").Value
	'ReturnSYSJson("郵遞區號") 	= rs.Fields("郵遞區號").Value

	If Trim(rs.Fields("縣市").Value) = "空白" Then
		ReturnSYSJson("地址")	= rs.Fields("地址").Value
		ReturnSYSJson("郵遞區號") 	= ""
	Else
		ReturnSYSJson("地址")	= rs.Fields("縣市").Value & rs.Fields("鄉鎮").Value & rs.Fields("地址").Value
		ReturnSYSJson("郵遞區號") 	= rs.Fields("郵遞區號").Value
	End If

	ReturnSYSJson("手機") 		= rs.Fields("手機").Value		
	ReturnSYSJson("性別") 		= rs.Fields("性別").Value
	ReturnSYSJson("生日") 		= rs.Fields("生日").Value
	ReturnSYSJson("時辰") 		= rs.Fields("時辰").Value
	ReturnSYSJson("EMAIL") 		= rs.Fields("EMAIL").Value
	ReturnSYSJson("生肖") 		= rs.Fields("生肖").Value

	ReturnSYSJson("result") = true
	ReturnSYSJson("message") = message
	ReturnSYSJson.Flush
%>