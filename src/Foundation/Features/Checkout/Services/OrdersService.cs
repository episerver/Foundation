using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Order;
using EPiServer.Logging;
using EPiServer.Security;
using Foundation.Commerce;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Extensions;
using Foundation.Features.MyOrganization;
using Foundation.Features.MyOrganization.Orders;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Managers;
using Mediachase.Commerce.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.Checkout.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly CustomerContext _customerContext;
        private readonly ICustomerService _customerService;
        private readonly IOrderGroupFactory _orderGroupFactory;
        private readonly IOrderRepository _orderRepository;
        private readonly IPlacedPriceProcessor _placedPriceProcessor;
        private readonly IPromotionEngine _promotionEngine;
        private readonly IPurchaseOrderFactory _purchaseOrderFactory;

        public OrdersService(IOrderRepository orderRepository,
            ICustomerService customerService,
            IPurchaseOrderFactory purchaseOrderFactory,
            CustomerContext customerContext,
            IPlacedPriceProcessor placedPriceProcessor,
            IPromotionEngine promotionEngine,
            IOrderGroupFactory orderGroupFactory)
        {
            _orderRepository = orderRepository;
            _customerService = customerService;
            _purchaseOrderFactory = purchaseOrderFactory;
            _customerContext = customerContext;
            _placedPriceProcessor = placedPriceProcessor;
            _promotionEngine = promotionEngine;
            _orderGroupFactory = orderGroupFactory;
        }

        public List<OrderOrganizationViewModel> GetUserOrders(Guid userGuid)
        {
            var purchaseOrders = OrderContext.Current.LoadByCustomerId<PurchaseOrder>(userGuid)
                .OrderByDescending(x => x.Created)
                .ToList();
            var ordersOrganization = new List<OrderOrganizationViewModel>();

            foreach (var purchaseOrder in purchaseOrders)
            {
                var orderViewModel = new OrderOrganizationViewModel
                {
                    OrderNumber = purchaseOrder.TrackingNumber,
                    OrderGroupId = purchaseOrder.OrderGroupId,
                    PlacedOrderDate = purchaseOrder.Created.ToString("yyyy MMMM dd"),
                    Ammount = purchaseOrder.GetTotal().Amount.ToString("N"),
                    Currency = purchaseOrder.BillingCurrency,
                    User = "",
                    Status = purchaseOrder.Status,
                    SubOrganization = "",
                    IsPaymentApproved = false
                };
                if (purchaseOrder[Constant.Customer.CurrentCustomerOrganization] != null)
                {
                    orderViewModel.SubOrganization =
                        purchaseOrder[Constant.Customer.CurrentCustomerOrganization].ToString();
                }

                if (purchaseOrder[Constant.Customer.CustomerFullName] != null)
                {
                    orderViewModel.User =
                        purchaseOrder[Constant.Customer.CustomerFullName].ToString();
                }

                if (!string.IsNullOrEmpty(purchaseOrder[Constant.Quote.QuoteStatus]?.ToString()) &&
                    (purchaseOrder.Status == OrderStatus.InProgress.ToString() ||
                     purchaseOrder.Status == OrderStatus.OnHold.ToString()))
                {
                    orderViewModel.Status = purchaseOrder[Constant.Quote.QuoteStatus].ToString();
                    _ = DateTime.TryParse(purchaseOrder[Constant.Quote.QuoteExpireDate].ToString(), out var quoteExpireDate);
                    if (DateTime.Compare(DateTime.Now, quoteExpireDate) > 0)
                    {
                        orderViewModel.Status = Constant.Quote.QuoteExpired;
                    }

                    orderViewModel.IsQuoteOrder = true;
                }

                var budgetPayment = GetOrderBudgetPayment(purchaseOrder);
                orderViewModel.IsOrganizationOrder = budgetPayment != null || orderViewModel.IsQuoteOrder;
                if (budgetPayment != null)
                {
                    orderViewModel.IsPaymentApproved = orderViewModel.IsOrganizationOrder &&
                                                       budgetPayment.TransactionType.Equals(TransactionType.Capture
                                                           .ToString());
                    orderViewModel.Status = orderViewModel.IsPaymentApproved
                        ? orderViewModel.Status
                        : Constant.Order.PendingApproval;
                }

                ordersOrganization.Add(orderViewModel);
            }

            return ordersOrganization.Where(order => order.IsOrganizationOrder).ToList();
        }

        public IPayment GetOrderBudgetPayment(IPurchaseOrder purchaseOrder)
        {
            if (purchaseOrder?.Forms == null || !purchaseOrder.Forms.Any())
            {
                return null;
            }

            return
                purchaseOrder.Forms.Where(orderForm => orderForm.Payments != null && orderForm.Payments.Any())
                    .SelectMany(orderForm => orderForm.Payments)
                    .FirstOrDefault(payment => payment.PaymentMethodName.Equals(Constant.Order.BudgetPayment));
        }

        public bool ApproveOrder(int orderGroupId)
        {
            var purchaseOrder = _orderRepository.Load<PurchaseOrder>(orderGroupId);
            if (purchaseOrder == null)
            {
                return false;
            }

            var budgetPayment = GetOrderBudgetPayment(purchaseOrder) as Payment;
            if (budgetPayment == null)
            {
                return false;
            }

            try
            {
                budgetPayment.TransactionType = TransactionType.Capture.ToString();
                budgetPayment.Status = PaymentStatus.Pending.ToString();
                budgetPayment.AcceptChanges();
                purchaseOrder.ProcessPayments();
                budgetPayment.Status = PaymentStatus.Processed.ToString();
                budgetPayment.AcceptChanges();
                _orderRepository.Save(purchaseOrder);
            }
            catch (Exception ex)
            {
                budgetPayment.TransactionType = TransactionType.Authorization.ToString();
                budgetPayment.Status = PaymentStatus.Processed.ToString();
                budgetPayment.AcceptChanges();
                _orderRepository.Save(purchaseOrder);
                LogManager.GetLogger(GetType()).Error("Failed processs on approve order.", ex);
                return false;
            }

            return true;
        }

        public ContactViewModel GetPurchaserCustomer(IOrderGroup order)
        {
            if (order == null)
            {
                return null;
            }

            var isQuoteOrder = order.Properties[Constant.Quote.ParentOrderGroupId] != null &&
                               Convert.ToInt32(order.Properties[Constant.Quote.ParentOrderGroupId]) != 0;
            if (!isQuoteOrder)
            {
                return new ContactViewModel(_customerService.GetContactById(order.CustomerId.ToString()));
            }

            var parentOrder =
                _orderRepository.Load<PurchaseOrder>(Convert.ToInt32(order.Properties[Constant.Quote.ParentOrderGroupId]));
            return parentOrder != null
                ? new ContactViewModel(_customerService.GetContactById(parentOrder.CustomerId.ToString()))
                : null;
        }

        /// <summary>
        ///     Create a return order
        /// </summary>
        /// <param name="orderGroupId"></param>
        /// <param name="shipmentId"></param>
        /// <param name="lineItemId"></param>
        /// <param name="returnQuantity"></param>
        /// <param name="reason"></param>
        public ReturnFormStatus CreateReturn(int orderGroupId, int shipmentId, int lineItemId, decimal returnQuantity, string reason)
        {
            //Get originial information about lineitem and shipment
            var purchaseOrder = _orderRepository.Load<IPurchaseOrder>(orderGroupId);
            var form = purchaseOrder.GetFirstForm();
            var shipment = form.Shipments.FirstOrDefault(s => s.ShipmentId == shipmentId);
            var lineItem = shipment.LineItems.First(l => l.LineItemId == lineItemId);

            //Create return order based on original line item and shipment
            var returnForm = _purchaseOrderFactory.CreateReturnOrderForm(purchaseOrder);
            var returnShipment = _purchaseOrderFactory.CreateReturnShipment(shipment);
            var returnLineItem = _purchaseOrderFactory.CreateReturnLineItem(lineItem, returnQuantity,
                string.IsNullOrEmpty(reason) ? "Faulty" : reason);

            purchaseOrder.ReturnForms.Add(returnForm);
            returnForm.Shipments.Add(returnShipment);
            returnShipment.LineItems.Add(returnLineItem);

            //Update quantiy and extended price of return lineitem
            returnLineItem.ReturnQuantity = returnQuantity;
            var orglineItem = (form as OrderForm)?.LineItems?.FindItem(lineItemId);
            var extendedPrice = orglineItem != null ? orglineItem.ExtendedPrice / orglineItem.Quantity : 0m;
            (returnLineItem as LineItem).ExtendedPrice = returnLineItem.ReturnQuantity * extendedPrice;

            //Save return form
            _orderRepository.Save(purchaseOrder);

            //Return status of return order
            return ReturnFormStatusManager.GetReturnFormStatus(returnForm as OrderForm);
        }

        public Dictionary<ILineItem, List<ValidationIssue>> ChangeLineItemQuantity(int orderGroupId, int shipmentId, int lineItemId, decimal quantity)
        {
            var purchaseOrder = _orderRepository.Load<IPurchaseOrder>(orderGroupId);
            var form = purchaseOrder.GetFirstForm();
            var shipment = form.Shipments.FirstOrDefault(s => s.ShipmentId == shipmentId);
            var lineItem = shipment?.LineItems.First(l => l.LineItemId == lineItemId);
            if (lineItem == null)
            {
                return new Dictionary<ILineItem, List<ValidationIssue>>();
            }

            lineItem.Quantity = quantity;
            var issues = ValidatePurchaseOrder(purchaseOrder);
            if (!issues.Any() || !issues.Where(x => x.Key.LineItemId == lineItemId)
                    .Any(y => y.Value.Any(z => z != ValidationIssue.None)))
            {
                var contact = PrincipalInfo.CurrentPrincipal.GetCustomerContact();
                var detail = (contact != null
                                 ? $"{contact.FullName} changed the quantity to "
                                 : "Quantity was changed to ") +
                             quantity.ToString("f0");
                AddNote(purchaseOrder, "Customer quantity change", detail);
                _orderRepository.Save(purchaseOrder);
            }

            return issues;
        }

        public Dictionary<ILineItem, List<ValidationIssue>> ChangeLineItemPrice(int orderGroupId, int shipmentId, int lineItemId, decimal price)
        {
            var purchaseOrder = _orderRepository.Load<IPurchaseOrder>(orderGroupId);
            var form = purchaseOrder.GetFirstForm();
            var shipment = form.Shipments.FirstOrDefault(s => s.ShipmentId == shipmentId);
            var lineItem = shipment?.LineItems.First(l => l.LineItemId == lineItemId);
            if (lineItem == null)
            {
                return new Dictionary<ILineItem, List<ValidationIssue>>();
            }

            lineItem.PlacedPrice = price;
            var issues = ValidatePurchaseOrder(purchaseOrder);
            if (!issues.Any() || !issues.Where(x => x.Key.LineItemId == lineItemId)
                    .Any(y => y.Value.Any(z => z != ValidationIssue.None)))
            {
                var contact = PrincipalInfo.CurrentPrincipal.GetCustomerContact();
                var detail = (contact != null
                                 ? $"{contact.FullName} changed the price to "
                                 : "Price was changed to ") +
                             price.ToString("c");
                AddNote(purchaseOrder, "Customer price change", detail);
                _orderRepository.Save(purchaseOrder);
            }

            return issues;
        }

        public IOrderNote AddNote(IPurchaseOrder order, string title, string detail)
        {
            var contact = PrincipalInfo.CurrentPrincipal.GetCustomerContact();
            var notes = order.Notes;

            var note = _orderGroupFactory.CreateOrderNote(order);
            note.CustomerId = contact?.PrimaryKeyId ?? PrincipalInfo.CurrentPrincipal.GetContactId();
            note.Type = OrderNoteTypes.Custom.ToString();
            note.Title = title;
            note.Detail = detail;
            note.Created = DateTime.UtcNow;
            notes.Add(note);

            return note;
        }

        private Dictionary<ILineItem, List<ValidationIssue>> ValidatePurchaseOrder(IPurchaseOrder purchaseOrder)
        {
            var validationIssues = new Dictionary<ILineItem, List<ValidationIssue>>();
            purchaseOrder.UpdatePlacedPriceOrRemoveLineItems(_customerContext.GetContactById(purchaseOrder.CustomerId),
                (item, issue) => validationIssues.AddValidationIssues(item, issue), _placedPriceProcessor);
            purchaseOrder.UpdateInventoryOrRemoveLineItems((item, issue) =>
                validationIssues.AddValidationIssues(item, issue));
            purchaseOrder.AdjustInventoryOrRemoveLineItems((item, issue) =>
                validationIssues.AddValidationIssues(item, issue));
            purchaseOrder.ApplyDiscounts(_promotionEngine, new PromotionEngineSettings());
            return validationIssues;
        }
    }
}