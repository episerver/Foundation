<%@ Page Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Core.Controls.DialogPage" Codebehind="DialogPage.aspx.cs"%> 
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ register TagPrefix="ibn" namespace="Mediachase.BusinessFoundation" Assembly="Mediachase.BusinessFoundation" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="headTag" runat="server">
    <title></title>
    <script type="text/javascript">
        // this page should be inside frame
        if (top == self) top.location.href = CSManagementClient.ResolveUrl("~/default.aspx");
    </script>
    
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/ext-all.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Core/Layout/Styles/ext-all2-workspace.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Core/Layout/Styles/workspace.css") %>" rel="stylesheet" type="text/css" />        
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/ComboBoxStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/BusinessFoundation/Theme.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/dashboard.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/FileUploaderStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/FilterBuilder.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/FontStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/FormStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/GeneralStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/grid.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/IbnLayout.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/MultiPage.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/reports.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/tabs.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/TabStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/TreeStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/GridStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/LoginStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/ToolbarStyle.css") %>" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="<%# CommerceHelper.GetAbsolutePath("~/Apps/MetaDataBase/Scripts/jquery-1.2.1.min.js")%>"></script>

    <script type="text/javascript" src="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Scripts/Shell/EPiServerManagementClient2.js") %>"></script>
    <script type="text/javascript" src="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Scripts/Shell/ManagementClientProxy.js") %>"></script>

    <!-- EPi Style-->
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/EPi/Shell/Light/Shell-ext.css") %>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/EPi/Shell/Light/Shell-ext.js")%>"></script>
</head>
<body style="overflow: hidden;">
    <form id="DialogForm1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnablePartialRendering="true" EnableScriptGlobalization="true" LoadScriptsBeforeUI="true" ScriptMode="debug" EnableCdn="true">
            <Scripts>
                <asp:ScriptReference Path="~/Apps/Shell/Scripts/AjaxControlToolkit/Resources.js" />
            </Scripts>
        </asp:ScriptManager>
        <ibn:CommandManager runat="server" ContainerId="divContainer" ID="CM" />
        <div id="divContainer" runat="server" style="height: 0px;" />
        <asp:Panel runat="server" ID="contentPanel" style="height:100%; overflow:auto;"></asp:Panel>
        <script type="text/javascript">            Ext.BLANK_IMAGE_URL = '../../shell/pages/images/s.gif';</script>
    </form>
	<script type="text/javascript">
	    //------------------------------------------------------------
	    // Call reLoadCss function from Shell-ext.js to overwrite style sheet
	    //------------------------------------------------------------
	    $(document).ready(function () {
	        // Because of the old version of jquery 1.2.1 loading page will take more time. The reLoadCss function should run
            // after the page is fully loaded.
	        setTimeout(reLoadCss, 10);
	    });
	</script>
</body>
</html>
