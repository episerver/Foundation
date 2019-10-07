<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShortInfo.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.ShortInfo" %>
<table border="0" style="width: 50%;">
    <tr>
        <td style="width: 33%;" class="orderform-datatable-column">
			<table class="orderform-innertable">
			    <tr>
					<td class="orderform-label">
						<asp:Label ID="Label2" runat="server" Text="<%$ Resources:OrderStrings, Order_No %>"></asp:Label>:
					</td>
					<td class="orderform-field orderform-label-left">
						<asp:Label ID="lblOrderNo" runat="server"></asp:Label>
					</td>
				</tr>
				<tr>
					<td class="orderform-label">
						<asp:Label ID="Label3" runat="server" Text="<%$ Resources:OrderStrings, Order_Total %>"></asp:Label>:
					</td>
					<td class="orderform-field orderform-label-left">
						<asp:Label ID="lblTotal" runat="server"></asp:Label>
					</td>
				</tr>
			</table>
		</td>
		<td style="width: 33%;" class="orderform-datatable-column">
			<table class="orderform-innertable">
				<tr>
					<td class="orderform-label">
						<asp:Label ID="Label1" runat="server" Text="<%$ Resources:OrderStrings, Customer %>"></asp:Label>:
					</td>
					<td class="orderform-field orderform-label-left">
						<asp:Label ID="lblCustomer" runat="server"></asp:Label>
					</td>
				</tr>
				<tr>
					<td class="orderform-label">
						<asp:Label ID="Label6" runat="server" Text="<%$ Resources:OrderStrings, Status %>"></asp:Label>:
					</td>
					<td class="orderform-field orderform-label-left">
						<asp:Label ID="lblStatus" runat="server"></asp:Label>
					</td>
				</tr>
			</table>
		</td>
		<td  class="orderform-datatable-column" runat="server" id="cellParentOrder" visible="false">
			<table class="orderform-innertable">
				<tr>
					<td class="orderform-label">
						<asp:Label ID="Label4" runat="server" Text="<%$ Resources:OrderStrings, Original_Order %>"></asp:Label>:
					</td>
					<td class="orderform-field orderform-label-left">
						<asp:HyperLink ID="linkParentOrder" runat="server"> </asp:HyperLink>
					</td>
				</tr>
			</table>
		</td>
    </tr>
</table>