using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Foundation.Commerce.Catalog;
using Foundation.Commerce.Extensions;
using Foundation.Commerce.Markets;
using Foundation.Commerce.Models.Catalog;
using Foundation.Commerce.Order.ViewModels;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Order.Services
{
    public class CartItemViewModelFactory
    {
        private readonly IContentLoader _contentLoader;
        private readonly IPricingService _pricingService;
        private readonly UrlResolver _urlResolver;
        private readonly ICurrentMarket _currentMarket;
        private readonly ICurrencyService _currencyService;
        private readonly IPromotionService _promotionService;
        private readonly ILineItemCalculator _lineItemCalculator;
        private readonly IProductService _productService;
        private readonly IRelationRepository _relationRepository;
        readonly ICartService _cartService;

        public CartItemViewModelFactory(
            IContentLoader contentLoader,
            IPricingService pricingService,
            UrlResolver urlResolver,
            ICurrentMarket currentMarket,
            ICurrencyService currencyService,
            IPromotionService promotionService,
            ILineItemCalculator lineItemCalculator,
            IProductService productService,
            IRelationRepository relationRepository,
            ICartService cartService)
        {
            _contentLoader = contentLoader;
            _pricingService = pricingService;
            _urlResolver = urlResolver;
            _currentMarket = currentMarket;
            _currencyService = currencyService;
            _promotionService = promotionService;
            _lineItemCalculator = lineItemCalculator;
            _productService = productService;
            _relationRepository = relationRepository;
            _cartService = cartService;
        }

        public virtual CartItemViewModel CreateCartItemViewModel(ICart cart, ILineItem lineItem, EntryContentBase entry)
        {
            var viewModel = new CartItemViewModel
            {
                Code = lineItem.Code,
                DisplayName = entry.DisplayName,
                ImageUrl = entry.GetAssets<IContentImage>(_contentLoader, _urlResolver).FirstOrDefault() ?? "",
                DiscountedPrice = GetDiscountedPrice(cart, lineItem),
                PlacedPrice = new Money(lineItem.PlacedPrice, _currencyService.GetCurrentCurrency()),
                Quantity = lineItem.Quantity,
                Url = entry.GetUrl(_relationRepository, _urlResolver),
                Entry = entry,
                IsAvailable = _pricingService.GetCurrentPrice(entry.Code).HasValue,
                DiscountedUnitPrice = GetDiscountedUnitPrice(cart, lineItem),
                IsGift = lineItem.IsGift,
                Description = entry["Description"] != null ? entry["Description"].ToString() : ""
            };

            var productLink = entry is VariationContent ?
                entry.GetParentProducts(_relationRepository).FirstOrDefault() :
                entry.ContentLink;

            if (_contentLoader.TryGet(productLink, out
            GenericProduct product))
            {
                viewModel.Brand = GetBrand(product);
            }

            var variant = entry as GenericVariant;
            if (variant != null)
            {
                viewModel.AvailableSizes = GetAvailableSizes(product, variant);
            }
            viewModel.Description = string.IsNullOrEmpty(viewModel.Description) ? viewModel.Description : product.Description.ToHtmlString();
            return viewModel;
        }

        private Money? GetDiscountedUnitPrice(ICart cart, ILineItem lineItem)
        {
            var discountedPrice = GetDiscountedPrice(cart, lineItem) / lineItem.Quantity;
            return discountedPrice.GetValueOrDefault().Amount < lineItem.PlacedPrice ? discountedPrice : null;
        }

        private IEnumerable<string> GetAvailableSizes(GenericProduct product, GenericVariant entry)
        {
            return product != null && entry != null ?
                _productService.GetVariants(product).OfType<GenericVariant>().Where(x => string.IsNullOrEmpty(x.Color) || string.IsNullOrEmpty(entry.Color) || x.Color.Equals(entry.Color))
                .Select(x => x.Size)
                : Enumerable.Empty<string>();
        }

        private string GetBrand(GenericProduct product) => product?.Brand;

        private Money? GetDiscountedPrice(ICart cart, ILineItem lineItem)
        {
            var marketId = _currentMarket.GetCurrentMarket().MarketId;
            var currency = _currencyService.GetCurrentCurrency();
            if (cart.Name.Equals(_cartService.DefaultWishListName))
            {
                var discountedPrice = _promotionService.GetDiscountPrice(new CatalogKey(lineItem.Code), marketId, currency);
                return discountedPrice?.UnitPrice;
            }
            return lineItem.GetDiscountedPrice(cart.Currency, _lineItemCalculator);
        }
    }
}