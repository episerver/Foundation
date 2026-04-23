namespace Foundation.Features.Locations
{
    // EPiServer.Find removed: AddFilter/ApplyFilter removed (used ITypeSearch<> from Find).
    // Filter blocks remain as CMS content types but do not perform any filtering.
    public interface IFilterBlock
    {
        string FilterTitle { get; set; }
        string AllConditionText { get; set; }
    }
}
