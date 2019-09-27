<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EcfGridCustomDataSource.ascx.cs" Inherits="Mediachase.Commerce.Manager.Apps.Core.Controls.EcfGridCustomDataSource" %>
<%@ Import Namespace="Mediachase.Commerce.Shared" %>
<%@ Register Assembly="Mediachase.BusinessFoundation" Namespace="Mediachase.BusinessFoundation" TagPrefix="mc" %>
<%@ Register Assembly="Mediachase.ConsoleManager" Namespace="Mediachase.UI.Web.Apps.MetaUI.Grid" TagPrefix="mc3" %>
<%@ Register TagPrefix="mc" TagName="MetaToolbar" Src="~/Apps/Core/Controls/MetaToolbar.ascx" %>
<link rel="stylesheet" type="text/css" href='<%= CommerceHelper.GetAbsolutePath("~/Apps/MetaUIEntity/styles/grid.css") %>' />
<asp:UpdatePanel runat="server" ID="panelToolbar" UpdateMode="Conditional">
	<ContentTemplate>
		<table runat="server" id="topTable" cellspacing="0" cellpadding="0" border="0" width="100%">
			<tr>
				<td style="padding-left: 0px; padding-right: 0px;" class="noBottomBorder">
					<mc:MetaToolbar runat="server" ID="MetaToolbar1" GridId="MainGrid" />
				</td>
			</tr>
		</table>
	</ContentTemplate>
</asp:UpdatePanel>
<mc:GridViewHeaderExtender2 runat="server" TargetControlID="MainGrid" ID="MainGridExt" WrapperDivCssClass="WrapperDiv" HeaderHeight="25" BottomHeight="30" PaddingWidth="10">
	<ContentTemplate>
		<mc3:IbnGridView runat="server" ID="MainGrid"
					AllowSorting="true"
					AllowPaging="True"
					AutoGenerateColumns="False"
					ShowFooter="False"
					EnableViewState="True"
					TextBoxControlId="tbCurrentPage"
					PostbackControlId="btnRefresh"
					CssClass="WrapperDiv"
					GridLines="None"
					CellSpacing="0"
					CellPadding="0"
					BorderStyle="None"
					EnableSortingAndPagingCallbacks="false"
					>
			<HeaderStyle CssClass="serverGridHeaderOnly"/>
			<PagerSettings Mode="NumericFirstLast" />
			<PagerTemplate>	
				<div style="padding-top: 5px;" id="DivPaging" runat="server">
					<div style="float: left; padding-left: 7px;">
						<asp:Literal ID="Literal1" runat="server" Text="<% $Resources: GlobalMetaInfo, PageSize %>" />
						<asp:DropDownList runat="server" ID="ddPaging" AutoPostBack="true" CssClass="GridPaging" OnSelectedIndexChanged="ddPaging_SelectedIndexChanged" Height="18">
							<asp:ListItem Text="<% $Resources: GlobalMetaInfo, PageSize10 %>" Value="10"></asp:ListItem>
							<asp:ListItem Text="<% $Resources: GlobalMetaInfo, PageSize25 %>" Value="25"></asp:ListItem>
							<asp:ListItem Text="<% $Resources: GlobalMetaInfo, PageSize50 %>" Value="50"></asp:ListItem>
							<asp:ListItem Text="<% $Resources: GlobalMetaInfo, PageSize100 %>" Value="100"></asp:ListItem>
						</asp:DropDownList>
							<span>&nbsp;</span>
							<asp:Literal ID="ltTotalElements2" runat="server" Text="<% $Resources: Common, tTotalRecords %>" Visible="false" />
							<asp:Literal ID="ltTotalElements" runat="server" Text="" />	
					</div>
					<div style="float: right; padding-right: 7px;" runat="server" id="pagingContainer">
						<span style="padding: 0px; vertical-align: bottom;">
							<asp:ImageButton runat="server" ID="ImageButtonFirst" CommandName="Page" CommandArgument="First" ImageUrl="~/Apps/MetaDataBase/images/ext/default/grid/page-first.gif" ToolTip="<% $Resources: Common, pagingFirst %>"/>
							<asp:ImageButton runat="server" ID="ImageButtonPrevious" CommandName="Page" CommandArgument="Prev" ImageUrl="~/Apps/MetaDataBase/images/ext/default/grid/page-prev.gif" ToolTip="<% $Resources: Common, pagingPrev %>"/>
							<asp:Image runat="server" ID="split1" ImageUrl="~/Apps/MetaDataBase/images/ext/default/grid/grid-blue-split.gif" />
						</span>
						<span style="padding: 2px 4px 2px 4px; vertical-align:bottom;">
							<asp:TextBox runat="server" ID="tbCurrentPage" Width="30" Height="12" AutoCompleteType="none"></asp:TextBox>
							<span style="vertical-align: middle;">
							<asp:Literal ID="Literal3" runat="server" Text="<% $Resources: Common, tOf %>" />
							<asp:Literal ID="ltTotalPage" runat="server" Text="" />
							</span>
						</span>
						<span style="padding: 0px; vertical-align: bottom;">
							<asp:ImageButton runat="server" ID="btnRefresh" CommandName="Page" ImageUrl="~/Apps/MetaDataBase/images/ext/default/grid/refresh.gif"/>
							<asp:Image runat="server" ID="split2" ImageUrl="~/Apps/MetaDataBase/images/ext/default/grid/grid-blue-split.gif" />
							<asp:ImageButton runat="server" ID="ImageButtonNext" CommandName="Page" CommandArgument="Next" ImageUrl="~/Apps/MetaDataBase/images/ext/default/grid/page-next.gif" ToolTip="<% $Resources: Common, pagingNext %>" />
							<asp:ImageButton runat="server" ID="ImageButtonLast" CommandName="Page" CommandArgument="Last" ImageUrl="~/Apps/MetaDataBase/images/ext/default/grid/page-last.gif" ToolTip="<% $Resources: Common, pagingLast %>"/>
						</span>
					</div>
				</div>
				<div style="display: none;" id="DivPagingDashboard">
				</div>
			</PagerTemplate>
		</mc3:IbnGridView>
	</ContentTemplate>
</mc:GridViewHeaderExtender2>
<input runat="server" id="hfCheckedCollection" type="Hidden"  />