<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.MetaDataBase.Modules.MetaClassViewControls.MetaClassInfo" Codebehind="MetaClassInfo.ascx.cs" %>
<div class="ibn-light">
<table class="ibn-propertysheet ibn-underline" width="100%" border="0" cellpadding="5" cellspacing="0">
	<tr>
		<td class="ibn-label" style="width:140px;"><asp:Literal ID="Literal1" runat="server" Text="<%$Resources : GlobalMetaInfo, SystemName%>" />:</td>
		<td class="ibn-value">
			<asp:Image runat="server" ID="imClassType" ImageAlign="AbsMiddle" Width="16px" Height="16px" />
			<asp:Label runat="server" ID="lblClassName"></asp:Label>
		</td>
		<td class="ibn-label" style="width:170px;"><asp:Literal ID="Literal2" runat="server" Text="<%$Resources : GlobalMetaInfo, FriendlyName%>" />:</td>
		<td>
			<asp:Label runat="server" ID="lblFriendlyName"></asp:Label>
		</td>
	</tr>
	<tr>
		<td class="ibn-label" style="width:140px;"><asp:Literal ID="Literal3" runat="server" Text="<%$Resources : GlobalMetaInfo, Type%>" />:</td>
		<td class="ibn-value">
			<asp:Label runat="server" ID="lblType"></asp:Label>
		</td>
		<td class="ibn-label" style="width:170px;"><asp:Literal ID="Literal4" runat="server" Text="<%$Resources : GlobalMetaInfo, PluralName%>" />:</td>
		<td>
			<asp:Label runat="server" ID="lblPluralName"></asp:Label>
		</td>
	</tr>
	<tr runat="server" id="trMoreInfo">
		<td class="ibn-label" style="width:130px;" valign="top"><asp:Label runat="server" ID="lblMoreInfoLabel"></asp:Label></td>
		<td class="ibn-value" colspan="3">
			<asp:Label runat="server" ID="lblMoreInfo"></asp:Label>
		</td>
	</tr>
</table>
</div>