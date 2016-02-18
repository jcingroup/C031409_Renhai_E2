<%@codepage="65001"%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/File_Function.asp"-->
<!--#include file="../../_Function/JSON_2.0.4.asp"-->
<%
'ajax 取得生肖
On Error Resume Next

'===變數定訂===
Dim p0
Dim rs,sql
Dim message
Dim YearAnimal '生肖
'===變數初始化
	message = ""
'===程新開始	
		'計算生肖
		p0 = Request.Form("LurYear")
		If p0 = "" Then LogicErrCheck "無法取得年分值，生肖無法計算!!!"
		
		Set ReturnSYSJson = MakeJson()
		
		sql = "Select * From 年度生肖表 Where 民國年=" & p0
		Set rs = ExecSQL_RTN_RST(sql,3,0,1)
		If rs.Eof Then	
			ReturnSYSJson("IsData") = false
			'LogicErrCheck "無法取得生肖資料!!!" & sql
		Else
			YearAnimal = rs.Fields("生肖").Value
			rs.Close
			Set rs = Nothing
			ReturnSYSJson("IsData") = true
		End If
		
		ReturnSYSJson("生肖") = YearAnimal
		ReturnSYSJson("result") = true
		ReturnSYSJson("message") = message
		ReturnSYSJson.Flush

%>