namespace Foundation.Features.Locations.Blocks
{
    // EPiServer.Find removed: AddFilter/ApplyFilter removed. Block compiles as a CMS content type
    // but does not perform filtering. Graph-based faceted search is a future enhancement.
    [ContentType(DisplayName = "Filter Continents Block",
        GUID = "9103a763-4c9c-431e-bc11-f2794c3b4b80",
        Description = "Continent facets for locations",
        GroupName = GroupNames.LocationBlocks)]
    [ImageUrl("/icons/cms/blocks/map.png")]
    [AvailableContentTypes(Include = new Type[] { typeof(LocationListPage.LocationListPage) })]
    public class FilterContinentsBlock : FoundationBlockData, IFilterBlock
    {
        [CultureSpecific]
        [Display(Name = "Filter title")]
        public virtual string FilterTitle { get; set; }

        [CultureSpecific]
        [Display(Name = "All condition text")]
        public virtual string AllConditionText { get; set; }
    }
}
