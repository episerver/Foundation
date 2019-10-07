<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Core.Controls.FileListControl" Codebehind="FileListControl.ascx.cs" %>
<script type="text/javascript">
    function FilesListDefaultGrid_onCallbackError(sender, eventArgs)
    {
      alert('Error during callback. Details: ' + eventArgs.get_errorMessage());
    }

</script>
<table cellpadding="0">
    <tr class="h100">
        <td id="tdLV" style="vertical-align: top">
            <asp:HiddenField ID="hfSelectedItems" runat="server" />
            <ComponentArt:Grid SkinID="Default" runat="server" ID="DefaultGrid" Width="800" 
                AutoPostBackOnInsert="true" AutoPostBackOnDelete="true" AutoCallBackOnUpdate="true" RunningMode="Callback"
                ImagesBaseUrl="~/Apps/Shell/styles/images/" CssClass="ca-Grid" SearchTextCssClass="ca-GridHeaderText" FooterTextCssClass="ca-GridFooterText"
                HeaderCssClass="ca-GridHeader" FooterCssClass="ca-GridFooter" GroupByCssClass="ca-GroupByCell"
                GroupByTextCssClass="ca-GroupByText" PageSize="20" PagerStyle="Numbered" PagerTextCssClass="ca-GridFooterText">
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
                    <ComponentArt:ClientTemplate ID="LoadingFeedbackTemplate">
                        <table cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td style="font-size: 10px;">
                                    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:SharedStrings, Loading %>"/>...&nbsp;</td>
                                <td>
                                <img src="../../../Apps/Shell/Styles/images/grid/spinner.gif" width="16" height="16" border="0"></td>
                            </tr>
                        </table>
                    </ComponentArt:ClientTemplate>
                </ClientTemplates>
                <ClientEvents>
                    <CallbackError EventHandler="FilesListDefaultGrid_onCallbackError" />
                    <ItemSelect EventHandler="FilesListDefaultGrid_onItemSelect" />
                </ClientEvents>
            </ComponentArt:Grid>
        </td>
    </tr>
</table>