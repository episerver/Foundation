<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaymentTransactionTypeControl.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.PaymentTransactionTypeControl" %>
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td style="padding: 7px" >
			<table class="DataForm" width="100%">
				<col width="150" />
				<asp:Panel runat="server" ID="pnlTransactionType">
				<tr>
					<td class="FormLabelCell">
						Transaction type: 
					</td>
					<td>
						<asp:DropDownList runat="server" ID="ddlTransactionType" AutoPostBack="true" Width="150" />
					</td>
				</tr>
				</asp:Panel>
				<asp:Panel runat="server" ID="pnlOrderPayments">
					<tr>
						<td class="FormLabelCell">
							<asp:Label ID="ExistingPayments" runat="server" Text="<%$ Resources:OrderStrings, Existing_Payments %>">:</asp:Label> 
						</td>
						<td>
							<asp:RadioButtonList CssClass="radio" runat="server" ID="rblOrderPayments" AutoPostBack="true">
							</asp:RadioButtonList>
						</td>
					</tr>
				</asp:Panel>
			</table>
		</td>
	</tr>
</table>