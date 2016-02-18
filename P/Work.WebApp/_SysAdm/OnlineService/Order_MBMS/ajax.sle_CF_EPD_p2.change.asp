<%@codepage="65001"%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/File_Function.asp"-->
<!--#include file="../../_Function/JSON_2.0.4.asp"-->
<!--#include file="pCar.asp"-->
<%
On Error Resume Next

Dim ProdcutID
Dim rs,sql
Dim message,rturnHTML
Dim ReturnSYSJson,WhereStr,GetWhereString	

	message = ""
	ProdcutID = Trim(Request.Form("ProdcutID"))	'
	
	Set ReturnSYSJson = MakeJson()
	
	If ProdcutID = "" Then LogicErrCheck "系統錯誤:無法取得會員ID"

	sql = "Select * from 產品資料表 Where 產品編號='" & ProdcutID & "'"
	Set rs = ExecSQL_RTN_RST(sql,3,1,2)
	ErrCheck "SQL ERR " & sql
		
	If rs.eof Then LogicErrCheck "系統錯誤:無法取得產品資料，ID=" & P0

	ReturnSYSJson("價格") 		= rs.Fields("價格").Value

	ReturnSYSJson("result") = true
	ReturnSYSJson("message") = message
	ReturnSYSJson.Flush
%>