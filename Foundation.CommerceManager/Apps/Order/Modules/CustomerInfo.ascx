<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerInfo.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.CustomerInfo" %>
<%@ Register TagPrefix="mc2" Namespace="Mediachase.Commerce.Manager.Apps.Common.Design"	Assembly="Mediachase.ConsoleManager" %>
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td style="padding: 7px">
			<mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" ID="bhl" runat="server"
				Title="Customer Information"></mc2:BlockHeaderLight>
			<table class="orderform-blockheaderlight-datatable">
				<tr>
					<td style="vertical-align:top; width:50%;">
						<table cellpadding="5">
							<tr>
					<td valign="top" style="width: 140px; font-weight:bold;text-align:right;">
						<asp:Label runat="server" ID="Label1" Text="Customer ID"></asp:Label>:
					</td>
					<td>
						<asp:Label runat="server" ID="lblCustId" Text=""></asp:Label>
					</td>
				</tr>
				<tr>
					<td valign="top" style="width: 140px; font-weight:bold;text-align:right;">
						<asp:Label runat="server" ID="lblCustomerText" Text="Customer"></asp:Label>:
					</td>
					<td>
						<asp:Label runat="server" ID="lblCustomer" Text=""></asp:Label>
					</td>
				</tr>
				<tr>
					<td valign="top" style="width: 140px; font-weight:bold;text-align:right;">
						<asp:Label runat="server" ID="Label2" Text="<%$ Resources:SharedStrings, Email_Address %>"></asp:Label>:
					</td>
					<td>
						<asp:Label runat="server" ID="lblEmailAddress" Text=""></asp:Label>
					</td>
				</tr>
				<tr>
					<td valign="top" style="width: 140px; font-weight:bold;text-align:right;">
						<asp:Label runat="server" ID="Label3" Text="Phone Number"></asp:Label>:
					</td>
					<td>
						<asp:Label runat="server" ID="Label4" Text="512-478-1234"></asp:Label>
					</td>
				</tr>
				</table>
					</td>
					<td style="vertical-align:top; width:50%;">
						<asp:Button ID="btnCancelOrder" runat="server" Text="<%$ Resources:SharedStrings, Open_Customer_Profile %>" Width="130px" Height="25px" />
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>