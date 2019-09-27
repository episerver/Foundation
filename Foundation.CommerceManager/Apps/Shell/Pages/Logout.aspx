<%@ Page Language="C#" AutoEventWireup="true" Inherits="Foundation.CommerceManager.Logout" CodeBehind="~/Logout.aspx.cs" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>EPiServer Commerce Manager Logout Page</title>
    
     <style type="text/css">
		table.login
		{
		    border-style: none;
		    border-width: 0;
		    width: 100%;
		}
		table.login td
		{
		    padding-top: 1px;
		    padding-bottom: 1px;
		}
	</style>

    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/FontStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/FormStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/GeneralStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/LoginStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/BusinessFoundation/Theme.css") %>" type="text/css" rel="stylesheet" />
    
    <!-- EPi Style START -->
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/EPi/Shell/Light/Shell-ext.css") %>" rel="stylesheet" type="text/css" />
	<!-- EPi Style END -->	
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <div id="epi-ecf-banner">
	        <div style="float:left;width:350px;padding:5px;">
	        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/"><img alt="EPiServer Commerce" align="absmiddle" src="../EPi/Shell/Light/Resources/EPiServer_ECF-NEG.png" width="300" height="40" /></asp:HyperLink></div>
        </div>

        <div class="LoginPanel">
        <div class="LoginTable">
            <table cellspacing="0" cellpadding="0" align="left">
                <tr>
                    <td>
                        <h1>Logout</h1>
                    </td>
                </tr>
                <tr>
                    <td class="text" width="90%">
                        <br />
                        You have been successfully logged out from EPiServer Commerce.
                        <br />
		                Close the browser to end your session.
                        <br />
                        <br />
                    </td>
                </tr>
                <tr valign="top">
                    <td width="550">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>
        <div class="LoginFooter">
        <asp:HyperLink Target="_blank" runat="server" NavigateUrl="http://www.episerver.com"><%=Mediachase.Commerce.FrameworkContext.ProductName%></asp:HyperLink>
        <br />
        Version:
        <%=Mediachase.Commerce.FrameworkContext.ProductVersionDesc%><br /><br />
        &copy; <%=DateTime.Now.Year.ToString()%> EPiServer AB.  All Rights Reserved.
    </div>
    </form>
</body>
</html>
