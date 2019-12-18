using EPiServer;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Security;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Customer.ViewModels;
using Foundation.Commerce.Models.Blocks;
using Foundation.Commerce.Models.Pages;
using Foundation.Commerce.Order.ViewModelFactories;
using Foundation.Commerce.Order.ViewModels;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Blocks
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
            PaymentMethodViewModelFactory paymentMethodViewModelFactory)
        {
            _addressBookService = addressBookService;
            _customerService = customerService;
            _orderGroupCalculator = orderGroupCalculator;
            _contentLoader = contentLoader;
            _paymentMethodViewModelFactory = paymentMethodViewModelFactory;
        }
        public override ActionResult Index(OrderSearchBlock currentBlock)
        {
            var filter = CreateFilter();
            OrderFilter.LoadDefault(filter, _paymentMethodViewModelFactory);
            var viewModel = CreateViewModel(currentBlock, filter);
            viewModel.OrderDetailUrl =
                UrlResolver.Current.GetUrl(_contentLoader.Get<CommerceHomePage>(ContentReference.StartPage).OrderDetailsPage);
            return PartialView("~/Features/Blocks/Views/OrderSearchBlock.cshtml", viewModel);
        }

        public ActionResult Filter(OrderFilter filter)
        {
            SetCookieFilter(filter);
            var currentBlock = _contentLoader.Get<IContent>(new ContentReference(filter.CurrentBlockId)) as OrderSearchBlock;
            var viewModel = CreateViewModel(currentBlock, filter);
            viewModel.OrderDetailUrl =
                UrlResolver.Current.GetUrl(_contentLoader.Get<CommerceHomePage>(ContentReference.StartPage).OrderDetailsPage);
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
                result = result ? order.Items.Where(x => x.LineItem.Code.Contains(filter.Keyword)).Count() > 0 : result;
            }

            if (result && filter.DateFrom.HasValue)
            {
                result = order.PurchaseOrder.Created >= filter.DateFrom.Value;
            }

            if (result && filter.DateTo.HasValue)
            {
                result = order.PurchaseOrder.Created <= filter.DateTo.Value;
            }

            if (result && !string.IsNullOrEmpty(filter.PaymentMethodId))
            {
                result =  order.OrderPayments.Where(x => x.PaymentMethodId.ToString() == filter.PaymentMethodId).Count() > 0;
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
            var filter = new OrderFilter();
            filter.Keyword = HttpContext.Request.Cookies[_KEYWORD] != null ? HttpContext.Request.Cookies[_KEYWORD].Value : string.Empty;

            if (HttpContext.Request.Cookies[_DATEFROM] != null)
            {
                if (DateTime.TryParse(HttpContext.Request.Cookies[_DATEFROM].Value, out DateTime dateFrom))
                {
                    filter.DateFrom = dateFrom;
                }
                else
                {
                    filter.DateFrom = null;
                }
            }

            if (HttpContext.Request.Cookies[_DATETO] != null)
            {
                if (DateTime.TryParse(HttpContext.Request.Cookies[_DATETO].Value, out DateTime dateTo))
                {
                    filter.DateTo = dateTo;
                }
                else
                {
                    filter.DateTo = null;
                }
            }

            if (HttpContext.Request.Cookies[_PRICEFROM] != null)
            {
                if (decimal.TryParse(HttpContext.Request.Cookies[_PRICEFROM].Value, out decimal priceFrom))
                {
                    filter.PriceFrom = priceFrom;
                }
                else
                {
                    filter.PriceFrom = 0;
                }
            }

            if (HttpContext.Request.Cookies[_PRICETO] != null)
            {
                if (decimal.TryParse(HttpContext.Request.Cookies[_PRICETO].Value, out decimal priceTo))
                {
                    filter.PriceTo = priceTo;
                }
                else
                {
                    filter.PriceTo = 0;
                }
            }

            if (HttpContext.Request.Cookies[_ORDERSTATUS] != null)
            {
                if (int.TryParse(HttpContext.Request.Cookies[_ORDERSTATUS].Value, out int status))
                {
                    filter.OrderStatusId = status;
                }
                else
                {
                    filter.OrderStatusId = 0;
                }
            }
            filter.PaymentMethodId = HttpContext.Request.Cookies[_PAYMENTMETHOD] != null ? HttpContext.Request.Cookies[_PAYMENTMETHOD].Value : string.Empty;

            if (filter.DateFrom == DateTime.MinValue) filter.DateFrom = null;
            if (filter.DateTo == DateTime.MaxValue) filter.DateTo= null;

            return filter;
        }

        private void SetCookieFilter(OrderFilter filter)
        {
            SetCookie(_KEYWORD, filter.Keyword);
            SetCookie(_DATEFROM, filter.DateFrom.ToString());
            SetCookie(_DATETO, filter.DateTo.ToString());
            SetCookie(_ORDERSTATUS, filter.OrderStatusId.ToString());
            SetCookie(_PAYMENTMETHOD, filter.PaymentMethodId);
            SetCookie(_PRICEFROM, filter.PriceFrom.ToString());
            SetCookie(_PRICETO, filter.PriceTo.ToString());
        }

        private void SetCookie(string key, string value)
        {
            if (HttpContext.Request.Cookies[key] == null)
            {
                HttpContext.Response.Cookies.Add(new System.Web.HttpCookie(key, value) { Expires = DateTime.Now.AddDays(365) });
            }
            else
            {
                HttpContext.Response.Cookies.Set(new System.Web.HttpCookie(key, value) { Expires = DateTime.Now.AddDays(365) });
            }
        }
    }
}
