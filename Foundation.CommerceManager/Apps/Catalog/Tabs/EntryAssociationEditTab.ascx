<%@ Control Language="C#" EnableViewState="true" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Catalog.Tabs.EntryAssociationEditTab" Codebehind="EntryAssociationEditTab.ascx.cs" %>
<%@ Register Src="../../Core/Controls/DialogControl.ascx" TagName="DialogControl" TagPrefix="uc1" %>
<script type="text/javascript">

    var SelectedAssociationId = 0;
    var SelectedEntryItem = null;
    
    /*---------- BEGIN: Dialog methods -----------------------------------------------------------------------*/
    function EntryAssociationEdit_CloseDialog(id, name)
    {
        EntryAssociationEditDialog.close();
        CSManagementClient.MarkDirty();

        if (id != 0) {

            // First check if association with that name already exists
            var itemTemp = AssociationsFilter.findItemByProperty('Text', name);
            if (itemTemp != null && itemTemp.get_value() != id) {
                alert('Association with that name already exists.')
                return;
            }

            // select the item
            // the method findItemByProperty must be used with CA v2007.2 (instead of FindComboBoxItem that is used now)
            var item = AssociationsFilter.findItemByProperty('Value', id);
            if (item != null) 
            {
				if(item.get_text() != name) {
            		AssociationsFilter.beginUpdate();
            		item.set_text(name);
            		AssociationsFilter.set_text(name);
            		AssociationsFilter.endUpdate();
            	}
            	AssociationsFilter.selectItem(item);
			}
            else
            // add a new item
            	if (itemTemp == null) {
            		AssociationsFilter.beginUpdate();
            		var newItem = new ComponentArt.Web.UI.ComboBoxItem();
            		newItem.set_value(id);
            		newItem.set_text(name);
            		AssociationsFilter.addItem(newItem);
            		AssociationsFilter.endUpdate();
				}

            AssociationsFilter.focus();
        }
    }
    
    function EntryAssociationEdit_OpenDialog()
    {
        EntryAssociationEditDialog.show(null, 'Edit Association Information');
    }
    
    function EntryAssociationItem_Edit(id)
    {
        // update dialog control
        ecf_UpdateEntryAssociationEditDialogControl(id);
            
        // show popup for editing/creating EntryAssociation
        EntryAssociationEdit_OpenDialog();
    }

    function EditAssociationItem()
    {
        var item = AssociationsFilter.getSelectedItem();
        if(item!=null)
            EntryAssociationItem_Edit(item.get_value());
        else
            alert("Must select an item!");
    }
    /*---------- END: Dialog methods -----------------------------------------------------------------------*/
    
    /*---------- BEGIN: Association Items -----------------------------------------------------------------------*/
    
    // searches for an item in AssociationFilter combobox by item's value
    // returns item index
    function FindComboBoxItem(itemValue)
    {
        var itemIndex = -1;
        
        var itemCount = AssociationsFilter.get_itemCount();
        if(itemCount==1)
            return 0;
        else
        {
            for(var i=0; i<itemCount; i++)
            {
                var itemTmp = AssociationsFilter.getItem(i);
                if(itemTmp!=null && itemTmp.get_value()==itemValue)
                {
                    itemIndex = i;
                    break;
                }
            }
        }
        
        return itemIndex;
    }
    
    function ToggleLoadDetailsButton(enable)
    {
    	var idObj = $get('<%= btnLoadDetails.ClientID %>');
        if(idObj!=null)
            idObj.disabled = !enable;
    }
    
    function ecf_ShowAssociation(val)
    {
        var index = AssociationsFilter.get_selectedIndex();
        if(index >= 0)
        {   
            try
            {
                var item = AssociationsFilter.getSelectedItem();
                if(item!=null)
                {
                    // remember selected association id
                    SetSelectedAssociationId(item.get_value());
                    SetItemsTriggerValue(item.get_value());
                    
                    // load association item
                    __doPostBack('<%=ItemsPanelTrigger.UniqueID %>','');
                    $get('<%= btnLoadDetails.ClientID %>').disabled = false;
                }
            }
            catch(e)
            {
                alert('Must select an item '+e.message);
                return;
            }
        }
    }
    
    function SetSelectedAssociationId(val)
    {
        SelectedAssociationId = val;
        var idObj = $get('<%= SelectedAssociationIdField.ClientID %>');
        if(idObj!=null)
            idObj.value = val;
        else
            alert('SetSelectedAssociationId: idObj==null');
    }
    
    function SetItemsTriggerValue(val)
    {
        var idObj2 = $get('<%= ItemsPanelTrigger.ClientID %>');
        if(idObj2!=null)
            idObj2.value = val;
        else
            alert('SetSelectedAssociationId: idObj2==null');
    }

    function AssociationsFilter_indexChanged()
    {   
        ToggleLoadDetailsButton(true);
        
        var item = AssociationsFilter.getSelectedItem();
        if(item!=null)
        {
             SetSelectedAssociationId(item.get_value());
         }
    }
    
    function AssociataionItemsFilter_indexChanged(sender, eventArgs)
    {
        SelectedEntryItem = sender.getSelectedItem();
    }
       
    function AssociationItemsGrid_AddRow(defaultAssociationType)
    {
        // check if we have an association selected
        var idObj = $get('<%= SelectedAssociationIdField.ClientID %>');
        if(idObj!=null && !isNaN(parseInt(idObj.value)))
            SelectedAssociationId = idObj.value;
    
        var selectedItem = null;
        
        try 
        {
            selectedItem = SelectedEntryItem;
        }
        catch(e)
        {
            alert('Must select an item');
            return;
        }
        
        if(selectedItem!=null)
        {
            var id = selectedItem.get_value();
            var name = selectedItem.get_text();
            
            // check if item with this id already exists
            for(var index = 0; index < AssociationItemsGrid.Table.getRowCount(); index++)
            {
                var row = AssociationItemsGrid.Table.getRow(index);

                if((row.getMemberAt(1).get_text() == SelectedAssociationId) &&
                    (row.getMemberAt(2).get_text() == id) )
                {
                    alert('Record with "'+id+'" id already exists');
                    return;
                }
            }
            
            var rowCount = AssociationItemsGrid.Table.getRowCount();
            
            // add new item
            var row = AssociationItemsGrid.Table.addEmptyRow(); 
            
            if(row != null)
            {
                AssociationItemsGrid.beginUpdate();
                row.setValue(1, SelectedAssociationId, true, true);
                row.setValue(2, id, true, true); 
                row.setValue(3, name, true, true);
                row.setValue(4, 0, true, true);
                row.setValue(5, defaultAssociationType, false, false);
                AssociationItemsGrid.endUpdate();
            }
        }

        return false;
    }
    
    function AssociationItemsGrid_onCallbackError(sender, eventArgs)
    {
        if (confirm('Invalid data has been entered. View details?')) 
            alert(eventArgs.get_errorMessage());
        sender.page(0);
    }
    /*---------- END: Association Items -----------------------------------------------------------------------*/

    function AssociationEditTabClose() {
        var dropDown = AssociationsFilter;
        dropDown.Collapse();

    }
