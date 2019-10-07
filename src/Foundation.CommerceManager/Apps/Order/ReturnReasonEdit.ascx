<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReturnReasonEdit.ascx.cs" Inherits="Mediachase.Commerce.Manager.Order.ReturnReasonEdit" %>
<%@ Register Src="~/Apps/Core/SaveControl.ascx" TagName="SaveControl" TagPrefix="ecf" %>
<%@ Register Src="~/Apps/Core/Controls/EditViewControl.ascx" TagName="EditViewControl" TagPrefix="ecf" %>
<div class="editDiv">
<ecf:EditViewControl AppId="Order" ViewId="ReturnReason-Edit" id="ViewControl" runat="server" MDContext="<%# Mediachase.Commerce.Orders.OrderContext.MetaDataContext %>"></ecf:EditViewControl>
<ecf:SaveControl id="EditSaveControl" ShowDeleteButton="false" runat="server" SavedClientScript="CSOrderClient.ReturnReasonSaveRedirect();" CancelClientScript="CSOrderClient.ReturnReasonSaveRedirect();"></ecf:SaveControl>
</div>