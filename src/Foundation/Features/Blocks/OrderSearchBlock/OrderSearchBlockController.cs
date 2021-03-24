using EPiServer;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Security;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Cms;
using Foundation.Cms.Extensions;
using Foundation.Cms.Settings;
using Foundation.Commerce.Customer.Services;
using Foundation.Features.Checkout.ViewModels;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.MyAccount.OrderHistory;
using Foundation.Features.Settings;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Blocks.OrderSearchBlock
{
    [Authorize]
    [TemplateDescriptor(Default = true)]
    public class OrderSearchBlockController : BlockController<OrderSearchBlock>
    {
        private readonly IAddressBookService _addressBookService;
        private readonly ICustomerService _customerService;
        private readonly IOrderGroupCalculator _orderGroupCalculator;
        private readonly IContentLoader _contentLoader;
        private readonly PaymentMethodViewModelFactory _paymentMethodViewModelFactory;
        private readonly CookieService _cookieService;
        private readonly ISettingsService _settingsService;

        private const string _KEYWORD = "OrderSearchBlock:Keyword";
        private const string _DATEFROM = "OrderSearchBlock:DateFrom";
        private const string _DATETO = "OrderSearchBlock:DateTo";
        private const string _ORDERSTATUS = "OrderSearchBlock:OrderStatus";
        private const string _PAYMENTMETHOD = "OrderSearchBlock:PaymentMethod";
        private const string _PRICEFROM = "OrderSearchBlock:PriceFrom";
        private const string _PRICETO = "OrderSearchBlock:PriceTo";

        public OrderSearchBlockController(IAddressBookService addressBookService,
            ICustomerService customerService,
            IOrderGroupCalculator orderGroupCalculator,
            IContentLoader contentLoader,
            PaymentMethodViewModelFactory paymentMethodViewModelFactory,
            CookieService cookieService,
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
        public override ActionResult Index(OrderSearchBlock currentBlock)
        {
            var referencePages = _settingsService.GetSiteSettings<ReferencePageSettings>();
            var filter = CreateFilter();
            OrderFilter.LoadDefault(filter, _paymentMethodViewModelFactory);
            var viewModel = CreateViewModel(currentBlock, filter);
            if (!referencePages?.OrderDetailsPage.IsNullOrEmpty() ?? false)
            {
                viewModel.OrderDetailUrl = UrlResolver.Current.GetUrl(referencePages.OrderDetailsPage);
            }

            return PartialView("~/Features/Blocks/Views/OrderSearchBlock.cshtml", viewModel);
        }

        public ActionResult Filter(OrderFilter filter)
        {
            var referencePages = _settingsService.GetSiteSettings<ReferencePageSettings>();
            SetCookieFilter(filter);
            var currentBlock = _contentLoader.Get<IContent>(new ContentReference(filter.CurrentBlockId)) as OrderSearchBlock;
            var viewModel = CreateViewModel(currentBlock, filter);
            if (!referencePages?.OrderDetailsPage.IsNullOrEmpty() ?? false)
            {
                viewModel.OrderDetailUrl = UrlResolver.Current.GetUrl(referencePages.OrderDetailsPage);
            }

            return PartialView("~/Features/Blocks/Views/_OrderSearchListing.cshtml", viewModel);
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

        private OrderFilter CreateFilter()
        {
            var filter = new OrderFilter
            {
                Keyword = _cookieService.Get(_KEYWORD)
            };

            var dateFromStr = _cookieService.Get(_DATEFROM);
            if (!string.IsNullOrEmpty(dateFromStr))
            {
                if (DateTime.TryParse(dateFromStr, out var dateFrom))
                {
                    filter.DateFrom = dateFrom;
                }
                else
                {
                    filter.DateFrom = null;
                }
            }

            var dateToStr = _cookieService.Get(_DATETO);
            if (!string.IsNullOrEmpty(dateToStr))
            {
                if (DateTime.TryParse(dateToStr, out var dateTo))
                {
                    filter.DateTo = dateTo;
                }
                else
                {
                    filter.DateTo = null;
                }
            }

            var priceFromStr = _cookieService.Get(_PRICEFROM);
            if (!string.IsNullOrEmpty(priceFromStr))
            {
                if (decimal.TryParse(priceFromStr, out var priceFrom))
                {
                    filter.PriceFrom = priceFrom;
                }
                else
                {
                    filter.PriceFrom = 0;
                }
            }

            var priceToStr = _cookieService.Get(_PRICETO);
            if (!string.IsNullOrEmpty(priceToStr))
            {
                if (decimal.TryParse(priceToStr, out var priceTo))
                {
                    filter.PriceTo = priceTo;
                }
                else
                {
                    filter.PriceTo = 0;
                }
            }

            var orderStatusStr = _cookieService.Get(_ORDERSTATUS);
            if (!string.IsNullOrEmpty(orderStatusStr))
            {
                if (int.TryParse(orderStatusStr, out var status))
                {
                    filter.OrderStatusId = status;
                }
                else
                {
                    filter.OrderStatusId = 0;
                }
            }

            filter.PaymentMethodId = _cookieService.Get(_PAYMENTMETHOD);

            return filter;
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
