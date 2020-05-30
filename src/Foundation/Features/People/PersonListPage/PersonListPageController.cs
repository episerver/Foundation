using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;
using EPiServer.Web.Mvc;
using Foundation.Cms;
using Foundation.Cms.Pages;
using Foundation.Find.Cms;
using Foundation.Find.Cms.Models.Pages;
using Foundation.Find.Cms.People.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Foundation.Features.People.PersonListPage
{
    public class PersonListPageController : PageController<Find.Cms.Models.Pages.PersonListPage>
    {
        private readonly IContentLoader _contentLoader;
        private readonly IContentTypeRepository _contentTypeRepository;
        
        public PersonListPageController(IContentLoader contentLoader, IContentTypeRepository contentTypeRepository)
        {
            _contentLoader = contentLoader;
            _contentTypeRepository = contentTypeRepository;
        }

        public ActionResult Index(Find.Cms.Models.Pages.PersonListPage currentPage)
        {
            var queryString = Request.QueryString;
            var query = SearchClient.Instance.Search<Find.Cms.Models.Pages.PersonItemPage>();

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

            var settingPage = _contentLoader.Get<CmsHomePage>(ContentReference.StartPage);

            var model = new PersonListViewModel(currentPage) { 
               Persons = persons,
               Sectors = settingPage?.Sectors?.OrderBy(x => x.Text).ToList() ?? new List<SelectionItem>(),
               Locations = settingPage?.Locations?.OrderBy(x => x.Text).ToList() ?? new List<SelectionItem>(),
               Names = GetNames(persons)
            };

            return View(model);
        }

        public List<string> GetNames(IContentResult<PersonItemPage> persons)
        {
            List<string> lstNames = new List<string>();
            foreach (var person in persons)
            {
                lstNames.Add(person.Name);
            }
            return lstNames.Distinct().OrderBy(x => x).ToList();
        }
    }
}
