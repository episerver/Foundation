using EPiServer.Web.Mvc;
using Foundation.Cms.Settings;
using Foundation.Features.Search;
using Foundation.Features.Settings;
using System.Web.Mvc;

namespace Foundation.Features.NewProducts
{
    public class NewProductsPageController : PageController<NewProductsPage>
    {
        private readonly ISearchService _searchService;
        private readonly ISettingsService _settingsService;

        public NewProductsPageController(ISearchService searchService,
           ISettingsService settingsService)
        {
            _searchService = searchService;
            _settingsService = settingsService;
        }

        public ActionResult Index(NewProductsPage currentPage, int page = 1)
        {
            var searchsettings = _settingsService.GetSiteSettings<SearchSettings>();
            var model = new NewProductsPageViewModel(currentPage)
            {
                ProductViewModels = _searchService.SearchNewProducts(currentPage, out var pages, searchsettings?.SearchCatalog ?? 0, page),
                PageNumber = page,
                Pages = pages
            };

            return View(model);
        }
    }
}