<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Catalog.CatalogExport" Codebehind="CatalogExport.ascx.cs" %>
<%@ Register Src="../Core/Controls/EditViewControl.ascx" TagName="EditViewControl" TagPrefix="ecf" %>
<div class="editDiv">
    <ecf:EditViewControl AppId="Catalog" ViewId="Catalog-Export" id="ViewControl" runat="server" MDContext="<%# Mediachase.Commerce.Catalog.CatalogContext.MetaDataContext %>"></ecf:EditViewControl>
</div>