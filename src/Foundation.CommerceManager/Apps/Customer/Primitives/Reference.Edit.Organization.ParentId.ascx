<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Reference.Edit.Organization.ParentId.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.MetaDataBase.MetaUI.Primitives.Reference_Edit_Organization_ParentId" %>
<%@ Reference Control="~/Apps/MetaUIEntity/Modules/EntityDropDown.ascx" %>
<%@ Register TagPrefix="mc" TagName="EntityDD" Src="~/Apps/MetaUIEntity/Modules/EntityDropDown.ascx" %>
<table id="tblEntity" runat="server" cellpadding="0" cellspacing="0" border="0" width="100%" class="ibn-propertysheet">
	<tr>
		<td>
			<mc:EntityDD ID="refObjects" ItemCount="10" runat="server" Width="100%" />
		</td>
		<td style="width:20px;">
			<asp:CustomValidator runat="server" ID="vldCustomEntity" ErrorMessage="*" Display="dynamic" OnServerValidate="vldCustom_ServerValidate"></asp:CustomValidator>
		</td>
	</tr>
</table>