using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Tracking.Commerce;
using EPiServer.Web.Mvc;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Models.Pages;
using Foundation.Commerce.Order.Services;
using Foundation.Commerce.Order.ViewModels;
using Mediachase.Commerce.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.NamedCarts.SharedCart
{
    [Authorize]
    public class SharedCartController : PageController<SharedCartPage>
    {
        private readonly IContentLoader _contentLoader;
        private readonly ICartService _cartService;
        private CartWithValidationIssues _sharedCart;
        private readonly IOrderRepository _orderRepository;
        readonly CartViewModelFactory _cartViewModelFactory;
        private readonly ICustomerService _customerService;
        private readonly ReferenceConverter _referenceConverter;

        public SharedCartController(
            IContentLoader contentLoader,
            ICartService cartService,
            IOrderRepository orderRepository,
            CartViewModelFactory cartViewModelFactory,
            ICustomerService customerService,
            ReferenceConverter referenceConverter)
        {
            _contentLoader = contentLoader;
            _cartService = cartService;
            _orderRepository = orderRepository;
            _cartViewModelFactory = cartViewModelFactory;
            _customerService = customerService;
            _referenceConverter = referenceConverter;
        }

        [HttpGet]
        [CommerceTracking(TrackingType.Other)]
        public ActionResult Index(SharedCartPage currentPage)
        {
            var viewModel = _cartViewModelFactory.CreateSharedCartViewModel(SharedCart.Cart, currentPage);
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult LoadMiniSharedCart()
        {
            var viewModel = _cartViewModelFactory.CreateMiniCartViewModel(SharedCart.Cart, true);
            return PartialView("~/Features/Shared/Foundation/Header/_MiniSharedCartItems.cshtml", viewModel);
        }

        public PartialViewResult LoadMobileSharedCartItems()
        {
            var viewModel = _cartViewModelFactory.CreateMiniCartViewModel(SharedCart.Cart);
            return PartialView("_MobileMiniSharedCartItems", viewModel);
        }

        [HttpPost]
        public ActionResult AddToCart(RequestParamsToCart param)
        {
            ModelState.Clear();

            if (SharedCart.Cart == null)
            {
                _sharedCart = new CartWithValidationIssues
                {
                    Cart = _cartService.LoadOrCreateCart(_cartService.DefaultSharedCartName, OrganizationId),
                    ValidationIssues = new Dictionary<ILineItem, List<ValidationIssue>>()
                };
            }

            var result = _cartService.AddToCart(SharedCart.Cart, param.Code, param.Quantity, "delivery", "");
            if (result.EntriesAddedToCart)
            {
                _orderRepository.Save(SharedCart.Cart);
                var productName = "";
                var entryLink = _referenceConverter.GetContentLink(param.Code);
                productName = _contentLoader.Get<EntryContentBase>(entryLink).DisplayName;

                return Json(new ChangeCartJsonResult
                {
                    StatusCode = 1,
                    CountItems = (int)SharedCart.Cart.GetAllLineItems().Sum(x => x.Quantity),
                    Message = productName + " is added to the shared cart  successfully."
                });
            }

            return Json(new ChangeCartJsonResult { StatusCode = -1, Message = result.GetComposedValidationMessage() });
        }

        [HttpPost]
        public ActionResult ChangeCartItem(RequestParamsToCart param)
        {
            ModelState.Clear();

            _cartService.ChangeCartItem(SharedCart.Cart, 0, param.Code, param.Quantity, param.Size, param.NewSize);
            _orderRepository.Save(SharedCart.Cart);

            var productName = "";
            var entryLink = _referenceConverter.GetContentLink(param.Code);
            productName = _contentLoader.Get<EntryContentBase>(entryLink).DisplayName;

            return Json(new ChangeCartJsonResult
            {
                StatusCode = 1,
                CountItems = (int)SharedCart.Cart.GetAllLineItems().Sum(x => x.Quantity),
                Message = productName + " is added to the shared cart successfully."
            });
        }

        [HttpPost]
        public ActionResult DeleteSharedCart()
        {
            if (SharedCart.Cart != null)
            {
                _orderRepository.Delete(SharedCart.Cart.OrderLink);
            }
            var startPage = _contentLoader.Get<CommerceHomePage>(ContentReference.StartPage);

            return RedirectToAction("Index", new { Node = startPage.SharedCartPage });
        }

        [HttpPost]
        public ActionResult RemoveCartItem(RequestParamsToCart param)
        {
            ModelState.Clear();
            var organizationId = param.OrganizationId;
            if (string.IsNullOrEmpty(organizationId))
            {
                organizationId = OrganizationId.ToString();
            }

            var userWishCart = _cartService.LoadSharedCardByCustomerId(new Guid(organizationId));
            if (userWishCart.GetAllLineItems().Count() == 1)
            {
                _orderRepository.Delete(userWishCart.OrderLink);
            }
            else
            {
                _cartService.ChangeQuantity(userWishCart, -1, param.Code, 0);
                _orderRepository.Save(userWishCart);
            }

            var productName = "";
            var entryLink = _referenceConverter.GetContentLink(param.Code);
            productName = _contentLoader.Get<EntryContentBase>(entryLink).DisplayName;

            return Json(new ChangeCartJsonResult
            {
                StatusCode = 1,
                CountItems = (int)SharedCart.Cart.GetAllLineItems().Sum(x => x.Quantity),
                Message = productName + " is removed from the shared cart successfully."
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestSharedCartQuote()
        {
            var currentCustomer = _customerService.GetCurrentContact();
            var startPage = _contentLoader.Get<CommerceHomePage>(ContentReference.StartPage);

            var sharedCart = _cartService.LoadSharedCardByCustomerId(new Guid(OrganizationId));
            var savedCart = _cartService.LoadOrCreateCart(_cartService.DefaultSharedCartName, currentCustomer.ContactId.ToString());

            //clone all items in shared cart to savedCart 
            var allLineItem = sharedCart.GetAllLineItems();
            foreach (var lineItem in allLineItem)
            {
                _cartService.AddToCart(savedCart, lineItem.Code, lineItem.Quantity, "delivery", "");
            }

            //Used saved cart to place
            if (savedCart != null)
            {
                // Set price on line item.
                foreach (var lineItem in savedCart.GetAllLineItems())
                {
                    lineItem.PlacedPrice = _cartService.GetDiscountedPrice(savedCart, lineItem).Value.Amount;
                }

                _cartService.PlaceCartForQuote(savedCart);
                _cartService.DeleteCart(savedCart);
                _cartService.DeleteCart(sharedCart);
                _cartService.LoadOrCreateCart(_cartService.DefaultSharedCartName, OrganizationId);

                return RedirectToAction("Index", "SharedCart");
            }

            return RedirectToAction("Index", new { Node = startPage.OrderHistoryPage });
        }

        private CartWithValidationIssues SharedCart => _sharedCart ?? (_sharedCart = _cartService.LoadCart(_cartService.DefaultSharedCartName, OrganizationId, true));
        private string OrganizationId => _customerService.GetCurrentContact().FoundationOrganization?.OrganizationId.ToString();
    }
}