namespace Foundation.Features.Locations.Blocks
{
    // EPiServer.Find removed: AddFilter/ApplyFilter removed. Block compiles as a CMS content type
    // but does not perform filtering. Graph-based range filtering is a future enhancement.
    [ContentType(DisplayName = "Filter Temperatures Block",
        GUID = "28629b4b-9475-4c44-9c15-31961391f166",
        Description = "Temperature slider for locations",
        GroupName = GroupNames.LocationBlocks)]
    [ImageUrl("/icons/cms/blocks/map.png")]
    [AvailableContentTypes(Include = new Type[] { typeof(LocationListPage.LocationListPage) })]
    public class FilterTemperaturesBlock : FoundationBlockData, IFilterBlock
    {
        [CultureSpecific]
        [Display(Name = "Filter title")]
        public virtual string FilterTitle { get; set; }

        [CultureSpecific]
        [Display(Name = "All condition text")]
        public virtual string AllConditionText { get; set; }
    }
}
