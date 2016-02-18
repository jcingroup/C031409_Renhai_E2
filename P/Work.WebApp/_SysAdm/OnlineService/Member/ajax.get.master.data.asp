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
'===變數初始化
	message = ""
'===程新開始
	
	id  = Request.Form("MasterID")
	
	sql = "Select * From 會員資料表 Where Is戶長=1 and 戶長SN=" & id 
	
	Set rs =ExecSQL_RTN_RST(sql,3,0,1)
	
	'If rs.Eof Then LogicErrCheck "無法取得ID:" & sql
	Set ReturnSYSJson = MakeJson()

	If Not rs.Eof Then
		ReturnSYSJson("IsData") = true
		ReturnSYSJson("序號")	= rs.fields("序號").value
		ReturnSYSJson("會員編號")	= rs.fields("會員編號").value
		ReturnSYSJson("戶長SN")	= rs.fields("戶長SN").value	
		ReturnSYSJson("電話尾碼")	= rs.fields("電話尾碼").value	
		ReturnSYSJson("郵遞區號")	= rs.fields("郵遞區號").value	
		ReturnSYSJson("地址")	= rs.fields("地址").value
		ReturnSYSJson("縣市")	= rs.fields("縣市").value
		ReturnSYSJson("鄉鎮")	= rs.fields("鄉鎮").value
	
	Else
		ReturnSYSJson("IsData") = false
	End If
	
	ReturnSYSJson("result")	= true
	ReturnSYSJson("message")= message
	ReturnSYSJson.Flush
%>