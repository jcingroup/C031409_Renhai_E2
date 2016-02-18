<% Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/Power_Function.asp"-->
<%

If hasDelPower=0 Then GoAway

Dim oRs,oRs1,strRs,oRs3,oRs4
Dim strScript,strPageNo,strQS
Dim strCon,intCon
Dim strUID,intUID,strUnit
Dim strCID,intCID,strCat
Dim strYear,strMonth,strDay
Dim strKey,strSql,strSql1,strSql2,strSql3,strSql4

Dim Did0,Did1
Dim p
Dim strPath
Dim deletePath
Dim s1,s2,s3,s4,s5,s6,s7,strsel,intsel

	strPageNo=Trim(Request("pageno"))
	'strQS = request("strQS")
	s1=Trim(Request("s1"))	'訂單編號	
	s2=Trim(Request("s2"))	'付款方式	
	s3=Trim(request("s3"))	'訂單金額最小值
	s4=Trim(request("s4"))	'訂單金額最大值
	s5=Trim(Request("s5"))	'狀態		
	s6=Trim(Request("s6"))	'銀行帳號
	s7=Trim(Request("s7"))	'經手人
	strSel=Trim(Request("Sel"))	'產品編號

	If Not IsNumeric(strSel) then intSel="" Else intSel=Cint(strSel)
	strQS="s1="&s1&"&s2="&s2&"&s3="&s3&"&s4="&s4&"&s5="&s5&"&s6="&s6&"&s7="&s7&"&sel="&intSel&""

'	'-- 刪除檔案-------------------------------------------
'	For Each p in Request.Form("did0")
'		strPath=Server.MapPath("../_upload/"&getScriptPath(2)&"/"&p)
'		deletePath strPath,0
'	Next'==================================================

	'-- 刪除資料-------------------------------------------
	Did0=Trim(Request.Form("did0"))	
		
	
	If Len(Did0)>0 Then
		'找出訂單序號
		strSql1 = "select * from 訂單主檔 where 訂單序號 IN ("&Did0&") "
		Set oRS1=ExecSQL_RTN_RST(StrSql1,3,1,2)
		
		'找出訂單編號(bug:目前刪多筆時只會找出最後一筆的編號)
		strSql3 = "select * from 訂單明細檔 where 訂單編號 IN ("&oRS1("訂單編號")&") "
		Set oRS3=ExecSQL_RTN_RST(StrSql3,3,1,2)
		
		response.write strSql1 & "<BR>" & strSql3 & "<BR>" 
		'response.end
		
		
		if not isNull(oRS3("點燈位置")) or oRS3("點燈位置") = "" then
			response.redirect "list.asp" 
		Else
		'先將點燈位置資料表的燈位釋放才刪除此筆訂單
			strSql4 = "select * from 點燈位置資料表 where 位置名稱 IN ("&oRS3("點燈位置")&") "
			Set oRs4=GetRST(StrSql4,3,1,2)
			oRs4("空位") = "1"
			oRs4.Update
		End if
	
			
		strSql="Delete 訂單主檔 WHERE 訂單序號 IN ("&Did0&")"
		
		ExecSql strSql
		
		strSql2="Delete 訂單明細檔 WHERE 訂單編號 IN ("&oRs1("訂單編號")&")"
		ExecSql strSql2
			
	End If'================================================
	
	
	response.redirect "list.asp?"&strQS&"" 
%>