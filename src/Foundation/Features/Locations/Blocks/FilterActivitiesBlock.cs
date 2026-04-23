namespace Foundation.Features.Locations.Blocks
{
    // EPiServer.Find removed: AddFilter/ApplyFilter removed. Block compiles as a CMS content type
    // but does not perform filtering. Graph-based faceted search is a future enhancement.
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
    }
}
