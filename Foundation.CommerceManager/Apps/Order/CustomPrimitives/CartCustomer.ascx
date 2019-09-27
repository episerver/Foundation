<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CartCustomer.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.CustomPrimitives.CartCustomer" %>
<%@ Register TagPrefix="mc" TagName="objectDD" Src="~/Apps/MetaUIEntity/Modules/EntityDropDown.ascx" %>
<%@ Reference Control="~/Apps/MetaUIEntity/Modules/EntityDropDown.ascx" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
			<asp:Label ID="Label3" runat="server" Text="<%$ Resources:SharedStrings, Customer %>"></asp:Label>:
		</td>
		<td style="padding: 5px;">
			<mc:objectDD ID="CustomerDD" runat="server" Width="250px" />
			<asp:Label ID="lblCustomerName" runat="server"></asp:Label>
		</td>
	</tr>
	<tr runat="server" id="customerRow">
		<td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
			<asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Customer_ID %>"></asp:Label>:
		</td>
		<td style="padding: 5px;width:300px;">
			<asp:Label ID="lblCustomerId" runat="server"></asp:Label>
		</td>
		<td style="padding: 5px;" rowspan="2" valign="top">
			<asp:button id="btnEditProfile" runat="server" style="width:150px;" Text="<%$ Resources:SharedStrings, Open_Customer_Profile %>" title="<%$ Resources:SharedStrings, Open_Customer_Profile %>" />
		</td>
	</tr>
	<tr>
		<td valign="top" style="width: 140px; font-weight:bold; text-align:right;padding: 5px;">
			<asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, Email_Address %>"></asp:Label>:
		</td>
		<td style="padding: 5px;">
			<asp:Label ID="lblEmailAddress" runat="server"></asp:Label>
		</td>
	</tr>
</table>