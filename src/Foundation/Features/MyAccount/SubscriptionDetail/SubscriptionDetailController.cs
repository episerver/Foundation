using EPiServer.Security;
using Foundation.Features.Checkout.ViewModels;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.MyAccount.OrderHistory;
using Foundation.Infrastructure.Cms.Settings;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Security;

namespace Foundation.Features.MyAccount.SubscriptionDetail
{
    public class SubscriptionDetailController : PageController<SubscriptionDetailPage>
    {
        private readonly ISettingsService _settingsService;
        private readonly IAddressBookService _addressBookService;
        private readonly IContentLoader _contentLoader;

        public SubscriptionDetailController(IAddressBookService addressBookService,
            IContentLoader contentLoader,
            ISettingsService settingsService)
        {
            _addressBookService = addressBookService;
            _contentLoader = contentLoader;
            _settingsService = settingsService;
        }

        public ActionResult Index(SubscriptionDetailPage currentPage, int paymentPlanId = 0)
        {
            var paymentDetail = OrderContext.Current.Get<PaymentPlan>(paymentPlanId);

            string subscriptionType = "Monthly";
            if (paymentDetail.CycleLength == 2)
            {
                subscriptionType = "2Month";
            }

            var viewModel = new SubscriptionDetailViewModel(currentPage)
            {
                CurrentContent = currentPage,
                PaymentPlan = paymentDetail,
                SubscriptionOptions = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("Monthly For A Year", "Monthly"),
                    new KeyValuePair<string, string>("Bi-Monthly For A Year", "2Month")
                },
                SelectedSubscriptionOption = subscriptionType
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
             UrlResolver.Current.GetUrl(_settingsService.GetSiteSettings<ReferencePageSettings>()?.OrderDetailsPage ?? ContentReference.StartPage);

            viewModel.Orders = orders;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeSubscriptionStatus(int orderGroupId = 0)
        {
            var paymentDetail = OrderContext.Current.Get<PaymentPlan>(orderGroupId);
            if (paymentDetail.IsActive)
            {
                paymentDetail.Status = "On Hold";
            }
            else
            {
                paymentDetail.Status = "InProgress";
            }
            paymentDetail.IsActive = (!paymentDetail.IsActive);
            paymentDetail.AcceptChanges();

            //redirect to ?paymentPlanId=@order.Id">#@order.Id
            var queryCollection = new NameValueCollection
            {
                {"paymentPlanId", orderGroupId.ToString() }
            };

            var referenceSettings = _settingsService.GetSiteSettings<ReferencePageSettings>();
            var detailPage = referenceSettings?.PaymentPlanDetailsPage ?? ContentReference.EmptyReference;

            string redirectString = new UrlBuilder(UrlResolver.Current.GetUrl(detailPage)) { QueryCollection = queryCollection }.ToString();
            return Redirect(redirectString);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeSubscriptionSetting(int orderGroupId = 0, string SubscriptionOption = "Monthly")
        {
            var paymentDetail = OrderContext.Current.Get<PaymentPlan>(orderGroupId);
            if (paymentDetail.CycleLength == 1)
            {
                paymentDetail.CycleLength = 2;
                paymentDetail.MaxCyclesCount = 6;
            }
            else
            {
                paymentDetail.CycleLength = 1;
                paymentDetail.MaxCyclesCount = 12;
            }
            paymentDetail.AcceptChanges();

            //redirect to ?paymentPlanId=@order.Id">#@order.Id
            var queryCollection = new NameValueCollection
            {
                {"paymentPlanId", orderGroupId.ToString() }
            };

            var referenceSettings = _settingsService.GetSiteSettings<ReferencePageSettings>();
            var detailPage = referenceSettings?.PaymentPlanDetailsPage ?? ContentReference.EmptyReference;

            string redirectString = new UrlBuilder(UrlResolver.Current.GetUrl(detailPage)) { QueryCollection = queryCollection }.ToString();
            return Redirect(redirectString);
        }
    }
}