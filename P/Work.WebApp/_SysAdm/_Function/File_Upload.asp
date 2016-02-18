<% Option Explicit%>
<!--#include file="../_Function/Upload_Function.asp"-->
<!--#include file="../_Function/DB_Function.asp"-->
<!--#include file="../_Function/RS_Function.asp"-->
<%

'--  本頁面為上傳檔案主要的頁面非副程式
'-- 如果是使用 ACCESS DB 請注意下列事項
'-- 1.這裡不能引用 DB_Function.asp (因為路徑不對找不到DB檔)
'-- 2.這裡不能引用 Base_Function.asp (因為DB_Function.asp 沒有引用)


Dim strScript 		'此程式名稱
Dim strPath			'此程式目的路徑(實體)
Dim strSub
Dim strTPath
Dim i				'計算
Dim strDPath		'刪除的目力路徑(實體)
Dim strFile			'檔案列表
Dim strCaption
Dim strSaveForder
Dim intFileType
Dim intOPTCount,intHeight
Dim AryImgResize,strImgResize
Dim intImg0W,intImg0H
Dim IntImgConvertFlag
Dim strQS
Dim p0
		
	p0=request("p0")
	strScript=Request.ServerVariables("SCRIPT_NAME")
	intFileType=Trim(Request("FileType"))
	strPath=Trim(Request("path"))
	strSub=Trim(Request("sub"))
	strCaption=Trim(Request("Caption"))
	strImgResize=Trim(Request("ImgReSize"))

	If strImgResize = "" Then strImgReSize="0,0,0,0,0,0"
	strQS="FileType="&intFileType&"&path="&strPath&"&sub="&strSub&"&Caption="&strCaption&"&ImgReSize="&strImgResize
	'RESPONSE.WRITE strQS

	'-- 設定上傳檔案ASP用的小圖(寬度固定)--------
	intImg0W=150
	intImg0H=120
	'============================================

	'RESPONSE.WRITE "("& intImg0W & "," & intImg0H & ")　"
	'RESPONSE.WRITE strImgResize & "<BR>"


	'If hasUploadPower(strPath)=0 Then GoAway

	'-- 設定上傳檔案的目錄
	'-- 因為這支ASP是被嵌在IFrame 裡面使用，
	'-- 所以上傳檔案的存放路徑階層，從這個檔案所在開始起算
	strSaveForder="../../_Upload"

	'-- 組合相關子路徑
	strTPath=strSaveForder & "/" & strPath & "/" & strSub

	'-- 轉換成實體路徑
	strTPath=Server.MapPath(strTPath)

	'-- 依造命令來處裡---------------------------
	Select Case Trim(Request("cmd"))
		Case "upload"	'上傳檔案

			'CALL FileUpload (strTPath)
			AryImgResize=Split(strImgResize,",")
			'Call FileUpload_Dundas (strTPath,intImg0W,intImg0H,intImg1W,intImg1H,intImg2W,intImg2H,intImg3W,intImg3H,IntImgConvertFlag)
			Call FileUpload_Dundas (strTPath,intImg0W,intImg0H,AryImgResize,p0)

		Case "delete"	'刪除檔案
			For i=1 to Request.Form("dfile").Count

				'-- 刪除
				strDPath=strTPath & "\" & Request.Form("dfile")(i)
				DeletePath strDPath,1

				'-- 刪除A圖
				strDPath=strTPath & "\A\" & Request.Form("dfile")(i)
				DeletePath strDPath,1

				'-- 刪除B圖
				strDPath=strTPath & "\B\" & Request.Form("dfile")(i)
				DeletePath strDPath,1

				'-- 刪除C圖
				strDPath=strTPath & "\C\" & Request.Form("dfile")(i)
				DeletePath strDPath,1

				'-- 刪除暫存
				strDPath=strTPath & "\_Temp\" & Request.Form("dfile")(i)
				DeletePath strDPath,1


			Next
	End Select
	'============================================

	'-- 把上傳的檔案列出來
	strFile=FileToOption(strTPath)


	'''=======================
	'''是否可做上傳:
	'''=======================
	''Function hasUploadPower(strPath)
	''	hasUploadPower=ExecSP("usp_sysHasUptPower",Array(strPath,Session("ID")))(0)
	''End Function
	'''=======================
	'''顯示無權限
	'''=======================
	''Sub GoAway()
	''		Response.Write "<script language=javascript>window.location.href='../_error/nopermission.asp'</script>"
	''		Response.End
	''End Sub



	Dim strFileShow
	strFileShow=strSaveForder & "/" & strPath & "/" & strSub & "/"

	Select Case intFileType
	Case 1
		'-- 標準(檔案上傳)
		If strCaption="" Then strCaption="檔　案　上　傳"
		strFileShow=FileToList(Server.MapPath(strFileShow),strFileShow)
		intOPTCount=1
		intHeight=intOPTCount*18
	Case 13
		'-- 變種(檔案上傳)
		If strCaption="" Then strCaption="檔　案　上　傳"
		strFileShow=FileToList(Server.MapPath(strFileShow),strFileShow)
		intOPTCount=3
		intHeight=intOPTCount*18
	Case 15
		'-- 變種(檔案上傳)
		If strCaption="" Then strCaption="檔　案　上　傳"
		strFileShow=FileToList(Server.MapPath(strFileShow),strFileShow)
		intOPTCount=5
		intHeight=intOPTCount*18


	Case 2
		'-- 標準(圖片上傳)
		If strCaption="" Then strCaption="圖　片　上　傳"
		strFileShow=FileToPhoto(Server.MapPath(strFileShow),strFileShow,0)
		intOPTCount=1
		intHeight=intOPTCount*150

	Case 23
		'-- 變種(圖片上傳)(產品小圖)
		If strCaption="" Then strCaption="圖　片　上　傳"
		strFileShow=FileToPhoto(Server.MapPath(strFileShow),strFileShow,150)
		intOPTCount=3
		intHeight=intOPTCount*150

	Case 25
		'-- 變種(圖片上傳)(產品小圖)
		If strCaption="" Then strCaption="圖　片　上　傳"
		strFileShow=FileToPhoto(Server.MapPath(strFileShow),strFileShow,150)
		intOPTCount=5
		intHeight=intOPTCount*150		

	Case Else
		RESPONSE.WRITE "上傳檔案參數錯誤!!"
		RESPONSE.END


	End Select

