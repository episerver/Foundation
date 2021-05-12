using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;
using EPiServer.Web.Mvc;
using Foundation.Features.People.PersonItemPage;
using Foundation.Features.Settings;
using Foundation.Infrastructure.Cms;
using Foundation.Infrastructure.Cms.Settings;
using Foundation.Infrastructure.Find;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

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
            var queryString = Request.Query;
            var query = SearchClient.Instance.Search<PersonPage>();

            if (!string.IsNullOrWhiteSpace(queryString["name"].ToString()))
            {
                query = query.AddWildCardQuery(queryString["name"].ToString(), x => x.Name);
            }

            if (!string.IsNullOrWhiteSpace(queryString["sector"].ToString()))
            {
                query = query.Filter(x => x.Sector.Match(queryString["sector"].ToString()));
            }

            if (!string.IsNullOrWhiteSpace(queryString["location"].ToString()))
            {
                query = query.Filter(x => x.Location.Match(queryString["location"].ToString()));
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
