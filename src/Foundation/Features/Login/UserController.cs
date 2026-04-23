using Foundation.Features.MyAccount.AddressBook;
using Foundation.Infrastructure.Attributes;
using Foundation.Infrastructure.Cms.Settings;

namespace Foundation.Features.Login
{
    public class UserController : Controller
    {
        private readonly IAddressBookService _addressBookService;
        private readonly LocalizationService _localizationService;
        private readonly ISettingsService _settingsService;

        public UserController(IAddressBookService addressBookService,
            LocalizationService localizationService,
            ISettingsService settingsService)
        {
            _addressBookService = addressBookService;
            _localizationService = localizationService;
            _settingsService = settingsService;
        }

        [OnlyAnonymous]
        public ActionResult Index(string returnUrl)
        {
            var model = new UserViewModel();
            var referenceSettings = _settingsService.GetSiteSettings<ReferencePageSettings>();
            var layoutSettings = _settingsService.GetSiteSettings<LayoutSettings>();
            model.Logo = Url.ContentUrl(layoutSettings?.SiteLogo ?? ContentReference.StartPage);
            model.ResetPasswordUrl = Url.ContentUrl(referenceSettings?.ResetPasswordPage ?? ContentReference.StartPage);
            model.Title = "Login";
            model.LoginViewModel.ReturnUrl = returnUrl;
            return View(model);
        }

        [OnlyAnonymous]
        public ActionResult Register()
        {
            var model = new UserViewModel();
            _addressBookService.LoadAddress(model.RegisterAccountViewModel.Address);
            model.RegisterAccountViewModel.Address.Name = _localizationService.GetString("/Shared/Address/DefaultAddressName", "Default Address");
            var layoutSettings = _settingsService.GetSiteSettings<LayoutSettings>();
            model.Logo = Url.ContentUrl(layoutSettings?.SiteLogo ?? ContentReference.StartPage);
            model.Title = "Register";
            return View(model);
        }
    }
}