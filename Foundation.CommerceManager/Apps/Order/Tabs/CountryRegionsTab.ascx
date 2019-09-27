<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="CountryRegionsTab.ascx.cs" Inherits="Mediachase.Commerce.Manager.Order.Tabs.CountryRegionsTab" %>
<script type="text/javascript">
    function RegionsGrid_AddRow()
    {
        if (typeof(Page_ClientValidate) == 'function') 
        {
		    if (Page_ClientValidate('<%= NewRegionButton.ValidationGroup %>') == false)
		        return false;
        }
        
        var newRegionNameObj = $get('<%= tbRegionName.ClientID %>');
        
        if(newRegionNameObj != null)
        {
            var newRegionName = newRegionNameObj.value;
    
            // check if item with this id already exists
            for(var index = 0; index < RegionsGrid.Table.getRowCount(); index++)
            {
                var row = RegionsGrid.Table.getRow(index);

                if (row.getMemberAt(1).get_text() == newRegionName)
                {
                    alert('Region with name="'+newRegionName+'" already exists');
                    return;
                }
            }
            
            // add new item
            var row = RegionsGrid.Table.addEmptyRow(); 
            
            if(row != null)
            {
                RegionsGrid.beginUpdate();
//                row.setValue(1, id, true, true);
//                row.setValue(2, countyId, true, true); 
                row.setValue(1, newRegionName, true, true);
                row.setValue(2, 0, true, true);
                row.setValue(3, true, false, false);
                RegionsGrid.endUpdate();
            }
        }

        return false;
    }
    
    function RegionsGrid_onCallbackError(sender, eventArgs)
    {
        if (confirm('Invalid data has been entered. View details?')) 
            alert(eventArgs.get_errorMessage());
        sender.page(0);
    }
</script>
<div id="DataForm">
    <asp:UpdatePanel ID="pnlRegions" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table class="DataForm">
                <tr>
                    <td class="FormLabelCell">
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:SharedStrings, Region_Name_New %>"></asp:Label>
                    </td>
                    <td class="FormFieldCell">
                        <asp:TextBox runat="server" ID="tbRegionName" Width="300px" MaxLength="100" ValidationGroup="NewRegionValidationGroup"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button runat="server" ID="NewRegionButton" Text="<%$ Resources:SharedStrings, Add %>" UseSubmitBehavior="false" ValidationGroup="NewRegionValidationGroup" OnClientClick="javascript:RegionsGrid_AddRow();" />
                        <br />
                        <asp:RequiredFieldValidator runat="server" ID="rfvRegionName" ControlToValidate="tbRegionName" Display="Dynamic" ValidationGroup="NewRegionValidationGroup" Text="<%$ Resources:OrderStrings, Order_Country_Name_Required %>"></asp:RequiredFieldValidator>
                        <asp:CustomValidator runat="server" ID="RegionNameCheckCustomValidator" ControlToValidate="tbRegionName" OnServerValidate="RegionNameCheck" ValidationGroup="NewRegionValidationGroup" Display="Dynamic" ErrorMessage="<%$ Resources:SharedStrings, Region_Name_Exists%>" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="FormSpacerCell">
                    </td>
                </tr>
                <tr>
                    <td class="FormLabelCell" colspan="2">
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:SharedStrings, Regions %>"></asp:Label>:
                    </td>
                </tr>
                <tr>
                    <td class="FormFieldCell" colspan="2">
                        <ComponentArt:Grid Debug="false" AllowEditing="true" EnableViewState="true" RunningMode="Callback" 
                            AutoFocusSearchBox="false" ShowHeader="false" ShowFooter="false" PagerStyle="Numbered" AllowPaging="true" PageSize="10"
                            Width="700" SkinID="Inline" runat="server" ID="RegionsGrid" 
                            AutoPostBackOnInsert="false" AutoPostBackOnDelete="false" AutoPostBackOnUpdate="false" 
                            AutoCallBackOnInsert="false" AutoCallBackOnDelete="false" AutoCallBackOnUpdate="false" 
                            FooterTextCssClass="ca-GridFooterText" ImagesBaseUrl="~/Apps/Shell/styles/images/" CssClass="ca-Grid" AllowMultipleSelect="false"
                            SearchTextCssClass="ca-GridHeaderText"
                            HeaderCssClass="ca-GridHeader" FooterCssClass="ca-GridFooter" GroupByCssClass="ca-GroupByCell"
                            GroupByTextCssClass="ca-GroupByText" PagerTextCssClass="ca-GridFooterText"
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
                                <ComponentArt:ClientTemplate ID="CheckHeaderTemplate">
                                    <input type="checkbox" name="HeaderCheck" />
                                </ComponentArt:ClientTemplate>
                                <ComponentArt:ClientTemplate ID="HyperlinkTemplate">
                                    <a href="javascript:RegionsGrid.edit(RegionsGrid.getItemFromClientId('## DataItem.Arguments ##'));"><img alt="edit" src="../../../Apps/Shell/Styles/Images/edit.gif" /></a> | <a href="javascript:RegionsGrid.deleteItem(RegionsGrid.getItemFromClientId('## DataItem.ClientId ##'))"><img alt="delete" src="../../../Apps/Shell/Styles/Images/toolbar/delete.gif" /></a>
                                </ComponentArt:ClientTemplate>
                                
                                <ComponentArt:ClientTemplate ID="LoadingFeedbackTemplate">
                                    <table cellspacing="0" cellpadding="0" border="0">
                                        <tr>
                                            <td style="font-size: 10px;">Loading...&nbsp;</td>
                                            <td>
                                                <img src="../../../Apps/Shell/Styles/images/grid/spinner.gif" alt="Loading" width="16" height="16" border="0">
                                        </tr>
                                    </table>
                                </ComponentArt:ClientTemplate>
                                  <ComponentArt:ClientTemplate Id="EditTemplate">
                                    <a href="javascript:RegionsGrid.edit(RegionsGrid.getItemFromClientId('## DataItem.ClientId ##'));">
                                        <img alt="edit" title="edit" src="../../../Apps/Shell/Styles/Images/edit.gif" /></a> | <a href="javascript:RegionsGrid.deleteItem(RegionsGrid.getItemFromClientId('## DataItem.ClientId ##'))">
                                            <img alt="delete" title="delete" src="../../../Apps/Shell/Styles/Images/toolbar/delete.gif" /></a>
                                  </ComponentArt:ClientTemplate>
                                  <ComponentArt:ClientTemplate Id="EditCommandTemplate">
                                    <a href="javascript:RegionsGrid.editComplete();">Update</a> | <a href="javascript:RegionsGrid.editCancel();">Cancel</a>
                                  </ComponentArt:ClientTemplate>
                                  <ComponentArt:ClientTemplate Id="InsertCommandTemplate">
                                    <a href="javascript:RegionsGrid.editComplete();">Insert</a> | <a href="javascript:RegionsGrid.editCancel();">Cancel</a>
                                  </ComponentArt:ClientTemplate>
                            </ClientTemplates>
                            <ClientEvents>
                                <CallbackError EventHandler="RegionsGrid_onCallbackError" />
                            </ClientEvents>
                        </ComponentArt:Grid>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>