<% Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/Power_Function.asp"-->
<%

If hasDelPower=0 Then GoAway

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

	strPageNo=Trim(Request("pageno"))
	'strQS = request("strQS")
	s1=Trim(Request("s1"))	'�q��s��	
	s2=Trim(Request("s2"))	'�I�ڤ覡	
	s3=Trim(request("s3"))	'�q����B�̤p��
	s4=Trim(request("s4"))	'�q����B�̤j��
	s5=Trim(Request("s5"))	'���A		
	s6=Trim(Request("s6"))	'�Ȧ�b��
	s7=Trim(Request("s7"))	'�g��H
	strSel=Trim(Request("Sel"))	'���~�s��

	If Not IsNumeric(strSel) then intSel="" Else intSel=Cint(strSel)
	strQS="s1="&s1&"&s2="&s2&"&s3="&s3&"&s4="&s4&"&s5="&s5&"&s6="&s6&"&s7="&s7&"&sel="&intSel&""

'	'-- �R���ɮ�-------------------------------------------
'	For Each p in Request.Form("did0")
'		strPath=Server.MapPath("../_upload/"&getScriptPath(2)&"/"&p)
'		deletePath strPath,0
'	Next'==================================================

	'-- �R�����-------------------------------------------
	Did0=Trim(Request.Form("did0"))	
		
	
	If Len(Did0)>0 Then
		'��X�q��Ǹ�
		strSql1 = "select * from �q��D�� where �q��Ǹ� IN ("&Did0&") "
		Set oRS1=ExecSQL_RTN_RST(StrSql1,3,1,2)
		
		'��X�q��s��(bug:�ثe�R�h���ɥu�|��X�̫�@�����s��)
		strSql3 = "select * from �q������� where �q��s�� IN ("&oRS1("�q��s��")&") "
		Set oRS3=ExecSQL_RTN_RST(StrSql3,3,1,2)
		
		response.write strSql1 & "<BR>" & strSql3 & "<BR>" 
		'response.end
		
		
		if not isNull(oRS3("�I�O��m")) or oRS3("�I�O��m") = "" then
			response.redirect "list.asp" 
		Else
		'���N�I�O��m��ƪ��O������~�R�������q��
			strSql4 = "select * from �I�O��m��ƪ� where ��m�W�� IN ("&oRS3("�I�O��m")&") "
			Set oRs4=GetRST(StrSql4,3,1,2)
			oRs4("�Ŧ�") = "1"
			oRs4.Update
		End if
	
			
		strSql="Delete �q��D�� WHERE �q��Ǹ� IN ("&Did0&")"
		
		ExecSql strSql
		
		strSql2="Delete �q������� WHERE �q��s�� IN ("&oRs1("�q��s��")&")"
		ExecSql strSql2
			
	End If'================================================
	
	
	response.redirect "list.asp?"&strQS&"" 
%>