</script>

<asp:HiddenField ID="SelectedAssociationIdField" runat="server" /> <!-- used for displaying selected association info -->
<asp:HiddenField runat="server" id="ItemsPanelTrigger"/>
<div id="DataForm">
 <table class="DataForm">
    <tr>
        <td>
            <input type="button" runat="server" onclick="EntryAssociationItem_Edit(0);" id="showDlg" value="<%$ Resources:CatalogStrings, Add_Association %>" />
        </td>
     </tr>
    <tr>
        <td>
            <table>
                <tr>
                    <td><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:CatalogStrings, Entry_Modify_Existing_Association %>" />:</td>
                    <td>
                          <ComponentArt:ComboBox 
                              id="AssociationsFilter" 
                              runat="server"  
                              RunningMode="CallBack"
                              AutoHighlight ="false"
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
                              DropHoverImageUrl="~/Apps/Shell/Styles/images/combobox/drop_hover.gif"
                              DropImageUrl="~/Apps/Shell/Styles/images/combobox/drop.gif"
                              DropDownPageSize="10"
                              Width="350"
                              Debug="False">
                              <ClientEvents>
                                  <Change EventHandler="AssociationsFilter_indexChanged" />
                              </ClientEvents>
                              <Items>
                                  <ComponentArt:ComboBoxItem Text="" />
                              </Items>
                          </ComponentArt:ComboBox>
                    </td>
                    <td>
                        <input type="button" id="btnLoadDetails" runat="server" onclick="this.disabled=true;ecf_ShowAssociation(AssociationsFilter.get_selectedIndex());" value="<%$ Resources:CatalogStrings, Catalog_Edit_Details %>" />&nbsp;&nbsp;
                        <input type="button" onclick="EditAssociationItem();" id="Button1" value='<asp:Literal ID="Literal11" runat="server" Text="<%$ Resources:CatalogStrings, Entry_Modify_Association %>" />' onmouseover="this.style.cursor='pointer';" />&nbsp;&nbsp;&nbsp;
                        <asp:Button runat="server" ID="btnDelete" Text="<%$ Resources:SharedStrings, Delete %>" CommandArgument="javascript:(AssociationsFilter.getSelectedItem()!=null)?AssociationsFilter.getSelectedItem().get_value():0;" />
                    </td>
                </tr>
            </table>
        </td>
     </tr>
    <tr>
       <td>&nbsp;</td>
    </tr>
 </table>

 <asp:UpdatePanel ID="pnlAssociationItems" runat="server" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ItemsPanelTrigger" />
    </Triggers>
    <ContentTemplate>
         <asp:Panel id="pnlSelectedAssociation" runat="server" Visible="false">
            <asp:Label ID="lblAssociationName" runat="server" CssClass="FormFieldCell" Font-Bold="true"></asp:Label><br />
            <asp:Label ID="lblAssociationDescription" runat="server" Font-Bold="true" CssClass="FormLabelCell"></asp:Label>
            <br />
            
            <table class="DataForm"> 
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td><asp:Label ID="lblPickItem" runat="server" CssClass="FormFieldCell" Text="<%$ Resources:CatalogStrings, Entry_Pick_Item %>"></asp:Label>:</td>
                                <td>
                                  <ComponentArt:ComboBox id="AssociataionItemsFilter" runat="server"
                                    DataTextField="Name" DataValueField="CatalogEntryId" RunningMode="Callback"
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
                                    DropHoverImageUrl="~/Apps/Shell/Styles/images/combobox/drop_hover.gif"
                                    DropImageUrl="~/Apps/Shell/Styles/images/combobox/drop.gif"
                                    DropDownPageSize="10" CacheSize="200" FilterCacheSize="1"
                                    Width="350px" ItemClientTemplateId="itemTemplate">
                                    <ClientEvents>
                                        <Change EventHandler="AssociataionItemsFilter_indexChanged" />
                                    </ClientEvents>
                                <ClientTemplates>
                                    <ComponentArt:ClientTemplate ID="itemTemplate"><img src="## DataItem.getProperty('icon') ##" />
                                        ## DataItem.getProperty('Text') ##</ComponentArt:ClientTemplate>
                                </ClientTemplates>                                                              
                                  </ComponentArt:ComboBox>
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="AssociationItemAddButton" UseSubmitBehavior="false" Text="<%$ Resources:CatalogStrings, Entry_Add_Item %>" />
                                </td>
                            </tr>
                        </table>
                    </td>
                 </tr>
                 <tr>
                    <td>
                        <ComponentArt:Grid Debug="false" AllowEditing="true" EnableViewState="true" Width="800"
                            RunningMode="Client" 
                            AutoFocusSearchBox="false" ShowHeader="false" ShowFooter="true" 
                            SkinID="Inline" runat="server" ID="AssociationItemsGrid" 
                            AutoPostBackOnInsert="false" AutoPostBackOnDelete="false" AutoPostBackOnUpdate="false" 
                            AutoCallBackOnInsert="false" AutoCallBackOnDelete="false" AutoCallBackOnUpdate="false"
                            FooterTextCssClass="ca-GridFooterText" ImagesBaseUrl="~/Apps/Shell/styles/images/" CssClass="ca-Grid" AllowMultipleSelect="false"
                            SearchTextCssClass="ca-GridHeaderText"
                            HeaderCssClass="ca-GridHeader" FooterCssClass="ca-GridFooter" GroupByCssClass="ca-GroupByCell"
                            GroupByTextCssClass="ca-GroupByText" PageSize="20" PagerStyle="Numbered" PagerTextCssClass="ca-GridFooterText"
                            PagerButtonWidth="41" PagerButtonHeight="22" SliderHeight="20" SliderWidth="150"
                            SliderGripWidth="9" SliderPopupOffsetX="20" GroupingPageSize="5" PreExpandOnGroup="true"    
                            IndentCellWidth="1" GroupingNotificationTextCssClass="ca-GridHeaderText" GroupBySortImageWidth="10" GroupBySortImageHeight="10"
                            LoadingPanelClientTemplateId="LoadingFeedbackTemplate" LoadingPanelPosition="MiddleCenter" FillContainer="False">
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
                                    <a href="javascript:AssociationItemsGrid.edit(AssociationItemsGrid.getItemFromClientId('## DataItem.Arguments ##'));"><img alt="edit" src="../../../Apps/Shell/Styles/Images/edit.gif" /></a> | <a href="javascript:AssociationItemsGrid.deleteItem(AssociationItemsGrid.getItemFromClientId('## DataItem.ClientId ##'))"><img alt="delete" src="../../../Apps/Shell/Styles/Images/toolbar/delete.gif" /></a>
                                </ComponentArt:ClientTemplate>
                                
                                <ComponentArt:ClientTemplate ID="LoadingFeedbackTemplate">
                                    <table cellspacing="0" cellpadding="0" border="0">
                                        <tr>
                                            <td style="font-size: 10px;">
                                                Loading...&nbsp;</td>
                                            <td>
                                                <img src="Apps/Shell/Styles/images/grid/spinner.gif" alt="Loading" width="16" height="16" border="0">
                                        </tr>
                                    </table>
                                </ComponentArt:ClientTemplate>
                                  <ComponentArt:ClientTemplate Id="EditTemplate">
                                    <a href="javascript:AssociationItemsGrid.edit(AssociationItemsGrid.getItemFromClientId('## DataItem.ClientId ##'));"><img alt="edit" title="edit" src="../../../Apps/Shell/Styles/Images/edit.gif" /></a> | <a href="javascript:AssociationItemsGrid.deleteItem(AssociationItemsGrid.getItemFromClientId('## DataItem.ClientId ##'))"><img alt="delete" title="delete" src="../../../Apps/Shell/Styles/Images/toolbar/delete.gif" /></a>
                                  </ComponentArt:ClientTemplate>
                                  <ComponentArt:ClientTemplate Id="EditCommandTemplate">
                                    <a href="javascript:AssociationItemsGrid.editComplete();">Update</a> | <a href="javascript:AssociationItemsGrid.editCancel();">Cancel</a>
                                  </ComponentArt:ClientTemplate>
                                  <ComponentArt:ClientTemplate Id="InsertCommandTemplate">
                                    <a href="javascript:AssociationItemsGrid.editComplete();">Insert</a> | <a href="javascript:AssociationItemsGrid.editCancel();">Cancel</a>
                                  </ComponentArt:ClientTemplate>
                            </ClientTemplates>
                            <ClientEvents>
                                <CallbackError EventHandler="AssociationItemsGrid_onCallbackError" />
                            </ClientEvents>
                        </ComponentArt:Grid>
                    </td>
                 </tr>
            </table>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

<!-- END: Association Items -->
</div>
<uc1:DialogControl id="EntryAssociationEditDialog" AppId="Catalog" Width="700" ViewId="View-EntryAssociation" runat="server" />