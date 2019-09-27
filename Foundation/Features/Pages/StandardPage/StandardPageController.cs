using EPiServer.DataAbstraction;
using EPiServer.Web.Mvc;
using Foundation.Cms.Personalization;
using Foundation.Cms.ViewModels.Pages;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.Pages.StandardPage
{
    public class StandardPageController : PageController<Cms.Pages.StandardPage>
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly ICmsTrackingService _trackingService;

        public StandardPageController(CategoryRepository categoryRepository,
            ICmsTrackingService trackingService)
        {
            _categoryRepository = categoryRepository;
            _trackingService = trackingService;
        }

        public async Task<ActionResult> Index(Cms.Pages.StandardPage currentPage)
        {
            await _trackingService.PageViewed(HttpContext, currentPage);
            var model = StandardPageViewModel.Create(currentPage, _categoryRepository);
            return View(model);
        }
    }
}