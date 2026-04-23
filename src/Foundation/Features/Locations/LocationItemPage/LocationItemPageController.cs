namespace Foundation.Features.Locations.LocationItemPage
{
    // EPiServer.Find removed: related location queries replaced with IContentRepository sibling lookups.
    // MoreLike, BoostMatching, and geo-distance ordering are not available without Find.
    public class LocationItemPageController : PageController<LocationItemPage>
    {
        private readonly IContentRepository _contentRepository;

        public LocationItemPageController(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public ActionResult Index(LocationItemPage currentPage)
        {
            var model = new LocationItemViewModel(currentPage);

            if (!ContentReference.IsNullOrEmpty(currentPage.Image))
            {
                model.Image = _contentRepository.Get<ImageData>(currentPage.Image);
            }

            // Load sibling locations from the parent LocationListPage.
            var parent = _contentRepository.Get<IContent>(currentPage.ParentLink);
            if (parent is LocationListPage.LocationListPage)
            {
                var siblings = _contentRepository
                    .GetChildren<LocationItemPage>(parent.ContentLink)
                    .Where(x => x.ContentLink != currentPage.ContentLink)
                    .ToList();

                model.LocationNavigation.ContinentLocations = siblings
                    .Where(x => string.Equals(x.Continent, currentPage.Continent, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(x => x.Name)
                    .ToList();

                model.LocationNavigation.CloseBy = siblings
                    .Where(x => string.Equals(x.Continent, currentPage.Continent, StringComparison.OrdinalIgnoreCase))
                    .Take(5)
                    .ToList();
            }

            var editingHints = ViewData.GetEditHints<LocationItemViewModel, LocationItemPage>();
            editingHints.AddFullRefreshFor(p => p.Image);

            return View(model);
        }
    }
}
