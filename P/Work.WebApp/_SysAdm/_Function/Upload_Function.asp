<%
'----------------------------------------------------------
'-- �ƻs��Ƨ�(�]�t�l�ؿ�)
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

''-- �o�@�ӼȮɤ��� JACK MARKED 20080616-------------------
'''--------------------------------------------------------
'''-- �W���ɮ��ɮ�
'''-- 20080601 JACK FIXed
'''-- �W�[�ɮפW�Ǥj�p����
''Function FileUpload(aPath)
''Dim strItemstr
''Dim xUpload
''Dim i,j
''Dim strPath
''Dim strName
''Dim intFileLimitSize
''Dim intFileSize
''
''	'-- �]�w�ɮפj�p �w�]�O 3 MB
''	intFileLimitSize=3000 ' * 1024 KB
''	
''	'-- �p�G�O IIS6.0 �X�{ "�n�D���� ���~ 'ASP 0104 : 80020009' " ��ܤW���ɮפӤj�n�� IIS6.0 �t���ɮ׭����j
''	Set xUpload=Server.CreateObject("XionSoft.XionFileUpload.1")
''	
''	'-- �إ�Folder
''	CreateFolder aPath
''
''	For i=1 to xUpload.FileCount
''		j="file"&cstr(i)
''		
''		'-- ���o�ɮפj�p (KB)		
''		intFileSize=CInt(xUpload.FileSize(j)/1024)
''	
''		'-- �ˬd�ɮפj�p�O�_OK,OK�~�ɮצs��
''		If intFileSize > 0  AND  intFileSize < intFileLimitSize Then
''			xUpload.SaveFile j,aPath & "\" & xUpload.FileName(j)
''		End If
''		
''	Next
''	fileUpload=xUpload.FileCount		
''	Set xUpload=Nothing
''End Function
'''==========================================================

'----------------------------------------------------------
'-- �W���ɮ��ɮ�
'-- 20080601 JACK ADDED
'-- �ϥ� Dundas.Upload.2 �W�Ǥ���
'-- �W�[�ɮפW�Ǥj�p����
'-- �W�[�ɮפj��p
'-- �j�ɤW�ǳt�׮Ĳv����
Function FileUpload_Dundas(APath,intImg0W,intImg0H,ByRef AryImgResize,p0)
'Function FileUpload_Dundas(APath,intImg0W,intImg0H,intImg1W,intImg1H,intImg2W,intImg2H,intImg3W,intImg3H,IntImgConvertFlag)
Dim objUpload				
Dim strTempPath,strRootPath,strSizeAPath,strSizeBPath,strSizeCPath
Dim strFileName,strFileExtName
Dim UPFile
Dim intImg1W,intImg1H,intImg2W,intImg2H,intImg3W,intImg3H
Dim IntImgConvertFlag

	intImg1W=Cint(Trim(AryImgResize(0)))
	intImg1H=Cint(Trim(AryImgResize(1)))
	intImg2W=Cint(Trim(AryImgResize(2)))
	intImg2H=Cint(Trim(AryImgResize(3)))
	intImg3W=Cint(Trim(AryImgResize(4)))
	intImg3H=Cint(Trim(AryImgResize(5)))
	'RESPONSE.WRITE intImg1W & "," & intImg1H & "," & intImg2W & "," & intImg2H & "," & intImg3W & "," & intImg3H 
	'RESPONSE.END
	
	'-- �p�G �[�`=0 ��ܤ�������
	If intImg1W+intImg1H+intImg2W+intImg2H+intImg3W+intImg3H = 0 Then
		IntImgConvertFlag = 0
	Else
		IntImgConvertFlag = 1
	End If
			

	'-- �]�w�ɮפW�� 5 ����
	Server.ScriptTimeout = 300
	'On Error Resume Next
	
	'-- �ŧi����
	Set objUpload = server.CreateObject("Dundas.Upload.2")

	'-- �]�w�W���ɮת��j�p(��@�ɮ� �� �P���h��)
	objUpload.MaxFileCount =-1
	objUpload.MaxFileSize = 10 *1024*1024
	objUpload.MaxUploadSize= -1
	
	'-- �]�w NextFile.FileName ���W�٭n���n�[�JUniqueName (�ߤ@�ɦW,����{7A50CA5E-7D64-44BC-BD22-A0FDF254A0DA})
	objUpload.UseUniqueNames = False	

	'-- �]�w�ɮצs���m
	strRootPath=aPath
	strTempPath=aPath & "\_Temp"
	strSizeAPath=aPath & "\A"
	strSizeBPath=aPath & "\B"
	strSizeCPath=aPath & "\C"
	
	'-- �إߦs��ؿ�
	objUpload.DirectoryCreate strRootPath
	
	'-- �]�w�W���ɮ��x�s����w��Ƨ�
	objUpload.Save strRootPath
	
	'-- �]�w�W���ɮ��x�s��O����(�o�@�Ӥ����p���)
	'objUpload.SaveToMemory	
	
	'-- ��ܫD�Ϥ��A�������ɡA�����s��-----------
	'-- �s�ɧ�������A���񪫥�A���}�C
	If IntImgConvertFlag = 0 Then

		'-- ���񪫥�-----------------------------
		Set objUpload = Nothing	
		
		'-- ���}�{��
		Exit Function
	End If	'====================================
		
		
	'-- �̧ǳB�̤W���ɮ�-------------------------
	For Each UPFile In objUpload.Files
		'-- ���o�ɮצW��			
		strFileName=objUpload.GetFileName(UPFile.Path) 
		'-- ���o�ɮװ��ɦW	
		strFileExtName=UCase(objUpload.GetFileExt(UPFile.Path))	

		'-- �P�_�O�_���i�ഫ�����ɮ榡------
		If strFileExtName="JPG" or strFileExtName="GIF" or strFileExtName="BMP" Then
			'RESPONSE.WRITE intImg1W & "," & intImg1H & "," & intImg2W & "," & intImg2H & "," & intImg3W & "," & intImg3H 
	
			'-- �إ��ɮ׼Ȯɦs��ؿ�
			objUpload.DirectoryCreate strTempPath

			'-- �]�w�ɮק�����|�W��
			strTempPath=strTempPath & "\" & strFileName			
			
			'-- �]�w�W���ɮײ�����w��Ƨ�(TEMP)
			UPFile.Move strTempPath,False

			
			'--�إߤW���ɮ�ASP�Ϊ��p��(�e�שT�w)
			If (intImg0W + intImg0H) > 0 Then
				strRootPath=strRootPath & "\" & strFileName
				Call ImageChange (strTempPath,strRootPath,strFileExtName,intImg0W,intImg0H)
			End If
			
			'-- 1�̻ݨD�إߥؿ��A2�]�w�ɮק�����|�W�١A3���ɮ���p�P�ɦs��
			'-- A��
			If (intImg1W + intImg1H) > 0 Then
				objUpload.DirectoryCreate strSizeAPath
				strSizeAPath=strSizeAPath & "\" & strFileName
				Call ImageChange (strTempPath,strSizeAPath,strFileExtName,intImg1W,intImg1H)
			End If
			
			'-- B��
			If (intImg2W + intImg2H) > 0 Then 
				objUpload.DirectoryCreate strSizeBPath
				strSizeBPath=strSizeBPath & "\" & strFileName
				Call ImageChange (strTempPath,strSizeBPath,strFileExtName,intImg2W,intImg2H)
			End If
			
			'-- C��
			If (intImg3W + intImg3H) > 0 Then
				objUpload.DirectoryCreate strSizeCPath	
				strSizeCPath=strSizeCPath & "\" & strFileName
				Call ImageChange (strTempPath,strSizeCPath,strFileExtName,intImg3W,intImg3H)
			End If
			

		Else
			'-- �p�G���ŦX���ɮ榡�A�N�����쥻�s�ɾ���
			'--	�]���ɮפw�g�s��(strRootPath)�A�o�̤�������ʰ�
			
					
		End If'==============================	
	
		
	
						
	Next'========================================

	
	'��s��Ʈw���
	'Dim StrSql,oRs
	'	StrSql="Select * From ���򥻸�� where ���򥻸��.�y����='"&p0&"'"
	'		Set oRs=GetRST(StrSql,3,1,2)
	'		oRs("�Ӥ�")=strFileName
	'		oRs("img")= objUpload.Files(0).Binary
	'	oRs.Update
		
	'-- ���񪫥�---------------------------------
	Set objUpload = Nothing	
	'============================================
	

End Function
'##########################################################






'----------------------------------------------------------
'-- �ˬd�O�_�s�b���|���ɮ�
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
'-- ���o�W�h�ɮ׸��|
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
'-- �إ߸��|
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
'-- �R�����|���ɮ�
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
'-- �ɮצC����Select Option �̭�
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
			'strSize="(" & Cstr(FormatNumber(File.Size/1024,2)) & "KB)"
			strSize="(" & GetImageWHS(aPath&"/"&File.Name,1)(2) & "KB)"
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

'----------------------------------------------------------
'-- �ɮצC����ܥX��
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
			'strOption=strOption & "�@<a href=""javascript:opNewWin('" & path &  File.Name & "','imageviewer','300px','300px')"">"&File.Name&"</a>�@<br>"
			strOption=strOption & "<a href=""" & path &  File.Name & """ Target=new>"&File.Name&" </a><br>"			
		Next
		Set File=Nothing
		Set Files=Nothing
		Set Folder=Nothing
	End if
	Set oFSO=Nothing
	FileToList=strOption
End Function
'==========================================================


'----------------------------------------------------------
'-- �ɮ�(�Ϥ�)�C����ܥX��
Function FileToPhoto(aPath,Path,Imgwth)
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
			strFileExtName=UCase(Right(File.Name,3))
			If strFileExtName="JPG" or strFileExtName="GIF" or strFileExtName="BMP" Then	
				'strOption=strOption & "<a href=""javascript:openWin('" & path &  File.Name & "','imageviewer','300px','300px')"">"&"<img border=0 alt="&File.Name&" width=" &Imgwth &" src=""" & path &  File.Name & """></a><br>"
				
				If Imgwth="" Or Imgwth=0 Then
					strOption=strOption & "<a href=""" & path &  File.Name & """Target=new>"&"<img border=0 alt="&File.Name&" src=""" & path &  File.Name & """></a><br>"
				Else
					strOption=strOption & "<a href=""" & path &  File.Name & """Target=new>"&"<img border=0 alt="&File.Name&" width=" & Imgwth &" src=""" & path &  File.Name & """></a><br>"
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










'-- Jack Add 20080522 -------------------------------------
'-- �ɮ�(�Ϥ�)�C����ܥX��
Function FileToPhoto_News(aPath,Path,ImgWth,ImgHht)
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
			strFileExtName=UCase(Right(File.Name,3))
			If strFileExtName="JPG" or strFileExtName="GIF" or strFileExtName="BMP" Then	
				strOption=strOption & "<img src=""" & path &  File.Name & """  border=0 alt="&File.Name&" width=" &Imgwth &" height=" &ImgHht&" border=""0"" class=""thumb_img"" rel=""" & path &  File.Name & """ >"
			End If
		Next
		Set File=Nothing
		Set Files=Nothing
		Set Folder=Nothing
	End if
	Set oFSO=Nothing
	FileToPhoto_News=strOption
End Function
'##########################################################
















'-- Jack Add 20080522 -------------------------------------
'-- ���o�ɮ׼�
Function GetFileCounts(strPath,intPK,intFileType,strFileFolder)
Dim oFSO
Dim Folder
Dim Files
Dim File
Dim strFileExtName
Dim aPath,bPath
Dim strPicPath
Dim StrFileType
Dim intFileCounts
	
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

	'RESPONSE.WRITE aPath & "<BR>"
	'RESPONSE.WRITE bPath & "<BR>"
	'RESPONSE.END
	
	
	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")	
	If existsPath(bPath,0)=true Then
	
		Set Folder=oFSO.GetFolder(bPath)
		Set Files=Folder.Files
		intFileCounts=0
		For Each File in Files
			strFileExtName=Right(File.Name,3)
			If UCase(strFileExtName)="JPG" or UCase(strFileExtName)="GIF" or UCase(strFileExtName)="WMV" or UCase(strFileExtName)="MPG" Then	
				intFileCounts=intFileCounts+1
			End If
		Next
		Set File=Nothing
		Set Files=Nothing
		Set Folder=Nothing
	end if
	Set oFSO=Nothing
	GetFileCounts=intFileCounts
End Function
'##########################################################


'-- Jack Add 20080522 -------------------------------------
'-- �ھګ��w���y�����A���o�ɮ׸��|�ɦW
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



'-- Jack Add 20080522 -------------------------------------
'-- ���o�Ϥ����e,��,�j�p-----------------------------------
Function GetImageWHS(varImgPath,varFlag)
'-- ���oImage Width,Height,Size
Dim ObjImg
Dim aryInfo(2)

	'RESPONSE.WRITE varImgPath
	
	Set ObjImg = New S_ImageInfo  
	If varFlag=1 Then
		'-- ������|
		aryInfo(0) = ObjImg.ImageWidth(varImgPath)  
		aryInfo(1) = ObjImg.ImageHeight(varImgPath) 
	  	aryInfo(2) = ObjImg.ImageSize(varImgPath)
	ElseIf varFlag=2 Then
		'-- �۹���|
		aryInfo(0) = ObjImg.ImageWidth(Server.Mappath(varImgPath))  
		aryInfo(1) = ObjImg.ImageHeight(Server.Mappath(varImgPath)) 
	  	aryInfo(2) = ObjImg.ImageSize(Server.Mappath(varImgPath)) 
	End If 
	Set ObjImg = Nothing 
	GetImageWHS=aryInfo

End Function
'##########################################################

'-- JACK FIXed 20071101 (�HTry�L�i�H��)--------------------
'-- �إߨ��o�v����ƪ�CLASS����
'-- �A�ι�H "gif","bmp","jpg","png"
Class S_ImageInfo 
    Dim ASO
    Private Sub Class_Initialize
        Set ASO=Server.CreateObject("ADODB.Stream")
        ASO.Mode=3
        ASO.Type=1
        ASO.Open
    End Sub
    Private Sub Class_Terminate
        Err.Clear
        Set ASO=Nothing
    End Sub 
    
    
   	Public Function ImageWidth(IMGPath)
        Dim FSO,IMGFile,FileExt,Arr
        Set FSO = Server.CreateObject("Scripting.FileSystemObject") 
        If (FSO.FileExists(IMGPath)) Then 
            Set IMGFile = FSO.GetFile(IMGPath) 
            FileExt=FSO.GetExtensionName(IMGPath) 
            Select Case LCase(FileExt)
                Case "gif","bmp","jpg","png": 
                	Arr=GetImageHead(IMGFile.Path) 
                	ImageWidth = Arr(1) 
            End Select 
            Set IMGFile=Nothing 
        Else
            ImageWidth = 0
        End If     
        Set FSO=Nothing 
    End Function 
     
    Public Function ImageHeight(IMGPath)
        Dim FSO,IMGFile,FileExt,Arr
        Set FSO = server.CreateObject("Scripting.FileSystemObject") 
        If (FSO.FileExists(IMGPath)) Then 
            Set IMGFile = FSO.GetFile(IMGPath) 
            FileExt=FSO.GetExtensionName(IMGPath) 
            Select Case LCase(FileExt) 
                Case "gif","bmp","jpg","png": 
                Arr=GetImageHead(IMGFile.Path) 
                ImageHeight = Arr(2) 
            End Select 
            Set IMGFile=Nothing 
        Else
            ImageHeight = 0 
        End If     
        Set FSO=Nothing 
    End Function 
    
    Public Function ImageSize(IMGPath)
        Dim FSO,IMGFile,FileExt,Arr
        Set FSO = Server.CreateObject("Scripting.FileSystemObject") 
        If (FSO.FileExists(IMGPath)) Then 
            Set IMGFile = FSO.GetFile(IMGPath) 
            FileExt=FSO.GetExtensionName(IMGPath) 
            Select Case LCase(FileExt)
                Case "gif","bmp","jpg","png": 
                	ImageSize = FormatNumber(IMGFile.Size/1024,2)
            End Select 
            Set IMGFile=Nothing 
        Else
            ImageSize = 0
        End If     
        Set FSO=Nothing 
    End Function 
    
    
    Private Function GetImageHead(varFilePath)
        Dim bFlag,p1
        Dim Ret(3) 
        ASO.LoadFromFile(varFilePath) 
        bFlag=ASO.Read(3) 
        
        Select Case Hex(binVal(bFlag)) 
        Case "4E5089": 
            ASO.Read(15) 
            ret(0)="PNG" 
            ret(1)=BinVal2(ASO.Read(2)) 
            ASO.Read(2) 
            ret(2)=BinVal2(ASO.Read(2)) 
        Case "464947": 
            ASO.read(3) 
            ret(0)="gif" 
            ret(1)=BinVal(ASO.Read(2)) 
            ret(2)=BinVal(ASO.Read(2)) 
        Case "535746": 
            ASO.read(5) 
            binData=ASO.Read(1) 
            sConv=Num2Str(ascb(binData),2 ,8) 
            nBits=Str2Num(left(sConv,5),2) 
            sConv=mid(sConv,6) 
            While(len(sConv)<nBits*4) 
                binData=ASO.Read(1) 
                sConv=sConv&Num2Str(AscB(binData),2 ,8) 
            Wend 
            ret(0)="SWF" 
            ret(1)=Int(Abs(Str2Num(Mid(sConv,1*nBits+1,nBits),2)-Str2Num(Mid(sConv,0*nBits+1,nBits),2))/20) 
            ret(2)=Int(Abs(Str2Num(Mid(sConv,3*nBits+1,nBits),2)-Str2Num(Mid(sConv,2*nBits+1,nBits),2))/20) 
        Case "FFD8FF": 
            Do  
            Do: p1=binVal(ASO.Read(1)): Loop While p1=255 And Not ASO.EOS 
            If p1>191 And p1<196 Then Exit Do Else ASO.read(binval2(ASO.Read(2))-2) 
            Do:p1=binVal(ASO.Read(1)):Loop While p1<255 And Not ASO.EOS 
            Loop While True 
            ASO.Read(3) 
            ret(0)="JPG" 
            ret(2)=binval2(ASO.Read(2)) 
            ret(1)=binval2(ASO.Read(2)) 
        Case Else: 
            If left(Bin2Str(bFlag),2)="BM" Then 
                ASO.Read(15) 
                ret(0)="BMP" 
                ret(1)=binval(ASO.Read(4)) 
                ret(2)=binval(ASO.Read(4)) 
            Else 
                ret(0)="" 
            End If 
        End Select 
        ret(3)="width=""" & ret(1) &""" height=""" & ret(2) &"""" 
        GetImageHead=ret 
    End Function  
    
    
    Private Function Bin2Str(Bin)
        Dim I, Str,clow
        For I=1 To LenB(Bin)
            clow=MidB(Bin,I,1)
            If ASCB(clow)<128 Then
                Str = Str & Chr(ASCB(clow))
            Else
                I=I+1
                If I <= LenB(Bin) Then Str = Str & Chr(ASCW(MidB(Bin,I,1)&clow))
            End If
        Next
        Bin2Str = Str
    End Function
      
    Private Function Num2Str(Num,Base,Lens)
        Dim Ret
        Ret = ""
        While(Num>=Base)
            Ret = (Num Mod Base) & Ret
            Num = (Num - Num Mod Base)/Base
        Wend
        Num2Str = Right(String(Lens,"0") & Num & Ret,Lens)
    End Function
      
    Private Function Str2Num(Str,Base) 
        Dim Ret,I
        Ret = 0 
        For I=1 To Len(Str) 
            Ret = Ret *base + Cint(Mid(Str,I,1)) 
        Next 
        Str2Num=Ret 
    End Function 
      
    Private Function BinVal(Bin) 
        Dim Ret,I
        Ret = 0 
        For I = LenB(Bin) To 1 Step -1 
            Ret = Ret *256 + AscB(MidB(Bin,I,1)) 
        Next 
        BinVal=Ret 
    End Function 
      
    Private Function BinVal2(Bin) 
        Dim Ret,I
        Ret = 0 
        For I = 1 To LenB(Bin) 
            Ret = Ret *256 + AscB(MidB(Bin,I,1)) 
        Next 
        BinVal2=Ret 
    End Function 
 
End Class
'##########################################################

'-- Jack Add 20080612 -------------------------------------
'-- �Ϥ��ഫ�禡
Function ImageChange(strSPathName,strDPathName,strFileExtName,intResizeToW,intResizeToH)
Dim objImg
Dim intSourceW,intSourceH
Dim intTemp
	
	'-- ���o��l�ɪ��j�p
	intSourceW=GetImageWHS(strSPathName,1)(0)
	intSourceH=GetImageWHS(strSPathName,1)(1)
	'RESPONSE.WRITE intSourceW &","& intSourceH & "<BR>"
	'RESPONSE.WRITE intResizeToW &","& intResizeToH & "<BR>"	
	
	If 	intResizeToW >= intSourceW And intResizeToH >=intSourceH Then
		intResizeToW=intSourceW
		intResizeToH=intSourceH
	End If

	'-- �P�_�ؼ��Y�Ϭ���Ϊ���
	'-- �Y�����	
	If intResizeToW > intResizeToH Then
		'-- �ؼ� W/H >= �ӷ� W/H ���,�ؼФ���e��,�ӷ�������D,�ҥH�n�H�ؼйϭn�H������¦���Y
		If (intResizeToW/intResizeToH) >= (intSourceW /intSourceH) Then
			intResizeToW=0
		Else
			intResizeToH=0
		End If'=====================
	
	'-- ������Ϯ�
	ElseIf intResizeToW = intResizeToH Then
		'-- �ؼ� W/H >= �ӷ� W/H ���,�ؼФ���e��,�ӷ�������D,�ҥH�n�H�ؼйϭn�H������¦���Y
		If (intResizeToW/intResizeToH) >= (intSourceW /intSourceH) Then
			intResizeToW=0
		Else
			intResizeToH=0
		End If'=====================
	
	
	'-- �Y��������	
	ElseIf intResizeToW < intResizeToH Then
		'-- �ؼ� H/W >= �ӷ� H/W ���,�ؼФ�����D,�ӷ�����e��,�ҥH�n�H�ؼйϭn�H�e����¦���Y
		If  (intResizeToH/intResizeToW) >= (intSourceH /intSourceW)Then
			intResizeToH=0
		Else
			intResizeToW=0
		End If'=====================
	End If	
	
	''	RESPONSE.WRITE intSourceW &","& intSourceH & "<BR>"
	''	RESPONSE.WRITE intResizeToW &","& intResizeToH & "<BR>"
		
	Set objImg = new S_ImageConvert
	objImg.imgType = strFileExtName
	objImg.TransImage strSPathName,strDPathName,intResizeToW, intResizeToH 
	Set objImg = Nothing
End Function
'##########################################################

'-- JACK FIXed 20080522 (�wTry�L�i�H��)--------------------
'-- �ഫ���ɪ���ŧi�Ϋإ�---------------------------------

'-- �إߪ���
Class S_ImageConvert
'���Φs���ܼ�
Dim dataFile		'�p�ƾ������
Dim imgType			'��X�ϧ�����
Dim jpgQuality		'�~�� 0~100
'�C���ݩ�
Dim fgColor			'�e����
Dim bgColor			'�I����
Dim bdColor			'��ئ�
Dim transparent		'�q�z��
'�r���ݩ�
Dim fontSize		'�r���j�p
Dim fontColor		'�r���C��
Dim fontFace		'�r��
Dim fontBold		'����
Dim fontItalic		'����
'�e��
Dim width			'�ϼe
Dim height			'�ϰ�
Dim width_plus		'�[�e�q
Dim height_plus		'�[���q
Dim bdSize			'��زʲ�

'�����s���ݩ�,��k
Private img			'�����Ϊ����ܼ�


  	'-- �}�l�ɰ��檺�u�@
  	Private Sub Class_Initialize()
		dataFile = "./counter.dat"
		imgType = 2
		jpgQuality = 100
		fontSize = 10
		fontColor = "#000000"
		bgColor = "#FFFFFF"
		fgColor = "#FFFFFF"
		bdSize = 1
		width_plus = 0
		height_plus = 0
		transparent = False
		set img = server.createobject("Overpower.ImageLib") 'Create�@�Ӫ��� 
  	End Sub

  	'-- �����ɰ��檺�u�@
  	Private Sub Class_Terminate()
		Set img = Nothing
  	End Sub  
  	
	'-- �ഫ����
	Function TransImage(sf, df, w, h)
		Dim imgwidth, imgheight,FileName
		GetImageSize(sf)
		
		'-- �p�G�e�פj��0,�N��W �Ӻ�H
		If w > 0 Then
			imgwidth = w
			imgheight = height * w/Width
			If h<>Empty Then imgheight=h
			
		'-- �p�G�e�׵���0,�N��H �Ӻ�W
		ElseIf w = 0 Then
			imgheight = h
			imgwidth = Width * h/height
		End If
		
		
		img.width = imgwidth '�]�w�����󪺼e�� 
		img.height = imgheight '�]�w�����󪺰��� 

		FileName=sf
		
		img.InsertPicture Filename, 0, 0, True, imgwidth, imgheight

		'WriteImage(Server.MapPath(df))
		WriteImage(df)
	End Function  	
  	

	'-- ���ͼƦr
	Function Counter()		
		Dim cnt
		cnt = CStr(Read_Cnt_Data())
		ImageText(cnt)
	End Function

	'-- �ϧΦr
	Function ImageText(char)
		Dim imgwidth, imgheight
    	SetFont() 
		imgwidth = img.GetTextWidth(char)+width_plus
		imgheight = img.GetTextHeight(char)+height_plus

		img.width = imgwidth '�]�w�����󪺼e�� 
		img.height = imgheight '�]�w�����󪺰��� 

		img.BrushColor = bgColor
		img.FillRect 0, 0, imgheight, imgwidth
		'(x1,y1) - (y2,x2)

		If bdColor<>Empty Then
			img.PenColor = bdColor
			img.PenWidth = bdSize
			pQty=1
			If bdSize Mod 2 = 1 Then pQty=0
			img.Box Int(bdSize/2), Int(bdSize/2), imgwidth-Int(bdSize/2)+pQty, imgheight-Int(bdSize/2)+pQty
			'(x1,y1) - (x2,y2)
		End If

		AddText char, width_plus/2, height_plus/2

		'��r, X, Y
		WriteImage("")
	End Function
 

	'-- ��ܹϤ�
	Function ShowImage(imgFile, w, h)
		Dim imgwidth, imgheight
		GetImageSize(imgFile)

		imgwidth = w
		imgheight = height * w/width
		If h<>Empty Then imgheight=h
		img.width = imgwidth '�]�w�����󪺼e�� 
		img.height = imgheight '�]�w�����󪺰��� 

		'FileName=Server.MapPath(imgFile)
		FileName=imgFile
		img.InsertPicture Filename, 0, 0, True, imgwidth, imgheight

		SetFont() 
		AddText text, 2, 2

		WriteImage("")
	End Function
	'======================================================

	
	
	
	
	
	'-- �U�C�����󤺳��禡---------------------------------
	'-- ��X�Φs��
	Private Function WriteImage(df)
		Dim transColor
		transColor = ""
	  If transparent<>Empty Then
			transColor = bgColor
		End If

		If df<>Empty Then
			imgType = Right(df, 3)
			ImageType()
			img.SavePicture df, imgType, jpgQuality, transColor '������X����, ���s�� 
		Else
			ImageType()
			img.PictureBinaryWrite imgType, jpgQuality, transColor '������X����, ���s�� 
			'����.��k �ɮ�����, ���Y�~��, �ٲ��q�z�C��� 
		End IF

	End Function

	
	'-- �[��r
	Private Function AddText(char, x, y)
	  img.FontColor=fontColor
		img.TextOut char, x, y
	End Function
	
	'�ഫ�Ϥ����A���Ʀr
	Private Function ImageType()
    If LCase(imgType)="bmp" Then 
			imgType=1
		ElseIf LCase(imgType)="gif" Then 
			imgType=2
		ElseIf LCase(imgType)="jpg" Then 
			imgType=3
		End If
		ImageType = imgType
	End Function

  	'-- �]�w�r���ݩ�
	Private Function setFont
		img.fontSize   = fontSize
		img.fontColor  = fontColor
		img.fontFace   = fontFace
		img.fontBold   = fontBold
		img.fontItalic = fontItalic
	End Function
	
	'-- Ū�Ʀr&�֥[
	Private Function Read_Cnt_Data()
		Dim fs, txtf, fileName, cnt
		fileName = Server.MapPath(dataFile)
		Set fs = Server.CreateObject("Scripting.FileSystemObject")
		
		Set txtf = fs.OpenTextFile(fileName, 1, True)
		If Not txtf.atEndOfStream Then cnt = txtf.ReadLine
		txtf.Close

		cnt = Clng(cnt)+1

		Set txtf = fs.OpenTextFile(fileName, 2, True)
		txtf.WriteLine cnt
		txtf.Close

		Set fs = Nothing
		Read_Cnt_Data = cnt
	End Function

	'-- ���o���ɤؤo
	Private Function GetImageSize(imgFile)
		img.PictureSize imgFile, width, height
	End Function
	'======================================================
	
	
	
	
End Class
'##########################################################


















































'''=========================
'''�q�����|�U���ɮ�
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
''Dim strFileExtName
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
''Dim strFileExtName
''	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")	
''	If existsPath(aPath,0)=true Then
''		Set Folder=oFSO.GetFolder(aPath)
''		Set Files=Folder.Files
''		For Each File in Files
''			strFileExtName=Right(File.Name,3)
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
''Dim strFileExtName
''	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")	
''	If existsPath(aPath,0)=true Then
''		Set Folder=oFSO.GetFolder(aPath)
''		Set Files=Folder.Files
''		For Each File in Files
''			strFileExtName=Right(File.Name,3)
''			If strFileExtName="htm" Then	
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
''Dim strFileExtName
''	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")	
''	If existsPath(aPath,0)=true Then
''		Set Folder=oFSO.GetFolder(aPath)
''		Set Files=Folder.Files
''		For Each File in Files
''			strFileExtName=Right(File.Name,3)
''			If strFileExtName="htm" Then	
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
''Dim strFileExtName
''	Set oFSO=Server.CreateObject("Scripting.FileSystemObject")	
''	If existsPath(aPath,0)=true Then
''		Set Folder=oFSO.GetFolder(aPath)
''		Set Files=Folder.Files
''		
''		
''		For Each File in Files
''			strFileExtName=Right(File.Name,3)
''			If strFileExtName<>"JPG" and strFileExtName<>"GIF" and strFileExtName<>"jpg" and strFileExtName <> "gif" Then
''			strDate="[" & FormatDateTime(File.DateLastModified,2) & " " & FormatDateTime(File.DateLastModified,4) & "]"
''			strSize="(" & Cstr(FormatNumber(File.Size/1024,2)) & "KB)"
''			strOption=strOption & "<a href="& Lpath &""& File.Name &" Target=_blank>"&File.Name & "</a><br><a href=""../../_include/download.asp?id=" & id & "&file=" & File.Name &"&FNAME="& path & """> <font color=red>�U����</font>"  &  strSize & "</a><br>"
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
