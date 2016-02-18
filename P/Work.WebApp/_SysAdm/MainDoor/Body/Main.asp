<%Option Explicit%>
<%

Dim CaptionUrlStr1 ,IframeUrlStr1
Dim CaptionUrlStr2 ,IframeUrlStr2
Dim CaptionUrlStr3 ,IframeUrlStr3
Dim CaptionUrlStr4 ,IframeUrlStr4
Dim strKey
	'response.write session("Lid") & "<br>"
	'response.write session("id") & "<br>"
	'response.write Session("NAME") 
	'response.end
	
	'If session("Lid") = "Admin" or session("Lid") = "Ren001" then
	Response.Redirect "../../OnlineService/Member/List.asp"
	'Else
	'Response.Redirect "../../news/news/list.asp"
	'End if
	
	'-- 店內訊息
	CaptionUrlStr1="../../News/NewsSearch/News_List.asp"
	IframeUrlStr1="../../Index/Index/News.asp"

	'-- 活動相簿
	CaptionUrlStr2="../../Activities/ActivitiesSearch/Activities_List.asp?con=1"
	IframeUrlStr2="../../Index/Index/Activities.asp"

	'-- 業績排行
	CaptionUrlStr3="../../Index/Index/rank.asp"
	IframeUrlStr3="../../Index/Index/rank.asp"
	
	'AspErrCheck "SQL Execute Err:"

%>

<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<script LANGUAGE="javascript">
	//自動開啟音樂視窗
	//window.open('index/music.asp','musicwindow','height=10,Width=300,top=20, left=0, toolbar=no, menubar=no, scrollbars=no, resizable=yes,location=no, status=no')
</script>
<title><%'=web_title%></title>
<link  href="../../CSS/layout.css" rel="stylesheet" type="text/css">
<link  href="../../CSS/font.css" rel="stylesheet" type="text/css">
<SCRIPT language="javascript" src="../../_JScript/AC_RunActiveContent.js"></SCRIPT>
</head>
<body>


<div  id="content">
	<div class="ch">
  		<h3><img src="images/news.gif" />店內訊息<a href="<%=CaptionUrlStr1%>" target="main" class="more">更多訊息...</a></h3>
  		<iframe name="I1" src="<%=IframeUrlStr1%>" scrolling="No" frameborder="0" height="200px" width="100%" style="margin:0;padding:0;"></iframe>
 	</div><!--part //-->
 
 	<div class="ch end">
 		<h3><img src="images/photo.gif" />活動相簿<a href="<%=CaptionUrlStr2%>" target="main" class="more">更多分享...</a></h3>
 		<iframe name="I1" src="<%=IframeUrlStr2%>" scrolling="No" frameborder="0" height="200px" width="100%"></iframe>
 	</div><!--part //-->

</div><!--content //-->


<div id="bar">
	<h3><img src="images/others.gif" />業績排行 <a href="<%=CaptionUrlStr3%>" target="main" class="more"></a></h3>
	<iframe name="I2" src="<%=IframeUrlStr3%>" scrolling="No" frameborder="0" height="470px" width="100%"></iframe>
</div><!--bar-->



</body>
</html>
<script src="javascript:'document.body.innerHTML+=\'\'';"></script>
<script type="text/JavaScript" src="../../_JScript/hidefocus.js"></script>
