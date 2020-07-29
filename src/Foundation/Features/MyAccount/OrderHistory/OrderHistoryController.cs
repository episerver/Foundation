using EPiServer;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Security;
using EPiServer.Web.Mvc.Html;
using EPiServer.Web.Routing;
using Foundation.Cms;
using Foundation.Cms.Settings;
using Foundation.Commerce.Customer.Services;
using Foundation.Features.Checkout.Services;
using Foundation.Features.Checkout.ViewModels;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.MyAccount.OrderConfirmation;
using Foundation.Features.Settings;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Managers;
using Mediachase.Commerce.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Foundation.Features.MyAccount.OrderHistory
{
    [Authorize]
    public class OrderHistoryController : OrderConfirmationControllerBase<OrderHistoryPage>
    {
        private readonly IAddressBookService _addressBookService;
        private readonly IOrderRepository _orderRepository;
        private readonly IContentLoader _contentLoader;
        private readonly ICartService _cartService;
        private readonly IOrderGroupFactory _orderGroupFactory;
        private readonly PaymentMethodViewModelFactory _paymentMethodViewModelFactory;
        private readonly CookieService _cookieService;
        private readonly ISettingsService _settingsService;

        private const string _KEYWORD = "OrderHistoryPage:Keyword";
        private const string _DATEFROM = "OrderHistoryPage:DateFrom";
        private const string _DATETO = "OrderHistoryPage:DateTo";
        private const string _ORDERSTATUS = "OrderHistoryPage:OrderStatus";
        private const string _SHIPPINGADDRESS = "OrderHistoryPage:ShippingAddress";
        private const string _PRICEFROM = "OrderHistoryPage:PriceFrom";
        private const string _PRICETO = "OrderHistoryPage:PriceTo";
        private const string _PURCHASENUMBER = "OrderHistoryPage:PurchaseNumber";
        private const string _ORDERGROUPID = "OrderHistoryPage:OrderGroupId";

        public OrderHistoryController(IAddressBookService addressBookService,
            IOrderRepository orderRepository,
            ConfirmationService confirmationService,
            ICartService cartService,
            IOrderGroupCalculator orderGroupCalculator,
            IContentLoader contentLoader,
            UrlResolver urlResolver, IOrderGroupFactory orderGroupFactory, ICustomerService customerService,
            PaymentMethodViewModelFactory paymentMethodViewModelFactory,
            CookieService cookieService,
            ISettingsService settingsService) :
            base(confirmationService, addressBookService, orderGroupCalculator, urlResolver, customerService)
        {
            _addressBookService = addressBookService;
            _orderRepository = orderRepository;
            _contentLoader = contentLoader;
            _cartService = cartService;
            _orderGroupFactory = orderGroupFactory;
            _paymentMethodViewModelFactory = paymentMethodViewModelFactory;
            _cookieService = cookieService;
            _settingsService = settingsService;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index(OrderHistoryPage currentPage, OrderFilter filter, int? page, int? size, int? isPaging)
        {
            if (isPaging.HasValue)
            {
                filter = GetFilter();
            }
            else
            {
                SetCookieFilter(filter);
            }
            var pageNum = page ?? 1;
            var pageSize = size ?? 10;
            var orders = _orderRepository.Load<IPurchaseOrder>(PrincipalInfo.CurrentPrincipal.GetContactId(), _cartService.DefaultCartName);
            var purchaseOrders = FilterOrders(orders, filter)
                                .OrderByDescending(x => x.Created)
                                .Skip((pageNum - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            var viewModel = new OrderHistoryViewModel(currentPage)
            {
                CurrentContent = currentPage,
                Orders = new List<OrderViewModel>(),
            };

            OrderFilter.LoadDefault(filter, _paymentMethodViewModelFactory);
            LoadAvailableAddresses(filter);

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

                foreach (var orderAddress in purchaseOrder.Forms.SelectMany(x => x.Shipments).Select(s => s.ShippingAddress))
                {
                    var shippingAddress = _addressBookService.ConvertToModel(orderAddress);
                    orderViewModel.ShippingAddresses.Add(shippingAddress);
                }

                viewModel.Orders.Add(orderViewModel);
            }
            viewModel.OrderDetailsPageUrl =
             UrlResolver.Current.GetUrl(_settingsService.GetSiteSettings<ReferencePageSettings>()?.OrderDetailsPage ?? ContentReference.StartPage);

            viewModel.PagingInfo.PageNumber = pageNum;
            viewModel.PagingInfo.TotalRecord = purchaseOrders.Count();
            viewModel.PagingInfo.PageSize = pageSize;
            viewModel.OrderHistoryUrl = Request.Url.PathAndQuery;
            viewModel.Filter = filter;
            return View(viewModel);
        }

        public ActionResult ViewAll() => Redirect(UrlResolver.Current.GetUrl(_settingsService.GetSiteSettings<ReferencePageSettings>()?.OrderHistoryPage ?? ContentReference.StartPage));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAsPaymentPlan(int orderid, int cycleMode, int cycleLength)
        {
            var purchaseOrder = _orderRepository.Load<IPurchaseOrder>(orderid);
            if (purchaseOrder == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var cart = _orderRepository.Create<ICart>(Guid.NewGuid().ToString());
            cart.CopyFrom(purchaseOrder, _orderGroupFactory);
            var orderReference = _orderRepository.SaveAsPaymentPlan(cart);
            _orderRepository.Delete(cart.OrderLink);
            var paymentPlan = _orderRepository.Load<IPaymentPlan>(orderReference.OrderGroupId);
            paymentPlan.CycleMode = (PaymentPlanCycle)cycleMode;
            paymentPlan.CycleLength = cycleLength;
            paymentPlan.StartDate = DateTime.UtcNow;
            paymentPlan.IsActive = true;

            var principal = PrincipalInfo.CurrentPrincipal;
            AddNoteToOrder(paymentPlan, $"Note: New payment plan placed by {principal.Identity.Name}.", OrderNoteTypes.System, principal.GetContactId());
            paymentPlan.AdjustInventoryOrRemoveLineItems((__, _) => { });
            _orderRepository.Save(paymentPlan);

            //create first order
            orderReference = _orderRepository.SaveAsPurchaseOrder(paymentPlan);
            var newPurchaseOrder = _orderRepository.Load<IPurchaseOrder>(orderReference.OrderGroupId);
            OrderGroupWorkflowManager.RunWorkflow((OrderGroup)newPurchaseOrder, OrderGroupWorkflowManager.CartCheckOutWorkflowName);
            var noteDetailPattern = "New purchase order placed by {0} in {1} from payment plan {2}";
            var noteDetail = string.Format(noteDetailPattern, principal.Identity.Name, "VNext site", (paymentPlan as PaymentPlan).Id);
            AddNoteToOrder(newPurchaseOrder, noteDetail, OrderNoteTypes.System, principal.GetContactId());
            _orderRepository.Save(newPurchaseOrder);

            paymentPlan.LastTransactionDate = DateTime.UtcNow;
            paymentPlan.CompletedCyclesCount++;
            _orderRepository.Save(paymentPlan);

            var paymentPlanPageUrl = Url.ContentUrl(_settingsService.GetSiteSettings<ReferencePageSettings>()?.PaymentPlanDetailsPage ?? ContentReference.StartPage)
                + $"?paymentPlanId={paymentPlan.OrderLink.OrderGroupId}";
            return Redirect(paymentPlanPageUrl);
        }

        private void AddNoteToOrder(IOrderGroup order, string noteDetails, OrderNoteTypes type, Guid customerId)
        {
            if (order == null)
            {
                throw new ArgumentNullException("purchaseOrder");
            }
            var orderNote = order.CreateOrderNote();

            if (!orderNote.OrderNoteId.HasValue)
            {
                var newOrderNoteId = -1;

                if (order.Notes.Count != 0)
                {
                    newOrderNoteId = Math.Min(order.Notes.ToList().Min(n => n.OrderNoteId.Value), 0) - 1;
                }

                orderNote.OrderNoteId = newOrderNoteId;
            }

            orderNote.CustomerId = customerId;
            orderNote.Type = type.ToString();
            orderNote.Title = noteDetails.Substring(0, Math.Min(noteDetails.Length, 24)) + "...";
            orderNote.Detail = noteDetails;
            orderNote.Created = DateTime.UtcNow;
        }

        private void LoadAvailableAddresses(OrderFilter filter)
        {
            var addresses = _addressBookService.List();
            filter.Addresses.AddRange(addresses.Select(x => new KeyValuePair<string, string>(x.Name, x.AddressId)));
        }

        private IEnumerable<IPurchaseOrder> FilterOrders(IEnumerable<IPurchaseOrder> orders, OrderFilter filter) => orders.Where(x => Filter(filter, x));

        private bool Filter(OrderFilter filter, IPurchaseOrder order)
        {
            var result = true;
            if (result && !string.IsNullOrEmpty(filter.OrderGroupId))
            {
                result = order.OrderLink.OrderGroupId.ToString().Contains(filter.OrderGroupId);
            }

            if (result && !string.IsNullOrEmpty(filter.PurchaseOrderNumber))
            {
                result = order.OrderNumber.Contains(filter.PurchaseOrderNumber);
            }

            if (result && filter.DateFrom.HasValue)
            {
                result = order.Created.Date >= filter.DateFrom.Value.Date;
            }

            if (result && filter.DateTo.HasValue)
            {
                result = order.Created.Date <= filter.DateTo.Value.Date;
            }

            if (result && !(filter.OrderStatusId == 0))
            {
                result = order.OrderStatus.Id == filter.OrderStatusId;
            }

            if (result && filter.PriceFrom > 0)
            {
                result = order.GetTotal() >= filter.PriceFrom;
            }

            if (result && filter.PriceTo > 0)
            {
                result = order.GetTotal() <= filter.PriceTo;
            }

            if (result && !string.IsNullOrEmpty(filter.AddressId))
            {
                result = order.GetFirstForm().Shipments.Where(x => x.ShippingAddress.Id == filter.AddressId).Count() > 0;
            }

            if (result && !string.IsNullOrEmpty(filter.PaymentMethodId))
            {
                result = order.GetFirstForm().Payments.Where(x => x.PaymentMethodId.ToString() == filter.PaymentMethodId).Count() > 0;
            }

            if (result && !string.IsNullOrEmpty(filter.Keyword))
            {
                result = order.GetAllLineItems().Where(x => x.DisplayName.Contains(filter.Keyword) || x.Code.Contains(filter.Keyword)).Count() > 0;
            }

            return result;
        }

        private void SetCookieFilter(OrderFilter filter)
        {
            _cookieService.Set(_KEYWORD, filter.Keyword);
            _cookieService.Set(_DATEFROM, filter.DateFrom.ToString());
            _cookieService.Set(_DATETO, filter.DateTo.ToString());
            _cookieService.Set(_ORDERSTATUS, filter.OrderStatusId.ToString());
            _cookieService.Set(_PRICEFROM, filter.PriceFrom.ToString());
            _cookieService.Set(_PRICETO, filter.PriceTo.ToString());
            _cookieService.Set(_PURCHASENUMBER, filter.PurchaseOrderNumber);
            _cookieService.Set(_ORDERGROUPID, filter.OrderGroupId);
            _cookieService.Set(_SHIPPINGADDRESS, filter.AddressId);
        }

        private OrderFilter GetFilter()
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

            filter.PurchaseOrderNumber = _cookieService.Get(_PURCHASENUMBER);
            filter.OrderGroupId = _cookieService.Get(_ORDERGROUPID);
            filter.AddressId = _cookieService.Get(_SHIPPINGADDRESS);

            return filter;
        }
    }
}