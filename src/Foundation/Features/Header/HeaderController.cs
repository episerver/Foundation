using EPiServer;
using EPiServer.Web.Routing;
using Foundation.Features.Home;
using Foundation.Features.MyAccount.AddressBook;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Features.Header
{
    public class HeaderController : Controller
    {
        private readonly IHeaderViewModelFactory _headerViewModelFactory;
        private readonly IContentRouteHelper _contentRouteHelper;
        private readonly IContentLoader _contentLoader;
        private readonly IAddressBookService _addressBookService;

        public HeaderController(IHeaderViewModelFactory headerViewModelFactory,
            IContentRouteHelper contentRouteHelper,
            IContentLoader contentLoader,
            IAddressBookService addressBookService)
        {
            _headerViewModelFactory = headerViewModelFactory;
            _contentRouteHelper = contentRouteHelper;
            _contentLoader = contentLoader;
            _addressBookService = addressBookService;
        }

        [ChildActionOnly]
        public ActionResult GetHeader(HomePage homePage)
        {
            var content = _contentRouteHelper.Content;
            return PartialView("_Header", _headerViewModelFactory.CreateHeaderViewModel(content, homePage));
        }

        [ChildActionOnly]
        public ActionResult GetHeaderLogoOnly()
        {
            return PartialView("_HeaderLogo", _headerViewModelFactory.CreateHeaderLogoViewModel());
        }

        public ActionResult GetCountryOptions(string inputName)
        {
            var model = new List<CountryViewModel>() { new CountryViewModel() { Name = "Select", Code = "undefined" } };
            model.AddRange(_addressBookService.GetAllCountries());
            ViewData["Name"] = inputName;
            return PartialView("~/Features/Shared/Views/DisplayTemplates/CountryOptions.cshtml", model);
        }
    }
}