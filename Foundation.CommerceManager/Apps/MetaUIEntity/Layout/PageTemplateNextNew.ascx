<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageTemplateNextNew.ascx.cs" Inherits="Mediachase.UI.Web.Modules.PageTemplateNextNew" %>
<%@ register TagPrefix="ibn2" namespace="Mediachase.BusinessFoundation" Assembly="Mediachase.BusinessFoundation" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head id="headTag" >
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
		<title><%=Title%></title>
	</head>

	<body class="UserBackground" id="bodyTag" runat="server">
	<form id="frmMain" method="post" runat="server" onkeypress="return disableEnterKey(event);">
	<asp:ScriptManager ID="ScriptManager2" runat="server" EnablePartialRendering="true" ScriptMode="Debug" EnableScriptGlobalization = "true" EnableScriptLocalization="true" EnableCdn="true">
		<Services>
			<asp:ServiceReference Path="~/Apps/MetaUIEntity/WebServices/EntityGridService.asmx" InlineScript="true" />
		</Services>
	</asp:ScriptManager>
	
	<ibn2:LayoutExtender ID="LayoutExtender1" runat="server" TargetControlID="IbnMainLayout"></ibn2:LayoutExtender>
	<div style="border-left: 1px solid #6B92CE;height:100%;">
	<ibn2:McLayout runat="server" ID="IbnMainLayout" ClientOnResize="LayoutResizeHandler">
		<Items>
<%--		<Ibn:Dock runat="server" Anchor="top" EnableSplitter=False DefaultSize="30">		
				<DockItems>
				</DockItems>
			</Ibn:Dock>--%>
			<asp:placeholder id="phMain" runat="server"></asp:placeholder>
		</Items>
	</ibn2:McLayout>
	</div>
	<asp:UpdatePanel ID="CommandManagerUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">			
		<ContentTemplate>
			<ibn2:CommandManager runat="server" ContainerId="divContainer" ID="CM" />
		</ContentTemplate>
	</asp:UpdatePanel>
	<div id="divContainer" runat="server" />
	<input type="hidden" id="PageX" name="PageX" value="0" runat="server" /><input type="hidden" id="PageY" name="PageY" value="0" runat="server" />
	</form>
	<script type="text/javascript">
	//<![CDATA[
		document.onclick = HideFrames2;
	//]]>
	</script>
</body>
</html>