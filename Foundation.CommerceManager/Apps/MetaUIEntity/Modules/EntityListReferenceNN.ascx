<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EntityListReferenceNN.ascx.cs" Inherits="Mediachase.Ibn.Web.UI.MetaUIEntity.Modules.EntityListReferenceNN" %>
<%@ Reference Control="~/Apps/Core/Controls/MetaToolbar.ascx" %>
<%@ Reference Control="~/Apps/MetaUIEntity/Grid/EntityGrid.ascx" %>
<%@ Reference Control="~/Apps/MetaUIEntity/Grid/MetaGridServerEventAction.ascx" %>
<%@ Register TagPrefix="mc" TagName="MetaToolbar" Src="~/Apps/Core/Controls/MetaToolbar.ascx" %>
<%@ Register TagPrefix="mc" TagName="EntityGrid" Src="~/Apps/MetaUIEntity/Grid/EntityGrid.ascx" %>
<%@ Register TagPrefix="mc" TagName="MCGridAction" Src="~/Apps/MetaUIEntity/Grid/MetaGridServerEventAction.ascx" %>

<div id="mainDiv" runat="server"> 
<table class="ibn-propertysheet" cellspacing="0" cellpadding="0" border="0" width="100%" style="table-layout: fixed">
	<tr runat="server" id="ToolbarRow">
		<td class="ibn-stylebox2noBottom">
			<mc:MetaToolbar runat="server" ID="MainMetaToolbar" ToolbarMode="ListViewUI" GridId="grdMain" />
		</td>
	</tr>
	<tr>
		<td style="max-height: 600px">
			<mc:EntityGrid ID="grdMain" runat="server" ShowPaging="true" LayoutResizeEnable="false" />	
			<mc:MCGridAction runat="server" ID="ctrlGridEventUpdater" />
			<asp:LinkButton ID="lbAddItems" runat="server" Visible="false" OnClick="lbAddItems_Click"></asp:LinkButton>
		</td>
	</tr>
</table>
</div>