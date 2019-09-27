<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Core.MetaData.Admin.MetaDataExport"
	CodeBehind="MetaDataExport.ascx.cs" %>
<%@ Register TagPrefix="console" Namespace="Mediachase.Web.Console.Controls" Assembly="Mediachase.WebConsoleLib" %>
<div id="FormMultiPage">
	<table class="FormMultiPage">
		<tr>
			<td class="FormLabelCell">
				<table cellpadding="5" width="70%">
					<tr runat="server" id="CatalogRow" visible="false">
						<td>
							<!-- START: Catalog Meta Classes -->
							<div style="width: 100%">
								<asp:Panel runat="server" ID="CatalogMetaClassesPanel1" Height="25px" CssClass="epi-ecf-toolbar-exp">
									<div style="padding: 1px; cursor: pointer; vertical-align: middle;">
										<div style="float: left;">
											<asp:Label ID="Label1" runat="server" Font-Bold="true" Text="<%$ Resources:CoreStrings, MetaData_Catalog_Meta_Classes %>"></asp:Label>
										</div>
										<div style="float: right; vertical-align: middle;">
											<asp:Image ID="ImageExpand1" runat="server" ImageUrl="~/Apps/Shell/styles/images/Expand.jpg" />
										</div>
									</div>
								</asp:Panel>
								<asp:Panel runat="server" ID="CatalogMetaClassesPanel2">
									<asp:DataGrid ID="CatalogItemsGrid" runat="server" CssClass="Grid" AllowPaging="False"
										Width="100%" ShowFooter="True" DataKeyField="MetaClassId" AutoGenerateColumns="False">
										<ItemStyle CssClass="Row_DataCell"></ItemStyle>
										<AlternatingItemStyle CssClass="AlternatingRow_DataCell"></AlternatingItemStyle>
										<HeaderStyle CssClass="HeadingCell"></HeaderStyle>
										<SelectedItemStyle CssClass="SelectedRow" />
										<Columns>
											<console:RowSelectorColumn ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center"
												HeaderStyle-HorizontalAlign="Center" AllowSelectAll="false" SelectionMode="Multiple"
												HeaderText="Select" AutoPostBack="False" />
											<asp:TemplateColumn HeaderText="Name" HeaderStyle-Width="55%">
												<ItemTemplate>
													<asp:Label runat="server" ID="MetaClassNameLabel" Text='<%# DataBinder.Eval(Container, "DataItem.FriendlyName")%>'></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="SystemName" HeaderStyle-Width="35%">
												<ItemTemplate>
													<asp:Label runat="server" ID="MetaClassSystemNameLabel" Text='<%# DataBinder.Eval(Container, "DataItem.SystemName")%>'></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
									</asp:DataGrid>
								</asp:Panel>
								<ajaxToolkit:CollapsiblePanelExtender runat="server" ID="CatalogCollapsiblePanelExtender"
									ExpandControlID="CatalogMetaClassesPanel1" CollapseControlID="CatalogMetaClassesPanel1"
									TargetControlID="CatalogMetaClassesPanel2" Collapsed="false" ExpandedText="Hide"
									CollapsedText="Show" SuppressPostBack="true" ImageControlID="ImageExpand1" ExpandedImage="~/Apps/Shell/styles/images/collapse.jpg"
									CollapsedImage="~/Apps/Shell/styles/images/expand.jpg">
								</ajaxToolkit:CollapsiblePanelExtender>
							</div>
							<!-- END: Catalog Meta Classes -->
						</td>
					</tr>
					<tr runat="server" id="OrderRow" visible="false">
						<td>
							<!-- START: Order Meta Classes -->
							<div style="width: 100%">
								<asp:Panel runat="server" ID="OrderMetaClassesPanel1" Height="25px" CssClass="epi-ecf-toolbar-exp">
									<div style="padding: 1px; cursor: pointer; vertical-align: middle;">
										<div style="float: left;">
											<asp:Label ID="Label2" runat="server" Font-Bold="true" Text="<%$ Resources:CoreStrings, MetaData_Order_Meta_Classes %>"></asp:Label>
										</div>
										<div style="float: right; vertical-align: middle;">
											<asp:Image ID="ImageExpand2" runat="server" ImageUrl="~/Apps/Shell/styles/images/Expand.jpg" />
										</div>
									</div>
								</asp:Panel>
								<asp:Panel runat="server" ID="OrderMetaClassesPanel2">
									<asp:DataGrid ID="OrderItemsGrid" runat="server" CssClass="Grid" AllowPaging="False"
										Width="100%" ShowFooter="True" DataKeyField="MetaClassId" AutoGenerateColumns="False">
										<ItemStyle CssClass="Row_DataCell"></ItemStyle>
										<AlternatingItemStyle CssClass="AlternatingRow_DataCell"></AlternatingItemStyle>
										<HeaderStyle CssClass="HeadingCell"></HeaderStyle>
										<SelectedItemStyle CssClass="SelectedRow" />
										
										<Columns>
											<console:RowSelectorColumn ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center"
												HeaderStyle-HorizontalAlign="Center" AllowSelectAll="false" SelectionMode="Multiple"
												HeaderText="Select" AutoPostBack="False" />
											<asp:TemplateColumn HeaderText="Name" HeaderStyle-Width="55%">
												<ItemTemplate>
													<asp:Label runat="server" ID="MetaClassNameLabel" Text='<%# DataBinder.Eval(Container, "DataItem.FriendlyName")%>'></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="SystemName" HeaderStyle-Width="35%">
												<ItemTemplate>
													<asp:Label runat="server" ID="MetaClassSystemNameLabel" Text='<%# DataBinder.Eval(Container, "DataItem.SystemName")%>'></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
									</asp:DataGrid>
								</asp:Panel>
								<ajaxToolkit:CollapsiblePanelExtender runat="server" ID="OrderCollapsiblePanelExtender"
									ExpandControlID="OrderMetaClassesPanel1" CollapseControlID="OrderMetaClassesPanel1"
									TargetControlID="OrderMetaClassesPanel2" Collapsed="false" ExpandedText="Hide"
									CollapsedText="Show" SuppressPostBack="true" ImageControlID="ImageExpand2" ExpandedImage="~/Apps/Shell/styles/images/collapse.jpg"
									CollapsedImage="~/Apps/Shell/styles/images/expand.jpg">
								</ajaxToolkit:CollapsiblePanelExtender>
							</div>
							<!-- END: Order Meta Classes -->
						</td>
					</tr>
					<tr runat="server" id="BafMetaClassesRow" visible="false">
						<td>
							<!-- START: Profile Meta Classes -->
							<div style="width: 100%">
								<asp:Panel runat="server" ID="BafMetaClassesPanel1" Height="25px" CssClass="epi-ecf-toolbar-exp">
									<div style="padding: 1px; cursor: pointer; vertical-align: middle;" >
										<div style="float: left;">
											<asp:Label ID="Label3" runat="server" Font-Bold="true" Text="<%$ Resources:CoreStrings, MetaData_Baf_Meta_Classes %>"></asp:Label>
										</div>
										<div style="float: right; vertical-align: middle;">
											<asp:Image ID="Image1" runat="server" ImageUrl="~/Apps/Shell/styles/images/Expand.jpg" />
										</div>
									</div>
								</asp:Panel>
								<asp:Panel runat="server" ID="BafMetaClassesPanel2">
									<asp:DataGrid ID="BafItemsGrid" runat="server" CssClass="Grid" AllowPaging="False"
										Width="100%" ShowFooter="True" DataKeyField="Name" AutoGenerateColumns="False">
										<ItemStyle CssClass="Row_DataCell"></ItemStyle>
										<AlternatingItemStyle CssClass="AlternatingRow_DataCell"></AlternatingItemStyle>
										<HeaderStyle CssClass="HeadingCell"></HeaderStyle>
										<SelectedItemStyle CssClass="SelectedRow" />
										<Columns>
											<console:RowSelectorColumn ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center"
												HeaderStyle-HorizontalAlign="Center" AllowSelectAll="false" SelectionMode="Multiple"
												HeaderText="Select" AutoPostBack="False" />
											<asp:TemplateColumn HeaderText="Name" HeaderStyle-Width="55%">
												<ItemTemplate>
													<asp:Label runat="server" ID="MetaClassNameLabel" Text='<%# DataBinder.Eval(Container, "DataItem.Name")%>'></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="FriendlyName" HeaderStyle-Width="35%">
												<ItemTemplate>
													<asp:Label runat="server" ID="MetaClassSystemNameLabel" Text='<%# DataBinder.Eval(Container, "DataItem.FriendlyName")%>'></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
									</asp:DataGrid>
								</asp:Panel>
								<ajaxToolkit:CollapsiblePanelExtender runat="server" ID="BafCollapsiblePanelExtender"
									ExpandControlID="BafMetaClassesPanel1" CollapseControlID="BafMetaClassesPanel1"
									TargetControlID="BafMetaClassesPanel2" Collapsed="false" ExpandedText="Hide"
									CollapsedText="Show" SuppressPostBack="true" ImageControlID="ImageExpand2" ExpandedImage="~/Apps/Shell/styles/images/collapse.jpg"
									CollapsedImage="~/Apps/Shell/styles/images/expand.jpg">
								</ajaxToolkit:CollapsiblePanelExtender>
							</div>
							<!-- END: Profile Meta Classes -->
						</td>
					</tr>
					<tr runat="server" id="BafEnumsRow" visible="false">
						<td>
							<div style="width: 100%">
								<asp:Panel runat="server" ID="BafEnumsPanel1" Height="25px" CssClass="epi-ecf-toolbar-exp">
									<div style="padding: 1px; cursor: pointer; vertical-align: middle;" >
										<div style="float: left;">
											<asp:Label ID="Label4" runat="server" Font-Bold="true" Text="<%$ Resources:CoreStrings, MetaData_Baf_Enums %>"></asp:Label>
										</div>
										<div style="float: right;vertical-align: middle;">
											<asp:Image ID="Image2" runat="server" ImageUrl="~/Apps/Shell/styles/images/Expand.jpg" />
										</div>
									</div>
								</asp:Panel>
								<asp:Panel runat="server" ID="BafEnumsPanel2">
									<asp:DataGrid ID="BafEnumsGrid" runat="server" CssClass="Grid" AllowPaging="False"
										Width="100%" ShowFooter="True" DataKeyField="MetaEnumTypeName" AutoGenerateColumns="False">
										<ItemStyle CssClass="Row_DataCell"></ItemStyle>
										<AlternatingItemStyle CssClass="AlternatingRow_DataCell"></AlternatingItemStyle>
										<HeaderStyle CssClass="HeadingCell"></HeaderStyle>
										<SelectedItemStyle CssClass="SelectedRow" />
										<Columns>
											<console:RowSelectorColumn ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center"
												HeaderStyle-HorizontalAlign="Center" AllowSelectAll="false" SelectionMode="Multiple"
												HeaderText="Select" AutoPostBack="False" />
											<asp:TemplateColumn HeaderText="Enum Name" HeaderStyle-Width="90%">
												<ItemTemplate>
													<asp:Label runat="server" ID="MetaClassNameLabel" Text='<%# DataBinder.Eval(Container, "DataItem.MetaEnumTypeName")%>'></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
									</asp:DataGrid>
								</asp:Panel>
								<ajaxToolkit:CollapsiblePanelExtender runat="server" ID="BafEnumsCollapsiblePanelExtender"
									ExpandControlID="BafEnumsPanel1" CollapseControlID="BafEnumsPanel1"
									TargetControlID="BafEnumsPanel2" Collapsed="false" ExpandedText="Hide"
									CollapsedText="Show" SuppressPostBack="true" ImageControlID="ImageExpand2" ExpandedImage="~/Apps/Shell/styles/images/collapse.jpg"
									CollapsedImage="~/Apps/Shell/styles/images/expand.jpg">
								</ajaxToolkit:CollapsiblePanelExtender>
							</div>
						</td>
					</tr>
					<tr>
						<td align="left">
							<!-- START: Export Button -->
							<asp:Button runat="server" ID="BtnExport" Text="<%$ Resources:SharedStrings, Export %>"
								OnClick="BtnExport_Click" />
							<!-- END: Export Button -->
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</div>
