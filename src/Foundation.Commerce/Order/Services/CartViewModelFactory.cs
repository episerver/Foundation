using EPiServer;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Customer.ViewModels;
using Foundation.Commerce.Markets;
using Foundation.Commerce.Models.Pages;
using Foundation.Commerce.Order.ViewModels;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Security;
using System;
using System.Linq;
using System.Web;

namespace Foundation.Commerce.Order.Services
{
    public class CartViewModelFactory
    {
        private readonly IContentLoader _contentLoader;
        private readonly ICurrencyService _currencyService;
        private readonly IOrderGroupCalculator _orderGroupCalculator;
        private readonly ShipmentViewModelFactory _shipmentViewModelFactory;
        private readonly ReferenceConverter _referenceConverter;
        private readonly UrlResolver _urlResolver;
        private readonly ServiceAccessor<HttpContextBase> _httpContextAccessor;
        private readonly IAddressBookService _addressBookService;

        public CartViewModelFactory(
            IContentLoader contentLoader,
            ICurrencyService currencyService,
            IOrderGroupCalculator orderGroupCalculator,
            ShipmentViewModelFactory shipmentViewModelFactory,
            ReferenceConverter referenceConverter,
            UrlResolver urlResolver,
            ServiceAccessor<HttpContextBase> httpContextAccessor,
            IAddressBookService addressBookService)
        {
            _contentLoader = contentLoader;
            _currencyService = currencyService;
            _orderGroupCalculator = orderGroupCalculator;
            _shipmentViewModelFactory = shipmentViewModelFactory;
            _referenceConverter = referenceConverter;
            _urlResolver = urlResolver;
            _httpContextAccessor = httpContextAccessor;
            _addressBookService = addressBookService;
        }

        public virtual MiniCartViewModel CreateMiniCartViewModel(ICart cart, bool isSharedCart = false)
        {
            var startPage = _contentLoader.Get<PageData>(ContentReference.StartPage) as CommerceHomePage;
            if (cart == null)
            {
                return new MiniCartViewModel
                {
                    ItemCount = 0,
                    CheckoutPage = startPage.CheckoutPage,
                    CartPage = isSharedCart ? startPage.SharedCartPage : startPage.CartPage,
                    Label = isSharedCart ? startPage.SharedCartLabel : startPage.CartLabel,
                    Shipments = Enumerable.Empty<ShipmentViewModel>(),
                    Total = new Money(0, _currencyService.GetCurrentCurrency()),
                    IsSharedCart = isSharedCart
                };
            }

            return new MiniCartViewModel
            {
                ItemCount = GetLineItemsTotalQuantity(cart),
                CheckoutPage = startPage.CheckoutPage,
                CartPage = isSharedCart ? startPage.SharedCartPage : startPage.CartPage,
                Label = isSharedCart ? startPage.SharedCartLabel : startPage.CartLabel,
                Shipments = _shipmentViewModelFactory.CreateShipmentsViewModel(cart),
                Total = _orderGroupCalculator.GetSubTotal(cart),
                IsSharedCart = isSharedCart
            };
        }

        public virtual LargeCartViewModel CreateSimpleLargeCartViewModel(ICart cart)
        {
            if (cart == null)
            {
                var zeroAmount = new Money(0, _currencyService.GetCurrentCurrency());
                return new LargeCartViewModel()
                {
                    TotalDiscount = zeroAmount,
                    Total = zeroAmount,
                    TaxTotal = zeroAmount,
                    ShippingTotal = zeroAmount,
                    Subtotal = zeroAmount,
                };
            }

            var totals = _orderGroupCalculator.GetOrderGroupTotals(cart);
            var orderDiscountTotal = _orderGroupCalculator.GetOrderDiscountTotal(cart);
            var shippingDiscountTotal = cart.GetShippingDiscountTotal();
            var discountTotal = shippingDiscountTotal + orderDiscountTotal;

            var model = new LargeCartViewModel()
            {
                TotalDiscount = discountTotal,
                Total = totals.Total,
                ShippingTotal = totals.ShippingTotal,
                Subtotal = totals.SubTotal,
                TaxTotal = totals.TaxTotal,
                ReferrerUrl = GetReferrerUrl(),
            };

            return model;
        }

