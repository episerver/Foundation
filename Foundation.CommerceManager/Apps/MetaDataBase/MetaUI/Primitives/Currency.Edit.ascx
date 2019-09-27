<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaUI.Primitives.Currency_Edit" Codebehind="Currency.Edit.ascx.cs" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%" class="ibn-propertysheet">
	<tr>
		<td style="padding-right:3px;">
			<asp:TextBox id="txtValue" runat="server" CssClass="text" Wrap="False" Width="99%"></asp:TextBox>
		</td>
		<td style="width:20px;">
			<asp:RequiredFieldValidator id="vldValue_Required" runat="server" ErrorMessage="*" ControlToValidate="txtValue"	Display="Dynamic"></asp:RequiredFieldValidator>
			<asp:RangeValidator ID="vldValue_Range" Runat="server" MinimumValue="-922337203685477" MaximumValue="922337203685477" Type="Currency" ErrorMessage="*" ControlToValidate="txtValue" Display=Dynamic></asp:RangeValidator>
		</td>
	</tr>
</table>
