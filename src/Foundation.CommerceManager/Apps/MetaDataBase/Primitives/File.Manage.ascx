<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Primitives.File_Manage" Codebehind="File.Manage.ascx.cs" %>
<table cellpadding="3" cellspacing="1" border="0" width="100%" class="ibn-propertysheet">
	<tr>
		<td class="ibn-label" width="120px">
		<asp:Literal ID="Literal1" runat="server" Text="<%$Resources : GlobalFieldManageControls, RegexPattern%>" />:
		</td>
		<td>
			<asp:TextBox Runat="server" ID="txtRegexPattern" Width="100%" MaxLength="250"></asp:TextBox>
		</td>
		<td width="20px"></td>
	</tr>
</table>