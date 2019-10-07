<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BillingAddress.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.BillingAddress" %>
<%@ Register TagPrefix="mc2" Namespace="Mediachase.Commerce.Manager.Apps.Common.Design"	Assembly="Mediachase.ConsoleManager" %>
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td style="padding: 7px">
			<mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" ID="bhl" runat="server"
				Title="Customer Information"></mc2:BlockHeaderLight>
			<table class="orderform-blockheaderlight-datatable">
				<tr>
					<td valign="top" style="width: 140px; font-weight:bold;text-align:right;">
						<asp:Label runat="server" ID="lblCustomerText" Text="<%$ Resources:SharedStrings, Name %>"></asp:Label>:
					</td>
					<td>
						<asp:Label runat="server" ID="lblCustomer" Text=""></asp:Label>
					</td>
				</tr>
				<tr>
					<td valign="top" style="width: 140px; font-weight:bold;text-align:right;">
						<asp:Label runat="server" ID="Label2" Text="<%$ Resources:SharedStrings, Address %>"></asp:Label>:
					</td>
					<td>
						<asp:Label runat="server" ID="lblAddress" Text=""></asp:Label>
					</td>
				</tr>
				<tr>
					<td valign="top" style="width: 140px; font-weight:bold;text-align:right;">
						<asp:Label runat="server" ID="Label3" Text="<%$ Resources:SharedStrings, Phone Number %>"></asp:Label>:
					</td>
					<td>
						<asp:Label runat="server" ID="Label4" Text=""></asp:Label>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>