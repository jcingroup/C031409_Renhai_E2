
<% Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->

<%

Dim oRs,strRs
Dim strScript,strPageNo,strQS

Dim strSql,strWhere
Dim strCourseList,strNewsList
Dim strURL
Dim strPic1

Dim strLID
Dim strMsg

Dim strCmd				'P:命令
Dim strPWD				'P:密碼
Dim strIP				'登入IP
	
	 strScript=Request.ServerVariables("SCRIPT_NAME")

	 strMsg=Trim(Request("ErrMsg"))
	 If Session("UserKind")="" OR IsEmpty(Session("UserKind")) Then Session("UserKind")=0
%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<title>中壢仁海宮會員點燈管理系統</title>
	
	<link href="../../_CSS/login.css" rel="stylesheet" type="text/css">
	<script type="text/javascript" src="../../_JScript/iepngfix_tilebg.js"></script> 
	<script Language="javascript">this.focus()</script>
</head>
<body>
<div id="container" >
	<h1>中壢仁海宮會員點燈管理系統</h1>
	<form action="LoginCheck.asp" method="post" target="_parent"> 	
		<Input type="hidden" name="cmd" value="login">
		<p>
			<label>帳號 UserName</label>
			<input name="lid" type="text" value="" tabindex="1">
		</p>
		<p>
			<label>密碼 Password</label>
			<input name="pwd" type="password" value="" tabindex="2">
		</p>
		
		<p >
			<label>驗證碼CheckCode</label>
			<input name="img_vildate" type="txt" value=""  tabindex="3" style="width:25%">
			<img src="../../_Function/ValidCode.asp"  border="1" WIDTH=80 HEIGHT=20>
		</p>		
		<font color=red><%=strMsg%></font>
		<p class="login" ><input align=absmiddle type="image" src="../../images/login.gif" name="I1" alt="按此登入" tabindex="3"><p>
	</form>
</div><!--container //-->
</body>
</html>