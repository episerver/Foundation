using EPiServer.Security;
using Foundation.Features.Checkout.ViewModels;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.MyAccount.OrderHistory;
using Foundation.Infrastructure.Cms;
using Foundation.Infrastructure.Cms.Settings;
using Foundation.Infrastructure.Commerce.Customer.Services;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Security;

namespace Foundation.Features.Blocks.OrderSearchBlock
{
    [Authorize]
    public class OrderSearchListingComponent : ViewComponent
    {
        private readonly IAddressBookService _addressBookService;
        private readonly ICustomerService _customerService;
        private readonly IOrderGroupCalculator _orderGroupCalculator;
        private readonly IContentLoader _contentLoader;
        private readonly PaymentMethodViewModelFactory _paymentMethodViewModelFactory;
        private readonly ICookieService _cookieService;
        private readonly ISettingsService _settingsService;

        private const string _KEYWORD = "OrderSearchBlock:Keyword";
        private const string _DATEFROM = "OrderSearchBlock:DateFrom";
        private const string _DATETO = "OrderSearchBlock:DateTo";
        private const string _ORDERSTATUS = "OrderSearchBlock:OrderStatus";
        private const string _PAYMENTMETHOD = "OrderSearchBlock:PaymentMethod";
        private const string _PRICEFROM = "OrderSearchBlock:PriceFrom";
        private const string _PRICETO = "OrderSearchBlock:PriceTo";

        public OrderSearchListingComponent(IAddressBookService addressBookService,
            ICustomerService customerService,
            IOrderGroupCalculator orderGroupCalculator,
            IContentLoader contentLoader,
            PaymentMethodViewModelFactory paymentMethodViewModelFactory,
            ICookieService cookieService,
            ISettingsService settingsService)
        {
            _addressBookService = addressBookService;
            _customerService = customerService;
            _orderGroupCalculator = orderGroupCalculator;
            _contentLoader = contentLoader;
            _paymentMethodViewModelFactory = paymentMethodViewModelFactory;
            _cookieService = cookieService;
            _settingsService = settingsService;
        }

        public IViewComponentResult Invoke(OrderFilter filter)
        {
            var referencePages = _settingsService.GetSiteSettings<ReferencePageSettings>();
            SetCookieFilter(filter);
            var currentBlock = _contentLoader.Get<IContent>(new ContentReference(filter.CurrentBlockId)) as OrderSearchBlock;
            var viewModel = CreateViewModel(currentBlock, filter);
            if (!referencePages?.OrderDetailsPage.IsNullOrEmpty() ?? false)
            {
                viewModel.OrderDetailUrl = UrlResolver.Current.GetUrl(referencePages.OrderDetailsPage);
            }

            return View("~/Features/Blocks/Views/_OrderSearchListing.cshtml", viewModel);
        }

        private OrderSearchBlockViewModel CreateViewModel(OrderSearchBlock currentBlock, OrderFilter filter)
        {
            var purchaseOrders = OrderContext.Current.LoadByCustomerId<PurchaseOrder>(PrincipalInfo.CurrentPrincipal.GetContactId())
                                             .OrderByDescending(x => x.Created)
                                             .ToList();

            var viewModel = new OrderSearchBlockViewModel(currentBlock)
            {
                CurrentCustomer = _customerService.GetCurrentContact(),
                Filter = filter
            };

            foreach (var purchaseOrder in purchaseOrders)
            {
                //Assume there is only one form per purchase.
                var form = purchaseOrder.GetFirstForm();
                var billingAddress = form.Payments.FirstOrDefault() != null ? form.Payments.First().BillingAddress : new OrderAddress();
                var orderViewModel = new OrderViewModel
                {
                    PurchaseOrder = purchaseOrder,
                    Items = form.GetAllLineItems().Select(lineItem => new OrderHistoryItemViewModel
                    {
                        LineItem = lineItem,
                    }).GroupBy(x => x.LineItem.Code).Select(group => group.First()),
                    BillingAddress = _addressBookService.ConvertToModel(billingAddress),
                    ShippingAddresses = new List<AddressModel>()
                };

                foreach (var orderAddress in form.Shipments.Select(s => s.ShippingAddress))
                {
                    var shippingAddress = _addressBookService.ConvertToModel(orderAddress);
                    orderViewModel.ShippingAddresses.Add(shippingAddress);
                    orderViewModel.OrderGroupId = purchaseOrder.OrderGroupId;
                }

                orderViewModel.OrderTotal = _orderGroupCalculator.GetTotal(purchaseOrder);
                orderViewModel.OrderPayments = form.Payments.ToList();

                if (FilterOrder(filter, orderViewModel))
                {
                    viewModel.Orders.Add(orderViewModel);
                }
            }

            return viewModel;
        }

        private bool FilterOrder(OrderFilter filter, OrderViewModel order)
        {
            var result = true;
            if (result && !string.IsNullOrEmpty(filter.Keyword))
            {
                result = result ? order.OrderGroupId.ToString().Contains(filter.Keyword) : result;
                result = !result ? order.Items.Where(x => x.LineItem.Code.Contains(filter.Keyword)).Any() : true;
            }

            if (result && filter.DateFrom.HasValue)
            {
                result = order.PurchaseOrder.Created.Date >= filter.DateFrom.Value.Date;
            }

            if (result && filter.DateTo.HasValue)
            {
                result = order.PurchaseOrder.Created.Date <= filter.DateTo.Value.Date;
            }

            if (result && !string.IsNullOrEmpty(filter.PaymentMethodId))
            {
                result = order.OrderPayments.Where(x => x.PaymentMethodId.ToString() == filter.PaymentMethodId).Any();
            }

            if (result && !(filter.OrderStatusId == 0))
            {
                result = order.PurchaseOrder.OrderStatus.Id == filter.OrderStatusId;
            }

            if (result && filter.PriceFrom > 0)
            {
                result = order.OrderTotal >= filter.PriceFrom;
            }

            if (result && filter.PriceTo > 0)
            {
                result = order.OrderTotal <= filter.PriceTo;
            }

            return result;
        }

        private void SetCookieFilter(OrderFilter filter)
        {
            _cookieService.Set(_KEYWORD, filter.Keyword);
            _cookieService.Set(_DATEFROM, filter.DateFrom.ToString());
            _cookieService.Set(_DATETO, filter.DateTo.ToString());
            _cookieService.Set(_ORDERSTATUS, filter.OrderStatusId.ToString());
            _cookieService.Set(_PAYMENTMETHOD, filter.PaymentMethodId);
            _cookieService.Set(_PRICEFROM, filter.PriceFrom.ToString());
            _cookieService.Set(_PRICETO, filter.PriceTo.ToString());
        }
    }
}
