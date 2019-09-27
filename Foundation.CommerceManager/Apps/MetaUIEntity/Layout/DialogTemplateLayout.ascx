<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DialogTemplateLayout.ascx.cs" Inherits="Mediachase.UI.Web.Modules.DialogTemplateLayout" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ Register TagPrefix="mc" Namespace="Mediachase.BusinessFoundation" Assembly="Mediachase.BusinessFoundation" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%=Title%></title>
    <script type="text/javascript" defer="defer">
    	function pageLoad() {
    		var obj = document.getElementById('ibn_divWithLoadingRss');
    		if (obj) {
    			obj.style.display = 'none';
    		}
    	}
    </script>    
    <!-- EPi Style-->
    <link href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/EPi/Shell/Light/Shell-ext.css") %>" rel="stylesheet" type="text/css" />
    
    <link type="text/css" rel="stylesheet" href="<%# CommerceHelper.GetAbsolutePath("~/Apps/MetaDataBase/styles/ext-all2.css")%>" />
    <link type="text/css" rel="stylesheet" href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/FormStyle.css")%>" />
    <link type="text/css" rel="stylesheet" href="<%# CommerceHelper.GetAbsolutePath("~/Apps/Shell/styles/css/IbnLayout.css")%>" />
</head>
<body class="ibn-WhiteBg">
	
    <form id="frmMain" runat="server" method="post">
	<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" 
		EnableScriptGlobalization="true" EnableScriptLocalization="true" LoadScriptsBeforeUI="false" EnableCdn="true">
	</asp:ScriptManager>
	<div id='ibn_divWithLoadingRss' style="position: absolute; left: 0px; top: 0px; height: 100%; width: 100%; background-color: White; z-index: 10000">
		<div style="left: 40%; top: 40%; height: 30px; width: 200px; position: absolute; z-index: 10001">
			<div style="position: relative;  z-index: 10002">
				<img style="position: absolute; left: 40%; top: 40%; z-index: 10003" src='<%= ResolveClientUrl("~/Apps/Shell/Styles/images/Shell/loading_rss.gif") %>' border='0' />
			</div>
		</div>
	</div>
	<mc:LayoutExtender ID="LayoutExtender1" runat="server" TargetControlID="IbnMainLayout"></mc:LayoutExtender>
	<div style="height:100%; overflow:auto;">
		<mc:McLayout runat="server" ID="IbnMainLayout" ClientOnResize="LayoutResizeHandler">
			<Items>
				<asp:placeholder id="phMain" runat="server"></asp:placeholder>
			</Items>
		</mc:McLayout>
	</div>		
	<asp:UpdatePanel ID="CommandManagerUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">			
		<ContentTemplate>
			<mc:CommandManager ID="cm" runat="server" ContainerId="divContainer" />
		</ContentTemplate>
	</asp:UpdatePanel>
	<div id="divContainer" runat="server"></div>
    </form>
</body>
</html>