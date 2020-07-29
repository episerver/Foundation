using EPiServer.Commerce.Order;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Managers;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.MyAccount.OrderDetails
{
    public class OrderDetailsItemViewModel
    {
        public PurchaseOrder PurchaseOrder { get; set; }
        public ILineItem LineItem { get; set; }
        public IShipment Shipment { get; set; }

        /// <summary>
        /// Get return form status
        /// </summary>
        public string ReturnFormStatus => ReturnForms.Any() && !CanReturnOrder() ? ReturnFormStatusManager.GetReturnFormStatus(ReturnForms.Last()).ToString() : "";

        /// <summary>
        /// Get all return forms
        /// </summary>
        public IEnumerable<OrderForm> ReturnForms => PurchaseOrder.ReturnOrderForms.Where(x =>
                                                                       x.Shipments.Any(y => (
                                                                            y.ShipmentTrackingNumber.Equals(Shipment.ShipmentId.ToString()) &&
                                                                            y.LineItems.Where(l => (l as IReturnLineItem).OriginalLineItemId == LineItem.LineItemId).Any())
                                                                            ));

        /// <summary>
        /// Check this line item can returned
        /// </summary>
        public bool CanReturnOrder() => TotalCanReturn() > 0;

        /// <summary>
        /// Get total returned line items
        /// </summary>
        public decimal GetTotalReturnedQuantity()
        {
            decimal total = 0;
            var returnForms = ReturnForms.GetEnumerator();
            while (returnForms.MoveNext())
            {
                var returnForm = returnForms.Current;
                var formStatus = ReturnFormStatusManager.GetReturnFormStatus(returnForm);
                if (!formStatus.Equals(Mediachase.Commerce.Orders.ReturnFormStatus.Canceled))
                {
                    total += returnForm.LineItems.Where(x => x.OrigLineItemId == LineItem.LineItemId).Sum(x => x.Quantity);
                }
            }

            return total;
        }

        /// <summary>
        /// Total line item can return
        /// </summary>
        public decimal TotalCanReturn() => LineItem.Quantity - GetTotalReturnedQuantity();
    }
}