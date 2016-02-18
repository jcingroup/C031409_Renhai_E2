<%On Error Resume Next%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/Power_Function.asp"-->
<!--#include file="../../_Function/JSON_2.0.4.asp"-->
<%
'If hasDelPower=0 Then GoAway

Dim oRs,oRs1,strRs,oRs3,oRs4
Dim strScript,strPageNo,strQS
Dim strCon,intCon
Dim strUID,intUID,strUnit
Dim strCID,intCID,strCat
Dim strYear,strMonth,strDay
Dim strKey,strSql,strSql1,strSql2,strSql3,strSql4

Dim Did0,Did1
Dim p
Dim strPath
Dim deletePath
Dim s1,s2,s3,s4,s5,s6,s7,strsel,intsel
Dim MasterID

	strPageNo = Trim(Request("pageno"))
	MasterID = Trim(Request("MasterID"))
	
	If MasterID = "" Then AspLogicErrCheck "無法取得戶長ID"

	strSel=Trim(Request("Sel"))	'產品編號

	'If Not IsNumeric(strSel) then intSel="" Else intSel=Cint(strSel)
	strQS= "MasterID=" & MasterID

'	'-- 刪除檔案-------------------------------------------
'	For Each p in Request.Form("did0")
'		strPath=Server.MapPath("../_upload/"&getScriptPath(2)&"/"&p)
'		deletePath strPath,0
'	Next'==================================================

	'-- 刪除資料-------------------------------------------
	Did0=Trim(Request.Form("did0"))	

    strSql = "Select 1 From 訂單明細檔 Where 會員編號 In (" & Did0 & ")"
    Set rs = ExecSQL_RTN_RST(strSql,3,0,1)
    'If Not rs.Eof Then
    '    LogicErrCheck "選取的會員中已有訂單資料無法刪除!"
    'End If

	If Len(Did0)>0 Then
		'戶長不可刪除
		
		'strSql="Delete 會員資料表 WHERE 序號 IN (" & Did0 & ") And Is戶長=0"
		'------2016/1/30 ajoe 將刪除改為假刪除-----------------'
		strSql="Update 會員資料表 Set is_delete=1 WHERE 序號 IN (" & Did0 & ") And Is戶長=0"
		'------------------------------------------------------'
		ExecSql strSql
		AspErrCheck "Delete Data Err " & strSql
			
	End If'================================================

	'response.redirect "ifrm_list_member.asp?" & strQS & "" 
	Set ReturnSYSJson = MakeJson() 
	
	ReturnSYSJson("result") = true
	ReturnSYSJson("message") = message
	ReturnSYSJson.Flush
%>