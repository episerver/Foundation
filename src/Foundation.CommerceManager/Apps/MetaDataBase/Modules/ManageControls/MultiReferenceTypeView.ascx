<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.Apps.MetaDataBase.Modules.ManageControls.MultiReferenceTypeView" Codebehind="MultiReferenceTypeView.ascx.cs" %>
<%@ Register TagPrefix="ibn" TagName="BlockHeader" Src="~/Apps/MetaDataBase/Common/Design/BlockHeader.ascx" %>
<table cellspacing="0" cellpadding="0" border="0" width="100%" class="ibn-stylebox2">
	<tr>
		<td>
			<ibn:BlockHeader id="secHeader" runat="server" />
		</td>
	</tr>
	<tr>
		<td class="ibn-light ibn-separator">
			<table cellspacing="0" cellpadding="0" width="100%" border="0" style="padding:5px"><tr><td>
				<table class="ibn-propertysheet" width="100%" border="0" cellpadding="5" cellspacing="0">
					<tr>
						<td class="ibn-label" align="right" style="width:200px;">
							<asp:Literal ID="Literal9" runat="server" Text="<%$Resources : GlobalMetaInfo, SystemName%>" />:
						</td>
						<td class="ibn-value">
							<asp:Label runat="server" ID="lblSystemName"></asp:Label>
						</td>
						<td class="ibn-label" align="right" valign="top">
							<asp:Literal ID="Literal2" runat="server" Text="<%$Resources : GlobalMetaInfo, MReferenceMetaClasses%>" />:
						</td>
						<td class="ibn-value" valign="top">
							<asp:Label runat="server" ID="lbClasses"></asp:Label>
						</td>
					</tr>
					<tr>
						<td class="ibn-label" align="right">
							<asp:Literal ID="Literal1" runat="server" Text="<%$Resources : GlobalMetaInfo, FriendlyName%>" />:
						</td>
						<td class="ibn-value">
							<asp:Label runat="server" ID="lblFriendlyName"></asp:Label>
						</td>
					</tr>
				</table>
			</td></tr></table>
		</td>
	</tr>
</table>