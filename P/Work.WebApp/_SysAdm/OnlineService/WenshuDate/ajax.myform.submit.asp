<%@codepage="65001"%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/File_Function.asp"-->
<!--#include file="../../_Function/JSON_2.0.4.asp"-->
<%
'On Error Resume Next

'===變數定訂===
Dim MasterID '戶帳序號
Dim p0,p1,p2,p3,p4,p5,p6

Dim cnn,rs,sql

'===變數初始化
	message = ""
'===程新開始	
	If Trim(Request.Form("cmd"))="set" Then 	'新增

		p0=Trim(Request.Form("p0"))		' 
		p1=Trim(Request.Form("p1"))		' 
		p2=Trim(Request.Form("p2"))		'
		p3=Trim(Request.Form("p3"))		'
		p4=Trim(Request.Form("p4"))		' 
		p5=Trim(Request.Form("p5"))		'
		'p6=Trim(Request.Form("p6"))		'
	
		Set cnn = GetTranConnection()

		'先檢查是否已有戶長資料
		sql = "Select * From 文疏梯次時間表 Where 序號=" & p0
		Set rs = GetTranRS(cnn,sql)
		'message = "無法取得該筆資料A!" & sql
		'If rs.Eof Then	LogicErrCheckTran message,cnn
		
		rs("時間")		= p2
		rs("農曆年")	= p3
		rs("農曆月")	= p4
		rs("農曆日")	= p5
					
		rs.Update
		rs.Close
		
		ErrCheckTran "rs.Update Err:",cnn

		sql = "Select * From 文疏梯次時間表 WHERE 序號=" & p0
		Set rs = GetTranRS(cnn,sql)
		'message = "無法取得該筆資料B!" & sql
		'If rs.Eof Then	LogicErrCheckTran message,cnn

		ErrCheckTran "re select data:",cnn
		
		'p0 = rs.Fields("序號").Value 
		p1 = rs.Fields("梯次").Value 	
		p2 = rs.Fields("時間").Value 
		p3 = rs.Fields("農曆年").Value 
		p4 = rs.Fields("農曆月").Value 
		p5 = rs.Fields("農曆日").Value 

		rs.Close
		Set rs = Nothing
		
		cnn.CommitTrans
		cnn.Close
		Set cnn = Nothing
		ErrCheckTran "Last Check:",cnn
	End If
	
	Set ReturnSYSJson = MakeJson() 
	ReturnSYSJson("result") = true
	ReturnSYSJson("message") = message
	ReturnSYSJson("InsertID") = p0
	ReturnSYSJson.Flush
%>