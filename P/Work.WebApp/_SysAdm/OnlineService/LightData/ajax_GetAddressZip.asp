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
	
	id  = Request.Form("id")
	ids = Request.Form("ids")
	
	sql = "Select * From 地址鄉鎮 Where 縣市='" & id & "' and 鄉鎮='" & ids & "'"
	
	Set rs =ExecSQL_RTN_RST(sql,3,0,1)
	
	If rs.Eof Then LogicErrCheck "無法取得ID:" & sql
	
	'OptionHtml = ""
	
	Set ReturnSYSJson = MakeJson()
	
	ReturnSYSJson("result")	= true
	ReturnSYSJson("message")= message
	ReturnSYSJson("Zip")	= Trim(rs.Fields("郵遞區號").Value)
	ReturnSYSJson.Flush
%>