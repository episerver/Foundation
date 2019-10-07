<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShippingInformation.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.ShippingInformation" %>
<%@ Register TagPrefix="mc2" Namespace="Mediachase.Commerce.Manager.Apps.Common.Design"
	Assembly="Mediachase.ConsoleManager" %>
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td style="padding: 5px;">
			<mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" ID="bhl" runat="server" Title="<%$ Resources:OrderStrings, Shipping_Information %>"></mc2:BlockHeaderLight>
			<table class="orderform-blockheaderlight-datatable">
                <colgroup style="width: 300px; text-align: left;"></colgroup>
                <colgroup></colgroup>
                <tbody>
                    <tr>
					    <td valign="top"><asp:Label ID="Label4" runat="server" Text="<%$ Resources:SharedStrings, Shipping_Address %>" Font-Bold="true"></asp:Label>:</td>
					    <td></td>
				    </tr>
				    <tr>
					    <td><asp:Literal Mode="Encode" ID="lblShipAddress" runat="server"></asp:Literal></td>
					    <td><asp:Button ID="btnEditAddress" runat="server" width="150" Text="<%$ Resources:SharedStrings, Edit_Shipping_Address %>" /></td>
				    </tr>
				    <tr>
					    <td valign="top"><asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Shipping_Method %>" Font-Bold="true"></asp:Label>:</td>
					    <td></td>
				    </tr>
				    <tr>
					    <td><asp:Label ID="lblShipMethod" runat="server"></asp:Label></td>
					    <td><asp:Button ID="btnEditShippingMethod" runat="server" width="150" Text="<%$ Resources:SharedStrings, Edit_Shipping_Method %>" /></td>
				    </tr>
                    <tr>
					    <td valign="top"><asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, Shipping_Warehouse %>" Font-Bold="true"></asp:Label>:</td>
					    <td></td>
                    </tr>
                    <tr>
					    <td><asp:Label ID="lblShipWarehouse" runat="server"></asp:Label></td>
				    </tr>
                </tbody>
			</table>
		</td>
	</tr>
</table>