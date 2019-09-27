<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Catalog.Tabs.EntryPricingEditTab" Codebehind="EntryPricingEditTab.ascx.cs" %>
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
    function SalePriceItem_Delete(rowId) {
        SalePricesGrid.deleteItem(SalePricesGrid.getItemFromClientId(rowId));
        SalePricesGrid.callback();
    }
    function SalePriceItem_EditOnDoubleClick(item) { 
        <%=SalePricesGrid.ClientID %>.edit(<%=SalePricesGrid.ClientID %>.getItemFromClientId(item.get_clientId()));
        return true;
    }
</script>
<div id="DataForm">
    <table class="DataForm">
        <tr id="ListPriceRow" runat="server">
            <td class="FormLabelCell">
                <asp:Label runat="server" Text="<%$ Resources:CatalogStrings, Entry_Display_Price %>"></asp:Label></td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" Width="250" ID="ListPrice"></asp:TextBox><br />
                <ajaxToolkit:MaskedEditExtender Enabled="false" ID="ListPriceMaskedEditExtender" runat="server" TargetControlID="ListPrice"
                    Mask="9,999,999.99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                    OnInvalidCssClass="MaskedEditError" MaskType="Number" InputDirection="RightToLeft"
                    AcceptNegative="Left" DisplayMoney="Left" />
                <asp:RangeValidator runat="server" ControlToValidate="ListPrice" MinimumValue="-100000000" MaximumValue="1000000000" 
                    Type="Currency" Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Entry_Enter_Valid_Display_Price %>"></asp:RangeValidator>
            </td>
            <td colspan="3" class="FormSpacerCell">
            </td>
        </tr>
        <tr id="ListPriceSpacerRow" runat="server">
            <td colspan="5" class="FormSpacerCell">
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label runat="server" Text="<%$ Resources:CatalogStrings, Entry_Min_Quantity %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" Width="250" ID="MinQty"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="MinQtyRequired" ControlToValidate="MinQty" Display="Dynamic" 
                    ErrorMessage="<%$ Resources:CatalogStrings, Entry_Min_Quantity_Required %>" />
                <asp:RangeValidator runat="server" ID="MinQtyRange" ControlToValidate="MinQty" MinimumValue="0" MaximumValue="1000000000" Type="Double" 
                    Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Entry_Enter_Valid_Quantity %>"></asp:RangeValidator>
                <asp:CompareValidator runat="server" ID="MinQtyCompareVaildator" ControlToValidate="MinQty" ControlToCompare="MaxQty" Operator="LessThanEqual"  Type="Double"
					ErrorMessage="<%$ Resources:CatalogStrings, Entry_Min_Quantity_Greather_That_Max %>"></asp:CompareValidator>    
            </td>
        </tr>
        <tr>
            <td colspan="5" class="FormSpacerCell">
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label runat="server" Text="<%$ Resources:CatalogStrings, Entry_Max_Quantity %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" Width="250" ID="MaxQty"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="MaxQtyRequired" ControlToValidate="MaxQty" Display="Dynamic" 
                    ErrorMessage="<%$ Resources:CatalogStrings, Entry_Max_Quantity_Required %>" />
                <asp:RangeValidator runat="server" ID="MaxQtyRange" ControlToValidate="MaxQty" MinimumValue="0" MaximumValue="1000000000" Type="Double" Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Entry_Enter_Valid_Quantity %>"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td colspan="5" class="FormSpacerCell">
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label runat="server" Text="<%$ Resources:CatalogStrings, Entry_Merchant %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <asp:DropDownList runat="server" ID="MerchantList" DataTextField="FriendlyName" DataValueField="Name">
                    <asp:ListItem Value="" Text="<%$ Resources:CatalogStrings, Entry_Select_Merchant %>"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="5" class="FormSpacerCell">
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label runat="server" Text="<%$ Resources:SharedStrings, Weight %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" Width="250" ID="Weight"></asp:TextBox><br />
                <asp:RangeValidator runat="server" ID="WeightRange" ControlToValidate="Weight" MinimumValue="0" MaximumValue="1000000000" Type="Double" Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Entry_Enter_Valid_Quantity %>"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td colspan="5" class="FormSpacerCell">
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label runat="server" Text="<%$ Resources:SharedStrings, Length %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" Width="250" ID="Length"></asp:TextBox><br />
                <asp:RangeValidator runat="server" ID="LengthRange" ControlToValidate="Length" MinimumValue="0" MaximumValue="1000000000" Type="Double" Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Entry_Enter_Valid_Quantity %>"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td colspan="5" class="FormSpacerCell">
            </td>
        </tr>
                <tr>
            <td class="FormLabelCell">
                <asp:Label runat="server" Text="<%$ Resources:SharedStrings, Height %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" Width="250" ID="Height"></asp:TextBox><br />
                <asp:RangeValidator runat="server" ID="HeightRange" ControlToValidate="Height" MinimumValue="0" MaximumValue="1000000000" Type="Double" Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Entry_Enter_Valid_Quantity %>"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td colspan="5" class="FormSpacerCell">
            </td>
        </tr>
                <tr>
            <td class="FormLabelCell">
                <asp:Label runat="server" Text="<%$ Resources:SharedStrings, Width %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" Width="250" ID="Width"></asp:TextBox><br />
                <asp:RangeValidator runat="server" ID="WidthRange" ControlToValidate="Width" MinimumValue="0" MaximumValue="1000000000" Type="Double" Display="Dynamic" ErrorMessage="<%$ Resources:CatalogStrings, Entry_Enter_Valid_Quantity %>"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td colspan="5" class="FormSpacerCell">
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label runat="server" Text="<%$ Resources:SharedStrings, Package %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <asp:DropDownList runat="server" ID="PackageList" DataTextField="FriendlyName" DataValueField="Name">
                    <asp:ListItem Value="0" Text="<%$ Resources:CatalogStrings, Entry_Select_Package %>"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="5" class="FormSpacerCell">
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label runat="server" Text="<%$ Resources:CatalogStrings, Entry_Tax_Category %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <asp:DropDownList runat="server" ID="TaxList" DataTextField="FriendlyName" DataValueField="Id">
                    <asp:ListItem Value="0" Text="<%$ Resources:CatalogStrings, Entry_Select_Tax_Category %>"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="5" class="FormSpacerCell">
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">
                <asp:Label runat="server" Text="<%$ Resources:CatalogStrings, Entry_Track_Inventory %>"></asp:Label>:</td>
            <td class="FormFieldCell">
                <ecf:BooleanEditControl ID="TrackInventory" runat="server"></ecf:BooleanEditControl>
            </td>
        </tr>
        <tr>
            <td colspan="5" class="FormSpacerCell">
            </td>
        </tr>
        <tr>
            <td colspan="5" class="FormSpacerCell">
            </td>
        </tr>        
        <tr>
        </tr>
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
                                <ComponentArt:Grid Debug="false" AllowEditing="true" RunningMode="Callback" EditOnClickSelectedItem="false" ClientSideOnDoubleClick="SalePriceItem_EditOnDoubleClick"
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
                                    LoadingPanelClientTemplateId="LoadingFeedbackTemplate" LoadingPanelPosition="MiddleCenter"
                                    AllowHtmlContent="false">
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
                                            <a href="javascript:SalePriceItem_Edit(SalePricesGrid.getItemFromClientId('## DataItem.ClientId ##').getMember('PriceValueId').get_text());"><img alt="edit" title="edit" src="../../../Apps/Shell/Styles/Images/edit.gif" /></a> | 
                                            <a href="javascript:SalePriceItem_Delete('## DataItem.ClientId ##');"><img alt="delete" title="delete" src="../../../Apps/Shell/Styles/Images/toolbar/delete.gif" /></a>
                                          </ComponentArt:ClientTemplate>
                                          <ComponentArt:ClientTemplate ID="EditCommandTemplate">
                                            <a href="javascript:SalePricesGrid.editComplete();">Update</a> | <a href="javascript:SalePricesGrid.editCancel();">Cancel</a>
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
