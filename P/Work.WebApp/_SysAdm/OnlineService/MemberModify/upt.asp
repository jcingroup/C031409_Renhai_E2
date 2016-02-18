<% Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/Power_Function.asp"-->
<%
'On Error Resume Next
Dim StrSql,oRs
Dim WhereObj,strwhere
Dim strQS,strScript,strPageNo,Uptmode
Dim strReturn,strRS
Dim aryIns,aryDel
Dim p0,p1,p2,p3,p4,p5
Dim s1,s2,s3,s4,s5A,s5B
Dim p14Option,p13Option
Dim MasterID
Dim OrderID
Dim p3RadioA,p3RadioB
Dim strMsg

	OrderID = Request.QueryString("p0")
	
	s1=Trim(Request("s1"))	''
	s2=Trim(Request("s2"))	'
	s3=Trim(Request("s3"))	'
	s4=Trim(Request("s4"))	'
	s5A=Trim(Request("s5A"))	'
	s5B=Trim(Request("s5B"))	'
	strPageNo=Trim(Request("pageno"))	'
	
	Uptmode = "Update"	'修改模式

	StrSql = "Select * From 訂單明細檔 Where 訂單序號=" & OrderID
	Set oRS=ExecSQL_RTN_RST(StrSql,3,1,2)
	p1 = oRS("申請人姓名")
	p2 = oRS("申請人生日")
	p3 = oRS("申請人性別")
	
	If p3="1" Then p3RadioA = "CHECKED"
	If p3="2" Then p3RadioB = "CHECKED"		
'=========================
	'-- 回上一頁
	strQS="s1="&s1&"&s2="&s2&"&s3="&s3&"&s4="&s4&"&s5A="&s5A&"&s5B="&s5B&"&pageno=" & strPageNo
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
	
	<input type="button" value="回上一頁" onclick="document.location.href='list.asp?<%=strQS%>'">
	<font color=red>
	<!--#include file="../../_include/top.asp"-->

<form  id="myform" name="myform" action="ajax.myform.submit.asp"  method="post">
	<Input type="hidden" id="Uptmode"  name="Uptmode" value="">
	<Input type="hidden" name="cmd" value="set">
	<Input type="hidden" id="p0" name="p0" value="<%=OrderID%>">
	
<table id="EditDataTable" class="gridtable" width="99%" border="1" cellspacing="0" cellpadding="0">
	<caption class="gridcaption style:margin:0">
		<table width="100%">
			<tr>
				<td class="gridtdcaption" id="CaptionText">安太歲性別生日修改</td>
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
				<th align="right">姓名：</th>
				<td><%=p1%></td>
			</tr>
				<th align="right">性別：</th>
				<td>
					<input type="radio" name="p3" id="p3A" value="1"<%=p3RadioA%>/>先生
					<input type="radio" name="p3" id="p3B" value="2"<%=p3RadioB%>/>小姐
				</td>
			</tr>

			<tr>
				<th align="right">農曆生日：</th>
				<td><input type="text" name="p2" id="p2" value="<%=p2%>" maxlength="10" size="10" />(請輸入農曆生日)</td>
			</tr>
			<tr>
				<td class="gridtdtool" colspan="4" align="Center"><span class="errmsg"><%=strMsg%></span><br />
					<input id="btn_submit" name="btn_submit" class="gridsubmit" type="button" value="確認" Style="Cursor:Hand" />
					
				</td>
			</tr>
		</table>
	</td>
	</tr>
</table>
</form>
<div id="wait" style="
	display:none;width:200px;height:25px; 
	line-height:25px; text-align:center; color:#009900; 
	font-size:12px; border:1px solid #ccc; background:#f8f8f8; 
	position:absolute;padding:2px;">
	傳送資料中，請稍侯．．．
</div>
</body>
</html>

<script type="text/javascript" language="javascript">
	
	$(document).ready(function () {

		$('#btn_SearchData').click(function () {
                	//AjaxSearchData();
            	});
	})
</script>

<script type="text/javascript" language="javascript">
	$(document).ready(function () {
		

		$("#myform").validate({
			rules:
			{
				p2:{ required: true }
			},
			messages:
			{
				//p1:{required: ""}
			}
		});
		
		$('#btn_submit').click(function () {
				//待所有的ajax完成 才可做儲檔的動作
				ajaxHasDone=$.when.apply( $, ajaxRequest );
				
				ajaxHasDone.done(function() {  
					$.fn.activeMyForm();
				});
		});

		(function ($) {
        		$.fn.activeMyForm = function () {
        			if($('#myform').valid()) {
                        		
                        		var FormFlag = false;
                        		
                        		if($('#p3A').prop('checked') || $('#p3B').prop('checked')){
                        			FormFlag = true;
                        		}else{
                        			
                        			FormFlag = false;
                        			alert('請選擇性別!');
                        		}
                        		
                        		if(!VialDate() && FormFlag){
                        			alert('生日日期格式輸入不正確!!!');
						FormFlag = false;
                        		}
                        		
  					if(FormFlag)
					{
						$('#myform').submit();	
					}
				}
         		}
        	})(jQuery);

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
				
				$('#Uptmode').val("Update");
				StatusInit();
				alert('更新完成');
			}else{
				alert(jsonobj.message);
			}
		}
		//ajax主要表單結束

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

        });

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

		var gDate = $('#p2').val().split('/');

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

<script type="text/javascript" language="javascript">
	//顯示讀取資料中..... Show and Hide
	$("#wait").center();
	
	$("#wait").ajaxStart(function(){    
    		$("#wait").css("display","block");    
  	});
  	
  	$("#wait").ajaxComplete(function(){    
    		$("#wait").css("display","none");    
  	}); 
	//讀取資料 Show and Hide
	$("#wait").center();
	
	$("#wait").ajaxStart(function(){    
    		$("#wait").css("display","block");    
  	});
  	
  	$("#wait").ajaxComplete(function(){    
    		$("#wait").css("display","none");    
  	}); 
</script>