<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Order.Tabs.LineItemsEditTab" Codebehind="LineItemsEditTab.ascx.cs" %>
<%@ Register Src="../../Core/Controls/DialogControl.ascx" TagName="DialogControl" TagPrefix="uc1" %>
<script type="text/javascript">
    function LineItemAddresses_CloseDialog() {
        LineItemAddressesDialog.close();
        LineItemsGrid.callback();
        CSManagementClient.MarkDirty();
    }
    
    function LineItemAddresses_OpenDialog()
    {
        LineItemAddressesDialog.show(null, 'Edit LineItem Information');
    }

    function LineItemsGrid_Delete(id) {
        LineItemsGrid.deleteItem(id);
        LineItemsGrid.callback();
    }
    
    function LineItem_Edit(id)
    {
        if(parseInt(id)==0)
        {
            // need to pass catalogEntryCode for the new LineItem
            var code = GetCatalogEntryCode();
            if(code!=null)
                ecf_UpdateLineItemAddressesDialogControl(id+'|'+code);
            else
                return;
        }
        else
        {
            // code is not needed if lineItem already exists
            ecf_UpdateLineItemAddressesDialogControl(id);
        }
            
        // show popup for editing/creating LineItem
        LineItemAddresses_OpenDialog();
    }
    
    function GetCatalogEntryCode()
    {
        // returns selected catalog entry
        
        var searchbox = LineItemsFilter; //$get('<%=LineItemsFilter.ClientID %>');
        var selectedItem = null;
        
        try 
        {
            selectedItem = searchbox.getSelectedItem();
        }
        catch(e)
        {
        }

        if(selectedItem == null)
        {
            //alert('Must select an item!');
            return "";
        }
        else
            return selectedItem.get_value();
    }
</script>
<div id="DataForm">
 <table width="100%" class="DataForm"> 
     <tr>
        <td>
            <table>
                <tr>
                    <td>
                        <ComponentArt:ComboBox id="LineItemsFilter" runat="server" RunningMode="Callback"
                            AutoHighlight="false"
                            AutoComplete="true"
                            AutoFilter="true"
                            CssClass="comboBox"
                            HoverCssClass="comboBoxHover"
                            FocusedCssClass="comboBoxHover"
                            TextBoxCssClass="comboTextBox"
                            TextBoxHoverCssClass="comboBoxHover"
                            DropDownCssClass="comboDropDown"
                            ItemCssClass="comboItem"
                            ItemHoverCssClass="comboItemHover"
                            SelectedItemCssClass="comboItemHover"
                            DropHoverImageUrl="../../../Apps/Shell/Styles/images/combobox/drop_hover.gif"
                            DropImageUrl="../../../Apps/Shell/Styles/images/combobox/drop.gif"
                            DropDownPageSize="10"
                            Width="350">
                            <Items>
                                <ComponentArt:ComboBoxItem Text="" />
                            </Items>
                        </ComponentArt:ComboBox>
                    </td>
                    <td>
                        <a href="#" onclick="LineItem_Edit('0')"><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:SharedStrings, Add_Line_Item %>"/></a>
                    </td>
                </tr>
            </table>
        </td>
     </tr>
     <tr>
        <td>&nbsp;</td>
     </tr>
     <tr>
        <td class="wh100">
            <ComponentArt:Grid Debug="false" AllowEditing="true" RunningMode="Client" AutoFocusSearchBox="false" AllowPaging="false" PageSize="500" PagerStyle="Numbered" PagerTextCssClass="ca-GridFooterText"
                ShowHeader="false" ShowFooter="false" SkinID="Inline" runat="server" ID="LineItemsGrid" ImagesBaseUrl="~/Apps/Shell/styles/images/" 
                AutoPostBackOnInsert="false" AutoPostBackOnDelete="false" AutoPostBackOnUpdate="false" 
                AutoCallBackOnInsert="false" AutoCallBackOnDelete="false" AutoCallBackOnUpdate="false">
                <Levels>
                    <ComponentArt:GridLevel ShowTableHeading="false" ShowSelectorCells="false" RowCssClass="ca-Row"
                        DataCellCssClass="ca-DataCell" HeadingCellCssClass="ca-HeadingCell" 
                        HeadingCellHoverCssClass="ca-HeadingCellHover" HeadingCellActiveCssClass="ca-HeadingCellActive"
                        HeadingRowCssClass="ca-HeadingRow" HeadingTextCssClass="ca-HeadingCellText" SelectedRowCssClass="ca-SelectedRow"
                        GroupHeadingCssClass="ca-GroupHeading" SortAscendingImageUrl="grid/asc.gif" SortDescendingImageUrl="grid/desc.gif"
                        SortImageWidth="10" SortImageHeight="19" EditCommandClientTemplateId="EditCommandTemplate" InsertCommandClientTemplateId="InsertCommandTemplate"
                        DataKeyField="LineItemId">
                        <Columns>
                        </Columns>
                    </ComponentArt:GridLevel>
                </Levels>
                <ClientTemplates>
                      <ComponentArt:ClientTemplate Id="EditTemplate">
                        <a href="javascript:LineItem_Edit(LineItemsGrid.getItemFromClientId('## DataItem.ClientId ##').getMember('LineItemId').get_text());">
                            <img alt="edit" title="edit" src="../../../Apps/Shell/Styles/images/edit.gif" /></a> | <a href="javascript:LineItemsGrid_Delete(LineItemsGrid.getItemFromClientId('## DataItem.ClientId ##'));">
                                <img alt="delete" title="delete" src="../../../Apps/Shell/Styles/images/toolbar/delete.gif" /></a>
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
</div>
<uc1:DialogControl id="LineItemAddressesDialog" AppId="Order" Width="700" ViewId="View-LineItem" runat="server" />