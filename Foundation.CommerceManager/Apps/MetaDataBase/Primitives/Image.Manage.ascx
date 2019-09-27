<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Primitives.Image_Manage" Codebehind="Image.Manage.ascx.cs" %>
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
	<tr>
		<td class="ibn-label" width="120px">
		<asp:Literal ID="Literal2" runat="server" Text="<%$Resources : GlobalFieldManageControls, Width%>" />:
		</td>
		<td>
			<asp:TextBox Runat="server" ID="txtWidth" Width="100px" MaxLength="4" Text="128"></asp:TextBox>
			<asp:RangeValidator ID="rvWidth" runat="server" ErrorMessage="*" ControlToValidate="txtWidth" Display="Dynamic" Type="Integer" MinimumValue="1" MaximumValue="1600"></asp:RangeValidator>
		</td>
		<td></td>
	</tr>
	<tr>
		<td class="ibn-label" width="120px">
		<asp:Literal ID="Literal3" runat="server" Text="<%$Resources : GlobalFieldManageControls, Height%>" />:
		</td>
		<td>
			<asp:TextBox Runat="server" ID="txtHeight" Width="100px" MaxLength="4" Text="128"></asp:TextBox>
			<asp:RangeValidator ID="rvHeight" runat="server" ErrorMessage="*" ControlToValidate="txtHeight" Display="Dynamic" Type="Integer" MinimumValue="1" MaximumValue="1600"></asp:RangeValidator>
		</td>
	</tr>
	<tr>
		<td class="ibn-label" width="120px"></td>
		<td>
			<asp:CheckBox runat="server" ID="chkShowBorder" Text="<%$Resources : GlobalFieldManageControls, ShowBorder%>" Checked="true" />
		</td>
		<td></td>
	</tr>
</table>