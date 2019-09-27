<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderShippingMethodEdit.ascx.cs"
	Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.OrderShippingMethodEdit" %>
<%@ Register TagPrefix="mc2" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>
<div class="popup-outer">
	<asp:Label runat="server" ID="lblErrorInfo" Style="color: Red"></asp:Label>
	<asp:Panel runat="server" ID="FormPanel">
		<table width="100%" class="DataForm">
			<tr>
				<td class="FormLabelCell">
					<asp:Label ID="Label5" runat="server" Text="<%$ Resources:OrderStrings, Shipping_Method %>"></asp:Label>:
				</td>
				<td class="FormFieldCell">
					<asp:DropDownList runat="server" ID="ddlShippingMethods" Width="250" DataValueField="ShippingMethodId"
						DataTextField="DisplayName">
					</asp:DropDownList>
					<asp:RequiredFieldValidator ID="ShippingMethodRequiredValidator" runat="server" ControlToValidate="ddlShippingMethods"
						Display="Dynamic" ValidationGroup="ShippingMethodEditValidationGroup">*</asp:RequiredFieldValidator>
				</td>
			</tr>
		</table>
	</asp:Panel>
</div>
