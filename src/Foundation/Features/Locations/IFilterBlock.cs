using EPiServer.Find;

namespace Foundation.Features.Locations
{
    public interface IFilterBlock
    {
        string FilterTitle { get; set; }

        string AllConditionText { get; set; }

        ITypeSearch<LocationItemPage.LocationItemPage> AddFilter(ITypeSearch<LocationItemPage.LocationItemPage> query);

        ITypeSearch<LocationItemPage.LocationItemPage> ApplyFilter(ITypeSearch<LocationItemPage.LocationItemPage> query, IQueryCollection filters);
    }
}