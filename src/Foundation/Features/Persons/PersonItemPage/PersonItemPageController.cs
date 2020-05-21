using EPiServer;
using EPiServer.Web.Mvc;
using Foundation.Find.Cms.Persons.ViewModels;
using System.Web.Mvc;
namespace Foundation.Features.Persons.PersonListPage
{
    public class PersonItemPageController : PageController<Find.Cms.Models.Pages.PersonItemPage>
    {
        private readonly IContentLoader _contentLoader;

        public PersonItemPageController(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public ActionResult Index(Find.Cms.Models.Pages.PersonItemPage currentPage)
        {
            var model = new PersonItemViewModel(currentPage);
            return View(model);
        }
    }
}
