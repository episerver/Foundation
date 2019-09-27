<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Catalog.Tabs.CatalogExportTab" Codebehind="CatalogExportTab.ascx.cs" %>
<%@ Register Src="~/Apps/Core/Controls/FileListControl.ascx" TagName="FileList" TagPrefix="core" %>
<%@ Register src="~/Apps/Core/Controls/ProgressControl.ascx" tagname="ProgressControl" tagprefix="core" %>
<div class="w100">
<div style="padding-left:10px; padding-right:5px; padding-top:5px; padding-bottom:5px;">
    <br />
    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_About_To_Export%>" />&nbsp;<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:CatalogStrings, Catalog_Click_Button_To_Start %>" /><br /><br />
    <!-- START: Export Button -->
    <asp:UpdatePanel ID="MainPanel" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button runat="server" ID="btnExport" Text="<%$ Resources:SharedStrings, Start_Export %>" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- END: Export Button -->
</div>
<br />
<br />
<div style="padding-left:5px; padding-right:5px; padding-top:5px; padding-bottom:5px;">
    <asp:Label runat="server" ID="lblFilesListHeader" Font-Bold="true" Text="<%$ Resources:CatalogStrings, Catalog_Exported_Files %>"></asp:Label>:
    <br />
    <asp:UpdatePanel ID="FilesPanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <table>
                <tr>
                    <td align="left">
                        <core:FileList ID="FilesControl" runat="server" GridAppId="Catalog" GridViewId="CatalogFilesList-Export" />
                    </td>
               </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <core:ProgressControl Title="<%$ Resources:CatalogStrings, Catalog_Exporting_Catalog %>" ID="ProgressControl1" runat="server" />
</div>
</div>