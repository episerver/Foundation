using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Commerce;
using EPiServer.ServiceLocation;
using Foundation.Commerce.Extensions;
using Foundation.Commerce.Models.Catalog;
using Foundation.Find.Cms;
using Foundation.Find.Commerce.ViewModels;
using Mediachase.Commerce.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Find.Commerce
{
    public static class Extensions
    {
        private static readonly Lazy<IContentLoader> ContentLoader =
            new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());

        private static readonly Lazy<ReferenceConverter> ReferenceConverter =
            new Lazy<ReferenceConverter>(() => ServiceLocator.Current.GetInstance<ReferenceConverter>());

        private static readonly Lazy<IRelationRepository> RelationRepository =
           new Lazy<IRelationRepository>(() => ServiceLocator.Current.GetInstance<IRelationRepository>());

        public static FilterBuilder<T> FilterOutline<T>(this FilterBuilder<T> filterBuilder,
            IEnumerable<string> value)
        {
            var outlineFilterBuilder = new FilterBuilder<ContentData>(filterBuilder.Client);
            outlineFilterBuilder = outlineFilterBuilder.And(x => !x.MatchTypeHierarchy(typeof(EntryContentBase)));
            outlineFilterBuilder = value.Aggregate(outlineFilterBuilder,
                (current, filter) => current.Or(x => ((EntryContentBase)x).Outline().PrefixCaseInsensitive(filter)));
            return filterBuilder.And(x => outlineFilterBuilder);
        }

        public static ITypeSearch<T> FilterOutline<T>(this ITypeSearch<T> search, IEnumerable<string> value)
        {
            var filterBuilder = new FilterBuilder<T>(search.Client)
                .FilterOutline(value);

            return search.Filter(x => filterBuilder);
        }

        public static List<string> AvailableSizes(this GenericProduct genericProduct)
        {
            return genericProduct.ContentLink.GetAllVariants<GenericVariant>()
                .Select(x => x.Size)
                .Distinct()
                .ToList();
        }

        public static List<string> AvailableColors(this GenericProduct genericProduct)
        {
            return genericProduct.ContentLink.GetAllVariants<GenericVariant>()
                .Select(x => x.Color)
                .Distinct()
                .ToList();
        }

        public static IEnumerable<VariationModel> VariationModels(this ProductContent productContent)
        {
            return ContentLoader.Value
                .GetItems(productContent.GetVariants(RelationRepository.Value), productContent.Language)
                .OfType<VariationContent>()
                .Select(x => new VariationModel
                {
                    Code = x.Code,
                    LanguageId = productContent.Language.Name,
                    Name = x.DisplayName,
                    DefaultAssetUrl = (x as IAssetContainer).DefaultImageUrl()
                });
        }
    }
}
