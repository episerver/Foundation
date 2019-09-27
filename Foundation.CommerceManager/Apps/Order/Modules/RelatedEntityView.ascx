<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelatedEntityView.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.RelatedEntityView" %>
<%@ Register Src="~/Apps/Customer/Modules/EcfListViewControlWithoutDockTop.ascx" TagName="EcfListViewControl" TagPrefix="cm" %>
<%@ Register Src="~/Apps/Core/Controls/EcfGridCustomDataSource.ascx" TagName="EcfCustomGrid" TagPrefix="cm" %>
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td style="padding:5px;" id="tdGrid" runat="server">
			<div>
				<cm:EcfCustomGrid runat="server" ID="MyListView" LayoutResizeEnable="false" />
			</div>
		</td>
	</tr>
</table>