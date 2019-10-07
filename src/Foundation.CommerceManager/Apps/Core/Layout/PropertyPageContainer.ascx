<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PropertyPageContainer.ascx.cs" Inherits="Mediachase.Ibn.Web.UI.Layout.PropertyPageContainer" %>
<asp:UpdatePanel runat="server" ID="PropertyPagePanel" RenderMode="block" UpdateMode="Conditional" ChildrenAsTriggers="true">
	<ContentTemplate>
		<asp:HiddenField runat="server" ID="hfPrimary" />		
		<div runat="server" id="mainContainer" class="popup-backgroundContainer" style="top: 100px; height: 200px; left: 30%;">
		</div>		
		<div runat="server" id="buttonContainer" class="popup-backgroundContainer" style="top: 300px; left: 30%; border-top: solid 1px Black;">
			<input type="Button" runat="server" id="btnSaveReal" value="<%$ Resources:SharedStrings, Save %>" />
			<input type="button" runat="server" id="btnCancel" value="<%$ Resources:SharedStrings, Cancel %>" />
		</div>
		<div runat="server" id="backgroundContainer" style="position: absolute; left: 0px; top:0px; height: 100%; width: 100%; background-color: #666666; display: none; z-index: 991; opacity: 0.4; filter: alpha(opacity=40);">
		</div>
	</ContentTemplate>
	<Triggers>
		<asp:PostBackTrigger ControlID="btnSave" />
	</Triggers>	
</asp:UpdatePanel>
<asp:Button runat="server" ID="btnSave" Text="<%$ Resources:SharedStrings, Save %>" />
