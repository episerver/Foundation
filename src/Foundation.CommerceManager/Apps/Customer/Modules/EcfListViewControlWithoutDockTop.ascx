<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EcfListViewControlWithoutDockTop.ascx.cs"
	Inherits="Mediachase.Commerce.Manager.Apps.Customer.Modules.EcfListViewControlWithoutDockTop" %>
<%@ Reference Control="~/Apps/Core/Controls/MetaToolbar.ascx" %>
<%@ Register TagPrefix="mc" TagName="MetaToolbar" Src="~/Apps/Core/Controls/MetaToolbar.ascx" %>
<asp:UpdatePanel runat="server" ID="panelToolbar" UpdateMode="Conditional">
	<ContentTemplate>
		<table runat="server" id="topTable" cellspacing="0" cellpadding="0" border="0" width="100%">
			<tr>
				<td style="padding-left: 0px; padding-right: 0px;" class="noBottomBorder">
					<mc:MetaToolbar runat="server" ID="MetaToolbar1" GridId="MainListView" />
				</td>
			</tr>
		</table>
	</ContentTemplate>
</asp:UpdatePanel>
<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="panelMainListView"
	EnableViewState="true" DynamicLayout="true">
	<ProgressTemplate>
		<div style="height: 20%; width: 30%; position: absolute; left: 35%; top: 40%; z-index: 1000;
			background-color: White; border: solid 1px #AAAAAA;">
			<div style="position: absolute; left: 45%; top: 40%;">
				<img src='<%= this.ResolveClientUrl("~/Apps/Shell/Styles/images/Shell/loading_rss.gif") %>'
					alt='loading' height="16" width="16" />
			</div>
		</div>
	</ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel runat="server" ID="panelMainListView" ChildrenAsTriggers="true"
	UpdateMode="Conditional" EnableViewState="true" RenderMode="Inline">
	<ContentTemplate>
		<custom:EcfListView runat="server" ID="MainListView" SkinID="EcfDefaultGrid" 
			InsertItemPosition="FirstItem" TableCellCssClass="DataCell"
			TableHeadingRowCssClass="HeadingRow" TableHeadingCellCssClass="HeadingCell">
			<LayoutTemplate>
				<div>
					<table id="lvTable" runat="server" cellspacing="0" cellpadding="2" width="100%">
						<tr id="headerTRow" runat="server">
						</tr>
						<tr runat="server" id="itemPlaceholder">
						</tr>
						<tr>
							<td runat="server" id="tdFooter">
								<div runat="server" id="footerDiv">
									<div style="float: left; padding-left: 10px; padding-top: 7px;">
										<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:SharedStrings, Page_Size %>" />:
										<asp:DropDownList runat="server" ID="ddPaging" AutoPostBack="true" OnSelectedIndexChanged="ddPaging_SelectedIndexChanged">
											<asp:ListItem Text="10" Value="10"></asp:ListItem>
											<asp:ListItem Text="20" Value="20"></asp:ListItem>
											<asp:ListItem Text="50" Value="50"></asp:ListItem>
											<asp:ListItem Text="100" Value="100"></asp:ListItem>
											<asp:ListItem Text="<%$ Resources:SharedStrings, All %>" Value="1000"></asp:ListItem>
										</asp:DropDownList>
									</div>
									<div style="float: right; padding-right: 5px; padding-top: 7px;">
										<asp:DataPager ID="mainListViewPager2" runat="server" PagedControlID="MainListView">
											<Fields>
												<asp:TemplatePagerField>
													<PagerTemplate>
														(<asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" />&nbsp;<asp:Literal runat="server" ID="itemLiteral" text="<%$ Resources:SharedStrings, Items %>" />)
														&nbsp;|&nbsp;<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:SharedStrings, Page %>" />
													</PagerTemplate>
												</asp:TemplatePagerField>
												<asp:NextPreviousPagerField ShowPreviousPageButton="true" ButtonType="Image" ShowNextPageButton="false"
													ShowLastPageButton="false" PreviousPageImageUrl="~/Apps/Shell/Styles/images/grid/pager/prev.gif" />
												<asp:NumericPagerField />
												<asp:NextPreviousPagerField ShowPreviousPageButton="false" ButtonType="Image" ShowNextPageButton="true"
													ShowLastPageButton="false" NextPageImageUrl="~/Apps/Shell/Styles/images/grid/pager/next.gif" />
											</Fields>
										</asp:DataPager>
									</div>
								</div>
							</td>
						</tr>
					</table>
				</div>
			</LayoutTemplate>
			<ItemTemplate>
				<tr id="itemTRow" runat="server" class='<%# (!(Container is ListViewDataItem) || ((ListViewDataItem)Container).DisplayIndex % 2 == 0) ? "Row" : "AlternatingRow" %>'>
					<%--   Data-bound content. --%>
				</tr>
			</ItemTemplate>
			<InsertItemTemplate>
				<tr id="itemTRow" runat="server" class='<%# (!(Container is ListViewDataItem) || ((ListViewDataItem)Container).DisplayIndex % 2 == 0) ? "Row" : "AlternatingRow" %>'>
					<td>
						<center>
							<div>
								<asp:Label ID="lblEmpty" runat="server" Text="<%$Resources: ConsoleResources, NoItemsInGrid %>" /></div>
						</center>
					</td>
				</tr>
			</InsertItemTemplate>
			<EditItemTemplate>
			</EditItemTemplate>
		</custom:EcfListView>
		
		<IbnWebControls:GridViewHeaderExtender ID="gvHeaderExtender" runat="server">
		</IbnWebControls:GridViewHeaderExtender>
		<table id="emptyTable" runat="server" visible="false">
		</table>
		
		
	</ContentTemplate>
</asp:UpdatePanel>
<style type="text/css">
div.LayoutBase > div
{
	overflow: auto;
	*position: relative; /*fix IE7 bug with scrolling tabs and grids*/
}
</style>
