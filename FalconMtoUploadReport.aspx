<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FalconMtoUploadReport.aspx.cs" Inherits="WebApplication1.FalconMtoUploadReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Falcon MTO Upload Report</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>Falcon MTO Upload Report</h2>
        
        <asp:Label ID="lblStartDate" runat="server" Text="Start Date:" />
        <asp:TextBox ID="txtStartDate" runat="server" /><br />
        <asp:Label ID="lblEndDate" runat="server" Text="End Date:" />
        <asp:TextBox ID="txtEndDate" runat="server" /><br />
        <asp:Label ID="lblCustNumber" runat="server" Text="Customer Number:" />
        <asp:TextBox ID="txtCustNumber" runat="server" /><br />
        <asp:Label ID="lblMtoNumber" runat="server" Text="MTO Number:" />
        <asp:TextBox ID="txtMtoNumber" runat="server" /><br />
        
        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" /><br /><br />
        
        <br /><br />
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />
    </div>
    </form>
</body>
</html>
