using EPiServer;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Security;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Customer.ViewModels;
using Foundation.Commerce.Models.Pages;
using Foundation.Commerce.Order.ViewModels;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Security;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.MyAccount.SubscriptionDetail
{
    public class SubscriptionDetailController : PageController<SubscriptionDetailPage>
    {

        private readonly AddressBookService _addressBookService;
        private readonly IContentLoader _contentLoader;

        public SubscriptionDetailController(AddressBookService addressBookService,
            IContentLoader contentLoader)
        {
            _addressBookService = addressBookService;
            _contentLoader = contentLoader;
        }

        public ActionResult Index(SubscriptionDetailPage currentPage, int paymentPlanId = 0)
        {
            var paymentDetail = OrderContext.Current.Get<PaymentPlan>(paymentPlanId);

            var viewModel = new SubscriptionDetailViewModel(currentPage)
            {
                CurrentContent = currentPage,
                PaymentPlan = paymentDetail
            };

            //Get order that created by 
            var purchaseOrders = OrderContext.Current.LoadByCustomerId<PurchaseOrder>(PrincipalInfo.CurrentPrincipal.GetContactId())
                                 .OrderByDescending(x => x.Created)
                                 .Where(x => x.ParentOrderGroupId.Equals(paymentPlanId))
                                 .ToList();

            var orders = new OrderHistoryViewModel
            {
                Orders = new List<OrderViewModel>()
            };

            foreach (var purchaseOrder in purchaseOrders)
            {

                // Assume there is only one form per purchase.
                var form = purchaseOrder.GetFirstForm();
                var billingAddress = new AddressModel();
                var payment = form.Payments.FirstOrDefault();
                if (payment != null)
                {
                    billingAddress = _addressBookService.ConvertToModel(payment.BillingAddress);
                }
                var orderViewModel = new OrderViewModel
                {
                    PurchaseOrder = purchaseOrder,
                    Items = form.GetAllLineItems().Select(lineItem => new OrderHistoryItemViewModel
                    {
                        LineItem = lineItem,
                    }).GroupBy(x => x.LineItem.Code).Select(group => group.First()),
                    BillingAddress = billingAddress,
                    ShippingAddresses = new List<AddressModel>()
                };

                foreach (var orderAddress in purchaseOrder.OrderForms.Cast<IOrderForm>().SelectMany(x => x.Shipments).Select(s => s.ShippingAddress))
                {
                    var shippingAddress = _addressBookService.ConvertToModel(orderAddress);
                    orderViewModel.ShippingAddresses.Add(shippingAddress);
                }

                orders.Orders.Add(orderViewModel);
            }
            orders.OrderDetailsPageUrl =
             UrlResolver.Current.GetUrl(_contentLoader.Get<CommerceHomePage>(ContentReference.StartPage).OrderDetailsPage);

            viewModel.Orders = orders;

            return View(viewModel);
        }


    }
}