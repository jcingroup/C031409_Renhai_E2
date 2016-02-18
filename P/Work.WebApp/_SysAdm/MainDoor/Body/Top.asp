<html>
<head>
 	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
 	<title>管理系統</title>
 	<base target="_self">
 	<link href="../../_CSS/MainDoor.css" rel="stylesheet" type="text/css">
 	<style type="text/css">
  		body {background:#ffffff url(images/top_bg.gif) repeat-x top left;text-align:left}
 	</style>
</head>

<body id="page_top">
	<div id="header">
 		<div id="today">
 	
			<span class="dec">現在時間：</span>
			<span class="date"><%=Date%></span>
			<span class="wk"><%=WK(Date)%></span>
			<span class="time" ID="TT"><%=Time()%></span>
  
			<Script Language=VBScript>
				Clock()
				Sub Clock()
					TT.InnerHtml=Time
					Window.SetTimeout "Clock()",1000
				End Sub
			</Script>
     
 		</div><!--today //-->
 
		<ul id="login">
			<li class="user">使用者：<%=Session("Name")%></li>
			<li class="user">單位：<%=Session("UName")%></li>
			<li><a href="Main.asp" target=main >回主畫面</a></li>
			<li><a href="#" onclick="parent.location.href='LogOut.asp?status=yes'" >登　出</a></li>
		</ul> 
	</div><!--header-->
</body>
</html>


<%
'-- 副程式-----------------------------------------
'-- 數字轉中文星期---------------------------------
'-- Jack 2004/01/30 -------------------------------
Function WK(varDate)
Dim MM

	Select Case Weekday(varDate,2)
	Case 1
		MM="<span class='wk1'>星期一</span>"
	Case 2                         
		MM="<span class='wk1'>星期二</span>"
	Case 3                         
		MM="<span class='wk1'>星期三</span>"
	Case 4                         
		MM="<span class='wk1'>星期四</span>"
	Case 5                         
		MM="<span class='wk1'>星期五</span>"
	Case 6                         
		MM="<span class='wk2'>星期六</span>"
	Case 7                         
		MM="<span class='wk2'>星期日</span>"
	End Select
	WK=MM

End Function

%>