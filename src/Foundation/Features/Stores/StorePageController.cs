using EPiServer.Web.Mvc;
using Foundation.Cms.Personalization;
using Foundation.Commerce.Catalog;
using Foundation.Commerce.Catalog.ViewModels;
using Foundation.Commerce.Models.Pages;
using Foundation.Commerce.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.Stores
{
    public class StorePageController : PageController<StorePage>
    {
        private readonly IStoreService _storeService;
        private readonly ICmsTrackingService _trackingService;

        public StorePageController(IStoreService storeService,
            ICmsTrackingService trackingService)
        {
            _storeService = storeService;
            _trackingService = trackingService;
        }

        public async Task<ActionResult> Index(StorePage currentPage)
        {
            await _trackingService.PageViewed(HttpContext, currentPage);
            var currentStore = _storeService.GetCurrentStoreViewModel();
            var storesViewModel = new StoreViewModel
            {
                ShowDelivery = false,
                SelectedStore = currentStore != null ? currentStore.Code : "",
                SelectedStoreName = currentStore != null ? currentStore.Name : "",
                Stores = _storeService.GetAllStoreViewModels(),
            };

            var store = new StorePageViewModel(currentPage)
            {
                StoreViewModel = storesViewModel
            };

            return View(store);
        }

        [HttpGet]
        public ActionResult GetStoreLocator()
        {
            var currentStore = _storeService.GetCurrentStoreViewModel();
            var storesViewModel = new StoreViewModel
            {
                ShowDelivery = false,
                SelectedStore = currentStore != null ? currentStore.Code : "",
                SelectedStoreName = currentStore != null ? currentStore.Name : "",
                Stores = _storeService.GetAllStoreViewModels(),
            };
            return PartialView("_Stores", storesViewModel);
        }

        [HttpPost]
        public ActionResult SetDefaultStore(string storeCode)
        {
            if (!_storeService.SetCurrentStore(storeCode))
            {
                return new HttpStatusCodeResult(400, "Unsupported");
            }

            return Json(new { returnUrl = Request.UrlReferrer.ToString() });
        }
    }
}