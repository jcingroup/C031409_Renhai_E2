<%@codepage="65001"%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/File_Function.asp"-->
<!--#include file="../../_Function/JSON_2.0.4.asp"-->
<%
On Error Resume Next

'===變數定訂===
Dim MasterID '戶帳序號
Dim p0,p1,p2,p3,p4,p5,p6,p7,p8,p9,p10,p11,p12,p13,p14,p15,p16
Dim p2Birthday,p2Array '資料庫存 農曆西元
DIm rs_Animal

Dim cnn,rs,sql,rs_OrderDetail,rs_Member,MemberID
Dim message
Dim YearAnimal '生肖
'===變數初始化
	message = ""
'===程新開始	
	If Trim(Request.Form("cmd"))="set" Then 	'新增

		MasterID = Request.Form("MasterID")
		p0=Trim(Request.Form("p0"))		' 
		'p1=Trim(Request.Form("p1"))		' 是否為戶長
		p2=Trim(Request.Form("p2"))		' 生日
		p3=Trim(Request.Form("p3"))		' 性別
		
		p2Array		= Split(p2,"/")
		p2Birthday 	= ( p2Array(0)+1911 ) & "/" & p2Array(1) & "/" & p2Array(2)
		ErrCheckTran "p2Array Err" & p2,cnn		
			
		Set cnn = GetTranConnection()
		ErrCheckTran "CNN Err" & sql,cnn
		'If p1="" Then LogicErrCheckTran "姓名不能為空值",cnn
		'生肖
		sql = "select * From 年度生肖表 Where 民國年=" & p2Array(0)
		Set rs_Animal = GetTranRS(cnn,sql)
		Dim animal
		animal = rs_Animal("生肖")

		sql= "Select * From 訂單明細檔 Where 訂單序號=" & p0
		Set rs_OrderDetail = GetTranRS(cnn,sql)

		rs_OrderDetail("申請人生日")	=	p2
		rs_OrderDetail("申請人性別")	=	p3
		rs_OrderDetail("申請人生肖")	=	animal
							
		rs_OrderDetail.Update
		ErrCheckTran "rs_OrderDetail Err" & sql,cnn
		
		MemberID = rs_OrderDetail("會員編號")
		
		sql= "Select * From 會員資料表 Where 序號=" & MemberID
		Set rs_Member = GetTranRS(cnn,sql)
		ErrCheckTran "rs_Member Select" & sql,cnn
		
		rs_Member("性別") = p3
		rs_Member("生日") = p2Birthday
		rs_Member("生肖") = animal
		
		rs_Member.Update
		
		ErrCheckTran "rs_Member.Update Err:",cnn

		rs_OrderDetail.Close
		Set rs_OrderDetail = Nothing
		
		rs_Member.Close
		Set rs_Member = Nothing		
		
		cnn.CommitTrans
		cnn.Close
		cnn = Nothing
		ErrCheckTran "Last Check:",cnn
		
		Set ReturnSYSJson = MakeJson() 
		ReturnSYSJson("result") = true
		ReturnSYSJson("message") = message
		ReturnSYSJson("InsertID") = p0
		ReturnSYSJson.Flush
	End If
	
	Function CheckBoolean(src)
		If Trim(src) = "" Or Trim(src) = "0" Then
			CheckBoolean = false
		Else
			CheckBoolean = true
		End If
			
	End Function 
%>