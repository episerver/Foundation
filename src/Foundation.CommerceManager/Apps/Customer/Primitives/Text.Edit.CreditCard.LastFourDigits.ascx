<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Text.Edit.CreditCard.LastFourDigits.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Customer.Primitives.Text_Edit_CreditCard_LastFourDigits" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%" class="ibn-propertysheet">
	<tr>
		<td>
			<asp:TextBox id="txtValue" runat="server" CssClass="text" Wrap="False" Width="99%"></asp:TextBox>
		</td>
		<td style="width:20px;">
			<asp:RequiredFieldValidator id="vldValue_Required" runat="server" ErrorMessage="*" ControlToValidate="txtValue"	Display="Dynamic"></asp:RequiredFieldValidator>
			<asp:RegularExpressionValidator id="lastFourDigits_validation" runat="server"  ControlToValidate="txtValue" ErrorMessage="*" ValidationExpression="\d{4}"></asp:RegularExpressionValidator>
			<asp:CustomValidator runat="server" ID="customValidator" ControlToValidate="txtValue"  ErrorMessage="*"  ></asp:CustomValidator>
		</td>
	</tr>
</table>