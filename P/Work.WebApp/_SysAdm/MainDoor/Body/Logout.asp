<% Option Explicit%>
<!--#include file="../../_Function/DB_Function.asp"-->
<%
''	If Not IsEmpty(Session("LogID")) Then
''		ExecSP_RTN_ARY "usp_sysLogout",Array(Session("LogID"))
''	End If

	'-- 把Session 清空
	Session.Abandon
	
	'-- 按鈕登出系統
	If request("status")="yes" then

		
		Response.Redirect "../../index.asp"
		Response.end
		
	'-- 直接關閉視窗或重新整理
	Else
		'-- jack 暫時MARK (20101028)
		Response.Write("<script language='JavaScript'>")   
		Response.Write("window.opener=null;  ")   
		Response.Write("window.close();")   
		Response.Write("</script>") 	
	End if

%>