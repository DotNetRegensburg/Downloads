<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TrackingDetails.aspx.cs" Inherits="TrackingDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Tracking Details</title>
</head>
<body>
    <form id="form1" runat="server">
    <h1>Workflow Instance Tracking Details</h1>
    <h2>ID: <%= this.InstanceId %></h2>
        <asp:GridView ID="trackingDetailView" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <Columns>
                <asp:BoundField DataField="TrackingWorkflowEvent" HeaderText="Event" />
                <asp:BoundField DataField="EventDateTime" HeaderText="Timestamp" />
            </Columns>
            <RowStyle BackColor="#EFF3FB" />
            <EditRowStyle BackColor="#2461BF" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <p><asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="Tracking.aspx">Back</asp:HyperLink></p>
    </form>
</body>
</html>
