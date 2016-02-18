<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Code.aspx.cs" Inherits="DotWeb._SysAdm.MainDoor.Body.Code" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div><a href="<%=ResolveUrl("~/Orders#/edit/gorders?member_id=10000001") %>">新增一般訂單(學勤)</a></div>
            <div><a href="<%=ResolveUrl("~/Orders#/edit/forders?member_id=10000001") %>">新增福燈訂單(學勤)</a></div>
            <div><a href="<%=ResolveUrl("~/Orders") %>">訂單管理</a></div>
            <div><a href="<%=ResolveUrl("~/MemberMark") %>">會員重複標記</a></div>
            <div><a href="<%=ResolveUrl("~/TempleMember") %>">契子會會員管理</a></div>
            <div><a href="<%=ResolveUrl("~/ExcelReport/downloadExcel_PostMember?year=2015") %>">會員郵寄標籤列印</a></div>
        </div>
    </form>
</body>
</html>
