using EPiServer.Find.Cms;
using EPiServer.Personalization;

namespace Foundation.Features.Locations.LocationListPage
{
    public class LocationListViewModel : ContentViewModel<LocationListPage>
    {
        public LocationListViewModel(LocationListPage currentPage) : base(currentPage)
        {
        }

        public GeoCoordinate MapCenter { get; set; }
        public IGeolocationResult UserLocation { get; set; }
        public IContentResult<LocationItemPage.LocationItemPage> Locations { get; set; }
        public IQueryCollection QueryString { get; set; }
    }
}