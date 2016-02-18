<link rel="Stylesheet" href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.9.2/themes/redmond/jquery-ui.css" type="text/css" />
<style>
.datepicker{width:90px}
</style>
<script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.1.min.js"></script>
<script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.9.2/jquery-ui.min.js"></script>
<script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.9.2/i18n/jquery.ui.datepicker-zh-TW.min.js"></script>

<script type="text/javascript" src="http://malsup.github.com/jquery.form.js"></script>
<script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.9/jquery.validate.min.js"></script>
<script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.9/localization/messages_tw.js"></script>
<script type="text/javascript" src="../../../_Code/JQScript/jquery.query-2.1.7.js"></script>
<script type="text/javascript" src="../../../_Code/JQScript/jquery.selectboxes.pack.js"></script>
<script type="text/javascript" src="../../../_Code/JQScript/CommFunc.js"></script>
<div id="wait" class="ajaxWait">資料讀取中，請稍侯．．．</div>
<script type="text/javascript">
	//顯示讀取資料中..... Show and Hide
	$("#wait").center();

	$("#wait").ajaxStart(function(){    
    		$("#wait").css("display","block");    
  	});

  	$("#wait").ajaxComplete(function(){    
    		$("#wait").css("display","none");    
  	}); 
</script>