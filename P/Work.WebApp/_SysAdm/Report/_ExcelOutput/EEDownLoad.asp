<%Option Explicit%>
<%
Dim strTitle
Dim strPathFile
Dim	strFileName
Dim strFileType
Dim strURL
Dim OpenFlag

	'-- UTF-8 偵錯時,要先定義網頁顯示編碼,才可正常顯示不會有亂碼
	'Response.Write "<html><head><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""></head>"
	'============================================================

	strTitle=Trim(Request("FileName"))
	strPathFile=Trim(Request("PathFile"))
	strFileName=Trim(Request("FileName"))
	
	strURL="EEDirectDL.asp?FileName="&Server.URLEncode(strFileName)

	'RESPONSE.WRITE "strTitle:" & strTitle & "<BR>"
	'RESPONSE.WRITE "strPathFile:" & strPathFile & "<BR>"
	'RESPONSE.WRITE "strFileName:" & strFileName  & "<BR>"
	'RESPONSE.WRITE "strURL:" & strURL & "<BR>"
	'RESPONSE.END

%>

<html>
<head>
	
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<title><%=strTitle%></title>
	<script language="JavaScript" >
		//-- 直接開啟----------------------
		function OpenExcel(OpenFlag){
			//var winWidth=(screen.availwidth)/2 - 10;
			//var winHeight=(screen.availheight)/2 - 70;
			var winWidth='1024';
			var winHeight='768';
			var intTop=0;
			var intLeft=0;
			
			
			if (OpenFlag=='1'){
				//用二進位的方式開 
				var URL="<%=strURL%>"
				window.open(URL,"EEDownLoad",'top='+intTop+',left='+intLeft+',width='+winWidth+',height='+winHeight+',resizable=no,scrollbars=no,status=no,menubar=No,toolbar=No');
				window.opener=null;
				window.close;
				parent.window.close();


				}
			else if(OpenFlag=='2'){
				//直接用 IE 開EXCEL,
				var URL="<%=strPathFile%>"	
			
				window.open(URL,"EEDownLoad",'top='+intTop+',left='+intLeft+',width='+winWidth+',height='+winHeight+',resizable=Yes,scrollbars=Yes,status=Yes,menubar=yes,toolbar=Yes');
				window.opener=null;
				window.close;
				parent.window.close();
			}
			
		}//================================
		
	</script>
</head>
<body onload="focus();OpenExcel('0');">
	<link rel=stylesheet href=.././_css/default.css></head>
	
	<table width=800 height=400 align=Center border=0 cellpadding=0 cellspacing=0  >
		<tr Height=200>
			<td Width=200>&nbsp;</td>
			<td >&nbsp;</td>
		</tr>
		<tr>
			<td >&nbsp;</td>
			<td align=LEFT >下列兩個檔案資料內容都一樣,請依照瀏覽器版本來選擇!!</td>
		</tr>

		<tr>
			<td >&nbsp;</td>
			<td align=LEFT ><a href="javascript:void(0);" onclick="OpenExcel('1');"  ><%=strFileName%></a>開啟檔案1_直接開啟EXCEL(IE8.IE9適用)</td>
		</tr>
		<tr>
			<td >&nbsp;</td>
			<td align=LEFT ><a href="javascript:void(0);" onclick="OpenExcel('2');"  ><%=strFileName%></a>開啟檔案2_瀏覽器開啟EXCEL(IE6,IE7適用)</td>
		</tr>		
	</table>
	
</body>
</Html>
	