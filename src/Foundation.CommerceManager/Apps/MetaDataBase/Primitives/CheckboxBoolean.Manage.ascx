<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Primitives.CheckboxBoolean_Manage" Codebehind="CheckboxBoolean.Manage.ascx.cs" %>
<table cellpadding="3" cellspacing="1" border="0" width="100%" class="ibn-propertysheet">
	<tr>
		<td class="ibn-label" width="120px">
		<asp:Literal ID="Literal1" runat="server" Text="<%$Resources : GlobalFieldManageControls, CheckBoxLabel%>" />:
		</td>
		<td>
			<asp:TextBox Runat="server" ID="txtCheckBoxLabel" Width="100%" MaxLength="100"></asp:TextBox>
		</td>
		<td width="20px">
			<asp:RequiredFieldValidator runat="server" ID="rfvCheckBoxLabel" ControlToValidate="txtCheckBoxLabel" Display="dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
		</td>
	</tr>
	<tr>
		<td></td>
		<td>
			<asp:CheckBox runat="server" ID="chkCheckedByDefault" Text="<%$Resources : GlobalFieldManageControls, CheckedByDefault%>" Checked="true" />
		</td>
		<td></td>
	</tr>
</table>