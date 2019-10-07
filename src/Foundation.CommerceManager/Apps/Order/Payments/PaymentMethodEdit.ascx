<%@ Control Language="c#" Inherits="Mediachase.Commerce.Manager.Order.Payments.PaymentMethodEdit" Codebehind="PaymentMethodEdit.ascx.cs" %>
<%@ Register Src="~/Apps/Core/SaveControl.ascx" TagName="SaveControl" TagPrefix="ecf" %>
<%@ Register Src="~/Apps/Core/Controls/EditViewControl.ascx" TagName="EditViewControl" TagPrefix="ecf" %>
<div class="editDiv">
    <%--<asp:ValidationSummary id="ValidationSummary1" runat="server" HeaderText="<%$ Resources:SharedStrings, Validation_Fix_The_Following_Problems %>"></asp:ValidationSummary>--%>
    <ecf:EditViewControl AppId="Order" ViewId="PaymentMethod-Edit" id="ViewControl" runat="server" MDContext="<%# Mediachase.Commerce.Orders.OrderContext.MetaDataContext %>"></ecf:EditViewControl>
    <ecf:SaveControl id="EditSaveControl" ShowDeleteButton="false" runat="server"></ecf:SaveControl>
</div>