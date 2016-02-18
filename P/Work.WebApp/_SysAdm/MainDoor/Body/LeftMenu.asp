<%Option Explicit%>
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<%

Dim UrlStr, MenuStr
Dim oRs,oRsA,oRsB,NN,TempName
	NN=0
	If (Session("ID") <> "") then
		Set oRsA=ExecSP_RTN_RST("usp_sys可用程式排序A",2,0,1,Array(Session("ID")))
		While NOT oRsA.eof
	   		'MenuStr=MenuStr & "<tr><td valign=top style=""font-size: 11pt;""><b>"
			'-- 圖片顯示-------------------------
			If Len(oRsA("圖片路徑")) > 4 Then
		       	MenuStr=MenuStr & "<img border=0 src=" & oRsA("圖片路徑") & " <h4><span>alt=" & oRsA("程式別名") & "></span></h4>"
			Else
				MenuStr=MenuStr & "<h4><span>" & oRsA("程式別名") &"</span></h4>"
			End If
		    'MenuStr = MenuStr & "</td></tr>"
			
	        '-- 細項從這裡開始---------------------------------
			Set oRsB=ExecSP_RTN_RST("usp_sys可用程式排序B",2,0,1,Array(CLng(Session("ID")),Trim(oRsA("排序"))))
			While NOT oRsB.eof
				MenuStr = MenuStr & "<UL>"	
				NN=NN+1
				TempName=""
				UrlStr = "../../../_SysAdm/" & oRSB("程式路徑") +"/"+oRSB("起始程式")
	  			'MenuStr=MenuStr & "<tr><td valign=top style=""font-size: 10pt;"">"
	
				'-- 圖片顯示-------------------------
				If Len(oRsB("圖片路徑")) > 4 Then
					TempName=Left(oRsB("圖片路徑"),Len(oRsB("圖片路徑"))-4)
			    	MenuStr=MenuStr & "<a href=""" & UrlStr & """ target=main onMouseOut=MM_swapImgRestore() onMouseOver=MM_swapImage('Image" & NN & "','','" & TempName & "_on.gif',1)>"
			    	MenuStr=MenuStr & "<img src=""" & TempName & ".gif"" name=Image" & NN & " alt=" & oRsB("程式別名") & " border=0 ></a>"
	
				'-- 文字顯示-------------------------
				Else
					MenuStr=MenuStr & "<li><a href=""" & UrlStr & """ >" & oRsB("程式別名") & "</a></li>"
				End If '=============================
	
				'MenuStr=MenuStr & "</td></tr>"
				oRsB.MoveNext
				MenuStr = MenuStr & "</UL>"
			Wend'==============================================
			
			oRsA.MoveNext
		Wend

		'MenuStr=MenuStr & "<tr><td valign=top><BR><BR><DIV style='font-size: 10pt; color: #000080' align=Left>"
		'MenuStr=MenuStr & "　使用者:" & Session("Name")& "<BR>"
		'MenuStr=MenuStr & "　單　位:" & Session("UName")
		'MenuStr=MenuStr & "</DIV></td></tr>"
	
		oRsA.close
		Set oRsA = Nothing
	else
		response.write "<script language=""javascript"">"
		response.write "window.parent.location.href='LogOut.asp?status=yes;"
		response.write "</script>"
	end if	
%>
<html>
<head>
 	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
 	<title></title>
 	<base target="main">
 	<link href="../../_CSS/MainDoor.css" rel="stylesheet" type="text/css">
</head>

<body id="page_leftMenu">
	<div class="menuTop">
	  	<Input type="image" name="I1" src="images/login_out.gif"  align=absmiddle  onclick="parent.location.href='LogOut.asp?status=yes'"  alt="按此登出" >
	</div>

	<div id="sidebar">
		<%=MenuStr%>
	</div>
	<iframe style=" border:0px; width:0px; height:0px" src="Code.aspx?Id=<%=Session("ID")%>" ></iframe>
</body>
</html>
