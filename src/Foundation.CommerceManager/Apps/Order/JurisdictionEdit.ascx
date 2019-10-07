<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JurisdictionEdit.ascx.cs" Inherits="Mediachase.Commerce.Manager.Order.JurisdictionEdit" %>
<%@ Register Src="~/Apps/Core/SaveControl.ascx" TagName="SaveControl" TagPrefix="ecf" %>
<%@ Register Src="~/Apps/Core/Controls/EditViewControl.ascx" TagName="EditViewControl" TagPrefix="ecf" %>
<div class="editDiv">
<ecf:EditViewControl AppId="Order" ViewId="Jurisdiction-Edit" id="ViewControl" runat="server" MDContext="<%# Mediachase.Commerce.Orders.OrderContext.MetaDataContext %>"></ecf:EditViewControl>
<ecf:SaveControl id="EditSaveControl" ShowDeleteButton="false" CancelClientScript="CSOrderClient.JurisdictionSaveRedirect();" SavedClientScript="CSOrderClient.JurisdictionSaveRedirect();" runat="server"></ecf:SaveControl>
</div>