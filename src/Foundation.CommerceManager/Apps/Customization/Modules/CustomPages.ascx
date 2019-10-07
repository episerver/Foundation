<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomPages.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Customization.Modules.CustomPages" %>
<%@ Reference Control="~/Apps/MetaUIEntity/Grid/EntityGrid.ascx" %>
<%@ Reference Control="~/Apps/Core/Controls/MetaToolbar.ascx" %>
<%@ Reference Control="~/Apps/MetaUIEntity/Grid/MetaGridServerEventAction.ascx" %>

<%@ Register Src="~/Apps/MetaUIEntity/Grid/EntityGrid.ascx" TagName="Grid" TagPrefix="mc" %>
<%@ Register Src="~/Apps/Core/Controls/MetaToolbar.ascx" TagName="Toolbar" TagPrefix="mc" %>
<%@ Register Src="~/Apps/MetaUIEntity/Grid/MetaGridServerEventAction.ascx" TagName="GridEventAction" TagPrefix="mc" %>

<script type="text/javascript">
function LayoutResizeHandler(sender, eventArgs)
{
}
</script>
<table cellspacing="0" cellpadding="0" border="0" width="100%">
	<tr>
		<td style="padding-left:5px; padding-right:5px; padding-top:5px;">
			<div class="noBottomBorder">
				<mc:Toolbar runat="server" ID="ctrlToolbar" ClassName="CustomPage" ViewName="" PlaceName="PageList" ToolbarMode="ListViewUI" />
			</div>		
		</td>
	</tr>
	<tr>
		<td style="padding-bottom:2px;">
			<div style="margin-left:5px; margin-right:5px;">
				<mc:Grid runat="server" ID="ctrlGrid" ShowPaging="false" DashboardMode="false" ClassName="CustomPage" ViewName="" PlaceName="PageList" />
			</div>
		</td>
	</tr>
</table>
