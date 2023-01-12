using Foundation.Features.Home;
using Foundation.Features.MyAccount.AddressBook;

namespace Foundation.Features.Header
{
    public class HeaderController : Controller
    {
        private readonly IHeaderViewModelFactory _headerViewModelFactory;
        private readonly IContentRouteHelper _contentRouteHelper;
        private readonly IAddressBookService _addressBookService;

        public HeaderController(IHeaderViewModelFactory headerViewModelFactory,
            IContentRouteHelper contentRouteHelper,
            IAddressBookService addressBookService)
        {
            _headerViewModelFactory = headerViewModelFactory;
            _contentRouteHelper = contentRouteHelper;
            _addressBookService = addressBookService;
        }

        public ActionResult GetHeader(HomePage homePage)
        {
            var content = _contentRouteHelper.Content;
            return PartialView("_Header", _headerViewModelFactory.CreateHeaderViewModel(content, homePage));
        }

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