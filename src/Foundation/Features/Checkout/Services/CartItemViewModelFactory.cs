using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Foundation.Commerce.Extensions;
using Foundation.Commerce.Markets;
using Foundation.Features.CatalogContent.Product;
using Foundation.Features.CatalogContent.Services;
using Foundation.Features.CatalogContent.Variation;
using Foundation.Features.Checkout.ViewModels;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.Checkout.Services
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
        private readonly ICartService _cartService;

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
            var basePrice = lineItem.Properties["BasePrice"] != null ? decimal.Parse(lineItem.Properties["BasePrice"].ToString()) : 0;
            var optionPrice = lineItem.Properties["OptionPrice"] != null ? decimal.Parse(lineItem.Properties["OptionPrice"].ToString()) : 0;
            var viewModel = new CartItemViewModel
            {
                Code = lineItem.Code,
                DisplayName = lineItem.DisplayName,
                ImageUrl = entry.GetAssets<IContentImage>(_contentLoader, _urlResolver).FirstOrDefault() ?? "",
                DiscountedPrice = GetDiscountedPrice(cart, lineItem),
                BasePrice = new Money(basePrice, _currencyService.GetCurrentCurrency()),
                OptionPrice = new Money(optionPrice, _currencyService.GetCurrentCurrency()),
                PlacedPrice = new Money(lineItem.PlacedPrice, _currencyService.GetCurrentCurrency()),
                Quantity = lineItem.Quantity,
                Url = entry.GetUrl(_relationRepository, _urlResolver),
                Entry = entry,
                IsAvailable = _pricingService.GetCurrentPrice(entry.Code).HasValue,
                DiscountedUnitPrice = GetDiscountedUnitPrice(cart, lineItem),
                IsGift = lineItem.IsGift,
                Description = entry["Description"] != null ? entry["Description"].ToString() : "",
                IsDynamicProduct = lineItem.Properties["VariantOptionCodes"] != null
            };

            var productLink = entry is VariationContent ?
                entry.GetParentProducts(_relationRepository).FirstOrDefault() :
                entry.ContentLink;

            if (_contentLoader.TryGet(productLink, out EntryContentBase catalogContent))
            {
                var product = catalogContent as GenericProduct;
                if (product != null)
                {
                    viewModel.Brand = GetBrand(product);
                    var variant = entry as GenericVariant;
                    if (variant != null)
                    {
                        viewModel.AvailableSizes = GetAvailableSizes(product, variant);
                    }
                }
            }

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