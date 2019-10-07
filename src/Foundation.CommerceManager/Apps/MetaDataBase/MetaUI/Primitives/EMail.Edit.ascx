<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EMail.Edit.ascx.cs" Inherits="Mediachase.Ibn.Web.UI.MetaUI.Primitives.EMail_Edit" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%" class="ibn-propertysheet">
	<tr>
		<td>
			<asp:TextBox id="txtValue" runat="server" CssClass="text" Wrap="False" Width="99%"></asp:TextBox>
			<br />
			<asp:regularexpressionvalidator id="vldValue_RegEx" runat="server" ErrorMessage="Invalid Email Address" ControlToValidate="txtValue" 
				ValidationExpression="^[\w-_./:\?&=]+" Display="Dynamic"></asp:regularexpressionvalidator>
		</td>
		<td style="width:20px;">
			<asp:RequiredFieldValidator id="vldValue_Required" runat="server" ErrorMessage="*" ControlToValidate="txtValue"	Display="Dynamic"></asp:RequiredFieldValidator>
		</td>
	</tr>
</table>
