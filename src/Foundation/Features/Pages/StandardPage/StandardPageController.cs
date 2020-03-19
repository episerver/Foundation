using EPiServer.DataAbstraction;
using EPiServer.Web.Mvc;
using Foundation.Cms.ViewModels.Pages;
using System.Web.Mvc;

namespace Foundation.Features.Pages.StandardPage
{
    public class StandardPageController : PageController<Cms.Pages.StandardPage>
    {
        private readonly CategoryRepository _categoryRepository;

        public StandardPageController(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public ActionResult Index(Cms.Pages.StandardPage currentPage)
        {
            var model = StandardPageViewModel.Create(currentPage, _categoryRepository);
            return View(model);
        }
    }
}