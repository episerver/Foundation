using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Globalization;
using EPiServer.Security;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using EPiServer.Web.Routing;
using Foundation.Cms.Extensions;
using Foundation.Cms.Settings;
using Foundation.Commerce;
using Foundation.Commerce.Customer;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Extensions;
using Foundation.Features.CatalogContent.Services;
using Foundation.Features.Checkout.Payments;
using Foundation.Features.Checkout.Services;
using Foundation.Features.Checkout.ViewModels;
using Foundation.Features.Header;
using Foundation.Features.MyAccount.OrderConfirmation;
using Foundation.Features.Settings;
using Foundation.Infrastructure;
using Foundation.Personalization;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Security;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.NamedCarts.DefaultCart
{
    public class DefaultCartController : PageController<CartPage>
    {
        private readonly ICartService _cartService;
        private CartWithValidationIssues _cart;
        private CartWithValidationIssues _wishlist;
        private CartWithValidationIssues _sharedcart;
        private readonly IOrderRepository _orderRepository;
        private readonly ICommerceTrackingService _recommendationService;
        private readonly CartViewModelFactory _cartViewModelFactory;
        private readonly IContentLoader _contentLoader;
        private readonly IContentRouteHelper _contentRouteHelper;
        private readonly ReferenceConverter _referenceConverter;
        private readonly IQuickOrderService _quickOrderService;
        private readonly ICustomerService _customerService;
        private readonly ShipmentViewModelFactory _shipmentViewModelFactory;
        private readonly CheckoutService _checkoutService;
        private readonly IOrderGroupCalculator _orderGroupCalculator;
        private readonly CartItemViewModelFactory _cartItemViewModelFactory;
        private readonly IProductService _productService;
        private readonly LanguageResolver _languageResolver;
        private readonly ISettingsService _settingsService;
        private readonly IPaymentService _paymentService;
        private readonly ICurrentMarket _currentMarket;

        private const string b2cMinicart = "~/Features/Shared/Foundation/Header/_HeaderCart.cshtml";

        public DefaultCartController(
            ICartService cartService,
            IOrderRepository orderRepository,
            ICommerceTrackingService recommendationService,
            CartViewModelFactory cartViewModelFactory,
            IContentLoader contentLoader,
            IContentRouteHelper contentRouteHelper,
            ReferenceConverter referenceConverter,
            IQuickOrderService quickOrderService,
            ICustomerService customerService,
            ShipmentViewModelFactory shipmentViewModelFactory,
            CheckoutService checkoutService,
            IOrderGroupCalculator orderGroupCalculator,
            CartItemViewModelFactory cartItemViewModelFactory,
            IProductService productService,
            LanguageResolver languageResolver,
            ISettingsService settingsService,
            IPaymentService paymentService,
            ICurrentMarket currentMarket)
        {
            _cartService = cartService;
            _orderRepository = orderRepository;
            _recommendationService = recommendationService;
            _cartViewModelFactory = cartViewModelFactory;
            _contentLoader = contentLoader;
            _contentRouteHelper = contentRouteHelper;
            _referenceConverter = referenceConverter;
            _quickOrderService = quickOrderService;
            _customerService = customerService;
            _shipmentViewModelFactory = shipmentViewModelFactory;
            _checkoutService = checkoutService;
            _orderGroupCalculator = orderGroupCalculator;
            _cartItemViewModelFactory = cartItemViewModelFactory;
            _productService = productService;
            _languageResolver = languageResolver;
            _settingsService = settingsService;
            _paymentService = paymentService;
            _currentMarket = currentMarket;
        }

        private CartWithValidationIssues CartWithValidationIssues => _cart ?? (_cart = _cartService.LoadCart(_cartService.DefaultCartName, true));

        private CartWithValidationIssues WishListWithValidationIssues => _wishlist ?? (_wishlist = _cartService.LoadCart(_cartService.DefaultWishListName, true));

        private CartWithValidationIssues SharedCardWithValidationIssues => _sharedcart ?? (_sharedcart = _cartService.LoadCart(_cartService.DefaultSharedCartName, true));

        private CartWithValidationIssues SharedCart => _sharedcart ?? (_sharedcart = _cartService.LoadCart(_cartService.DefaultSharedCartName, OrganizationId, true));

        private string OrganizationId => _customerService.GetCurrentContact().FoundationOrganization?.OrganizationId.ToString();

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult MiniCartDetails()
        {
            var viewModel = _cartViewModelFactory.CreateMiniCartViewModel(CartWithValidationIssues.Cart);
            return PartialView(b2cMinicart, viewModel);
        }

        public PartialViewResult LoadCartItems()
        {
            var viewModel = _cartViewModelFactory.CreateMiniCartViewModel(CartWithValidationIssues.Cart);
            return PartialView("_MiniCartItems", viewModel);
        }

        public PartialViewResult LoadMobileCartItems()
        {
            var viewModel = _cartViewModelFactory.CreateMiniCartViewModel(CartWithValidationIssues.Cart);
            return PartialView("_MobileMiniCartItems", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public async Task<ActionResult> Index(CartPage currentPage)
        {
            var messages = string.Empty;
            if (TempData[Constant.Quote.RequestQuoteStatus] != null)
            {
                var requestQuote = (bool)TempData[Constant.Quote.RequestQuoteStatus];
                if (requestQuote)
                {
                    ViewBag.QuoteMessage = "Request quote successfully";
                }
                else
                {
                    ViewBag.ErrorMessage = "Request quote unsuccessfully";
                }
            }

            if (CartWithValidationIssues.Cart != null && CartWithValidationIssues.ValidationIssues.Any())
            {
                foreach (var item in CartWithValidationIssues.Cart.GetAllLineItems())
                {
                    messages = GetValidationMessages(item, CartWithValidationIssues.ValidationIssues);
                }
            }

            var viewModel = _cartViewModelFactory.CreateLargeCartViewModel(CartWithValidationIssues.Cart, currentPage);
            viewModel.Message = messages;
            var trackingResponse = await _recommendationService.TrackCart(HttpContext, CartWithValidationIssues.Cart);
            //viewModel.Recommendations = trackingResponse.GetCartRecommendations(_referenceConverter);
            return View("LargeCart", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> AddToCart(RequestParamsToCart param)
        {
            var warningMessage = string.Empty;

            ModelState.Clear();

            if (CartWithValidationIssues.Cart == null)
            {
                _cart = new CartWithValidationIssues
                {
                    Cart = _cartService.LoadOrCreateCart(_cartService.DefaultCartName),
                    ValidationIssues = new Dictionary<ILineItem, List<ValidationIssue>>()
                };
            }

            var result = _cartService.AddToCart(CartWithValidationIssues.Cart, param);

            if (result.EntriesAddedToCart)
            {
                _orderRepository.Save(CartWithValidationIssues.Cart);
                await _recommendationService.TrackCart(HttpContext, CartWithValidationIssues.Cart);
                if (string.Equals(param.RequestFrom, "axios", StringComparison.OrdinalIgnoreCase))
                {
                    var product = "";
                    var entryLink = _referenceConverter.GetContentLink(param.Code);
                    var entry = _contentLoader.Get<EntryContentBase>(entryLink);
                    if (entry is BundleContent || entry is PackageContent)
                    {
                        product = entry.DisplayName;
                    }
                    else
                    {
                        var parentProduct = _contentLoader.Get<EntryContentBase>(entry.GetParentProducts().FirstOrDefault());
                        product = parentProduct?.DisplayName;
                    }

                    if (result.ValidationMessages.Count > 0)
                    {
                        return Json(new ChangeCartJsonResult
                        {
                            StatusCode = result.EntriesAddedToCart ? 1 : 0,
                            CountItems = (int)CartWithValidationIssues.Cart.GetAllLineItems().Sum(x => x.Quantity),
                            Message = product + " is added to the cart successfully.\n" + result.GetComposedValidationMessage(),
                            SubTotal = CartWithValidationIssues.Cart.GetSubTotal()
                        });
                    }

                    return Json(new ChangeCartJsonResult
                    {
                        StatusCode = result.EntriesAddedToCart ? 1 : 0,
                        CountItems = (int)CartWithValidationIssues.Cart.GetAllLineItems().Sum(x => x.Quantity),
                        Message = product + " is added to the cart successfully.",
                        SubTotal = CartWithValidationIssues.Cart.GetSubTotal()
                    });
                }

                return MiniCartDetails();
            }

            return new HttpStatusCodeResult(500, result.GetComposedValidationMessage());
        }

        [HttpPost]
        public async Task<ActionResult> AddAllToCart()
        {
            ModelState.Clear();

            var allLineItem = SharedCart.Cart.GetAllLineItems();
            var entriesAddedToCart = true;
            var validationMessage = "";

            foreach (var lineitem in allLineItem)
            {
                var result = _cartService.AddToCart(CartWithValidationIssues.Cart,
                    new RequestParamsToCart { Code = lineitem.Code, Quantity = lineitem.Quantity, Store = "delivery", SelectedStore = "" });
                entriesAddedToCart &= result.EntriesAddedToCart;
                validationMessage += result.GetComposedValidationMessage();
            }

            if (entriesAddedToCart)
            {
                _orderRepository.Save(CartWithValidationIssues.Cart);
                await _recommendationService.TrackCart(HttpContext, CartWithValidationIssues.Cart);

                return Json(new ChangeCartJsonResult
                {
                    StatusCode = 1,
                    CountItems = (int)CartWithValidationIssues.Cart.GetAllLineItems().Sum(x => x.Quantity),
                    Message = "Success",
                    SubTotal = CartWithValidationIssues.Cart.GetSubTotal()
                });
            }

            return new HttpStatusCodeResult(500, validationMessage);
        }

        [HttpPost]
        public async Task<ActionResult> Subscription(RequestParamsToCart param)
        {
            var warningMessage = string.Empty;

            ModelState.Clear();

            if (CartWithValidationIssues.Cart == null)
            {
                _cart = new CartWithValidationIssues
                {
                    Cart = _cartService.LoadOrCreateCart(_cartService.DefaultCartName),
                    ValidationIssues = new Dictionary<ILineItem, List<ValidationIssue>>()
                };
            }

            var result = _cartService.AddToCart(CartWithValidationIssues.Cart, param);
            if (result.EntriesAddedToCart)
            {
                var item = CartWithValidationIssues.Cart.GetAllLineItems().FirstOrDefault(x => x.Code.Equals(param.Code));
                var subscriptionPrice = PriceCalculationService.GetSubscriptionPrice(param.Code, CartWithValidationIssues.Cart.MarketId, CartWithValidationIssues.Cart.Currency);
                if (subscriptionPrice != null)
                {
                    item.Properties["SubscriptionPrice"] = subscriptionPrice.UnitPrice.Amount;
                    item.PlacedPrice = subscriptionPrice.UnitPrice.Amount;
                }

                _orderRepository.Save(CartWithValidationIssues.Cart);
                await _recommendationService.TrackCart(HttpContext, CartWithValidationIssues.Cart);
                return MiniCartDetails();
            }

            return new HttpStatusCodeResult(500, result.GetComposedValidationMessage());
        }

        public JsonResult RedirectToCart(string message)
        {
            var referencePages = _settingsService.GetSiteSettings<ReferencePageSettings>();
            if (referencePages?.CartPage.IsNullOrEmpty() ?? false)
            {
                var cartPage = _contentLoader.Get<CartPage>(referencePages.CartPage);
                return Json(new { Redirect = cartPage.StaticLinkURL, Message = message });
            }

            return Json(new { Redirect = Request.UrlReferrer.PathAndQuery, Message = message });
        }

        [HttpPost]
        public async Task<ActionResult> BuyNow(RequestParamsToCart param)
        {
            var warningMessage = string.Empty;

            ModelState.Clear();

            if (CartWithValidationIssues.Cart == null)
            {
                _cart = new CartWithValidationIssues
                {
                    Cart = _cartService.LoadOrCreateCart(_cartService.DefaultCartName),
                    ValidationIssues = new Dictionary<ILineItem, List<ValidationIssue>>()
                };
            }

            var result = _cartService.AddToCart(CartWithValidationIssues.Cart, param);
            if (!result.EntriesAddedToCart)
            {
                return new HttpStatusCodeResult(500, result.GetComposedValidationMessage());
            }
            var contact = PrincipalInfo.CurrentPrincipal.GetCustomerContact();
            if (contact == null)
            {
                return RedirectToCart("The contact is invalid");
            }

            var creditCard = contact.ContactCreditCards.FirstOrDefault();
            if (creditCard == null)
            {
                return RedirectToCart("There is not any credit card");
            }

            var shipment = CartWithValidationIssues.Cart.GetFirstShipment();
            if (shipment == null)
            {
                return RedirectToCart("The shopping cart is not exist");
            }

            var shippingAddress = (contact.PreferredShippingAddress ?? contact.ContactAddresses.FirstOrDefault())?.ConvertToOrderAddress(CartWithValidationIssues.Cart);
            if (shippingAddress == null)
            {
                return RedirectToCart("The shipping address is not exist");
            }

            shipment.ShippingAddress = shippingAddress;

            var shippingMethodViewModels = _shipmentViewModelFactory.CreateShipmentsViewModel(CartWithValidationIssues.Cart).SelectMany(x => x.ShippingMethods);
            var shippingMethodViewModel = shippingMethodViewModels.Where(x => x.Price != 0)
                .OrderBy(x => x.Price)
                .FirstOrDefault();

            //If product is virtual set shipping method is Free
            if (shipment.LineItems.FirstOrDefault().IsVirtualVariant())
            {
                shippingMethodViewModel = shippingMethodViewModels.Where(x => x.Price == 0).FirstOrDefault();
            }

            if (shippingMethodViewModel == null)
            {
                return RedirectToCart("The shipping method is invalid");
            }

            shipment.ShippingMethodId = shippingMethodViewModel.Id;

            var paymentAddress = (contact.PreferredBillingAddress ?? contact.ContactAddresses.FirstOrDefault())?.ConvertToOrderAddress(CartWithValidationIssues.Cart);
            if (paymentAddress == null)
            {
                return RedirectToCart("The billing address is not exist");
            }

            var totals = _orderGroupCalculator.GetOrderGroupTotals(CartWithValidationIssues.Cart);
            var creditCardPayment = _paymentService.GetPaymentMethodsByMarketIdAndLanguageCode(CartWithValidationIssues.Cart.MarketId.Value, _currentMarket.GetCurrentMarket().DefaultLanguage.Name).FirstOrDefault(x => x.SystemKeyword == "GenericCreditCard");
            var payment = CartWithValidationIssues.Cart.CreateCardPayment();

            payment.BillingAddress = paymentAddress;
            payment.CardType = "Credit card";
            payment.PaymentMethodId = creditCardPayment.PaymentMethodId;
            payment.PaymentMethodName = creditCardPayment.SystemKeyword;
            payment.Amount = CartWithValidationIssues.Cart.GetTotal().Amount;
            payment.CreditCardNumber = creditCard.CreditCardNumber;
            payment.CreditCardSecurityCode = creditCard.SecurityCode;
            payment.ExpirationMonth = creditCard.ExpirationMonth ?? 1;
            payment.ExpirationYear = creditCard.ExpirationYear ?? DateTime.Now.Year;
            payment.Status = PaymentStatus.Pending.ToString();
            payment.CustomerName = contact.FullName;
            payment.TransactionType = TransactionType.Authorization.ToString();
            CartWithValidationIssues.Cart.GetFirstForm().Payments.Add(payment);

            var issues = _cartService.ValidateCart(CartWithValidationIssues.Cart);
            if (issues.Keys.Any(x => issues.HasItemBeenRemoved(x)))
            {
                return RedirectToCart("The product is invalid");
            }
            var order = _checkoutService.PlaceOrder(CartWithValidationIssues.Cart, new ModelStateDictionary(), new CheckoutViewModel());

            //await _checkoutService.CreateOrUpdateBoughtProductsProfileStore(CartWithValidationIssues.Cart);
            //await _checkoutService.CreateBoughtProductsSegments(CartWithValidationIssues.Cart);
            await _recommendationService.TrackOrder(HttpContext, order);

            var referencePages = _settingsService.GetSiteSettings<ReferencePageSettings>();
            if (!(referencePages?.OrderConfirmationPage.IsNullOrEmpty() ?? true))
            {
                var orderConfirmationPage = _contentLoader.Get<OrderConfirmationPage>(referencePages.OrderConfirmationPage);
                var queryCollection = new NameValueCollection
                {
                    {"contactId", contact.PrimaryKeyId?.ToString()},
                    {"orderNumber", order.OrderLink.OrderGroupId.ToString()}
                };
                var urlRedirect = new UrlBuilder(orderConfirmationPage.StaticLinkURL) { QueryCollection = queryCollection };
                return Json(new { Redirect = urlRedirect.ToString() });
            }

            return RedirectToCart("Something went wrong");
        }

        [HttpPost]
        public ActionResult MoveToWishlist(RequestParamsToCart param)
        {
            ModelState.Clear();
            var productName = "";
            var entryLink = _referenceConverter.GetContentLink(param.Code);
            productName = _contentLoader.Get<EntryContentBase>(entryLink).DisplayName;

            var currentPage = _contentRouteHelper.Content as CartPage;
            if (WishListWithValidationIssues.Cart == null)
            {
                _wishlist = new CartWithValidationIssues
                {
                    Cart = _cartService.LoadOrCreateCart(_cartService.DefaultWishListName),
                    ValidationIssues = new Dictionary<ILineItem, List<ValidationIssue>>()
                };
            }

            var items = new Dictionary<int, string>();
            foreach (var shipment in CartWithValidationIssues.Cart.Forms.SelectMany(x => x.Shipments))
            {
                foreach (var lineItem in shipment.LineItems)
                {
                    if (!lineItem.Code.Equals(param.Code))
                    {
                        continue;
                    }
                    items.Add(shipment.ShipmentId, param.Code);
                }
            }

            if (WishListWithValidationIssues.Cart.GetAllLineItems().Any(item => item.Code.Equals(param.Code, StringComparison.OrdinalIgnoreCase)))
            {
                return Json(new ChangeCartJsonResult
                {
                    StatusCode = 0,
                    Message = productName + " already existed in the wishlist.",
                });
            }

            foreach (var key in items.Keys)
            {
                _cartService.ChangeCartItem(CartWithValidationIssues.Cart, key, items[key], 0, "", "");
            }
            _orderRepository.Save(CartWithValidationIssues.Cart);

            var result = _cartService.AddToCart(WishListWithValidationIssues.Cart,
                new RequestParamsToCart { Code = param.Code, Quantity = 1, Store = "delivery", SelectedStore = "" });
            if (!result.EntriesAddedToCart)
            {
                return new HttpStatusCodeResult(500, result.GetComposedValidationMessage());
            }

            _orderRepository.Save(WishListWithValidationIssues.Cart);

            var viewModel = _cartViewModelFactory.CreateLargeCartViewModel(CartWithValidationIssues.Cart, currentPage);

            return Json(new ChangeCartJsonResult
            {
                StatusCode = 1,
                Message = productName + " has moved to the wishlist.",
                CountItems = (int)CartWithValidationIssues.Cart.GetAllLineItems().Sum(x => x.Quantity),
                SubTotal = viewModel.Subtotal,
                Total = viewModel.Total,
                ShippingTotal = viewModel.ShippingTotal,
                TaxTotal = viewModel.TaxTotal,
                TotalDiscount = viewModel.TotalDiscount
            });
        }

        [HttpPost]
        public ActionResult AddToSharedCart(RequestParamsToCart param)
        {
            ModelState.Clear();
            var currentPage = _contentRouteHelper.Content as CartPage;
            if (SharedCardWithValidationIssues.Cart == null)
            {
                _sharedcart = new CartWithValidationIssues
                {
                    Cart = _cartService.LoadOrCreateCart(_cartService.DefaultSharedCartName, _customerService.GetCurrentContact().FoundationOrganization?.OrganizationId.ToString()),
                    ValidationIssues = new Dictionary<ILineItem, List<ValidationIssue>>()
                };
            }

            var items = new Dictionary<int, string>();
            foreach (var shipment in CartWithValidationIssues.Cart.Forms.SelectMany(x => x.Shipments))
            {
                foreach (var lineItem in shipment.LineItems)
                {
                    if (!lineItem.Code.Equals(param.Code))
                    {
                        continue;
                    }
                    items.Add(shipment.ShipmentId, param.Code);
                }
            }
            foreach (var key in items.Keys)
            {
                _cartService.ChangeCartItem(CartWithValidationIssues.Cart, key, items[key], 0, "", "");
            }
            _orderRepository.Save(CartWithValidationIssues.Cart);

            if (SharedCardWithValidationIssues.Cart.GetAllLineItems().Any(item => item.Code.Equals(param.Code, StringComparison.OrdinalIgnoreCase)))
            {
                return View("LargeCart", _cartViewModelFactory.CreateLargeCartViewModel(CartWithValidationIssues.Cart, currentPage));
            }

            var result = _cartService.AddToCart(SharedCardWithValidationIssues.Cart,
                new RequestParamsToCart { Code = param.Code, Quantity = 1, Store = "delivery", SelectedStore = "" });
            if (!result.EntriesAddedToCart)
            {
                return new HttpStatusCodeResult(500, result.GetComposedValidationMessage());
            }

            _orderRepository.Save(SharedCardWithValidationIssues.Cart);

            var viewModel = _cartViewModelFactory.CreateLargeCartViewModel(CartWithValidationIssues.Cart, currentPage);
            return View("LargeCart", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Reorder(string orderId)
        {
            if (!int.TryParse(orderId, out var orderIntId))
            {
                return new HttpStatusCodeResult(500, "Error reordering order");
            }
            var order = _orderRepository.Load<IPurchaseOrder>(orderIntId);

            if (order == null)
            {
                return new HttpStatusCodeResult(500, "Error reordering order");
            }
            ModelState.Clear();

            if (CartWithValidationIssues.Cart == null)
            {
                _cart = new CartWithValidationIssues
                {
                    Cart = _cartService.LoadOrCreateCart(_cartService.DefaultCartName),
                    ValidationIssues = new Dictionary<ILineItem, List<ValidationIssue>>()
                };
            }

            var lineitems = order.Forms.First().GetAllLineItems();
            foreach (var item in lineitems)
            {
                var result = _cartService.AddToCart(CartWithValidationIssues.Cart,
                    new RequestParamsToCart { Code = item.Code, Quantity = item.Quantity, Store = "delivery", SelectedStore = "" });
                if (result.EntriesAddedToCart)
                {
                    await _recommendationService.TrackCart(HttpContext, CartWithValidationIssues.Cart);
                }
                else
                {
                    return new HttpStatusCodeResult(500, result.GetComposedValidationMessage());
                }
            }

            _orderRepository.Save(CartWithValidationIssues.Cart);
            return Redirect(Url.ContentUrl(_settingsService.GetSiteSettings<ReferencePageSettings>()?.CheckoutPage ?? ContentReference.StartPage));
        }

        [HttpPost]
        public async Task<ActionResult> ChangeCartItem(RequestParamsToCart param) // change quantity
        {
            ModelState.Clear();

            var validationIssues = _cartService.ChangeCartItem(CartWithValidationIssues.Cart, param.ShipmentId, param.Code, param.Quantity, param.Size, param.NewSize);
            _orderRepository.Save(CartWithValidationIssues.Cart);
            var model = _cartViewModelFactory.CreateLargeCartViewModel(CartWithValidationIssues.Cart, null);
            if (validationIssues.Any())
            {
                foreach (var item in validationIssues.Keys)
                {
                    model.Message += GetValidationMessages(item, validationIssues);
                }
            }
            var trackingResponse = await _recommendationService.TrackCart(HttpContext, CartWithValidationIssues.Cart);
            //model.Recommendations = trackingResponse.GetCartRecommendations(_referenceConverter);
            var viewModel = _cartViewModelFactory.CreateLargeCartViewModel(CartWithValidationIssues.Cart, null);

            if (param.RequestFrom == "changeSizeItem")
            {
                var preferredCulture = _languageResolver.GetPreferredCulture();
                var newCode = _productService.GetSiblingVariantCodeBySize(param.Code, param.NewSize);
                var shipment = CartWithValidationIssues.Cart.GetFirstForm().Shipments.FirstOrDefault(x => x.ShipmentId == param.ShipmentId);
                var lineItem = shipment.LineItems.FirstOrDefault(x => x.Code == newCode);
                var entries = _contentLoader.GetItems(shipment.LineItems.Select(x => _referenceConverter.GetContentLink(x.Code)),
                    preferredCulture).OfType<EntryContentBase>();
                var entry = entries.FirstOrDefault(x => x.Code == lineItem.Code);
                var newItemViewModel = _cartItemViewModelFactory.CreateCartItemViewModel(CartWithValidationIssues.Cart, lineItem, entry);
                ViewData["ShipmentId"] = param.ShipmentId;
                return PartialView("_ItemTemplate", newItemViewModel);
            }

            var productName = "";
            var entryLink = _referenceConverter.GetContentLink(param.Code);
            productName = _contentLoader.Get<EntryContentBase>(entryLink).DisplayName;

            var result = new ChangeCartJsonResult
            {
                CountItems = (int)CartWithValidationIssues.Cart.GetAllLineItems().Sum(x => x.Quantity),
                SubTotal = viewModel.Subtotal,
                Total = viewModel.Total,
                ShippingTotal = viewModel.ShippingTotal,
                TaxTotal = viewModel.TaxTotal,
                TotalDiscount = viewModel.TotalDiscount
            };

            if (validationIssues.Count > 0)
            {
                result.StatusCode = 0;
                result.Message = string.Join("\n", validationIssues.Select(x => string.Join("\n", x.Value.Select(v => v.ToString()))));
            }
            else
            {
                result.StatusCode = 1;
                result.Message = productName + " has changed from the cart.";
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> RemoveCartItem(RequestParamsToCart param) // only use ShipmentId, Code (variant Code)
        {
            ModelState.Clear();
            var productName = "";
            var entryLink = _referenceConverter.GetContentLink(param.Code);
            productName = _contentLoader.Get<EntryContentBase>(entryLink).DisplayName;

            var result = _cartService.ChangeCartItem(CartWithValidationIssues.Cart, param.ShipmentId, param.Code, 0, null, null);
            _orderRepository.Save(CartWithValidationIssues.Cart);
            await _recommendationService.TrackCart(HttpContext, CartWithValidationIssues.Cart);

            if (result.Count > 0)
            {
                return Json(new ChangeCartJsonResult
                {
                    StatusCode = 0,
                    Message = "Remove " + productName + " error.",
                    CountItems = (int)CartWithValidationIssues.Cart.GetAllLineItems().Sum(x => x.Quantity),
                    SubTotal = CartWithValidationIssues.Cart.GetSubTotal()
                });
            }

            if (param.RequestFrom == "large-cart")
            {
                var viewModel = _cartViewModelFactory.CreateLargeCartViewModel(CartWithValidationIssues.Cart, null);
                return Json(new ChangeCartJsonResult
                {
                    StatusCode = 1,
                    Message = productName + " has removed from the cart.",
                    CountItems = (int)CartWithValidationIssues.Cart.GetAllLineItems().Sum(x => x.Quantity),
                    SubTotal = viewModel.Subtotal,
                    Total = viewModel.Total,
                    ShippingTotal = viewModel.ShippingTotal,
                    TaxTotal = viewModel.TaxTotal,
                    TotalDiscount = viewModel.TotalDiscount
                });
            }

            return Json(new ChangeCartJsonResult
            {
                StatusCode = 1,
                Message = productName + " has removed from the cart.",
                CountItems = (int)CartWithValidationIssues.Cart.GetAllLineItems().Sum(x => x.Quantity),
                SubTotal = CartWithValidationIssues.Cart.GetSubTotal()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCouponCode(string couponCode)
        {
            if (_cartService.AddCouponCode(CartWithValidationIssues.Cart, couponCode))
            {
                _orderRepository.Save(CartWithValidationIssues.Cart);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }

            var viewModel = _cartViewModelFactory.CreateSimpleLargeCartViewModel(CartWithValidationIssues.Cart);
            return PartialView("_CartSummary", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveCouponCode(string couponCode)
        {
            _cartService.RemoveCouponCode(CartWithValidationIssues.Cart, couponCode);
            _orderRepository.Save(CartWithValidationIssues.Cart);
            var viewModel = _cartViewModelFactory.CreateSimpleLargeCartViewModel(CartWithValidationIssues.Cart);
            return PartialView("_CartSummary", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EstimateShipping(CartPage currentPage, LargeCartViewModel largeCartViewModel)
        {
            var orderAddress = CartWithValidationIssues.Cart.GetFirstShipment().ShippingAddress;
            if (orderAddress == null)
            {
                orderAddress = CartWithValidationIssues.Cart.CreateOrderAddress(Guid.NewGuid().ToString());
                CartWithValidationIssues.Cart.GetFirstShipment().ShippingAddress = orderAddress;
            }

            orderAddress.CountryName = largeCartViewModel.AddressModel.CountryName;
            orderAddress.CountryCode = largeCartViewModel.AddressModel.CountryCode;
            orderAddress.RegionName = largeCartViewModel.AddressModel.CountryRegion.Region;
            orderAddress.PostalCode = largeCartViewModel.AddressModel.PostalCode;

            _orderRepository.Save(CartWithValidationIssues.Cart);
            var viewModel = _cartViewModelFactory.CreateLargeCartViewModel(CartWithValidationIssues.Cart, currentPage);
            return View("LargeCart", viewModel);
        }

        [HttpPost]
        public ActionResult ClearCart(CartPage currentPage)
        {
            if (CartWithValidationIssues.Cart != null)
            {
                _orderRepository.Delete(CartWithValidationIssues.Cart.OrderLink);
                _cart = null;
            }
            //var viewModel = _cartViewModelFactory.CreateLargeCartViewModel(CartWithValidationIssues.Cart, currentPage);
            var redirect = currentPage.LinkURL;
            return Json(redirect);
        }

        [HttpPost]
        public async Task<ActionResult> RemoveItem(CartPage currentPage, int shipmentId, string code)
        {
            var message = string.Empty;
            var issues = _cartService.ChangeCartItem(CartWithValidationIssues.Cart, shipmentId, code, 0, "", "");
            _orderRepository.Save(CartWithValidationIssues.Cart);
            await _recommendationService.TrackCart(HttpContext, CartWithValidationIssues.Cart);
            var viewModel = _cartViewModelFactory.CreateLargeCartViewModel(CartWithValidationIssues.Cart, currentPage);
            if (issues.Any())
            {
                foreach (var item in issues.Keys)
                {
                    viewModel.Message += GetValidationMessages(item, issues);
                }
            }
            return View("LargeCart", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestQuote(CartPage currentPage)
        {
            bool succesRequest;

            if (CartWithValidationIssues.Cart == null)
            {
                _cart = new CartWithValidationIssues
                {
                    Cart = _cartService.LoadOrCreateCart(_cartService.DefaultCartName),
                    ValidationIssues = new Dictionary<ILineItem, List<ValidationIssue>>()
                };
                succesRequest = _cartService.PlaceCartForQuote(_cart.Cart);
            }
            else
            {
                succesRequest = _cartService.PlaceCartForQuote(CartWithValidationIssues.Cart);
            }

            if (succesRequest)
            {
                _cartService.DeleteCart(_cart.Cart);
                _cart = new CartWithValidationIssues
                {
                    Cart = _cartService.CreateNewCart(),
                    ValidationIssues = new Dictionary<ILineItem, List<ValidationIssue>>()
                };

                TempData[Constant.Quote.RequestQuoteStatus] = true;
            }
            else
            {
                TempData[Constant.Quote.RequestQuoteStatus] = false;
            }

            return Redirect(currentPage.StaticLinkURL);
        }

        [HttpPost]
        public ActionResult RequestQuoteById(int orderId)
        {
            var currentCustomer = _customerService.GetCurrentContact();
            if (currentCustomer.B2BUserRole != B2BUserRoles.Purchaser)
            {
                return Json(new { result = false });
            }

            var placedOrderId = _cartService.PlaceCartForQuoteById(orderId, currentCustomer.ContactId);

            var referencePages = _settingsService.GetSiteSettings<ReferencePageSettings>();
            var orderDetailUrl = Url.ContentUrl(referencePages.OrderDetailsPage);
            return Redirect(orderDetailUrl + "?orderGroupId=" + placedOrderId);
        }

        [HttpPost]
        public JsonResult ClearQuotedCart()
        {
            _cartService.DeleteCart(CartWithValidationIssues.Cart);
            _cart = new CartWithValidationIssues
            {
                Cart = _cartService.CreateNewCart(),
                ValidationIssues = new Dictionary<ILineItem, List<ValidationIssue>>()
            };

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddVariantsToCart(List<string> variants)
        {
            var returnedMessages = new List<string>();

            ModelState.Clear();

            if (CartWithValidationIssues.Cart == null)
            {
                _cart = new CartWithValidationIssues
                {
                    Cart = _cartService.LoadOrCreateCart(_cartService.DefaultCartName),
                    ValidationIssues = new Dictionary<ILineItem, List<ValidationIssue>>()
                };
            }

            foreach (var product in variants)
            {
                var sku = product.Split(';')[0];
                var quantity = Convert.ToInt32(product.Split(';')[1]);

                var variationReference = _referenceConverter.GetContentLink(sku);

                var responseMessage = _quickOrderService.ValidateProduct(variationReference, Convert.ToDecimal(quantity), sku);
                if (responseMessage.IsNullOrEmpty())
                {
                    var result = _cartService.AddToCart(CartWithValidationIssues.Cart,
                        new RequestParamsToCart { Code = sku, Quantity = quantity, Store = "delivery", SelectedStore = "" });
                    if (result.EntriesAddedToCart)
                    {
                        _cartService.ChangeCartItem(CartWithValidationIssues.Cart, 0, sku, quantity, "", "");
                        _orderRepository.Save(CartWithValidationIssues.Cart);
                    }
                }
                else
                {
                    returnedMessages.Add(responseMessage);
                }
            }
            Session[Constant.ErrorMessages] = returnedMessages;

            return Json(returnedMessages, JsonRequestBehavior.AllowGet);
        }

        private static string GetValidationMessages(ILineItem lineItem, Dictionary<ILineItem, List<ValidationIssue>> validationIssues)
        {
            var message = string.Empty;
            foreach (var validationIssue in validationIssues)
            {
                var warning = new StringBuilder();
                warning.Append(string.Format("Line Item with code {0} ", lineItem.Code));
                validationIssue.Value.Aggregate(warning, (current, issue) => current.Append(issue).Append(", "));

                message += (warning.ToString().TrimEnd(',', ' '));
            }
            return message;
        }
    }
}