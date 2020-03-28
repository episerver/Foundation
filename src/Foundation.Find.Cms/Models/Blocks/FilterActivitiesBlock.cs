using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Find;
using EPiServer.Find.Framework;
using Foundation.Cms.Blocks;
using Foundation.Find.Cms.Locations;
using Foundation.Find.Cms.Models.Pages;
using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Foundation.Find.Cms.Models.Blocks
{
    [ContentType(DisplayName = "Filter Activities Block",
        GUID = "918c590e-b2cd-4b87-9116-899b1db19117",
        Description = "Activity facets for locations",
        GroupName = FindTabNames.Location)]
    [ImageUrl("~/assets/icons/cms/blocks/map.png")]
    [AvailableContentTypes(Include = new Type[] { typeof(LocationListPage) })]
    public class FilterActivitiesBlock : FoundationBlockData, IFilterBlock
    {
        [CultureSpecific]
        [Display(Name = "Filter title")]
        public virtual string FilterTitle { get; set; }

        [CultureSpecific]
        [Display(Name = "All condition text")]
        public virtual string AllConditionText { get; set; }

        public ITypeSearch<LocationItemPage> AddFilter(ITypeSearch<LocationItemPage> query)
        {
            return query.TermsFacetFor(x => x.TagString(), facet => facet.Size = 25);
        }

        public ITypeSearch<LocationItemPage> ApplyFilter(ITypeSearch<LocationItemPage> query, NameValueCollection filters)
        {
            var filterString = filters["a"];
            if (!string.IsNullOrWhiteSpace(filterString))
            {
                var activities = filters["a"].Split(',').ToList();
                var activitiesFilter = SearchClient.Instance.BuildFilter<LocationItemPage>();
                activitiesFilter = activities.Aggregate(activitiesFilter,
                                                        (current, name) =>
                                                        current.Or(
                                                            x => x.TagString().Match(HttpUtility.UrlDecode(name))));
                query = query.Filter(x => activitiesFilter);
            }
            return query;
        }
    }
}