using EPiServer;
using EPiServer.Commerce.Catalog;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Commerce.SpecializedProperties;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Foundation.Cms;
using Foundation.Commerce.Catalog;
using Foundation.Commerce.Catalog.ViewModels;
using Foundation.Commerce.Models.Catalog;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.InventoryService;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Foundation.Commerce.Extensions
{
    public static class EntryContentBaseExtensions
    {
        private const int MaxHistory = 10;
        private const string Delimiter = "^!!^";

        private static readonly Lazy<IInventoryService> InventoryService =
            new Lazy<IInventoryService>(() => ServiceLocator.Current.GetInstance<IInventoryService>());

        private static readonly Lazy<ReferenceConverter> ReferenceConverter =
            new Lazy<ReferenceConverter>(() => ServiceLocator.Current.GetInstance<ReferenceConverter>());

        private static readonly Lazy<IPriceService> PriceService =
            new Lazy<IPriceService>(() => ServiceLocator.Current.GetInstance<IPriceService>());

        private static readonly Lazy<AssetUrlResolver> AssetUrlResolver =
            new Lazy<AssetUrlResolver>(() => ServiceLocator.Current.GetInstance<AssetUrlResolver>());

        private static readonly Lazy<UrlResolver> UrlResolver =
            new Lazy<UrlResolver>(() => ServiceLocator.Current.GetInstance<UrlResolver>());

        private static readonly Lazy<CookieService> CookieService =
            new Lazy<CookieService>(() => ServiceLocator.Current.GetInstance<CookieService>());

        private static readonly Lazy<ICurrentMarket> CurrentMarket =
            new Lazy<ICurrentMarket>(() => ServiceLocator.Current.GetInstance<ICurrentMarket>());

        private static readonly Lazy<IMarketService> MarketService =
            new Lazy<IMarketService>(() => ServiceLocator.Current.GetInstance<IMarketService>());

        private static readonly Lazy<IRelationRepository> RelationRepository =
            new Lazy<IRelationRepository>(() => ServiceLocator.Current.GetInstance<IRelationRepository>());

        private static readonly Lazy<IContentLoader> ContentLoader =
            new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());

        private static readonly Lazy<PromotionService> PromotionService =
            new Lazy<PromotionService>(() => ServiceLocator.Current.GetInstance<PromotionService>());

        public static IEnumerable<Inventory> Inventories(this EntryContentBase entryContentBase)
        {
            if (entryContentBase is ProductContent productContent)
            {
                var variations = ContentLoader.Value
                    .GetItems(productContent.GetVariants(RelationRepository.Value), productContent.Language)
                    .OfType<VariationContent>();
                return variations.SelectMany(x => x.GetStockPlacement());
            }

            if (entryContentBase is PackageContent packageContent)
            {
                return packageContent.ContentLink.GetStockPlacements();
            }

            return entryContentBase is VariationContent variationContent
                ? variationContent.ContentLink.GetStockPlacements()
                : Enumerable.Empty<Inventory>();
        }

        public static decimal DefaultPrice(this EntryContentBase entryContentBase)
        {
            var market = MarketService.Value.GetAllMarkets()
                .FirstOrDefault(x => x.DefaultLanguage.Name.Equals(entryContentBase.Language.Name));

            if (market == null)
            {
                return 0m;
            }

            var maxPrice = new Price();
            if (entryContentBase is ProductContent productContent)
            {
                var variationLinks = productContent.GetVariants(RelationRepository.Value);
                foreach (var variationLink in variationLinks)
                {
                    var defaultPrice =
                        variationLink.GetDefaultPrice(market.MarketId, market.DefaultCurrency, DateTime.UtcNow);
                    if (defaultPrice.UnitPrice.Amount > maxPrice.UnitPrice.Amount)
                    {
                        maxPrice = defaultPrice;
                    }
                }

                return maxPrice.UnitPrice.Amount;
            }

            if (entryContentBase is PackageContent packageContent)
            {
                return packageContent.ContentLink
                           .GetDefaultPrice(market.MarketId, market.DefaultCurrency, DateTime.UtcNow)?.UnitPrice
                           .Amount ?? 0m;
            }

            if (entryContentBase is VariationContent variationContent)
            {
                return variationContent.ContentLink
                           .GetDefaultPrice(market.MarketId, market.DefaultCurrency, DateTime.UtcNow)?.UnitPrice
                           .Amount ?? 0m;
            }

            return 0m;
        }

        public static IEnumerable<Price> Prices(this EntryContentBase entryContentBase)
        {
            //var market = MarketService.Value.GetAllMarkets().FirstOrDefault(x => x.DefaultLanguage.Name.Equals(entryContentBase.Language.Name));
            var market = CurrentMarket.Value.GetCurrentMarket();

            if (market == null)
            {
                return Enumerable.Empty<Price>();
            }

            var priceFilter = new PriceFilter
            {
                CustomerPricing = new[] { CustomerPricing.AllCustomers }
            };

            if (entryContentBase is ProductContent productContent)
            {
                var variationLinks = productContent.GetVariants();
                return variationLinks.GetPrices(market.MarketId, priceFilter);
            }

            if (entryContentBase is PackageContent packageContent)
            {
                return packageContent.ContentLink.GetPrices(market.MarketId, priceFilter);
            }

            return entryContentBase is VariationContent variationContent
                ? variationContent.ContentLink.GetPrices(market.MarketId, priceFilter)
                : Enumerable.Empty<Price>();
        }

        public static IEnumerable<VariationContent> VariationContents(this ProductContent productContent)
        {
            return ContentLoader.Value
                .GetItems(productContent.GetVariants(RelationRepository.Value), productContent.Language)
                .OfType<VariationContent>();
        }

        public static IEnumerable<string> Outline(this EntryContentBase productContent)
        {
            var nodes = ContentLoader.Value
                .GetItems(productContent.GetNodeRelations().Select(x => x.Parent), productContent.Language)
                .OfType<NodeContent>();

            return nodes.Select(x => GetOutlineForNode(x.Code));
        }

        public static int SortOrder(this EntryContentBase productContent)
        {
            var node = productContent.GetNodeRelations().FirstOrDefault();
            return node?.SortOrder ?? 0;
        }

        public static CatalogKey GetCatalogKey(this EntryContentBase productContent) => new CatalogKey(productContent.Code);

        public static CatalogKey GetCatalogKey(this ContentReference contentReference) => new CatalogKey(ReferenceConverter.Value.GetCode(contentReference));

        private static string ShortenLongDescription(string longDescription)
        {
            var wordColl = Regex.Matches(longDescription, @"[\S]+");
            var sb = new StringBuilder();

            if (wordColl.Count > 40)
            {
                foreach (var subWord in wordColl.Cast<Match>().Select(r => r.Value).Take(40))
                {
                    sb.Append(subWord);
                    sb.Append(" ");
                }
            }

            return sb.Length > 0 ? sb.Append("...").ToString() : "";
        }

        public static ItemCollection<Inventory> GetStockPlacements(this ContentReference contentLink)
        {
            var code = GetCode(contentLink.ToReferenceWithoutVersion());
            return string.IsNullOrEmpty(code)
                ? new ItemCollection<Inventory>()
                : new ItemCollection<Inventory>(InventoryService.Value.QueryByEntry(new[] { code }).Select(x =>
                      new Inventory(x)
                      {
                          ContentLink = contentLink
                      }));
        }

        public static Price GetDefaultPrice(this ContentReference contentLink, MarketId marketId, Currency currency,
            DateTime validOn)
        {
            var entry = ContentLoader.Value.Get<EntryContentBase>(contentLink.ToReferenceWithoutVersion());
            var catalogKey = GetCatalogKey(entry);

            var priceValue = PriceService.Value.GetDefaultPrice(marketId, validOn, catalogKey, currency);
            return priceValue != null ? new Price(priceValue, entry) : new Price();
        }

        public static ItemCollection<Price> GetPrices(this ContentReference entryContents,
            MarketId marketId, PriceFilter priceFilter) => new[] { entryContents }.GetPrices(marketId, priceFilter);

        public static ItemCollection<Price> GetPrices(this IEnumerable<ContentReference> entryContents,
            MarketId marketId, PriceFilter priceFilter)
        {
            var customerPricingList = priceFilter.CustomerPricing != null
                ? priceFilter.CustomerPricing.Where(x => x != null).ToList()
                : Enumerable.Empty<CustomerPricing>().ToList();

            var entryContentsList = entryContents.Where(x => x != null).ToList();

            var catalogKeys = entryContentsList.Select(GetCatalogKey);
            IEnumerable<IPriceValue> priceCollection;
            if (marketId == MarketId.Empty && (!customerPricingList.Any() ||
                                               customerPricingList.Any(x => string.IsNullOrEmpty(x.PriceCode))))
            {
                priceCollection = PriceService.Value.GetCatalogEntryPrices(catalogKeys);
            }
            else
            {
                var customerPricingsWithPriceCode =
                    customerPricingList.Where(x => !string.IsNullOrEmpty(x.PriceCode)).ToList();
                if (customerPricingsWithPriceCode.Any())
                {
                    priceFilter.CustomerPricing = customerPricingsWithPriceCode;
                }

                priceCollection = PriceService.Value.GetPrices(marketId, DateTime.UtcNow, catalogKeys, priceFilter);
            }

            var result = new ItemCollection<Price>();
            foreach (var priceValue in priceCollection)
            {
                var entryLink = entryContentsList.FirstOrDefault(c =>
                    GetCode(c).Equals(priceValue.CatalogKey.CatalogEntryCode, StringComparison.OrdinalIgnoreCase));
                var entryContent = ContentLoader.Value.Get<EntryContentBase>(entryLink);
                result.Add(new Price(priceValue, entryContent));
            }

            return result;
        }

        public static string GetCode(this ContentReference contentLink) => ReferenceConverter.Value.GetCode(contentLink);

        public static EntryContentBase GetEntryContent(this CatalogKey catalogKey)
        {
            var entryContentLink = ReferenceConverter.Value
                .GetContentLink(catalogKey.CatalogEntryCode, CatalogContentType.CatalogEntry);

            return ContentLoader.Value.Get<EntryContentBase>(entryContentLink);
        }

        public static IEnumerable<VariationContent> GetAllVariants(this ContentReference contentLink)
        {
            return GetAllVariants<VariationContent>(contentLink);
        }

        public static IEnumerable<T> GetAllVariants<T>(this ContentReference contentLink) where T : VariationContent
        {
            switch (ReferenceConverter.Value.GetContentType(contentLink))
            {
                case CatalogContentType.CatalogNode:
                    var entries = ContentLoader.Value.GetChildren<T>(contentLink,
                        new LoaderOptions { LanguageLoaderOption.FallbackWithMaster() });

                    foreach (var productContent in entries.OfType<ProductContent>())
                    {
                        entries = entries.Union(productContent.GetVariants()
                            .Select(c => ContentLoader.Value.Get<T>(c)));
                    }

                    return entries;
                case CatalogContentType.CatalogEntry:
                    var entryContent = ContentLoader.Value.Get<EntryContentBase>(contentLink);

                    if (entryContent is ProductContent p)
                    {
                        return p.GetVariants().Select(c => ContentLoader.Value.Get<T>(c));
                    }

                    if (entryContent is T)
                    {
                        return new List<T> { entryContent as T };
                    }
                    break;
            }

            return Enumerable.Empty<T>();
        }

        private static string GetOutlineForNode(string nodeCode)
        {
            if (string.IsNullOrEmpty(nodeCode))
            {
                return "";
            }
            var outline = nodeCode;
            var currentNode = ContentLoader.Value.Get<NodeContent>(ReferenceConverter.Value.GetContentLink(nodeCode));
            var parent = ContentLoader.Value.Get<CatalogContentBase>(currentNode.ParentLink);
            while (!ContentReference.IsNullOrEmpty(parent.ParentLink))
            {
                if (parent is CatalogContent catalog)
                {
                    outline = string.Format("{1}/{0}", outline, catalog.Name);
                }

                if (parent is NodeContent parentNode)
                {
                    outline = string.Format("{1}/{0}", outline, parentNode.Code);
                }

                parent = ContentLoader.Value.Get<CatalogContentBase>(parent.ParentLink);
            }
            return outline;
        }

        public static ProductTileViewModel GetProductTileViewModel(this EntryContentBase entry, IMarket market, Currency currency)
        {
            var placedPrice = entry.Prices()
                .Where(id => id.MarketId == market.MarketId)
                .OrderBy(sort => sort.UnitPrice.Amount)
                .FirstOrDefault(x => x.UnitPrice.Currency == currency);
            var entryRecommendations = entry as IProductRecommendations;
            var product = entry;
            var entryUrl = "";
            var firstCode = placedPrice != null && placedPrice.EntryContent is VariationContent ? (placedPrice.EntryContent as VariationContent).Code : "";
            var type = typeof(GenericProduct);

            if (entry is GenericProduct)
            {
                entryUrl = UrlResolver.Value.GetUrl(product.ContentLink);
            }

            if (entry is GenericBundle)
            {
                type = typeof(GenericBundle);
                firstCode = product.Code;
                entryUrl = UrlResolver.Value.GetUrl(product.ContentLink);
            }

            if (entry is GenericPackage)
            {
                type = typeof(GenericPackage);
                firstCode = product.Code;
                entryUrl = UrlResolver.Value.GetUrl(product.ContentLink);
            }

            if (entry is GenericVariant)
            {
                var variantEntry = entry as GenericVariant;
                type = typeof(GenericVariant);
                product = ContentLoader.Value.Get<EntryContentBase>(entry.GetParentProducts().FirstOrDefault()) as GenericProduct;
                entryUrl = UrlResolver.Value.GetUrl(product.ContentLink) + "?variationCode=" + variantEntry.Code;
            }

            return new ProductTileViewModel
            {
                ProductId = product.ContentLink.ID,
                Brand = entry.Property.Keys.Contains("Brand") ? entry.Property["Brand"]?.Value?.ToString() ?? "" : "",
                Code = product.Code,
                DisplayName = entry.DisplayName,
                Description = entry.Property.Keys.Contains("Description") ? entry.Property["Description"]?.Value != null ? ((XhtmlString)entry.Property["Description"].Value).ToHtmlString() : "" : "",
                LongDescription = ShortenLongDescription(entry.Property.Keys.Contains("LongDescription") ? entry.Property["LongDescription"]?.Value != null ? ((XhtmlString)entry.Property["LongDescription"].Value).ToHtmlString() : "" : ""),
                PlacedPrice = placedPrice?.UnitPrice ?? new Money(0, currency),
                DiscountedPrice = placedPrice == null ? new Money(0, currency) : PromotionService.Value.GetDiscountPrice(placedPrice.EntryContent.GetCatalogKey(), market.MarketId, currency).UnitPrice,
                FirstVariationCode = firstCode,
                ImageUrl = AssetUrlResolver.Value.GetAssetUrl<IContentImage>(entry),
                Url = entryUrl,
                IsAvailable = entry.Prices().Where(price => price.MarketId == market.MarketId)
                    .Any(x => x.UnitPrice.Currency == currency),
                OnSale = entry.Property.Keys.Contains("OnSale") && ((bool?)entry.Property["OnSale"]?.Value ?? false),
                NewArrival = entry.Property.Keys.Contains("NewArrival") && ((bool?)entry.Property["NewArrival"]?.Value ?? false),
                ShowRecommendations = entryRecommendations != null ? entryRecommendations.ShowRecommendations : true,
                EntryType = type
            };
        }

        public static string GetUrl(this EntryContentBase entry) => GetUrl(entry, RelationRepository.Value, UrlResolver.Value);

        public static string GetUrl(this EntryContentBase entry, IRelationRepository linksRepository,
            UrlResolver urlResolver)
        {
            var productLink = entry is VariationContent
                ? entry.GetParentProducts(linksRepository).FirstOrDefault()
                : entry.ContentLink;

            if (productLink == null)
            {
                return string.Empty;
            }

            var urlBuilder = new UrlBuilder(urlResolver.GetUrl(productLink));

            if (entry.Code != null && entry is VariationContent)
            {
                urlBuilder.QueryCollection.Add("variationCode", entry.Code);
            }

            return urlBuilder.ToString();
        }

        public static void AddBrowseHistory(this EntryContentBase entry)
        {

            var history = CookieService.Value.Get("BrowseHistory");
            var values = string.IsNullOrEmpty(history) ? new List<string>() :
                history.Split(new[] { Delimiter }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (values.Contains(entry.Code))
            {
                return;
            }

            if (values.Any())
            {
                if (values.Count == MaxHistory)
                {
                    values.RemoveAt(0);
                }
            }

            values.Add(entry.Code);

            CookieService.Value.Set("BrowseHistory", string.Join(Delimiter, values));
        }

        public static IList<EntryContentBase> GetBrowseHistory()
        {
            var entryCodes = CookieService.Value.Get("BrowseHistory");
            if (string.IsNullOrEmpty(entryCodes))
            {
                return new List<EntryContentBase>();
            }

            var contentLinks = ReferenceConverter.Value.GetContentLinks(entryCodes.Split(new[]
            {
                Delimiter
            }, StringSplitOptions.RemoveEmptyEntries));

            return ContentLoader.Value.GetItems(contentLinks.Select(x => x.Value), new LoaderOptions())
                .OfType<EntryContentBase>()
                .ToList();
        }
    }
}