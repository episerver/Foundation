<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Catalog.Tabs.ProductPricingEditTab" Codebehind="ProductPricingEditTab.ascx.cs" %>
<%@ Register Src="~/Apps/Core/Controls/BooleanEditControl.ascx" TagName="BooleanEditControl" TagPrefix="ecf" %>
<%@ Register Src="~/Apps/Core/Controls/CalendarDatePicker.ascx" TagName="CalendarDatePicker" TagPrefix="ecf" %>
<%@ Register Src="~/Apps/Core/Controls/DialogControl.ascx" TagName="DialogControl" TagPrefix="uc1" %>
<script type="text/javascript">
    function SalePriceEdit_CloseDialog()
    {
        SalePriceEditDialog.close();
        SalePricesGrid.callback();
        CSManagementClient.MarkDirty();
    }
    
    function SalePriceEdit_OpenDialog()
    {
        SalePriceEditDialog.show(null, '<asp:Literal runat="server" Text="<%$ Resources:CatalogStrings, Edit_Sale_Price_Information %>" />');      //'Edit Sale Price Information');
    }

    function SalePriceNew_OpenDialog()
    {
        SalePriceEditDialog.show(null, '<asp:Literal runat="server" Text="<%$ Resources:CatalogStrings, New_Sale_Price_Information %>" />');      //'Edit Sale Price Information');
    }
    
    function SalePriceItem_Edit(id)
    {
        ecf_UpdateSalePriceEditDialogControl(id);
            
        // show popup for editing/creating SalePrice
        SalePriceEdit_OpenDialog();
    }

    function SalePriceItem_New(id) {
        ecf_UpdateSalePriceEditDialogControl(id);

        // show popup for editing/creating SalePrice
        SalePriceNew_OpenDialog();
    }
</script>
<div id="DataForm">

        <!-- Price section -->
        <tr>
            <td colspan="5" class="FormSpacerCell">
                <fieldset>
                    <legend>
                        <asp:Literal runat="server" Text="<%$ Resources:CatalogStrings, Entry_Pricing %>" />
                    </legend>
                    <table>
                        <tr>
                            <td>
                                <a href="#" onclick="SalePriceItem_New('0')"><asp:Literal runat="server" Text="<%$ Resources:CatalogStrings, Entry_Add_Item %>" /></a>
                            </td>
                        </tr>
                        <tr>
                            <td class="wh100">
                                <ComponentArt:Grid Debug="false" AllowEditing="true" RunningMode="Callback" 
                                    AutoFocusSearchBox="false" ShowHeader="false" ShowFooter="true" 
                                    Width="100%" SkinID="Inline" runat="server" ID="SalePricesGrid" 
                                    AutoPostBackOnInsert="false" AutoPostBackOnDelete="false" AutoPostBackOnUpdate="false" 
                                    AutoCallBackOnInsert="false" AutoCallBackOnDelete="false" AutoCallBackOnUpdate="false" 
                                    FooterTextCssClass="ca-GridFooterText" ImagesBaseUrl="~/Apps/Shell/styles/images/" CssClass="ca-Grid" AllowMultipleSelect="false"
                                    SearchTextCssClass="ca-GridHeaderText"
                                    HeaderCssClass="ca-GridHeader" FooterCssClass="ca-GridFooter" GroupByCssClass="ca-GroupByCell"
                                    GroupByTextCssClass="ca-GroupByText" PageSize="20" PagerStyle="Numbered" PagerTextCssClass="ca-GridFooterText"
                                    PagerButtonWidth="41" PagerButtonHeight="22" SliderHeight="20" SliderWidth="150"
                                    SliderGripWidth="9" SliderPopupOffsetX="20" GroupingPageSize="5" PreExpandOnGroup="true"    
                                    IndentCellWidth="1" GroupingNotificationTextCssClass="ca-GridHeaderText" GroupBySortImageWidth="10" GroupBySortImageHeight="10"
                                    LoadingPanelClientTemplateId="LoadingFeedbackTemplate" LoadingPanelPosition="MiddleCenter">
                                    <Levels>
                                        <ComponentArt:GridLevel ShowTableHeading="false" ShowSelectorCells="false"  RowCssClass="ca-Row"
                                            DataCellCssClass="ca-DataCell" HeadingCellCssClass="ca-HeadingCell"
                                            HeadingCellHoverCssClass="ca-HeadingCellHover" HeadingCellActiveCssClass="ca-HeadingCellActive"
                                            HeadingRowCssClass="ca-HeadingRow" HeadingTextCssClass="ca-HeadingCellText" SelectedRowCssClass="ca-SelectedRow"
                                            GroupHeadingCssClass="ca-GroupHeading" SortAscendingImageUrl="grid/asc.gif" SortDescendingImageUrl="grid/desc.gif"
                                            SortImageWidth="10" SortImageHeight="19" EditCommandClientTemplateId="EditCommandTemplate" InsertCommandClientTemplateId="InsertCommandTemplate">
                                            <Columns>
                                            </Columns>
                                        </ComponentArt:GridLevel>
                                    </Levels>
                                    <ClientTemplates>
                                          <ComponentArt:ClientTemplate Id="EditTemplate">
                                            <a href="javascript:SalePriceItem_Edit(SalePricesGrid.getItemFromClientId('## DataItem.ClientId ##').getMember('PriceValueId').get_text());"><img alt="edit" title="edit" src="../../../Apps/Shell/Styles/Images/edit.gif" /></a> | <a href="javascript:SalePricesGrid.deleteItem(SalePricesGrid.getItemFromClientId('## DataItem.ClientId ##'));"><img alt="delete" title="delete" src="../../../Apps/Shell/Styles/Images/toolbar/delete.gif" /></a>
                                          </ComponentArt:ClientTemplate>
                                          <ComponentArt:ClientTemplate ID="LoadingFeedbackTemplate">
                                            <table cellspacing="0" cellpadding="0" border="0">
                                                <tr>
                                                    <td style="font-size: 10px;">
                                                        Loading...&nbsp;</td>
                                                    <td>
                                                        <img src="../../../Apps/Shell/Styles/images/grid/spinner.gif" width="16" height="16" border="0">
                                                    </td>
                                                </tr>
                                            </table>
                                        </ComponentArt:ClientTemplate>
                                    </ClientTemplates>
                                </ComponentArt:Grid>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
    </table>
</div>
<uc1:DialogControl id="SalePriceEditDialog" AppId="Catalog" Width="700" ViewId="View-SalePrice" runat="server" />
