<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CurrencyEdit.ascx.cs" Inherits="Mediachase.Commerce.Manager.Catalog.CurrencyEdit" %>
<%@ Register Src="~/Apps/Core/SaveControl.ascx" TagName="SaveControl" TagPrefix="ecf" %>
<%@ Register Src="~/Apps/Core/Controls/EditViewControl.ascx" TagName="EditViewControl" TagPrefix="ecf" %>
<div class="editDiv">
<ecf:EditViewControl AppId="Catalog" ViewId="Currency-Edit" id="ViewControl" runat="server"></ecf:EditViewControl>
<ecf:SaveControl id="EditSaveControl" CancelClientScript="CSCatalogClient.CurrencySaveRedirect();" SavedClientScript="CSCatalogClient.CurrencySaveRedirect();" runat="server"></ecf:SaveControl>
</div>