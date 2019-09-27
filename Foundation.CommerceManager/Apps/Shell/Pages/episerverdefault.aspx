<%@ Page Language="C#" AutoEventWireup="True" Inherits="EPiServer.Commerce.Manager.Apps.Shell.Pages._default" CodeBehind="episerverdefault.aspx.cs" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ Register TagPrefix="mc" TagName="MetaToolbar" Src="~/Apps/Core/Controls/MetaToolbar.ascx" %>
<%@ Register TagPrefix="mc" TagName="leftTemplate" Src="~/Apps/Shell/Modules/leftTemplate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <asp:Literal ID="Literal1" runat="server" /></title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    
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
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/ext-all.css") %>" rel="stylesheet" type="text/css" />
	<!-- EPi Style-->
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/EPi/Shell/Light/Shell-ext.css") %>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src='<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Scripts/ManagementClient2.js")%>'></script>

    <style type="text/css">
        table.imageMiddle tbody tr td em button { background-position: 4pt 2px !important; }
    </style>
    <!--[if IE]>
	<style type="text/css">
		table.imageMiddle tbody tr td em button
		{
			background-position: 0pt 2px !important;
		}	
	</style>
	<![endif]-->
</head>

<script type="text/javascript">
    var CSManagementClient = new ManagementClient('right', 'center_div');
    CSManagementClient.BASE_URL = '<%= ResolveUrl("~/Apps/Shell/Pages/") %>';
    CSManagementClient.HELP_URL = '<%= ConfigurationManager.AppSettings.Get("epi:HelpUrl") %>';
    
    function GetManagementClient() {
        return CSManagementClient;
    }

    function pageLoad() {
        var obj = document.getElementById('ibnMain_divWithLoadingRss');
        if (obj) {
            obj.style.display = 'none';
        }

        initLayout(); // main layout
        initIframe(); // history
        //frame load
        var bookmarkedFrameSrc = Sys.Application._state.right;
        if (bookmarkedFrameSrc)
            mainLayout_initialFrameSrc = bookmarkedFrameSrc;
        else
            mainLayout_initialFrameSrc = escapeWithAmp('<%=defaultLink%>');

        mainLayout_initialize();
    }

    function escapeWithAmp(str) {
        var re = /&/gi;
        var ampEncoded = "%26";
        return escape(str.replace(re, ampEncoded));
    }
</script>

<body>
    <form id="form1" runat="server">
    <div id='ibnMain_divWithLoadingRss' style="position: absolute; left: 0px; top: 0px;
        height: 100%; width: 100%; background-color: White; z-index: 10000">
        <div style="left: 40%; top: 40%; height: 30px; width: 200px; position: absolute;
            z-index: 10001">
            <div style="position: relative; z-index: 10002">
                <img alt="" style="position: absolute; left: 40%; top: 40%; z-index: 10003; border: 0"
                    src='<%= ResolveClientUrl("~/Apps/Shell/styles/Images/Shell/loading_rss.gif") %>' />
            </div>
        </div>
    </div>
    <asp:ScriptManager ID="sm1" runat="server" EnablePartialRendering="true" ScriptMode="Debug"
        EnableScriptGlobalization="true" EnableScriptLocalization="true" EnableHistory="true" EnableCdn="true" />
    <IbnWebControls:CommandManager ID="cm1" runat="server" ContainerId="containerDiv" />
    <div id="containerDiv" runat="server">
    </div>
    <div id="up_div" style="display: none">
        <mc:MetaToolbar runat="server" ID="MainMetaToolbar" ClassName="" ViewName="TopMenu" />
    </div>
    <div id="left_div">
        <mc:leftTemplate ID="leftCtrl" runat="server"></mc:leftTemplate>
    </div>
    <span id="SaveStatusCtrl" class="SaveStatus" style="visibility: hidden"></span>
    <div id="center_div" style="height: 100%;">
        <iframe frameborder="0" scrolling="auto" name="right" id="right" width="100%" marginheight="0"
            marginwidth="0" src='<%=ResolveUrl("~/Apps/Shell/Pages/Empty.html") %>'></iframe>
    </div>

    <script type="text/javascript">
        Ext.onReady(function() {
            //GetManagementClient().Initialize();
        });
    </script>

    </form>
</body>
</html>
