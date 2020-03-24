using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.Web.Mvc.Html;
using Foundation.Attributes;
using Foundation.Commerce.Customer.Services;
using Foundation.Demo.Models;
using System.Web.Mvc;

namespace Foundation.Features.Login
{
    [OnlyAnonymous]
    public class UserController : Controller
    {
        private readonly IContentLoader _contentLoader;
        private readonly IAddressBookService _addressBookService;
        private readonly LocalizationService _localizationService;

        public UserController(IContentLoader contentLoader, IAddressBookService addressBookService, LocalizationService localizationService)
        {
            _contentLoader = contentLoader;
            _addressBookService = addressBookService;
            _localizationService = localizationService;
        }

        public ActionResult Index(string returnUrl)
        {
            var model = new UserViewModel();
            _contentLoader.TryGet(ContentReference.StartPage, out DemoHomePage homePage);

            model.Logo = Url.ContentUrl(homePage?.SiteLogo);
            model.ResetPasswordUrl = Url.ContentUrl(homePage?.ResetPasswordPage);
            model.Title = "Login";
            model.LoginViewModel.ReturnUrl = returnUrl;
            return View(model);
        }

        public ActionResult Register()
        {
            var model = new UserViewModel();
            _addressBookService.LoadAddress(model.RegisterAccountViewModel.Address);
            model.RegisterAccountViewModel.Address.Name = _localizationService.GetString("/Shared/Address/DefaultAddressName", "Default Address");
            var homePage = _contentLoader.Get<PageData>(ContentReference.StartPage) as DemoHomePage;
            model.Logo = Url.ContentUrl(homePage?.SiteLogo);
            model.Title = "Register";
            return View(model);
        }
    }
}