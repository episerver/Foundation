using EPiServer;
using EPiServer.Core;
using EPiServer.SpecializedProperties;
using Foundation.Cms.Settings;
using Foundation.Features.Header;
using Foundation.Features.Home;
using Foundation.Features.MyOrganization.Organization;
using Foundation.Features.Settings;
using System;
using System.Web.Mvc;

namespace Foundation.Features.MyOrganization
{
    public class B2BNavigationController : Controller
    {
        private readonly IContentLoader _contentLoader;
        private readonly IOrganizationService _organizationService;
        private readonly IB2BNavigationService _b2bNavigationService;
        private readonly ISettingsService _settingsService;

        public B2BNavigationController(IContentLoader contentLoader,
            IOrganizationService organizationService,
            IB2BNavigationService b2bNavigationService,
            ISettingsService settingsService)
        {
            _contentLoader = contentLoader;
            _organizationService = organizationService;
            _b2bNavigationService = b2bNavigationService;
            _settingsService = settingsService;
        }

        public ActionResult Index(IContent currentContent)
        {
            var startPage = _contentLoader.Get<HomePage>(ContentReference.StartPage);
            var layoutSettings = _settingsService.GetSiteSettings<LayoutSettings>();
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

            if (layoutSettings?.OrganizationMenu != null)
            {
                viewModel.UserLinks.AddRange(_b2bNavigationService.FilterB2BNavigationForCurrentUser(layoutSettings.OrganizationMenu));
            }

            return PartialView("_B2BNavigation", viewModel);
        }
    }
}