using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.MyOrganization.Organization;
using Foundation.Infrastructure.Cms;
using Foundation.Infrastructure.Cms.Settings;
using Foundation.Infrastructure.Commerce;
using Mediachase.Commerce.Customers;

namespace Foundation.Features.MyOrganization.SubOrganization
{
    [Authorize]
    public class SubOrganizationController : PageController<SubOrganizationPage>
    {
        private readonly IContentLoader _contentLoader;
        private readonly IOrganizationService _organizationService;
        private readonly IAddressBookService _addressService;
        private readonly ICookieService _cookieService;
        private readonly ISettingsService _settingsService;

        public SubOrganizationController(IOrganizationService organizationService,
            IContentLoader contentLoader,
            IAddressBookService addressService,
            ISettingsService settingsService,
            ICookieService cookieService)
        {
            _organizationService = organizationService;
            _contentLoader = contentLoader;
            _addressService = addressService;
            _settingsService = settingsService;
            _cookieService = cookieService;
        }

        public IActionResult Index(SubOrganizationPage currentPage)
        {
            var subOrg = Request.Query["suborg"];
            var viewModel = new SubOrganizationPageViewModel
            {
                CurrentContent = currentPage,
                SubOrganizationModel = _organizationService.GetSubOrganizationById(subOrg)
            };
            //Set selected suborganization
            _cookieService.Set(Constant.Fields.SelectedOrganization, subOrg);
            _cookieService.Set(Constant.Fields.SelectedNavOrganization, subOrg);

            if (viewModel.SubOrganizationModel == null)
            {
                var referencePages = _settingsService.GetSiteSettings<ReferencePageSettings>();
                return Redirect(UrlResolver.Current.GetUrl(referencePages?.OrganizationMainPage ?? ContentReference.StartPage));
            }
            viewModel.IsAdmin = CustomerContext.Current.CurrentContact.Properties[Constant.Fields.UserRole].Value.ToString() == Constant.UserRoles.Admin;

            return View(viewModel);
        }

        public IActionResult Edit(SubOrganizationPage currentPage)
        {
            var viewModel = new SubOrganizationPageViewModel
            {
                CurrentContent = currentPage,
                SubOrganizationModel = _organizationService.GetSubOrganizationById(Request.Query["suborg"])
            };
            if (viewModel.SubOrganizationModel == null)
            {
                var referencePages = _settingsService.GetSiteSettings<ReferencePageSettings>();
                return Redirect(UrlResolver.Current.GetUrl(referencePages?.OrganizationMainPage ?? ContentReference.StartPage));
            }
            if (viewModel.SubOrganizationModel.Locations.Count == 0)
            {
                viewModel.SubOrganizationModel.Locations.Add(new B2BAddressViewModel());
            }
            viewModel.SubOrganizationModel.CountryOptions = _addressService.GetAllCountries();
            return View(viewModel);
        }

        [HttpPost]
        [AllowDBWrite]
        [ValidateAntiForgeryToken]
        public IActionResult Save(SubOrganizationPageViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.SubOrganizationModel.Name))
            {
                ModelState.AddModelError("SubOrganization.Name", "SubOrganization Name is requried");
            }

            if (viewModel.SubOrganizationModel.OrganizationId != Guid.Empty)
            {
                //update the locations list
                var updatedLocations = new List<B2BAddressViewModel>();
                foreach (var location in viewModel.SubOrganizationModel.Locations)
                {
                    if (location.Name != "removed")
                    {
                        updatedLocations.Add(location);
                    }
                    else
                    {
                        if (location.AddressId != Guid.Empty)
                        {
                            _addressService.DeleteAddress(viewModel.SubOrganizationModel.OrganizationId.ToString(), location.AddressId.ToString());
                        }
                    }
                }
                viewModel.SubOrganizationModel.Locations = updatedLocations;
                _organizationService.UpdateSubOrganization(viewModel.SubOrganizationModel);
            }
            return RedirectToAction("Index", new { suborg = viewModel.SubOrganizationModel.OrganizationId });
        }

        public IActionResult DeleteAddress(SubOrganizationPage currentPage)
        {
            var subOrg = Request.Query["suborg"].ToString();
            var addressId = Request.Query["addressId"].ToString();
            if (string.IsNullOrEmpty(subOrg) || string.IsNullOrEmpty(addressId))
            {
                var referencePages = _settingsService.GetSiteSettings<ReferencePageSettings>();
                return Redirect(UrlResolver.Current.GetUrl(referencePages?.OrganizationMainPage ?? ContentReference.StartPage));
            }
            _addressService.DeleteAddress(subOrg, addressId);
            return RedirectToAction("Edit", new { suborg = subOrg });
        }
    }
}