<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrgItemsDeleteConfirm.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Customer.Modules.OrgItemsDeleteConfirm" %>
<div style="padding:10px;">
	<asp:Literal runat="server" ID="DeleteTypeText"></asp:Literal>
	<br /><br />
	<div style="padding-left:40px;">
		<asp:RadioButtonList runat="server" ID="DeleteTypeList" RepeatDirection="Vertical">
			<asp:ListItem Value="0" Selected="True"></asp:ListItem>
			<asp:ListItem Value="1"></asp:ListItem>
		</asp:RadioButtonList>
	</div>
	<br /><br />
	<div style="text-align:center">
		<asp:Button runat="server" ID="OkButton" Text="<%$Resources: SharedStrings, OK %>" CssClass="ibn-button" Width="80px" OnClick="OkButton_Click"/>&nbsp;&nbsp;
		<asp:Button runat="server" ID="CancelButton" Text="<%$Resources: SharedStrings, Cancel %>" Width="80px"/>
	</div>
</div>