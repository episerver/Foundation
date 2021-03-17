using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Commerce.SpecializedProperties;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Foundation.Cms;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.InventoryService;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;

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

            var minPrice = new Price();
            if (entryContentBase is ProductContent productContent)
            {
                var variationLinks = productContent.GetVariants(RelationRepository.Value);
                foreach (var variationLink in variationLinks)
                {
                    var defaultPrice =
                        variationLink.GetDefaultPrice(market.MarketId, market.DefaultCurrency, DateTime.UtcNow);

                    if ((defaultPrice.UnitPrice.Amount < minPrice.UnitPrice.Amount && defaultPrice.UnitPrice.Amount > 0) || minPrice.UnitPrice.Amount == 0)
                    {
                        minPrice = defaultPrice;
                    }
                }

                return minPrice.UnitPrice.Amount;
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

        public static Price GetDefaultPrice(this ContentReference contentLink, MarketId marketId, Currency currency, DateTime validOn)
        {
            var catalogKey = new CatalogKey(ReferenceConverter.Value.GetCode(contentLink));

            var priceValue = PriceService.Value.GetPrices(marketId, validOn, catalogKey, new PriceFilter() { Currencies = new[] { currency } })
                .OrderBy(x => x.UnitPrice).FirstOrDefault();
            return priceValue == null ? new Price() : new Price(priceValue);
        }

        public static IEnumerable<Price> GetPrices(this ContentReference entryContents,
            MarketId marketId, PriceFilter priceFilter) => new[] { entryContents }.GetPrices(marketId, priceFilter);

        public static IEnumerable<Price> GetPrices(this IEnumerable<ContentReference> entryContents, MarketId marketId, PriceFilter priceFilter)
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

                // if the entry has no price without sale code
                if (!priceCollection.Any())
                {
                    priceCollection = PriceService.Value.GetCatalogEntryPrices(catalogKeys)
                       .Where(x => x.ValidFrom <= DateTime.Now && (!x.ValidUntil.HasValue || x.ValidUntil.Value >= DateTime.Now))
                       .Where(x => x.MarketId == marketId);
                }
            }

            return priceCollection.Select(x => new Price(x));
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

        public static string GetUrl(this EntryContentBase entry) => GetUrl(entry, RelationRepository.Value, UrlResolver.Value);

        public static string GetUrl(this EntryContentBase entry, IRelationRepository linksRepository, UrlResolver urlResolver)
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