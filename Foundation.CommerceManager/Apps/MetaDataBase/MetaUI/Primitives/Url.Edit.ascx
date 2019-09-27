<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Url.Edit.ascx.cs" Inherits="Mediachase.Ibn.Web.UI.MetaUI.Primitives.Url_Edit" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%" class="ibn-propertysheet">
	<tr>
		<td>
			<asp:TextBox id="txtValue" runat="server" CssClass="text" Wrap="False" Width="99%"></asp:TextBox>
			<asp:CustomValidator id="uniqValue_Required" OnServerValidate="uniqValue_Required_ServerValidate" runat="server" ValidateEmptyText="true" ControlToValidate="txtValue" Display="Dynamic"></asp:CustomValidator>
		</td>
		<td style="width:20px;">
			<asp:RequiredFieldValidator id="vldValue_Required" runat="server" ErrorMessage="*" ControlToValidate="txtValue"	Display="Dynamic"></asp:RequiredFieldValidator>
			<asp:regularexpressionvalidator id="vldValue_RegEx" runat="server" ErrorMessage="*" ControlToValidate="txtValue" ValidationExpression="^[\w-_./:\?&=]+" Display="Dynamic"></asp:regularexpressionvalidator>
		</td>
	</tr>
</table>
