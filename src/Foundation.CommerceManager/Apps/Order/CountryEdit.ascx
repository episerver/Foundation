<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CountryEdit.ascx.cs" Inherits="Mediachase.Commerce.Manager.Order.CountryEdit" %>
<%@ Register Src="~/Apps/Core/SaveControl.ascx" TagName="SaveControl" TagPrefix="ecf" %>
<%@ Register Src="~/Apps/Core/Controls/EditViewControl.ascx" TagName="EditViewControl" TagPrefix="ecf" %>
<div class="editDiv">
<ecf:EditViewControl AppId="Order" ViewId="Country-Edit" id="ViewControl" runat="server" MDContext="<%# Mediachase.Commerce.Orders.OrderContext.MetaDataContext %>"></ecf:EditViewControl>
<ecf:SaveControl id="EditSaveControl" ShowDeleteButton="false" runat="server" SavedClientScript="CSOrderClient.CountrySaveRedirect();" CancelClientScript="CSOrderClient.CountrySaveRedirect();"></ecf:SaveControl>
</div>