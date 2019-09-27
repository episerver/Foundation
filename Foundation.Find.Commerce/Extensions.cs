using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;
using Foundation.Commerce.Extensions;
using Foundation.Commerce.Models.Catalog;
using Foundation.Find.Cms;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Find.Commerce
{
    public static class Extensions
    {
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
    }
}
