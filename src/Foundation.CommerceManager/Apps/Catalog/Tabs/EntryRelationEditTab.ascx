<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Mediachase.Commerce.Manager.Catalog.Tabs.EntryRelationEditTab" Codebehind="EntryRelationEditTab.ascx.cs" %>

<script type="text/javascript">
    function Variations_AddRow()
    {
        var searchbox = ItemsFilter;        
        var selectedItem = null;
        
        try 
        {
            selectedItem = searchbox.getSelectedItem();
        }
        catch(e)
        {
            alert('Must select an item');
            return;
        }
        
        var id = selectedItem.get_value();
        var name = selectedItem.get_text();
        
        // check if item with this id already exists
        for(var index = 0; index < EntryRelationDefaultGrid.Table.getRowCount(); index++)
        {
            var row = EntryRelationDefaultGrid.Table.getRow(index);

            if(row.getMemberAt(1).get_text() == id)
            {
                alert('Record with id "' + id + '" already exists');
                return;
            }
        }

        var row = EntryRelationDefaultGrid.Table.addEmptyRow(); 
        EntryRelationDefaultGrid.beginUpdate();
        row.SetValue(1, id, true, true); 
        row.SetValue(2, name, true, true);
        row.SetValue(3, 'unknown', true, true);
        row.SetValue(5, 1, true, true); 
        row.SetValue(6, 'default', true, true); 
        row.SetValue(7, 0, false, false);
        EntryRelationDefaultGrid.endUpdate();
        
        return false;
    }

    function EntryRelationDefaultGrid_delete(rowId) {
        EntryRelationDefaultGrid.editCancel();
        EntryRelationDefaultGrid.deleteItem(EntryRelationDefaultGrid.getItemFromClientId(rowId));
    }

</script>

<div id="DataForm">
    <table class="DataForm">
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:CatalogStrings, Entry_Find_Item %>" />:</td>
                        <td>
                            <ComponentArt:ComboBox ID="ItemsFilter" runat="server" RunningMode="CallBack" AutoHighlight="false" AutoComplete="true"
                                AutoFilter="true" CssClass="comboBox" HoverCssClass="comboBoxHover" FocusedCssClass="comboBoxHover"
                                TextBoxCssClass="comboTextBox" TextBoxHoverCssClass="comboBoxHover" DropDownCssClass="comboDropDown"
                                ItemCssClass="comboItem" ItemHoverCssClass="comboItemHover" SelectedItemCssClass="comboItemHover"
                                DropHoverImageUrl="~/Apps/Shell/Styles/images/combobox/drop_hover.gif" DropImageUrl="~/Apps/Shell/Styles/images/combobox/drop.gif"
                                DropDownPageSize="10" ItemClientTemplateId="itemTemplate" Width="350">
                                <ClientTemplates>
                                    <ComponentArt:ClientTemplate ID="itemTemplate"><img src="## DataItem.getProperty('icon') ##" />
                                        ## DataItem.getProperty('Text') ##</ComponentArt:ClientTemplate>
                                </ClientTemplates>
                            </ComponentArt:ComboBox>
                        </td>
                        <td>
                            <asp:Button runat="server" ID="VariationAddButton" UseSubmitBehavior="false" OnClientClick="Variations_AddRow();return;"
                                Text="<%$ Resources:CatalogStrings, Entry_Add_Item %>" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <ComponentArt:Grid AllowEditing="true" RunningMode="Client" AutoFocusSearchBox="false"
                    ShowHeader="false" ShowFooter="true" Width="800" SkinID="Inline" runat="server"
                    ID="EntryRelationDefaultGrid" AutoPostBackOnInsert="false" AutoPostBackOnDelete="false"
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
                            SortImageWidth="10" SortImageHeight="19" EditCommandClientTemplateId="EditCommandTemplate" InsertCommandClientTemplateId="InsertCommandTemplate">
                            <Columns>
                            </Columns>
                        </ComponentArt:GridLevel>
                    </Levels>
                    <ClientTemplates>
                        <ComponentArt:ClientTemplate ID="CheckHeaderTemplate">
                            <input type="checkbox" name="HeaderCheck" />
                        </ComponentArt:ClientTemplate>
                        <ComponentArt:ClientTemplate ID="HyperlinkTemplate">
                            <a href="javascript:EntryRelationDefaultGrid.edit(EntryRelationDefaultGrid.getItemFromClientId('## DataItem.Arguments ##'));">
                                <img alt="edit" src="../../../Apps/Shell/Styles/Images/edit.gif" /></a> | <a href="javascript:EntryRelationDefaultGrid.deleteItem(EntryRelationDefaultGrid.getItemFromClientId('## DataItem.ClientId ##'))">
                                    <img alt="delete" src="../../../Apps/Shell/Styles/Images/toolbar/delete.gif" /></a>
                        </ComponentArt:ClientTemplate>
                        <ComponentArt:ClientTemplate ID="LoadingFeedbackTemplate">
                            <table cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td style="font-size: 10px;">
                                        Loading...&nbsp;</td>
                                    <td>
                                        <img src="../../../Apps/Shell/Styles/images/grid/spinner.gif" width="16" height="16" border="0"></td>
                                </tr>
                            </table>
                        </ComponentArt:ClientTemplate>
                        <ComponentArt:ClientTemplate ID="EditTemplate">
                            <a href="javascript:EntryRelationDefaultGrid.edit(EntryRelationDefaultGrid.getItemFromClientId('## DataItem.ClientId ##'));">
                                <img alt="edit" title="edit" src="../../../Apps/Shell/Styles/Images/edit.gif" /></a> | 
                            <a href="javascript:EntryRelationDefaultGrid_delete('## DataItem.ClientId ##')">
                                    <img alt="delete" title="delete" src="../../../Apps/Shell/Styles/Images/toolbar/delete.gif" /></a>
                        </ComponentArt:ClientTemplate>
                        <ComponentArt:ClientTemplate ID="EditCommandTemplate">
                            <a href="javascript:EntryRelationDefaultGrid.editComplete();">Update</a> | <a href="javascript:EntryRelationDefaultGrid.editCancel();">Cancel</a>
                        </ComponentArt:ClientTemplate>
                        <ComponentArt:ClientTemplate ID="InsertCommandTemplate">
                            <a href="javascript:EntryRelationDefaultGrid.editComplete();">Insert</a> | <a href="javascript:EntryRelationDefaultGrid.editCancel();">Cancel</a>
                        </ComponentArt:ClientTemplate>
                        <ComponentArt:ClientTemplate ID="RelatedItemLinkTemplate">
                            <a href="javascript:CSCatalogClient.ViewItem('## DataItem.getMember('Type').get_value() ##', '## DataItem.getMember('ID').get_value() ##');">## DataItem.getMember('Name').get_value() ##</a>
                        </ComponentArt:ClientTemplate>
                    </ClientTemplates>
                </ComponentArt:Grid>
            </td>
        </tr>
    </table>
</div>
