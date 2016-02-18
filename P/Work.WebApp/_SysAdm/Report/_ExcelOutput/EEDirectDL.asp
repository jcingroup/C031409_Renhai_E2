<%Option Explicit%>
<%
Response.Buffer = True

Dim strPathFile
Dim	strFileName
Dim strFileSize
Dim strFileType

Dim objStream
Dim ContentType

	'-- UTF-8 偵錯時,要先定義網頁顯示編碼,才可正常顯示不會有亂碼
	Response.Write "<html><head><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""></head>"

	
	'============================================================

	strFileName=Trim(Request("FileName"))
	strPathFile="../../_DLoadTmp/ExcelSample/"&strFileName
	strFileSize = Trim(Request("FileSize"))

	
	'RESPONSE.WRITE strPathFile & "<BR>"
	RESPONSE.WRITE strFileName  & "<BR>"
	'RESPONSE.WRITE Server.UrlEncode(strFileName)  & "<BR>"
	'RESPONSE.WRITE Server.UrlPathEncode(strFileName)  & "<BR>"
	'RESPONSE.WRITE strFileSize  & "<BR>"
	'RESPONSE.WRITE Server.MapPath(strPathFile)  & "<BR>"
	'RESPONSE.END

		
	Response.Clear


	'-- 用二進位的方式輸出---------------------------------
	Const adTypeBinary = 1
	'-- Requires MDAC 2.5 to be stable, I recommend MDAC 2.6 or 2.7
	Set objStream = Server.CreateObject("ADODB.Stream")
	objStream.Open
	objStream.Type = adTypeBinary
	objStream.LoadFromFile  Server.MapPath(strPathFile)
	
	'-- Inline表示在瀏覽器內開啟文件,attachment 表示以附件方式開啟文件
	'-- 如果你的檔名有中文,Server.UrlPathEncode 請勿省略(IE瀏覽器有些版本會怪怪的)
	Response.AddHeader "Content-Disposition", "attachment; filename=" & Server.UrlPathEncode(strFileName)
	
	'Response.AddHeader "Content-Length", strFileSize
	
	'-- 開啟檔案的格式
	Response.ContentType ="application/vnd.ms-excel"
	
	'-- 檔案編碼的格式
	Response.Charset = "UTF-8"
	
	Response.BinaryWrite objStream.Read
	
	objStream.Close
	Set objStream = Nothing
	
	Response.Flush
	RESPONSE.END
	'======================================================
	
	'RESPONSE.WRITE 1 & "<BR>"
	'RESPONSE.WRITE strFileType & "<BR>"
	'RESPONSE.WRITE ContentType & "<BR>"
	'RESPONSE.WRITE 2



	' ' strFileType = LCase(Right(strFileName, 4))
	' ' Select Case strFileType
' ' '		Case ".asf"
' ' '		    ContentType = "video/x-ms-asf"
' ' '		Case ".avi"
' ' '		    ContentType = "video/avi"
' ' '		Case ".doc"
' ' '		    ContentType = "application/msword"
' ' '		Case ".zip"
' ' '		    ContentType = "application/zip"
		' ' Case ".xls"
		    ' ' ContentType = "application/vnd.ms-excel"
' ' '		Case ".gif"
' ' '		    ContentType = "image/gif"
' ' '		Case ".jpg", "jpeg"
' ' '		    ContentType = "image/jpeg"
' ' '		Case ".pdf"
' ' '		    ContentType = "application/pdf"
' ' '		Case ".wav"
' ' '		    ContentType = "audio/wav"
' ' '		Case ".mp3"
' ' '		    ContentType = "audio/mpeg3"
' ' '		Case ".mpg", "mpeg"
' ' '		    ContentType = "video/mpeg"
' ' '		Case ".rtf"
' ' '		    ContentType = "application/rtf"
' ' '		Case ".htm", "html"
' ' '		    ContentType = "text/html"
' ' '		Case ".asp"
' ' '		    ContentType = "text/asp"
' ' '		Case Else
' ' '		    'Handle All Other Files
' ' '		    ContentType = "application/octet-stream"
	' ' End Select	
	
%>
