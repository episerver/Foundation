<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Duration.Edit.ascx.cs" Inherits="Mediachase.Ibn.Web.UI.MetaUI.Primitives.Duration_Edit" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%" class="ibn-propertysheet">
	<tr>
		<td>
			<asp:TextBox id="txtValue" runat="server" CssClass="text" Wrap="False" Width="99%"></asp:TextBox>
		</td>
		<td style="width:20px;">
			<asp:RequiredFieldValidator id="vldValue_Required" runat="server" ErrorMessage="*" ControlToValidate="txtValue"	Display="Dynamic"></asp:RequiredFieldValidator>
			<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtValue" ErrorMessage="*" ValidationExpression="^\s*(([0-9]*[:\s]+([0-5]?[0-9]))|((2[0-4])|([01]?[0-9])))\s*$" Display="Dynamic"></asp:RegularExpressionValidator>
		</td>
	</tr>
</table>