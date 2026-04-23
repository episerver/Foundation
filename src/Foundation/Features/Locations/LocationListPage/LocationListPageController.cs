using System.Globalization;

namespace Foundation.Features.Locations.LocationListPage
{
    // EPiServer.Find removed: location browsing uses IContentLoader with in-memory filtering.
    // Supported filters: continent (checkbox, ?continent=Europe,Asia) and temperature (?t=min,max).
    // Geo-distance filtering is not available without Find.
    public class LocationListPageController : PageController<LocationListPage>
    {
        private readonly IContentLoader _contentLoader;

        public LocationListPageController(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public ActionResult Index(LocationListPage currentPage)
        {
            var locations = _contentLoader
                .GetDescendents(currentPage.ContentLink)
                .Select(r =>
                {
                    try { return _contentLoader.Get<IContent>(r); }
                    catch { return null; }
                })
                .OfType<LocationItemPage.LocationItemPage>()
                .OrderBy(x => x.Name)
                .Take(500)
                .ToList();

            // Continent filter: ?continent=Europe,Asia (comma-separated)
            var continentParam = Request.Query["continent"].ToString();
            if (!string.IsNullOrWhiteSpace(continentParam))
            {
                var selected = continentParam
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(c => c.Trim())
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);
                locations = locations
                    .Where(l => selected.Contains(l.Continent ?? ""))
                    .ToList();
            }

            // Temperature filter: ?t=min,max  (e.g. ?t=-10,30) — set by the JS slider
            var tempParam = Request.Query["t"].ToString();
            if (!string.IsNullOrWhiteSpace(tempParam))
            {
                var parts = tempParam.Split(',');
                if (parts.Length == 2 &&
                    double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double tempMin) &&
                    double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double tempMax))
                {
                    locations = locations
                        .Where(l => l.AvgTemp.HasValue && l.AvgTemp.Value >= tempMin && l.AvgTemp.Value <= tempMax)
                        .ToList();
                }
            }

            var model = new LocationListViewModel(currentPage)
            {
                Locations = locations,
                QueryString = Request.Query
            };

            return View(model);
        }
    }
}
