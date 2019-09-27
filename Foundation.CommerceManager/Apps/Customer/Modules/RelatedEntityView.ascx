<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelatedEntityView.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Customer.Modules.RelatedEntityView" %>
<%@ Register Src="~/Apps/Customer/Modules/EcfListViewControlWithoutDockTop.ascx" TagName="EcfListViewControl" TagPrefix="cm" %>
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td style="padding:5px;">
			<div>
				<cm:EcfListViewControl id="MyListView" runat="server" ShowTopToolbar="false" LayoutResizeEnable="false"></cm:EcfListViewControl>
				<asp:LinkButton ID="AddItemsLink" runat="server" Visible="false" OnClick="AddItemsLink_Click"></asp:LinkButton>
			</div>
		</td>
	</tr>
</table>
