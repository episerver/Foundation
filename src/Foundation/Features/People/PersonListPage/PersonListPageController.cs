using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;
using EPiServer.Web.Mvc;
using Foundation.Cms;
using Foundation.Cms.Settings;
using Foundation.Features.People.PersonItemPage;
using Foundation.Features.Settings;
using Foundation.Find;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Foundation.Features.People.PersonListPage
{
    public class PersonListPageController : PageController<PersonList>
    {
        private readonly ISettingsService _settingsService;

        public PersonListPageController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public ActionResult Index(PersonList currentPage)
        {
            var queryString = Request.QueryString;
            var query = SearchClient.Instance.Search<PersonPage>();

            if (!string.IsNullOrWhiteSpace(queryString.Get("name")))
            {
                query = query.AddWildCardQuery(queryString.Get("name"), x => x.Name);
            }

            if (!string.IsNullOrWhiteSpace(queryString.Get("sector")))
            {
                query = query.Filter(x => x.Sector.Match(queryString.Get("sector")));
            }

            if (!string.IsNullOrWhiteSpace(queryString.Get("location")))
            {
                query = query.Filter(x => x.Location.Match(queryString.Get("location")));
            }

            var persons = query.OrderBy(x => x.PageName)
                                    .Take(500)
                                    .GetContentResult();

            var settingPage = _settingsService.GetSiteSettings<CollectionSettings>();

            var model = new PersonListViewModel(currentPage)
            {
                Persons = persons,
                Sectors = settingPage?.Sectors?.OrderBy(x => x.Text).ToList() ?? new List<SelectionItem>(),
                Locations = settingPage?.Locations?.OrderBy(x => x.Text).ToList() ?? new List<SelectionItem>(),
                Names = GetNames(persons)
            };

            return View(model);
        }

        public List<string> GetNames(IContentResult<PersonPage> persons)
        {
            var lstNames = new List<string>();
            foreach (var person in persons)
            {
                lstNames.Add(person.Name);
            }
            return lstNames.Distinct().OrderBy(x => x).ToList();
        }
    }
}
