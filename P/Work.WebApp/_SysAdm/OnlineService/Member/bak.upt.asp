<% Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/Power_Function.asp"-->
<%
On Error Resume Next
Dim StrSql,oRs
Dim WhereObj,strwhere
Dim strQS,strScript,strPageNo,Uptmode
Dim strReturn,strRS
Dim aryIns,aryDel
Dim p0,p1,p2,p3,p4,p5,p6,p7,p8,p9,p10,p11,p12,p13,p14,p15
Dim p14Option,p13Option
Dim MasterID

	MasterID = Request.QueryString("p0")

	If MasterID = "" Then
		Uptmode = "Insert"	'新增模式
		MasterID = GetSysID("會員戶長資料")
	Else
		Uptmode = "Update"	'修改模式
	End If

'=========================

	p6 = "320"
	p13 = "桃園縣"
	p14 = "中壢市"

	
	StrSql="SELECT 縣市,縣市 FROM 地址縣市 order by 排序 "
	Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	p13Option = RsToOption(oRs,0,1,Cstr(p13),"","")
	AspErrCheck "p13Option"
		
	StrSql="SELECT 鄉鎮,鄉鎮 FROM 地址鄉鎮 Where 縣市='" & p13 & "' order by 排序 "
	Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	p14Option = RsToOption(oRs,0,1,Cstr(p14),"","")
	AspErrCheck "p14Option"	
	
	'-- 回上一頁
	strQS="MasterID=" & MasterID & "&pageno=" & strPageNo
	strReturn="<a href=""list.asp?"&strQS&"&pageno="&strPageNo&""">←回上一頁</a>"	
	

%>
<html>
<head>
	<title></title>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<link rel=stylesheet href=../../_CSS/default.css>
	<script language=javascript src="lunar.js"></script>
	<style type="text/css">
		.gridtd1{color:black;text-align:center}
		.gridtd2{color:black;text-align:center}	
		.gridtd3{color:black;text-align:center}	
		.gridtd4{color:black;text-align:center}
		.gridtd5{color:black;text-align:center}
		.gridtd6{color:black;text-align:center}
		.gridtd7{color:black;text-align:center}
		.gridtd8{color:black;text-align:center}
		.gridtd9{color:black;text-align:center}
		.gridtd10{color:black;text-align:center}
		.gridtd11{color:black;text-align:center}
	</style>
	
		
	<style>
		.GridTable{
			margin-top:10px;
			border:5 double #778899;
			}
		TH{
			background:#F5F5DC;
			}
			
			
		.TDLine
		{
			background-color:#FF7573;
			color:#FFFFFF;
			text-align:center;
			font-size:11pt;
			}
	</style>
	
	
	
		<!-- 這一段要放在HEAD裡面 -->
		
</head>
<body>
	<%'=strReturn %>
	
	<input type="button" value="回上一頁" onclick="history.back()">
	<font color=red>*使用說明：1.請先確定戶長及家庭成員人數姓名，「安太歲」農曆生日生辰要正確。2.確認後「新增訂單」。</font>
	<!--#include file="../../_include/top.asp"-->
	<!-- Grid List區段 -->
	<form id="frmGrid" name="frmGrid" action="ifrm_del.member.asp" method="post">
		<input type="hidden" name="MemberCount" id="MemberCount" value="0">
		<input type="hidden" name="MasterID" id="MasterID" value="<%=MasterID%>">
		<div id="RsToTabel"></div>
	</form>
	<!-- Grid List區段 -->

<form  id="myform" name="myform" action="ajax.myform.submit.asp"  method="post">
	<Input type="hidden" id="Uptmode"  name="Uptmode" value="">
	<Input type="hidden" name="cmd" value="set">
	<Input type="hidden" id="p0" name="p0" value="">
	
<table id="EditDataTable" class="gridtable" width="99%" border="1" cellspacing="0" cellpadding="0">
	<caption class="gridcaption style:margin:0">
		<table width="100%">
			<tr>
				<td class="gridtdcaption" id="CaptionText"></td>
				<td align="right"></td>
			</tr>
		</table>
	</caption>
	<tr>
	<td valign="top">
		<table width="100%" border="0" cellspacing="1" cellpadding="1">
			<tr>
				<th width="90"></th><td Width="210"></td>
				<th width="120"></th><td Width="180"></td>
			</tr>
<!--<tr><td colspan="4" class="TDLine">基本資料填寫</td></tr>-->
			<tr>
				<th align="right">戶長ID：</th>
				<td><%=MasterID%><input type="hidden" name="MasterID" id="MasterID" value="<%=MasterID%>"/></td>

				<th align="right">是否為戶長：</th>
				<td><input type="checkbox" name="p1" id="p1" value="1"<%=p1Check%>/></td>
			</tr>

			<tr>
				<th align="right">姓名：</th>
				<td><input type="text" name="p2" id="p2" value="<%=p2%>" maxlength="20" size="20" />
					<font color=red>*姓名最長20個字元，外籍人士請簡寫。</font>
					</td>

				<th align="right">性別：</th>
				<td>
					<input type="radio" name="p3" id="p3A" value="1"<%=p3RadioA%>/>先生
					<input type="radio" name="p3" id="p3B" value="2"<%=p3RadioB%>/>小姐
				</td>
			</tr>

			<tr>
				<th align="right">生日：</th>
				<td>
					<input type="radio" name="p15" id="p15A" value="國"/>國曆
					<input type="radio" name="p15" id="p15B" value="農"/>農曆					
					<input type="text" name="p4" id="p4" value="<%=p4%>" maxlength="10" size="10" />
					
					<select id="p16" name="p16">
						<option value="鼠">鼠</option>
						<option value="牛">牛</option>
						<option value="虎">虎</option>
						<option value="兔">兔</option>
						<option value="龍">龍</option>
						<option value="蛇">蛇</option>
						<option value="馬">馬</option>
						<option value="羊">羊</option>
						<option value="猴">猴</option>
						<option value="雞">雞</option>
						<option value="狗">狗</option>
						<option value="豬">豬</option>
					</Select>
					<input type="button" name="btn_NoBirthday" id="btn_NoBirthday" value="不填生日" />
					<br /><font color=red>*安太歲、入斗保運一定要打生日，請輸入民國年(格式:66/1/1)</font>
				</td>

				<th align="right">生辰：</th>
				<td>
				<select name="p5" id="p5">
				<option value="吉"> 00:00~23:59 吉時</option>
				<option value="子"> 23:00~01:00 子時</option>
				<option value="丑"> 01:00~03:00 丑時</option>
				<option value="寅"> 03:00~05:00 寅時</option>
				<option value="卯"> 05:00~07:00 卯時</option>
				<option value="辰"> 07:00~09:00 辰時</option>
				<option value="巳"> 09:00~11:00 巳時</option>
				<option value="午"> 11:00~13:00 午時</option>
				<option value="未"> 13:00~15:00 未時</option>
				<option value="申"> 15:00~17:00 申時</option>
				<option value="酉"> 17:00~19:00 酉時</option>
				<option value="戌"> 19:00~21:00 戌時</option>
				<option value="亥"> 21:00~23:00 亥時</option>
				
				</select>
				</td>
			</tr>

			<tr>
				<th align="right">地址：</th>
				<td colspan="3"><input type="text" name="p6" id="p6" size="5" value="<%=p6%>" />-
					<Select id="p13" name="p13"><%=p13Option%></select>
					<Select id="p14" name="p14"><%=p14Option%></select>
					<input type="text" name="p7" id="p7" value="<%=p7%>" maxlength="128" size="44" /><font color=red>*戶長一定要打地址</font>
					</td>
			</tr>			
			<tr>
				<th align="right">手機：</th>
				<td><input type="text" name="p8" id="p8" value="<%=p8%>" maxlength="16" size="16" /></td>

				<th align="right">電話：</th>
				<td>	<input type="hidden" name="p11" id="p11" value=""/>
					<input type="text" name="p9" id="p9" value="<%=p9%>" maxlength="16" size="16" /></td>
			</tr>
			
			<tr>
				<th align="right">EMAIL：</th>
				<td colspan="3"><input type="text" name="p10" id="p10" value="<%=p10%>" maxlength="64" size="64" /></td>
			</tr>
			<tr>
				<td class="gridtdtool" colspan="4" align="Center"><span class="errmsg"><%=strMsg%></span><br />
					<input id="btn_submit" name="btn_submit" class="gridsubmit" type="button" value="確認" Style="Cursor:Hand" />
					<input type="button" id="CancelEdit" value="取消編輯" class="gridsubmit" Style="Cursor:Hand" />
				</td>
			</tr>
		</table>
	</td>
	</tr>
</table>
</form>

</body>
</html>

<script type="text/javascript" language="javascript">
	
	function RedireOrder(){
		document.location.href='../Order/Upt.asp?MasterID=' + $('#MasterID').val();
	}

	//按下新增成員
	function AddNewMember(){
		$('#Uptmode').val('Insert');
		$('#p0').val('');

        	$('#p2').val('');
        	$('#p13').val('桃園縣');
        	countryName = '中壢市';
        	$('#p13').trigger('change');

        	$('#p7').val('');
        	$('#p1').attr('checked',false);
        	$('#p4').val('');
        	$('#p5').val('吉');
        	$('#p8').val('');  		       	
        	$('#p9').val('');
       		$('#p10').val('');   		       	
        	$('#p3A').attr('checked', true); 

		$('#p15B').attr('checked', true); 

		var data_jsonstyle = {MasterID:$('#MasterID').val()};
		EvevtAjaxHandle (data_jsonstyle,"ajax.get.master.data.asp",AddNewMember_Handle);	

		var SupportDiv = document.getElementById('EditDataTable');
   		window.scroll(0,findPos(SupportDiv));	
	}
	//此Ajax主要為新增成員時 可先放入預設的資料
	function AddNewMember_Handle(contact){
		if(contact.result==true)
        	{
        		AllowEdit();
        		$('#btnins2').attr('disabled',true); //新增Button Disabled 
        		$('#btnins0').attr('disabled',true); //新增Button Disabled 
        		if(contact.IsData){
        		        
        		        $('#p13').val(contact.縣市);
        			$('#p14').val(contact.鄉鎮);	
        			$('#p7').val(contact.地址);	
        			$('#p9').val(contact.電話尾碼);	
        			$('#CaptionText').text("會員管理：新增");
        			
        		}else{
        			$('#p1').attr('checked',true);
        			//alert('Not Data')
        		}
        		$('#p2').focus(); //新增家庭成員 姓名會成 focus
        	}
        	else
        	{alert(contact.message);}
	}	
	
	
	$(document).ready(function () {
		var countryName = '';

		$('#btn_SearchData').click(function () {
                	AjaxSearchData();
            	});
            	
            	EnterDownData();

	})
	
	function EnterDownData(){
		var data_jsonstyle = {MasterID:$('#MasterID').val()};
		EvevtAjaxHandle (data_jsonstyle,"ifrm_list.ajax.load.data.asp",EnterDownData_Handle);
	}
	function EnterDownData_Handle(contact){
		if(contact.result==true)
        	{
        		$('#RsToTabel').html(contact.GridList);
        		$('#MemberCount').val(contact.RecordCount);
			
			//將西元 - 1911
        		$('#.LBirthday').each(function(index){
        			var LBirthdayArray = $(this).text().split('/');
        			var LBirthdayYear =parseInt(LBirthdayArray[0],10)-1911;
        			
        			$(this).text(LBirthdayYear + '/' + LBirthdayArray[1] + '/' + LBirthdayArray[2]);
        		})

        		if(contact.RecordCount==0){ //如果都沒有家庭成員 則新增訂單鍵取消
        			$('#btnins0').attr('disabled',true);
        		}else
        		{
        			$('#btnins0').attr('disabled',false);
        		}
        	}
        	else
        	{alert(contact.message);}
	}	
	
	//這個是給 換頁Select Box Ajax使用
	$('#PageNo').live('change',function () {
        	AjaxChangePage($('#PageNo').val());
	})	


	function AjaxChangePage(page){

		var data_jsonstyle = $.fn.CollectQuery();
		data_jsonstyle['pageno'] = page;
		data_jsonstyle['MasterID'] = $('#MasterID').val();
		
		EvevtAjaxHandle (data_jsonstyle,"ifrm_list.ajax.load.data.asp",AjaxChangePage_Handle);	
	}

	function AjaxChangePage_Handle(contact){
		
		if(contact.result==true)
        	{
			$('#RsToTabel').html(contact.GridList);
			
			//將西元 - 1911
			$('#.LBirthday').each(function(index){
        			var LBirthdayArray = $(this).text().split('/');
        			var LBirthdayYear =parseInt(LBirthdayArray[0],10)-1911;
        			$(this).text(LBirthdayYear + '/' + LBirthdayArray[1] + '/' + LBirthdayArray[2]);
        		})
        	}
        	else
        	{
			alert(contact.message);
		}	
	}
	
	function AjaxSearchData(){
		var data_jsonstyle = $.fn.CollectQuery();
		EvevtAjaxHandle (data_jsonstyle,"ifrm_list.ajax.load.data.asp",AjaxChangePage_Handle);
	}
	
	function AjaxSearchData_Handle(contact){
		
		if(contact.result==true)
        	{
			$('#RsToTabel').html(contact.GridList);
        	}
        	else
        	{
			alert(contact.message);
		}	
	}
	
	//修改會員資料 Ajax程序
	function AjaxGetMemberData(id){
		
		if($('#Uptmode').val()=='Insert'){
			alert('新增模式下無法修改');
		}
		else{
            		var data_jsonstyle = {MemberID:id};
			EvevtAjaxHandle (data_jsonstyle,"ajax.get.member.data.asp",AjaxGetMemberData_Handle);
			
			var SupportDiv = document.getElementById('EditDataTable');
   			window.scroll(0,findPos(SupportDiv));
   		}
        }
            	
	function AjaxGetMemberData_Handle(contact){
			if(contact.result==true)
        		{
        			$('#btnins2').attr('disabled',true); //修改模式，則新增禁止
        			$('#btnins0').attr('disabled',true); //修改模式，則新增禁止
        			
        			AllowEdit();
        			$('#p0').val(contact.序號);
        			$('#p2').val(contact.姓名);
        			
        			$('#p13').val(contact.縣市);
        			countryName = contact.鄉鎮;
           			$('#p13').trigger('change');  			

        		       	$('#p7').val(contact.地址);
        		       	$('#p1').attr('checked',contact.Is戶長);
        		       	$('#p4').val(contact.生日);
        		       	$('#p5').val(contact.時辰);
        		       	$('#p8').val(contact.手機);  		       	
        		       	$('#p9').val(contact.電話尾碼);
       		       		$('#p10').val(contact.EMAIL);   		       	
       		       		$('#p16').val(contact.生肖);
        		       	$('#p6').val(contact.郵遞區號);

        		        if (contact.性別=='1')	$('#p3A').attr('checked', true); 
        			if (contact.性別=='2')	$('#p3B').attr('checked', true); 
        			
        			$('#p15B').attr('checked', true); 
        			
        			$('#Uptmode').val('Update');
        			$('#CaptionText').text("家庭成員管理：修改(修改後需點「確認」或「取消編輯」)"); 			       	
        		}
        		else
        		{alert(contact.message);}

        		
	}
</script>

<script type="text/javascript" language="javascript">
	$(document).ready(function () {
		
		//縣市 鄉鎮 地址切換
		$(function () {
        	    $('#p13').change(function () {
        	
        	        $('#p14').removeOption(/.?/).ajaxAddOption(
        	        	'ajax_GetAddressCountry.asp',
        	        	{ 'id': $(this).val() },
        	        	false,
        	        	function () {
        	    			// 設定預設選項
       	                		$(this).selectOptions(countryName).trigger('change');
        	        	}
        	    	)
        	    })
        	})
        
        	//取得郵遞區號
        	$('#p14').change(function () {
        	 	var data_jsonstyle = {id:$('#p13').val(),ids:$('#p14').val()};
			EvevtAjaxHandle (data_jsonstyle,"ajax_GetAddressZip.asp",Ajax_GetZipCode)
        	})
        
        
        	function Ajax_GetZipCode(contact){
        		if(contact.result==true)
        		{
				$('#p6').val(contact.Zip);
        			if(contact.message!=''){
        				alert(contact.message);
        			}        		
        		}
        		else
        		{
        			alert(contact.message);
        		}
        	}
		
		//$("#p8").datepicker();		
		//$("#p11").datepicker();
		//ajax主要表單設定
		//表單填寫規則設定
		$("#myform").validate({
			rules:
			{
				//p1:{ required: true },
				p7:{ required: true },
				p10:{ email: true }
				//p3:{ required: true, date: true },
				//p4:{ required: true, digits: true }
			},
			messages:
			{
				//p1:{required: ""}
			}
		});
		
		$('#btn_submit').click(function () {
		
			if($('#myform').valid()) {

				if(VialDate())
				{
					var gDate = $('#p4').val().split('/');
					
					if($('input[name$="p15"]:checked').val()=='國'){
						Lunar(0,parseInt(gDate[0],10)+1911,gDate[1],gDate[2]);
						$('#p4').val((lunar.y-1911) + '/' + lunar.m + '/' + lunar.d);
					}else{
						//$('#p4').val( (parseInt(gDate[0],10)+1911) + '/' + gDate[1] + '/' + gDate[2]);
					}
					$('#myform').submit();	
				}
				else
				{
					alert('生日日期格式輸入不正確!!!');
				}
			}
		});

		$('#myform').submit(function () {
			$(this).ajaxSubmit(options);
			return false;
		});
            
		var options = { target: '#outputmessagefromserver',    
			beforeSubmit: showRequest,  
			success: showResponse, 
			dataType: 'json'
		};

		function showRequest(formData, jqForm, options) {
			var queryString = $.param(formData);
			return true;
		}
        
        	//表單在Server完成 傳送Data回來，由此function處理
		function showResponse(jsonobj, statusText, xhr, $form) {
			//
			if(jsonobj.result){
				$('#p0').val(jsonobj.InsertID);
				$('#Uptmode').val("Update");
				$('#p15B').attr('checked',true);
				EnterDownData();
				StatusInit();
				alert('更新完成');
				NotEdit(); //更新完成後 就不可編輯
				//alert(jsonobj.InsertID);
			}else{
				alert(jsonobj.message);
			}
		}
		//ajax主要表單結束


		//Grid表單填寫規則設定
		//$("#frmGrid").validate({
		//	rules:	{},
		//	messages:{}
		//});

		var frmGrid_options = {    
			beforeSubmit: showRequest_frmGrid,  
			success: showResponse_frmGrid, 
			dataType: 'json'
		};

		$('#cmd').live('click',function () {
			if(confirm("確定要刪除資料？")){
				if($('#frmGrid').valid()) {
					$('#frmGrid').submit();
                        	}
                	}
		});

		$('#frmGrid').submit(function () {
			$(this).ajaxSubmit(frmGrid_options);
			return false;
		});
 
		function showRequest_frmGrid(formData, jqForm, frmGrid_options) {
			var queryString = $.param(formData);
			return true;
		}
        
        	//表單在Server完成 傳送Data回來，由此function處理
		function showResponse_frmGrid(jsonobj, statusText, xhr, $form) {
			//
			if(jsonobj.result){
				EnterDownData();
				alert('刪除完成');
				//alert(jsonobj.InsertID);
			}else{
				alert(jsonobj.message);
			}
		}
		//ajax主要表單結束

		//取消編輯
        	$('#CancelEdit').click(function () {
        		
			$('#Uptmode').val('');
        		$('#btnins2').attr('disabled',false); //開起新增 
        		if($('#MemberCount').val()!='0'){
	       			$('#btnins0').attr('disabled',false); //開起新增
        		}
			NotEdit();
        	})


		//無生日
        	$('#btn_NoBirthday').click(function () {
        		
			$('#p4').val('1/1/1');
			$('#p14B').attr('checked',true);
			$('#p16').val('鼠');
        	})



		//填完生日後取得生肖
        	$('#p4').blur(function () {
        		if(VialDate()){
				var gDate = $('#p4').val().split('/');
        			if($('input[name$="p15"]:checked').val()=='國'){
					Lunar(0,parseInt(gDate[0],10)+1911,gDate[1],gDate[2]);
					var GetLurDateYear = (lunar.y-1911) ;
				}else{
					var GetLurDateYear = gDate[0] ;
				}

        		        var data_jsonstyle = {LurYear:GetLurDateYear};
				EvevtAjaxHandle (data_jsonstyle,"ajax_GetYearAnimal.asp",ajax_GetYearAnimal_Handle)	
        		}
        	})
        
        	function ajax_GetYearAnimal_Handle(contact){
        		if(contact.result==true)
        		{
        			if(contact.IsData){
					$('#p16').val(contact.生肖);
				}
        			if(contact.message!=''){
        				alert(contact.message);
        			}        		
        		}
        		else
        		{
        			alert(contact.message);
        		}
        	}

		//狀態設定
		function StatusInit(){

			if($('#Uptmode').val()=="Insert"){
				$('#btn_submit').val("確認");
				$('#CaptionText').text("會員管理：新增");
			}
			
			if($('#Uptmode').val()=="Update"){
				$('#btn_submit').val("確認");
				$('#CaptionText').text("會員管理：修改");					
			}			
		}
		

		
		StatusInit();
		NotEdit(); //一開始進來是禁止編輯
        });

	function AllowEdit(){
			$('#myform').attr('disabled',false);
			$('#myform input').attr('disabled',false);
			$('#myform select').attr('disabled',false);
	}
		
	function NotEdit(){
			$('#myform').attr('disabled',true);
			$('#myform input').attr('disabled',true);
			$('#myform select').attr('disabled',true);
	}

        function trim(txt){
	var left, right;
	var txt2 = ""
	for(i=0; i<txt.length; i++){
		if(txt.charAt(i) == " "){
			continue;
		}else{
			left = i;
			break;
		}
	}
	for(i=txt.length-1; i>=0; i--){
		if(txt.charAt(i) == " "){
			continue;
		}else{
			right = i;
			break;
		}
	}
	for(i=left; i<=right; i++){
		txt2 = txt2 + txt.charAt(i);
	}
	return txt2;
	}
	
	
	function VialDate(){

		var gDate = $('#p4').val().split('/');

		if(gDate.length!=3)
		{
			return false;
		}
		else
		{
			if(isNaN(gDate[0]) ||  isNaN(gDate[1]) || isNaN(gDate[2]) || trim(gDate[0])=='' ||  trim(gDate[1])=='' ||  trim(gDate[2])=='')
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}
        
</script>