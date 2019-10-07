<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Primitives.Currency_Manage" Codebehind="Currency.Manage.ascx.cs" %>
<table cellpadding="3" cellspacing="1" border="0" width="100%" class="ibn-propertysheet">
	<tr>
		<td class="ibn-label" width="120px"><asp:Literal ID="Literal1" runat="server" Text="<%$Resources : GlobalFieldManageControls, DefaultValue%>" />:</td>
		<td>
			<asp:TextBox Runat="server" ID="txtDefaultValue" Width="150px" MaxLength="21"></asp:TextBox>
			<asp:RangeValidator ID="rvDefaultValue" runat="server" ErrorMessage="*" ControlToValidate="txtDefaultValue" Display="Dynamic" Type="Currency" MinimumValue="-922337203685477" MaximumValue="922337203685477"></asp:RangeValidator>
		</td>
	</tr>
	<tr>
		<td></td>
		<td>
			<asp:CheckBox runat="server" ID="chkAllowNegative" Text="<%$Resources : GlobalFieldManageControls, AllowNegativeValues%>" Checked="true" />
		</td>
	</tr>
</table>