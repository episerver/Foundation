<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Catalog.CatalogEntryEdit" Codebehind="CatalogEntryEdit.ascx.cs" %>
<%@ Register Src="../Core/SaveControl.ascx" TagName="SaveControl" TagPrefix="ecf" %>
<%@ Register Src="../Core/Controls/EditViewControl.ascx" TagName="EditViewControl" TagPrefix="ecf" %>
<div class="editDiv">
    <asp:Label runat="server" Text="<%$ Resources:CatalogStrings, Delete_Versions_Warning %>" CssClass="notification" />
    <ecf:EditViewControl AppId="Catalog" id="ViewControl" runat="server" MDContext="<%# Mediachase.Commerce.Catalog.CatalogContext.MetaDataContext %>"></ecf:EditViewControl>
    <ecf:SaveControl id="EditSaveControl" CancelMessage="<%$ Resources:CatalogStrings, Catalog_Catalog_Entry_Changes_Has_Been_Discarded %>" SavedMessage="<%$ Resources:CatalogStrings, Catalog_Catalog_Entry_Updated_Successfully %>" CancelClientScript="CSCatalogClient.CatalogSaveRedirect();" SavedClientScript="CSCatalogClient.CatalogSaveRedirect();" runat="server"></ecf:SaveControl>
</div>