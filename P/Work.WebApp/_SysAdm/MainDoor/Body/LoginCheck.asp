<% Option Explicit%>
<%%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../..//_Function/SEC_MD5.asp"-->
<%
on error resume next

Function IsSafeStr(str)
    Dim s_BadStr, n, i
    s_BadStr = "'   &<>?%,;:()`~!@#$^*{}[]|+-=" & Chr(34) & Chr(9) & Chr(32)
    n = Len(s_BadStr)
    IsSafeStr = True
    For i = 1 To n
        If Instr(str, Mid(s_BadStr, i, 1)) > 0 Then
            IsSafeStr = False
            Exit Function
        End If
    Next
End Function
%>
<%
Dim strScript			'程式名稱
Dim strCmd				'P:命令
Dim strLID				'P:帳號
Dim strPWD				'P:密碼
Dim strIP				'登入IP
Dim strMsg				'訊息
Dim str_img_vilidate

Dim oRs,strSql,strWhere


	strScript=Request.ServerVariables("SCRIPT_NAME")
	strIP=Request.ServerVariables("REMOTE_ADDR")
	
	strLID=Trim(Request.Form("lid"))
	strPWD=Trim(Request.Form("pwd"))
	strCmd=Trim(Request.Form("cmd"))
	'response.write strIP&"</BR>"
'	RESPONSE.WRITE strLID & "<BR>"
'	RESPONSE.WRITE strPWD & "<BR>"
'	RESPONSE.WRITE strCmd & "<BR>"
'	RESPONSE.END

	'檢查驗證碼---------------------------------------------------------------------
	'Add By jack 20110106
	str_img_vilidate = request("img_vildate")
	If (LCase(str_img_vilidate) <> LCase(session("img_code"))) then
		
		strMsg = "驗證碼輸入錯誤，請重新輸入！！"
		'正式上線時，以下兩行要運作
		'Response.redirect "Login.asp?ErrMsg="&strMsg
		'RESPONSE.END
	End If
	'--------------------------------------------------------------------------------
	


	If strCmd = "login" Then

	'//判斷 SQL Injection
	IF IsSafeStr(strLID) = TRUE AND IsSafeStr(strPWD) = TRUE THEN
		'-- 先判斷是否為管理者
		strSql = "Select A.帳號, A.人員代碼, A.姓名, B.單位代碼,B.單位名稱,A.ISAdmin, A.停權 " 
		strSql = strSql & " From 人員 A " 
		strSql = strSql & " Left Join 單位 B ON A.單位代碼 = B.單位代碼 " 
		strSql = strSql & " Where A.帳號='"& strLID & "' "
		strSql = strSql & " AND A.MD5='" & MD5(strPWD) & "'" 
		'strSql = strSql & " AND A.使用IP='"&strIP&"'"

		Set oRs = ExecSql_RTN_RST(strSql,3,0,1)

		AspErrCheck "SQL Execute Err:"

		If Not oRs.Eof Then
				If (oRs("停權") = "0") then
					Session("LID") = oRs("帳號")		'登入帳號
					Session("ID") = oRs("人員代碼")		'人員代碼
					Session("NAME") = oRs("姓名")		'姓名
					Session("UID") = oRs("單位代碼")	'單位代碼
					Session("UName") = oRs("單位名稱")	'單位名稱
					
					'-- 使用者種類("":一般訪客,0:一般訪客,1:一般會員,2:高級會員,,99:管理者,)
					If (Session("ID")=1000001 AND Session("UID")=1) or Session("ID")=2000027  or Session("ID")=2000028 or Session("ID")=2000003 or Session("ID")=2000056 or Session("ID")=2000029 Then
						Session("UserKind") = 99	
					Else
						Session("UserKind") = 1	
					End if			
					Session("ISAdmin") = oRs("ISAdmin")	' 是不是管理者，

					'-- 編輯器專用-------------------------
					Session("FCKFolderID")=Session("ID")
					'-- 虛擬目錄	
					Session("FCK_UploadFilePath") ="/RenHai/_FCK_UpLoadFile/" & Session("FCKFolderID") & "/"
					'-- 網站
					'Session("FCK_UploadFilePath") ="../../../../../_FCK_UpLoadFile/" & Session("FCKFolderID") & "/"
	
					'======================================
				
					Call ReleaseObj(Array(oRs))

					'//存入登入紀錄檔
					Dim aryReturn
					aryReturn=ExecSP_RTN_ARY("usp_sysLogin",Array(strLID,strPWD,strIP))
					Select Case aryReturn(0)
					Case 1	'登入成功
						Session("LogID")=aryReturn(5)		'登入紀錄檔流水號
					End Select

					Response.Redirect "MainFrame.asp"
					RESPONSE.END
				Else
					strMsg = "無系統使用權限!!"
				End If
		Else
			strMsg = "帳號或密碼錯誤!!"
		End If
	ELSE
		strMsg="帳號或密碼錯誤!!"
	END IF
	End If

	Response.Redirect "Login.asp?Lid="&strLid&"&ErrMsg="&strMsg
	RESPONSE.END

%>