using EPiServer;
using EPiServer.Core;
using EPiServer.SpecializedProperties;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Models.Pages;
using Foundation.Commerce.ViewModels.Header;
using System;
using System.Web.Mvc;

namespace Foundation.Features.MyOrganization
{
    public class B2BNavigationController : Controller
    {
        private readonly IContentLoader _contentLoader;
        private readonly IOrganizationService _organizationService;
        private readonly IB2BNavigationService _b2bNavigationService;

        public B2BNavigationController(IContentLoader contentLoader, IOrganizationService organizationService, IB2BNavigationService b2bNavigationService)
        {
            _contentLoader = contentLoader;
            _organizationService = organizationService;
            _b2bNavigationService = b2bNavigationService;
        }

        public ActionResult Index(IContent currentContent)
        {
            var startPage = _contentLoader.Get<CommerceHomePage>(ContentReference.StartPage);

            var viewModel = new NavigationViewModel
            {
                StartPage = startPage,
                CurrentContentLink = currentContent?.ContentLink,
                CurrentContentGuid = currentContent?.ContentGuid ?? Guid.Empty,
                UserLinks = new LinkItemCollection()
            };

            var organization = _organizationService.GetCurrentFoundationOrganization();
            if (organization == null)
            {
                return PartialView("_B2BNavigation", viewModel);
            }

            if (startPage.OrganizationMenu != null)
            {
                viewModel.UserLinks.AddRange(_b2bNavigationService.FilterB2BNavigationForCurrentUser(startPage.OrganizationMenu));
            }

            return PartialView("_B2BNavigation", viewModel);
        }
    }
}