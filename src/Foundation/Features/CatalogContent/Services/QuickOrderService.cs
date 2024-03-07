using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Commerce;
using EPiServer.Find.Commerce.Services.Internal;
using EPiServer.Find.Framework.Statistics;
using Foundation.Features.MyOrganization.QuickOrderBlock;
using Foundation.Features.MyOrganization.QuickOrderPage;
using Mediachase.Commerce.InventoryService;

namespace Foundation.Features.CatalogContent.Services
{
    public interface IQuickOrderService
    {
        string ValidateProduct(ContentReference variationReference, decimal quantity, string code);
        QuickOrderProductViewModel GetProductByCode(ContentReference productReference);
        decimal GetTotalInventoryByEntry(string code);
        IEnumerable<SkuSearchResultModel> SearchSkus(string query);
    }

    public class QuickOrderService : IQuickOrderService
    {
        private readonly IContentLoader _contentLoader;
        private readonly IInventoryService _inventoryService;
        private readonly ICurrentMarket _currentMarket;
        private readonly ICurrencyService _currencyService;
        private readonly IClient _findClient;
        private readonly IPriceService _priceService;
        private readonly IPromotionService _promotionService;
        private readonly IContentLanguageAccessor _languageResolver;

        public QuickOrderService(IContentLoader contentLoader,
            IInventoryService inventoryService,
            ICurrentMarket currentMarket,
            ICurrencyService currencyService,
            IClient findClient,
            IPriceService priceService,
            IPromotionService promotionService,
            IContentLanguageAccessor languageResolver)
        {
            _contentLoader = contentLoader;
            _inventoryService = inventoryService;
            _currentMarket = currentMarket;
            _currencyService = currencyService;
            _findClient = findClient;
            _priceService = priceService;
            _promotionService = promotionService;
            _languageResolver = languageResolver;
        }

        public string ValidateProduct(ContentReference variationReference, decimal quantity, string code)
        {
            if (ContentReference.IsNullOrEmpty(variationReference))
            {
                return $"The product with SKU {code} does not exist.";
            }

            var variantContent = _contentLoader.Get<VariationContent>(variationReference);
            var maxQuantity = GetTotalInventoryByEntry(variantContent.Code);
            if (quantity > maxQuantity)
            {
                return $"Quantity ordered is bigger than in stock quantity for the product with SKU {code}.";
            }

            return null;
        }

        public QuickOrderProductViewModel GetProductByCode(ContentReference productReference)
        {
            var product = new QuickOrderProductViewModel();
            if (!ContentReference.IsNullOrEmpty(productReference))
            {
                var variantContent = _contentLoader.Get<VariationContent>(productReference);
                product.ProductName = variantContent.Name;
                product.Sku = variantContent.Code;
                product.UnitPrice = variantContent.GetDefaultPrice() != null
                    ? variantContent.GetDefaultPrice().UnitPrice.Amount
                    : 0;
            }

            return product;
        }

        public decimal GetTotalInventoryByEntry(string code) => _inventoryService.QueryByEntry(new[] { code }).Sum(x => x.PurchaseAvailableQuantity);
        public IEnumerable<SkuSearchResultModel> SearchSkus(string query)
        {
            var market = _currentMarket.GetCurrentMarket();
            var currency = _currencyService.GetCurrentCurrency();

            var results = _findClient.Search<ProductContent>()
                .Filter(_ => _.VariationModels(), x => x.Code.PrefixCaseInsensitive(query))
                .FilterMarket(market)
                .Filter(x => x.Language.Name.Match(_languageResolver.Language.Name))
                .Track()
                .FilterForVisitor()
                .Select(_ => _.VariationModels())
                .GetResult()
                .SelectMany(x => x)
                .ToList();

            if (results != null && results.Any())
            {
                return results.Select(variation =>
                {
                    var defaultPrice = _priceService.GetDefaultPrice(market.MarketId, DateTime.Now,
                        new CatalogKey(variation.Code), currency);
                    var discountedPrice = defaultPrice != null ? _promotionService.GetDiscountPrice(defaultPrice.CatalogKey, market.MarketId,
                        currency) : null;
                    return new SkuSearchResultModel
                    {
                        Sku = variation.Code,
                        ProductName = string.IsNullOrEmpty(variation.Name) ? "" : variation.Name,
                        UnitPrice = discountedPrice?.UnitPrice.Amount ?? 0,
                        UrlImage = variation.DefaultAssetUrl
                    };
                });
            }
            return Enumerable.Empty<SkuSearchResultModel>();
        }
    }
}