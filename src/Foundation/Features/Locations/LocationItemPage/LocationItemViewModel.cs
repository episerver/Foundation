namespace Foundation.Features.Locations.LocationItemPage
{
    public class LocationItemViewModel : ContentViewModel<LocationItemPage>
    {
        public LocationItemViewModel(LocationItemPage currentPage) : base(currentPage)
        {
            LocationNavigation = new LocationNavigationModel
            {
                CurrentLocation = currentPage
            };
        }

        public ImageData Image { get; set; }
        public LocationNavigationModel LocationNavigation { get; set; }
        public IEnumerable<LocationItemPage> SimilarLocations { get; set; }
        //public IEnumerable<StandardCategory> Tags { get; set; }
    }

    public class LocationNavigationModel
    {
        public LocationNavigationModel()
        {
            CloseBy = Enumerable.Empty<LocationItemPage>();
            ContinentLocations = Enumerable.Empty<LocationItemPage>();
        }

        public IEnumerable<LocationItemPage> CloseBy { get; set; }
        public IEnumerable<LocationItemPage> ContinentLocations { get; set; }
        public LocationItemPage CurrentLocation { get; set; }
    }
}