<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaymentPlanView.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.PaymentPlanView" %>
<%@ Register TagPrefix="mc2" Namespace="Mediachase.Commerce.Manager.Apps.Common.Design"	Assembly="Mediachase.ConsoleManager" %>
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td style="padding: 7px">
			<mc2:BlockHeaderLight HeaderCssClass="ibn-toolbar-light" ID="bhl" runat="server"
				Title="Overview"></mc2:BlockHeaderLight>
			<table class="orderform-blockheaderlight-datatable">
				<tr>
					<td style="vertical-align:top; width:50%;">
						<table cellpadding="5">
							<tr>
								<td valign="top" style="width: 140px;font-weight:bold;text-align:right;">
									<asp:Label runat="server" ID="lblStatusText" Text="Status"></asp:Label>:
								</td>
								<td>
									<asp:Label runat="server" ID="lblStatus" Text=""></asp:Label>
								</td>
							</tr>
							<tr>
								<td valign="top" style="width: 140px; font-weight:bold; text-align:right;">
									<asp:Label runat="server" ID="lblCurrencyText" Text="Currency"></asp:Label>:
								</td>
								<td>
									<asp:Label runat="server" ID="lblCurrency" Text=""></asp:Label>
								</td>
							</tr>
							<tr>
								<td valign="top" style="width: 140px; font-weight:bold; text-align:right;">
									<asp:Label runat="server" ID="Label1" Text="Customer"></asp:Label>:
								</td>
								<td>
									<asp:Label runat="server" ID="lblCustomer" Text=""></asp:Label>
								</td>
							</tr>
						</table>
					</td>
					<td style="vertical-align:top; width:50%;">
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>