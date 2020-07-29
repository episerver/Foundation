using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Find;
using EPiServer.Find.Framework;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Foundation.Features.Locations.Blocks
{
    [ContentType(DisplayName = "Filter Continents Block",
        GUID = "9103a763-4c9c-431e-bc11-f2794c3b4b80",
        Description = "Continent facets for locations",
        GroupName = TabNames.Location)]
    [ImageUrl("~/assets/icons/cms/blocks/map.png")]
    [AvailableContentTypes(Include = new Type[] { typeof(LocationListPage.LocationListPage) })]
    public class FilterContinentsBlock : FoundationBlockData, IFilterBlock
    {
        [CultureSpecific]
        [Display(Name = "Filter title")]
        public virtual string FilterTitle { get; set; }

        [CultureSpecific]
        [Display(Name = "All condition text")]
        public virtual string AllConditionText { get; set; }

        public ITypeSearch<LocationItemPage.LocationItemPage> AddFilter(ITypeSearch<LocationItemPage.LocationItemPage> query) => query.TermsFacetFor(x => x.Continent);

        public ITypeSearch<LocationItemPage.LocationItemPage> ApplyFilter(ITypeSearch<LocationItemPage.LocationItemPage> query, NameValueCollection filters)
        {
            var filterString = filters["c"];
            if (!string.IsNullOrWhiteSpace(filterString))
            {
                var continents = filterString.Split(',').ToList();
                var continentsFilter = SearchClient.Instance.BuildFilter<LocationItemPage.LocationItemPage>();
                continentsFilter = continents.Aggregate(continentsFilter,
                                                        (current, name) => current.Or(x => x.Continent.Match(name)));
                query = query.Filter(x => continentsFilter);
            }
            return query;
        }
    }
}