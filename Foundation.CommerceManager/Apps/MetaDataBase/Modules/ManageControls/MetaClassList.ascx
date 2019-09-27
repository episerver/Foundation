<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Ibn.Web.UI.Apps.MetaDataBase.Modules.ManageControls.MetaClassList" CodeBehind="MetaClassList.ascx.cs" %>
<%@ Reference Control="~/Apps/Core/Controls/MetaToolbar.ascx" %>
<%@ Register TagPrefix="mc" TagName="MetaToolbar" Src="~/Apps/Core/Controls/MetaToolbar.ascx" %>
<asp:Panel ID="Panel1" ScrollBars="Auto" runat="server">
	<asp:UpdatePanel runat="server" ID="upMain">
		<ContentTemplate>
			<table cellspacing="0" cellpadding="0" border="0" width="100%" class="ibn-stylebox2">
				<tr>
					<td>
						<mc:MetaToolbar runat="server" ID="MainMetaToolbar" ViewName="MetaClass" PlaceName="MetaClassList" ToolbarMode="MetaView" />
					</td>
				</tr>
				<tr>
					<td class="ibn-light ibn-underline" style="padding-left: 5px; padding-top: 5px; padding-bottom: 5px">
						<span class="text">
							<asp:Literal ID="Literal1" runat="server" Text="<%$Resources : GlobalMetaInfo, Show%>"></asp:Literal>:
						</span>&nbsp;
						<asp:DropDownList runat="server" ID="ddlFilter" AutoPostBack="true" Width="250px">
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td>
						<asp:DataGrid runat="server" ID="grdMain" AutoGenerateColumns="false" Width="100%"
							CellPadding="4" GridLines="None" AllowPaging="false" AllowSorting="true" EnableViewState="true">
							<Columns>
								<asp:TemplateColumn HeaderText="" HeaderStyle-Height="22px">
									<ItemStyle CssClass="ibn-vb" Width="20px" Height="22px" HorizontalAlign="Center" />
									<ItemTemplate>
										<asp:Image runat="server" ID="imClassType" ImageUrl='<%#Eval("ClassTypeImageUrl")%>'
											BorderStyle="None" Width="16px" Height="16px" />
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="<%$Resources : GlobalMetaInfo, SystemName%>" SortExpression="Name">
									<ItemTemplate>
										<a href='MetaClassView.aspx?class=<%# Eval("Name")%>'>
											<%# Eval("Name")%></a>
									</ItemTemplate>
									<ItemStyle CssClass="ibn-vb" />
									<HeaderStyle CssClass="ibn-vh" />
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="FriendlyName" HeaderText="<%$Resources : GlobalMetaInfo, FriendlyName%>"
									ItemStyle-CssClass="ibn-vb" HeaderStyle-CssClass="ibn-vh" SortExpression="FriendlyName" />
								<asp:BoundColumn DataField="PluralName" HeaderText="<%$Resources : GlobalMetaInfo, PluralName%>"
									ItemStyle-CssClass="ibn-vb" HeaderStyle-CssClass="ibn-vh" SortExpression="PluralName" />
								<asp:BoundColumn DataField="TypeString" HeaderText="<%$Resources : GlobalMetaInfo, Type%>"
									ItemStyle-CssClass="ibn-vb" HeaderStyle-CssClass="ibn-vh" SortExpression="TypeString" />
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:HyperLink ImageUrl="../../Images/edit.gif" ID="ibEdit" runat="server" NavigateUrl='<%# Eval("EditLink")%>'
											ToolTip="<%$Resources : GlobalMetaInfo, Edit%>" Visible='<%# !((bool)Eval("IsSystem")) %>'></asp:HyperLink>&nbsp;&nbsp;
										<asp:ImageButton ImageUrl="../../Images/delete.gif" runat="server" ToolTip="<%$Resources : GlobalMetaInfo, Delete%>"
											Width="16" Height="16" CommandName="Delete" CommandArgument='<%# Eval("Name") %>'
											ID="ibDelete" Visible='<%# !((bool)Eval("IsSystem")) %>'></asp:ImageButton>
									</ItemTemplate>
									<ItemStyle CssClass="ibn-vb" Width="50px" Wrap="false" />
									<HeaderStyle CssClass="ibn-vh" Width="50px" Wrap="false" />
								</asp:TemplateColumn>
							</Columns>
						</asp:DataGrid>
					</td>
				</tr>
			</table>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Panel>
