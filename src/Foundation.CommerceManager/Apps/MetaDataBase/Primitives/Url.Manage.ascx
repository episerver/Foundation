<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Url.Manage.ascx.cs" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Primitives.Url_Manage" %>
<table cellpadding="3" cellspacing="1" border="0" width="100%" class="ibn-propertysheet">
	<tr>
		<td class="ibn-label" width="120px">
		<asp:Literal ID="Literal2" runat="server" Text="<%$Resources : GlobalFieldManageControls, Target%>" />:
		</td>
		<td>
			<asp:DropDownList runat="server" ID="ddlTarget" Width="100%"></asp:DropDownList>
		</td>
		<td width="20px"></td>
	</tr>
	<tr>
		<td class="ibn-label" width="120px">
		<asp:Literal ID="Literal1" runat="server" Text="<%$Resources : GlobalFieldManageControls, MaximumLength%>" />:
		</td>
		<td>
			<asp:TextBox Runat="server" ID="txtMaxLen" Width="100px" MaxLength="4" Text="250"></asp:TextBox>
			<asp:RequiredFieldValidator id="rfvMaxLen" runat="server" ErrorMessage="*" ControlToValidate="txtMaxLen" Display="Dynamic"></asp:RequiredFieldValidator>
			<asp:RangeValidator ID="rvMaxLen" runat="server" ErrorMessage="*" ControlToValidate="txtMaxLen" Display="Dynamic" Type="Integer" MinimumValue="1" MaximumValue="8000"></asp:RangeValidator>
		</td>
	</tr>
	<tr>
		<td></td>
		<td>
			<asp:CheckBox runat="server" ID="chkUnique" Text="<%$Resources : GlobalFieldManageControls, UniqueValue%>" Checked="false" />
		</td>
	</tr>
</table>