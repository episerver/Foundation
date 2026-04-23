using EPiServer.SpecializedProperties;
using Foundation.Features.Header;
using Foundation.Features.MyOrganization.Organization;
using Foundation.Infrastructure.Cms;
using Foundation.Infrastructure.Cms.Settings;
using Foundation.Infrastructure.Commerce;
using Foundation.Infrastructure.Commerce.Customer.Services;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Foundation.Features.MyAccount
{
    public class MyAccountNavigationViewComponent : ViewComponent
    {
        private readonly IContentLoader _contentLoader;
        private readonly LocalizationService _localizationService;
        private readonly ICookieService _cookieService;
        private readonly IOrganizationService _organizationService;
        private readonly ICustomerService _customerService;
        private readonly IPageRouteHelper _pageRouteHelper;
        private readonly UrlResolver _urlResolver;
        private readonly ISettingsService _settingsService;

        public MyAccountNavigationViewComponent(
            IContentLoader contentLoader,
            LocalizationService localizationService,
            IOrganizationService organizationService,
            ICustomerService customerService,
            IPageRouteHelper pageRouteHelper,
            UrlResolver urlResolver,
            ISettingsService settingsService,
            ICookieService cookieService)
        {
            _contentLoader = contentLoader;
            _localizationService = localizationService;
            _organizationService = organizationService;
            _customerService = customerService;
            _pageRouteHelper = pageRouteHelper;
            _urlResolver = urlResolver;
            _settingsService = settingsService;
            _cookieService = cookieService;
        }

        public IViewComponentResult Invoke(MyAccountPageType id)
        {
            var referenceSettings = _settingsService.GetSiteSettings<ReferencePageSettings>();
            var layoutsettings = _settingsService.GetSiteSettings<LayoutSettings>();
            if (referenceSettings == null || layoutsettings == null)
            {
                return new ViewViewComponentResult();
            }

            var selectedSubNav = _cookieService.Get(Constant.Fields.SelectedNavOrganization);
            var organization = _organizationService.GetCurrentFoundationOrganization();
            var canSeeOrganizationNav = _customerService.CanSeeOrganizationNav();

            var model = new MyAccountNavigationViewModel
            {
                Organization = canSeeOrganizationNav ? _organizationService.GetOrganizationModel(organization) : null,
                CurrentOrganization = canSeeOrganizationNav ? !string.IsNullOrEmpty(selectedSubNav) ?
                    _organizationService.GetOrganizationModel(_organizationService.GetSubFoundationOrganizationById(selectedSubNav)) :
                    _organizationService.GetOrganizationModel(organization) : null,
                CurrentPageType = id,
                OrganizationPage = referenceSettings.OrganizationMainPage,
                SubOrganizationPage = referenceSettings.SubOrganizationPage,
                MenuItemCollection = new LinkItemCollection()
            };

            var menuItems = layoutsettings.MyAccountMenu;
            if (menuItems == null)
            {
                return View("/Features/MyAccount/_ProfileSidebar.cshtml", model);
            }

            var wishlist = referenceSettings.WishlistPage != null ? _contentLoader.Get<PageData>(referenceSettings.WishlistPage) : null;
            menuItems = menuItems.CreateWritableClone();

            if (model.Organization != null)
            {
                if (wishlist != null)
                {
                    var url = wishlist.LinkURL.Contains("?") ? wishlist.LinkURL.Split('?').First() : wishlist.LinkURL;
                    var item = menuItems.FirstOrDefault(x => x.Href.Substring(1).Equals(url));
                    if (item != null)
                    {
                        menuItems.Remove(item);
                    }
                }
                menuItems.Add(new LinkItem
                {
                    Href = _urlResolver.GetUrl(referenceSettings.QuickOrderPage),
                    Text = _localizationService.GetString("/Dashboard/Labels/QuickOrder", "Quick Order")
                });
            }
            else if (organization != null)
            {
                if (wishlist != null)
                {
                    var url = wishlist.LinkURL.Contains("?") ? wishlist.LinkURL.Split('?').First() : wishlist.LinkURL;
                    var item = menuItems.FirstOrDefault(x => x.Href.Substring(1).Equals(url));
                    if (item != null)
                    {
                        item.Text = _localizationService.GetString("/Dashboard/Labels/OrderPad", "Order Pad");
                    }
                }
            }

            model.MenuItemCollection.AddRange(menuItems);

            return View("/Features/MyAccount/_ProfileSidebar.cshtml", model);
        }
    }
}