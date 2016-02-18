<%Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/Power_Function.asp"-->
<!--#include file="../../_Function/function.asp"-->
<%
Dim p0	'訂單編號
Dim p1	'姓名
Dim p2	'生日
Dim p3	'Email
Dim p4	'電話區碼
Dim p5	'電話尾碼
Dim p6	'手機
Dim p7	'地址
Dim p8	'祈福事項
Dim p9	'性別
Dim p10	'訂單編號
Dim p11	'產品編號
Dim p12	'產品名稱
Dim p13	'產品價格
Dim p14	'香油捐贈金
Dim p15	'郵遞區號
Dim p16	'國農曆
Dim p17	'時辰
Dim id	'會員編號 頁面參數
Dim order	'訂單編號 頁面參數
Dim n	'時間
Dim light '前頁燈種參數
Dim now_time	'現在時間
Dim s1,s2,s3,s4,s5,s6
Dim strScript,strQS,strPageNo,strReturn
Dim new_order_No,old_order_no_num,old_order_no 
Dim oRs,oRs1,oRs2,oRs3,oRs4,oRs5,oRs6,oRs7
Dim strSql,strSql1,strSql2,strSql3,strSql4,strSql5,StrSql6,StrSql7
Dim strMsg		'判斷是否有無填寫資料
Dim strWhere
Dim sqlst
Dim mem,sn,payment,stat,Ydate
Dim ans
	
	'====================================================================
	'製造最新訂單編號共十碼
	new_order_No = right("00" & year(now()),2) & right("00" & month(now()),2) & right("00" & day(now()),2)
	do
		strsql2 = "select 訂單編號 from 訂單主檔 where 訂單編號 like '" & new_order_no & "%' order by 訂單序號 desc"
		set ors2 = ExecSQL_RTN_RST(strsql2,3,0,1)
		if not ors2.eof then
			old_order_no = ors2(0)
			old_order_no_num = right("0000"&cInt(right(ors2(0),4)) + 1,4)
			new_order_no = new_order_no & old_order_no_num
		else
			new_order_no = new_order_no & "0001"
		end if
		ors2.close

		strsql2 = "select * from 訂單主檔 where 訂單編號='" & new_order_no & "'"
		set ors2 = ExecSQL_RTN_RST(strsql2,3,0,1)
	loop while not ors2.eof
	'------------------------------------------------------------------------
	p0 = new_order_no
	
	strQS="s1="&s1&"&s2="&s2&"&s3="&s3&"&s4="&s4&"&s5="&s5&""
	'-- 回上一頁
	strReturn="<a href=""list.asp?"&strQS&"&pageno="&strPageNo&"""><img border=0 src=../../_images/return.gif></a>"
	'======================================================
	now_time= transTime(now())
	'response.write now_time & "<BR>"
	'response.write DateAdd(SS,1,now_time)
	
	Ydate=Year(now_time) '取最新年份
	
	'--新增訂單資料----------------------------------------
	If request("cmd")="set" then
	
	mem = "A" & right("0000000000" & int(10000*Rnd),10)		
		p1=request("p1")
		'=============判別國曆或農曆==========
		Dim y,m,d,yearold
		y=request("year")
		m=request("month")
		d=request("day")
		p16=request("calen")	
		'-------------------------------------
		if p16 = "1" then
		ans=request("answer")	'國曆轉農曆
		Yearold = Ydate-Year(ans)+1 '計算年齡
		else
		y = y+1911	'農曆年分轉西元年份
		ans= y&"/"&m&"/"&d
		Yearold = Ydate-y+1	'計算年齡
		End if
		'=====================================
		
		p3=request("p3")
		p4=request("p4")
		p5=request("p5")
		p6=request("p6")
		p7=request("p7")
		p8=request("p8")
		p9=request("sex")
		s1=request("s1")
		s2=request("s2")
		s5=request("s5")
		'p11=request("p11")
		'p12=request("p12")
		'p13=request("p13")
		p14=request("p14")
		p15=request("p15")
		
		p17=request("time")
		
		'讀取產品資料表內容==============================================
		strWhere=""
		strWhere=MakeWhere(strWhere,"AND","產品編號","=",s1,"","N","","")
		strWhere=MakeWhereEnd(strWhere)
		'----------------------------------------------------------------
		StrSql1 = " Select * from 產品資料表 "
		StrSql1 = StrSql1 & strWhere
		Set oRs1=ExecSQL_RTN_RST(StrSql1,3,1,2)
		'================================================================
		p11 = oRs1("產品編號")
		p12 = oRs1("產品名稱")
		p13 = oRs1("價格")
		
		'加入會員資料表================================
			StrSql="Select Top 1 * From 會員資料表 "
			Set oRs=GetRST(StrSql,3,1,2)
			oRs.AddNew
			oRs("會員編號")=SaveDataCheck(mem,NULL)
			oRs("姓名")=SaveDataCheck(p1,NULL)
			oRs("生日")=ans
			oRs("性別")=SaveDataCheck(p9,NULL)
			oRs("Email")=SaveDataCheck(p3,NULL)
			oRs("電話區碼")=SaveDataCheck(p4,NULL)
			oRs("電話尾碼")=SaveDataCheck(p5,NULL)
			oRs("手機")=SaveDataCheck(p6,NULL)
			oRs("地址")=SaveDataCheck(p7,NULL)
			oRs("祈福事項")=SaveDataCheck(p8,NULL)
			oRs("時辰")=SaveDataCheck(p17,NULL)
			oRs.Update
		'----讀出會員資料表序號----------------------------------
		StrSql4="Select Top 1 * From 會員資料表 order by 序號 desc"
		Set oRs4=GetRST(StrSql4,3,1,2)
		id = oRs4("序號")
		
		'==========查詢燈座是否還有空位==============
		StrSql7 = "select Top 1 * from 點燈位置資料表 where 空位 = '1' order by 序號"
		Set oRs7=GetRST(StrSql7,3,1,2)
		'============================================
		'--------------------------------------------------------	
		'寫入訂單明細檔============================		
			StrSql2="Select Top 1 * From 訂單明細檔 "
			Set oRs2=GetRST(StrSql2,3,1,2)
			oRs2.AddNew
			oRs2("訂單編號")=p0
			oRs2("產品編號")=p11
			oRs2("產品名稱")=p12
			'---判別為捐香油錢或是其他服務
			if p11 = "7" then
			oRs2("價格")=p14
			Else
			oRs2("價格")=p13
			End if
			'------------------------------
			oRs2("數量")=1
			oRs2("申請人姓名")=p1
			oRs2("申請人地址")=p7
			oRs2("申請人性別")=p9
			oRs2("申請人生日")=ans
			oRs2("購買時間")= now_time
			oRs2("付款時間") = now_time
			oRs2("祈福事項")=p8
			oRs2("會員編號")=id		
			oRs2("郵遞區號")=p15
			oRs2("申請人年齡") = yearold
			oRs2("申請人時辰") = p17
			oRs2("經手人") = session("name")
			
			'為關聖燈
			'If p6 = "關聖燈" then
			If p11 = "5" then
			'狀態為已付款.以上傳及完成
				if s5 = "2" or s5 = "3" or s5 = "4" then 
					'假如點燈位置是空的
					if oRs2("點燈位置") = "" or isNull(oRs2("點燈位置")) then
						'假如已經沒位置的話=============
						if isNull(oRs7("位置名稱")) then
						oRs2("點燈位置") = ""
						'===============================
						Else 
						'給一個最前面的位置且將位置表更新=
						oRs2("點燈位置") = oRs7("位置名稱")
						oRs7("空位") = "0"
						oRs7.Update
						'=================================
						End if
					End if
				Else
				'狀態不為以上三個
				'釋出位置且更新點燈位置資料表
				'strSql6 = "select * from 點燈位置資料表 where 位置名稱 = '"&oRs2("點燈位置")&"' "
				'Set oRs6=GetRST(StrSql6,3,1,2)
				'oRs6("空位") = "1"
				'oRs6.Update
				oRs2("點燈位置") = ""
				End if
			End if	
			
			oRs2.Update				
			'StrSql4="Select Top 1 * From 訂單明細檔 order by 訂單序號 desc "
			'Set oRs4=GetRST(StrSql4,3,1,2)					
		'============================================
		'產生查詢序號======================
		Randomize
		sn = right("00000000" & int(10000000*Rnd),8)
		'==================================
		'寫入訂單主檔================================
			strSql3="Select Top 1 * from 訂單主檔 "
			Set oRs3=GetRST(StrSql3,3,1,2)
			oRs3.AddNew
			oRs3("訂單編號")=p0
			oRs3("會員編號")=id
			'=======需再增加會員資料
			'oRs3("申請人姓名")=p1
			'=======================
			'---判別為捐香油錢或是其他服務
			if p11 = "7" then
			oRs3("總額")= p14 * 1
			Else
			oRs3("總額")= p13 * 1
			End if
			'------------------------------
			oRs3("付款方式")=SaveDataCheck(s2,NULL)
			oRs3("訂單時間")= now_time
			oRs3("付款時間") = now_time
			oRs3("訂單狀態")=s5
			oRs3("查詢序號")=sn
			'判別付款方式名稱----------
			if s2 = "1" then
			payment = "線上刷卡"
			Elseif s2 = "2" then
			payment = "匯款轉帳"
			Elseif s2 = "3" then
			payment = "現金交易"
			End if
			oRs3("付款方式名稱")=payment
			'---------------------------
			'判別訂單狀態名稱----------
			if s5 = "1" then
			stat = "未處理"
			Elseif s5 = "2" then
			stat = "已付款"
			Elseif s5 = "3" then
			stat = "已上傳"
			Elseif s5 = "4" then
			stat = "完成"
			Elseif s5 = "5" then
			stat = "退貨"
			Elseif s5 = "0" then
			stat = "無效"
			End if
			oRs3("訂單狀態名稱")=stat
			'---------------------------
			oRs3.Update
			
			StrSql5="Select Top 1 * From 訂單主檔 order by 訂單序號 desc "
			Set oRs5=GetRST(StrSql5,3,1,2)
			Response.Redirect "upt.asp?p0="&oRs5("訂單序號")&"&flg=1"
		'=============================================
		
	End if
			
	
	'-- 取得訂單狀態
	StrSql="SELECT 序號,狀態名稱 FROM 訂單狀態檔 Where 序號 = '2' order by 排序 "
	Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	s5 = RsToOption(oRs,0,1,Cstr(s5),"","")
	'======================================================
	'-- 取得付款方式
	StrSql="SELECT 序號,分類名稱 FROM 訂單分類檔 where 序號='3' order by 序號 desc "
	Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	s2 = RsToOption(oRs,0,1,Cstr(s2),"","")
	'======================================================
	'-- 取得產品資料
	StrSql="SELECT 產品編號,產品名稱 FROM 產品資料表 where 隱藏 = '0'"
	Set oRs=ExecSQL_RTN_RST(StrSql,3,0,1)
	s1 = RsToOption(oRs,0,1,Cstr(s1),"","")
	
	
	'-- 關閉物件-------------------------------------------
	oRs.Close
	Set oRs=Nothing

%>

<html>
<head>
<title>訂單管理</title>
<meta http-equiv=Content-Type content="text/html; charset=big5">
<link rel=stylesheet href=../../_Css/Set.css>
<script type="text/javascript" src="../../_JScript/List.js"></script>
<script type="text/javascript" src="lunar.js"></script>
<script type="text/JavaScript" src="common.js"></script>
<script type="text/javascript" src="check.js"></script>

<script>
function start()
	{
		document.all.lunar.style.visibility='hidden';//visible
	}
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
<body onload=start();>
<form  Name="v" action="<%=strScript%>" id="v" method="post" onSubmit="return(mainx(this))">
	<Input type=hidden name=cmd value=set>
<table class=gridtable width=99% Height=450 border=1 cellspacing="0" cellpadding="0">
	<caption class=gridcaption style:margin:0>
		<table width=100%>
			<tr>
				<td class=gridtdcaption>訂單管理:新增
				<td align=right>
			</tr>
		</table>
	</caption>
	<tr>
	<td  valign=top>
		<table width=100% border=0 cellspacing="1" cellpadding="1">
			<tr>
				<th width="160" align="center">訂單狀態：</th>
				<td width="220" ><select name="s5"><%=s5%></select></td>
				<th width="160" align="center">祈福種類：</th>
				<td width="220" ><select name="s1" ><option></option><%=s1%></select></td>
			</tr>
			<tr>
				<th width="160" align="center">付款方式：</th>
				<td width="220" ><select name="s2" ><%=s2%></select></td>
				<th width="160" align="center">金額：</th>
				<td width="220" ><input type=text name="p14" size=10 value="300"  style="width:100%" ></td>
			</tr>
			<tr>
				<th width="160" align="center">祈福姓名：</th>
				<td width="220" ><input type=text name="p1" size=10 value="<%=p1%>"  style="width:100%" ></td>
				<th width="160" align="center">類別：</th>
				<td width="220" >國曆 <input type="radio" name="calen" value="1" checked = "true"> 農曆 <input type="radio" name="calen" value="2"></td>
			</tr>
			<tr>
				<th width="160" align="center">祈福性別：</th>
				<td width="220" >男 <input type="radio" name="sex" value="1" checked = "true"> 女 <input type="radio" name="sex" value="2"></td></td>
				<th width="160" align="center">民國-年：</th>
				<td width="220" ><input type=text name="year" size="4" maxlength="3" value=""></td>
			</tr>
			<tr>
			<th width="160" align="center">祈福地址：</th>
			<td width="220" ><input type=text name="p7" size=10 value="<%=p7%>"  style="width:100%" ></td>
			<th width="160" align="center">民國-月：</th>
				<td width="220" >
				<select name="month" size="1">
				<option value="1"> 01</option>
				<option value="2"> 02</option>
				<option value="3"> 03</option>
				<option value="4"> 04</option>
				<option value="5"> 05</option>
				<option value="6"> 06</option>
				<option value="7"> 07</option>
				<option value="8"> 08</option>
				<option value="9"> 09</option>
				<option value="10"> 10</option>
				<option value="11"> 11</option>
				<option value="12"> 12</option>
				</select></td>
			</tr>
			<tr>
				<th width="160" align="center">電話：</th>
				<td width="220" ><input type=text name="p4" size=3 value="<%=p4%>" >─<input type=text name="p5" size=8 value="<%=p5%>" ></td>
				<th width="160" align="center">民國-日：</th>
			<td width="220" ><select name="day" size="1">
			<option value="1">01 </option>
			<option value="2">02 </option>
			<option value="3">03 </option>
			<option value="4">04 </option>
			<option value="5">05 </option>
			<option value="6">06 </option>
			<option value="7">07 </option>
			<option value="8">08 </option>
			<option value="9">09 </option>
			<option value="10">10 </option>
			<option value="11">11 </option>
			<option value="12">12 </option>
			<option value="13">13 </option>
			<option value="14">14 </option>
			<option value="15">15 </option>
			<option value="16">16 </option>
			<option value="17">17 </option>
			<option value="18">18 </option>
			<option value="19">19 </option>
			<option value="20">20 </option>
			<option value="21">21 </option>
			<option value="22">22 </option>
			<option value="23">23 </option>
			<option value="24">24 </option>
			<option value="25">25 </option>
			<option value="26">26 </option>
			<option value="27">27 </option>
			<option value="28">28 </option>
			<option value="29">29 </option>
			<option value="30">30 </option>
			<option value="31">31 </option>
			</select><!--<input type=text name="p2" size=10 value="<p2%>" id="date" onClick="ShowCalendar()" style="Cursor:Hand" style="width:100%" readonly>--></td>
			</tr>
			<tr>
				<th width="160" align="center">行動電話：</th>
				<td width="220" ><input type=text name="p6" size=10 value="<%=p6%>"  style="width:100%" ></td>
				<th width="160" align="center">出生時辰：</th>
				<td width="220" ><select name="time" size="1">
				<option value="子"> 23:00~01:00 子時</option>
				<option value="丑"> 01:00~03:00 丑時</option>
				<option value="寅"> 03:00~05:00 寅時</option>
				<option value="卯"> 05:00~07:00 卯時</option>
				<option value="辰"> 07:00~09:00 辰時</option>
				<option value="巳"> 09:00~11:00 巳時</option>
				<option value="午"> 11:00~13:00 午時</option>
				<option value="未"> 13:00~15:00 未時</option>
				<option value="申"> 15:00~17:00 申時</option>
				<option value="酉"> 17:00~19:00 酉時</option>
				<option value="戌"> 19:00~21:00 戌時</option>
				<option value="亥"> 21:00~23:00 亥時</option>
				<option value="吉"> 00:00~23:59 吉時</option>
				</select></td>
			</tr>
			<tr>
				<th width="160" align="center">郵遞區號：</th>
				<td width="220" ><input type=text name="p15" size=10 value="<%=p15%>"  style="width:100%" ></td>
				<th width="160" align="center">E-mail：</th>
				<td width="220" ><input type=text name="p3" size=10 value="<%=p3%>"  style="width:100%" ></td>
			</tr>
			<tr>
				
				<th width="160" valign=top align="center">祈福事項：</th>
				<td Colspan=3 valign=top>
					<textarea  style="width:100%" rows="5" name="p8" maxlength="400" ><%=p8%></textarea>
				</td>
			</tr>
			<tr>
				<td class=gridtdtool colspan=4 align=Center><span class=errmsg><%=strMsg%></span><br>			
					<% 'IF ((Cstr(Session("ID")) = Cstr(p101)) OR (Session("AP_Power")=1)) Then 
					%>
				<input type="hidden" name="p11" value=<%=p11%>>
				<input type="hidden" name="p12" value=<%=p12%>>
				<input type="hidden" name="flg"/>
				<a class="lunar" id="lunar">
					<input type="text" name="type" value="0">
					<input type="text" name="answer" size="30">
				</a>
				<input class=gridsubmit type="submit" onClick="document.v.flg.value='ok';" value="新增訂單" Style="Cursor:Hand">	
				</td>
			</tr>
		</table>
	</td>
	<%'-- 上傳檔案-----------%>

	<td width=305 valign=top>
		<%'=strLink1%>
		<BR>
		<%'=strLink2%>
	</td>	
	
	</tr>
</table>
</form>


</body>
</html>
