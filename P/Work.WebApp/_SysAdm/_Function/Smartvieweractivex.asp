<%
'This file contains the HTML code to instantiate the Smart Viewer ActiveX.      
'                                                                     
'You will notice that the Report Name parameter references the RDCrptserver10.asp file.
'This is because the report pages are actually created by RDCrptserver10.asp.
'RDCrptserver10.asp accesses session("oApp"), session("oRpt") and session("oPageEngine")
'to create the report pages that will be rendered by the ActiveX Smart Viewer.
'

'Dim strRptSP
'	strRptSP=Request.ServerVariables("SCRIPT_NAME")	

'	strRptSP=Left(strRptSP,InStrRev(strRptSP,"/")) & "../../_scripts/RDCrptserver10.asp"
		

%>
<HTML>
<HEAD>
<TITLE>Crystal Reports ActiveX Viewer</TITLE>
<!--<meta http-equiv="refresh" content="2";url="<%=Request.ServerVariables("SCRIPT_NAME")	%>">-->
</HEAD>
<BODY BGCOLOR=C6C6C6 ONUNLOAD="this.focus();CallDestroy();" leftmargin=0 topmargin=0 rightmargin=0 bottommargin=0>
<OBJECT ID="CRViewer"
	CLASSID="CLSID:A1B8A30B-8AAA-4a3e-8869-1DA509E8A011"
	WIDTH=100% HEIGHT=99%
	CODEBASE="../../../ActiveXViewer10/10/ActiveXViewer.cab#Version=10,0,0,280" VIEWASTEXT>
<PARAM NAME="EnableRefreshButton" VALUE=1>
<PARAM NAME="EnableGroupTree" VALUE=1>
<PARAM NAME="DisplayGroupTree" VALUE=0>
<PARAM NAME="EnablePrintButton" VALUE=1>
<PARAM NAME="EnableExportButton" VALUE=1>
<PARAM NAME="EnableDrillDown" VALUE=1>
<PARAM NAME="EnableSearchControl" VALUE=1>
<PARAM NAME="EnableAnimationControl" VALUE=1>
<PARAM NAME="EnableZoomControl" VALUE=1>
</OBJECT>

<SCRIPT LANGUAGE="VBScript">
<!--
Sub Window_Onload
	On Error Resume Next
	Dim webBroker
	Set webBroker = CreateObject("CrystalReports10.WebReportBroker.1")
	if ScriptEngineMajorVersion < 2 then
		window.alert "IE 3.02 users on NT4 need to get the latest version of VBScript or install IE 4.01 SP1. IE 3.02 users on Win95 need DCOM95 and latest version of VBScript, or install IE 4.01 SP1. These files are available at Microsoft's web site."
	else
		Dim webSource
		Set webSource = CreateObject("CrystalReports10.WebReportSource.1")
		webSource.ReportSource = webBroker
		webSource.URL = "../../_Function/RDCrptserver10.asp"

		'webSource.URL =strRptSP
		webSource.PromptOnRefresh = True
		CRViewer.ReportSource = webSource
	end if
	CRViewer.ViewReport
End Sub
-->
</SCRIPT>

<script language="javascript">
function CallDestroy()
{
	//window.open("Cleanup.asp");
}
</script>

</BODY>
</HTML>
