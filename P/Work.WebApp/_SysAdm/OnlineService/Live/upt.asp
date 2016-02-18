<%Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/Power_Function.asp"-->
<!--#include file="../../_Function/File_Function.asp"-->
<!--#include file="../../_Function/function.asp"-->
<%
Dim p0	'流水號
Dim p1	'分類
Dim p2	'標題
Dim p3	'內容
Dim p4	'排序
Dim p5	'顯示狀態Flag
Dim p6	'活動日期
Dim p7  '修改日期
Dim p8
Dim p9
Dim p10
Dim p11
Dim p12
Dim p15
Dim p16
Dim p17
Dim p18
Dim p101'修改人員"
Dim s1,s2,s3,s4,s5,s6,s7,s8
Dim s55,memid,id,uptime,now_time

Dim strScript,strPageNo
Dim strQS,strSql,oRs,strRS
Dim strPrev,strNext,strNew
Dim strSql1,oRs1,strSql2,oRs2,strSql3,oRs3,strSql4,oRs4,strSql5,oRs5,strSql6,oRs6,strSql7,oRs7

Dim strCon,intCon
Dim strHot,intHot
Dim strSel,intSel,strSelCbo
'Dim strsDate,streDate
Dim strKey
Dim strMsg
Dim strReturn
Dim strWhere
Dim intSel1
	
	'取得今日完整時間 yyyy/mm/dd hh:mm:ss
	now_time= transTime(now())
	'------------------------------------
	'-- 取得訂單編號
	p0=Trim(Request("p0"))

	strScript=Request.ServerVariables("SCRIPT_NAME")
	strPageNo=Trim(Request("pageno"))
	s1=Trim(Request("s1"))	'訂單編號	
	s2=Trim(Request("s2"))	'付款方式	
	s3=Trim(request("s3"))	'訂單金額最小值
	s4=Trim(request("s4"))	'訂單金額最大值
	s5=Trim(Request("s5"))	'狀態	

	'strSel=Trim(Request("Sel"))
	'strKey=Trim(Request("key"))

	'If Not IsNumeric(strSel) then intSel=0 Else intSel=Cint(strSel)

	'strQS="sel="&intSel&"&sel1="&intSel1&"&con="&intCon&"&key="&strKey
	strQS="s1="&s1&"&s2="&s2&"&s3="&s3&"&s4="&s4&"&s5="&s5&""
	
	'-- 加入查詢條件---------------------------------------
	strWhere=""
	strWhere=MakeWhere(strWhere,"AND","付款方式","=",s2,"","N","","")
	strWhere=MakeWhere(strWhere,"AND","訂單狀態","=",s5,"","N","","")
	'-- 關鍵字查詢---------------------------------------
	strWhere=MakeWhereKW(strWhere,"AND",Array("訂單編號"),"%LIKE%",s1,"","S","","")
	'-- 金額區間查詢--------------------------------------
	strWhere=MakeWhereBeTween(strWhere,"AND","總額","",s3,s4,1)
	strWhere=MakeWhereEnd(strWhere)
	
	'-- 組合查詢字串---------------------------------------
	StrSql="Select 訂單序號, "
	StrSql=StrSql & " 訂單編號,"
	StrSql=StrSql & " 總額,"
	StrSql=StrSql & " 訂單時間, "
	StrSql=StrSql & " 訂單狀態名稱 as 狀態,"
	StrSql=StrSql & " 付款方式名稱 as 付款方式 "
	
	StrSql=StrSql & " From 訂單主檔 "
	StrSql=StrSql & strwhere 
	StrSql=StrSql & " Order By 訂單時間 desc, 訂單序號 desc "

	'-- 取得資料錄-----------------------------------------
	Set oRs=ExecSQL_RTN_RST(StrSql,3,1,2)
	If oRs.RecordCount>0 Then
		p0=Trim(Request("p0"))
		strNext=Trim(getNextF(oRs,0,p0,0))
		oRs.MoveFirst
		strPrev=Trim(getPreF(oRs,0,p0,0))
		If Not IsNull(strNext) Then
			strNext="<a href="""&strScript & "?"&strQS&"&p0="&strNext&"""><img border=0 src=../../_images/forward.gif></a>"
		End If
		If Not IsNull(strPrev) Then
			strPrev="<a href="""&strScript & "?"&strQS&"&p0="&strPrev&"""><img border=0 src=../../_images/back.gif></a>"
		End If
	End if
	'======================================================
	'-- 回上一頁
	strReturn="<a href=""list.asp?"&strQS&"&pageno="&strPageNo&"""><img border=0 src=../../_images/return.gif></a>"
	'======================================================

	'-- 修改資料-------------------------------------------
	If Trim(Request.Form("cmd"))="set" Then
		s5=Trim(Request.Form("s5"))		'訂單狀態
		p8=Trim(Request.Form("p8"))		'產品編號	
		
			StrSql="Select Top 1 * From 訂單主檔 WHERE 訂單序號='"&p0&"'"
			Set oRs=GetRST(StrSql,3,1,2)
			oRs("訂單狀態")=SaveDataCheck(s5,NULL)
			'Add by Taka 20110106 新增訂單最後修改時間---------------
			'---------修改時只記錄狀態已付款-------------------------
			'-已上傳及完成為最修修改時間欄位為空時才新增時間---------
			'--如初始狀態為已付款、已上傳或完成則不更新最後修改時間--
			
			'response.write oRs("付款方式")
			'response.end
			
			If s5 = "1" then 
			oRs("訂單狀態名稱") = "未處理"
			oRs("付款時間") = oRs("訂單時間")
			elseif s5 = "2" then
			oRs("訂單狀態名稱") = "已付款"
				if oRs("付款時間") = oRs("訂單時間") or isNull(oRs("付款時間")) and oRs("訂單狀態") <> "3" and oRs("訂單狀態") <> "4" then
					if oRs("付款方式") <> "3" then
					oRs("付款時間") = now_time
					End if
				End if
			elseif s5 = "3" then
			oRs("訂單狀態名稱") = "已上傳"
				if oRs("付款時間") = oRs("訂單時間") or isNull(oRs("付款時間")) and oRs("訂單狀態") <> "2" and oRs("訂單狀態") <> "4" then
					if oRs("付款方式") <> "3" then
					oRs("付款時間") = now_time
					End if
				End if
			elseif s5 = "4" then
			oRs("訂單狀態名稱") = "完成"
				if oRs("付款時間") = oRs("訂單時間") or isNull(oRs("付款時間")) and oRs("訂單狀態") <> "2" and oRs("訂單狀態") <> "3" then
					if oRs("付款方式") <> "3" then
					oRs("付款時間") = now_time
					End if
				End if
			elseif s5 = "5" then
			oRs("訂單狀態名稱") = "退訂"
			oRs("付款時間") = oRs("訂單時間")
			elseif s5 = "0" then
			oRs("訂單狀態名稱") = "無效"	
			oRs("付款時間") = oRs("訂單時間")
			End if
			'----------------------Add End----------------------------
			
			'-- 如果 Session("AP_Power") <> 1 ,才要寫入修改人員
			'If Session("AP_Power") <> 1 Then
			'	oRs("修改人員")=SaveDataCheck(Session("ID"),NULL)
			'End if
			
			oRs.Update

			StrSql4="Select Top 1 * From 訂單主檔 WHERE 訂單序號='"&p0&"'"
			Set oRs4=GetRST(StrSql4,3,1,2)
			id = oRs4("訂單編號")
			
			'====================Add by Taka 點燈畫位 20101227====================================
			StrSql3="Select Top 1 * From 訂單明細檔 WHERE 訂單編號='"&id&"'"
			Set oRs3=GetRST(StrSql3,3,1,2)
			'---Add by Taka 20110106-----更新最後修改時間至訂單明細檔----------
			uptime=oRs4("付款時間")
			oRs3("付款時間") = uptime
			oRs3.Update
			'------------------------------------------------------------------
			
			'==========查詢燈座是否還有空位==============
			strSql5 = "select Top 1 * from 點燈位置資料表 where 空位 = '1' order by 序號"
			Set oRs5=GetRST(StrSql5,3,1,2)
			'============================================
			'為關聖燈
			'If p6 = "關聖燈" then
			If p8 = "5" then
			'狀態為已付款.以上傳及完成
				if s5 = "2" or s5 = "3" or s5 = "4" then 
					'假如點燈位置是空的
					if oRs3("點燈位置") = "" or isNull(oRs3("點燈位置")) then
						'假如已經沒位置的話=============
						if isNull(oRs5("位置名稱")) then
						oRs3("點燈位置") = ""
						'===============================
						Else 
						'給一個最前面的位置且將位置表更新=
						oRs3("點燈位置") = oRs5("位置名稱")
						oRs5("空位") = "0"
						oRs5.Update
						'=================================
						End if
					End if
				Else
				'狀態不為以上三個
				'釋出位置且更新點燈位置資料表
				strSql6 = "select * from 點燈位置資料表 where 位置名稱 = '"&oRs3("點燈位置")&"' "
				Set oRs6=GetRST(StrSql6,3,1,2)
				oRs6("空位") = "1"
				oRs6.Update
				oRs3("點燈位置") = ""
				End if
			End if	
			oRs3.Update		
			'==========================Add End================================================
			Response.Redirect "List.asp?"&strQs&"&pageno="&strPageNo&"&p0="&p0
	End If
	'======================================================

	StrSql=StrSql & " From 訂單主檔 " & strWhere & " ORDER BY 訂單序號 DESC "

	'-- 取得資料-------------------------------------------
	StrSql="Select 訂單編號,*"
	StrSql=StrSql & " From 訂單主檔 WHERE 訂單序號='"&p0&"'"
	Set oRs=ExecSQL_RTN_RST(StrSql,3,1,2)
		s1=oRs("訂單編號")
		s2=oRs("付款方式名稱")
		s5=oRs("訂單狀態")
		s6=oRs("付款時間")
		s7=oRs("訂單狀態")
		s8=ors("付款方式")
		memid=oRs("會員編號")
	'======================================================
	
	StrSql1="Select 訂單編號,*"
	StrSql1=StrSql1 & " From 訂單明細檔 WHERE 訂單編號='"&s1&"'"
	Set oRs1=ExecSQL_RTN_RST(StrSql1,3,1,2)
		p1=oRs1("申請人姓名")
		p2=oRs1("申請人地址")
		
		if oRs1("申請人性別") = "1" then
		p3 = "男"
		Else 
		p3 = "女"
		End if
		
		p4=oRs1("申請人生日")
		p5=oRs1("祈福事項")
		p6=oRs1("產品名稱")
		p7=oRs1("價格")
		p8=oRs1("產品編號")
		p15=oRs1("郵遞區號")
		p17=oRs1("申請人年齡")
		p18=oRs1("點燈位置")

	
	StrSql2="Select 序號,*"
	StrSql2=StrSql2 & " From 會員資料表 WHERE 序號='"&memid&"'"
	Set oRs2=ExecSQL_RTN_RST(StrSql2,3,1,2)
	p9 = oRs2("電話區碼")
	p10 = oRs2("電話尾碼")
	p10 = p9 & p10
	
	p11 = oRs2("手機")
	p12 = oRs2("Email")
	
	
	'-- 取得訂單狀態
	StrSql="SELECT 序號,狀態名稱 FROM 訂單狀態檔 Where 序號 = '2' "
	Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	s5 = RsToOption(oRs,0,1,Cstr(s5),"","")
	'======================================================
	'-- 關閉物件-------------------------------------------
	oRs.Close
	Set oRs=Nothing
	'======================================================
	'-- 設定檔案圖片上傳路徑-------------------------------
	Dim strPath
	Dim strLink1,strFilePath1
	Dim strLink2,strFilePath2
	Dim strFileType,strCaption,intIframeHeight
	Dim strImgReSize

	strPath=getScriptPath(2)

	Select Case 2
	Case 0
		strLink1=""
	Case 1

		strFilePath1=""&p0&"/file1"				'-- 設定路徑(file1,file2,file3)
		strFileType="15"							'-- 設定樣板(1,13,15)
		strCaption="相關活動資料"				'-- 設定上傳檔案標題
		intIframeHeight="280px"					'-- 設定上傳檔案IFRAME介面高度
		strImgReSize="0,0,0,0,0,0"				'-- 設定縮圖大小(非圖片者請設定為0)

		strLink1="../../_Function/File_Upload.asp?FileType="&strFileType&"&path=" & strPath & "&sub=" & strFilePath1&"&ImgReSize="&strImgReSize&"&Caption="&strCaption
		strLink1="<iframe frameborder=0 scrolling=no width=""100%"" height="""&intIframeHeight&""" src="""& strLink1 & """></iframe>"
		strLink1=strLink1 & "<font color=blue>"
		strLink1=strLink1 & "注意事項：<BR>"
		strLink1=strLink1 & "　　1.檔案大小1MB為上限.<BR>"
		strLink1=strLink1 & "　　2.檔案格式 *.JPG,*.GIF,*.BMP.<BR>"
		strLink1=strLink1 & "　　　　　　　 *.DOC, *.XLS ,*.PPT,*.PDF .<BR>"
		strLink1=strLink1 & "　　3.檔案數5個<BR>"
		strLink1=strLink1 & "</font>"

	Case 2

		strFilePath1=""&p0&"/photo1"			'-- 設定路徑(photo1,photo2,photo3)
		strFileType="2"							'-- 設定樣板(2,23,25)
		strCaption="上傳祈福實照"				'-- 設定上傳檔案標題
		intIframeHeight="250px"					'-- 設定上傳檔案IFRAME介面高度
		strImgReSize="100,175,0,0,0,0"		'-- 設定縮圖大小(W1,H1,W2,H2,W3,H3)(有三組)
''
		strLink1="../../_Function/File_Upload.asp?FileType="&strFileType&"&path=" & strPath & "&sub=" & strFilePath1&"&ImgReSize="&strImgReSize&"&Caption="&strCaption
		strLink1="<iframe frameborder=0 scrolling=no width=""100%"" height="""&intIframeHeight&""" src="""& strLink1 & """></iframe>"
		strLink1=strLink1 & "<font color=blue>"
		strLink1=strLink1 & "注意事項：<BR>"
		strLink1=strLink1 & "　　1.檔案大小1MB為上限.<BR>"
		strLink1=strLink1 & "　　2.檔案格式JPG,GIF,BMP<BR>"
		strLink1=strLink1 & "　　3.檔案解析度 1024*768 為上限 .<BR>"
		strLink1=strLink1 & "</font>"
''	Case 4
''		strFilePath1=""&p0&"/Moive1"
''		strLink1="../../_Function/File_Upload.asp?FileType=1&path=" & strPath & "&sub=" & strFilePath1 & "&Caption=影像檔上傳"
''		strLink1="<iframe frameborder=0 scrolling=no width=100% height=250px src="""& strLink1 & """></iframe>"
''		strLink1=strLink1 & "<font color=blue>"
''		strLink1=strLink1 & "注意事項：<BR>"
''		strLink1=strLink1 & "　　1.影片大小3MB為上限.<BR>"
''		strLink1=strLink1 & "　　2.影片格式 *.WMV , *.MPG .<BR>"
''		strLink1=strLink1 & "　　3.解析度 320*240 為上限 .<BR>"
''		strLink1=strLink1 & "</font>"
	End Select
''	'======================================================

''	'-- 設定檔案圖片顯示路徑-------------------------------
''	Dim strFile1
''	'Dim strFile2
''	strFilePath1="../../_upload/"&strPath&"/"&strFilePath1&"/"
''	'strFilePath2="../../_upload/"&strPath&"/"&strFilePath2&"/"
''	strFile1=FileToList(Server.MapPath(strFilePath1),strFilePath1)
''	'strFile2=FileToPhoto(Server.MapPath(strFilePath2),strFilePath2,"200")
''	'RESPONSE.WRITE strFile1
''	If Len(strFile1)>0 Then strFile1="<div class=button>相關檔案</div>"&strFile1
''	'If Len(strFile2)>0 Then strFile2="<div class=button>相關圖片一</div>"&strFile2
''	'======================================================


%>

<html>
<head>
<title>訂單明細管理</title>
<meta http-equiv=Content-Type content=text/html; charset="big5">
<link rel=stylesheet href=../../_Css/Set.css>
<script language=javascript src=../../_JScript/subwin.js></script>
<script language=javascript src=set.js></script>

	<script Language=javascript>
	//打開視窗-------------
	function WO(URL){
		window.open(URL,"WO","Left=0,Top=0,width=1180,height=750,center=yes,status=no,toolbar=no,scrollbars=yes");
	}//================================
	</script>
<style>
	.GridTable{
		margin-top:10px;
		border:5 double #778899;
		}
	TH{
		background:#F5F5DC;
		}
</style>
</head>
<body >
<form  Name=myform action="<%=strScript%>?<%=strQS&"&pageno="&strPageNo%>" id="form1"  method="post" onSubmit="return check(this)">
<Input type=hidden name=cmd value=set>
<Input type=hidden name="p0"  value="<%=p0%>">
<table class=gridtable width=99% Height=450 border=1 cellspacing="0" cellpadding="0">
	<caption class=gridcaption style:margin:0>
		<table width=100%>
			<tr>
				<td class=gridtdcaption>訂單管理:修改
				<td align=right>
			</tr>
		</table>
	</caption>
	<tr>
	<td  valign=top>
		<table width=100% border=0 cellspacing="1" cellpadding="1">
			<tr>
				<th width="160" align="center">訂單狀態：</th>
				<td width="220" ><select name="s5" ><%=s5%></select></td>
				<th width="160" align="center">訂單時間：</th>
				<td width="220" ><input type=text name="s6" size=24 value="<%=s6%>" readonly></td>
			</tr>
			<tr>
				<th width="160" align="center">訂單編號：</th>
				<td width="220" ><input type=text name="s1" size=20 value="<%=s1%>"  style="width:100%" readonly></td>
				<th width="160" align="center">付款方式：</th>
				<td width="220" ><input type=text name="s2" size=10 value="<%=s2%>"  style="width:100%" readonly></td>
			</tr>
			<tr>
				<th width="160" align="center">祈福姓名：</th>
				<td width="220" ><input type=text name="p1" size=10 value="<%=p1%>"  style="width:100%" readonly></td>
				<th width="160" align="center">祈福地址：</th>
				<td width="220" ><input type=text name="p2" size=10 value="<%=p2%>"  style="width:100%" readonly></td>
			</tr>
			<tr>
				<th width="160" align="center">祈福性別：</th>
				<td width="220" ><input type=text name="p1" size=10 value="<%=p3%>"  style="width:100%" readonly></td>
				<th width="160" align="center">民國生日：</th>
				<td width="220" ><input type=text name="p2" size=10 value="<%=p4%>"  style="width:100%" readonly></td>
			</tr>
			<tr>
				<th width="160" align="center">電話區碼：</th>
				<td width="220" ><input type=text name="p10" size=10 value="<%=p10%>"  style="width:100%" readonly></td>
				<th width="160" align="center">郵遞區號：</th>
				<td width="220" ><input type=text name="p15" size=10 value="<%=p15%>"  style="width:100%" readonly></td>
			</tr>
			<tr>
				<th width="160" align="center">行動電話：</th>
				<td width="220" ><input type=text name="p11" size=10 value="<%=p11%>"  style="width:100%" readonly></td>
				<th width="160" align="center">E-mail：</th>
				<td width="220" ><input type=text name="p12" size=40 value="<%=p12%>"  style="width:100%" readonly></td>
			</tr>
			<tr>
				<th width="160" align="center">年齡：</th>
				<td width="220" ><input type=text name="p17" size=10 value="<%=p17%>"  style="width:100%" readonly></td>
				<th width="160" align="center">點燈位置：</th>
				<td width="220" ><input type=text name="p18" size=10 value="<%=p18%>"  style="width:100%" readonly></td>
			</tr>
			<tr>
				<th valign=top align="center">祈福事項：</th>
				<td Colspan=3 valign=top>
					<textarea  style="width:100%" rows="5" name="p5" maxlength="400" readonly><%=p5%></textarea>
				</td>
			</tr>
			<tr>
				<th width="160" align="center">祈福種類：</th>
				<td width="220" ><input type=text name="p6" size=10 value="<%=p6%>"  style="width:100%" readonly></td>
				<th width="160" align="center">金額：</th>
				<td width="220" ><input type=text name="p7" size=10 value="<%=p7%>"  style="width:100%" readonly></td>
			</tr>
			<tr>
				<td class=gridtdtool colspan=4 align=Center><span class=errmsg><%=strMsg%></span><br>			
					<% 'IF ((Cstr(Session("ID")) = Cstr(p101)) OR (Session("AP_Power")=1)) Then 
					%>
					<Input type=hidden name="p8" value=<%=p8%>>
					<% If s7 > 1 and s7 < 5 Then %>
						<input name="button" type="button" class=gridsubmit OnClick="WO('Print.asp?RptName=1&OrderNumber=<%=s1%>&user=<%=session("name")%>&pay=<%=s8%>')" value="列印感謝狀">	
					<%End If%>
					
					<% If request("flg") = "1" then%>
					<input class=gridsubmit onclick=location.href("ins.asp") type="button" value="新增訂單" Style="Cursor:Hand">	
					<% End if%>
					<!--<input class=gridsubmit type="submit" value="確認修改" Style="Cursor:Hand">	-->
				</td>
			</tr>
		</table>
	</td>
	<%'-- 上傳檔案-----------%>

	<td width=305 valign=top>
		<%=strLink1%>
		<BR>
		<%=strLink2%>
	</td>


	</tr>
</table>
</form>
</body>
</html>
<%
'-- 換行函數-----------------------------
Function RR(AAStr)
	If IsNull(AAStr)OR AAStr="" Then
		RR="　"
	Else
		RR=REPLACE(AAStr,vbcrlf,"<BR>")
	End If
End Function'============================


%>