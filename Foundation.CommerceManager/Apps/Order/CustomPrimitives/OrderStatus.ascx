<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderStatus.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.CustomPrimitives.OrderStatus" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td colspan="2" style="text-align:center;padding:5px;">
			<asp:Button ID="btnEditInfo" runat="server" style="width:150px;" Text="<%$ Resources:OrderStrings, Edit_Currency %>" />
		</td>
	</tr>
	<tr>
		<td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
			<asp:Label runat="server" ID="Label1" Text="<%$ Resources:OrderStrings, Order_No_Sign %>"></asp:Label>:
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
			<asp:Label runat="server" ID="Label4" Text="<%$ Resources:OrderStrings, Items %>"></asp:Label>:
		</td>
		<td style="padding: 5px;">
			<asp:Label ID="lblSubtotal" runat="server"></asp:Label>
		</td>
	</tr>
	<tr>
		<td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
			<asp:Label runat="server" ID="Label12" Text="<%$ Resources:OrderStrings, Line_Item_Discounts %>"></asp:Label>:
		</td>
		<td style="padding: 5px;">
			<asp:Label ID="lblLineItemDiscounts" runat="server"></asp:Label>
		</td>
	</tr>
	<tr>
		<td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
			<asp:Label runat="server" ID="Label10" Text="<%$ Resources:OrderStrings, Order_Level_Discounts %>"></asp:Label>:
		</td>
		<td style="padding: 5px;">
			<asp:Label ID="lblOrderLevelDiscounts" runat="server"></asp:Label>
		</td>
	</tr>
    <tr>
		<td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
			<asp:Label runat="server" ID="Label11" Text="<%$ Resources:OrderStrings, Discounts_Total %>"></asp:Label>:
		</td>
		<td style="padding: 5px;">
			<asp:Label ID="lblDiscounts" runat="server"></asp:Label>
		</td>
	</tr>
    <tr>
		<td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
			<asp:Label runat="server" ID="lblTotalExcludingShippingAndTax" Text=""></asp:Label>:
		</td>
		<td style="padding: 5px;">
			<asp:Label ID="lblTotal" runat="server"></asp:Label>
		</td>
	</tr>
	<tr>
		<td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
			<asp:Label runat="server" ID="Label9" Text="<%$ Resources:OrderStrings, Shipping_Total %>"></asp:Label>:
		</td>
		<td style="padding: 5px;">
			<asp:Label ID="lblShipping" runat="server"></asp:Label>
		</td>
	</tr>
	<tr>
		<td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
			<asp:Label runat="server" ID="Label8" Text="<%$ Resources:OrderStrings, Shipping_Discounts %>"></asp:Label>:
		</td>
		<td style="padding: 5px;">
			<asp:Label ID="lblShippingDiscounts" runat="server"></asp:Label>
		</td>
	</tr>
    
    <tr>
		<td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
			<asp:Label runat="server" ID="Label14" Text="<%$ Resources:SharedStrings, Handling_Total %>"></asp:Label>:
		</td>
		<td style="padding: 5px;">
			<asp:Label ID="lblHandling" runat="server"></asp:Label>
		</td>
	</tr>
    <tr>
		<td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
			<asp:Label runat="server" ID="Label7" Text="<%$ Resources:OrderStrings, Taxes_Total %>"></asp:Label>:
		</td>
		<td style="padding: 5px;">
			<asp:Label ID="lblTaxes" runat="server"></asp:Label>
		</td>
	</tr>
	<tr>
		<td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
			<asp:Label runat="server" ID="Label3" Text="<%$ Resources:OrderStrings, Order_Total %>"></asp:Label>:
		</td>
		<td style="padding: 5px;">
			<asp:Label ID="lblOrderTotal" runat="server"></asp:Label>
		</td>
	</tr>
	<tr>
		<td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
			<asp:Label runat="server" ID="Label6" Text="<%$ Resources:OrderStrings, Balance_Due %>"></asp:Label>:
		</td>
		<td style="padding: 5px;">
			<asp:Label ID="lblBalanceDue" runat="server"></asp:Label>
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
			<asp:Label ID="Label13" runat="server" Text="<%$ Resources:OrderStrings, Order_Market_Name %>"></asp:Label>:
		</td>
		<td style="padding: 5px;">
			<asp:Label ID="lblMarketId" runat="server"></asp:Label>
		</td>
	</tr>
</table>