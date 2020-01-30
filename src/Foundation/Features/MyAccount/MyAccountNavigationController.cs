using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.SpecializedProperties;
using EPiServer.Web.Routing;
using Foundation.Cms;
using Foundation.Commerce;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Models.Pages;
using Foundation.Commerce.ViewModels.Header;
using Foundation.Demo.Models;
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

        public MyAccountNavigationController(
            IContentLoader contentLoader,
            LocalizationService localizationService,
            IOrganizationService organizationService,
            ICustomerService customerService,
            IPageRouteHelper pageRouteHelper,
            UrlResolver urlResolver)
        {
            _contentLoader = contentLoader;
            _localizationService = localizationService;
            _organizationService = organizationService;
            _customerService = customerService;
            _pageRouteHelper = pageRouteHelper;
            _urlResolver = urlResolver;
        }

        public ActionResult MyAccountMenu(MyAccountPageType id)
        {
            var startPage = _contentLoader.Get<DemoHomePage>(ContentReference.StartPage);
            if (startPage == null)
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
                OrganizationPage = startPage.OrganizationMainPage,
                SubOrganizationPage = startPage.SubOrganizationPage,
                MenuItemCollection = new LinkItemCollection()
            };

            var menuItems = startPage.ShowCommerceHeaderComponents ? startPage.MyAccountCommerceMenu : startPage.MyAccountCmsMenu;
            if (menuItems == null)
            {
                return PartialView("_ProfileSidebar", model);
            }
            var wishlist = _contentLoader.Get<WishListPage>(startPage.WishlistPage);
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
                    Href = _urlResolver.GetUrl(startPage.QuickOrderPage),
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