<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaxImport.ascx.cs" Inherits="Mediachase.Commerce.Manager.Order.TaxImport" %>
<%@ Register Src="../Core/Controls/EditViewControl.ascx" TagName="EditViewControl" TagPrefix="ecf" %>

<div class="editDiv">
    <ecf:EditViewControl AppId="Order" ViewId="Tax-Import" id="ViewControl" runat="server" MDContext="<%# Mediachase.Commerce.Orders.OrderContext.MetaDataContext %>"></ecf:EditViewControl>
</div>