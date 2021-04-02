using EPiServer;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using EPiServer.Web.Routing;
using Foundation.Cms.Identity;
using Foundation.Cms.Settings;
using Foundation.Commerce;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.GiftCard;
using Foundation.Features.Checkout.Payments;
using Foundation.Features.Checkout.Services;
using Foundation.Features.Checkout.ViewModels;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.MyOrganization.Organization;
using Foundation.Features.NamedCarts;
using Foundation.Features.Settings;
using Foundation.Personalization;
using Mediachase.Commerce;
using Mediachase.Commerce.Shared;
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
        private readonly ShipmentViewModelFactory _shipmentViewModelFactory;
        private readonly IGiftCardService _giftCardService;
        private readonly ISettingsService _settingsService;

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
            IOrganizationService organizationService,
            ShipmentViewModelFactory shipmentViewModelFactory,
            IGiftCardService giftCardService,
            ISettingsService settingsService)
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
            _shipmentViewModelFactory = shipmentViewModelFactory;
            _giftCardService = giftCardService;
            _settingsService = settingsService;
        }

        [HttpGet]
        [OutputCache(Duration = 0, NoStore = true)]
        public ActionResult Index(CheckoutPage currentPage, int? isGuest)
        {
            if (CartIsNullOrEmpty())
            {
                return View("EmptyCart", new CheckoutMethodViewModel(currentPage));
            }

            if (!Request.IsAuthenticated && (!isGuest.HasValue || isGuest.Value != 1))
            {
                return RedirectToAction("CheckoutMethod", new { node = currentPage.ContentLink });
            }

            if (CartWithValidationIssues.Cart.GetFirstShipment().ShippingMethodId == Guid.Empty)
            {
                _checkoutService.UpdateShippingMethods(CartWithValidationIssues.Cart, _shipmentViewModelFactory.CreateShipmentsViewModel(CartWithValidationIssues.Cart).ToList());
                _orderRepository.Save(CartWithValidationIssues.Cart);
            }

            var viewModel = CreateCheckoutViewModel(currentPage);
            viewModel.OrderSummary = _orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);
            viewModel.BillingAddress = _addressBookService.ConvertToModel(CartWithValidationIssues.Cart.GetFirstForm()?.Payments.FirstOrDefault()?.BillingAddress);
            _addressBookService.LoadAddress(viewModel.BillingAddress);

            var shipmentBillingTypes = TempData["ShipmentBillingTypes"] as List<KeyValuePair<string, int>>;

            if (shipmentBillingTypes != null && shipmentBillingTypes.Any(x => x.Key == "Billing"))
            {
                viewModel.BillingAddressType = 0;
            }
            else
            {
                if (viewModel.Shipments.Count == 1)
                {
                    viewModel.BillingAddressType = 2;
                }
                else if (Request.IsAuthenticated)
                {
                    viewModel.BillingAddressType = 1;
                }
                else
                {
                    viewModel.BillingAddressType = 0;
                }
            }

            var shippingAddressType = Request.IsAuthenticated ? 1 : 0;
            for (var i = 0; i < viewModel.Shipments.Count; i++)
            {
                if (shipmentBillingTypes != null && shipmentBillingTypes.Where(x => x.Key == "Shipment").Any(x => x.Value == i))
                {
                    viewModel.Shipments[i].ShippingAddressType = 0;
                }
                else
                {
                    if (string.IsNullOrEmpty(viewModel.Shipments[i].Address.AddressId))
                    {
                        viewModel.Shipments[i].ShippingAddressType = shippingAddressType;
                    }
                    else
                    {
                        viewModel.Shipments[i].ShippingAddressType = 1;
                    }
                }
            }

            if (TempData[Constant.ErrorMessages] != null)
            {
                ViewBag.ErrorMessages = (string)TempData[Constant.ErrorMessages];
            }

            var tempDataState = TempData["ModelState"] as List<KeyValuePair<string, string>>;
            if (tempDataState != null)
            {
                foreach (var e in tempDataState)
                {
                    ViewData.ModelState.AddModelError(e.Key, e.Value);
                }
            }

            return View("Checkout", viewModel);
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
        public ActionResult ChangeAddress(CheckoutPage currentPage, UpdateAddressViewModel addressViewModel)
        {
            ModelState.Clear();
            try
            {
                var viewModel = CreateCheckoutViewModel(currentPage);
                viewModel.BillingAddress = _addressBookService.ConvertToModel(CartWithValidationIssues.Cart.GetFirstForm()?.Payments.FirstOrDefault()?.BillingAddress);
                _addressBookService.LoadAddress(viewModel.BillingAddress);
                _checkoutService.CheckoutAddressHandling.ChangeAddress(viewModel, addressViewModel);
                _checkoutService.ChangeAddress(CartWithValidationIssues.Cart, viewModel, addressViewModel);
                _orderRepository.Save(CartWithValidationIssues.Cart);
                return Json(new { Status = true });
            }
            catch (Exception e)
            {
                return Json(new { Status = false, Message = e.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAddress(CheckoutPage currentPage, AddressModel viewModel, string returnUrl)
        {
            if (string.IsNullOrEmpty(viewModel.Name))
            {
                ModelState.AddModelError("Address.Name", _localizationService.GetString("/Shared/Address/Form/Empty/Name", "Name is required"));
            }

            if (!_addressBookService.CanSave(viewModel))
            {
                ModelState.AddModelError("Address.Name", _localizationService.GetString("/AddressBook/Form/Error/ExistingAddress", "An address with the same name already exists"));
            }

            if (!ModelState.IsValid)
            {
                var error = ModelState.Select(x =>
                {
                    if (x.Value.Errors.Count > 0)
                    {
                        return x.Key + ": " + string.Join(" ", x.Value.Errors.Select(y => y.ErrorMessage)) + "</br>";
                    }
                    return "";
                });

                return Json(new { Status = false, Message = error });
            }

            _addressBookService.Save(viewModel);
            return Json(new { Status = true, RedirectUrl = returnUrl });
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
                    var paymentViewmodel = new CheckoutViewModel
                    {
                        Payment = payment
                    };
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
                var paymentViewmodel = new CheckoutViewModel
                {
                    Payment = payment
                };
                _checkoutService.RemovePaymentFromCart(CartWithValidationIssues.Cart, paymentViewmodel);
            }
            _orderRepository.Save(CartWithValidationIssues.Cart);
            model = CreateCheckoutViewModel(currentPage);
            model.OrderSummary = _orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);
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

            var confirmationSentSuccessfully = await _checkoutService.SendConfirmation(viewModel, purchaseOrder);
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

            return RedirectToAction("Index", new { node = content.ContentLink, isGuest = 1 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(CheckoutMethodViewModel viewModel)
        {
            var result = await _applicationSignInManager.PasswordSignInAsync(viewModel.LoginViewModel.Email, viewModel.LoginViewModel.Password, true, true);
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
        public ActionResult UpdateShippingMethods(CheckoutPage currentPage, CheckoutViewModel viewModel)
        {
            _checkoutService.UpdateShippingMethods(CartWithValidationIssues.Cart, viewModel.Shipments);
            _checkoutService.ApplyDiscounts(CartWithValidationIssues.Cart);
            _orderRepository.Save(CartWithValidationIssues.Cart);

            var model = CreateCheckoutViewModel(currentPage);

            foreach (var payment in model.Payments)
            {
                var paymentViewmodel = new CheckoutViewModel
                {
                    Payment = payment
                };
                _checkoutService.RemovePaymentFromCart(CartWithValidationIssues.Cart, paymentViewmodel);
            }
            _orderRepository.Save(CartWithValidationIssues.Cart);
            model = CreateCheckoutViewModel(currentPage);
            model.OrderSummary = _orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);
            return PartialView("_AddPayment", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PlaceOrder(CheckoutPage currentPage, CheckoutViewModel checkoutViewModel)
        {
            ModelState.Clear();

            // store the shipment indexes and billing address properties if they are invalid when run TryValidateModel
            // format: key = Shipment | Billing
            var errorTypes = new List<KeyValuePair<string, int>>();

            // shipping information
            UpdateShipmentAddress(checkoutViewModel, errorTypes);

            // subscription
            AddSubscription(checkoutViewModel);

            // billing address
            UpdatePaymentAddress(checkoutViewModel, errorTypes);
            _orderRepository.Save(CartWithValidationIssues.Cart);

            if (!ModelState.IsValid)
            {
                var stateValues = new List<KeyValuePair<string, string>>();
                stateValues.AddRange(ModelState.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.Errors.FirstOrDefault().ErrorMessage)));
                TempData["ModelState"] = stateValues;
                TempData["ShipmentBillingTypes"] = errorTypes;
                return RedirectToAction("Index");
            }

            try
            {
                var purchaseOrder = _checkoutService.PlaceOrder(CartWithValidationIssues.Cart, ModelState, checkoutViewModel);
                if (purchaseOrder == null)
                {
                    TempData[Constant.ErrorMessages] = "There is no payment was processed";
                    return RedirectToAction("Index");
                }

                if (checkoutViewModel.BillingAddressType == 0)
                {
                    _addressBookService.Save(checkoutViewModel.BillingAddress);
                }

                foreach (var shipment in checkoutViewModel.Shipments)
                {
                    if (shipment.ShippingAddressType == 0 && shipment.ShippingMethodId != _cartService.InStorePickupInfoModel.MethodId)
                    {
                        _addressBookService.Save(shipment.Address);
                    }
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
                var confirmationSentSuccessfully = await _checkoutService.SendConfirmation(checkoutViewModel, purchaseOrder);
                //await _checkoutService.CreateOrUpdateBoughtProductsProfileStore(CartWithValidationIssues.Cart);
                //await _checkoutService.CreateBoughtProductsSegments(CartWithValidationIssues.Cart);
                await _recommendationService.TrackOrder(HttpContext, purchaseOrder);

                return Redirect(_checkoutService.BuildRedirectionUrl(checkoutViewModel, purchaseOrder, confirmationSentSuccessfully));
            }
            catch (Exception e)
            {
                TempData[Constant.ErrorMessages] = e.Message;
                return RedirectToAction("Index");
            }
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
        public ActionResult UpdatePayment(CheckoutPage currentPage, CheckoutViewModel viewModel, IPaymentMethod paymentOption)
        {
            if (!paymentOption.ValidateData())
            {
                return View(viewModel);
            }

            if (paymentOption is GiftCardPaymentOption)
            {
                var giftCard = _giftCardService.GetGiftCard(((GiftCardPaymentOption)paymentOption).SelectedGiftCardId);
                var paymentTotal = CurrencyFormatter.ConvertCurrency(new Money(viewModel.OrderSummary.PaymentTotal, CartWithValidationIssues.Cart.Currency), Currency.USD);
                if (paymentTotal > giftCard.RemainBalance)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Not enought money in Gift Card");
                }
            }

            viewModel.Payment = paymentOption;
            _checkoutService.CreateAndAddPaymentToCart(CartWithValidationIssues.Cart, viewModel);
            _orderRepository.Save(CartWithValidationIssues.Cart);

            var model = CreateCheckoutViewModel(currentPage);
            model.OrderSummary = _orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);

            if (Request.IsAuthenticated)
            {
                model.BillingAddressType = 1;
            }
            else
            {
                model.BillingAddressType = 0;
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
                model.BillingAddressType = 1;
            }
            else
            {
                model.BillingAddressType = 0;
            }
            return PartialView("_AddPayment", model);
        }

        public void UpdatePaymentAddress(CheckoutViewModel viewModel, List<KeyValuePair<string, int>> errorTypes)
        {
            var orderSummary = _orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);
            var isMissingPayment = !CartWithValidationIssues.Cart.Forms.SelectMany(x => x.Payments).Any();
            if (isMissingPayment || orderSummary.PaymentTotal != 0)
            {
                if (viewModel.BillingAddressType == 1)
                {
                    if (string.IsNullOrEmpty(viewModel.BillingAddress.AddressId))
                    {
                        ModelState.AddModelError("BillingAddress.AddressId", "Address is required.");
                    }
                }

                if (isMissingPayment)
                {
                    ModelState.AddModelError("SelectedPayment", _localizationService.GetString("/Shared/PaymentRequired"));
                    return;
                }

                if (orderSummary.PaymentTotal != 0)
                {
                    ModelState.AddModelError("PaymentTotal", "PaymentTotal is invalid.");
                    return;
                }
            }

            if (viewModel.BillingAddressType == 1)
            {
                if (string.IsNullOrEmpty(viewModel.BillingAddress.AddressId))
                {
                    ModelState.AddModelError("BillingAddress.AddressId", "Address is required.");
                    return;
                }

                _addressBookService.LoadAddress(viewModel.BillingAddress);
            }
            else if (viewModel.BillingAddressType == 2)
            {
                viewModel.BillingAddress = viewModel.Shipments.FirstOrDefault()?.Address;
                if (viewModel.BillingAddress == null)
                {
                    ModelState.AddModelError("BillingAddress.AddressId", "Shipping address is required.");
                    return;
                }
            }
            else
            {
                var addressName = viewModel.BillingAddress.FirstName + " " + viewModel.BillingAddress.LastName;
                viewModel.BillingAddress.AddressId = null;
                viewModel.BillingAddress.Name = addressName + " " + DateTime.Now.ToString();

                if (!TryValidateModel(viewModel.BillingAddress, "BillingAddress"))
                {
                    errorTypes.Add(new KeyValuePair<string, int>("Billing", 1));
                }
            }

            foreach (var payment in CartWithValidationIssues.Cart.GetFirstForm().Payments)
            {
                payment.BillingAddress = _addressBookService.ConvertToAddress(viewModel.BillingAddress, CartWithValidationIssues.Cart);
            }
        }

        public void AddSubscription(CheckoutViewModel checkoutViewModel) => _checkoutService.UpdatePaymentPlan(CartWithValidationIssues.Cart, checkoutViewModel);

        public void UpdateShipmentAddress(CheckoutViewModel checkoutViewModel, List<KeyValuePair<string, int>> errorTypes)
        {
            var content = Request.RequestContext.GetRoutedData<CheckoutPage>();
            var viewModel = CreateCheckoutViewModel(content);
            if (!checkoutViewModel.UseShippingingAddressForBilling)
            {
                for (var i = 0; i < checkoutViewModel.Shipments.Count; i++)
                {
                    if (checkoutViewModel.Shipments[i].ShippingAddressType == 0)
                    {
                        var addressName = checkoutViewModel.Shipments[i].Address.FirstName + " " + checkoutViewModel.Shipments[i].Address.LastName;
                        checkoutViewModel.Shipments[i].Address.AddressId = null;
                        checkoutViewModel.Shipments[i].Address.Name = addressName + " " + DateTime.Now.ToString();
                        viewModel.Shipments[i].Address = checkoutViewModel.Shipments[i].Address;

                        if (!TryValidateModel(checkoutViewModel.Shipments[i].Address, "Shipments[" + i + "].Address"))
                        {
                            errorTypes.Add(new KeyValuePair<string, int>("Shipment", i));
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(checkoutViewModel.Shipments[i].Address.AddressId))
                        {
                            viewModel.Shipments[i].ShippingAddressType
                                = 1;
                            ModelState.AddModelError("Shipments[" + i + "].Address.AddressId", "Address is required.");
                        }

                        _addressBookService.LoadAddress(checkoutViewModel.Shipments[i].Address);
                        viewModel.Shipments[i].Address = checkoutViewModel.Shipments[i].Address;
                    }
                }
            }

            _checkoutService.UpdateShippingAddresses(CartWithValidationIssues.Cart, viewModel);
        }

        // using on OrderDetail page
        public ActionResult LoadOrder(int orderLink)
        {
            var success = false;
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

            var referenceSettings = _settingsService.GetSiteSettings<ReferencePageSettings>();
            _cartService.ValidateCart(returnedCart);
            return Json(new { link = _urlResolver.GetUrl(referenceSettings?.CheckoutPage ?? ContentReference.StartPage) });
        }

        public ActionResult ChangeCartItem(CheckoutPage currentPage, string code, int quantity, int shipmentId = -1)
        {
            var result = _cartService.ChangeQuantity(CartWithValidationIssues.Cart, shipmentId, code, quantity);
            var model = CreateCheckoutViewModel(currentPage);

            foreach (var payment in model.Payments)
            {
                var paymentViewmodel = new CheckoutViewModel
                {
                    Payment = payment
                };
                _checkoutService.RemovePaymentFromCart(CartWithValidationIssues.Cart, paymentViewmodel);
            }
            _orderRepository.Save(CartWithValidationIssues.Cart);
            model = CreateCheckoutViewModel(currentPage);
            model.OrderSummary = _orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);
            return PartialView("_AddPayment", model);
        }

        [HttpPost]
        public ActionResult SeparateShipment(CheckoutPage currentPage, RequestParamsToCart param)
        {
            var result = _cartService.SeparateShipment(CartWithValidationIssues.Cart, param.Code, (int)param.Quantity, param.ShipmentId, param.ToShipmentId, param.DeliveryMethodId, param.SelectedStore);

            if (result.EntriesAddedToCart)
            {
                var model = CreateCheckoutViewModel(currentPage);
                foreach (var payment in model.Payments)
                {
                    var paymentViewmodel = new CheckoutViewModel
                    {
                        Payment = payment
                    };
                    _checkoutService.RemovePaymentFromCart(CartWithValidationIssues.Cart, paymentViewmodel);
                }
                _orderRepository.Save(CartWithValidationIssues.Cart);

                return Json(new { Status = true, RedirectUrl = Url.Action("Index") });
            }

            return Json(new { Status = false, Message = string.Join(", ", result.ValidationMessages) });
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

        private ViewResult View(CheckoutViewModel checkoutViewModel) => View(checkoutViewModel.ViewName, CreateCheckoutViewModel(checkoutViewModel.CurrentContent, checkoutViewModel.Payments.FirstOrDefault()));

        private CheckoutViewModel CreateCheckoutViewModel(CheckoutPage currentPage, IPaymentMethod paymentOption = null) => _checkoutViewModelFactory.CreateCheckoutViewModel(CartWithValidationIssues.Cart, currentPage, paymentOption);

        private CartWithValidationIssues CartWithValidationIssues => _cart ?? (_cart = _cartService.LoadCart(_cartService.DefaultCartName, true));

        private bool CartIsNullOrEmpty() => CartWithValidationIssues.Cart == null || !CartWithValidationIssues.Cart.GetAllLineItems().Any();
    }
}
