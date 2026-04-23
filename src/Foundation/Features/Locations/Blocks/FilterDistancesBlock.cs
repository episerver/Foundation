namespace Foundation.Features.Locations.Blocks
{
    // EPiServer.Find removed: AddFilter/ApplyFilter removed (geo-distance faceting requires Find).
    // Block compiles as a CMS content type but does not perform filtering.
    [ContentType(DisplayName = "Filter Distances Block",
        GUID = "eab40a8c-9006-4766-a87e-1dec153e735f",
        Description = "Distance facets for locations",
        GroupName = GroupNames.LocationBlocks)]
    [ImageUrl("/icons/cms/blocks/map.png")]
    [AvailableContentTypes(Include = new Type[] { typeof(LocationListPage.LocationListPage) })]
    public class FilterDistancesBlock : FoundationBlockData, IFilterBlock
    {
        [CultureSpecific]
        [Display(Name = "Filter title")]
        public virtual string FilterTitle { get; set; }

        [CultureSpecific]
        [Display(Name = "All condition text")]
        public virtual string AllConditionText { get; set; }
    }
}
