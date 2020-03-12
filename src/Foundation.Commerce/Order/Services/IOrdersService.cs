using EPiServer.Commerce.Order;
using Foundation.Commerce.Customer.ViewModels;
using Foundation.Commerce.Order.ViewModels;
using Mediachase.Commerce.Orders;
using System;
using System.Collections.Generic;

namespace Foundation.Commerce.Order.Services
{
    public interface IOrdersService
    {
        List<OrderOrganizationViewModel> GetUserOrders(Guid userGuid);
        IPayment GetOrderBudgetPayment(IPurchaseOrder purchaseOrder);
        bool ApproveOrder(int orderGroupId);
        ContactViewModel GetPurchaserCustomer(IOrderGroup order);

        ReturnFormStatus CreateReturn(int orderGroupId, int shipmentId, int lineItemId, decimal returnQuantity, string reason);

        Dictionary<ILineItem, List<ValidationIssue>> ChangeLineItemPrice(int orderGroupId, int shipmentId,
            int lineItemId, decimal price);

        Dictionary<ILineItem, List<ValidationIssue>> ChangeLineItemQuantity(int orderGroupId, int shipmentId, int lineItemId, decimal quantity);

        IOrderNote AddNote(IPurchaseOrder order, string title, string detail);
    }
}