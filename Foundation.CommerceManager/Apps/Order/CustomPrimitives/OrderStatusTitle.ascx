<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderStatusTitle.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.CustomPrimitives.OrderStatusTitle" %>
<table border="0">
	<tr>
		<td valign="top" style="font-weight:bold; text-align:right;padding:5px;">
			<asp:Label ID="Label8" runat="server" Text="<%$ Resources:OrderStrings, Status %>"></asp:Label>:
		</td>
		<td valign="top" style="padding:5px;">
			<asp:Label ID="lblOrderStatus" runat="server"></asp:Label>
		</td>
	</tr>
	<tr>
		<td valign="top" style="font-weight:bold; text-align:right;padding:5px;">
			<asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Coupon_Code %>"></asp:Label>:
		</td>
		<td valign="top" style="padding:5px;">
			<asp:Label ID="lblCouponCode" runat="server"></asp:Label>
		</td>
	</tr>
</table>