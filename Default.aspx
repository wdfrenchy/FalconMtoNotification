<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FalconMtoNotification._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Falcon MTO Notification</title>
    <style type="text/css">
        .red {color: #F00; font-weight: bold; font-size: 120%;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>Falcon MTO Shipment Entry</h2>
        
        <asp:Label ID="lblRefNumber" runat="server" Text="Reference Number:" />
        <span class="red">*</span>
        <asp:TextBox ID="txtRefNumber" runat="server" /><br />
        
        <asp:Label ID="lblEta" runat="server" Text="ETA:" />
        <span class="red">*</span>
        <asp:TextBox ID="txtEta" runat="server" /><br />
        
        <asp:Label ID="lblDepartureDate" runat="server" Text="Departure Date:" />
        <span class="red">*</span>
        <asp:TextBox ID="txtDepartureDate" runat="server" /><br />
        
        <asp:Label ID="lblDepartureAddress" runat="server" Text="Departure Address:" />
        <span class="red">*</span>
        <asp:TextBox ID="txtDepartureAddress" runat="server" /><br />
        
        <asp:Label ID="lblCarrier" runat="server" Text="Carrier:" />
        <span class="red">*</span>
        <asp:TextBox ID="txtCarrier" runat="server" /><br />
        
        <asp:Label ID="lblShipmentType" runat="server" Text="Shipment Type (Ocean/Air):" />
        <span class="red">*</span>
        <asp:TextBox ID="txtShipmentType" runat="server" /><br />
        
        <asp:Label ID="lblContactDetails" runat="server" Text="Contact Details (If Any):" />
        <asp:TextBox ID="txtContactDetails" runat="server" /><br />
        
        <asp:Label ID="lblTrackingNumber" runat="server" Text="Tracking Number (If Available):" />
        <asp:TextBox ID="txtTrackingNumber" runat="server" /><br /><br />
        
        <span class="red">*All fields except "Contact Details" and "Tracking Number" are required.</span><br /><br />
        
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" /><br /><br />
        
        <br /><br />
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />
    </div>
    </form>
</body>
</html>
