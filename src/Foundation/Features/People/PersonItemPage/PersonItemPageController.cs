using EPiServer.Web.Mvc;
using Foundation.Find.Cms.Models.Pages;
using Foundation.Find.Cms.People.ViewModels;
using System.Web.Mvc;

namespace Foundation.Features.People.PersonListPage
{
    public class PersonItemPageController : PageController<PersonItemPage>
    {
        public ActionResult Index(PersonItemPage currentPage)
        {
            var model = new PersonItemViewModel(currentPage);
            return View(model);
        }
    }
}