        public virtual LargeCartViewModel CreateLargeCartViewModel(ICart cart, CartPage cartPage)
        {
            var startPage = _contentLoader.Get<PageData>(ContentReference.StartPage) as CommerceHomePage;
            _ = _contentLoader.Get<CheckoutPage>(startPage.CheckoutPage);
            var contact = PrincipalInfo.CurrentPrincipal.GetCustomerContact();
            AddressModel addressModel = null;
            if (cart == null)
            {
                var zeroAmount = new Money(0, _currencyService.GetCurrentCurrency());
                addressModel = new AddressModel();
                _addressBookService.LoadCountriesAndRegionsForAddress(addressModel);
                return new LargeCartViewModel(cartPage)
                {
                    Shipments = Enumerable.Empty<ShipmentViewModel>(),
                    TotalDiscount = zeroAmount,
                    Total = zeroAmount,
                    TaxTotal = zeroAmount,
                    ShippingTotal = zeroAmount,
                    Subtotal = zeroAmount,
                    ReferrerUrl = GetReferrerUrl(),
                    CheckoutPage = startPage.CheckoutPage,
                    //MultiShipmentPage = checkoutPage.MultiShipmentPage,
                    AppliedCouponCodes = Enumerable.Empty<string>(),
                    AddressModel = addressModel,
                    ShowRecommendations = true
                };
            }

            var totals = _orderGroupCalculator.GetOrderGroupTotals(cart);
            var orderDiscountTotal = _orderGroupCalculator.GetOrderDiscountTotal(cart);
            var shippingDiscountTotal = cart.GetShippingDiscountTotal();
            var discountTotal = shippingDiscountTotal + orderDiscountTotal;

            var model = new LargeCartViewModel(cartPage)
            {
                Shipments = _shipmentViewModelFactory.CreateShipmentsViewModel(cart),
                TotalDiscount = discountTotal,
                Total = totals.Total,
                ShippingTotal = totals.ShippingTotal,
                Subtotal = totals.SubTotal,
                TaxTotal = totals.TaxTotal,
                ReferrerUrl = GetReferrerUrl(),
                CheckoutPage = startPage.CheckoutPage,
                //MultiShipmentPage = checkoutPage.MultiShipmentPage,
                AppliedCouponCodes = cart.GetFirstForm().CouponCodes.Distinct(),
                HasOrganization = contact?.OwnerId != null,
                ShowRecommendations = cartPage != null ? cartPage.ShowRecommendations : true
            };

            var shipment = model.Shipments.FirstOrDefault();
            addressModel = shipment?.Address ?? new AddressModel();
            _addressBookService.LoadCountriesAndRegionsForAddress(addressModel);
            model.AddressModel = addressModel;

            return model;
        }

        public virtual WishListViewModel CreateWishListViewModel(ICart cart, WishListPage wishListPage)
        {
            if (cart == null)
            {
                return new WishListViewModel(wishListPage)
                {
                    ItemCount = 0,
                    CartItems = Array.Empty<CartItemViewModel>(),
                    Total = new Money(0, _currencyService.GetCurrentCurrency())
                };
            }

            var contact = PrincipalInfo.CurrentPrincipal.GetCustomerContact();
            return new WishListViewModel(wishListPage)
            {
                ItemCount = GetLineItemsTotalQuantity(cart),
                CartItems = _shipmentViewModelFactory.CreateShipmentsViewModel(cart).SelectMany(x => x.CartItems),
                Total = _orderGroupCalculator.GetSubTotal(cart),
                HasOrganization = contact?.OwnerId != null
            };
        }

        public virtual MiniWishlistViewModel CreateMiniWishListViewModel(ICart cart)
        {
            var startPage = _contentLoader.Get<CommerceHomePage>(ContentReference.StartPage);
            var contact = PrincipalInfo.CurrentPrincipal.GetCustomerContact();
            if (cart == null)
            {
                return new MiniWishlistViewModel
                {
                    ItemCount = 0,
                    Items = Array.Empty<CartItemViewModel>(),
                    WishlistPage = startPage.WishlistPage,
                    HasOrganization = contact?.OwnerId != null,
                    Label = startPage.WishlistLabel,
                };
            }

            return new MiniWishlistViewModel
            {
                ItemCount = GetLineItemsTotalQuantity(cart),
                Items = _shipmentViewModelFactory.CreateShipmentsViewModel(cart).SelectMany(x => x.CartItems),
                WishlistPage = startPage.WishlistPage,
                Total = _orderGroupCalculator.GetSubTotal(cart),
                Label = startPage.WishlistLabel,
                HasOrganization = contact?.OwnerId != null
            };
        }

