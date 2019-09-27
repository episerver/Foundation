<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShipmentSummary.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.ShipmentSummary" %>
<%@ Register TagPrefix="mc2" Namespace="Mediachase.Commerce.Manager.Apps.Common.Design"
	Assembly="Mediachase.ConsoleManager" %>
<%@ Register src="~/Apps/Core/Controls/ButtonsHolder.ascx" tagname="ButtonsHolder" tagprefix="uc1" %>
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td style="padding: 5px;width:300px;" valign="top">
			<mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" ID="bhl" runat="server"
				Title="<%$ Resources:OrderStrings, Shipment_Summary %>"></mc2:BlockHeaderLight>
			<table class="orderform-blockheaderlight-datatable">
				<tr>
					<td valign="top" style="width: 170px; font-weight:bold; text-align:right;">
						<asp:Label ID="Label4" runat="server" Text="<%$ Resources:OrderStrings, Item_Subtotal %>"></asp:Label>:
					</td>
					<td valign="top">
						<asp:Label ID="lblItemsSubTotal" runat="server"></asp:Label>
					</td>
				</tr>
				<tr>
					<td valign="top" style="font-weight:bold; text-align:right;">
						<asp:Label ID="Label5" runat="server" Text="<%$ Resources:OrderStrings, Order_Level_Discounts %>"></asp:Label>:
					</td>
					<td valign="top">
						<asp:Label ID="lblOrderLevelDiscount" runat="server"></asp:Label>
					</td>
				</tr>
				<tr>
					<td valign="top" style="font-weight:bold; text-align:right;">
						<asp:Label ID="Label9" runat="server" Text="<%$ Resources:OrderStrings, Order_Subtotal %>"></asp:Label>:
					</td>
					<td valign="top">
						<asp:Label ID="lblOrderSubTotal" runat="server"></asp:Label>
					</td>
				</tr>
				<tr>
					<td valign="top" style="font-weight:bold; text-align:right;">
						<asp:Label ID="Label7" runat="server" Text="<%$ Resources:OrderStrings, Shipping_Cost %>"></asp:Label>:
					</td>
					<td valign="top">
						<asp:Label ID="lblShippingCost" runat="server"></asp:Label>
					</td>
				</tr>
                <tr>
					<td valign="top" style="font-weight:bold; text-align:right;">
						<asp:Label ID="Label2" runat="server" Text="<%$ Resources:OrderStrings, Shipping_Discount %>"></asp:Label>:
					</td>
					<td valign="top">
						<asp:Label ID="lblShippingDiscount" runat="server"></asp:Label>
					</td>
				</tr>
                <tr>
					<td valign="top" style="font-weight:bold; text-align:right;">
						<asp:Label ID="Label3" runat="server" Text="<%$ Resources:OrderStrings, Shipping_Tax %>"></asp:Label>:
					</td>
					<td valign="top">
						<asp:Label ID="lblShippingTax" runat="server"></asp:Label>
					</td>
				</tr>
				<tr>
					<td valign="top" style="font-weight:bold; text-align:right;">
						<asp:Label ID="Label1" runat="server" Text="<%$ Resources:OrderStrings, Shipment_Total %>"></asp:Label>:
					</td>
					<td valign="top" style="font-weight:bold; ">
						<asp:Label ID="lblShipTotal" runat="server"></asp:Label>
					</td>
				</tr>
			</table>
		</td>
		<td style="padding: 3px;width:250px;" valign="top">
			<mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" ID="bh2" runat="server"
				Title="<%$ Resources:OrderStrings, Shipment_Status %>"></mc2:BlockHeaderLight>
			<table class="orderform-blockheaderlight-datatable">
				<tr>
					<td valign="top" style="width: 90px; font-weight:bold; text-align:right;">
						<asp:Label ID="Label8" runat="server" Text="<%$ Resources:SharedStrings, Status %>"></asp:Label>:
					</td>
					<td valign="top">
						<asp:Label ID="lblShipStatus" runat="server"></asp:Label>
					</td>
				</tr>
				<tr>
					<td colspan="2" style="padding:7px;text-align:center;" align="center">
						<uc1:ButtonsHolder ID="btnShipmentStatus" runat="server" />
					</td>
				</tr>
			</table>
		</td>
        <td style="padding: 3px;width:250px;" valign="top">
            <mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" ID="bh3" runat="server"
                Title="<%$ Resources:OrderStrings, Pickup_Orders %>" />
            <table class="orderform-blockheaderlight-datatable">
                <tr>
                    <td style="padding:7px;text-align:center;" align="center">
                        <uc1:ButtonsHolder ID="CompletePickupButton" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
		<td style="padding: 3px;width:250px;" valign="top">
			<mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" ID="BlockHeaderLight1" runat="server"
				Title="<%$ Resources:OrderStrings, Returns_Exchanges %>"></mc2:BlockHeaderLight>
			<table class="orderform-blockheaderlight-datatable">
				<tr>
					<td style="padding:7px;text-align:center;" align="center">
					    <uc1:ButtonsHolder ID="ReturnExchangeButtonsHolder" runat="server" />
					</td>
				</tr>
			</table>
		</td>
		<td>&nbsp;</td>
	</tr>
</table>