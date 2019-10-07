using EPiServer.Find.Cms;
using EPiServer.Personalization;
using Foundation.Cms.ViewModels;
using Foundation.Find.Cms.Models.Pages;
using System.Collections.Specialized;

namespace Foundation.Find.Cms.Locations.ViewModels
{
    public class LocationListViewModel : ContentViewModel<LocationListPage>
    {
        public LocationListViewModel(LocationListPage currentPage) : base(currentPage) { }

        public GeoCoordinate MapCenter { get; set; }
        public IGeolocationResult UserLocation { get; set; }
        public IContentResult<LocationItemPage> Locations { get; set; }
        public NameValueCollection QueryString { get; set; }
    }
}