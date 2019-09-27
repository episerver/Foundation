<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Primitives.ReferencedField_Manage" Codebehind="ReferencedField.Manage.ascx.cs" %>
<table cellpadding="3" cellspacing="1" border="0" width="100%" class="ibn-propertysheet">
	<tr>
		<td class="ibn-label" width="120px">
		<asp:Literal ID="Literal1" runat="server" Text="<%$Resources : GlobalFieldManageControls, ReferencedClass%>" />:
		</td>
		<td>
			<asp:DropDownList runat="server" ID="ddlClass" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlClass_SelectedIndexChanged"></asp:DropDownList>
		</td>
		<td width="20px"></td>
	</tr>
	<tr>
		<td class="ibn-label" width="120px">
		<asp:Literal ID="Literal2" runat="server" Text="<%$Resources : GlobalFieldManageControls, ReferencedField%>" />:
		</td>
		<td>
			<asp:DropDownList runat="server" ID="ddlField" Width="100%"></asp:DropDownList>
		</td>
		<td width="20px"></td>
	</tr>
</table>