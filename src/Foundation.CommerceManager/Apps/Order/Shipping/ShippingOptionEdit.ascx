<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Order.Shipping.ShippingOptionEdit" Codebehind="ShippingOptionEdit.ascx.cs" %>
<%@ Register Src="~/Apps/Core/SaveControl.ascx" TagName="SaveControl" TagPrefix="ecf" %>
<%@ Register Src="~/Apps/Core/Controls/EditViewControl.ascx" TagName="EditViewControl" TagPrefix="ecf" %>
<div class="editDiv">
<ecf:EditViewControl AppId="Order" ViewId="ShippingOption-Edit" id="ViewControl" runat="server" MDContext="<%# Mediachase.Commerce.Orders.OrderContext.MetaDataContext %>"></ecf:EditViewControl>
<ecf:SaveControl id="EditSaveControl" ShowDeleteButton="false" CancelClientScript="CSOrderClient.ShippingOptionSaveRedirect();" SavedClientScript="CSOrderClient.ShippingOptionSaveRedirect();" runat="server"></ecf:SaveControl>
</div>