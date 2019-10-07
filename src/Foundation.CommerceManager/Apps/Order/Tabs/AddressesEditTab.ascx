<%@ Control Language="C#" EnableViewState="true" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Order.Tabs.AddressesEditTab" Codebehind="AddressesEditTab.ascx.cs" %>
<%@ Register Src="~/Apps/Core/Controls/DialogControl.ascx" TagName="DialogControl" TagPrefix="uc1" %>
<%@ Register Src="~/Apps/Core/MetaData/EditTab.ascx" TagName="MetaData" TagPrefix="ecf" %>
<script type="text/javascript">
    function Address_CloseDialog() {
        AddressViewDialog.close();
        AddressList.callback();
        CSManagementClient.MarkDirty();
    }

    function Address_OpenDialog() {
        AddressViewDialog.show(null, 'Edit Address Information');
    }

    function Address_Edit(id) {
        ecf_UpdateAddressDialogControl(id);
        Address_OpenDialog();
    }

    function Address_Delete(id) {
        AddressList.deleteItem(id);
        AddressList.callback();
    }

    function AddressList_onCallbackComplete() {
        ecf_UpdateOrderAddressDropDown();
    }
</script>
<div id="DataForm">
 <table width="100%" class="DataForm"> 
 <tr>
    <td>
        <table>
            <tr>
                <td>
                    <a href="#" onclick="Address_Edit(0)"><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:SharedStrings, New_Address %>"/></a>
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
                        
<ComponentArt:Grid Debug="false" AllowEditing="false" RunningMode="Client" 
    AutoFocusSearchBox="false" ShowHeader="false" ShowFooter="false" Width="700" SkinID="Inline" runat="server" 
    ID="AddressList" AutoPostBackOnInsert="false" AutoPostBackOnDelete="false" AutoCallBackOnUpdate="false"
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
            SortImageWidth="10" SortImageHeight="19" EditCommandClientTemplateId="EditCommandTemplate" InsertCommandClientTemplateId="InsertCommandTemplate"
            DataKeyField="OrderGroupAddressId">
            <Columns>
            </Columns>
        </ComponentArt:GridLevel>
    </Levels>
    <ClientTemplates>
          <ComponentArt:ClientTemplate Id="EditTemplate">
            <a href="javascript:Address_Edit(AddressList.getItemFromClientId('## DataItem.ClientId ##').getMember('OrderGroupAddressId').get_text());">
                <img alt="edit" title="edit" src="../../../Apps/Shell/Styles/Images/edit.gif" /></a> | <a href="javascript:Address_Delete(AddressList.getItemFromClientId('## DataItem.ClientId ##'));">
                    <img alt="delete" title="delete" src="../../../Apps/Shell/Styles/Images/toolbar/delete.gif" /></a>
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
    </ClientTemplates>
    <ClientEvents>
        <CallbackComplete EventHandler="AddressList_onCallbackComplete" />
    </ClientEvents>
</ComponentArt:Grid>

    </td>
 </tr>
 </table>
</div>
<uc1:DialogControl id="AddressViewDialog" AppId="Order" Width="700" ViewId="View-Address" runat="server">
</uc1:DialogControl>