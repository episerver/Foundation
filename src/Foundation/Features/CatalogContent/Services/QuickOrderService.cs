// EPiServer.Find removed: SearchSkus stubbed to return empty results. Phase 4 will restore Graph-based SKU search.
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
        private readonly IPriceService _priceService;
        private readonly IPromotionService _promotionService;
        private readonly IContentLanguageAccessor _languageResolver;

        public QuickOrderService(IContentLoader contentLoader,
            IInventoryService inventoryService,
            ICurrentMarket currentMarket,
            ICurrencyService currencyService,
            IPriceService priceService,
            IPromotionService promotionService,
            IContentLanguageAccessor languageResolver)
        {
            _contentLoader = contentLoader;
            _inventoryService = inventoryService;
            _currentMarket = currentMarket;
            _currencyService = currencyService;
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
                // Commerce 15: EntryContentBaseExtensions.GetDefaultPrice signature changed.
                // Now requires marketId, currency, and validOn parameters.
                var market = _currentMarket.GetCurrentMarket();
                var currency = _currencyService.GetCurrentCurrency();
                var defaultPrice = variantContent.ContentLink.GetDefaultPrice(market.MarketId, currency, DateTime.UtcNow);
                product.UnitPrice = defaultPrice != null ? defaultPrice.UnitPrice.Amount : 0;
            }

            return product;
        }

        public decimal GetTotalInventoryByEntry(string code)
            => _inventoryService.QueryByEntry(new[] { code }).Sum(x => x.PurchaseAvailableQuantity);

        // EPiServer.Find removed: returns empty. Phase 4 will restore Graph-based SKU search.
        public IEnumerable<SkuSearchResultModel> SearchSkus(string query)
            => Enumerable.Empty<SkuSearchResultModel>();
    }
}
