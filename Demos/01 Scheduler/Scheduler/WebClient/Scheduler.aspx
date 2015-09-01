<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Scheduler.aspx.cs" Inherits="Scheduler" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Scheduler Demo</title>
    <style type="text/css">
        body{font-family:arial,sans-serif;font-size:120%;}
    </style>
</head>
<body>
    <form id="mainForm" runat="server">
        <h1>ManualWorkflowScheduler Demo</h1>
        <asp:Button ID="btnStartWorkflow" runat="server" OnClick="btnStartWorkflow_Click"
            Text="Start Useless Workflow" />
        <br />
        <br />
        <asp:Label ID="lblOutput" runat="server"></asp:Label>
    </form>
</body>
</html>