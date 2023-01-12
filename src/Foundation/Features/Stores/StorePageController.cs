namespace Foundation.Features.Stores
{
    public class StorePageController : PageController<StorePage>
    {
        private readonly IStoreService _storeService;

        public StorePageController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        public IActionResult Index(StorePage currentPage)
        {
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
        public IActionResult GetStoreLocator()
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
        public IActionResult SetDefaultStore(string storeCode)
        {
            if (!_storeService.SetCurrentStore(storeCode))
            {
                return StatusCode(400, "Unsupported");
            }

            return Json(new { returnUrl = Request.Headers["Referer"].ToString() });
        }
    }
}