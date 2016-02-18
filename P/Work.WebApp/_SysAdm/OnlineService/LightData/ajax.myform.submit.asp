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
Dim p4Birthday,p4Array '資料庫存 農曆西元

Dim cnn,rs,sql,rs_Master,rs_YearAnimal
Dim message
Dim YearAnimal '生肖
'===變數初始化
	message = ""
'===程新開始	
	If Trim(Request.Form("cmd"))="set" Then 	'新增

		MasterID = Request.Form("MasterID")
		p0=Trim(Request.Form("p0"))		' 
		p1=Trim(Request.Form("p1"))		' 是否為戶長
		p2=Trim(Request.Form("p2"))		'
		p3=Trim(Request.Form("p3"))		'
		p4=Trim(Request.Form("p4"))		' 生日
		p5=Trim(Request.Form("p5"))		'
		p6=Trim(Request.Form("p6"))		'
		p7=Trim(Request.Form("p7"))		'
		p8=Trim(Request.Form("p8"))		'
		p9=Trim(Request.Form("p9"))		'
		p10=Trim(Request.Form("p10"))		'
		p11=Trim(Request.Form("p11"))		'

		p12=Trim(Request.Form("p12"))		'
		p13=Trim(Request.Form("p13"))		'
		p14=Trim(Request.Form("p14"))		'
		
		p16=Trim(Request.Form("p16"))		'生肖
		
		p4Array		= Split(p4,"/")
		p4Birthday 	= ( p4Array(0)+1911 ) & "/" & p4Array(1) & "/" & p4Array(2)
		
				
		Set cnn = GetTranConnection()


		
		'If p1="" Then LogicErrCheckTran "姓名不能為空值",cnn

		If p1="1" Then
			sql= "Update 會員資料表 Set Is戶長=0 Where 戶長SN=" & MasterID
			ExecTranSQL cnn,sql
			ErrCheckTran "sql Err" & sql,cnn
		End If

		If Trim(Request.Form("Uptmode"))="Insert" Then
			
			'先檢查是否已有戶長資料
			sql="Select * From 會員戶長資料 Where 戶長SN=" & MasterID
			Set rs_Master = GetTranRS(cnn,sql)
			If rs_Master.Eof Then
				rs_Master.AddNew
				rs_Master("戶長SN") = MasterID

				If p1 = "1" Then '如果是該會員為戶長
					rs_Master("姓名") = p2
					rs_Master("郵遞區號") = p6
					rs_Master("地址") = p13 & p14 & p7
					rs_Master("電話") = p9
				End If
				rs_Master.Update
				ErrCheckTran "rs_Master.UpdateD Err:",cnn
			Else
				If p1 = "1" Then '如果是該會員為戶長
					rs_Master("姓名") = p2
					rs_Master("郵遞區號") = p6
					rs_Master("地址") = p13 & p14 & p7
					rs_Master("電話") = p9
					
					rs_Master.Update
					ErrCheckTran "rs_Master.UpdateC Err:",cnn
				End If
			End If
			
			rs_Master.Close
			Set rs_Master = Nothing
		
			sql="Select Top 1 * From 會員資料表"
			Set rs = GetTranRS(cnn,sql)
			rs.AddNew
		Else

			'先檢查是否已有戶長資料
			sql="Select * From 會員戶長資料 Where 戶長SN=" & MasterID
			Set rs_Master = GetTranRS(cnn,sql)
			If rs_Master.Eof Then
				rs_Master.AddNew
				rs_Master("戶長SN") = MasterID

				If p1 = "1" Then '如果是該會員為戶長
					rs_Master("姓名") = p2
					rs_Master("郵遞區號") = p6
					rs_Master("地址") = p13 & p14 & p7
					rs_Master("電話") = p9
				End If
				rs_Master.Update
				ErrCheckTran "rs_Master.UpdateA Err:",cnn
			Else
				If p1 = "1" Then '如果是該會員為戶長
					rs_Master("姓名") = p2
					rs_Master("郵遞區號") = p6
					rs_Master("地址") = p13 & p14 & p7
					rs_Master("電話") = p9
					
					rs_Master.Update
					ErrCheckTran "rs_Master.UpdateB Err:",cnn
				End If
			End If
			
			rs_Master.Close
			Set rs_Master = Nothing
			

			sql="Select * From 會員資料表 WHERE 序號="	&	p0
			Set rs = GetTranRS(cnn,sql)
			message = "無法取得該筆資料!" & sql
			If rs.Eof Then	LogicErrCheckTran message,cnn
		End If


		
		

		rs("戶長SN")	=	MasterID
		rs("Is戶長")	=	CheckBoolean(p1)		
		rs("姓名")	=	p2
		rs("電話區碼")	=	p11
		rs("電話尾碼")	=	p9
		rs("郵遞區號")	=	p6
		rs("地址")	=	p7
		rs("手機")	=	p8
		rs("性別")	=	p3
		rs("生日")	=	p4Birthday
		rs("時辰")	=	p5
		rs("EMAIL")	=	p10
		rs("生肖")	=	p16			
		'
		rs("縣市")	=	p13
		rs("鄉鎮")	=	p14
							
		rs.Update
		ErrCheckTran "rs.Update Err:",cnn
		
		p0 = rs.Fields("序號").Value '取得 自動新增序號

		'
		rs.Close
		rs = Nothing
		
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