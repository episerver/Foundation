using EPiServer.Web.Mvc;
using Foundation.Cms.Settings;
using Foundation.Features.Search;
using Foundation.Features.Settings;
using System.Web.Mvc;

namespace Foundation.Features.Sales
{
    public class SalesPageController : PageController<SalesPage>
    {
        private readonly ISearchService _searchService;
        private readonly ISettingsService _settingsService;

        public SalesPageController(ISearchService searchService,
            ISettingsService settingsService)
        {
            _searchService = searchService;
            _settingsService = settingsService;
        }

        public ActionResult Index(SalesPage currentPage, int page = 1)
        {
            var searchSettings = _settingsService.GetSiteSettings<SearchSettings>();
            var model = new SalesPageViewModel(currentPage)
            {
                ProductViewModels = _searchService.SearchOnSale(currentPage, out var pages, searchSettings?.SearchCatalog ?? 0, page, 12),
                PageNumber = page,
                Pages = pages
            };

            return View(model);
        }
    }
}