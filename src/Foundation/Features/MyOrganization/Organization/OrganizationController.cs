using EPiServer.Web.Mvc;
using Foundation.Cms;
using Foundation.Cms.Attributes;
using Foundation.Cms.Extensions;
using Foundation.Cms.Settings;
using Foundation.Commerce;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.MyOrganization.Budgeting;
using Foundation.Features.MyOrganization.SubOrganization;
using Foundation.Features.Settings;
using Mediachase.Commerce.Customers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.MyOrganization.Organization
{
    [Authorize]
    public class OrganizationController : PageController<OrganizationPage>
    {
        private readonly IOrganizationService _organizationService;
        private readonly IAddressBookService _addressService;
        private readonly IBudgetService _budgetService;
        private readonly CookieService _cookieService = new CookieService();
        private readonly ISettingsService _settingsService;

        public OrganizationController(IOrganizationService organizationService,
            IAddressBookService addressService,
            IBudgetService budgetService,
            ISettingsService settingsService)
        {
            _organizationService = organizationService;
            _addressService = addressService;
            _budgetService = budgetService;
            _settingsService = settingsService;
        }

        public ActionResult Index(OrganizationPage currentPage)
        {
            if (Request.QueryString["showForm"] != null && bool.Parse(Request.QueryString["showForm"]))
            {
                return RedirectToAction("Create");
            }

            var currentOrganization = _organizationService.GetCurrentFoundationOrganization();
            _cookieService.Set(Constant.Fields.SelectedOrganization, currentOrganization.OrganizationId.ToString());
            _cookieService.Set(Constant.Fields.SelectedNavOrganization, currentOrganization.OrganizationId.ToString());

            var viewModel = new OrganizationPageViewModel
            {
                CurrentContent = currentPage,
                Organization = _organizationService.GetOrganizationModel(currentOrganization)
            };

            var referencePages = _settingsService.GetSiteSettings<ReferencePageSettings>();
            if (referencePages != null && !referencePages.SubOrganizationPage.IsNullOrEmpty())
            {
                viewModel.SubOrganizationPage = referencePages.SubOrganizationPage;
            }

            if (viewModel.Organization != null && viewModel.Organization?.Address == null)
            {
                viewModel.Organization.Address = new B2BAddressViewModel();
            }
            if (viewModel.Organization == null)
            {
                return RedirectToAction("Edit");
            }

            if (viewModel.Organization?.SubOrganizations != null)
            {
                var suborganizations = viewModel.Organization.SubOrganizations;
                var organizationIndex = 0;
                foreach (var suborganization in suborganizations)
                {
                    var budget = _budgetService.GetCurrentOrganizationBudget(suborganization.OrganizationId);
                    if (budget != null)
                    {
                        viewModel.Organization.SubOrganizations.ElementAt(organizationIndex).CurrentBudgetViewModel =
                            new BudgetViewModel(budget);
                    }

                    organizationIndex++;
                }
            }

            viewModel.IsAdmin = CustomerContext.Current.CurrentContact.Properties[Constant.Fields.UserRole].Value.ToString() == Constant.UserRoles.Admin;

            return View(viewModel);
        }

        [NavigationAuthorize("Admin,None")]
        public ActionResult Create(OrganizationPage currentPage)
        {
            var viewModel = new OrganizationPageViewModel
            {
                Organization = new OrganizationModel
                {
                    Address = new B2BAddressViewModel
                    {
                        CountryOptions = _addressService.GetAllCountries()
                    }
                },
                CurrentContent = currentPage
            };
            return View("Edit", viewModel);
        }

        [NavigationAuthorize("Admin")]
        public ActionResult Edit(OrganizationPage currentPage, string organizationId)
        {
            var currentOrganization = _organizationService.GetCurrentFoundationOrganization();
            var viewModel = new OrganizationPageViewModel
            {
                Organization = _organizationService.GetOrganizationModel(currentOrganization) ?? new OrganizationModel(),
                CurrentContent = currentPage
            };
            if (viewModel.Organization?.Address != null)
            {
                viewModel.Organization.Address.CountryOptions = _addressService.GetAllCountries();
            }
            else
            {
                if (viewModel.Organization != null)
                {
                    viewModel.Organization.Address = new B2BAddressViewModel
                    {
                        CountryOptions = _addressService.GetAllCountries()
                    };
                }
            }
            return View(viewModel);
        }

        [NavigationAuthorize("Admin")]
        public ActionResult AddSub(OrganizationPage currentPage)
        {
            var currentOrganization = _organizationService.GetCurrentFoundationOrganization();
            var viewModel = new OrganizationPageViewModel
            {
                CurrentContent = currentPage,
                Organization = _organizationService.GetOrganizationModel(currentOrganization) ?? new OrganizationModel(),
                NewSubOrganization = new SubOrganizationModel()
                {
                    CountryOptions = _addressService.GetAllCountries(),
                }
            };
            viewModel.NewSubOrganization.Locations.Add(new B2BAddressViewModel());
            return View(viewModel);
        }

        [HttpPost]
        [AllowDBWrite]
        [NavigationAuthorize("Admin,None")]
        [ValidateAntiForgeryToken]
        public ActionResult Save(OrganizationPageViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.Organization.Name))
            {
                ModelState.AddModelError("Organization.Name", "Organization Name is requried");
            }

            if (viewModel.Organization.OrganizationId == Guid.Empty)
            {
                _organizationService.CreateOrganization(viewModel.Organization);
            }
            else
            {
                _organizationService.UpdateOrganization(viewModel.Organization);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AllowDBWrite]
        [NavigationAuthorize("Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSub(OrganizationPageViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.NewSubOrganization.Name))
            {
                ModelState.AddModelError("NewSubOrganization.Name", "Sub organization Name is requried");
            }

            //update the locations list
            var updatedLocations = viewModel.NewSubOrganization.Locations.Where(location => location.Name != "removed").ToList();
            viewModel.NewSubOrganization.Locations = updatedLocations;

            _organizationService.CreateSubOrganization(viewModel.NewSubOrganization);
            return RedirectToAction("Index");
        }
    }
}