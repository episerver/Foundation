using EPiServer;
using EPiServer.Commerce.Catalog;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Commerce.Marketing;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Foundation.Features.CatalogContent.Bundle;
using Foundation.Features.CatalogContent.Package;
using Foundation.Features.CatalogContent.Product;
using Foundation.Features.CatalogContent.Services;
using Foundation.Features.CatalogContent.Variation;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Foundation.Features.CatalogContent
{
    public static class Extensions
    {
        private static readonly Lazy<ReferenceConverter> ReferenceConverter =
            new Lazy<ReferenceConverter>(() => ServiceLocator.Current.GetInstance<ReferenceConverter>());

        private static readonly Lazy<AssetUrlResolver> AssetUrlResolver =
            new Lazy<AssetUrlResolver>(() => ServiceLocator.Current.GetInstance<AssetUrlResolver>());

        private static readonly Lazy<IContentLoader> ContentLoader =
           new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());

        private static readonly Lazy<IPromotionEngine> PromotionEngine =
            new Lazy<IPromotionEngine>(() => ServiceLocator.Current.GetInstance<IPromotionEngine>());

        private static readonly Lazy<UrlResolver> UrlResolver =
            new Lazy<UrlResolver>(() => ServiceLocator.Current.GetInstance<UrlResolver>());

        private static readonly Lazy<IRelationRepository> RelationRepository =
            new Lazy<IRelationRepository>(() => ServiceLocator.Current.GetInstance<IRelationRepository>());

        private static readonly Lazy<LanguageResolver> LanguageResolver =
            new Lazy<LanguageResolver>(() => ServiceLocator.Current.GetInstance<LanguageResolver>());

        private static readonly Lazy<ICurrentMarket> CurrentMarket =
            new Lazy<ICurrentMarket>(() => ServiceLocator.Current.GetInstance<ICurrentMarket>());

        private static readonly Lazy<FilterPublished> FilterPublished =
            new Lazy<FilterPublished>(() => ServiceLocator.Current.GetInstance<FilterPublished>());

        private static readonly Lazy<IPromotionService> PromotionService =
            new Lazy<IPromotionService>(() => ServiceLocator.Current.GetInstance<IPromotionService>());

        public static ProductTileViewModel GetProductTileViewModel(this EntryContentBase entry, IMarket market, Currency currency, bool isFeaturedProduct = false)
        {
            var entryRecommendations = entry as IProductRecommendations;
            var product = entry;
            var entryUrl = "";
            var firstCode = "";
            var type = typeof(GenericProduct);

            if (entry is GenericProduct)
            {
                var variants = GetProductVariants(entry);
                if (variants != null && variants.Any())
                {
                    firstCode = variants.First().Code;
                }
                entryUrl = UrlResolver.Value.GetUrl(entry.ContentLink);
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
                firstCode = entry.Code;
                var parentLink = entry.GetParentProducts().FirstOrDefault();
                if (ContentReference.IsNullOrEmpty(parentLink))
                {
                    product = ContentLoader.Value.Get<EntryContentBase>(variantEntry.ContentLink);
                    entryUrl = UrlResolver.Value.GetUrl(variantEntry.ContentLink);
                }
                else
                {
                    product = ContentLoader.Value.Get<EntryContentBase>(parentLink) as GenericProduct;
                    entryUrl = UrlResolver.Value.GetUrl(product.ContentLink) + "?variationCode=" + variantEntry.Code;
                }
            }

            IPriceValue price = PriceCalculationService.GetSalePrice(firstCode, market.MarketId, currency);
            if (price == null)
            {
                price = GetEmptyPrice(entry, market, currency);
            }
            IPriceValue discountPrice = price;
            if (price.UnitPrice.Amount > 0 && !string.IsNullOrEmpty(firstCode))
            {
                discountPrice = PromotionService.Value.GetDiscountPrice(new CatalogKey(firstCode), market.MarketId, currency);
            }

            bool isAvailable = price.UnitPrice.Amount > 0;
            return new ProductTileViewModel
            {
                ProductId = product.ContentLink.ID,
                Brand = entry.Property.Keys.Contains("Brand") ? entry.Property["Brand"]?.Value?.ToString() ?? "" : "",
                Code = product.Code,
                DisplayName = entry.DisplayName,
                Description = entry.Property.Keys.Contains("Description") ? entry.Property["Description"]?.Value != null ? ((XhtmlString)entry.Property["Description"].Value).ToHtmlString() : "" : "",
                LongDescription = ShortenLongDescription(entry.Property.Keys.Contains("LongDescription") ? entry.Property["LongDescription"]?.Value != null ? ((XhtmlString)entry.Property["LongDescription"].Value).ToHtmlString() : "" : ""),
                PlacedPrice = price.UnitPrice,
                DiscountedPrice = discountPrice.UnitPrice,
                FirstVariationCode = firstCode,
                ImageUrl = AssetUrlResolver.Value.GetAssetUrl<IContentImage>(entry),
                VideoAssetUrl = AssetUrlResolver.Value.GetAssetUrl<IContentVideo>(entry),
                Url = entryUrl,
                IsAvailable = isAvailable,
                OnSale = entry.Property.Keys.Contains("OnSale") && ((bool?)entry.Property["OnSale"]?.Value ?? false),
                NewArrival = entry.Property.Keys.Contains("NewArrival") && ((bool?)entry.Property["NewArrival"]?.Value ?? false),
                ShowRecommendations = entryRecommendations != null ? entryRecommendations.ShowRecommendations : true,
                EntryType = type,
                ProductStatus = entry.Property.Keys.Contains("ProductStatus") ? entry.Property["ProductStatus"]?.Value?.ToString() ?? "Active" : "Active",
                Created = entry.Created,
                IsFeaturedProduct = isFeaturedProduct
            };
        }

        private static IPriceValue GetEmptyPrice(EntryContentBase entry, IMarket market, Currency currency)
        {
            return new PriceValue()
            {
                CatalogKey = new CatalogKey(entry.Code),
                MarketId = market.MarketId,
                CustomerPricing = new CustomerPricing(CustomerPricing.PriceType.AllCustomers, string.Empty),
                ValidFrom = DateTime.Now.AddDays(-1),
                ValidUntil = DateTime.Now.AddDays(1),
                MinQuantity = 1,
                UnitPrice = new Money(0, currency)
            };
        }

        private static IEnumerable<VariationContent> GetProductVariants(EntryContentBase entry)
        {
            var product = entry as ProductContent;
            if (product != null)
            {
                return ContentLoader.Value
                    .GetItems(product.GetVariants(RelationRepository.Value), LanguageResolver.Value.GetPreferredCulture())
                    .OfType<VariationContent>()
                    .Where(v => v.IsAvailableInCurrentMarket(CurrentMarket.Value) && !FilterPublished.Value.ShouldFilter(v))
                    .ToArray();
            }
            else
            {
                return null;
            }
        }
        private static string ShortenLongDescription(string longDescription)
        {
            var wordColl = Regex.Matches(longDescription, @"[\S]+");
            var sb = new StringBuilder();

            if (wordColl.Count > 40)
            {
                foreach (var subWord in wordColl.Cast<Match>().Select(r => r.Value).Take(40))
                {
                    sb.Append(subWord);
                    sb.Append(' ');
                }
            }

            return sb.Length > 0 ? sb.Append("...").ToString() : "";
        }

        private static IEnumerable<DiscountedEntry> GetDiscountPriceCollection(EntryContentBase entry, IMarket market, Currency currency)
        {
            if (entry is ProductContent productContent)
            {
                var variationLinks = productContent.GetVariants();
                return PromotionEngine.Value.GetDiscountPrices(variationLinks, market, currency);
            }

            if (!(entry is BundleContent))
            {
                return PromotionEngine.Value.GetDiscountPrices(entry.ContentLink, market, currency);
            }

            return new List<DiscountedEntry>();
        }

        private static KeyValuePair<ContentReference, DiscountPrice> GetMinDiscountPrice(IEnumerable<DiscountedEntry> discountedEntries)
        {
            if (discountedEntries != null && discountedEntries.Any())
            {
                DiscountPrice minPrice = null;
                ContentReference contentLink = null;
                foreach (var d in discountedEntries)
                {
                    var discountPrice = d.DiscountPrices.OrderBy(x => x.Price).FirstOrDefault();
                    if (minPrice == null)
                    {
                        minPrice = discountPrice;
                        contentLink = d.EntryLink;
                    }
                    else
                    {
                        if (minPrice.Price.Amount > discountPrice.Price.Amount)
                        {
                            minPrice = discountPrice;
                            contentLink = d.EntryLink;
                        }
                    }
                }

                return new KeyValuePair<ContentReference, DiscountPrice>(contentLink, minPrice);
            }

            return new KeyValuePair<ContentReference, DiscountPrice>(null, null);
        }
    }
}