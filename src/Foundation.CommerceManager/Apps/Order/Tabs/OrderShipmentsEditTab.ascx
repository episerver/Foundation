<%@ Control Language="C#" EnableViewState="true" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Order.Tabs.OrderShipmentsEditTab" Codebehind="OrderShipmentsEditTab.ascx.cs" %>
<%@ Register Src="../../Core/Controls/DialogControl.ascx" TagName="DialogControl"
    TagPrefix="uc1" %>
<script type="text/javascript">  
    function Shipment_CloseDialog()
    {
		var obj = document.getElementById(ShipmentViewDialog.Id);
		obj.style.display = 'none';
        ShipmentViewDialog.close();
        ShipmentList.callback();
        CSManagementClient.MarkDirty();
    }
    
    function Shipment_OpenDialog()
    {
		var obj = document.getElementById(ShipmentViewDialog.Id);
		obj.style.display = 'block';
        ShipmentViewDialog.show(null, 'Edit Shipment Information');
    }
    
    function Shipment_Edit(id)
    {
        ecf_UpdateShipmentDialogControl(id);
        Shipment_OpenDialog();
    }

    function Shipment_Delete(id) {
        ShipmentList.deleteItem(id);
        ShipmentList.callback();
    }
</script>
<div id="DataForm">
 <table width="100%" class="DataForm"> 
 <tr>
    <td>
        <table>
            <tr>
                <td>
                    <a href="#" onclick="Shipment_Edit(0)"><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:SharedStrings, New_Shipment %>"/></a>
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
            AutoFocusSearchBox="false" ShowHeader="false" ShowFooter="false" SkinID="Inline" runat="server" 
            ID="ShipmentList" ImagesBaseUrl="~/Apps/Shell/styles/images/"
            AutoPostBackOnInsert="false" AutoPostBackOnDelete="false" AutoCallBackOnUpdate="false">
            <Levels>
                <ComponentArt:GridLevel ShowTableHeading="false" ShowSelectorCells="false"  RowCssClass="ca-Row"
                    DataCellCssClass="ca-DataCell" HeadingCellCssClass="ca-HeadingCell"
                    HeadingCellHoverCssClass="ca-HeadingCellHover" HeadingCellActiveCssClass="ca-HeadingCellActive"
                    HeadingRowCssClass="ca-HeadingRow" HeadingTextCssClass="ca-HeadingCellText" SelectedRowCssClass="ca-SelectedRow"
                    GroupHeadingCssClass="ca-GroupHeading" SortAscendingImageUrl="grid/asc.gif" SortDescendingImageUrl="grid/desc.gif"
                    SortImageWidth="10" SortImageHeight="19" EditCommandClientTemplateId="EditCommandTemplate" InsertCommandClientTemplateId="InsertCommandTemplate"
                    DataKeyField="ShipmentId">
                    <Columns>
                    </Columns>
                </ComponentArt:GridLevel>
            </Levels>
            <ClientTemplates>
                <ComponentArt:ClientTemplate Id="EditTemplate">
                    <a href="javascript:Shipment_Edit(ShipmentList.getItemFromClientId('## DataItem.ClientId ##').getMember('ShipmentId').get_text());">
                        <img alt="edit" title="edit" src="../../../Apps/Shell/Styles/images/edit.gif" /></a> | <a href="javascript:Shipment_Delete(ShipmentList.getItemFromClientId('## DataItem.ClientId ##'));">
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
<uc1:DialogControl id="ShipmentViewDialog" AppId="Order" Width="700" ViewId="View-Shipment" runat="server">
</uc1:DialogControl>