using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Cms;
using Foundation.Cms.Attributes;
using Foundation.Cms.Settings;
using Foundation.Commerce;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.MyOrganization.Organization;
using Foundation.Features.Settings;
using Mediachase.Commerce.Customers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Features.MyOrganization.SubOrganization
{
    [Authorize]
    public class SubOrganizationController : PageController<SubOrganizationPage>
    {
        private readonly IContentLoader _contentLoader;
        private readonly IOrganizationService _organizationService;
        private readonly IAddressBookService _addressService;
        private readonly CookieService _cookieService = new CookieService();
        private readonly ISettingsService _settingsService;

        public SubOrganizationController(IOrganizationService organizationService,
            IContentLoader contentLoader,
            IAddressBookService addressService,
            ISettingsService settingsService)
        {
            _organizationService = organizationService;
            _contentLoader = contentLoader;
            _addressService = addressService;
            _settingsService = settingsService;
        }

        public ActionResult Index(SubOrganizationPage currentPage)
        {
            var viewModel = new SubOrganizationPageViewModel
            {
                CurrentContent = currentPage,
                SubOrganizationModel = _organizationService.GetSubOrganizationById(Request["suborg"])
            };
            //Set selected suborganization
            _cookieService.Set(Constant.Fields.SelectedOrganization, Request["suborg"]);
            _cookieService.Set(Constant.Fields.SelectedNavOrganization, Request["suborg"]);

            if (viewModel.SubOrganizationModel == null)
            {
                var referencePages = _settingsService.GetSiteSettings<ReferencePageSettings>();
                return Redirect(UrlResolver.Current.GetUrl(referencePages?.OrganizationMainPage ?? ContentReference.StartPage));
            }
            viewModel.IsAdmin = CustomerContext.Current.CurrentContact.Properties[Constant.Fields.UserRole].Value.ToString() == Constant.UserRoles.Admin;

            return View(viewModel);
        }

        public ActionResult Edit(SubOrganizationPage currentPage)
        {
            var viewModel = new SubOrganizationPageViewModel
            {
                CurrentContent = currentPage,
                SubOrganizationModel = _organizationService.GetSubOrganizationById(Request["suborg"])
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
        public ActionResult Save(SubOrganizationPageViewModel viewModel)
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

        public ActionResult DeleteAddress(SubOrganizationPage currentPage)
        {
            if (Request["suborg"] == null || Request["addressId"] == null)
            {
                var referencePages = _settingsService.GetSiteSettings<ReferencePageSettings>();
                return Redirect(UrlResolver.Current.GetUrl(referencePages?.OrganizationMainPage ?? ContentReference.StartPage));
            }
            _addressService.DeleteAddress(Request["suborg"], Request["addressId"]);
            return RedirectToAction("Edit", new { suborg = Request["suborg"] });
        }
    }
}