<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShoppingCartStatus.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.CustomPrimitives.ShoppingCartStatus" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td colspan="2" style="text-align:center;padding:5px;">
			<asp:button id="btnEditCurrency" runat="server" style="width:150px;" text="<%$ Resources:OrderStrings, Edit_Currency %>" />
		</td>
	</tr>
    <tr>
		<td colspan="2" style="text-align:center;padding:5px;">
			<asp:button id="btnEditMarket" runat="server" style="width:150px;" text="Edit Market" />
		</td>
	</tr>
	<tr>
		<td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
			<asp:Label runat="server" ID="Label1" Text="<%$ Resources:OrderStrings, Cart_No_Sign %>"></asp:Label>:
		</td>
		<td style="padding: 5px;">
			<asp:Label ID="lblNo" runat="server"></asp:Label>
		</td>
	</tr>
	<tr>
		<td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
			<asp:Label runat="server" ID="Label2" Text="<%$ Resources:OrderStrings, Created_Date %>"></asp:Label>:
		</td>
		<td style="padding: 5px;">
			<asp:Label ID="lblCreated" runat="server"></asp:Label>
		</td>
	</tr>
    <tr>
		<td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
			<asp:Label runat="server" ID="Label5" Text="<%$ Resources:OrderStrings, Order_Level_Discounts %>"></asp:Label>:
		</td>
		<td style="padding: 5px;">
			<asp:Label ID="lblOrderLevelDiscount" runat="server"></asp:Label>
		</td>
	</tr>
	<tr>
		<td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
			<asp:Label runat="server" ID="Label3" Text="<%$ Resources:OrderStrings, Cart_Total %>"></asp:Label>:
		</td>
		<td style="padding: 5px;">
			<asp:Label ID="lblOrderTotal" runat="server"></asp:Label>
		</td>
	</tr>
	<tr>
		<td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
			<asp:Label ID="lblCurrency" runat="server" Text="<%$ Resources:SharedStrings, Currency %>"></asp:Label>:
		</td>
		<td style="padding: 5px;">
			<asp:Label ID="lblCurrencyValue" runat="server"></asp:Label>
		</td>
	</tr>
    <tr>
		<td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
			<asp:Label ID="Label4" runat="server" Text="<%$ Resources:SharedStrings, Market %>"></asp:Label>:
		</td>
		<td style="padding: 5px;">
			<asp:Label ID="lblMarket" runat="server"></asp:Label>
		</td>
	</tr>
</table>