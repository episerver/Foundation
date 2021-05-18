using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
namespace Foundation.Features.People.PersonItemPage
{
    public class PersonItemPageController : PageController<PersonPage>
    {
        public ActionResult Index(PersonPage currentPage)
        {
            var model = new PersonItemViewModel(currentPage);
            return View(model);
        }
    }
}
