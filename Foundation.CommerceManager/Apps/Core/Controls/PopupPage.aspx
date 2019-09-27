<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopupPage.aspx.cs" Inherits="Mediachase.Commerce.Manager.Core.Controls.PopupPage" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><asp:Literal runat="server" Text="<%$ Resources:SharedStrings, EPiServer_Commerce_Manager %>"/></title>
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/ext-all.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/FontStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/FormStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/GeneralStyle.css") %>" type="text/css" rel="stylesheet" />
    
     <!-- EPi Style-->     
	<link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/EPi/Shell/Light/Shell-ext.css") %>" rel="stylesheet" type="text/css" />	
	
</head>
<body class="popupWindowBody">
    <form id="form1" runat="server">
    <div>
        <div class="popupWindowHeader">
            <asp:Label runat="server" ID="HeaderText" CssClass="popupWindowHeader"></asp:Label>
        </div>
        <div id="popupWindowContent">
            <asp:Label runat="server" ID="ErrorText" ForeColor="Red" Visible="false"></asp:Label>
            <asp:PlaceHolder ID="phMain" runat="server"></asp:PlaceHolder>
        </div>
    </div>
    </form>
</body>
</html>