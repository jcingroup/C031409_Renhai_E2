<%Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/Power_Function.asp"-->
<!--#include file="../../_Function/File_Function.asp"-->
<!--#include file="../../_Function/function.asp"-->
<%
Dim p0	'�y����
Dim p1	'����
Dim p2	'���D
Dim p3	'���e
Dim p4	'�Ƨ�
Dim p5	'��ܪ��AFlag
Dim p6	'���ʤ��
Dim p7  '�ק���
Dim p8
Dim p9
Dim p10
Dim p11
Dim p12
Dim p15
Dim p16
Dim p17
Dim p18
Dim p101'�ק�H��"
Dim s1,s2,s3,s4,s5,s6,s7,s8
Dim s55,memid,id,uptime,now_time

Dim strScript,strPageNo
Dim strQS,strSql,oRs,strRS
Dim strPrev,strNext,strNew
Dim strSql1,oRs1,strSql2,oRs2,strSql3,oRs3,strSql4,oRs4,strSql5,oRs5,strSql6,oRs6,strSql7,oRs7

Dim strCon,intCon
Dim strHot,intHot
Dim strSel,intSel,strSelCbo
'Dim strsDate,streDate
Dim strKey
Dim strMsg
Dim strReturn
Dim strWhere
Dim intSel1
	
	'���o���駹��ɶ� yyyy/mm/dd hh:mm:ss
	now_time= transTime(now())
	'------------------------------------
	'-- ���o�q��s��
	p0=Trim(Request("p0"))

	strScript=Request.ServerVariables("SCRIPT_NAME")
	strPageNo=Trim(Request("pageno"))
	s1=Trim(Request("s1"))	'�q��s��	
	s2=Trim(Request("s2"))	'�I�ڤ覡	
	s3=Trim(request("s3"))	'�q����B�̤p��
	s4=Trim(request("s4"))	'�q����B�̤j��
	s5=Trim(Request("s5"))	'���A	

	'strSel=Trim(Request("Sel"))
	'strKey=Trim(Request("key"))

	'If Not IsNumeric(strSel) then intSel=0 Else intSel=Cint(strSel)

	'strQS="sel="&intSel&"&sel1="&intSel1&"&con="&intCon&"&key="&strKey
	strQS="s1="&s1&"&s2="&s2&"&s3="&s3&"&s4="&s4&"&s5="&s5&""
	
	'-- �[�J�d�߱���---------------------------------------
	strWhere=""
	strWhere=MakeWhere(strWhere,"AND","�I�ڤ覡","=",s2,"","N","","")
	strWhere=MakeWhere(strWhere,"AND","�q�檬�A","=",s5,"","N","","")
	'-- ����r�d��---------------------------------------
	strWhere=MakeWhereKW(strWhere,"AND",Array("�q��s��"),"%LIKE%",s1,"","S","","")
	'-- ���B�϶��d��--------------------------------------
	strWhere=MakeWhereBeTween(strWhere,"AND","�`�B","",s3,s4,1)
	strWhere=MakeWhereEnd(strWhere)
	
	'-- �զX�d�ߦr��---------------------------------------
	StrSql="Select �q��Ǹ�, "
	StrSql=StrSql & " �q��s��,"
	StrSql=StrSql & " �`�B,"
	StrSql=StrSql & " �q��ɶ�, "
	StrSql=StrSql & " �q�檬�A�W�� as ���A,"
	StrSql=StrSql & " �I�ڤ覡�W�� as �I�ڤ覡 "
	
	StrSql=StrSql & " From �q��D�� "
	StrSql=StrSql & strwhere 
	StrSql=StrSql & " Order By �q��ɶ� desc, �q��Ǹ� desc "

	'-- ���o��ƿ�-----------------------------------------
	Set oRs=ExecSQL_RTN_RST(StrSql,3,1,2)
	If oRs.RecordCount>0 Then
		p0=Trim(Request("p0"))
		strNext=Trim(getNextF(oRs,0,p0,0))
		oRs.MoveFirst
		strPrev=Trim(getPreF(oRs,0,p0,0))
		If Not IsNull(strNext) Then
			strNext="<a href="""&strScript & "?"&strQS&"&p0="&strNext&"""><img border=0 src=../../_images/forward.gif></a>"
		End If
		If Not IsNull(strPrev) Then
			strPrev="<a href="""&strScript & "?"&strQS&"&p0="&strPrev&"""><img border=0 src=../../_images/back.gif></a>"
		End If
	End if
	'======================================================
	'-- �^�W�@��
	strReturn="<a href=""list.asp?"&strQS&"&pageno="&strPageNo&"""><img border=0 src=../../_images/return.gif></a>"
	'======================================================

	'-- �ק���-------------------------------------------
	If Trim(Request.Form("cmd"))="set" Then
		s5=Trim(Request.Form("s5"))		'�q�檬�A
		p8=Trim(Request.Form("p8"))		'���~�s��	
		
			StrSql="Select Top 1 * From �q��D�� WHERE �q��Ǹ�='"&p0&"'"
			Set oRs=GetRST(StrSql,3,1,2)
			oRs("�q�檬�A")=SaveDataCheck(s5,NULL)
			'Add by Taka 20110106 �s�W�q��̫�ק�ɶ�---------------
			'---------�ק�ɥu�O�����A�w�I��-------------------------
			'-�w�W�ǤΧ������̭׭ק�ɶ���쬰�Ůɤ~�s�W�ɶ�---------
			'--�p��l���A���w�I�ڡB�w�W�ǩΧ����h����s�̫�ק�ɶ�--
			
			'response.write oRs("�I�ڤ覡")
			'response.end
			
			If s5 = "1" then 
			oRs("�q�檬�A�W��") = "���B�z"
			oRs("�I�ڮɶ�") = oRs("�q��ɶ�")
			elseif s5 = "2" then
			oRs("�q�檬�A�W��") = "�w�I��"
				if oRs("�I�ڮɶ�") = oRs("�q��ɶ�") or isNull(oRs("�I�ڮɶ�")) and oRs("�q�檬�A") <> "3" and oRs("�q�檬�A") <> "4" then
					if oRs("�I�ڤ覡") <> "3" then
					oRs("�I�ڮɶ�") = now_time
					End if
				End if
			elseif s5 = "3" then
			oRs("�q�檬�A�W��") = "�w�W��"
				if oRs("�I�ڮɶ�") = oRs("�q��ɶ�") or isNull(oRs("�I�ڮɶ�")) and oRs("�q�檬�A") <> "2" and oRs("�q�檬�A") <> "4" then
					if oRs("�I�ڤ覡") <> "3" then
					oRs("�I�ڮɶ�") = now_time
					End if
				End if
			elseif s5 = "4" then
			oRs("�q�檬�A�W��") = "����"
				if oRs("�I�ڮɶ�") = oRs("�q��ɶ�") or isNull(oRs("�I�ڮɶ�")) and oRs("�q�檬�A") <> "2" and oRs("�q�檬�A") <> "3" then
					if oRs("�I�ڤ覡") <> "3" then
					oRs("�I�ڮɶ�") = now_time
					End if
				End if
			elseif s5 = "5" then
			oRs("�q�檬�A�W��") = "�h�q"
			oRs("�I�ڮɶ�") = oRs("�q��ɶ�")
			elseif s5 = "0" then
			oRs("�q�檬�A�W��") = "�L��"	
			oRs("�I�ڮɶ�") = oRs("�q��ɶ�")
			End if
			'----------------------Add End----------------------------
			
			'-- �p�G Session("AP_Power") <> 1 ,�~�n�g�J�ק�H��
			'If Session("AP_Power") <> 1 Then
			'	oRs("�ק�H��")=SaveDataCheck(Session("ID"),NULL)
			'End if
			
			oRs.Update

			StrSql4="Select Top 1 * From �q��D�� WHERE �q��Ǹ�='"&p0&"'"
			Set oRs4=GetRST(StrSql4,3,1,2)
			id = oRs4("�q��s��")
			
			'====================Add by Taka �I�O�e�� 20101227====================================
			StrSql3="Select Top 1 * From �q������� WHERE �q��s��='"&id&"'"
			Set oRs3=GetRST(StrSql3,3,1,2)
			'---Add by Taka 20110106-----��s�̫�ק�ɶ��ܭq�������----------
			uptime=oRs4("�I�ڮɶ�")
			oRs3("�I�ڮɶ�") = uptime
			oRs3.Update
			'------------------------------------------------------------------
			
			'==========�d�߿O�y�O�_�٦��Ŧ�==============
			strSql5 = "select Top 1 * from �I�O��m��ƪ� where �Ŧ� = '1' order by �Ǹ�"
			Set oRs5=GetRST(StrSql5,3,1,2)
			'============================================
			'�����t�O
			'If p6 = "���t�O" then
			If p8 = "5" then
			'���A���w�I��.�H�W�ǤΧ���
				if s5 = "2" or s5 = "3" or s5 = "4" then 
					'���p�I�O��m�O�Ū�
					if oRs3("�I�O��m") = "" or isNull(oRs3("�I�O��m")) then
						'���p�w�g�S��m����=============
						if isNull(oRs5("��m�W��")) then
						oRs3("�I�O��m") = ""
						'===============================
						Else 
						'���@�ӳ̫e������m�B�N��m���s=
						oRs3("�I�O��m") = oRs5("��m�W��")
						oRs5("�Ŧ�") = "0"
						oRs5.Update
						'=================================
						End if
					End if
				Else
				'���A�����H�W�T��
				'���X��m�B��s�I�O��m��ƪ�
				strSql6 = "select * from �I�O��m��ƪ� where ��m�W�� = '"&oRs3("�I�O��m")&"' "
				Set oRs6=GetRST(StrSql6,3,1,2)
				oRs6("�Ŧ�") = "1"
				oRs6.Update
				oRs3("�I�O��m") = ""
				End if
			End if	
			oRs3.Update		
			'==========================Add End================================================
			Response.Redirect "List.asp?"&strQs&"&pageno="&strPageNo&"&p0="&p0
	End If
	'======================================================

	StrSql=StrSql & " From �q��D�� " & strWhere & " ORDER BY �q��Ǹ� DESC "

	'-- ���o���-------------------------------------------
	StrSql="Select �q��s��,*"
	StrSql=StrSql & " From �q��D�� WHERE �q��Ǹ�='"&p0&"'"
	Set oRs=ExecSQL_RTN_RST(StrSql,3,1,2)
		s1=oRs("�q��s��")
		s2=oRs("�I�ڤ覡�W��")
		s5=oRs("�q�檬�A")
		s6=oRs("�I�ڮɶ�")
		s7=oRs("�q�檬�A")
		s8=ors("�I�ڤ覡")
		memid=oRs("�|���s��")
	'======================================================
	
	StrSql1="Select �q��s��,*"
	StrSql1=StrSql1 & " From �q������� WHERE �q��s��='"&s1&"'"
	Set oRs1=ExecSQL_RTN_RST(StrSql1,3,1,2)
		p1=oRs1("�ӽФH�m�W")
		p2=oRs1("�ӽФH�a�}")
		
		if oRs1("�ӽФH�ʧO") = "1" then
		p3 = "�k"
		Else 
		p3 = "�k"
		End if
		
		p4=oRs1("�ӽФH�ͤ�")
		p5=oRs1("��֨ƶ�")
		p6=oRs1("���~�W��")
		p7=oRs1("����")
		p8=oRs1("���~�s��")
		p15=oRs1("�l���ϸ�")
		p17=oRs1("�ӽФH�~��")
		p18=oRs1("�I�O��m")

	
	StrSql2="Select �Ǹ�,*"
	StrSql2=StrSql2 & " From �|����ƪ� WHERE �Ǹ�='"&memid&"'"
	Set oRs2=ExecSQL_RTN_RST(StrSql2,3,1,2)
	p9 = oRs2("�q�ܰϽX")
	p10 = oRs2("�q�ܧ��X")
	p10 = p9 & p10
	
	p11 = oRs2("���")
	p12 = oRs2("Email")
	
	
	'-- ���o�q�檬�A
	StrSql="SELECT �Ǹ�,���A�W�� FROM �q�檬�A�� Where �Ǹ� = '2' "
	Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	s5 = RsToOption(oRs,0,1,Cstr(s5),"","")
	'======================================================
	'-- ��������-------------------------------------------
	oRs.Close
	Set oRs=Nothing
	'======================================================
	'-- �]�w�ɮ׹Ϥ��W�Ǹ��|-------------------------------
	Dim strPath
	Dim strLink1,strFilePath1
	Dim strLink2,strFilePath2
	Dim strFileType,strCaption,intIframeHeight
	Dim strImgReSize

	strPath=getScriptPath(2)

	Select Case 2
	Case 0
		strLink1=""
	Case 1

		strFilePath1=""&p0&"/file1"				'-- �]�w���|(file1,file2,file3)
		strFileType="15"							'-- �]�w�˪O(1,13,15)
		strCaption="�������ʸ��"				'-- �]�w�W���ɮ׼��D
		intIframeHeight="280px"					'-- �]�w�W���ɮ�IFRAME��������
		strImgReSize="0,0,0,0,0,0"				'-- �]�w�Y�Ϥj�p(�D�Ϥ��̽г]�w��0)

		strLink1="../../_Function/File_Upload.asp?FileType="&strFileType&"&path=" & strPath & "&sub=" & strFilePath1&"&ImgReSize="&strImgReSize&"&Caption="&strCaption
		strLink1="<iframe frameborder=0 scrolling=no width=""100%"" height="""&intIframeHeight&""" src="""& strLink1 & """></iframe>"
		strLink1=strLink1 & "<font color=blue>"
		strLink1=strLink1 & "�`�N�ƶ��G<BR>"
		strLink1=strLink1 & "�@�@1.�ɮפj�p1MB���W��.<BR>"
		strLink1=strLink1 & "�@�@2.�ɮ׮榡 *.JPG,*.GIF,*.BMP.<BR>"
		strLink1=strLink1 & "�@�@�@�@�@�@�@ *.DOC, *.XLS ,*.PPT,*.PDF .<BR>"
		strLink1=strLink1 & "�@�@3.�ɮ׼�5��<BR>"
		strLink1=strLink1 & "</font>"

	Case 2

		strFilePath1=""&p0&"/photo1"			'-- �]�w���|(photo1,photo2,photo3)
		strFileType="2"							'-- �]�w�˪O(2,23,25)
		strCaption="�W�Ǭ�ֹ��"				'-- �]�w�W���ɮ׼��D
		intIframeHeight="250px"					'-- �]�w�W���ɮ�IFRAME��������
		strImgReSize="100,175,0,0,0,0"		'-- �]�w�Y�Ϥj�p(W1,H1,W2,H2,W3,H3)(���T��)
''
		strLink1="../../_Function/File_Upload.asp?FileType="&strFileType&"&path=" & strPath & "&sub=" & strFilePath1&"&ImgReSize="&strImgReSize&"&Caption="&strCaption
		strLink1="<iframe frameborder=0 scrolling=no width=""100%"" height="""&intIframeHeight&""" src="""& strLink1 & """></iframe>"
		strLink1=strLink1 & "<font color=blue>"
		strLink1=strLink1 & "�`�N�ƶ��G<BR>"
		strLink1=strLink1 & "�@�@1.�ɮפj�p1MB���W��.<BR>"
		strLink1=strLink1 & "�@�@2.�ɮ׮榡JPG,GIF,BMP<BR>"
		strLink1=strLink1 & "�@�@3.�ɮ׸ѪR�� 1024*768 ���W�� .<BR>"
		strLink1=strLink1 & "</font>"
''	Case 4
''		strFilePath1=""&p0&"/Moive1"
''		strLink1="../../_Function/File_Upload.asp?FileType=1&path=" & strPath & "&sub=" & strFilePath1 & "&Caption=�v���ɤW��"
''		strLink1="<iframe frameborder=0 scrolling=no width=100% height=250px src="""& strLink1 & """></iframe>"
''		strLink1=strLink1 & "<font color=blue>"
''		strLink1=strLink1 & "�`�N�ƶ��G<BR>"
''		strLink1=strLink1 & "�@�@1.�v���j�p3MB���W��.<BR>"
''		strLink1=strLink1 & "�@�@2.�v���榡 *.WMV , *.MPG .<BR>"
''		strLink1=strLink1 & "�@�@3.�ѪR�� 320*240 ���W�� .<BR>"
''		strLink1=strLink1 & "</font>"
	End Select
''	'======================================================

''	'-- �]�w�ɮ׹Ϥ���ܸ��|-------------------------------
''	Dim strFile1
''	'Dim strFile2
''	strFilePath1="../../_upload/"&strPath&"/"&strFilePath1&"/"
''	'strFilePath2="../../_upload/"&strPath&"/"&strFilePath2&"/"
''	strFile1=FileToList(Server.MapPath(strFilePath1),strFilePath1)
''	'strFile2=FileToPhoto(Server.MapPath(strFilePath2),strFilePath2,"200")
''	'RESPONSE.WRITE strFile1
''	If Len(strFile1)>0 Then strFile1="<div class=button>�����ɮ�</div>"&strFile1
''	'If Len(strFile2)>0 Then strFile2="<div class=button>�����Ϥ��@</div>"&strFile2
''	'======================================================


%>

<html>
<head>
<title>�q����Ӻ޲z</title>
<meta http-equiv=Content-Type content=text/html; charset="big5">
<link rel=stylesheet href=../../_Css/Set.css>
<script language=javascript src=../../_JScript/subwin.js></script>
<script language=javascript src=set.js></script>

	<script Language=javascript>
	//���}����-------------
	function WO(URL){
		window.open(URL,"WO","Left=0,Top=0,width=1180,height=750,center=yes,status=no,toolbar=no,scrollbars=yes");
	}//================================
	</script>
<style>
	.GridTable{
		margin-top:10px;
		border:5 double #778899;
		}
	TH{
		background:#F5F5DC;
		}
</style>
</head>
<body >
<form  Name=myform action="<%=strScript%>?<%=strQS&"&pageno="&strPageNo%>" id="form1"  method="post" onSubmit="return check(this)">
<Input type=hidden name=cmd value=set>
<Input type=hidden name="p0"  value="<%=p0%>">
<table class=gridtable width=99% Height=450 border=1 cellspacing="0" cellpadding="0">
	<caption class=gridcaption style:margin:0>
		<table width=100%>
			<tr>
				<td class=gridtdcaption>�q��޲z:�ק�
				<td align=right>
			</tr>
		</table>
	</caption>
	<tr>
	<td  valign=top>
		<table width=100% border=0 cellspacing="1" cellpadding="1">
			<tr>
				<th width="160" align="center">�q�檬�A�G</th>
				<td width="220" ><select name="s5" ><%=s5%></select></td>
				<th width="160" align="center">�q��ɶ��G</th>
				<td width="220" ><input type=text name="s6" size=24 value="<%=s6%>" readonly></td>
			</tr>
			<tr>
				<th width="160" align="center">�q��s���G</th>
				<td width="220" ><input type=text name="s1" size=20 value="<%=s1%>"  style="width:100%" readonly></td>
				<th width="160" align="center">�I�ڤ覡�G</th>
				<td width="220" ><input type=text name="s2" size=10 value="<%=s2%>"  style="width:100%" readonly></td>
			</tr>
			<tr>
				<th width="160" align="center">��֩m�W�G</th>
				<td width="220" ><input type=text name="p1" size=10 value="<%=p1%>"  style="width:100%" readonly></td>
				<th width="160" align="center">��֦a�}�G</th>
				<td width="220" ><input type=text name="p2" size=10 value="<%=p2%>"  style="width:100%" readonly></td>
			</tr>
			<tr>
				<th width="160" align="center">��֩ʧO�G</th>
				<td width="220" ><input type=text name="p1" size=10 value="<%=p3%>"  style="width:100%" readonly></td>
				<th width="160" align="center">����ͤ�G</th>
				<td width="220" ><input type=text name="p2" size=10 value="<%=p4%>"  style="width:100%" readonly></td>
			</tr>
			<tr>
				<th width="160" align="center">�q�ܰϽX�G</th>
				<td width="220" ><input type=text name="p10" size=10 value="<%=p10%>"  style="width:100%" readonly></td>
				<th width="160" align="center">�l���ϸ��G</th>
				<td width="220" ><input type=text name="p15" size=10 value="<%=p15%>"  style="width:100%" readonly></td>
			</tr>
			<tr>
				<th width="160" align="center">��ʹq�ܡG</th>
				<td width="220" ><input type=text name="p11" size=10 value="<%=p11%>"  style="width:100%" readonly></td>
				<th width="160" align="center">E-mail�G</th>
				<td width="220" ><input type=text name="p12" size=40 value="<%=p12%>"  style="width:100%" readonly></td>
			</tr>
			<tr>
				<th width="160" align="center">�~�֡G</th>
				<td width="220" ><input type=text name="p17" size=10 value="<%=p17%>"  style="width:100%" readonly></td>
				<th width="160" align="center">�I�O��m�G</th>
				<td width="220" ><input type=text name="p18" size=10 value="<%=p18%>"  style="width:100%" readonly></td>
			</tr>
			<tr>
				<th valign=top align="center">��֨ƶ��G</th>
				<td Colspan=3 valign=top>
					<textarea  style="width:100%" rows="5" name="p5" maxlength="400" readonly><%=p5%></textarea>
				</td>
			</tr>
			<tr>
				<th width="160" align="center">��ֺ����G</th>
				<td width="220" ><input type=text name="p6" size=10 value="<%=p6%>"  style="width:100%" readonly></td>
				<th width="160" align="center">���B�G</th>
				<td width="220" ><input type=text name="p7" size=10 value="<%=p7%>"  style="width:100%" readonly></td>
			</tr>
			<tr>
				<td class=gridtdtool colspan=4 align=Center><span class=errmsg><%=strMsg%></span><br>			
					<% 'IF ((Cstr(Session("ID")) = Cstr(p101)) OR (Session("AP_Power")=1)) Then 
					%>
					<Input type=hidden name="p8" value=<%=p8%>>
					<% If s7 > 1 and s7 < 5 Then %>
						<input name="button" type="button" class=gridsubmit OnClick="WO('Print.asp?RptName=1&OrderNumber=<%=s1%>&user=<%=session("name")%>&pay=<%=s8%>')" value="�C�L�P�ª�">	
					<%End If%>
					
					<% If request("flg") = "1" then%>
					<input class=gridsubmit onclick=location.href("ins.asp") type="button" value="�s�W�q��" Style="Cursor:Hand">	
					<% End if%>
					<!--<input class=gridsubmit type="submit" value="�T�{�ק�" Style="Cursor:Hand">	-->
				</td>
			</tr>
		</table>
	</td>
	<%'-- �W���ɮ�-----------%>

	<td width=305 valign=top>
		<%=strLink1%>
		<BR>
		<%=strLink2%>
	</td>


	</tr>
</table>
</form>
</body>
</html>
<%
'-- ������-----------------------------
Function RR(AAStr)
	If IsNull(AAStr)OR AAStr="" Then
		RR="�@"
	Else
		RR=REPLACE(AAStr,vbcrlf,"<BR>")
	End If
End Function'============================


%>