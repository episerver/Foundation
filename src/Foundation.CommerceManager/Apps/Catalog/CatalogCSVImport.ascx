<%@ Control Language="C#" AutoEventWireup="true" Inherits="Mediachase.Commerce.Manager.Catalog.CatalogCSVImport" Codebehind="CatalogCSVImport.ascx.cs" %>
<%@ Register Src="../Core/Controls/EditViewControl.ascx" TagName="EditViewControl" TagPrefix="ecf" %>

<div class="editDiv">
    <ecf:EditViewControl AppId="Catalog" ViewId="Catalog-CSVImport" id="ViewControl" runat="server" MDContext="<%# Mediachase.Commerce.Catalog.CatalogContext.MetaDataContext %>"></ecf:EditViewControl>
</div>