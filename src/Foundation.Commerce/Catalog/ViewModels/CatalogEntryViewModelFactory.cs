using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Globalization;
using EPiServer.PdfPreview.Models;
using EPiServer.Security;
using EPiServer.Web.Routing;
using Foundation.Cms.Media;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Extensions;
using Foundation.Commerce.Markets;
using Foundation.Commerce.Models.Catalog;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Pricing;
using Mediachase.Commerce.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Commerce.Catalog.ViewModels
{
    public class CatalogEntryViewModelFactory
    {
        private readonly IPromotionService _promotionService;
        private readonly IContentLoader _contentLoader;
        private readonly IPriceService _priceService;
        private readonly ICurrentMarket _currentMarket;
        private readonly ICurrencyService _currencyservice;
        private readonly IRelationRepository _relationRepository;
        private readonly UrlResolver _urlResolver;
        private readonly FilterPublished _filterPublished;
        private readonly LanguageResolver _languageResolver;
        private readonly IStoreService _storeService;
        private readonly IProductService _productService;
        private readonly IQuickOrderService _quickOrderService;

        public CatalogEntryViewModelFactory(
            IPromotionService promotionService,
            IContentLoader contentLoader,
            IPriceService priceService,
            ICurrentMarket currentMarket,
            CurrencyService currencyservice,
            IRelationRepository relationRepository,
            UrlResolver urlResolver,
            FilterPublished filterPublished,
            LanguageResolver languageResolver,
            IStoreService storeService,
            IProductService productService,
            IQuickOrderService quickOrderService)
        {
            _promotionService = promotionService;
            _contentLoader = contentLoader;
            _priceService = priceService;
            _currentMarket = currentMarket;
            _currencyservice = currencyservice;
            _relationRepository = relationRepository;
            _urlResolver = urlResolver;
            _filterPublished = filterPublished;
            _languageResolver = languageResolver;
            _storeService = storeService;
            _productService = productService;
            _quickOrderService = quickOrderService;
        }

        public virtual TViewModel Create<TProduct, TVariant, TViewModel>(TProduct currentContent, string variationCode)
            where TProduct : ProductContent
            where TVariant : VariationContent
            where TViewModel : ProductViewModelBase<TProduct, TVariant>, new()
        {
            var variants = GetVariants<TVariant, TProduct>(currentContent)
                .Where(v => v.Prices().Any(x => x.MarketId == _currentMarket.GetCurrentMarket().MarketId))
                .ToList();

            if (!TryGetVariant(variants, variationCode, out var variant))
            {
                return new TViewModel
                {
                    Product = currentContent,
                    CurrentContent = currentContent,
                    Images = currentContent.GetAssets<IContentImage>(_contentLoader, _urlResolver),
                    Colors = new List<SelectListItem>(),
                    Sizes = new List<SelectListItem>(),
                    StaticAssociations = new List<ProductTileViewModel>(),
                    Variants = new List<VariantViewModel>()
                };
            }

            var market = _currentMarket.GetCurrentMarket();
            var currency = _currencyservice.GetCurrentCurrency();
            var defaultPrice = PriceCalculationService.GetSalePrice(variant.Code, market.MarketId, currency);
            var subscriptionPrice = PriceCalculationService.GetSubscriptionPrice(variant.Code, market.MarketId, currency);
            var discountedPrice = GetDiscountPrice(defaultPrice, market, currency);
            var currentStore = _storeService.GetCurrentStoreViewModel();
            var relatedProducts = currentContent.GetRelatedEntries().ToList();
            var associations = relatedProducts.Any() ?
                _productService.GetProductTileViewModels(relatedProducts) :
                new List<ProductTileViewModel>();
            var contact = PrincipalInfo.CurrentPrincipal.GetCustomerContact();
            var baseVariant = variant as GenericVariant;
            var productRecommendations = currentContent as IProductRecommendations;
            var isSalesRep = PrincipalInfo.CurrentPrincipal.IsInRole("SalesRep");

            return new TViewModel
            {
                CurrentContent = currentContent,
                Product = currentContent,
                Variant = variant,
                ListingPrice = defaultPrice?.UnitPrice ?? new Money(0, currency),
                DiscountedPrice = discountedPrice,
                SubscriptionPrice = subscriptionPrice?.UnitPrice ?? new Money(0, currency),
                Colors = variants.OfType<GenericVariant>()
                    .Where(x => x.Color != null)
                    .GroupBy(x => x.Color)
                    .Select(g => new SelectListItem
                    {
                        Selected = false,
                        Text = g.Key,
                        Value = g.Key
                    }).ToList(),
                Sizes = variants.OfType<GenericVariant>()
                    .Where(x => (x.Color == null || x.Color.Equals(baseVariant?.Color, StringComparison.OrdinalIgnoreCase)) && x.Size != null)
                    .Select(x => new SelectListItem
                    {
                        Selected = false,
                        Text = x.Size,
                        Value = x.Size
                    }).ToList(),
                Color = baseVariant?.Color,
                Size = baseVariant?.Size,
                Images = variant.GetAssets<IContentImage>(_contentLoader, _urlResolver),
                IsAvailable = defaultPrice != null,
                Stores = new StoreViewModel
                {
                    Stores = _storeService.GetEntryStoresViewModels(variant.Code),
                    SelectedStore = currentStore != null ? currentStore.Code : "",
                    SelectedStoreName = currentStore != null ? currentStore.Name : ""
                },
                StaticAssociations = associations,
                Variants = variants.Select(x =>
                {
                    var variantImage = x.GetAssets<IContentImage>(_contentLoader, _urlResolver).FirstOrDefault();
                    var variantDefaultPrice = GetDefaultPrice(x.Code, market, currency);
                    return new VariantViewModel
                    {
                        Sku = x.Code,
                        Size = x is GenericVariant ? $"{(x as GenericVariant).Color} {(x as GenericVariant).Size}" : "",
                        ImageUrl = string.IsNullOrEmpty(variantImage) ? "http://placehold.it/54x54/" : variantImage,
                        DiscountedPrice = GetDiscountPrice(variantDefaultPrice, market, currency),
                        ListingPrice = variantDefaultPrice?.UnitPrice ?? new Money(0, currency),
                        StockQuantity = _quickOrderService.GetTotalInventoryByEntry(x.Code)
                    };
                }).ToList(),
                HasOrganization = contact?.OwnerId != null,
                ShowRecommendations = productRecommendations?.ShowRecommendations ?? true,
                IsSalesRep = isSalesRep,
                SalesMaterials = isSalesRep ? currentContent.CommerceMediaCollection.Where(x => !string.IsNullOrEmpty(x.GroupName) && x.GroupName.Equals("sales"))
                    .Select(x => _contentLoader.Get<MediaData>(x.AssetLink)).ToList() : new List<MediaData>(),
                Documents = currentContent.CommerceMediaCollection
                    .Where(o => o.AssetType.Equals(typeof(PdfFile).FullName.ToLowerInvariant()) || o.AssetType.Equals(typeof(StandardFile).FullName.ToLowerInvariant()))
                    .Select(x => _contentLoader.Get<MediaData>(x.AssetLink)).ToList()
            };
        }

        public virtual TViewModel CreateBundle<TBundle, TVariant, TViewModel>(TBundle currentContent)
            where TBundle : BundleContent
            where TVariant : VariationContent
            where TViewModel : BundleViewModelBase<TBundle>, new()
        {
            var relatedProducts = currentContent.GetRelatedEntries().ToList();
            var associations = relatedProducts.Any() ?
                _productService.GetProductTileViewModels(relatedProducts) :
                new List<ProductTileViewModel>();
            var variants = GetVariants<TVariant, TBundle>(currentContent).ToList();
            var currentStore = _storeService.GetCurrentStoreViewModel();
            var contact = PrincipalInfo.CurrentPrincipal.GetCustomerContact();
            var productRecommendations = currentContent as IProductRecommendations;
            var isSalesRep = PrincipalInfo.CurrentPrincipal.IsInRole("SalesRep");

            return new TViewModel
            {
                CurrentContent = currentContent,
                Bundle = currentContent,
                Images = currentContent.GetAssets<IContentImage>(_contentLoader, _urlResolver),
                Entries = variants,
                Stores = new StoreViewModel
                {
                    Stores = _storeService.GetEntryStoresViewModels(currentContent.Code),
                    SelectedStore = currentStore != null ? currentStore.Code : "",
                    SelectedStoreName = currentStore != null ? currentStore.Name : ""
                },
                StaticAssociations = associations,
                HasOrganization = contact?.OwnerId != null,
                ShowRecommendations = productRecommendations?.ShowRecommendations ?? true,
                IsSalesRep = isSalesRep,
                SalesMaterials = isSalesRep ? currentContent.CommerceMediaCollection.Where(x => !string.IsNullOrEmpty(x.GroupName) && x.GroupName.Equals("sales"))
                    .Select(x => _contentLoader.Get<MediaData>(x.AssetLink)).ToList() : new List<MediaData>()
            };
        }


        public virtual TViewModel CreateVariant<TVariant, TViewModel>(TVariant currentContent)
            where TVariant : VariationContent
            where TViewModel : EntryViewModelBase<TVariant>, new()
        {

            var market = _currentMarket.GetCurrentMarket();
            var currency = _currencyservice.GetCurrentCurrency();
            var defaultPrice = PriceCalculationService.GetSalePrice(currentContent.Code, market.MarketId, currency);
            var subscriptionPrice = PriceCalculationService.GetSubscriptionPrice(currentContent.Code, market.MarketId, currency);
            var discountedPrice = GetDiscountPrice(defaultPrice, market, currency);
            var currentStore = _storeService.GetCurrentStoreViewModel();
            var relatedProducts = currentContent.GetRelatedEntries().ToList();
            var associations = relatedProducts.Any() ?
                _productService.GetProductTileViewModels(relatedProducts) :
                new List<ProductTileViewModel>();
            var contact = PrincipalInfo.CurrentPrincipal.GetCustomerContact();
            var productRecommendations = currentContent as IProductRecommendations;
            var isSalesRep = PrincipalInfo.CurrentPrincipal.IsInRole("SalesRep");

            return new TViewModel
            {
                CurrentContent = currentContent,
                ListingPrice = defaultPrice?.UnitPrice ?? new Money(0, currency),
                DiscountedPrice = discountedPrice,
                SubscriptionPrice = subscriptionPrice?.UnitPrice ?? new Money(0, currency),
                Images = currentContent.GetAssets<IContentImage>(_contentLoader, _urlResolver),
                IsAvailable = defaultPrice != null,
                Stores = new StoreViewModel
                {
                    Stores = _storeService.GetEntryStoresViewModels(currentContent.Code),
                    SelectedStore = currentStore != null ? currentStore.Code : "",
                    SelectedStoreName = currentStore != null ? currentStore.Name : ""
                },
                StaticAssociations = associations,
                HasOrganization = contact?.OwnerId != null,
                ShowRecommendations = productRecommendations?.ShowRecommendations ?? true,
                IsSalesRep = isSalesRep,
                SalesMaterials = isSalesRep ? currentContent.CommerceMediaCollection.Where(x => !string.IsNullOrEmpty(x.GroupName) && x.GroupName.Equals("sales"))
                    .Select(x => _contentLoader.Get<MediaData>(x.AssetLink)).ToList() : new List<MediaData>()
            };
        }

        public virtual TViewModel CreatePackage<TPackage, TVariant, TViewModel>(TPackage currentContent)
            where TPackage : PackageContent
            where TVariant : VariationContent
            where TViewModel : PackageViewModelBase<TPackage>, new()
        {
            var variants = GetVariants<TVariant, TPackage>(currentContent).ToList();
            var market = _currentMarket.GetCurrentMarket();
            var currency = _currencyservice.GetCurrentCurrency();
            var defaultPrice = PriceCalculationService.GetSalePrice(currentContent.Code, market.MarketId, currency);
            var subscriptionPrice = PriceCalculationService.GetSubscriptionPrice(currentContent.Code, market.MarketId, currency);
            var currentStore = _storeService.GetCurrentStoreViewModel();
            var relatedProducts = currentContent.GetRelatedEntries().ToList();
            var associations = relatedProducts.Any() ?
                _productService.GetProductTileViewModels(relatedProducts) :
                new List<ProductTileViewModel>();
            var contact = PrincipalInfo.CurrentPrincipal.GetCustomerContact();
            var productRecommendations = currentContent as IProductRecommendations;
            var isSalesRep = PrincipalInfo.CurrentPrincipal.IsInRole("SalesRep");

            return new TViewModel
            {
                CurrentContent = currentContent,
                Package = currentContent,
                ListingPrice = defaultPrice?.UnitPrice ?? new Money(0, currency),
                DiscountedPrice = GetDiscountPrice(defaultPrice, market, currency),
                SubscriptionPrice = subscriptionPrice?.UnitPrice ?? new Money(0, currency),
                Images = currentContent.GetAssets<IContentImage>(_contentLoader, _urlResolver),
                IsAvailable = defaultPrice != null,
                Entries = variants,
                //Reviews = GetReviews(currentContent.Code),
                Stores = new StoreViewModel
                {
                    Stores = _storeService.GetEntryStoresViewModels(currentContent.Code),
                    SelectedStore = currentStore != null ? currentStore.Code : "",
                    SelectedStoreName = currentStore != null ? currentStore.Name : ""
                },
                StaticAssociations = associations,
                HasOrganization = contact?.OwnerId != null,
                ShowRecommendations = productRecommendations?.ShowRecommendations ?? true,
                IsSalesRep = isSalesRep,
                SalesMaterials = isSalesRep ? currentContent.CommerceMediaCollection.Where(x => !string.IsNullOrEmpty(x.GroupName) && x.GroupName.Equals("sales"))
                    .Select(x => _contentLoader.Get<MediaData>(x.AssetLink)).ToList() : new List<MediaData>()
            };
        }

        public virtual GenericVariant SelectVariant(GenericProduct currentContent, string color, string size)
        {
            var variants = GetVariants<GenericVariant, GenericProduct>(currentContent);

            if (TryGetFashionVariantByColorAndSize(variants, color, size, out var variant)
                || TryGetFashionVariantByColorAndSize(variants, color, string.Empty, out variant))//if we cannot find variation with exactly both color and size then we will try to get variant by color only
            {
                return variant;
            }

            return null;
        }

        private IEnumerable<TVariant> GetVariants<TVariant, TEntryBase>(TEntryBase currentContent) where TVariant : VariationContent where TEntryBase : EntryContentBase
        {
            var bundle = currentContent as BundleContent;
            if (bundle != null)
            {
                return _contentLoader
                    .GetItems(bundle.GetEntries(_relationRepository), _languageResolver.GetPreferredCulture())
                    .OfType<TVariant>()
                    .Where(v => v.IsAvailableInCurrentMarket(_currentMarket) && !_filterPublished.ShouldFilter(v))
                    .ToArray();
            }

            var package = currentContent as PackageContent;
            if (package != null)
            {
                return _contentLoader
                    .GetItems(package.GetEntries(_relationRepository), _languageResolver.GetPreferredCulture())
                    .OfType<TVariant>()
                    .Where(v => v.IsAvailableInCurrentMarket(_currentMarket) && !_filterPublished.ShouldFilter(v))
                    .ToArray();
            }

            var product = currentContent as ProductContent;
            if (product != null)
            {
                return _contentLoader
                    .GetItems(product.GetVariants(_relationRepository), _languageResolver.GetPreferredCulture())
                    .OfType<TVariant>()
                    .Where(v => v.IsAvailableInCurrentMarket(_currentMarket) && !_filterPublished.ShouldFilter(v))
                    .ToArray();
            }

            return Enumerable.Empty<TVariant>();
        }

        private static bool TryGetVariant<T>(IEnumerable<T> variations, string variationCode, out T variation) where T : VariationContent
        {
            variation = !string.IsNullOrEmpty(variationCode) ?
                variations.FirstOrDefault(x => x.Code == variationCode) :
                variations.FirstOrDefault();

            return variation != null;
        }

        private IPriceValue GetDefaultPrice(string entryCode, IMarket market, Currency currency)
        {
            return _priceService.GetDefaultPrice(
                market.MarketId,
                DateTime.Now,
                new CatalogKey(entryCode),
                currency);
        }

        private Money? GetDiscountPrice(IPriceValue defaultPrice, IMarket market, Currency currency)
        {
            if (defaultPrice == null)
            {
                return null;
            }

            return _promotionService.GetDiscountPrice(defaultPrice.CatalogKey, market.MarketId, currency).UnitPrice;
        }

        private static bool TryGetFashionVariantByColorAndSize(IEnumerable<GenericVariant> variants, string color, string size, out GenericVariant variant)
        {
            variant = variants.FirstOrDefault(x =>
                (string.IsNullOrEmpty(color) || x.Color.Equals(color, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(size) || x.Size.Equals(size, StringComparison.OrdinalIgnoreCase)));

            return variant != null;
        }
    }
}