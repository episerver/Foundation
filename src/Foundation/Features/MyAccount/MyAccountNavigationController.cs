using EPiServer;
using EPiServer.Framework.Localization;
using EPiServer.SpecializedProperties;
using EPiServer.Web.Routing;
using Foundation.Cms;
using Foundation.Cms.Settings;
using Foundation.Commerce;
using Foundation.Commerce.Customer.Services;
using Foundation.Features.Header;
using Foundation.Features.MyOrganization.Organization;
using Foundation.Features.NamedCarts.Wishlist;
using Foundation.Features.Settings;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.MyAccount
{
    public class MyAccountNavigationController : Controller
    {
        private readonly IContentLoader _contentLoader;
        private readonly LocalizationService _localizationService;
        private readonly CookieService _cookieService = new CookieService();
        private readonly IOrganizationService _organizationService;
        private readonly ICustomerService _customerService;
        private readonly IPageRouteHelper _pageRouteHelper;
        private readonly UrlResolver _urlResolver;
        private readonly ISettingsService _settingsService;

        public MyAccountNavigationController(
            IContentLoader contentLoader,
            LocalizationService localizationService,
            IOrganizationService organizationService,
            ICustomerService customerService,
            IPageRouteHelper pageRouteHelper,
            UrlResolver urlResolver,
            ISettingsService settingsService)
        {
            _contentLoader = contentLoader;
            _localizationService = localizationService;
            _organizationService = organizationService;
            _customerService = customerService;
            _pageRouteHelper = pageRouteHelper;
            _urlResolver = urlResolver;
            _settingsService = settingsService;
        }

        public ActionResult MyAccountMenu(MyAccountPageType id)
        {
            var referenceSettings = _settingsService.GetSiteSettings<ReferencePageSettings>();
            var layoutsettings = _settingsService.GetSiteSettings<LayoutSettings>();
            if (referenceSettings == null || layoutsettings == null)
            {
                return new EmptyResult();
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
                return PartialView("_ProfileSidebar", model);
            }
            var wishlist = _contentLoader.Get<WishListPage>(referenceSettings.WishlistPage);
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

            return PartialView("_ProfileSidebar", model);
        }
    }
}