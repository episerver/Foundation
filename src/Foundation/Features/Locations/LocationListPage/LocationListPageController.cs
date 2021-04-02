using EPiServer;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;
using EPiServer.Personalization;
using EPiServer.Tracking.PageView;
using EPiServer.Web.Mvc;
using Foundation.Find;
using System.Web.Mvc;

namespace Foundation.Features.Locations.LocationListPage
{
    public class LocationListPageController : PageController<LocationListPage>
    {
        private readonly IContentLoader _contentLoader;

        public LocationListPageController(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        [PageViewTracking]
        public ActionResult Index(LocationListPage currentPage)
        {
            var query = SearchClient.Instance.Search<LocationItemPage.LocationItemPage>()
                .PublishedInCurrentLanguage()
                .FilterOnReadAccess();

            if (currentPage.FilterArea != null)
            {
                foreach (var filterBlock in currentPage.FilterArea.FilteredItems)
                {
                    var b = _contentLoader.Get<BlockData>(filterBlock.ContentLink) as IFilterBlock;
                    if (b != null)
                    {
                        query = b.AddFilter(query);
                    }
                }

                foreach (var filterBlock in currentPage.FilterArea.FilteredItems)
                {
                    var b = _contentLoader.Get<BlockData>(filterBlock.ContentLink) as IFilterBlock;
                    if (b != null)
                    {
                        query = b.ApplyFilter(query, Request.QueryString);
                    }
                }
            }

            var locations = query.OrderBy(x => x.PageName)
                                    .Take(500)
                                    .StaticallyCacheFor(new System.TimeSpan(0, 1, 0)).GetContentResult();

            var model = new LocationListViewModel(currentPage)
            {
                Locations = locations,
                MapCenter = GetMapCenter(),
                UserLocation = GeoPosition.GetUsersLocation(),
                QueryString = Request.QueryString
            };

            return View(model);
        }

        private static GeoCoordinate GetMapCenter()
        {
            var userLocation = GeoPosition.GetUsersPosition();
            if (userLocation != null)
            {
                return new GeoCoordinate(30, userLocation.Longitude);
            }
            return new GeoCoordinate(30, 0);
        }
    }
}