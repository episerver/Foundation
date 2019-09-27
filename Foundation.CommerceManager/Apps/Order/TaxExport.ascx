<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Order.TaxExport" Codebehind="TaxExport.ascx.cs" %>
<%@ Register Src="../Core/Controls/EditViewControl.ascx" TagName="EditViewControl" TagPrefix="ecf" %>
<div class="editDiv">
    <ecf:EditViewControl AppId="Order" ViewId="Tax-Export" id="ViewControl" runat="server" MDContext="<%# Mediachase.Commerce.Catalog.CatalogContext.MetaDataContext %>"></ecf:EditViewControl>
</div>