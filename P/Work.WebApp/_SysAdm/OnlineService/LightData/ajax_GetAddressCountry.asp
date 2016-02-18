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
Dim id
Dim ReturnSYSJson
'===變數初始化
	message = ""
'===程新開始
	
	id = Request("id")
	
	sql = "Select * From 地址鄉鎮 Where 縣市='" & id & "'"
	
	Set rs =ExecSQL_RTN_RST(sql,3,0,1)
	
	If rs.Eof Then LogicErrCheck "無法取得ID:" & sql
	
	'OptionHtml = ""
	
	Set ReturnSYSJson = MakeJson()
	Do Until rs.Eof
		ReturnSYSJson(Trim(rs.Fields("鄉鎮").Value))	= Trim(rs.Fields("鄉鎮").Value)
		rs.MoveNext
	Loop
	ReturnSYSJson.Flush
%>