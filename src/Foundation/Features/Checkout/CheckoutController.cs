using EPiServer;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using EPiServer.Web.Routing;
using Foundation.Cms.Identity;
using Foundation.Commerce;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Customer.ViewModels;
using Foundation.Commerce.Models.Pages;
using Foundation.Commerce.Order.Services;
using Foundation.Commerce.Order.ViewModelFactories;
using Foundation.Commerce.Order.ViewModels;
using Foundation.Commerce.Personalization;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.Checkout
{
    public class CheckoutController : PageController<CheckoutPage>
    {
        private readonly CheckoutViewModelFactory _checkoutViewModelFactory;
        private readonly OrderSummaryViewModelFactory _orderSummaryViewModelFactory;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartService _cartService;
        private readonly ICommerceTrackingService _recommendationService;
        private CartWithValidationIssues _cart;
        private readonly CheckoutService _checkoutService;
        private readonly UrlHelper _urlHelper;
        private readonly ApplicationSignInManager<SiteUser> _applicationSignInManager;
        private readonly LocalizationService _localizationService;
        private readonly IAddressBookService _addressBookService;
        private readonly MultiShipmentViewModelFactory _multiShipmentViewModelFactory;
        private readonly IOrderGroupFactory _orderGroupFactory;
        private readonly IContentLoader _contentLoader;
        private readonly UrlResolver _urlResolver;
        private readonly ICustomerService _customerContext;
        private readonly IOrganizationService _organizationService;

        public CheckoutController(
            IOrderRepository orderRepository,
            CheckoutViewModelFactory checkoutViewModelFactory,
            ICartService cartService,
            OrderSummaryViewModelFactory orderSummaryViewModelFactory,
            ICommerceTrackingService recommendationService,
            CheckoutService checkoutService,
            UrlHelper urlHelper,
            ApplicationSignInManager<SiteUser> applicationSignInManager,
            LocalizationService localizationService,
            IAddressBookService addressBookService,
            MultiShipmentViewModelFactory multiShipmentViewModelFactory,
            IOrderGroupFactory orderGroupFactory,
            IContentLoader contentLoader,
            UrlResolver urlResolver,
            ICustomerService customerContext,
            IOrganizationService organizationService)
        {
            _orderRepository = orderRepository;
            _checkoutViewModelFactory = checkoutViewModelFactory;
            _cartService = cartService;
            _orderSummaryViewModelFactory = orderSummaryViewModelFactory;
            _recommendationService = recommendationService;
            _checkoutService = checkoutService;
            _urlHelper = urlHelper;
            _applicationSignInManager = applicationSignInManager;
            _localizationService = localizationService;
            _addressBookService = addressBookService;
            _multiShipmentViewModelFactory = multiShipmentViewModelFactory;
            _orderGroupFactory = orderGroupFactory;
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
            _customerContext = customerContext;
            _organizationService = organizationService;
        }

        [HttpGet]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult Index(CheckoutPage currentPage)
        {
            if (CartIsNullOrEmpty())
            {
                return View("EmptyCart", new CheckoutMethodViewModel(currentPage));
            }

            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("CheckoutMethod", new { node = currentPage.ContentLink });
            }

            if (CartWithValidationIssues.Cart.GetFirstForm().Shipments.Count(x => x.ShippingMethodId != _cartService.InStorePickupInfoModel.MethodId) == 1)
            {
                return RedirectToAction("SingleAddress", new { node = currentPage.ContentLink });
            }

            if (CartWithValidationIssues.Cart.GetFirstForm().Shipments.All(x => x.ShippingMethodId == _cartService.InStorePickupInfoModel.MethodId))
            {
                return RedirectToAction("BillingInformation", new { node = currentPage.ContentLink });
            }

            return RedirectToAction("MutipleAddresses", new { node = currentPage.ContentLink });
        }

        [HttpGet]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult CheckoutMethod(CheckoutPage currentPage)
        {
            var viewModel = new CheckoutMethodViewModel(currentPage, _urlHelper.Action("Index", "Checkout"));
            return View("CheckoutMethod", viewModel);
        }

        [HttpGet]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult ShippingInformation(CheckoutPage currentPage)
        {
            var viewModel = CreateCheckoutViewModel(currentPage);
            return View("ShippingInformation", viewModel);
        }

        [HttpGet]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult BillingInformation(CheckoutPage currentPage, int? addressType)
        {
            var viewModel = CreateCheckoutViewModel(currentPage);
            viewModel.OrderSummary = _orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);
            viewModel.BillingAddress = _addressBookService.ConvertToModel(CartWithValidationIssues.Cart.GetFirstForm()?.Payments.FirstOrDefault()?.BillingAddress);
            _addressBookService.LoadAddress(viewModel.BillingAddress);
            if (addressType == null && Request.IsAuthenticated)
            {
                viewModel.AddressType = 1;
            }
            else if (addressType == null)
            {
                viewModel.AddressType = 0;
            }
            else
            {
                viewModel.AddressType = addressType.Value;
            }
            return View("BillingInformation", viewModel);
        }

        [HttpGet]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult AddPayment(CheckoutPage currentPage)
        {
            var viewModel = CreateCheckoutViewModel(currentPage);
            viewModel.OrderSummary = _orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);
            return PartialView("AddPayment", viewModel);
        }

        [HttpGet]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult PlaceOrder(CheckoutPage currentPage)
        {
            var viewModel = CreateCheckoutViewModel(currentPage);
            viewModel.OrderSummary = _orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);
            return View("PlaceOrder", viewModel);
        }

        [HttpGet]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult PunchoutOrder(CheckoutPage currentPage)
        {
            var viewModel = CreateCheckoutViewModel(currentPage);
            viewModel.OrderSummary = _orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);
            return View("PunchoutOrder", viewModel);
        }

        [HttpGet]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult SingleAddress(CheckoutPage currentPage, int? addressType)
        {
            var viewModel = CreateCheckoutViewModel(currentPage);
            if (addressType == null && Request.IsAuthenticated)
            {
                viewModel.AddressType = 1;
            }
            else if (addressType == null)
            {
                viewModel.AddressType = 0;
            }
            else
            {
                viewModel.AddressType = addressType.Value;
            }

            return View("SingleAddress", viewModel);
        }

        [HttpGet]
        public ActionResult MutipleAddresses(CheckoutPage currentPage)
        {
            var viewModel = _multiShipmentViewModelFactory.CreateMultiShipmentViewModel(CartWithValidationIssues.Cart, currentPage, User.Identity.IsAuthenticated);
            return View("MultipleAddresses", viewModel);
        }

        [HttpGet]
        public ActionResult SingleShipment(CheckoutPage currentPage)
        {
            if (!CartIsNullOrEmpty())
            {
                _cartService.MergeShipments(CartWithValidationIssues.Cart);
                _orderRepository.Save(CartWithValidationIssues.Cart);
            }

            return RedirectToAction("Index", new { node = currentPage.ContentLink });
        }

        [HttpPost]
        public ActionResult Update(CheckoutPage currentPage, UpdateShippingMethodViewModel shipmentViewModel, IPaymentMethod paymentOption)
        {
            ModelState.Clear();

            _checkoutService.UpdateShippingMethods(CartWithValidationIssues.Cart, shipmentViewModel.Shipments);
            _checkoutService.ApplyDiscounts(CartWithValidationIssues.Cart);
            _orderRepository.Save(CartWithValidationIssues.Cart);

            var viewModel = CreateCheckoutViewModel(currentPage, paymentOption);

            return PartialView("Partial", viewModel);
        }

        [HttpPost]
        public ActionResult ChangeAddress(UpdateAddressViewModel addressViewModel)
        {
            ModelState.Clear();
            var viewModel = CreateCheckoutViewModel(addressViewModel.CurrentPage);
            _checkoutService.CheckoutAddressHandling.ChangeAddress(viewModel, addressViewModel);

            _checkoutService.UpdateShippingAddresses(CartWithValidationIssues.Cart, viewModel);

            _orderRepository.Save(CartWithValidationIssues.Cart);

            var addressViewName = addressViewModel.ShippingAddressIndex > -1 ? "SingleShippingAddress" : "BillingAddress";

            return PartialView(addressViewName, viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult OrderSummary()
        {
            var viewModel = _orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);
            return PartialView(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCouponCode(CheckoutPage currentPage, string couponCode)
        {
            if (_cartService.AddCouponCode(CartWithValidationIssues.Cart, couponCode))
            {
                var model = CreateCheckoutViewModel(currentPage);

                foreach (var payment in model.Payments)
                {
                    var paymentViewmodel = new CheckoutViewModel();
                    paymentViewmodel.Payment = payment;
                    _checkoutService.RemovePaymentFromCart(CartWithValidationIssues.Cart, paymentViewmodel);
                }
                _orderRepository.Save(CartWithValidationIssues.Cart);
                model = CreateCheckoutViewModel(currentPage);
                model.OrderSummary = _orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);
                return PartialView("_AddPayment", model);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveCouponCode(CheckoutPage currentPage, string couponCode)
        {
            _cartService.RemoveCouponCode(CartWithValidationIssues.Cart, couponCode);
            var model = CreateCheckoutViewModel(currentPage);

            foreach (var payment in model.Payments)
            {
                var paymentViewmodel = new CheckoutViewModel();
                paymentViewmodel.Payment = payment;
                _checkoutService.RemovePaymentFromCart(CartWithValidationIssues.Cart, paymentViewmodel);
            }
            _orderRepository.Save(CartWithValidationIssues.Cart);
            model = CreateCheckoutViewModel(currentPage);
            model.OrderSummary = _orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart); ;
            return PartialView("_AddPayment", model);
        }

        [HttpPost]
        public async Task<ActionResult> Purchase(CheckoutViewModel viewModel, IPaymentMethod paymentOption)
        {
            if (CartIsNullOrEmpty())
            {
                return Redirect(Url.ContentUrl(ContentReference.StartPage));
            }

            // Since the payment property is marked with an exclude binding attribute in the CheckoutViewModel
            // it needs to be manually re-added again.
            //viewModel.Payments = paymentOption;

            if (User.Identity.IsAuthenticated)
            {
                _checkoutService.CheckoutAddressHandling.UpdateAuthenticatedUserAddresses(viewModel);

                var validation = _checkoutService.AuthenticatedPurchaseValidation;

                if (!validation.ValidateModel(ModelState, viewModel) ||
                    !validation.ValidateOrderOperation(ModelState, _cartService.ValidateCart(CartWithValidationIssues.Cart)) ||
                    !validation.ValidateOrderOperation(ModelState, _cartService.RequestInventory(CartWithValidationIssues.Cart)))
                {
                    return View(viewModel);
                }
            }
            else
            {
                _checkoutService.CheckoutAddressHandling.UpdateAnonymousUserAddresses(viewModel);

                var validation = _checkoutService.AnonymousPurchaseValidation;

                if (!validation.ValidateModel(ModelState, viewModel) ||
                    !validation.ValidateOrderOperation(ModelState, _cartService.ValidateCart(CartWithValidationIssues.Cart)) ||
                    !validation.ValidateOrderOperation(ModelState, _cartService.RequestInventory(CartWithValidationIssues.Cart)))
                {
                    return View(viewModel);
                }
            }

            if (!paymentOption.ValidateData())
            {
                return View(viewModel);
            }

            _checkoutService.UpdateShippingAddresses(CartWithValidationIssues.Cart, viewModel);

            _checkoutService.CreateAndAddPaymentToCart(CartWithValidationIssues.Cart, viewModel);

            var purchaseOrder = _checkoutService.PlaceOrder(CartWithValidationIssues.Cart, ModelState, viewModel);
            if (purchaseOrder == null)
            {
                return View(viewModel);
            }

            if (Request.IsAuthenticated)
            {
                var contact = _customerContext.GetCurrentContact().Contact;
                var organization = contact.ContactOrganization;
                if (organization != null)
                {
                    purchaseOrder.Properties[Constant.Customer.CustomerFullName] = contact.FullName;
                    purchaseOrder.Properties[Constant.Customer.CustomerEmailAddress] = contact.Email;
                    purchaseOrder.Properties[Constant.Customer.CurrentCustomerOrganization] = organization.Name;
                    _orderRepository.Save(purchaseOrder);
                }
            }

            var confirmationSentSuccessfully = _checkoutService.SendConfirmation(viewModel, purchaseOrder);
            //await _checkoutService.CreateOrUpdateBoughtProductsProfileStore(CartWithValidationIssues.Cart);
            //await _checkoutService.CreateBoughtProductsSegments(CartWithValidationIssues.Cart);
            await _recommendationService.TrackOrder(HttpContext, purchaseOrder);

            return Redirect(_checkoutService.BuildRedirectionUrl(viewModel, purchaseOrder, confirmationSentSuccessfully));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuestOrRegister(string checkoutMethod)
        {
            if (CartIsNullOrEmpty())
            {
                return View("EmptyCart", new CheckoutMethodViewModel());
            }

            var content = Request.RequestContext.GetRoutedData<CheckoutPage>();
            if (checkoutMethod.Equals("register"))
            {
                return RedirectToAction("Index", "Login", new { returnUrl = content != null ? _urlHelper.ContentUrl(content.ContentLink) : "/" });
            }

            if (CartWithValidationIssues.Cart.GetFirstForm().Shipments.All(x => x.ShippingMethodId != _cartService.InStorePickupInfoModel.MethodId))
            {
                return RedirectToAction("SingleAddress", new { node = content.ContentLink });
            }

            return RedirectToAction(CartWithValidationIssues.Cart.GetFirstForm().Shipments.Count == 1 ? "ShippingInformation" : "MutipleAddresses", new { node = content.ContentLink });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(CheckoutMethodViewModel viewModel)
        {
            var result = _applicationSignInManager.PasswordSignInAsync(viewModel.LoginViewModel.Email, viewModel.LoginViewModel.Password, true, true).Result;
            switch (result)
            {
                case SignInStatus.Success:
                    break;
                default:
                    ModelState.AddModelError("LoginViewModel.Password", _localizationService.GetString("/Login/Form/Error/WrongPasswordOrEmail"));
                    return View("CheckoutMethod", viewModel);
            }

            return RedirectToAction("Index", "Checkout");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSingleShipmentAddress(CheckoutViewModel checkoutViewModel)
        {
            ModelState.Clear();
            var content = Request.RequestContext.GetRoutedData<CheckoutPage>();
            var viewModel = CreateCheckoutViewModel(content);
            if (checkoutViewModel.AddressType == 0)
            {
                checkoutViewModel.Shipments[0].Address.AddressId = null;
                checkoutViewModel.Shipments[0].Address.Name = DateTime.Now.ToString();
                _addressBookService.Save(checkoutViewModel.Shipments[0].Address);
                viewModel.Shipments[0].Address = checkoutViewModel.Shipments[0].Address;
            }
            else
            {
                if (string.IsNullOrEmpty(checkoutViewModel.Shipments[0].Address.AddressId))
                {
                    viewModel.AddressType = 1;
                    ModelState.AddModelError("Shipments[0].Address.AddressId", "Address is required.");
                    return View("SingleAddress", viewModel);
                }

                _addressBookService.LoadAddress(checkoutViewModel.Shipments[0].Address);
                viewModel.Shipments[0].Address = checkoutViewModel.Shipments[0].Address;
            }

            _checkoutService.UpdateShippingAddresses(CartWithValidationIssues.Cart, viewModel);
            _orderRepository.Save(CartWithValidationIssues.Cart);

            return RedirectToAction("ShippingInformation", "Checkout");
        }

        //[HttpPost]
        //public ActionResult UpdateMultipleShipmentAddresses(MultiAddressViewModel viewModel)
        //{
        //    for (var i = 0; i < viewModel.CartItems.Length; i++)
        //    {
        //        if (string.IsNullOrEmpty(viewModel.CartItems[i].AddressId))
        //        {
        //            ModelState.AddModelError($"CartItems[{i}].AddressId", _localizationService.GetString("/Checkout/MultiShipment/Empty/AddressId"));
        //        }
        //    }

        //    var content = Request.RequestContext.GetRoutedData<CheckoutPage>();
        //    if (!ModelState.IsValid)
        //    {
        //        return View("MultipleAddresses", _multiShipmentViewModelFactory.CreateMultiShipmentViewModel(CartWithValidationIssues.Cart, content, User.Identity.IsAuthenticated));
        //    }

        //    _cartService.RecreateLineItemsBasedOnShipments(CartWithValidationIssues.Cart, viewModel.CartItems, GetAddresses(viewModel));

        //    _orderRepository.Save(CartWithValidationIssues.Cart);

        //    return RedirectToAction("ShippingInformation", "Checkout");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PlaceOrder(CheckoutPage currentPage, CheckoutViewModel checkoutViewModel)
        {
            var purchaseOrder = _checkoutService.PlaceOrder(CartWithValidationIssues.Cart, ModelState, checkoutViewModel);
            if (purchaseOrder == null)
            {
                var viewModel = CreateCheckoutViewModel(currentPage);
                viewModel.OrderSummary = _orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);
                return View("PlaceOrder", viewModel);
            }
            if (Request.IsAuthenticated)
            {
                var contact = _customerContext.GetCurrentContact().Contact;
                var organization = contact.ContactOrganization;
                if (organization != null)
                {
                    purchaseOrder.Properties[Constant.Customer.CustomerFullName] = contact.FullName;
                    purchaseOrder.Properties[Constant.Customer.CustomerEmailAddress] = contact.Email;
                    purchaseOrder.Properties[Constant.Customer.CurrentCustomerOrganization] = organization.Name;
                    _orderRepository.Save(purchaseOrder);
                }
            }
            checkoutViewModel.CurrentContent = currentPage;
            var confirmationSentSuccessfully = _checkoutService.SendConfirmation(checkoutViewModel, purchaseOrder);
            //await _checkoutService.CreateOrUpdateBoughtProductsProfileStore(CartWithValidationIssues.Cart);
            //await _checkoutService.CreateBoughtProductsSegments(CartWithValidationIssues.Cart);
            await _recommendationService.TrackOrder(HttpContext, purchaseOrder);

            return Redirect(_checkoutService.BuildRedirectionUrl(checkoutViewModel, purchaseOrder, confirmationSentSuccessfully));
        }

        [HttpPost]
        public ActionResult UpdatePaymentOption(CheckoutPage currentPage, IPaymentMethod paymentOption)
        {
            ModelState.Clear();

            var viewModel = CreateCheckoutViewModel(currentPage, paymentOption);
            var partialView = string.Format("_{0}PaymentMethod", paymentOption.SystemKeyword);

            return PartialView(partialView, viewModel.Payment);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateShippingMethods(CheckoutViewModel viewModel)
        {
            _checkoutService.UpdateShippingMethods(CartWithValidationIssues.Cart, viewModel.Shipments);
            _checkoutService.ApplyDiscounts(CartWithValidationIssues.Cart);
            _orderRepository.Save(CartWithValidationIssues.Cart);
            return RedirectToAction("BillingInformation", "Checkout");
        }

        [HttpPost]
        public ActionResult UpdatePayment(CheckoutPage currentPage, CheckoutViewModel viewModel, IPaymentMethod paymentOption)
        {
            if (!paymentOption.ValidateData())
            {
                return View(viewModel);
            }

            viewModel.Payment = paymentOption;
            _checkoutService.CreateAndAddPaymentToCart(CartWithValidationIssues.Cart, viewModel);
            _orderRepository.Save(CartWithValidationIssues.Cart);

            var model = CreateCheckoutViewModel(currentPage);
            model.OrderSummary = _orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);
            if (Request.IsAuthenticated)
            {
                model.AddressType = 1;
            }
            else
            {
                model.AddressType = 0;
            }
            return PartialView("_AddPayment", model);
        }

        [HttpPost]
        public ActionResult RemovePayment(CheckoutPage currentPage, CheckoutViewModel viewModel, IPaymentMethod paymentOption)
        {
            if (!paymentOption.ValidateData())
            {
                return View(viewModel);
            }

            viewModel.Payment = paymentOption;
            _checkoutService.RemovePaymentFromCart(CartWithValidationIssues.Cart, viewModel);
            _orderRepository.Save(CartWithValidationIssues.Cart);

            var model = CreateCheckoutViewModel(currentPage);
            model.OrderSummary = _orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);
            if (Request.IsAuthenticated)
            {
                model.AddressType = 1;
            }
            else
            {
                model.AddressType = 0;
            }
            return PartialView("_AddPayment", model);
        }

        [HttpPost]
        public ActionResult UpdatePaymentAddress(CheckoutPage currentPage, CheckoutViewModel viewModel)
        {
            var orderSummary = _orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);
            var isMissingPayment = !CartWithValidationIssues.Cart.Forms.SelectMany(x => x.Payments).Any();
            if (isMissingPayment || orderSummary.PaymentTotal != 0)
            {
                var model = CreateCheckoutViewModel(currentPage);
                model.OrderSummary = orderSummary;
                model.AddressType = viewModel.AddressType;
                model.BillingAddress = viewModel.BillingAddress;

                if (viewModel.AddressType == 1)
                {
                    if (string.IsNullOrEmpty(viewModel.BillingAddress.AddressId))
                    {
                        ModelState.AddModelError("BillingAddress.AddressId", "Address is required.");
                        model.AddressType = 1;
                    }
                }

                if (isMissingPayment)
                {
                    ModelState.AddModelError("SelectedPayment", _localizationService.GetString("/Shared/PaymentRequired"));
                }

                if (orderSummary.PaymentTotal != 0)
                {
                    ModelState.AddModelError("PaymentTotal", "PaymentTotal is invalid.");
                }

                return View("BillingInformation", model);
            }

            if (viewModel.AddressType == 1)
            {
                if (string.IsNullOrEmpty(viewModel.BillingAddress.AddressId))
                {
                    ModelState.AddModelError("BillingAddress.AddressId", "Address is required.");
                    var model = CreateCheckoutViewModel(currentPage);
                    model.OrderSummary = orderSummary;
                    model.AddressType = 1;
                    return View("BillingInformation", model);
                }

                _addressBookService.LoadAddress(viewModel.BillingAddress);
            }
            else
            {
                viewModel.BillingAddress.AddressId = null;
                viewModel.BillingAddress.Name = DateTime.Now.ToString();
                _addressBookService.Save(viewModel.BillingAddress);
            }

            foreach (var payment in CartWithValidationIssues.Cart.GetFirstForm().Payments)
            {
                payment.BillingAddress = _addressBookService.ConvertToAddress(viewModel.BillingAddress, CartWithValidationIssues.Cart);
            }

            _orderRepository.Save(CartWithValidationIssues.Cart);
            return RedirectToAction("CreateSubscription", "Checkout");
        }

        public ActionResult LoadOrder(int orderLink)
        {
            bool success = false;
            var purchaseOrder = _orderRepository.Load<IPurchaseOrder>(orderLink);

            DateTime.TryParse(purchaseOrder.Properties[Constant.Quote.QuoteExpireDate].ToString(), out var quoteExpireDate);
            if (DateTime.Compare(DateTime.Now, quoteExpireDate) > 0)
            {
                return Json(new { success });
            }

            if (CartWithValidationIssues.Cart != null && CartWithValidationIssues.Cart.OrderLink != null)
            {
                _orderRepository.Delete(CartWithValidationIssues.Cart.OrderLink);
            }

            _cart = new CartWithValidationIssues
            {
                Cart = _cartService.CreateNewCart(),
                ValidationIssues = new Dictionary<ILineItem, List<ValidationIssue>>()
            };
            var returnedCart = _cartService.PlaceOrderToCart(purchaseOrder, _cart.Cart);

            returnedCart.Properties[Constant.Quote.ParentOrderGroupId] = purchaseOrder.OrderLink.OrderGroupId;
            _orderRepository.Save(returnedCart);


            var checkoutPage = _contentLoader.Get<CommerceHomePage>(ContentReference.StartPage).CheckoutPage;
            _cartService.ValidateCart(returnedCart);
            return Json(new { link = _urlResolver.GetUrl(checkoutPage) });
        }

        [HttpGet]
        public ActionResult CreateSubscription(CheckoutPage currentPage)
        {
            var viewModel = CreateCheckoutViewModel(currentPage);
            return View("Subscription", viewModel);
        }

        [HttpPost]
        public ActionResult AddSubscription(CheckoutViewModel checkoutViewModel)
        {
            _checkoutService.UpdatePaymentPlan(CartWithValidationIssues.Cart, checkoutViewModel);
            _orderRepository.Save(CartWithValidationIssues.Cart);
            return RedirectToAction("PlaceOrder", "Checkout");
        }

        public ActionResult OnPurchaseException(ExceptionContext filterContext)
        {
            var currentPage = filterContext.RequestContext.GetRoutedData<CheckoutPage>();
            if (currentPage == null)
            {
                return new EmptyResult();
            }

            var viewModel = CreateCheckoutViewModel(currentPage);
            ModelState.AddModelError("Purchase", filterContext.Exception.Message);

            return View(viewModel.ViewName, viewModel);
        }

        //protected override void OnException(ExceptionContext filterContext) => _controllerExceptionHandler.HandleRequestValidationException(filterContext, "purchase", OnPurchaseException);

        private ViewResult View(CheckoutViewModel checkoutViewModel) => View(checkoutViewModel.ViewName, CreateCheckoutViewModel(checkoutViewModel.CurrentContent, checkoutViewModel.Payments.FirstOrDefault()));

        private CheckoutViewModel CreateCheckoutViewModel(CheckoutPage currentPage, IPaymentMethod paymentOption = null) => _checkoutViewModelFactory.CreateCheckoutViewModel(CartWithValidationIssues.Cart, currentPage, paymentOption);

        private CartWithValidationIssues CartWithValidationIssues => _cart ?? (_cart = _cartService.LoadCart(_cartService.DefaultCartName, true));

        private bool CartIsNullOrEmpty() => CartWithValidationIssues.Cart == null || !CartWithValidationIssues.Cart.GetAllLineItems().Any();

        private IList<AddressModel> GetAddresses(MultiAddressViewModel viewModel)
        {
            var addresses = new List<AddressModel>();
            var savedAddresses = _addressBookService.List();
            var orderAddresses = CartWithValidationIssues.Cart.GetFirstForm()
                .Shipments
                .Select(x => x.ShippingAddress)
                .Where(x => x != null);

            foreach (var addressId in viewModel.CartItems.Select(x => x.AddressId).Distinct())
            {
                var address = new AddressModel { AddressId = addressId };
                var savedAddress = savedAddresses.FirstOrDefault(x => x.AddressId.Equals(addressId));
                if (savedAddress != null)
                {
                    _addressBookService.LoadAddress(address);
                    addresses.Add(address);
                    continue;
                }

                var orderAddress = orderAddresses.FirstOrDefault(x => x.Id.Equals(addressId));
                if (orderAddress == null)
                {
                    continue;
                }
                addresses.Add(_addressBookService.ConvertToModel(orderAddress));
            }


            return addresses;
        }
    }
}
