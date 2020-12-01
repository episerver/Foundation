using EPiServer;
using EPiServer.Core;
using EPiServer.Data;
using EPiServer.Editor;
using EPiServer.Filters;
using EPiServer.Framework.Cache;
using EPiServer.Framework.Localization;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Foundation.Cms.Extensions;
using Foundation.Cms.Settings;
using Foundation.Commerce;
using Foundation.Commerce.Customer;
using Foundation.Commerce.Customer.Services;
using Foundation.Features.Blocks.MenuItemBlock;
using Foundation.Features.Checkout.Services;
using Foundation.Features.Home;
using Foundation.Features.Login;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.MyAccount.Bookmarks;
using Foundation.Features.Settings;
using Mediachase.Commerce.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundation.Features.Header
{
    public class HeaderViewModelFactory : IHeaderViewModelFactory
    {
        private readonly LocalizationService _localizationService;
        private readonly CartViewModelFactory _cartViewModelFactory;
        private readonly IUrlResolver _urlResolver;
        private readonly IBookmarksService _bookmarksService;
        private readonly ICartService _cartService;
        private readonly IContentCacheKeyCreator _contentCacheKeyCreator;
        private readonly IContentLoader _contentLoader;
        private readonly IDatabaseMode _databaseMode;
        private readonly ICustomerService _customerService;
        private readonly CustomerContext _customerContext;
        private readonly ISettingsService _settingsService;

        public HeaderViewModelFactory(LocalizationService localizationService,
            ICustomerService customerService,
            CartViewModelFactory cartViewModelFactory,
            IUrlResolver urlResolver,
            IBookmarksService bookmarksService,
            ICartService cartService,
            CustomerContext customerContext,
            IContentCacheKeyCreator contentCacheKeyCreator,
            IContentLoader contentLoader,
            IDatabaseMode databaseMode,
            ISettingsService settingsService)
        {
            _localizationService = localizationService;
            _customerService = customerService;
            _cartViewModelFactory = cartViewModelFactory;
            _urlResolver = urlResolver;
            _bookmarksService = bookmarksService;
            _cartService = cartService;
            _contentCacheKeyCreator = contentCacheKeyCreator;
            _contentLoader = contentLoader;
            _databaseMode = databaseMode;
            _customerContext = customerContext;
            _settingsService = settingsService;
        }

        public virtual HeaderViewModel CreateHeaderViewModel(IContent content, HomePage home)
        {
            var layoutSettings = _settingsService.GetSiteSettings<LayoutSettings>();
            var contact = _customerService.GetCurrentContact();
            var isBookmarked = IsBookmarked(content);
            var viewModel = CreateViewModel(content, home, contact, isBookmarked);
            AddCommerceComponents(contact, viewModel);
            AddAnonymousComponents(home, viewModel);
            AddMyAccountMenu(home, viewModel);
            viewModel.LargeHeaderMenu = layoutSettings?.LargeHeaderMenu ?? true;
            viewModel.ShowCommerceControls = layoutSettings?.ShowCommerceHeaderComponents ?? true;
            viewModel.DemoUsers = GetDemoUsers(layoutSettings?.ShowCommerceHeaderComponents ?? true);
            viewModel.LayoutSettings = layoutSettings;
            viewModel.SearchSettings = _settingsService.GetSiteSettings<SearchSettings>();
            viewModel.ReferencePageSettings = _settingsService.GetSiteSettings<ReferencePageSettings>();
            viewModel.LabelSettings = _settingsService.GetSiteSettings<LabelSettings>();

            return viewModel;
        }

        public virtual HeaderLogoViewModel CreateHeaderLogoViewModel()
        {
            var layoutSettings = _settingsService.GetSiteSettings<LayoutSettings>();
            var viewModel = new HeaderLogoViewModel()
            {
                LargeHeaderMenu = layoutSettings.LargeHeaderMenu,
                HeaderMenuStyle = layoutSettings.HeaderMenuStyle,
                SiteLogo = layoutSettings.SiteLogo
            };

            return viewModel;
        }

        public virtual void AddMyAccountMenu(HomePage homePage, HeaderViewModel viewModel)
        {
            if (HttpContext.Current != null && !HttpContext.Current.Request.IsAuthenticated)
            {
                viewModel.UserLinks = new LinkItemCollection();
                return;
            }

            var menuItems = new LinkItemCollection();
            var filter = new FilterContentForVisitor();
            var contact = _customerService.GetCurrentContact();
            var referenceSettings = _settingsService.GetSiteSettings<ReferencePageSettings>();
            var layoutSettings = _settingsService.GetSiteSettings<LayoutSettings>();

            if (contact != null && contact.FoundationOrganization != null)
            {
                var orgLink = new LinkItem
                {
                    Href = _urlResolver.GetUrl(referenceSettings?.OrganizationMainPage ?? ContentReference.StartPage),
                    Text = _localizationService.GetString("My Organization", "My Organization"),
                    Title = _localizationService.GetString("My Organization", "My Organization")
                };

                menuItems.Add(orgLink);
            }

            foreach (var linkItem in layoutSettings?.MyAccountMenu ?? new LinkItemCollection())
            {
                if (!UrlResolver.Current.TryToPermanent(linkItem.Href, out var linkUrl))
                {
                    continue;
                }

                if (linkUrl.IsNullOrEmpty())
                {
                    continue;
                }

                var urlBuilder = new UrlBuilder(linkUrl);
                var content = _urlResolver.Route(urlBuilder);
                if (content == null || filter.ShouldFilter(content))
                {
                    continue;
                }

                linkItem.Title = linkItem.Text;
                menuItems.Add(linkItem);
            }

            var signoutText = _localizationService.GetString("/Header/Account/SignOut", "Sign Out");
            var link = new LinkItem
            {
                Href = "/publicapi/signout",
                Text = signoutText,
                Title = signoutText
            };
            link.Attributes.Add("css", "fa-sign-out");
            menuItems.Add(link);

            viewModel.UserLinks.AddRange(menuItems);
        }

        protected virtual bool IsBookmarked(IContent currentContent)
        {
            var isBookmarked = false;
            var bookmarks = _bookmarksService.Get();

            if (bookmarks != null && bookmarks.Any() && bookmarks.FirstOrDefault().ContentGuid != null && currentContent != null)
            {
                isBookmarked = bookmarks.FirstOrDefault(x => x.ContentGuid == currentContent.ContentGuid) != null;
            }

            return isBookmarked;
        }

        protected virtual HeaderViewModel CreateViewModel(IContent currentContent, HomePage homePage, FoundationContact contact, bool isBookmarked)
        {
            var menuItems = new List<MenuItemViewModel>();
            var homeLanguage = homePage.Language.DisplayName;
            var layoutSettings = _settingsService.GetSiteSettings<LayoutSettings>();
            var referenceSettings = _settingsService.GetSiteSettings<ReferencePageSettings>();
            var filter = new FilterContentForVisitor();
            menuItems = layoutSettings?.MainMenu?.FilteredItems.Where(x =>
            {
                var _menuItem = _contentLoader.Get<IContent>(x.ContentLink);
                MenuItemBlock _menuItemBlock;
                if (_menuItem is MenuItemBlock)
                {
                    _menuItemBlock = _menuItem as MenuItemBlock;
                    if (_menuItemBlock.Link == null)
                    {
                        return true;
                    }
                    var linkedItem = UrlResolver.Current.Route(new UrlBuilder(_menuItemBlock.Link));
                    if (linkedItem != null && filter.ShouldFilter(linkedItem))
                    {
                        return false;
                    }
                    return true;
                }
                return true;
            }).Select(x =>
            {
                var itemCached = CacheManager.Get(x.ContentLink.ID + homeLanguage + ":" + Constant.CacheKeys.MenuItems) as MenuItemViewModel;
                if (itemCached != null && !PageEditing.PageIsInEditMode)
                {
                    return itemCached;
                }
                else
                {
                    var content = _contentLoader.Get<IContent>(x.ContentLink);
                    MenuItemBlock _;
                    MenuItemViewModel menuItem;
                    if (content is MenuItemBlock)
                    {
                        _ = content as MenuItemBlock;
                        menuItem = new MenuItemViewModel
                        {
                            Name = _.Name,
                            ButtonText = _.ButtonText,
                            TeaserText = _.TeaserText,
                            Uri = _.Link == null ? string.Empty : _urlResolver.GetUrl(new UrlBuilder(_.Link.ToString()), new UrlResolverArguments() { ContextMode = ContextMode.Default }),
                            ImageUrl = !ContentReference.IsNullOrEmpty(_.MenuImage) ? _urlResolver.GetUrl(_.MenuImage) : "",
                            ButtonLink = _.ButtonLink?.Host + _.ButtonLink?.PathAndQuery,
                            ChildLinks = _.ChildItems?.ToList() ?? new List<GroupLinkCollection>()
                        };
                    }
                    else
                    {
                        menuItem = new MenuItemViewModel
                        {
                            Name = content.Name,
                            Uri = _urlResolver.GetUrl(content.ContentLink),
                            ChildLinks = new List<GroupLinkCollection>()
                        };
                    }

                    if (!PageEditing.PageIsInEditMode)
                    {
                        var keyDependency = new List<string>
                        {
                            _contentCacheKeyCreator.CreateCommonCacheKey(homePage.ContentLink), // If The HomePage updates menu (remove MenuItems)
                            _contentCacheKeyCreator.CreateCommonCacheKey(x.ContentLink)
                        };

                        var eviction = new CacheEvictionPolicy(TimeSpan.FromDays(1), CacheTimeoutType.Sliding, keyDependency);
                        CacheManager.Insert(x.ContentLink.ID + homeLanguage + ":" + Constant.CacheKeys.MenuItems, menuItem, eviction);
                    }

                    return menuItem;
                }
            }).ToList();

            return new HeaderViewModel
            {
                HomePage = homePage,
                CurrentContentLink = currentContent?.ContentLink,
                CurrentContentGuid = currentContent?.ContentGuid ?? Guid.Empty,
                UserLinks = new LinkItemCollection(),
                Name = contact?.FirstName ?? "",
                IsBookmarked = isBookmarked,
                IsReadonlyMode = _databaseMode.DatabaseMode == DatabaseMode.ReadOnly,
                MenuItems = menuItems ?? new List<MenuItemViewModel>(),
                LoginViewModel = new LoginViewModel
                {
                    ResetPasswordPage = referenceSettings?.ResetPasswordPage ?? ContentReference.StartPage
                },
                RegisterAccountViewModel = new RegisterAccountViewModel
                {
                    Address = new AddressModel()
                },
            };
        }

        protected virtual void AddCommerceComponents(FoundationContact contact, HeaderViewModel viewModel)
        {
            if (_databaseMode.DatabaseMode == DatabaseMode.ReadOnly)
            {
                viewModel.MiniCart = new MiniCartViewModel();
                viewModel.WishListMiniCart = new MiniWishlistViewModel();
                viewModel.SharedMiniCart = new MiniCartViewModel();
                return;
            }

            viewModel.MiniCart = _cartViewModelFactory.CreateMiniCartViewModel(
                _cartService.LoadCart(_cartService.DefaultCartName, true)?.Cart);

            viewModel.WishListMiniCart = _cartViewModelFactory.CreateMiniWishListViewModel(
                _cartService.LoadCart(_cartService.DefaultWishListName, true)?.Cart);

            var organizationId = contact?.FoundationOrganization?.OrganizationId.ToString();
            if (!organizationId.IsNullOrEmpty())
            {
                viewModel.SharedMiniCart = _cartViewModelFactory.CreateMiniCartViewModel(
                    _cartService.LoadCart(_cartService.DefaultSharedCartName, organizationId, true)?.Cart, true);

                viewModel.ShowSharedCart = true;
            }
        }

        protected virtual void AddAnonymousComponents(HomePage homePage, HeaderViewModel viewModel)
        {
            if (HttpContext.Current != null && !HttpContext.Current.Request.IsAuthenticated)
            {
                var referenceSettings = _settingsService.GetSiteSettings<ReferencePageSettings>();
                viewModel.LoginViewModel = new LoginViewModel
                {
                    ResetPasswordPage = referenceSettings?.ResetPasswordPage ?? ContentReference.StartPage
                };

                viewModel.RegisterAccountViewModel = new RegisterAccountViewModel
                {
                    Address = new AddressModel
                    {
                        CountryRegion = new CountryRegionViewModel
                        {
                            SelectClass = "select-menu-small",
                            TextboxClass = "textbox-small"
                        }
                    }
                };

                viewModel.RegisterAccountViewModel.Address.Name = _localizationService.GetString("/Shared/Address/DefaultAddressName", "Default Address");
            }
        }

        private List<DemoUserViewModel> GetDemoUsers(bool showCommerceUsers)
        {
            return _customerContext.GetContacts(0, 1000)
                .Select(_ => new FoundationContact(_))
                .Where(_ => showCommerceUsers ? _.ShowInDemoUserMenu > 1 : _.ShowInDemoUserMenu == 2)
                .Select(_ => new DemoUserViewModel
                {
                    Description = _.DemoUserDescription,
                    Title = _.DemoUserTitle,
                    Id = _.ContactId,
                    Email = _.Email,
                    FullName = _.FullName,
                    SortOrder = _.DemoSortOrder
                })
                .OrderBy(_ => _.SortOrder)
                .ToList();
        }
    }
}