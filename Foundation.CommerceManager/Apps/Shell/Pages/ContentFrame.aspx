<%@ Page Language="C#" AutoEventWireup="true" Inherits="ContentFrame" EnableEventValidation="false" CodeBehind="ContentFrame.aspx.cs" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ Register Src="~/Apps/Core/ErrorModule.ascx" TagName="ErrorModule" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:SharedStrings, EPiServer_Commerce_Manager %>" /></title>
    
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/css/ext-all.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/ComboBoxStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/css/BusinessFoundation/Theme.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/css/dashboard.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/css/FileUploaderStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/css/FilterBuilder.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/css/FontStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/css/FormStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/css/GeneralStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/css/grid.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/css/IbnLayout.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/css/MultiPage.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/css/reports.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/css/tabs.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/css/TabStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/css/TreeStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/GridStyle.css") %>" type="text/css" rel="stylesheet" />
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Styles/LoginStyle.css") %>" type="text/css" rel="stylesheet" />

     <script type="text/javascript" src="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Scripts/Shell/ManagementClient2.js")%>"></script>
     <script type="text/javascript" src="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/Scripts/Shell/ManagementClientProxy.js")%>"></script>    


    <!-- EPi Style START -->
	<link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/EPi/Shell/Light/Shell-ext.css") %>" rel="stylesheet" type="text/css" />	
	<script type="text/javascript" src="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/EPi/Shell/Light/jquery.min.js") %>"></script>
	<script type="text/javascript" src="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/EPi/Shell/Light/Shell-ext.js") %>"></script>
	<!-- EPi Style END -->
	
    <script type="text/javascript">
		function escapeWithAmp(str)
		{
			var re = /&/gi;
			var ampEncoded = "%26";
			return escape(str.replace(re, ampEncoded));
		}
	
		getEcfMainFrame = function()
		{
            var win = window;
            var retVal = null;
            // use try catch to ignore permission denied error when call cross-frame scripting
            try {
                while (win.parent != win) {
                    if (typeof (CSManagementClient) != "undefined")
                        retVal = win;
                    win = win.parent;
                }
            }
            catch (err) {
            }

            return retVal;
        };
		
        // this page should be inside frame
		if (top == self)
		{
			var url = '<%# ResolveUrl("~/Apps/Shell/Pages/default.aspx") %>';
			url += '#right=' + encodeURIComponent(escapeWithAmp(location.href));
			if (getEcfMainFrame() != null)
				getEcfMainFrame().location.href = url;
			else
				top.location.href = url;
		 }

        function pageLoad(sender, args) {
            var obj = document.getElementById('ibnMain_divWithLoadingRss');
            if (obj) {
                obj.style.display = 'none';
            }

            if (Sys.Browser.agent == Sys.Browser.Firefox) {
                initFixTables();
            }
            
            if (window.childPageLoad) {
                window.childPageLoad(sender, args);
            }

            //------------------------------------------------------------
            // Call reLoadCss function from Shell-ext.js to overwrite style sheet
            //------------------------------------------------------------ 
            reLoadCss();
        }

        function initFixTables() {
            setTimeout(fixTables, 200);
            return true;
        }

        function fixTables() {
            var colTables = document.getElementsByTagName('TABLE');
            for (var i = 0; i < colTables.length; i++)
                if (colTables[i].style.display == 'inline-block')
                colTables[i].style.display = '';
        }
    </script>

</head>
<body scroll="auto" class="view">
    <form id="form1" runat="server" style="height: 100%;">
    <div id='ibnMain_divWithLoadingRss' style="position: absolute; left: 0px; top: 0px;
        height: 100%; width: 100%; z-index: 10000">
        <div style="left: 40%; top: 40%; height: 30px; width: 200px; position: absolute;
            z-index: 100001">
            <div style="position: relative; z-index: 100002">
                <img alt="" style="position: absolute; left: 30%; top: 40%; z-index: 100003; border: 0"
                    src='<%= ResolveClientUrl("~/Apps/Shell/styles/Images/Shell/loading_rss.gif") %>' />
            </div>
        </div>
    </div>
    <asp:ScriptManager runat="server" ID="ScriptManager1" EnablePageMethods="true" EnablePartialRendering="true" 
		EnableScriptGlobalization="true" EnableScriptLocalization="false" LoadScriptsBeforeUI="true"
		ScriptMode="Auto" EnableCdn="true">
        <Services>
			<asp:ServiceReference Path="~/Apps/Core/Controls/WebServices/EcfListViewExtenderService.asmx"
				InlineScript="true" />
 		</Services>
        <Scripts>
            <asp:ScriptReference Path="~/Apps/Shell/Scripts/AjaxControlToolkit/Resources.js"/>
        </Scripts>
        </asp:ScriptManager>       
    <uc1:ErrorModule ID="ErrorModule1" runat="server"></uc1:ErrorModule>
    <asp:HiddenField runat="server" ID="_action" Value="" />
    <asp:HiddenField runat="server" ID="_params" Value="" />
    <IbnWebControls:LayoutExtender ID="LayoutExtender1" runat="server" TargetControlID="IbnMainLayout">
    </IbnWebControls:LayoutExtender>
    <IbnWebControls:McLayout runat="server" ID="IbnMainLayout">
        <Items>
            <asp:PlaceHolder EnableViewState="true" ID="ContentHolderControl" runat="server">
            </asp:PlaceHolder>
        </Items>
    </IbnWebControls:McLayout>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="cmPanel"
        EnableViewState="true" DynamicLayout="true">
        <ProgressTemplate> 
        <div style="left: 40%; top: 40%; height: 30px; width: 200px; position: absolute;
            z-index: 100001">
            <div style="position: relative; z-index: 100002; ">
                <img alt="" style="position: absolute; left: 30%; top: 40%; z-index: 100003; border: 0"
                    src='<%= ResolveClientUrl("~/Apps/Shell/styles/Images/Shell/loading_rss.gif") %>' />
            </div>
        </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel runat="server" ID="cmPanel" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="height: 0px;">
                <IbnWebControls:CommandManager ID="cmContent" runat="server" ContainerId="containerDiv" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="containerDiv" runat="server" style="height: 0px">
    </div>
    </form>
        <!--[if IE 8]>
        <script type="text/javascript">
        // get rid of redundant vertical scrolling in IE8
        if ((Sys.Browser.agent == Sys.Browser.InternetExplorer) && (Sys.Browser.version == 8)) {
            var divLayout = $get('IbnMainLayout_divLayoutBase');
            if (divLayout != null && document.body.clientHeight > 0) {
                divLayout.style.height = document.body.clientHeight - 1; /* 1 here is padding-top for .LayoutBase */
            }
        }
        </script>
        <![endif]-->
</body>
</html>
