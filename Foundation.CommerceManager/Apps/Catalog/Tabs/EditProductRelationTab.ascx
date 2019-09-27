<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditProductRelationTab.ascx.cs" Inherits="Mediachase.Commerce.Manager.Catalog.Tabs.EditProductRelationTab" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" %>

<div id="DataForm">
    <table class="DataForm">
        <tr>
            <td>
                <ComponentArt:Grid RunningMode="Client" AutoFocusSearchBox="false"
                    ShowHeader="false" ShowFooter="true" Width="800" SkinID="Inline" runat="server"
                    ID="RelationsGrid" AutoPostBackOnInsert="false" AutoPostBackOnDelete="false"
                    AutoCallBackOnUpdate="false" 
                    FooterTextCssClass="ca-GridFooterText" ImagesBaseUrl="~/Apps/Shell/styles/images/" CssClass="ca-Grid" AllowMultipleSelect="false"
                    SearchTextCssClass="ca-GridHeaderText"
                    HeaderCssClass="ca-GridHeader" FooterCssClass="ca-GridFooter" GroupByCssClass="ca-GroupByCell"
                    GroupByTextCssClass="ca-GroupByText" PageSize="20" PagerStyle="Numbered" PagerTextCssClass="ca-GridFooterText"
                    PagerButtonWidth="41" PagerButtonHeight="22" SliderHeight="20" SliderWidth="150"
                    SliderGripWidth="9" SliderPopupOffsetX="20" GroupingPageSize="5" PreExpandOnGroup="true"    
                    IndentCellWidth="1" GroupingNotificationTextCssClass="ca-GridHeaderText" GroupBySortImageWidth="10" GroupBySortImageHeight="10"
                    LoadingPanelClientTemplateId="LoadingFeedbackTemplate" LoadingPanelPosition="MiddleCenter" FillContainer="True">
                    <Levels>
                        <ComponentArt:GridLevel ShowTableHeading="false" ShowSelectorCells="false"  RowCssClass="ca-Row"
                            DataCellCssClass="ca-DataCell" HeadingCellCssClass="ca-HeadingCell"
                            HeadingCellHoverCssClass="ca-HeadingCellHover" HeadingCellActiveCssClass="ca-HeadingCellActive"
                            HeadingRowCssClass="ca-HeadingRow" HeadingTextCssClass="ca-HeadingCellText" SelectedRowCssClass="ca-SelectedRow"
                            GroupHeadingCssClass="ca-GroupHeading" SortAscendingImageUrl="grid/asc.gif" SortDescendingImageUrl="grid/desc.gif"
                            SortImageWidth="10" SortImageHeight="19" EditCommandClientTemplateId="EditCommandTemplate">
                            <Columns>
                            </Columns>
                        </ComponentArt:GridLevel>
                    </Levels>
                    <ClientTemplates>
                        <ComponentArt:ClientTemplate ID="EditTemplate">
                            <a href="javascript:RelationsGrid.deleteItem(RelationsGrid.getItemFromClientId('## DataItem.ClientId ##'))">
                                <img alt="delete" title="delete" src="../../../Apps/Shell/Styles/Images/toolbar/delete.gif" />
                            </a>
                        </ComponentArt:ClientTemplate>
                        <ComponentArt:ClientTemplate ID="EditCommandTemplate">
                            <a href="javascript:RelationsGrid.deleteItem(RelationsGrid.getItemFromClientId('## DataItem.ClientId ##'))">
                                <img alt="delete" title="delete" src="../../../Apps/Shell/Styles/Images/toolbar/delete.gif" />
                            </a>
                        </ComponentArt:ClientTemplate>
                    </ClientTemplates>
                </ComponentArt:Grid>
            </td>
        </tr>
    </table>
</div>