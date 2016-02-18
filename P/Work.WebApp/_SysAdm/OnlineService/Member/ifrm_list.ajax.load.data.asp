<% Option Explicit%>
<!--#include file="../../_Function/Base_Function.asp"-->
<!--#include file="../../_Function/DB_Function.asp"-->
<!--#include file="../../_Function/RS_Function.asp"-->
<!--#include file="../../_Function/Power_Function.asp"-->
<!--#include file="../../_Function/JSON_2.0.4.asp"-->
<%
On Error Resume Next
Dim oRs,strRs
Dim strScript,strPageNo,strQS
Dim strCon,intCon
Dim strUID,intUID,strUnit
Dim strCID,intCID,strCat
Dim strSDate,strEDate
Dim strKey
Dim aryDel,aryIns
Dim StrSql,WhereObj,strwhere
Dim p0,MasterID
Dim s1
Dim strSelCbo
Dim ReturnSYSJson,message
Dim RecordCount
	message = ""
	strScript=Request.ServerVariables("SCRIPT_NAME")
	strPageNo=Trim(Request("pageno"))
	
	MasterID = Trim(Request("MasterID"))	'訂單編號	
	strQS="MasterID=" & MasterID
	
	'-- 加入查詢條件---------------------------------------
	
	Set WhereObj = new WhereSet
	WhereObj.Add "戶長SN","=",MasterID,""
	strwhere = WhereObj.ToWhereString
	If strwhere <> "" Then strwhere = " Where A.is_delete=0 AND " & strwhere
	
	'加span 是給Jquery 做西元轉換-1911
	StrSql =  "SELECT	A.序號,A.姓名,C.是否二 as 戶長,   B.性別,'<span class=""LBirthday"">' + A.生日 + '</span>' as 農曆生日  ,A.生肖 , A.手機, A.電話尾碼 as 電話 " _
		& "FROM	會員資料表 AS A INNER JOIN " _
		& "性別表 AS B ON A.性別 = B.序號 INNER JOIN TF表 AS C ON A.Is戶長 = C.Value" _
		& strwhere & " Order By A.Is戶長 desc,A.生日 "
	
	'======================================================
	'-- 取得資料錄-----------------------------------------
	Set oRS=ExecSQL_RTN_RST(StrSql,3,1,2)
	ErrCheck "SQL Err" & StrSql
	'======================================================
	'-- 組合表單-------------------------------------------
	aryIns=Array("新增訂單","RedireOrder()","福燈","RedireOrder_Fortune()","新增家庭成員","AddNewMember()","主副斗","RedireOrderM()","大中小斗","RedireOrderMBMS()")
	aryDel=Array("刪除")
	
	ErrCheck "Is Err"
	'"ifrm_upt_member.asp?pageno="&request("pageno")&"&"&strQS&"&p0="
	
	strRS=RsToTableAJAX(oRs,"ifrm_del.member.asp",strQS,0,1,"function:AjaxGetMemberData","",5,"<div align=left>家庭成員(戶長ID：" & MasterID & ")</div>","99%", _
			Array("10%","15%","7%","15%","15%","15%","15%"), _
			aryDel,0,"",1,"../../_Images/_pager/", _
			aryIns,"OpenWindow_No","HighLight_Yes","SelectAll_Yes")
	RecordCount = oRs.RecordCount
	oRs.Close
	Set oRs = Nothing	
	ErrCheck "RsToTableAJAX"
	'======================================================
	Set ReturnSYSJson = MakeJson() 
	ReturnSYSJson("result") = true
	ReturnSYSJson("message") = message
	ReturnSYSJson("GridList") = strRS
	ReturnSYSJson("RecordCount") = RecordCount
	ReturnSYSJson.Flush
%>