using Foundation.Features.People.PersonItemPage;
using Foundation.Infrastructure.Cms;
using Foundation.Infrastructure.Cms.Settings;

namespace Foundation.Features.People.PersonListPage
{
    // EPiServer.Find removed: person search replaced with IContentLoader descendant lookup + in-memory filtering.
    // Wildcard name search is replaced with case-insensitive Contains().
    public class PersonListPageController : PageController<PersonList>
    {
        private readonly ISettingsService _settingsService;
        private readonly IContentLoader _contentLoader;

        public PersonListPageController(ISettingsService settingsService, IContentLoader contentLoader)
        {
            _settingsService = settingsService;
            _contentLoader = contentLoader;
        }

        public ActionResult Index(PersonList currentPage)
        {
            var queryString = Request.Query;

            IEnumerable<PersonPage> allPersons = _contentLoader
                .GetDescendents(currentPage.ContentLink)
                .Select(r =>
                {
                    try { return _contentLoader.Get<IContent>(r) as PersonPage; }
                    catch { return null; }
                })
                .Where(p => p != null);

            var nameFilter = queryString["name"].ToString();
            var sectorFilter = queryString["sector"].ToString();
            var locationFilter = queryString["location"].ToString();

            if (!string.IsNullOrWhiteSpace(nameFilter))
                allPersons = allPersons.Where(p => p.Name.Contains(nameFilter, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(sectorFilter))
                allPersons = allPersons.Where(p => string.Equals(p.Sector, sectorFilter, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(locationFilter))
                allPersons = allPersons.Where(p => string.Equals(p.Location, locationFilter, StringComparison.OrdinalIgnoreCase));

            var persons = allPersons.OrderBy(p => p.Name).Take(500).ToList();

            var settingPage = _settingsService.GetSiteSettings<CollectionSettings>();

            var model = new PersonListViewModel(currentPage)
            {
                Persons = persons,
                Sectors = settingPage?.Sectors?.OrderBy(x => x.Text).ToList() ?? new List<SelectionItem>(),
                Locations = settingPage?.Locations?.OrderBy(x => x.Text).ToList() ?? new List<SelectionItem>(),
                Names = persons.Select(p => p.Name).Distinct().OrderBy(x => x).ToList()
            };

            return View(model);
        }
    }
}
