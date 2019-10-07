<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderAffiliate.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.CustomPrimitives.OrderAffiliate" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
			<asp:Label ID="Label6" runat="server" Text="<%$ Resources:SharedStrings, Affiliate %>"></asp:Label>:
		</td>
		<td style="padding: 5px;">
			<asp:DropDownList runat="server" ID="AffiliateList" DataTextField="FriendlyName"
				DataValueField="Id" Width="250px">
				<asp:ListItem Value="" Text="<%$ Resources:SharedStrings, select_affiliate %>"></asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
</table>