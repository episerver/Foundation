using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Globalization;
using EPiServer.Tracking.Commerce;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Cms.Extensions;
using Foundation.Cms.Settings;
using Foundation.Commerce;
using Foundation.Commerce.Customer.Services;
using Foundation.Features.CatalogContent.Bundle;
using Foundation.Features.CatalogContent.Services;
using Foundation.Features.Checkout;
using Foundation.Features.Checkout.Services;
using Foundation.Features.Checkout.ViewModels;
using Foundation.Features.Settings;
using Foundation.Personalization;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.NamedCarts.Wishlist
{
    [Authorize]
    public class WishListController : PageController<WishListPage>
    {
        private readonly IContentLoader _contentLoader;
        private readonly ICartService _cartService;
        private CartWithValidationIssues _wishlist;
        private CartWithValidationIssues _cart;
        private readonly IOrderRepository _orderRepository;
        private readonly ICommerceTrackingService _trackingService;
        private readonly CartViewModelFactory _cartViewModelFactory;
        private readonly IQuickOrderService _quickOrderService;
        private readonly ReferenceConverter _referenceConverter;
        private readonly ICustomerService _customerService;
        private readonly IUrlResolver _urlResolver;
        private readonly IRelationRepository _relationRepository;
        private readonly LanguageResolver _languageResolver;
        private readonly ICurrentMarket _currentMarket;
        private readonly FilterPublished _filterPublished;
        private readonly ISettingsService _settingsService;

        public WishListController(
            IContentLoader contentLoader,
            ICartService cartService,
            IOrderRepository orderRepository,
            ICommerceTrackingService recommendationService,
            CartViewModelFactory cartViewModelFactory,
            IQuickOrderService quickOrderService,
            ReferenceConverter referenceConverter,
            ICustomerService customerService,
            IUrlResolver urlResolver,
            IRelationRepository relationRepository,
            LanguageResolver languageResolver,
            ICurrentMarket currentMarket,
            FilterPublished filterPublished,
            ISettingsService settingsService)
        {
            _contentLoader = contentLoader;
            _cartService = cartService;
            _orderRepository = orderRepository;
            _trackingService = recommendationService;
            _cartViewModelFactory = cartViewModelFactory;
            _quickOrderService = quickOrderService;
            _referenceConverter = referenceConverter;
            _customerService = customerService;
            _urlResolver = urlResolver;
            _relationRepository = relationRepository;
            _languageResolver = languageResolver;
            _currentMarket = currentMarket;
            _filterPublished = filterPublished;
            _settingsService = settingsService;
        }

        [HttpGet]
        [CommerceTracking(TrackingType.Wishlist)]
        public ActionResult Index(WishListPage currentPage)
        {
            var viewModel = _cartViewModelFactory.CreateWishListViewModel(WishList.Cart, currentPage);
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult WishListMiniCartDetails()
        {
            var viewModel = _cartViewModelFactory.CreateMiniWishListViewModel(WishList.Cart);
            return PartialView("_MiniWishList", viewModel);
        }

        public PartialViewResult LoadWishlistItems()
        {
            var viewModel = _cartViewModelFactory.CreateMiniWishListViewModel(WishList.Cart);
            return PartialView("_MiniWishlistItems", viewModel);
        }

        public PartialViewResult LoadMobileWishlistItems()
        {
            var viewModel = _cartViewModelFactory.CreateMiniWishListViewModel(WishList.Cart);
            return PartialView("_MobileMiniWishlistItems", viewModel);
        }

        [HttpPost]
        public async Task<JsonResult> AddToCart(RequestParamsToCart param) // only use Code
        {
            if (WishList.Cart == null)
            {
                _wishlist = new CartWithValidationIssues
                {
                    Cart = _cartService.LoadOrCreateCart(_cartService.DefaultWishListName),
                    ValidationIssues = new Dictionary<ILineItem, List<ValidationIssue>>()
                };
            }

            // return 0 if the variant already exist in wishlist
            // return 1 if added susscessfully
            var result = new AddToCartResult();
            var allLineItems = WishList.Cart.GetAllLineItems();
            var contentLink = _referenceConverter.GetContentLink(param.Code);
            var message = "";
            var productName = "";
            var entryLink = _referenceConverter.GetContentLink(param.Code);
            productName = _contentLoader.Get<EntryContentBase>(entryLink).DisplayName;

            if (_contentLoader.Get<EntryContentBase>(contentLink) is GenericBundle bundle) // Add bundle
            {
                var variantCodes = _contentLoader
                    .GetItems(bundle.GetEntries(_relationRepository), _languageResolver.GetPreferredCulture())
                    .OfType<VariationContent>()
                    .Where(v => v.IsAvailableInCurrentMarket(_currentMarket) && !_filterPublished.ShouldFilter(v))
                    .Select(x => x.Code);
                var allLineItemCodes = allLineItems.Select(x => x.Code);
                var allNewCodes = variantCodes.Where(x => !allLineItemCodes.Contains(x));
                if (!allNewCodes.Any())
                {
                    return Json(new ChangeCartJsonResult { StatusCode = 0, Message = productName + " already exist in the wishlist." });
                }
                else
                {
                    foreach (var v in allNewCodes)
                    {
                        result = _cartService.AddToCart(WishList.Cart, new RequestParamsToCart { Code = v, Quantity = 1, Store = "delivery", SelectedStore = "" });
                        if (result.ValidationMessages.Count > 0)
                        {
                            message += string.Join("\n", result.ValidationMessages);
                        }
                    }
                }
            }
            else // Add variant
            {
                if (allLineItems.Any(item => item.Code.Equals(param.Code, StringComparison.OrdinalIgnoreCase)))
                {
                    return Json(new ChangeCartJsonResult { StatusCode = 0, Message = productName + " already exist in the wishlist." });
                }

                result = _cartService.AddToCart(WishList.Cart,
                    new RequestParamsToCart { Code = param.Code, Quantity = 1, Store = "delivery", SelectedStore = "" });
            }

            if (result.EntriesAddedToCart)
            {
                _orderRepository.Save(WishList.Cart);
                await _trackingService.TrackWishlist(HttpContext);
                return Json(new ChangeCartJsonResult
                {
                    StatusCode = 1,
                    CountItems = (int)WishList.Cart.GetAllLineItems().Sum(x => x.Quantity),
                    Message = productName + " is added to the wishlist successfully.\n" + message
                });
            }
            return Json(new ChangeCartJsonResult { StatusCode = -1, Message = result.GetComposedValidationMessage() });
        }

        [HttpPost]
        public ActionResult ChangeCartItem(RequestParamsToCart param)
        {
            ModelState.Clear();
            var productName = "";
            var entryLink = _referenceConverter.GetContentLink(param.Code);
            productName = _contentLoader.Get<EntryContentBase>(entryLink).DisplayName;

            if (WishList.Cart.GetAllLineItems().FirstOrDefault(x => x.Code == param.Code) == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent, productName + " is not exist in the wishlist.");
            }

            _cartService.ChangeCartItem(WishList.Cart, 0, param.Code, param.Quantity, param.Size, param.NewSize);
            _orderRepository.Save(WishList.Cart);
            _trackingService.TrackWishlist(HttpContext);
            var referencePages = _settingsService.GetSiteSettings<ReferencePageSettings>();
            WishListPage wishlistPage = null;
            if (!referencePages?.WishlistPage.IsNullOrEmpty() ?? false)
            {
                wishlistPage = _contentLoader.Get<WishListPage>(referencePages.WishlistPage);
            }

            if (param.RequestFrom.Equals("axios", StringComparison.OrdinalIgnoreCase))
            {
                var viewModel = _cartViewModelFactory.CreateWishListViewModel(WishList.Cart, wishlistPage);
                return PartialView("_WishlistListItem", viewModel);
            }

            return Redirect(_urlResolver.GetUrl(wishlistPage));
        }

        [HttpPost]
        public async Task<JsonResult> RemoveWishlistItem(RequestParamsToCart param) // only use Code
        {
            var productName = "";
            var entryLink = _referenceConverter.GetContentLink(param.Code);
            productName = _contentLoader.Get<EntryContentBase>(entryLink).DisplayName;

            if (WishList.Cart.GetAllLineItems().FirstOrDefault(x => x.Code == param.Code) == null)
            {
                return Json(new ChangeCartJsonResult { StatusCode = 0, Message = productName + " is not exist in the wishlist." });
            }

            var result = _cartService.ChangeCartItem(WishList.Cart, 0, param.Code, 0, null, null);
            _orderRepository.Save(WishList.Cart);
            await _trackingService.TrackWishlist(HttpContext);
            if (result.Count > 0)
            {
                return Json(new ChangeCartJsonResult { StatusCode = 0, Message = "Remove " + productName + " error.", CountItems = (int)WishList.Cart.GetAllLineItems().Sum(x => x.Quantity) });
            }

            return Json(new ChangeCartJsonResult { StatusCode = 1, Message = productName + " has removed from the wishlist.", CountItems = (int)WishList.Cart.GetAllLineItems().Sum(x => x.Quantity) });
        }

        [HttpPost]
        public ActionResult DeleteWishList()
        {
            if (WishList.Cart != null)
            {
                _orderRepository.Delete(WishList.Cart.OrderLink);
            }
            var referencePages = _settingsService.GetSiteSettings<ReferencePageSettings>();

            return RedirectToAction("Index", new { Node = referencePages?.WishlistPage ?? ContentReference.StartPage });
        }

        [HttpPost]
        public JsonResult AddVariantsToOrderPad(List<string> variants)
        {
            var returnedMessages = new List<string>();

            ModelState.Clear();

            if (WishList.Cart == null)
            {
                _wishlist = new CartWithValidationIssues
                {
                    Cart = _cartService.LoadOrCreateCart(_cartService.DefaultWishListName),
                    ValidationIssues = new Dictionary<ILineItem, List<ValidationIssue>>()
                };
            }

            foreach (var product in variants)
            {
                var sku = product.Split(';')[0];
                var quantity = Convert.ToInt32(product.Split(';')[1]);

                var variationReference = _referenceConverter.GetContentLink(sku);

                var responseMessage = _quickOrderService.ValidateProduct(variationReference, Convert.ToDecimal(quantity), sku);
                if (string.IsNullOrEmpty(responseMessage))
                {
                    if (_cartService.AddToCart(WishList.Cart, new RequestParamsToCart { Code = sku, Quantity = 1, Store = "delivery", SelectedStore = "" }).EntriesAddedToCart)
                    {
                        _orderRepository.Save(WishList.Cart);
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

        [HttpPost]
        public ActionResult RemoveCartItem(string code, string userId)
        {
            ModelState.Clear();
            var userWishCart = _cartService.LoadWishListCardByCustomerId(new Guid(userId));
            if (userWishCart.GetAllLineItems().Count() == 1)
            {
                _orderRepository.Delete(userWishCart.OrderLink);
            }
            else
            {
                _cartService.ChangeQuantity(userWishCart, 0, code, 0);
                _orderRepository.Save(userWishCart);
            }

            var referencePages = _settingsService.GetSiteSettings<ReferencePageSettings>();
            var pageUrl = _urlResolver.GetUrl(referencePages?.OrganizationOrderPadsPage ?? ContentReference.StartPage);

            return Redirect(pageUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestWishListQuote()
        {
            var currentCustomer = _customerService.GetCurrentContact();
            var referencePages = _settingsService.GetSiteSettings<ReferencePageSettings>();

            var wishListCart = _cartService.LoadWishListCardByCustomerId(currentCustomer.ContactId);
            if (wishListCart != null)
            {
                // Set price on line item.
                foreach (var lineItem in wishListCart.GetAllLineItems())
                {
                    lineItem.PlacedPrice = _cartService.GetDiscountedPrice(wishListCart, lineItem).Value.Amount;
                }

                _cartService.PlaceCartForQuote(wishListCart);
                _cartService.DeleteCart(wishListCart);
                _cartService.LoadOrCreateCart(_cartService.DefaultWishListName);

                return RedirectToAction("Index", "WishList");
            }

            return RedirectToAction("Index", new { Node = referencePages?.OrderHistoryPage ?? ContentReference.StartPage });
        }

        [HttpPost]
        public async Task<ActionResult> AddAllToCart()
        {
            var allLineItem = WishList.Cart.GetAllLineItems();
            var entriesAddedToCart = true;
            var validationMessage = "";

            if (Cart.Cart == null)
            {
                _cart = new CartWithValidationIssues
                {
                    Cart = _cartService.LoadOrCreateCart(_cartService.DefaultCartName),
                    ValidationIssues = new Dictionary<ILineItem, List<ValidationIssue>>()
                };
            }

            foreach (var lineitem in allLineItem)
            {
                var result = _cartService.AddToCart(Cart.Cart,
                    new RequestParamsToCart { Code = lineitem.Code, Quantity = lineitem.Quantity, Store = "delivery", SelectedStore = "", DynamicCodes = lineitem.Properties["VariantOptionCodes"]?.ToString().Split(',').ToList() });
                entriesAddedToCart &= result.EntriesAddedToCart;
                validationMessage += result.GetComposedValidationMessage();
            }

            if (entriesAddedToCart)
            {
                _orderRepository.Save(Cart.Cart);
                await _trackingService.TrackCart(HttpContext, Cart.Cart);
                return Json(new ChangeCartJsonResult
                {
                    StatusCode = 1,
                    Message = "Add all LineItems from the wishlist to the cart.",
                    CountItems = (int)Cart.Cart.GetAllLineItems().Sum(x => x.Quantity),
                });
            }

            return new HttpStatusCodeResult(500, validationMessage);
        }

        private CartWithValidationIssues WishList => _wishlist ?? (_wishlist = _cartService.LoadCart(_cartService.DefaultWishListName, true));
        private CartWithValidationIssues Cart => _cart ?? (_cart = _cartService.LoadCart(_cartService.DefaultCartName, true));
    }
}