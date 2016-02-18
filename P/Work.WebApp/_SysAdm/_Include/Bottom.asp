<%
Dim  errMsg
if errMsg = "" then
	errMsg = Request("errMsg")
end if
%>
<Script Language="JavaScript">
	var errMsg = '<%=errMsg%>';
	if(errMsg!=''){alert(errMsg)}
</Script>