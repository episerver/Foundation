<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaUI.Primitives.Guid_Edit" Codebehind="Guid.Edit.ascx.cs" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%" class="ibn-propertysheet">
	<tr>
		<td style="padding-right:3px;">
			<asp:TextBox id="txtValue" runat="server" CssClass="text" Wrap="False" Width="99%"></asp:TextBox>
		</td>
		<td style="width:20px;">
			<asp:RequiredFieldValidator id="vldValue_Required" runat="server" ErrorMessage="*" ControlToValidate="txtValue"	Display="Dynamic"></asp:RequiredFieldValidator>
			<asp:CustomValidator runat="server" ID="vldCustom" ErrorMessage="*" ControlToValidate="txtValue" Display="dynamic" OnServerValidate="vldCustom_ServerValidate"></asp:CustomValidator>
		</td>
	</tr>
</table>
