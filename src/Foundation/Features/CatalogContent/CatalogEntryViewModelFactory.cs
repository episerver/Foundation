using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Commerce.SpecializedProperties;
using EPiServer.Core;
using EPiServer.Data;
using EPiServer.Filters;
using EPiServer.Globalization;
using EPiServer.PdfPreview.Models;
using EPiServer.Security;
using EPiServer.Web.Routing;
using Foundation.Commerce.Extensions;
using Foundation.Commerce.Markets;
using Foundation.Features.CatalogContent.Bundle;
using Foundation.Features.CatalogContent.Package;
using Foundation.Features.CatalogContent.Product;
using Foundation.Features.CatalogContent.Services;
using Foundation.Features.CatalogContent.Variation;
using Foundation.Features.Media;
using Foundation.Features.Stores;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Inventory;
using Mediachase.Commerce.InventoryService;
using Mediachase.Commerce.Pricing;
using Mediachase.Commerce.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.CatalogContent
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
        private readonly IInventoryService _inventoryService;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IDatabaseMode _databaseMode;

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
            IQuickOrderService quickOrderService,
            IInventoryService inventoryService,
            IWarehouseRepository warehouseRepository,
            IDatabaseMode databaseMode)
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
            _inventoryService = inventoryService;
            _warehouseRepository = warehouseRepository;
            _databaseMode = databaseMode;
        }

        public virtual TViewModel Create<TProduct, TVariant, TViewModel>(TProduct currentContent, string variationCode)
            where TProduct : ProductContent
            where TVariant : VariationContent
            where TViewModel : ProductViewModelBase<TProduct, TVariant>, new()
        {
            var viewModel = new TViewModel();
            var market = _currentMarket.GetCurrentMarket();
            var currency = _currencyservice.GetCurrentCurrency();
            var variants = GetVariants<TVariant, TProduct>(currentContent)
                .Where(v => v.Prices().Any(x => x.MarketId == _currentMarket.GetCurrentMarket().MarketId))
                .ToList();
            var variantsState = GetVarantsState(variants, market);
            if (!TryGetVariant(variants, variationCode, out var variant))
            {
                return new TViewModel
                {
                    Product = currentContent,
                    CurrentContent = currentContent,
                    Images = currentContent.GetAssets<IContentImage>(_contentLoader, _urlResolver),
                    Media = currentContent.GetAssetsWithType(_contentLoader, _urlResolver),
                    Colors = new List<SelectListItem>(),
                    Sizes = new List<SelectListItem>(),
                    StaticAssociations = new List<ProductTileViewModel>(),
                    Variants = new List<VariantViewModel>()
                };
            }

            variationCode = string.IsNullOrEmpty(variationCode) ? variants.FirstOrDefault()?.Code : variationCode;
            var isInstock = true;
            var currentWarehouse = _warehouseRepository.GetDefaultWarehouse();
            if (!string.IsNullOrEmpty(variationCode))
            {
                var inStockQuantity = GetAvailableStockQuantity(variant, currentWarehouse);
                isInstock = inStockQuantity > 0;
                viewModel.InStockQuantity = inStockQuantity;
            }

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

            viewModel.CurrentContent = currentContent;
            viewModel.Product = currentContent;
            viewModel.Variant = variant;
            viewModel.ListingPrice = defaultPrice?.UnitPrice ?? new Money(0, currency);
            viewModel.DiscountedPrice = discountedPrice;
            viewModel.SubscriptionPrice = subscriptionPrice?.UnitPrice ?? new Money(0, currency);
            viewModel.Colors = variants.OfType<GenericVariant>()
                .Where(x => x.Color != null)
                .GroupBy(x => x.Color)
                .Select(g => new SelectListItem
                {
                    Selected = false,
                    Text = g.Key,
                    Value = g.Key
                }).ToList();
            viewModel.Sizes = variants.OfType<GenericVariant>()
                .Where(x => (x.Color == null || x.Color.Equals(baseVariant?.Color, StringComparison.OrdinalIgnoreCase)) && x.Size != null)
                .Select(x => new SelectListItem
                {
                    Selected = false,
                    Text = x.Size,
                    Value = x.Size,
                    Disabled = !variantsState.FirstOrDefault(v => v.Key == x.Code).Value
                }).ToList();
            viewModel.Color = baseVariant?.Color;
            viewModel.Size = baseVariant?.Size;
            viewModel.Images = variant.GetAssets<IContentImage>(_contentLoader, _urlResolver);
            viewModel.Media = variant.GetAssetsWithType(_contentLoader, _urlResolver);
            viewModel.IsAvailable = _databaseMode.DatabaseMode != DatabaseMode.ReadOnly && defaultPrice != null && isInstock;
            viewModel.Stores = new StoreViewModel
            {
                Stores = _storeService.GetEntryStoresViewModels(variant.Code),
                SelectedStore = currentStore != null ? currentStore.Code : "",
                SelectedStoreName = currentStore != null ? currentStore.Name : ""
            };
            viewModel.StaticAssociations = associations;
            viewModel.Variants = variants.Select(x =>
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
            }).ToList();
            viewModel.HasOrganization = contact?.OwnerId != null;
            viewModel.ShowRecommendations = productRecommendations?.ShowRecommendations ?? true;
            viewModel.IsSalesRep = isSalesRep;
            viewModel.SalesMaterials = isSalesRep ? currentContent.CommerceMediaCollection.Where(x => !string.IsNullOrEmpty(x.GroupName) && x.GroupName.Equals("sales"))
                .Select(x => _contentLoader.Get<MediaData>(x.AssetLink)).ToList() : new List<MediaData>();
            viewModel.Documents = currentContent.CommerceMediaCollection
                .Where(o => o.AssetType.Equals(typeof(PdfFile).FullName.ToLowerInvariant()) || o.AssetType.Equals(typeof(StandardFile).FullName.ToLowerInvariant()))
                .Select(x => _contentLoader.Get<MediaData>(x.AssetLink)).ToList();
            viewModel.MinQuantity = (int)defaultPrice.MinQuantity;
            viewModel.HasSaleCode = defaultPrice != null ? !string.IsNullOrEmpty(defaultPrice.CustomerPricing.PriceCode) : false;

            return viewModel;
        }

        public virtual TViewModel CreateBundle<TBundle, TVariant, TViewModel>(TBundle currentContent)
            where TBundle : BundleContent
            where TVariant : VariationContent
            where TViewModel : BundleViewModelBase<TBundle>, new()
        {
            var viewModel = new TViewModel();
            var relatedProducts = currentContent.GetRelatedEntries().ToList();
            var associations = relatedProducts.Any() ?
                _productService.GetProductTileViewModels(relatedProducts) :
                new List<ProductTileViewModel>();
            var variants = GetVariants<TVariant, TBundle>(currentContent).Where(x => x.Prices().Where(p => p.UnitPrice > 0).Any()).ToList();
            var entries = GetEntriesRelation(currentContent);
            var currentStore = _storeService.GetCurrentStoreViewModel();
            var contact = PrincipalInfo.CurrentPrincipal.GetCustomerContact();
            var productRecommendations = currentContent as IProductRecommendations;
            var isSalesRep = PrincipalInfo.CurrentPrincipal.IsInRole("SalesRep");
            var currentWarehouse = _warehouseRepository.GetDefaultWarehouse();
            var isInstock = true;

            if (variants != null && variants.Count > 0)
            {
                foreach (var v in variants)
                {
                    var inStockQuantity = GetAvailableStockQuantity(v, currentWarehouse);

                    if (inStockQuantity <= 0)
                    {
                        isInstock = false;
                        break;
                    }
                }
            }
            else
            {
                isInstock = false;
            }

            viewModel.CurrentContent = currentContent;
            viewModel.Bundle = currentContent;
            viewModel.Images = currentContent.GetAssets<IContentImage>(_contentLoader, _urlResolver);
            viewModel.Media = currentContent.GetAssetsWithType(_contentLoader, _urlResolver);
            viewModel.Entries = variants;
            viewModel.EntriesRelation = entries;
            viewModel.Stores = new StoreViewModel
            {
                Stores = _storeService.GetEntryStoresViewModels(currentContent.Code),
                SelectedStore = currentStore != null ? currentStore.Code : "",
                SelectedStoreName = currentStore != null ? currentStore.Name : ""
            };
            viewModel.StaticAssociations = associations;
            viewModel.HasOrganization = contact?.OwnerId != null;
            viewModel.ShowRecommendations = productRecommendations?.ShowRecommendations ?? true;
            viewModel.IsSalesRep = isSalesRep;
            viewModel.SalesMaterials = isSalesRep ? currentContent.CommerceMediaCollection.Where(x => !string.IsNullOrEmpty(x.GroupName) && x.GroupName.Equals("sales"))
                .Select(x => _contentLoader.Get<MediaData>(x.AssetLink)).ToList() : new List<MediaData>();
            viewModel.IsAvailable = _databaseMode.DatabaseMode != DatabaseMode.ReadOnly && isInstock;

            return viewModel;
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
            var currentWarehouse = _warehouseRepository.GetDefaultWarehouse();
            var inStockQuantity = GetAvailableStockQuantity(currentContent, currentWarehouse);
            var isInstock = inStockQuantity > 0;

            return new TViewModel
            {
                CurrentContent = currentContent,
                ListingPrice = defaultPrice?.UnitPrice ?? new Money(0, currency),
                DiscountedPrice = discountedPrice,
                SubscriptionPrice = subscriptionPrice?.UnitPrice ?? new Money(0, currency),
                Images = currentContent.GetAssets<IContentImage>(_contentLoader, _urlResolver),
                Media = currentContent.GetAssetsWithType(_contentLoader, _urlResolver),
                IsAvailable = _databaseMode.DatabaseMode != DatabaseMode.ReadOnly && defaultPrice != null && isInstock,
                InStockQuantity = inStockQuantity,
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
                SalesMaterials = isSalesRep ? currentContent.CommerceMediaCollection?.Where(x => !string.IsNullOrEmpty(x.GroupName) && x.GroupName.Equals("sales"))
                    .Select(x => _contentLoader.Get<MediaData>(x.AssetLink)).ToList() : new List<MediaData>(),
                MinQuantity = defaultPrice != null ? (int)defaultPrice.MinQuantity : 0,
                HasSaleCode = defaultPrice != null ? !string.IsNullOrEmpty(defaultPrice.CustomerPricing.PriceCode) : false
            };
        }

        public virtual TViewModel CreatePackage<TPackage, TVariant, TViewModel>(TPackage currentContent)
            where TPackage : PackageContent
            where TVariant : VariationContent
            where TViewModel : PackageViewModelBase<TPackage>, new()
        {
            var viewModel = new TViewModel();
            var variants = GetVariants<TVariant, TPackage>(currentContent).Where(x => x.Prices().Where(p => p.UnitPrice > 0).Any()).ToList();
            var entries = GetEntriesRelation(currentContent);
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
            var currentWarehouse = _warehouseRepository.GetDefaultWarehouse();
            var inStockQuantity = GetAvailableStockQuantity(currentContent, currentWarehouse);
            var isInstock = inStockQuantity > 0;

            viewModel.InStockQuantity = inStockQuantity;
            viewModel.CurrentContent = currentContent;
            viewModel.Package = currentContent;
            viewModel.ListingPrice = defaultPrice?.UnitPrice ?? new Money(0, currency);
            viewModel.DiscountedPrice = GetDiscountPrice(defaultPrice, market, currency);
            viewModel.SubscriptionPrice = subscriptionPrice?.UnitPrice ?? new Money(0, currency);
            viewModel.Images = currentContent.GetAssets<IContentImage>(_contentLoader, _urlResolver);
            viewModel.Media = currentContent.GetAssetsWithType(_contentLoader, _urlResolver);
            viewModel.IsAvailable = _databaseMode.DatabaseMode != DatabaseMode.ReadOnly && defaultPrice != null && isInstock;
            viewModel.Entries = variants;
            viewModel.EntriesRelation = entries;
            //Reviews = GetReviews(currentContent.Code);
            viewModel.Stores = new StoreViewModel
            {
                Stores = _storeService.GetEntryStoresViewModels(currentContent.Code),
                SelectedStore = currentStore != null ? currentStore.Code : "",
                SelectedStoreName = currentStore != null ? currentStore.Name : ""
            };
            viewModel.StaticAssociations = associations;
            viewModel.HasOrganization = contact?.OwnerId != null;
            viewModel.ShowRecommendations = productRecommendations?.ShowRecommendations ?? true;
            viewModel.IsSalesRep = isSalesRep;
            viewModel.SalesMaterials = isSalesRep ? currentContent.CommerceMediaCollection.Where(x => !string.IsNullOrEmpty(x.GroupName) && x.GroupName.Equals("sales"))
                .Select(x => _contentLoader.Get<MediaData>(x.AssetLink)).ToList() : new List<MediaData>();
            viewModel.MinQuantity = defaultPrice != null ? (int)defaultPrice.MinQuantity : 0;
            viewModel.HasSaleCode = defaultPrice != null ? !string.IsNullOrEmpty(defaultPrice.CustomerPricing.PriceCode) : false;

            return viewModel;
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

        /// <summary>
        /// Get variants state of the product (is available or not)
        /// </summary>
        /// <typeparam name="TVariant">inherited VariationContent</typeparam>
        /// <param name="variants">List variants of the product</param>
        /// <param name="market">the market.</param>
        /// <returns>Dictionary with Key is the Variant Code and Value is IsAvailable or not</returns>
        private Dictionary<string, bool> GetVarantsState<TVariant>(List<TVariant> variants, IMarket market) where TVariant : VariationContent
        {
            var results = new Dictionary<string, bool>();
            foreach (var v in variants)
            {
                var available = _databaseMode.DatabaseMode != DatabaseMode.ReadOnly;
                if (!available)
                {
                    results.Add(v.Code, available);
                    continue;
                }

                var price = PriceCalculationService.GetSalePrice(v.Code, market.MarketId, market.DefaultCurrency);
                if (price == null)
                {
                    results.Add(v.Code, false);
                    continue;
                }

                if (!v.TrackInventory)
                {
                    results.Add(v.Code, true);
                    continue;
                }

                var currentWarehouse = _warehouseRepository.GetDefaultWarehouse();
                var inventoryRecord = _inventoryService.Get(v.Code, currentWarehouse.Code);
                var inventory = new Inventory(inventoryRecord);
                available = inventory.IsTracked && inventory.InStockQuantity == 0 ? false : true;
                results.Add(v.Code, available);
            }

            return results;
        }

        private decimal GetAvailableStockQuantity(EntryContentBase entry, IWarehouse currentWarehouse)
        {
            decimal quantity = 0;
            if ((entry as IStockPlacement).TrackInventory)
            {
                var inventoryRecord = _inventoryService.Get(entry.Code, currentWarehouse.Code);
                var inventory = new Inventory(inventoryRecord);
                quantity = inventory.IsTracked ? inventory.InStockQuantity - inventory.ReorderMinQuantity : 1;
            }

            return quantity;
        }

        private IEnumerable<EntryRelation> GetEntriesRelation(EntryContentBase content) => _relationRepository.GetChildren<EntryRelation>(content.ContentLink);
    }
}