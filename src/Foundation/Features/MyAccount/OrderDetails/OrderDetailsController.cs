using EPiServer;
using EPiServer.Commerce.Order;
using EPiServer.Logging;
using EPiServer.Web.Mvc;
using Foundation.Commerce;
using Foundation.Commerce.Customer.Services;
using Foundation.Features.Checkout.Services;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Infrastructure;
using Mediachase.Commerce.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Foundation.Features.MyAccount.OrderDetails
{
    public class OrderDetailsController : PageController<OrderDetailsPage>
    {
        private readonly IAddressBookService _addressBookService;
        private readonly IOrdersService _ordersService;
        private readonly ICustomerService _customerService;
        private readonly IOrderRepository _orderRepository;
        private readonly IContentLoader _contentLoader;
        private readonly ICartService _cartService;
        private readonly IPurchaseOrderFactory _purchaseOrderFactory;

        public OrderDetailsController(IAddressBookService addressBookService, IOrdersService ordersService, ICustomerService customerService, IOrderRepository orderRepository, IContentLoader contentLoader, ICartService cartService, IPurchaseOrderFactory purchaseOrderFactory)
        {
            _addressBookService = addressBookService;
            _ordersService = ordersService;
            _customerService = customerService;
            _orderRepository = orderRepository;
            _contentLoader = contentLoader;
            _cartService = cartService;
            _purchaseOrderFactory = purchaseOrderFactory;
        }

        [HttpGet]
        public ActionResult Index(OrderDetailsPage currentPage, int orderGroupId = 0) => View(GetModel(orderGroupId, currentPage));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveOrder(int orderGroupId = 0)
        {
            if (orderGroupId == 0)
            {
                return Json(new { result = true });
            }

            var success = _ordersService.ApproveOrder(orderGroupId);

            return success ? Json(new { Status = true, Message = "" }) : Json(new { Status = false, Message = "Failed to process your payment." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReturn(int orderGroupId, int shipmentId, int lineItemId, decimal returnQuantity, string reason)
        {
            var formStatus = _ordersService.CreateReturn(orderGroupId, shipmentId, lineItemId, returnQuantity, reason);
            return Json(new
            {
                Result = true,
                ReturnFormStatus = formStatus.ToString()
            });
        }

        [HttpPost]
        public ActionResult ChangePrice(int orderGroupId, int shipmentId, int lineItemId, decimal placedPrice, OrderDetailsPage currentPage)
        {
            var issues = _ordersService.ChangeLineItemPrice(orderGroupId, shipmentId, lineItemId, placedPrice);
            var model = GetModel(orderGroupId, currentPage);
            model.ErrorMessage = GetValidationMessages(issues);
            return PartialView("Index", model);
        }

        [HttpPost]
        public ActionResult ChangeQuantity(int orderGroupId, int shipmentId, int lineItemId, decimal quantity, OrderDetailsPage currentPage)
        {
            var issues = _ordersService.ChangeLineItemQuantity(orderGroupId, shipmentId, lineItemId, quantity);
            var model = GetModel(orderGroupId, currentPage);
            model.ErrorMessage = GetValidationMessages(issues);
            return PartialView("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNote(int orderGroupId, string note)
        {
            var order = _orderRepository.Load<IPurchaseOrder>(orderGroupId);
            var orderNote = _ordersService.AddNote(order, "Customer Manual Note", note);
            _orderRepository.Save(order);
            return Json(orderNote);
        }

        private static string GetValidationMessages(Dictionary<ILineItem, List<ValidationIssue>> validationIssues)
        {
            var messages = new List<string>();
            foreach (var validationIssue in validationIssues)
            {
                var warning = new StringBuilder();
                warning.Append($"Line Item with code {validationIssue.Key.Code} ");
                validationIssue.Value.Aggregate(warning, (current, issue) => current.Append(issue).Append(", "));
                messages.Add(warning.ToString().TrimEnd(',', ' '));
            }

            return string.Join(".", messages);
        }

        private OrderDetailsViewModel GetModel(int orderGroupId, OrderDetailsPage currentPage)
        {
            var orderViewModel = new OrderDetailsViewModel
            {
                CurrentContent = currentPage,
                CurrentCustomer = _customerService.GetCurrentContactViewModel()
            };

            var purchaseOrder = OrderContext.Current.Get<PurchaseOrder>(orderGroupId);
            if (purchaseOrder == null)
            {
                return orderViewModel;
            }

            var currentContact = _customerService.GetCurrentContact();
            var currentOrganization = currentContact.FoundationOrganization;
            if (currentOrganization != null)
            {
                var usersOrganization = _customerService.GetContactsForOrganization(currentOrganization);
                if (!usersOrganization.Where(x => x.ContactId == purchaseOrder.CustomerId).Any())
                {
                    return orderViewModel;
                }
            }
            else
            {
                if (currentContact.ContactId != purchaseOrder.CustomerId)
                {
                    return orderViewModel;
                }
            }

            // Assume there is only one form per purchase.
            var form = purchaseOrder.GetFirstForm();

            var billingAddress = form.Payments.FirstOrDefault() != null
                ? form.Payments.First().BillingAddress
                : new OrderAddress();

            orderViewModel.PurchaseOrder = purchaseOrder;

            orderViewModel.Items = form.Shipments.SelectMany(shipment => shipment.LineItems.Select(lineitem => new OrderDetailsItemViewModel
            {
                LineItem = lineitem,
                Shipment = shipment,
                PurchaseOrder = orderViewModel.PurchaseOrder as PurchaseOrder,
            }
            ));

            orderViewModel.BillingAddress = _addressBookService.ConvertToModel(billingAddress);
            orderViewModel.ShippingAddresses = new List<AddressModel>();

            foreach (var orderAddress in form.Shipments.Select(s => s.ShippingAddress))
            {
                var shippingAddress = _addressBookService.ConvertToModel(orderAddress);
                orderViewModel.ShippingAddresses.Add(shippingAddress);
                orderViewModel.OrderGroupId = purchaseOrder.OrderGroupId;
            }
            if (purchaseOrder[Constant.Quote.QuoteExpireDate] != null &&
                !string.IsNullOrEmpty(purchaseOrder[Constant.Quote.QuoteExpireDate].ToString()))
            {
                DateTime.TryParse(purchaseOrder[Constant.Quote.QuoteExpireDate].ToString(), out var quoteExpireDate);
                if (DateTime.Compare(DateTime.Now, quoteExpireDate) > 0)
                {
                    orderViewModel.QuoteStatus = Constant.Quote.QuoteExpired;
                    try
                    {
                        // Update order quote status to expired
                        purchaseOrder[Constant.Quote.QuoteStatus] = Constant.Quote.QuoteExpired;
                        _orderRepository.Save(purchaseOrder);
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(GetType()).Error("Failed to update order status to Quote Expired.", ex.StackTrace);
                    }
                }
            }

            if (!string.IsNullOrEmpty(purchaseOrder["QuoteStatus"]?.ToString()) &&
                (purchaseOrder.Status == OrderStatus.InProgress.ToString() ||
                 purchaseOrder.Status == OrderStatus.OnHold.ToString()))
            {
                orderViewModel.QuoteStatus = purchaseOrder["QuoteStatus"].ToString();
            }

            orderViewModel.BudgetPayment = _ordersService.GetOrderBudgetPayment(purchaseOrder);
            return orderViewModel;
        }
    }
}