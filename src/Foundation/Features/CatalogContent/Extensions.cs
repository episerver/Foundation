using EPiServer;
using EPiServer.Commerce.Catalog;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Marketing;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Foundation.Commerce.Extensions;
using Foundation.Features.CatalogContent.Bundle;
using Foundation.Features.CatalogContent.Package;
using Foundation.Features.CatalogContent.Product;
using Foundation.Features.CatalogContent.Variation;
using Mediachase.Commerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Foundation.Features.CatalogContent
{
    public static class Extensions
    {
        private static readonly Lazy<AssetUrlResolver> AssetUrlResolver =
            new Lazy<AssetUrlResolver>(() => ServiceLocator.Current.GetInstance<AssetUrlResolver>());

        private static readonly Lazy<IContentLoader> ContentLoader =
           new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());

        private static readonly Lazy<IPromotionEngine> PromotionEngine =
            new Lazy<IPromotionEngine>(() => ServiceLocator.Current.GetInstance<IPromotionEngine>());

        private static readonly Lazy<UrlResolver> UrlResolver =
            new Lazy<UrlResolver>(() => ServiceLocator.Current.GetInstance<UrlResolver>());

        public static ProductTileViewModel GetProductTileViewModel(this EntryContentBase entry, IMarket market, Currency currency, bool isFeaturedProduct = false)
        {
            var prices = entry.Prices();
            var minPrice = prices.OrderBy(x => x.UnitPrice).ThenBy(x => x.MinQuantity).FirstOrDefault();
            var discountPriceList = GetDiscountPriceCollection(entry, market, currency);
            var minDiscountPrice = GetMinDiscountPrice(discountPriceList);

            // if discount price is selected
            var isDiscounted = minDiscountPrice.Value != null
                ? (minDiscountPrice.Value.Price < minPrice.UnitPrice ? true : false)
                : false;

            var entryRecommendations = entry as IProductRecommendations;
            var product = entry;
            var entryUrl = "";
            var firstCode = "";
            var type = typeof(GenericProduct);

            if (entry is GenericProduct)
            {
                entryUrl = UrlResolver.Value.GetUrl(product.ContentLink);
                firstCode = isDiscounted ? ContentLoader.Value.Get<EntryContentBase>(minDiscountPrice.Key).Code : minPrice.EntryContent.Code;
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

            return new ProductTileViewModel
            {
                ProductId = product.ContentLink.ID,
                Brand = entry.Property.Keys.Contains("Brand") ? entry.Property["Brand"]?.Value?.ToString() ?? "" : "",
                Code = product.Code,
                DisplayName = entry.DisplayName,
                Description = entry.Property.Keys.Contains("Description") ? entry.Property["Description"]?.Value != null ? ((XhtmlString)entry.Property["Description"].Value).ToHtmlString() : "" : "",
                LongDescription = ShortenLongDescription(entry.Property.Keys.Contains("LongDescription") ? entry.Property["LongDescription"]?.Value != null ? ((XhtmlString)entry.Property["LongDescription"].Value).ToHtmlString() : "" : ""),
                PlacedPrice = isDiscounted ? minDiscountPrice.Value.DefaultPrice : (minPrice != null ? minPrice.UnitPrice : new Money(0, currency)),
                DiscountedPrice = isDiscounted ? minDiscountPrice.Value.Price : (minPrice != null ? minPrice.UnitPrice : new Money(0, currency)),
                FirstVariationCode = firstCode,
                ImageUrl = AssetUrlResolver.Value.GetAssetUrl<IContentImage>(entry),
                VideoAssetUrl = AssetUrlResolver.Value.GetAssetUrl<IContentVideo>(entry),
                Url = entryUrl,
                IsAvailable = entry.Prices().Where(price => price.MarketId == market.MarketId)
                    .Any(x => x.UnitPrice.Currency == currency),
                OnSale = entry.Property.Keys.Contains("OnSale") && ((bool?)entry.Property["OnSale"]?.Value ?? false),
                NewArrival = entry.Property.Keys.Contains("NewArrival") && ((bool?)entry.Property["NewArrival"]?.Value ?? false),
                ShowRecommendations = entryRecommendations != null ? entryRecommendations.ShowRecommendations : true,
                EntryType = type,
                ProductStatus = entry.Property.Keys.Contains("ProductStatus") ? entry.Property["ProductStatus"]?.Value?.ToString() ?? "Active" : "Active",
                Created = entry.Created,
                IsFeaturedProduct = isFeaturedProduct
            };
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
                    sb.Append(" ");
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