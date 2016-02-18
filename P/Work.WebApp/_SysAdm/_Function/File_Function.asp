<%


'----------------------------------------------------------
'-- 複製檔案(單一檔案)
'-- JACK HSIEH 20061109
Function CopyFile(SPathFile,DPathFile)
Dim oFSO
Dim Folder
Dim strSPath,strDPath
Dim intPos
	
	intPos=InStrRev(SPathFile,"\")
	strSPath=Left(SPathFile,intPos-1)

	intPos=InStrRev(DPathFile,"\")
	strDPath=Left(DPathFile,intPos-1)

	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")	
		
	If ExistsPath(strSPath,0)=true Then
		Set Folder=oFSO.GetFolder(strSPath)
		'-- 呼叫 CreateFolder 
		CreateFolder(strDPath) 
		
		'-- 把檔案複製過去
		oFSO.CopyFile SPathFile,DPathFile,True
		Set Folder=Nothing	
	End If
	
	Set oFSO=Nothing
End Function
'==========================================================

'----------------------------------------------------------
'-- 複製資料夾(包含子目錄)
'-- JACK HSIEH 20040608
Function CopyFolder(sPath,dPath)
Dim oFSO
Dim Folder
	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")	
	If existsPath(sPath,0)=true Then
		Set Folder=oFSO.GetFolder(sPath)
		createFolder(getParentPath(dPath)) 
		Folder.copy (dPath) ,1
		Set Folder=Nothing	
	End IF
	Set oFSO=Nothing
End Function
'==========================================================
'----------------------------------------------------------
'-- 上傳檔案檔案
Function FileUpload(aPath)
Dim strItemstr
Dim xUpload
Dim i,j
Dim strPath
Dim strName

	Set xUpload=Server.CreateObject("XionSoft.XionFileUpload.1")
	
	'-- 建立Folder
	CreateFolder aPath
	For i=1 to xUpload.ItemCount
		strName=xUpload.ItemName(i)
		If Left(strName,4)="file" Then
			strItemStr=strItemStr & strName & ","
		End If
	Next
	For i=1 to xUpload.FileCount
	j="file"&cstr(i)
	If Instr(strItemstr,j&",")=0 Then
		If xUpload.FileSize(j)>0 Then
			strPath=aPath & "\" & xUpload.FileName(j)
			xUpload.SaveFile j,strPath
		End If
	End If
	Next
	i=xUpload.FileCount		
	Set xUpload=Nothing
	
	fileUpload=i

End Function
'==========================================================
'----------------------------------------------------------
'-- 檢查是否存在路徑或檔案
Function ExistsPath(aPath,fType)
Dim oFSO
Dim iReturn
	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")	
	Select Case fType	
	Case 0	'Folder 
		iReturn=oFSO.FolderExists(aPath)
	Case 1	'File
		iReturn=oFSO.FileExists(aPath)
	End Select
	
	Set oFSO=Nothing
	ExistsPath=iReturn

End Function
'==========================================================
'----------------------------------------------------------
'-- 取得上層檔案路徑
Function GetParentPath(strPath)
Dim strTPath
	strTPath=""
	If Len(strPath)>0 Then
		If Right(strPath,1)="\" Then 	strPath=Left(strPath,Len(strPath)-1)
		If InStrRev(strPath,"\")>0 Then	strTPath=Left(strPath,InStrRev(strPath,"\")-1)
	End IF
	GetParentPath=strTPath
End Function
'==========================================================
'----------------------------------------------------------
'-- 建立路徑
Sub CreateFolder(aPath)
Dim oFSO
Dim strTPath
	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")
	If oFSO.FolderExists(aPath)=false Then
		strTPath=getParentPath(aPath)
		If oFSO.FolderExists(strTPath)=false  Then	createFolder strTPath
		oFSO.CreateFolder aPath
	End IF
	Set oFSO=Nothing
End Sub
'==========================================================
'----------------------------------------------------------
'-- 刪除路徑或檔案
Sub DeletePath(aPath,fType)
Dim oFSO
	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")
	Select Case fType
	Case 0	'Folder
		If oFSO.FolderExists(aPath)=true Then oFSO.DeleteFolder aPath
	Case 1	'File
		If oFSO.FileExists(aPath)=true Then oFSO.DeleteFile aPath
	End Select
	Set oFSO=Nothing
End Sub
'==========================================================

'----------------------------------------------------------
Function FileToOption(aPath)
Dim oFSO
Dim Folder
Dim Files
Dim File
Dim strOption
Dim strSize
Dim strDate
	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")	
	If ExistsPath(aPath,0)=true Then
		Set Folder=oFSO.GetFolder(aPath)
		Set Files=Folder.Files
		For Each File in Files
			strDate="[" & FormatDateTime(File.DateLastModified,2) & " " & FormatDateTime(File.DateLastModified,4) & "]"
			strSize="(" & Cstr(FormatNumber(File.Size/1024,2)) & "KB)"
			strOption=strOption & "<option value=""" & File.Name & """>" & File.Name & strSize & strDate
		Next
		Set File=Nothing
		Set Files=Nothing
		Set Folder=Nothing
	end if
	Set oFSO=Nothing
	FileToOption=strOption
End Function
'==========================================================


'-- 檔案列表顯示出來
Function FileToList(aPath,Path)
Dim oFSO
Dim Folder
Dim Files
Dim File
Dim strOption
Dim strSize
Dim strDate
Dim strFileExtName
	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")	
	If ExistsPath(aPath,0)=true Then
		Set Folder=oFSO.GetFolder(aPath)
		Set Files=Folder.Files
		For Each File in Files
			strFileExtName=Right(File.Name,3)
			Select Case strFileExtName
				Case "JPG","GIF","BMP"
					strOption=strOption & "<a href=""" & path &  File.Name & """ Target=new>"&File.Name&" </a><br>"		
				Case "DOC","XLS","PPT","PDF"
					strOption=strOption & "<a href=""" & path &  File.Name & """ Target=new>"&File.Name&" </a><br>"		
			End Select	
			'strOption=strOption & "　<a href=""javascript:opNewWin('" & path &  File.Name & "','imageviewer','300px','300px')"">"&File.Name&"</a>　<br>"

		Next
		Set File=Nothing
		Set Files=Nothing
		Set Folder=Nothing
	End if
	Set oFSO=Nothing
	FileToList=strOption
End Function




'----------------------------------------------------------
Function FileToPhoto(aPath,Path,Imgwth)
Dim oFSO
Dim Folder
Dim Files
Dim File
Dim strOption
Dim strSize
Dim strDate
Dim strExt
	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")	
	If ExistsPath(aPath,0)=true Then
		Set Folder=oFSO.GetFolder(aPath)
		Set Files=Folder.Files
		For Each File in Files
			strExt=UCase(Right(File.Name,3))
			If strExt="JPG" or strExt="GIF" or strExt="BMP" Then	
				'strOption=strOption & "<a href=""javascript:openWin('" & path &  File.Name & "','imageviewer','300px','300px')"">"&"<img border=0 alt="&File.Name&" width=" &Imgwth &" src=""" & path &  File.Name & """></a><br>"
				If Imgwth="" Or Imgwth=0 Then
					strOption=strOption & "<a href=""" & path &  File.Name & """Target=new>"&"<img border=0 alt="&File.Name&" src=""" & path &  File.Name & """></a><br>"
				Else
					strOption=strOption & "<a href=""" & path &  File.Name & """Target=new>"&"<img border=0 alt="&File.Name&" width=" &Imgwth &" src=""" & path &  File.Name & """></a><br>"
				End If
			End If
		Next
		Set File=Nothing
		Set Files=Nothing
		Set Folder=Nothing
	End if
	Set oFSO=Nothing
	FileToPhoto=strOption
End Function
'==========================================================

'----------------------------------------------------------
Function FileToList_Dialog(aPath,Path)
Dim oFSO
Dim Folder
Dim Files
Dim File
Dim strOption
Dim strSize
Dim strDate
Dim strFileExtName
	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")	
	If ExistsPath(aPath,0)=true Then
		Set Folder=oFSO.GetFolder(aPath)
		Set Files=Folder.Files
		For Each File in Files
			strFileExtName=UCASE(Right(File.Name,3))
			Select Case strFileExtName
				Case "JPG","GIF","BMP"
					strOption=strOption & "<a href='#' onclick=""content1('"&path &  File.Name&"');""> "&File.Name&" </a>"		
				Case "DOC","XLS","PPT","PDF"
					strOption=strOption & "<a href='#' onclick=""content1('"&path &  File.Name&"');""> "&File.Name&" </a>"		
			End Select	
			'strOption=strOption & "　<a href=""javascript:opNewWin('" & path &  File.Name & "','imageviewer','300px','300px')"">"&File.Name&"</a>　<br>"

		Next
		Set File=Nothing
		Set Files=Nothing
		Set Folder=Nothing
	End if
	Set oFSO=Nothing
	FileToList_dialog=strOption
End Function
'==========================================================

Function FileToList_New(aPath,Path)
Dim oFSO
Dim Folder
Dim Files
Dim File
Dim strOption
Dim strSize
Dim strDate
Dim strExt
Dim i
i=0

	'response.write aPath
	'response.end
	
	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")	
	If ExistsPath(aPath,0)=true Then
		Set Folder=oFSO.GetFolder(aPath)
		Set Files=Folder.Files
		For Each File in Files
			strExt=Right(File.Name,3)
			'strOption=strOption & "　<a href=""javascript:opNewWin('" & path &  File.Name & "','imageviewer','300px','300px')"">"&File.Name&"</a>　<br>"
			i=i+1
			If i>15 then
			strOption=strOption & ""
			i=0
			else
			strOption=strOption & "<a href=""" & path &  File.Name & """ Target=new>"&File.Name&" </a></BR>"
			End If
		Next
		Set File=Nothing
		Set Files=Nothing
		Set Folder=Nothing
	End if
	Set oFSO=Nothing
	FileToList_new=strOption
End Function
'==========================================================



Function GetPicPath(strPath,intPK,strFlag)
Dim oFSO
Dim Folder
Dim Files
Dim File
Dim strExt
Dim aPath,bPath
Dim strPicPath
	
	aPath=strPath&intPK&"/photo"&strFlag

	bPath=SERVER.MapPath(aPath)

	
	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")	
	If existsPath(bPath,0)=true Then
	
		Set Folder=oFSO.GetFolder(bPath)
		Set Files=Folder.Files
		For Each File in Files
			strExt=Right(File.Name,3)
			If strExt="jpg" or strExt="gif" or strExt="JPG" or strExt="GIF" Then	
				strPicPath=aPath & "/" & File.Name
			End If
		Next
		Set File=Nothing
		Set Files=Nothing
		Set Folder=Nothing
	end if
	Set oFSO=Nothing
	
	If strPicPath="" Then 
		RESPONSE.WRITE bPath & "<BR>"
		'strPicPath="../../Images/None.gif"
	End If
		
	GetPicPath=strPicPath
End Function


'-- Jack Add 20080522 -------------------------------------
'-- 根據指定的流水號，取得檔案路徑檔名
Function GetFilePath(strPath,intPK,intFileType,strFileFolder,strTempImage)
Dim oFSO
Dim Folder
Dim Files
Dim File
Dim strFileExtName
Dim aPath,bPath
Dim strPicPath
Dim StrFileType
	
	Select Case intFileType
	Case 1
		StrFileType="File"
	Case 2
		StrFileType="Photo"
	Case 3
		StrFileType="Moive"
	End Select

	aPath=strPath&intPK&"/"&strFileType & strFileFolder
	bPath=SERVER.MapPath(aPath)

''	RESPONSE.WRITE aPath & "<BR>"
''	RESPONSE.WRITE bPath & "<BR>"
''	RESPONSE.END
	
	
	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")	
	If existsPath(bPath,0)=true Then
	
		Set Folder=oFSO.GetFolder(bPath)
		Set Files=Folder.Files
		For Each File in Files
			strFileExtName=Right(File.Name,3)
			If UCase(strFileExtName)="JPG" or UCase(strFileExtName)="GIF" OR UCase(strFileExtName)="BMP"  or UCase(strFileExtName)="WMV" or UCase(strFileExtName)="MPG" Then	
				strPicPath=aPath & "/" & File.Name
			End If
		Next
		Set File=Nothing
		Set Files=Nothing
		Set Folder=Nothing
	end if
	Set oFSO=Nothing
	
	'RESPONSE.WRITE strPicPath
	'RESPONSE.END
	
	If strPicPath="" Then 
		'RESPONSE.WRITE bPath & "<BR>"
		If strTempImage ="" Then
			strPicPath="../Images/None.gif"
		Else
			strPicPath=strTempImage
		End If
	End If
		
	GetFilePath=strPicPath
End Function
'##########################################################


'==========================================================
''Jack 20101019 Mark
'''----------------------------------------------------------
''Function FileToList(aPath,Path)
''Dim oFSO
''Dim Folder
''Dim Files
''Dim File
''Dim strOption
''Dim strSize
''Dim strDate
''Dim strExt
''Dim i
''i=0
''
''	'response.write aPath
''	'response.end
''	
''	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")	
''	If ExistsPath(aPath,0)=true Then
''		Set Folder=oFSO.GetFolder(aPath)
''		Set Files=Folder.Files
''		For Each File in Files
''			strExt=Right(File.Name,3)
''			'strOption=strOption & "　<a href=""javascript:opNewWin('" & path &  File.Name & "','imageviewer','300px','300px')"">"&File.Name&"</a>　<br>"
''			i=i+1
''			If i>15 then
''			strOption=strOption & "<tr>"
''			i=0
''			else
''			strOption=strOption & "<td><a href=""" & path &  File.Name & """ Target=new>"&File.Name&" </a></td>"
''			End If
''		Next
''		Set File=Nothing
''		Set Files=Nothing
''		Set Folder=Nothing
''	End if
''	Set oFSO=Nothing
''	FileToList=strOption
''End Function
'''==========================================================


'''=========================
'''從此路徑下載檔案
'''=========================
''Sub fileDownload(aPath)
''Dim xUpload
'' If existsPath(aPath,1)=true then
'' 	'response.write aPath
'' 	'response.end
'' 	Set xUpload = Server.CreateObject("XionSoft.XionFileDownload.1")
'' 	xUpload.DownFromFile apath
'' 	Set xUpload=Nothing
'' end if
''End Sub
''Sub fileDownloadX(aPath)
''Dim oSmartUpload
'' If existsPath(aPath,1)=true then
'' 	Set oSmartUpload =  Server.CreateObject("UpDownExpress.FileDownload")
'' 	oSmartUpload.DownFromFile apath
'' 	Set oSmartUpload=Nothing
'' end if
''End Sub
''
''
''
'''==============================================================
''Function fileUploadX(aPath)
''Dim oSmartUpload
''Dim xattach
''Dim intCount
'' Set oSmartUpload =  Server.CreateObject("UpDownExpress.FileUpload")
'' intCount=oSmartUpload.Attachments.count
'' If intCount>0 Then
''	 createFolder aPath
''	 For Each xattach in oSmartUpload.Attachments 	
''	 	xattach.SaveFile aPath & "\" & xattach.FileName
''	 Next
'' End If
''
'' Set oSmartUpload=Nothing
'' fileUpload=intCount
''End Function
''
''
''
''Function vbFileToList(aPath,Path)
''Dim oFSO
''Dim Folder
''Dim Files
''Dim File
''Dim strOption
''Dim strSize
''Dim strDate
''Dim strExt
''	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")
''	createFolder(aPath)
''	If existsPath(aPath,0)=true Then
''		Set Folder=oFSO.GetFolder(aPath)
''		Set Files=Folder.Files
''		For Each File in Files
''			strOption=strOption & File.Name & ","			
''		Next
''		if len(strOption)>0 then strOption=left(strOption,len(strOption)-1)
''		Set File=Nothing
''		Set Files=Nothing
''		Set Folder=Nothing
''	end if
''	
''	Set oFSO=Nothing
''	
''	vbFileToList=strOption
''End Function
''
''Function MovieToList(aPath,Path)
''Dim oFSO
''Dim Folder
''Dim Files
''Dim File
''Dim strOption
''Dim strSize
''Dim strDate
''Dim strExt
''	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")	
''	If existsPath(aPath,0)=true Then
''		Set Folder=oFSO.GetFolder(aPath)
''		Set Files=Folder.Files
''		For Each File in Files
''			strExt=Right(File.Name,3)
''				strOption=strOption & "<a style=cursor:hand Onclick=""opNewWin('ShowMovie.asp?path=" & path &  File.Name & "','Movie','300','10px')"">" _
''				& File.Name&" </a><br>"			
''			
''		Next
''		Set File=Nothing
''		Set Files=Nothing
''		Set Folder=Nothing
''	end if
''	
''	Set oFSO=Nothing
''	
''	MovieToList=strOption
''End Function
''

''
''Function HtmlToShow(aPath,Path)
''Dim oFSO
''Dim Folder
''Dim Files
''Dim File
''Dim strOption
''Dim strSize
''Dim strDate
''Dim strExt
''	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")	
''	If existsPath(aPath,0)=true Then
''		Set Folder=oFSO.GetFolder(aPath)
''		Set Files=Folder.Files
''		For Each File in Files
''			strExt=Right(File.Name,3)
''			If strExt="htm" Then	
''			strOption=strOption & "<iframe frameborder=0 scrolling=yes width=100% height=400" 
''			strOption=strOption & " src="""& path &  File.Name &"""></iframe>"
''			End If
''		Next
''		Set File=Nothing
''		Set Files=Nothing
''		Set Folder=Nothing
''	end if
''	Set oFSO=Nothing
''	
''	HtmlToShow=strOption
''End Function
''
''
''Function HtmlToList(aPath,Path)
''Dim oFSO
''Dim Folder
''Dim Files
''Dim File
''Dim strOption
''Dim strSize
''Dim strDate
''Dim strExt
''	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")	
''	If existsPath(aPath,0)=true Then
''		Set Folder=oFSO.GetFolder(aPath)
''		Set Files=Folder.Files
''		For Each File in Files
''			strExt=Right(File.Name,3)
''			If strExt="htm" Then	
''				strOption=strOption & "<a href=""javascript:openWin('" & path &  File.Name & "','imageviewer','500px','500px')"">" _
''				& File.Name&" </a><br>"
''			end if
''		Next
''		Set File=Nothing
''		Set Files=Nothing
''		Set Folder=Nothing
''	end if
''	Set oFSO=Nothing
''	
''	HtmlToList=strOption
''End Function
''Function FileToLists(id,aPath,LPath,path)
''
''Dim oFSO
''Dim Folder
''Dim Files
''Dim File
''Dim strOption
''Dim strSize
''Dim strDate
''Dim strExt
''	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")	
''	If existsPath(aPath,0)=true Then
''		Set Folder=oFSO.GetFolder(aPath)
''		Set Files=Folder.Files
''		
''		
''		For Each File in Files
''			strExt=Right(File.Name,3)
''			If strExt<>"JPG" and strExt<>"GIF" and strExt<>"jpg" and strExt <> "gif" Then
''			strDate="[" & FormatDateTime(File.DateLastModified,2) & " " & FormatDateTime(File.DateLastModified,4) & "]"
''			strSize="(" & Cstr(FormatNumber(File.Size/1024,2)) & "KB)"
''			strOption=strOption & "<a href="& Lpath &""& File.Name &" Target=_blank>"&File.Name & "</a><br><a href=""../../_include/download.asp?id=" & id & "&file=" & File.Name &"&FNAME="& path & """> <font color=red>下載→</font>"  &  strSize & "</a><br>"
''			End If
''		Next
''		Set File=Nothing
''		Set Files=Nothing
''		Set Folder=Nothing
''	end if
''	Set oFSO=Nothing
''	
''	FileToLists=strOption
''End Function



%>