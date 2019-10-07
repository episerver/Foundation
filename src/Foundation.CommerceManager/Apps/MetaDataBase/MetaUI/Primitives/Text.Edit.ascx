<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Text.Edit.ascx.cs" Inherits="Mediachase.Ibn.Web.UI.MetaUI.Primitives.Text_Edit" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%" class="ibn-propertysheet">
	<tr>
		<td>
			<asp:TextBox id="txtValue" runat="server" CssClass="text" Wrap="False" Width="99%"></asp:TextBox>
		</td>
		<td style="width:20px;">
			<asp:CustomValidator id="uniqValue_Required" OnServerValidate="uniqValue_Required_ServerValidate" runat="server" ValidateEmptyText="true" ControlToValidate="txtValue" Display="Dynamic"></asp:CustomValidator>
			<asp:RequiredFieldValidator id="vldValue_Required" runat="server" ErrorMessage="*" ControlToValidate="txtValue"	Display="Dynamic"></asp:RequiredFieldValidator>
		</td>
	</tr>
</table>
