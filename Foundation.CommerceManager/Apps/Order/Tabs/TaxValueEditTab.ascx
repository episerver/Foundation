<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaxValueEditTab.ascx.cs" Inherits="Mediachase.Commerce.Manager.Order.Tabs.TaxValueEditTab" %>
<%@ Register Src="~/Apps/Core/Controls/DialogControl.ascx" TagName="DialogControl" TagPrefix="uc1" %>
<script type="text/javascript">
    function TaxValueDialog_CloseDialog()
    {
        TaxValueDialog.close();
        TaxValuesGrid.callback();
        CSManagementClient.MarkDirty();
        if (typeof(_lastCalendarInstance) != 'undefined' && _lastCalendarInstance.CalendarBehavior)
        {
			_lastCalendarInstance.CalendarBehavior.hide();
        }
    }
    
    function TaxValueDialog_OpenDialog()
    {
        TaxValueDialog.show(null, 'Edit Tax Value Information');
        TaxValueDialog.add_onClose(testCloseFunc);
    }
    
	function testCloseFunc()
	{
		if (typeof(_lastCalendarInstance) != 'undefined' && _lastCalendarInstance.CalendarBehavior)
		{
			_lastCalendarInstance.CalendarBehavior.hide();
		}
	}
    
    function TaxValue_Edit(id)
    {
        ecf_UpdateTaxValueDialogControl(id);
            
        // show popup for editing/creating TaxValue
        TaxValueDialog_OpenDialog();
    }
</script>
<div id="DataForm">
 <table width="100%" class="DataForm"> 
     <tr>
        <td>
            <a href="#" onclick="TaxValue_Edit(0)"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:OrderStrings, Tax_Add_Value %>" /></a>
        </td>
     </tr>
     <tr>
        <td>&nbsp;</td>
     </tr>
     <tr>
        <td class="wh100">
            <ComponentArt:Grid Debug="false" AllowEditing="true" RunningMode="Callback" AutoFocusSearchBox="false" 
                ShowHeader="false" ShowFooter="false" Width="700" SkinID="Inline" runat="server" ID="TaxValuesGrid" 
                AutoPostBackOnInsert="false" AutoPostBackOnDelete="false" AutoPostBackOnUpdate="false" 
                AutoCallBackOnInsert="false" AutoCallBackOnDelete="false" AutoCallBackOnUpdate="false" 
                FooterTextCssClass="ca-GridFooterText" ImagesBaseUrl="~/Apps/Shell/styles/images/" CssClass="ca-Grid" AllowMultipleSelect="false"
                SearchTextCssClass="ca-GridHeaderText"
                HeaderCssClass="ca-GridHeader" FooterCssClass="ca-GridFooter" GroupByCssClass="ca-GroupByCell"
                GroupByTextCssClass="ca-GroupByText" PageSize="50" PagerStyle="Numbered" PagerTextCssClass="ca-GridFooterText"
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
                        <a href="javascript:TaxValue_Edit(TaxValuesGrid.getItemFromClientId('## DataItem.ClientId ##').getMember('TaxValueId').get_text());">
                            <img alt="edit" title="edit" src="../../../Apps/Shell/Styles/Images/edit.gif" /></a> | <a href="javascript:TaxValuesGrid.deleteItem(TaxValuesGrid.getItemFromClientId('## DataItem.ClientId ##'));">
                                <img alt="delete" title="delete" src="../../../Apps/Shell/Styles/Images/toolbar/delete.gif" /></a>
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
<uc1:DialogControl id="TaxValueDialog" AppId="Order" Width="400" ViewId="View-TaxValueEdit" runat="server" />