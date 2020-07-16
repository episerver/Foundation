using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Editor;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using Foundation.Commerce.Customer.Services;
using Foundation.Features.Checkout.Services;
using Foundation.Features.Checkout.ViewModels;
using Foundation.Features.MyAccount.AddressBook;
using Mediachase.Commerce.Orders;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Checkout.ConfirmationMail
{
    public class OrderConfirmationMailController : PageController<OrderConfirmationMailPage>
    {
        private readonly ConfirmationService _confirmationService;
        private readonly AddressBookService _addressBookService;
        private readonly CustomerService _customerService;
        private readonly IOrderGroupCalculator _orderGroupCalculator;

        public OrderConfirmationMailController(ConfirmationService confirmationService,
            AddressBookService addressBookService,
            CustomerService customerService,
            IOrderGroupCalculator orderGroupCalculator)
        {
            _confirmationService = confirmationService;
            _addressBookService = addressBookService;
            _customerService = customerService;
            _orderGroupCalculator = orderGroupCalculator;
        }

        public ActionResult Index(OrderConfirmationMailPage currentPage, int? orderNumber)
        {
            IPurchaseOrder order;
            if (PageEditing.PageIsInEditMode)
            {
                order = _confirmationService.CreateFakePurchaseOrder();
            }
            else
            {
                order = _confirmationService.GetOrder(orderNumber.Value);
                if (order == null)
                {
                    return Redirect(Url.ContentUrl(ContentReference.StartPage));
                }
            }

            var viewModel = CreateViewModel(currentPage, order);

            return View("~/Features/Checkout/ConfirmationMail/Index.cshtml", viewModel);
        }

        private OrderConfirmationViewModel<OrderConfirmationMailPage> CreateViewModel(OrderConfirmationMailPage currentPage, IPurchaseOrder order)
        {
            var hasOrder = order != null;

            if (!hasOrder)
            {
                return new OrderConfirmationViewModel<OrderConfirmationMailPage>(currentPage);
            }

            var lineItems = order.GetFirstForm().Shipments.SelectMany(x => x.LineItems);
            var totals = _orderGroupCalculator.GetOrderGroupTotals(order);

            var viewModel = new OrderConfirmationViewModel<OrderConfirmationMailPage>(currentPage)
            {
                Currency = order.Currency,
                CurrentContent = currentPage,
                HasOrder = hasOrder,
                OrderId = order.OrderNumber,
                Created = order.Created,
                Items = lineItems,
                BillingAddress = new AddressModel(),
                ShippingAddresses = new List<AddressModel>(),
                ContactId = _customerService.CurrentContactId,
                Payments = order.GetFirstForm().Payments.Where(c => c.TransactionType == TransactionType.Authorization.ToString() || c.TransactionType == TransactionType.Sale.ToString()),
                OrderGroupId = order.OrderLink.OrderGroupId,
                OrderLevelDiscountTotal = order.GetOrderDiscountTotal(),
                ShippingSubTotal = order.GetShippingSubTotal(),
                ShippingDiscountTotal = order.GetShippingDiscountTotal(),
                ShippingTotal = totals.ShippingTotal,
                HandlingTotal = totals.HandlingTotal,
                TaxTotal = totals.TaxTotal,
                CartTotal = totals.Total,
                SubTotal = order.GetSubTotal()
            };

            var billingAddress = order.GetFirstForm().Payments.First().BillingAddress;

            // Map the billing address using the billing id of the order form.
            _addressBookService.MapToModel(billingAddress, viewModel.BillingAddress);

            // Map the remaining addresses as shipping addresses.
            foreach (var orderAddress in order.Forms.SelectMany(x => x.Shipments).Select(s => s.ShippingAddress))
            {
                var shippingAddress = new AddressModel();
                _addressBookService.MapToModel(orderAddress, shippingAddress);
                viewModel.ShippingAddresses.Add(shippingAddress);
            }

            return viewModel;
        }
    }
}