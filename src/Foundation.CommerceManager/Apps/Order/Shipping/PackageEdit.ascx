<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PackageEdit.ascx.cs" Inherits="Mediachase.Commerce.Manager.Order.Shipping.PackageEdit" %>
<%@ Register Src="~/Apps/Core/SaveControl.ascx" TagName="SaveControl" TagPrefix="ecf" %>
<%@ Register Src="~/Apps/Core/Controls/EditViewControl.ascx" TagName="EditViewControl" TagPrefix="ecf" %>
<div class="editDiv">
<ecf:EditViewControl AppId="Order" ViewId="OrderPackage-Edit" id="ViewControl" runat="server" MDContext="<%# Mediachase.Commerce.Orders.OrderContext.MetaDataContext %>"></ecf:EditViewControl>
<ecf:SaveControl id="EditSaveControl" ShowDeleteButton="false" CancelClientScript="CSManagementClient.ChangeView('Order','Packages-List');" SavedClientScript="CSManagementClient.ChangeView('Order','Packages-List');" runat="server"></ecf:SaveControl>
</div>