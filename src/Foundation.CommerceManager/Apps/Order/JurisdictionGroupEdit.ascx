<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JurisdictionGroupEdit.ascx.cs" Inherits="Mediachase.Commerce.Manager.Order.JurisdictionGroupEdit" %>
<%@ Register Src="~/Apps/Core/SaveControl.ascx" TagName="SaveControl" TagPrefix="ecf" %>
<%@ Register Src="~/Apps/Core/Controls/EditViewControl.ascx" TagName="EditViewControl" TagPrefix="ecf" %>
<div class="editDiv">
<ecf:EditViewControl AppId="Order" ViewId="JurisdictionGroup-Edit" id="ViewControl" runat="server" MDContext="<%# Mediachase.Commerce.Orders.OrderContext.MetaDataContext %>"></ecf:EditViewControl>
<ecf:SaveControl id="EditSaveControl" ShowDeleteButton="false" CancelClientScript="CSOrderClient.JurisdictionGroupSaveRedirect();" SavedClientScript="CSOrderClient.JurisdictionGroupSaveRedirect();" runat="server"></ecf:SaveControl>
</div>