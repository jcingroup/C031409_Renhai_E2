<!--#include file="DB_Function.asp"-->
<%
'-- 權限函式庫---------------------------------------------
'==========================================================



'-- 檢查 是否有登入系統------------------------------------
If IsEmpty(Session("LID")) or Len(Session("LID"))= 0 Then
	CALL KickOut()

'-- 如果有登入,則檢查是否有權限使用此隻程式功能
Else		
	'If hasPermission = 0 Then GoAway	
	
End If'====================================================

'=======================
'勒令離開系統-顯示登入畫面
'=======================
Sub KickOut()
		Response.Write "<script language=javascript>top.location.href='../../index.asp'</script>"				
		Response.End
End Sub

'=======================
'顯示無該功能權限
'=======================
Sub GoAway()
		Response.Write "<script language=javascript>window.location.href='../../_error/nopermission.asp'</script>"
		Response.End
End Sub


'-- 執行網路流量計數器-
Call Knock()
'======================


Sub Knock()
	Dim strPath
	strPath=getScriptPath(2) 
	ExecSP_RTN_ARY "usp_sysKnock",Array(strPath)
End Sub





'=======================
'00是否可使用此隻程式功能
'=======================
Function hasPermission()
	Dim strPath
	strPath=getScriptPath(2)
	hasPermission=ExecSP_RTN_ARY("usp_sysHasPermission",Array(strPath,Session("ID")))(0)
'	RESPONSE.WRITE strPath & "<BR>"
'	RESPONSE.WRITE Session("ID") & "<BR>"
'	response.write hasPermission & "<BR>"
'	response.end 
End Function

'=======================
'01是否可瀏覽:
'並傳回最大權限值:
'	完全控制(1),管理(2),瀏覽(3)
'=======================
Function hasLstPower()
	Dim strPath
	strPath=getScriptPath(2)
	hasLstPower=ExecSP_RTN_ARY("usp_sysHasLstPower",Array(strPath,Session("ID")))(0)
End Function


'=======================
'02是否可新增:
'=======================
Function hasInsPower()
	Dim strPath
	strPath=getScriptPath(2)
	hasInsPower=ExecSP_RTN_ARY("usp_sysHasInsPower",Array(strPath,Session("ID")))(0)
	
End Function



'=======================
'03是否可修改:
'=======================
Function hasUptPower()
	Dim strPath
	strPath=getScriptPath(2)
	hasUptPower=ExecSP_RTN_ARY("usp_sysHasUptPower",Array(strPath,Session("ID")))(0)
End Function


'=======================
'04是否可刪除:
'=======================
Function hasDelPower()
	Dim strPath
	strPath=getScriptPath(2)
	hasDelPower=ExecSP_RTN_ARY("usp_sysHasDelPower",Array(strPath,Session("ID")))(0)
End Function


'=======================
'05是否可回覆:
'=======================
Function hasReplyPower()
	Dim strPath
	strPath=getScriptPath(2)
	hasReplyPower=ExecSP_RTN_ARY("usp_sysHasReplyPower",Array(strPath,Session("ID")))(0)
End Function

'=======================
'06是否可做上傳:
'=======================
Function hasUploadPower(strPath)
	hasUploadPower=ExecSP_RTN_ARY("usp_sysHasUptPower",Array(strPath,Session("ID")))(0)
End Function



%>