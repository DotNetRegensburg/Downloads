<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Tracking.aspx.cs" Inherits="Tracking" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Workflow Tracking</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>Workflow Tracking</h1>
        <asp:GridView ID="trackingGrid" runat="server" AutoGenerateColumns="False" CellPadding="4"
            DataKeyNames="WorkflowInstanceID" ForeColor="#333333" GridLines="None">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="WorkflowInstanceId" DataNavigateUrlFormatString="TrackingDetails.aspx?id={0}"
                    HeaderText="Instance ID" DataTextField="WorkflowInstanceId" />
                <asp:BoundField DataField="Initialized" HeaderText="Time" />
            </Columns>
            <RowStyle BackColor="#EFF3FB" />
            <EditRowStyle BackColor="#2461BF" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
    
    </div>
    </form>
</body>
</html>