        public virtual WishListMiniCartViewModel CreateWishListMiniCartViewModel(ICart cart)
        {
            var wishListLink = (_contentLoader.Get<PageData>(ContentReference.StartPage) as CommerceHomePage)?.WishlistPage;
            var wishListPage = wishListLink != null ? _contentLoader.Get<WishListPage>(wishListLink) : null;
            var contact = PrincipalInfo.CurrentPrincipal.GetCustomerContact();
            if (cart == null && wishListPage != null)
            {
                return new WishListMiniCartViewModel(wishListPage)
                {
                    ItemCount = 0,
                    WishListPage = wishListLink,
                    CartItems = Array.Empty<CartItemViewModel>(),
                    Total = new Money(0, _currencyService.GetCurrentCurrency()),
                    HasOrganization = contact?.OwnerId != null
                };
            }

            if (wishListPage != null)
            {
                return new WishListMiniCartViewModel(wishListPage)
                {
                    ItemCount = GetLineItemsTotalQuantity(cart),
                    WishListPage = wishListLink,
                    CartItems = _shipmentViewModelFactory.CreateShipmentsViewModel(cart).SelectMany(x => x.CartItems),
                    Total = _orderGroupCalculator.GetSubTotal(cart),
                    HasOrganization = contact?.OwnerId != null
                };
            }
            else
            {
                return null;
            }
        }

        public virtual SharedCartViewModel CreateSharedCartViewModel(ICart cart, SharedCartPage sharedCartPage)
        {
            if (cart == null)
            {
                return new SharedCartViewModel(sharedCartPage)
                {
                    ItemCount = 0,
                    CartItems = Array.Empty<CartItemViewModel>(),
                    Total = new Money(0, _currencyService.GetCurrentCurrency())
                };
            }

            var contact = PrincipalInfo.CurrentPrincipal.GetCustomerContact();
            return new SharedCartViewModel(sharedCartPage)
            {
                ItemCount = GetLineItemsTotalQuantity(cart),
                CartItems = _shipmentViewModelFactory.CreateShipmentsViewModel(cart).SelectMany(x => x.CartItems),
                Total = _orderGroupCalculator.GetSubTotal(cart),
                HasOrganization = contact?.OwnerId != null
            };
        }

        public virtual SharedMiniCartViewModel CreateSharedMiniCartViewModel(ICart cart)
        {
            var sharedCartLink = (_contentLoader.Get<PageData>(ContentReference.StartPage) as CommerceHomePage)?.SharedCartPage;
            var sharedCartPage = sharedCartLink != null ? _contentLoader.Get<SharedCartPage>(sharedCartLink) : null;
            if (cart == null && sharedCartPage != null)
            {
                return new SharedMiniCartViewModel(sharedCartPage)
                {
                    ItemCount = 0,
                    SharedCartPage = sharedCartLink,
                    CartItems = Array.Empty<CartItemViewModel>(),
                    Total = new Money(0, _currencyService.GetCurrentCurrency())
                };
            }

            if (sharedCartPage != null)
            {
                return new SharedMiniCartViewModel(sharedCartPage)
                {
                    ItemCount = GetLineItemsTotalQuantity(cart),
                    SharedCartPage = sharedCartLink,
                    CartItems = _shipmentViewModelFactory.CreateShipmentsViewModel(cart).SelectMany(x => x.CartItems),
                    Total = _orderGroupCalculator.GetSubTotal(cart)
                };
            }
            else
            {
                return null;
            }
        }

        private decimal GetLineItemsTotalQuantity(ICart cart)
        {
            if (cart != null)
            {
                var cartItems = cart
                .GetAllLineItems()
                .Where(c => !ContentReference.IsNullOrEmpty(_referenceConverter.GetContentLink(c.Code)));
                return cartItems.Sum(x => x.Quantity);
            }
            else
            {
                return 0;
            }
        }

        private string GetReferrerUrl()
        {
            var httpContext = _httpContextAccessor();
            if (httpContext.Request.UrlReferrer != null &&
                httpContext.Request.UrlReferrer.Host.Equals(httpContext.Request.Url.Host, StringComparison.OrdinalIgnoreCase))
            {
                return httpContext.Request.UrlReferrer.ToString();
            }

            return _urlResolver.GetUrl(ContentReference.StartPage);
        }
    }
}