using EPiServer.Find.Cms;
using EPiServer.Personalization;
using Foundation.Features.Shared;
using System.Collections.Specialized;

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
        public NameValueCollection QueryString { get; set; }
    }
}