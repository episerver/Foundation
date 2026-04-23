using Foundation.Infrastructure.Cms.Settings;

namespace Foundation.Features.MyAccount.AddressBook
{
    public class NewAddressComponent : ViewComponent
    {
        private readonly IContentLoader _contentLoader;
        private readonly ISettingsService _settingsService;
        private readonly IAddressBookService _addressBookService;

        public NewAddressComponent(IContentLoader contentLoader, ISettingsService settingsService, IAddressBookService addressBookService)
        {
            _contentLoader = contentLoader;
            _settingsService = settingsService;
            _addressBookService = addressBookService;
        }

        public IViewComponentResult Invoke(string multiShipmentUrl)
        {
            var referenceSettings = _settingsService.GetSiteSettings<ReferencePageSettings>();
            var addressBookPage = _contentLoader.Get<PageData>(referenceSettings.AddressBookPage) as AddressBookPage;
            var model = new AddressViewModel(addressBookPage)
            {
                Address = new AddressModel()
            };
            _addressBookService.LoadAddress(model.Address);
            ViewData["IsInMultiShipment"] = true;
            ViewData["MultiShipmentUrl"] = multiShipmentUrl;

            return View("~/Features/MyAccount/AddressBook/EditAddress.cshtml", model);
        }
    }
}
