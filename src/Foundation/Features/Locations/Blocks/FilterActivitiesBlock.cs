using EPiServer.Find;
using EPiServer.Find.Framework;
using System.Web;

namespace Foundation.Features.Locations.Blocks
{
    [ContentType(DisplayName = "Filter Activities Block",
        GUID = "918c590e-b2cd-4b87-9116-899b1db19117",
        Description = "Activity facets for locations",
        GroupName = GroupNames.LocationBlocks)]
    [ImageUrl("/icons/cms/blocks/map.png")]
    [AvailableContentTypes(Include = new Type[] { typeof(LocationListPage.LocationListPage) })]
    public class FilterActivitiesBlock : FoundationBlockData, IFilterBlock
    {
        [CultureSpecific]
        [Display(Name = "Filter title")]
        public virtual string FilterTitle { get; set; }

        [CultureSpecific]
        [Display(Name = "All condition text")]
        public virtual string AllConditionText { get; set; }

        public ITypeSearch<LocationItemPage.LocationItemPage> AddFilter(ITypeSearch<LocationItemPage.LocationItemPage> query)
        {
            return query.TermsFacetFor(x => x.TagString(), facet => facet.Size = 25);
        }

        public ITypeSearch<LocationItemPage.LocationItemPage> ApplyFilter(ITypeSearch<LocationItemPage.LocationItemPage> query, IQueryCollection filters)
        {
            var filterString = filters["a"];
            if (!string.IsNullOrWhiteSpace(filterString))
            {
                var activities = filters["a"].ToList();
                var activitiesFilter = SearchClient.Instance.BuildFilter<LocationItemPage.LocationItemPage>();
                activitiesFilter = activities.Aggregate(activitiesFilter,
                        (current, name) => current.Or(x => x.TagString().Match(HttpUtility.UrlDecode(name)))
                    );
                query = query.Filter(x => activitiesFilter);
            }
            return query;
        }
    }
}