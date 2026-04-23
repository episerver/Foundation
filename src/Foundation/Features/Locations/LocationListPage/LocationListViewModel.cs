namespace Foundation.Features.Locations.LocationListPage
{
    public class LocationListViewModel : ContentViewModel<LocationListPage>
    {
        public LocationListViewModel(LocationListPage currentPage) : base(currentPage)
        {
        }

        // EPiServer.Find removed: IContentResult replaced with IEnumerable.
        // MapCenter/UserLocation removed: GeoCoordinate and IGeolocationResult required EPiServer.Find/Personalization geo APIs.
        public IEnumerable<LocationItemPage.LocationItemPage> Locations { get; set; }
        public IQueryCollection QueryString { get; set; }
    }
}
