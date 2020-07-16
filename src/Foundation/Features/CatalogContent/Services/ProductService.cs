using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Globalization;
using EPiServer.Personalization.Commerce.Tracking;
using EPiServer.Web.Routing;
using Foundation.Commerce.Extensions;
using Foundation.Commerce.Markets;
using Foundation.Features.CatalogContent.Product;
using Foundation.Features.CatalogContent.Variation;
using Foundation.Features.Stores;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Pricing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Foundation.Features.CatalogContent.Services
{
    public interface IProductService
    {
        ProductTileViewModel GetProductTileViewModel(EntryContentBase entry);
        IEnumerable<ProductTileViewModel> GetProductTileViewModels(IEnumerable<ContentReference> entryLinks);
        string GetSiblingVariantCodeBySize(string siblingCode, string size);
        IEnumerable<VariationContent> GetVariants(ProductContent currentContent);
        IEnumerable<RecommendedProductTileViewModel> GetRecommendedProductTileViewModels(IEnumerable<Recommendation> recommendations);
    }

    public class ProductService : IProductService
    {
        private readonly IContentLoader _contentLoader;
        private readonly IPromotionService _promotionService;
        private readonly UrlResolver _urlResolver;
        private readonly IRelationRepository _relationRepository;
        private readonly CultureInfo _preferredCulture;
        private readonly ICurrentMarket _currentMarketService;
        private readonly ICurrencyService _currencyService;
        private readonly ReferenceConverter _referenceConverter;
        private readonly LanguageService _languageService;
        private readonly FilterPublished _filterPublished;
        private readonly IStoreService _storeService;
        private readonly ICurrentMarket _currentMarket;

        public ProductService(IContentLoader contentLoader,
            IPromotionService promotionService,
            UrlResolver urlResolver,
            IRelationRepository relationRepository,
            ICurrentMarket currentMarketService,
            ICurrencyService currencyService,
            ReferenceConverter referenceConverter,
            LanguageService languageService,
            FilterPublished filterPublished,
            IStoreService storeService,
            ICurrentMarket currentMarket)
        {
            _contentLoader = contentLoader;
            _promotionService = promotionService;
            _urlResolver = urlResolver;
            _relationRepository = relationRepository;
            _preferredCulture = ContentLanguage.PreferredCulture;
            _currentMarketService = currentMarketService;
            _currencyService = currencyService;
            _referenceConverter = referenceConverter;
            _languageService = languageService;
            _filterPublished = filterPublished;
            _storeService = storeService;
            _currentMarket = currentMarket;
        }

        public IEnumerable<VariationContent> GetVariants(ProductContent currentContent) => GetAvailableVariants(currentContent.GetVariants(_relationRepository));

        public string GetSiblingVariantCodeBySize(string siblingCode, string size)
        {
            var variationReference = _referenceConverter.GetContentLink(siblingCode);
            var productRelations = _relationRepository.GetParents<ProductVariation>(variationReference).ToList();
            var siblingsRelations = _relationRepository.GetChildren<ProductVariation>(productRelations.First().Parent);
            var siblingsReferences = siblingsRelations.Select(x => x.Child);
            var siblingVariants = GetAvailableVariants(siblingsReferences).OfType<GenericVariant>().ToList();

            var siblingVariant = siblingVariants.First(x => x.Code == siblingCode);

            foreach (var variant in siblingVariants)
            {
                if (variant.Size.Equals(size, StringComparison.OrdinalIgnoreCase) && variant.Code != siblingCode
                    && variant.Color.Equals(siblingVariant.Color, StringComparison.OrdinalIgnoreCase))
                {
                    return variant.Code;
                }
            }

            return null;
        }

        public IEnumerable<ProductTileViewModel> GetProductTileViewModels(IEnumerable<ContentReference> entryLinks)
        {
            var language = _languageService.GetCurrentLanguage();
            var contentItems = _contentLoader.GetItems(entryLinks, language);
            return contentItems.OfType<EntryContentBase>().Select(x => x.GetProductTileViewModel(_currentMarket.GetCurrentMarket(), _currencyService.GetCurrentCurrency()));
        }

        public virtual ProductTileViewModel GetProductTileViewModel(EntryContentBase entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            if (entry is PackageContent)
            {
                return CreateProductViewModelForEntry((PackageContent)entry);
            }

            if (entry is ProductContent)
            {
                var product = (ProductContent)entry;
                var variant = GetAvailableVariants(product.GetVariants()).FirstOrDefault();

                return CreateProductViewModelForVariant(product, variant);
            }

            if (entry is VariationContent)
            {
                ProductContent product = null;
                var parentLink = entry.GetParentProducts(_relationRepository).SingleOrDefault();
                if (!ContentReference.IsNullOrEmpty(parentLink))
                {
                    product = _contentLoader.Get<ProductContent>(parentLink);
                }

                return CreateProductViewModelForVariant(product, (VariationContent)entry);
            }

            throw new ArgumentException("BundleContent is not supported", nameof(entry));
        }

        public IEnumerable<RecommendedProductTileViewModel> GetRecommendedProductTileViewModels(IEnumerable<Recommendation> recommendations)
        {
            try
            {
                var returnValue = new List<RecommendedProductTileViewModel>();
                var language = _languageService.GetCurrentLanguage();
                var currentMarket = _currentMarket.GetCurrentMarket();

                foreach (var recommendation in recommendations)
                {
                    try
                    {
                        returnValue.Add(
                            new RecommendedProductTileViewModel(recommendation.RecommendationId,
                            _contentLoader.Get<EntryContentBase>(recommendation.ContentLink, language).GetProductTileViewModel(currentMarket, currentMarket.DefaultCurrency))
                        );
                    }
                    catch
                    {
                    }
                }

                return returnValue;
            }
            catch
            {
                return new List<RecommendedProductTileViewModel>();
            }
        }

        private IEnumerable<VariationContent> GetAvailableVariants(IEnumerable<ContentReference> contentLinks)
        {
            return _contentLoader.GetItems(contentLinks, _preferredCulture)
                                                            .OfType<VariationContent>()
                                                            .Where(v => v.IsAvailableInCurrentMarket(_currentMarketService) && !_filterPublished.ShouldFilter(v));
        }

        private ProductTileViewModel CreateProductViewModelForEntry(EntryContentBase entry)
        {
            var market = _currentMarketService.GetCurrentMarket();
            var currency = _currencyService.GetCurrentCurrency();
            var originalPrice = PriceCalculationService.GetSalePrice(entry.Code, market.MarketId, market.DefaultCurrency);
            Money? discountedPrice;

            if (originalPrice?.UnitPrice == null || originalPrice.UnitPrice.Amount == 0)
            {
                originalPrice = new PriceValue() { UnitPrice = new Money(0, market.DefaultCurrency) };
                discountedPrice = null;
            }
            else
            {
                discountedPrice = GetDiscountPrice(entry, market, currency, originalPrice.UnitPrice);
            }

            var image = entry.GetAssets<IContentImage>(_contentLoader, _urlResolver).FirstOrDefault() ?? "";
            var currentStore = _storeService.GetCurrentStoreViewModel();
            return new ProductTileViewModel
            {
                Code = entry.Code,
                DisplayName = entry.DisplayName,
                PlacedPrice = originalPrice.UnitPrice,
                DiscountedPrice = discountedPrice,
                ImageUrl = image,
                Url = entry.GetUrl(),
                IsAvailable = originalPrice.UnitPrice != null && originalPrice.UnitPrice.Amount > 0,
                Stores = new StoreViewModel
                {
                    Stores = _storeService.GetEntryStoresViewModels(entry.Code),
                    SelectedStore = currentStore != null ? currentStore.Code : "",
                    SelectedStoreName = currentStore != null ? currentStore.Name : ""
                }
            };
        }

        private ProductTileViewModel CreateProductViewModelForVariant(ProductContent product, VariationContent variant)
        {
            if (variant == null)
            {
                return null;
            }

            var viewModel = CreateProductViewModelForEntry(variant);
            if (product == null)
            {
                return viewModel;
            }

            viewModel.Brand = product is GenericProduct baseProduct ? baseProduct.Brand : string.Empty;

            return viewModel;
        }

        private Money GetDiscountPrice(EntryContentBase entry, IMarket market, Currency currency, Money originalPrice)
        {
            var discountedPrice = _promotionService.GetDiscountPrice(new CatalogKey(entry.Code), market.MarketId, currency);
            return discountedPrice?.UnitPrice ?? originalPrice;
        }
    }
}