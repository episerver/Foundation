using Foundation.Features.Home;
using Foundation.Infrastructure.Cms.Settings;
using Foundation.Infrastructure.Commerce.Customer.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Foundation.Features.MyAccount.AddressBook
{
    [Authorize]
    public class AddressBookController : PageController<AddressBookPage>
    {
        private readonly IContentLoader _contentLoader;
        private readonly IAddressBookService _addressBookService;
        private readonly LocalizationService _localizationService;
        private readonly ICustomerService _customerService;
        private readonly ISettingsService _settingsService;
        private readonly IPageRouteHelper _pageRouteHelper;
        private readonly IUrlResolver _urlResolver;

        public AddressBookController(
            IContentLoader contentLoader,
            IAddressBookService addressBookService,
            LocalizationService localizationService,
            ICustomerService customerService,
            ISettingsService settingsService,
            IPageRouteHelper pageRouteHelper,
            IUrlResolver urlResolver)
        {
            _contentLoader = contentLoader;
            _addressBookService = addressBookService;
            _localizationService = localizationService;
            _customerService = customerService;
            _settingsService = settingsService;
            _pageRouteHelper = pageRouteHelper;
            _urlResolver = urlResolver;
        }

        [HttpGet]
        public IActionResult Index(AddressBookPage currentPage) => View(GetAddressBookViewModel(currentPage));

        [HttpGet]
        public IActionResult EditForm(AddressBookPage currentPage, string addressId)
        {
            var viewModel = new AddressViewModel(currentPage)
            {
                Address = new AddressModel
                {
                    AddressId = addressId,
                },
                CurrentContent = currentPage
            };

            _addressBookService.LoadAddress(viewModel.Address);

            return AddressEditView(viewModel);
        }

        // Use NewAddress component
        //[ChildActionOnly]
        //public PartialViewResult AddNewAddress(string multiShipmentUrl)
        //{
        //    var referenceSettings = _settingsService.GetSiteSettings<ReferencePageSettings>();
        //    var addressBookPage = _contentLoader.Get<PageData>(referenceSettings.AddressBookPage) as AddressBookPage;
        //    var model = new AddressViewModel(addressBookPage)
        //    {
        //        Address = new AddressModel()
        //    };
        //    _addressBookService.LoadAddress(model.Address);
        //    ViewData["IsInMultiShipment"] = true;
        //    ViewData["MultiShipmentUrl"] = multiShipmentUrl;

        //    return PartialView("EditAddress", model);
        //}

        [HttpPost]
        [AllowAnonymous]
        public IActionResult GetRegionsForCountry(string countryCode, string region, string htmlPrefix)
        {
            ViewData.TemplateInfo.HtmlFieldPrefix = htmlPrefix;
            var countryRegion = new CountryRegionViewModel
            {
                RegionOptions = _addressBookService.GetRegionsByCountryCode(countryCode),
                Region = region
            };

            return PartialView("_AddressRegion", countryRegion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save([FromForm] AddressViewModel viewModel, string returnUrl = "")
        {
            var referenceSettings = _settingsService.GetSiteSettings<ReferencePageSettings>();
            if (string.IsNullOrEmpty(viewModel.Address.Name))
            {
                ModelState.AddModelError("Address.Name", _localizationService.GetString("/Shared/Address/Form/Empty/Name", "Name is required"));
            }

            if (!_addressBookService.CanSave(viewModel.Address))
            {
                ModelState.AddModelError("Address.Name", _localizationService.GetString("/AddressBook/Form/Error/ExistingAddress", "An address with the same name already exists"));
            }

            if (!ModelState.IsValid)
            {
                _addressBookService.LoadAddress(viewModel.Address);

                return AddressEditView(viewModel);
            }

            _addressBookService.Save(viewModel.Address);

            if (string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(_urlResolver.GetUrl(new ContentReference(viewModel.ContentReference)));
            }

            return Redirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(string addressId)
        {
            _addressBookService.Delete(addressId);
            var referenceSettings = _settingsService.GetSiteSettings<ReferencePageSettings>();
            var addressRootPage = referenceSettings?.AddressBookPage ?? ContentReference.StartPage;
            return Redirect(_urlResolver.GetUrl(new ContentReference(addressRootPage.ID)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetPreferredShippingAddress(string addressId)
        {
            _addressBookService.SetPreferredShippingAddress(addressId);
            var referenceSettings = _settingsService.GetSiteSettings<ReferencePageSettings>();
            var addressRootPage = referenceSettings?.AddressBookPage ?? ContentReference.StartPage;
            return Redirect(_urlResolver.GetUrl(new ContentReference(addressRootPage.ID)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetPreferredBillingAddress(string addressId)
        {
            _addressBookService.SetPreferredBillingAddress(addressId);
            var referenceSettings = _settingsService.GetSiteSettings<ReferencePageSettings>();
            var addressRootPage = referenceSettings?.AddressBookPage ?? ContentReference.StartPage;
            return Redirect(_urlResolver.GetUrl(new ContentReference(addressRootPage.ID)));
        }

        public IActionResult OnSaveException(ExceptionContext filterContext)
        {
            //var currentPage = filterContext.RequestContext.GetRoutedData<AddressBookPage>();
            var currentPage = _pageRouteHelper.Page as AddressBookPage;

            var viewModel = new AddressViewModel
            {
                Address = new AddressModel
                {
                    AddressId = filterContext.HttpContext.Request.Form["addressId"],
                    ErrorMessage = filterContext.Exception.Message,
                },
                CurrentContent = currentPage
            };

            _addressBookService.LoadAddress(viewModel.Address);

            return AddressEditView(viewModel);
        }

        private IActionResult AddressEditView(AddressViewModel viewModel)
        {
            //if (Request.IsAjaxRequest())
            //{
            //    return PartialView("~/Features/MyAccount/AddressBook/ModalAddressDialog.cshtml", viewModel);
            //}

            return View("~/Features/MyAccount/AddressBook/EditForm.cshtml", viewModel);
        }

        private HomePage GetStartPage() => _contentLoader.Get<PageData>(ContentReference.StartPage) as HomePage;

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetRegions(string countryCode, string region, string inputName)
        {
            var countryRegion = new CountryRegionViewModel
            {
                RegionOptions = _addressBookService.GetRegionsByCountryCode(countryCode),
                Region = region
            };
            ViewData["Name"] = inputName;
            return PartialView("~/Features/Shared/Views/DisplayTemplates/CountryRegionViewModel.cshtml", countryRegion);
        }

        public AddressCollectionViewModel GetAddressBookViewModel(AddressBookPage addressBookPage)
        {
            return new AddressCollectionViewModel(addressBookPage)
            {
                CurrentContent = addressBookPage,
                Addresses = _customerService.GetCurrentContact()?
                    .Contact?
                    .ContactAddresses
                    .Select(x => _addressBookService.ConvertAddress(x))
                    ?? Enumerable.Empty<AddressModel>()
            };
        }
    }
}