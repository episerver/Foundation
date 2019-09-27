<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EntityViewWithButtons.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.MetaUIEntity.Modules.EntityViewWithButtons" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ Register TagPrefix="ibn" Namespace="Mediachase.BusinessFoundation" Assembly="Mediachase.BusinessFoundation" %>

<link rel="stylesheet" type="text/css" href="<%= CommerceHelper.GetAbsolutePath("~/Apps/MetaDataBase/Styles/Theme.css") %>" />
<!-- EPi Style -->
<link href="<%= CommerceHelper.GetAbsolutePath("~/Apps/Shell/EPi/Shell/Light/Shell-ext.css") %>" rel="stylesheet" type="text/css" />
	
<asp:UpdatePanel runat="server" ID="ContentPanel">
	<ContentTemplate>
		<table cellpadding="0" cellspacing="0" border="0" width="100%" >
			<tr>
				<td valign="top" style="padding:5px;">
					<div id="mainDiv" style="overflow:auto; background-color:White;" class="ibn-stylebox2 ibn-propertysheet">
						<div>
							<ibn:XMLFormBuilder ID="xmlStruct" runat="server" />
						</div>
					</div>
				</td>
			</tr>
			<tr>
				<td style="padding:5px;">
					<asp:Button runat="server" ID="EditButton" Text="<%$ Resources: SharedStrings, Edit %>" CssClass="button" style="width:72px;" />
					<asp:Button runat="server" ID="CancelButton" Text="<%$ Resources: SharedStrings, Cancel %>" CssClass="button" style="width:72px;" />
				</td>
			</tr>
		</table>
	</ContentTemplate>
</asp:UpdatePanel>
