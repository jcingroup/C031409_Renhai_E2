<%Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/Power_Function.asp"-->
<!--#include file="../../_Function/function.asp"-->
<%
Dim p0	'�q��s��
Dim p1	'�m�W
Dim p2	'�ͤ�
Dim p3	'Email
Dim p4	'�q�ܰϽX
Dim p5	'�q�ܧ��X
Dim p6	'���
Dim p7	'�a�}
Dim p8	'��֨ƶ�
Dim p9	'�ʧO
Dim p10	'�q��s��
Dim p11	'���~�s��
Dim p12	'���~�W��
Dim p13	'���~����
Dim p14	'���o���ت�
Dim p15	'�l���ϸ�
Dim p16	'��A��
Dim p17	'�ɨ�
Dim id	'�|���s�� �����Ѽ�
Dim order	'�q��s�� �����Ѽ�
Dim n	'�ɶ�
Dim light '�e���O�ذѼ�
Dim now_time	'�{�b�ɶ�
Dim s1,s2,s3,s4,s5,s6
Dim strScript,strQS,strPageNo,strReturn
Dim new_order_No,old_order_no_num,old_order_no 
Dim oRs,oRs1,oRs2,oRs3,oRs4,oRs5,oRs6,oRs7
Dim strSql,strSql1,strSql2,strSql3,strSql4,strSql5,StrSql6,StrSql7
Dim strMsg		'�P�_�O�_���L��g���
Dim strWhere
Dim sqlst
Dim mem,sn,payment,stat,Ydate
Dim ans
	
	'====================================================================
	'�s�y�̷s�q��s���@�Q�X
	new_order_No = right("00" & year(now()),2) & right("00" & month(now()),2) & right("00" & day(now()),2)
	do
		strsql2 = "select �q��s�� from �q��D�� where �q��s�� like '" & new_order_no & "%' order by �q��Ǹ� desc"
		set ors2 = ExecSQL_RTN_RST(strsql2,3,0,1)
		if not ors2.eof then
			old_order_no = ors2(0)
			old_order_no_num = right("0000"&cInt(right(ors2(0),4)) + 1,4)
			new_order_no = new_order_no & old_order_no_num
		else
			new_order_no = new_order_no & "0001"
		end if
		ors2.close

		strsql2 = "select * from �q��D�� where �q��s��='" & new_order_no & "'"
		set ors2 = ExecSQL_RTN_RST(strsql2,3,0,1)
	loop while not ors2.eof
	'------------------------------------------------------------------------
	p0 = new_order_no
	
	strQS="s1="&s1&"&s2="&s2&"&s3="&s3&"&s4="&s4&"&s5="&s5&""
	'-- �^�W�@��
	strReturn="<a href=""list.asp?"&strQS&"&pageno="&strPageNo&"""><img border=0 src=../../_images/return.gif></a>"
	'======================================================
	now_time= transTime(now())
	'response.write now_time & "<BR>"
	'response.write DateAdd(SS,1,now_time)
	
	Ydate=Year(now_time) '���̷s�~��
	
	'--�s�W�q����----------------------------------------
	If request("cmd")="set" then
	
	mem = "A" & right("0000000000" & int(10000*Rnd),10)		
		p1=request("p1")
		'=============�P�O���ιA��==========
		Dim y,m,d,yearold
		y=request("year")
		m=request("month")
		d=request("day")
		p16=request("calen")	
		'-------------------------------------
		if p16 = "1" then
		ans=request("answer")	'�����A��
		Yearold = Ydate-Year(ans)+1 '�p��~��
		else
		y = y+1911	'�A��~����褸�~��
		ans= y&"/"&m&"/"&d
		Yearold = Ydate-y+1	'�p��~��
		End if
		'=====================================
		
		p3=request("p3")
		p4=request("p4")
		p5=request("p5")
		p6=request("p6")
		p7=request("p7")
		p8=request("p8")
		p9=request("sex")
		s1=request("s1")
		s2=request("s2")
		s5=request("s5")
		'p11=request("p11")
		'p12=request("p12")
		'p13=request("p13")
		p14=request("p14")
		p15=request("p15")
		
		p17=request("time")
		
		'Ū�����~��ƪ��e==============================================
		strWhere=""
		strWhere=MakeWhere(strWhere,"AND","���~�s��","=",s1,"","N","","")
		strWhere=MakeWhereEnd(strWhere)
		'----------------------------------------------------------------
		StrSql1 = " Select * from ���~��ƪ� "
		StrSql1 = StrSql1 & strWhere
		Set oRs1=ExecSQL_RTN_RST(StrSql1,3,1,2)
		'================================================================
		p11 = oRs1("���~�s��")
		p12 = oRs1("���~�W��")
		p13 = oRs1("����")
		
		'�[�J�|����ƪ�================================
			StrSql="Select Top 1 * From �|����ƪ� "
			Set oRs=GetRST(StrSql,3,1,2)
			oRs.AddNew
			oRs("�|���s��")=SaveDataCheck(mem,NULL)
			oRs("�m�W")=SaveDataCheck(p1,NULL)
			oRs("�ͤ�")=ans
			oRs("�ʧO")=SaveDataCheck(p9,NULL)
			oRs("Email")=SaveDataCheck(p3,NULL)
			oRs("�q�ܰϽX")=SaveDataCheck(p4,NULL)
			oRs("�q�ܧ��X")=SaveDataCheck(p5,NULL)
			oRs("���")=SaveDataCheck(p6,NULL)
			oRs("�a�}")=SaveDataCheck(p7,NULL)
			oRs("��֨ƶ�")=SaveDataCheck(p8,NULL)
			oRs("�ɨ�")=SaveDataCheck(p17,NULL)
			oRs.Update
		'----Ū�X�|����ƪ�Ǹ�----------------------------------
		StrSql4="Select Top 1 * From �|����ƪ� order by �Ǹ� desc"
		Set oRs4=GetRST(StrSql4,3,1,2)
		id = oRs4("�Ǹ�")
		
		'==========�d�߿O�y�O�_�٦��Ŧ�==============
		StrSql7 = "select Top 1 * from �I�O��m��ƪ� where �Ŧ� = '1' order by �Ǹ�"
		Set oRs7=GetRST(StrSql7,3,1,2)
		'============================================
		'--------------------------------------------------------	
		'�g�J�q�������============================		
			StrSql2="Select Top 1 * From �q������� "
			Set oRs2=GetRST(StrSql2,3,1,2)
			oRs2.AddNew
			oRs2("�q��s��")=p0
			oRs2("���~�s��")=p11
			oRs2("���~�W��")=p12
			'---�P�O�������o���άO��L�A��
			if p11 = "7" then
			oRs2("����")=p14
			Else
			oRs2("����")=p13
			End if
			'------------------------------
			oRs2("�ƶq")=1
			oRs2("�ӽФH�m�W")=p1
			oRs2("�ӽФH�a�}")=p7
			oRs2("�ӽФH�ʧO")=p9
			oRs2("�ӽФH�ͤ�")=ans
			oRs2("�ʶR�ɶ�")= now_time
			oRs2("�I�ڮɶ�") = now_time
			oRs2("��֨ƶ�")=p8
			oRs2("�|���s��")=id		
			oRs2("�l���ϸ�")=p15
			oRs2("�ӽФH�~��") = yearold
			oRs2("�ӽФH�ɨ�") = p17
			oRs2("�g��H") = session("name")
			
			'�����t�O
			'If p6 = "���t�O" then
			If p11 = "5" then
			'���A���w�I��.�H�W�ǤΧ���
				if s5 = "2" or s5 = "3" or s5 = "4" then 
					'���p�I�O��m�O�Ū�
					if oRs2("�I�O��m") = "" or isNull(oRs2("�I�O��m")) then
						'���p�w�g�S��m����=============
						if isNull(oRs7("��m�W��")) then
						oRs2("�I�O��m") = ""
						'===============================
						Else 
						'���@�ӳ̫e������m�B�N��m���s=
						oRs2("�I�O��m") = oRs7("��m�W��")
						oRs7("�Ŧ�") = "0"
						oRs7.Update
						'=================================
						End if
					End if
				Else
				'���A�����H�W�T��
				'���X��m�B��s�I�O��m��ƪ�
				'strSql6 = "select * from �I�O��m��ƪ� where ��m�W�� = '"&oRs2("�I�O��m")&"' "
				'Set oRs6=GetRST(StrSql6,3,1,2)
				'oRs6("�Ŧ�") = "1"
				'oRs6.Update
				oRs2("�I�O��m") = ""
				End if
			End if	
			
			oRs2.Update				
			'StrSql4="Select Top 1 * From �q������� order by �q��Ǹ� desc "
			'Set oRs4=GetRST(StrSql4,3,1,2)					
		'============================================
		'���ͬd�ߧǸ�======================
		Randomize
		sn = right("00000000" & int(10000000*Rnd),8)
		'==================================
		'�g�J�q��D��================================
			strSql3="Select Top 1 * from �q��D�� "
			Set oRs3=GetRST(StrSql3,3,1,2)
			oRs3.AddNew
			oRs3("�q��s��")=p0
			oRs3("�|���s��")=id
			'=======�ݦA�W�[�|�����
			'oRs3("�ӽФH�m�W")=p1
			'=======================
			'---�P�O�������o���άO��L�A��
			if p11 = "7" then
			oRs3("�`�B")= p14 * 1
			Else
			oRs3("�`�B")= p13 * 1
			End if
			'------------------------------
			oRs3("�I�ڤ覡")=SaveDataCheck(s2,NULL)
			oRs3("�q��ɶ�")= now_time
			oRs3("�I�ڮɶ�") = now_time
			oRs3("�q�檬�A")=s5
			oRs3("�d�ߧǸ�")=sn
			'�P�O�I�ڤ覡�W��----------
			if s2 = "1" then
			payment = "�u�W��d"
			Elseif s2 = "2" then
			payment = "�״���b"
			Elseif s2 = "3" then
			payment = "�{�����"
			End if
			oRs3("�I�ڤ覡�W��")=payment
			'---------------------------
			'�P�O�q�檬�A�W��----------
			if s5 = "1" then
			stat = "���B�z"
			Elseif s5 = "2" then
			stat = "�w�I��"
			Elseif s5 = "3" then
			stat = "�w�W��"
			Elseif s5 = "4" then
			stat = "����"
			Elseif s5 = "5" then
			stat = "�h�f"
			Elseif s5 = "0" then
			stat = "�L��"
			End if
			oRs3("�q�檬�A�W��")=stat
			'---------------------------
			oRs3.Update
			
			StrSql5="Select Top 1 * From �q��D�� order by �q��Ǹ� desc "
			Set oRs5=GetRST(StrSql5,3,1,2)
			Response.Redirect "upt.asp?p0="&oRs5("�q��Ǹ�")&"&flg=1"
		'=============================================
		
	End if
			
	
	'-- ���o�q�檬�A
	StrSql="SELECT �Ǹ�,���A�W�� FROM �q�檬�A�� Where �Ǹ� = '2' order by �Ƨ� "
	Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	s5 = RsToOption(oRs,0,1,Cstr(s5),"","")
	'======================================================
	'-- ���o�I�ڤ覡
	StrSql="SELECT �Ǹ�,�����W�� FROM �q������� where �Ǹ�='3' order by �Ǹ� desc "
	Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	s2 = RsToOption(oRs,0,1,Cstr(s2),"","")
	'======================================================
	'-- ���o���~���
	StrSql="SELECT ���~�s��,���~�W�� FROM ���~��ƪ� where ���� = '0'"
	Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	s1 = RsToOption(oRs,0,1,Cstr(s1),"","")
	
	
	'-- ��������-------------------------------------------
	oRs.Close
	Set oRs=Nothing

%>

<html>
<head>
<title>�q��޲z</title>
<meta http-equiv=Content-Type content="text/html; charset=big5">
<link rel=stylesheet href=../../_Css/Set.css>
<script type="text/javascript" src="../../_JScript/List.js"></script>
<script type="text/javascript" src="lunar.js"></script>
<script type="text/JavaScript" src="common.js"></script>
<script type="text/javascript" src="check.js"></script>

<script>
function start()
	{
		document.all.lunar.style.visibility='hidden';//visible
	}
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
<body onload=start();>
<form  Name="v" action="<%=strScript%>" id="v" method="post" onSubmit="return(mainx(this))">
	<Input type=hidden name=cmd value=set>
<table class=gridtable width=99% Height=450 border=1 cellspacing="0" cellpadding="0">
	<caption class=gridcaption style:margin:0>
		<table width=100%>
			<tr>
				<td class=gridtdcaption>�q��޲z:�s�W
				<td align=right>
			</tr>
		</table>
	</caption>
	<tr>
	<td  valign=top>
		<table width=100% border=0 cellspacing="1" cellpadding="1">
			<tr>
				<th width="160" align="center">�q�檬�A�G</th>
				<td width="220" ><select name="s5"><%=s5%></select></td>
				<th width="160" align="center">��ֺ����G</th>
				<td width="220" ><select name="s1" ><option></option><%=s1%></select></td>
			</tr>
			<tr>
				<th width="160" align="center">�I�ڤ覡�G</th>
				<td width="220" ><select name="s2" ><%=s2%></select></td>
				<th width="160" align="center">���B�G</th>
				<td width="220" ><input type=text name="p14" size=10 value="300"  style="width:100%" ></td>
			</tr>
			<tr>
				<th width="160" align="center">��֩m�W�G</th>
				<td width="220" ><input type=text name="p1" size=10 value="<%=p1%>"  style="width:100%" ></td>
				<th width="160" align="center">���O�G</th>
				<td width="220" >��� <input type="radio" name="calen" value="1" checked = "true"> �A�� <input type="radio" name="calen" value="2"></td>
			</tr>
			<tr>
				<th width="160" align="center">��֩ʧO�G</th>
				<td width="220" >�k <input type="radio" name="sex" value="1" checked = "true"> �k <input type="radio" name="sex" value="2"></td></td>
				<th width="160" align="center">����-�~�G</th>
				<td width="220" ><input type=text name="year" size="4" maxlength="3" value=""></td>
			</tr>
			<tr>
			<th width="160" align="center">��֦a�}�G</th>
			<td width="220" ><input type=text name="p7" size=10 value="<%=p7%>"  style="width:100%" ></td>
			<th width="160" align="center">����-��G</th>
				<td width="220" >
				<select name="month" size="1">
				<option value="1"> 01</option>
				<option value="2"> 02</option>
				<option value="3"> 03</option>
				<option value="4"> 04</option>
				<option value="5"> 05</option>
				<option value="6"> 06</option>
				<option value="7"> 07</option>
				<option value="8"> 08</option>
				<option value="9"> 09</option>
				<option value="10"> 10</option>
				<option value="11"> 11</option>
				<option value="12"> 12</option>
				</select></td>
			</tr>
			<tr>
				<th width="160" align="center">�q�ܡG</th>
				<td width="220" ><input type=text name="p4" size=3 value="<%=p4%>" >�w<input type=text name="p5" size=8 value="<%=p5%>" ></td>
				<th width="160" align="center">����-��G</th>
			<td width="220" ><select name="day" size="1">
			<option value="1">01 </option>
			<option value="2">02 </option>
			<option value="3">03 </option>
			<option value="4">04 </option>
			<option value="5">05 </option>
			<option value="6">06 </option>
			<option value="7">07 </option>
			<option value="8">08 </option>
			<option value="9">09 </option>
			<option value="10">10 </option>
			<option value="11">11 </option>
			<option value="12">12 </option>
			<option value="13">13 </option>
			<option value="14">14 </option>
			<option value="15">15 </option>
			<option value="16">16 </option>
			<option value="17">17 </option>
			<option value="18">18 </option>
			<option value="19">19 </option>
			<option value="20">20 </option>
			<option value="21">21 </option>
			<option value="22">22 </option>
			<option value="23">23 </option>
			<option value="24">24 </option>
			<option value="25">25 </option>
			<option value="26">26 </option>
			<option value="27">27 </option>
			<option value="28">28 </option>
			<option value="29">29 </option>
			<option value="30">30 </option>
			<option value="31">31 </option>
			</select><!--<input type=text name="p2" size=10 value="<p2%>" id="date" onClick="ShowCalendar()" style="Cursor:Hand" style="width:100%" readonly>--></td>
			</tr>
			<tr>
				<th width="160" align="center">��ʹq�ܡG</th>
				<td width="220" ><input type=text name="p6" size=10 value="<%=p6%>"  style="width:100%" ></td>
				<th width="160" align="center">�X�ͮɨ��G</th>
				<td width="220" ><select name="time" size="1">
				<option value="�l"> 23:00~01:00 �l��</option>
				<option value="��"> 01:00~03:00 ����</option>
				<option value="�G"> 03:00~05:00 �G��</option>
				<option value="�f"> 05:00~07:00 �f��</option>
				<option value="��"> 07:00~09:00 ����</option>
				<option value="�x"> 09:00~11:00 �x��</option>
				<option value="��"> 11:00~13:00 �Ȯ�</option>
				<option value="��"> 13:00~15:00 ����</option>
				<option value="��"> 15:00~17:00 �Ӯ�</option>
				<option value="��"> 17:00~19:00 ����</option>
				<option value="��"> 19:00~21:00 ����</option>
				<option value="��"> 21:00~23:00 ���</option>
				<option value="�N"> 00:00~23:59 �N��</option>
				</select></td>
			</tr>
			<tr>
				<th width="160" align="center">�l���ϸ��G</th>
				<td width="220" ><input type=text name="p15" size=10 value="<%=p15%>"  style="width:100%" ></td>
				<th width="160" align="center">E-mail�G</th>
				<td width="220" ><input type=text name="p3" size=10 value="<%=p3%>"  style="width:100%" ></td>
			</tr>
			<tr>
				
				<th width="160" valign=top align="center">��֨ƶ��G</th>
				<td Colspan=3 valign=top>
					<textarea  style="width:100%" rows="5" name="p8" maxlength="400" ><%=p8%></textarea>
				</td>
			</tr>
			<tr>
				<td class=gridtdtool colspan=4 align=Center><span class=errmsg><%=strMsg%></span><br>			
					<% 'IF ((Cstr(Session("ID")) = Cstr(p101)) OR (Session("AP_Power")=1)) Then 
					%>
				<input type="hidden" name="p11" value=<%=p11%>>
				<input type="hidden" name="p12" value=<%=p12%>>
				<input type="hidden" name="flg"/>
				<a class="lunar" id="lunar">
					<input type="text" name="type" value="0">
					<input type="text" name="answer" size="30">
				</a>
				<input class=gridsubmit type="submit" onClick="document.v.flg.value='ok';" value="�s�W�q��" Style="Cursor:Hand">	
				</td>
			</tr>
		</table>
	</td>
	<%'-- �W���ɮ�-----------%>

	<td width=305 valign=top>
		<%'=strLink1%>
		<BR>
		<%'=strLink2%>
	</td>	
	
	</tr>
</table>
</form>


</body>
</html>
