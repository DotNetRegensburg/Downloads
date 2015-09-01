<%@ Page Title="" Language="C#" EnableEventValidation="true" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="UserGroup.DemoProject.WebApp._default" %>
<%@ Register assembly="UserGroup.DemoProject.WebApp" namespace="UserGroup.DemoProject.WebApp.controls" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <cc1:TestControl ID="TestControl1" runat="server" />

<div class="SprFav"></div>

<asp:Button ID="Button2" runat="server" Text="Button" onclick="Button2_Click" />
</asp:Content>
