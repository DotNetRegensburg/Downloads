<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Newsletter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Newsletter</title>
    <style type="text/css">
        body{font-family:arial, sans-serif;font-size:120%;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <h1>Newsletter Administration</h1>
        <asp:MultiView ID="mView" runat="server" ActiveViewIndex="0">
        <asp:View ID="SubscriptionFormView" runat="server">
        Your Email:
        <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
        <asp:RegularExpressionValidator ID="valRegex" runat="server" ControlToValidate="txtEmail"
            ErrorMessage="Invalid email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator><br />
        <asp:RadioButtonList ID="rblSubscribe" runat="server" RepeatDirection="Horizontal"
            RepeatLayout="Flow">
            <asp:ListItem Selected="True" Value="Subscribe">Subscribe</asp:ListItem>
            <asp:ListItem Value="Unsubscribe">Unsubscribe</asp:ListItem>
        </asp:RadioButtonList><br />
        <br />
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" /><br />
        </asp:View>
        <asp:View ID="MessageView" runat="server">
            <asp:Panel runat="server" ID="messagePanel">
                <asp:Label runat="server" ID="lblMessage" />
            </asp:Panel>
        </asp:View>
        </asp:MultiView>
    </form>
</body>
</html>