%>
<html>
<head>
	<title>檔案上傳管理</title>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<link rel=stylesheet href=../_CSS/Default.css>
</head>
<style>
	.gridbutton{
		background:#b0c4de;
		font-size:10pt;
		cursor:hand;
	}
</style>
<body scroll=no>

<table width=100% Border=0 cellspacing=0 cellpadding=0>
<tr><td>

	<form action="<%=strScript%>?p0=<%=p0%>&cmd=upload&<%=strQS%>" method="post" ENCTYPE="multipart/form-data">
		<table width=100% cellspacing=0 cellpadding=0 border=0>
			<tr>
				<th class=CaptionUpload><%=strCaption%></th>
			</tr>
			<tr>
				<td>
					<table width=100% cellspacing=0 cellpadding=0 border=0>
					<tr>
						<td><input type="file" name="file1" value=""  style="width:100%"></td>
						<td width=40><input type="submit" value="上傳" class=gridbutton></td>
						</tr>
					</table>
				</td>
			</tr>
	
		</table>
	</form>
	
	<form action="<%=strScript%>?<%=strQS%>" method="post" >
	<Input type="hidden" name="p0"  value="<%=p0%>">
	<input type="hidden" name=cmd value="delete">
		<table width=100% cellspacing=0 cellpadding=0 border=0>
			<tr>
				<td>
					<input type="submit" value="刪　除" style="width:100%" class=gridbutton>
					<select style="width:100%" multiple name=dfile size=<%=intOPTCount%>>
						<%=strFile%>
					</select>
	
				</td>
			</tr>
			<tr>
				<td>
					<div style="overflow:auto;width:100%;height:<%=intHeight%>;" align=Left>
					<%=strFileShow%>
					</div>
				</td>
			</tr>
		</table>
	</form>

</td></tr>
</table>
</body>
</html>

