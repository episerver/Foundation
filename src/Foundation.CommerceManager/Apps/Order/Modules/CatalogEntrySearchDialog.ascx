<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CatalogEntrySearchDialog.ascx.cs"
	Inherits="Mediachase.Commerce.Manager.Apps.Order.Modules.CatalogEntrySearchDialog" %>
<%@ Register TagPrefix="mc" Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" %>
<%@ Register Src="~/Apps/Core/Controls/EcfListViewControl.ascx" TagName="EcfListViewControl"
	TagPrefix="core" %>
<style type="text/css">
	.filter
	{
		padding-bottom: 5px;
	}
</style>
<script type="text/javascript">  
	function SaveSelectedId()
	{
	    var grid = $find("<%= MyListView.InnerListViewTable.ClientID %>");
		var hdn = document.getElementById("<%= SelectedEntryId.ClientID %>");

		if (grid && hdn)
		{
			hdn.value = grid.getSelectedElement();
		}
	}
</script>
<mc:McDock ID="DockTop" runat="server" Anchor="top" EnableSplitter="False" DefaultSize="30">
	<DockItems>
		<div style="padding: 5px;">
			<asp:Label runat="server" ID="lblErrorInfo" Style="color: Red" Visible="false"></asp:Label>
			<table runat="server" id="SearchTable" cellpadding="0" cellspacing="0" width="100%"
				class="filter">
				<tr>					
					<td style="width: 205px;">
						<asp:TextBox runat="server" ID="tbKeywordValue" Width="200"></asp:TextBox>
					</td>
					<td style="width: 55px;">
						<asp:Button runat="server" ID="SearchButton" Text="<%$ Resources:SharedStrings, Find %>"
							OnClick="SearchButton_Click" Width="50" />
					</td>
					<td>
						<asp:LinkButton runat="server" ID="AdvancedSearchButton" Text="<%$ Resources:SharedStrings, AdvancedSearch %>"
							OnClick="AdvancedSearchButton_Click"></asp:LinkButton>
					</td>
				</tr>
			</table>
			<table runat="server" id="AdvancedTable" cellpadding="0" cellspacing="0" width="100%"
				visible="false">
				<tr>
					<td style="width: 155px;">
						<asp:DropDownList runat="server" ID="ddlCatalogs" Width="150"></asp:DropDownList>
					</td>
					<td style="width: 155px;">
						<asp:DropDownList runat="server" ID="ddlLanguages" Width="150"></asp:DropDownList>
					</td>
					<td>
						<asp:DropDownList runat="server" ID="ddlEntryTypes" Width="150">
							<asp:ListItem Text="<%$ Resources:OrderStrings, SelectEntryType %>" Value="[all]"></asp:ListItem>
							<asp:ListItem Text="<%$ Resources:CatalogStrings, Catalog_Product %>" Value="Product"></asp:ListItem>
							<asp:ListItem Text="<%$ Resources:CatalogStrings, Catalog_Variation_Sku %>" Value="Variation"></asp:ListItem>
							<asp:ListItem Text="<%$ Resources:CatalogStrings, Catalog_Bundle %>" Value="Bundle"></asp:ListItem>
							<asp:ListItem Text="<%$ Resources:CatalogStrings, Catalog_Package %>" Value="Package"></asp:ListItem>
							<asp:ListItem Text="<%$ Resources:CatalogStrings, Catalog_Dynamic_Package %>" Value="DynamicPackage"></asp:ListItem>
						</asp:DropDownList>
					</td>
				</tr>
			</table>
			<div runat="server" id="EntryInfoBlock" visible="false" style="padding-left:10px; padding-top:3px;">
				<asp:Label runat="server" ID="SelectedEntry" Text="<%$ Resources:OrderStrings, SelectedEntry %>" Font-Bold="true"></asp:Label>:
				<asp:Label runat="server" ID="SelectedEntryLabel"></asp:Label>
			</div>
		</div>
	</DockItems>
</mc:McDock>

<core:EcfListViewControl ID="MyListView" runat="server" AppId="Order" ViewId="CatalogEntrySearchDialog" ShowTopToolbar="false"></core:EcfListViewControl>
<input type="hidden" runat="server" id="SelectedEntryId" />
<div style="padding-left:10px; padding-right:10px;">
	<asp:UpdatePanel runat="server" ID="EntryUpdatePanel" ChildrenAsTriggers="true" RenderMode="Block">
		<ContentTemplate>
			<asp:Panel runat="server" ID="ControlPanel" ScrollBars="Auto" Height="357" Visible="false" BackColor="White" BorderColor="#95b7f3" BorderStyle="Solid" BorderWidth="1"></asp:Panel>
		</ContentTemplate>
	</asp:UpdatePanel>
</div>

<mc:McDock ID="DockBottom" runat="server" Anchor="bottom" EnableSplitter="False" DefaultSize="39">
	<DockItems>
		<div style="text-align:right; padding-top:10px; padding-right:15px;">
			<asp:Button runat="server" ID="CancelButton" Width="100" />
			&nbsp;
			<asp:Button runat="server" ID="ConfigureButton" Width="150" Text="<%$ Resources:OrderStrings, ConfigureSelectedEntry %>" OnClick="ConfigureButton_Click" />
			&nbsp;
			<asp:Button runat="server" ID="BackButton" Width="100" Text="<%$ Resources:SharedStrings, Back %>" OnClick="BackButton_Click" Visible="false"/>
			&nbsp;
			<asp:Button runat="server" ID="AddItemButton" Width="150" Text="<%$ Resources:OrderStrings, AddItem %>" OnClick="AddItemButton_Click" Visible="false"/>
		</div>
	</DockItems>
</mc:McDock>
