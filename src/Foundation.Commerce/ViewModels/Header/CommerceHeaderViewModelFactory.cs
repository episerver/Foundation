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
using Foundation.Cms.Blocks;
using Foundation.Cms.Extensions;
using Foundation.Cms.Pages;
using Foundation.Cms.ViewModels.Header;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Customer.ViewModels;
using Foundation.Commerce.Markets.ViewModels;
using Foundation.Commerce.Models.Pages;
using Foundation.Commerce.Order.Services;
using Foundation.Commerce.Order.ViewModels;
using Mediachase.Commerce;
using Mediachase.Commerce.Markets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundation.Commerce.ViewModels.Header
{
    public class CommerceHeaderViewModelFactory : IHeaderViewModelFactory
    {
        private const string FlagLocation = "/Assets/icons/flags/";

        private readonly LocalizationService _localizationService;
        private readonly ICustomerService _customerService;
        private readonly CartViewModelFactory _cartViewModelFactory;
        private readonly IUrlResolver _urlResolver;
        private readonly IMarketService _marketService;
        private readonly ICurrentMarket _currentMarket;
        private readonly IBookmarksService _bookmarksService;
        private readonly ICartService _cartService;
        private readonly IContentCacheKeyCreator _contentCacheKeyCreator;
        private readonly IContentLoader _contentLoader;
        private readonly IDatabaseMode _databaseMode;

        public CommerceHeaderViewModelFactory(LocalizationService localizationService,
            ICustomerService customerService,
            CartViewModelFactory cartViewModelFactory,
            IUrlResolver urlResolver,
            IMarketService marketService,
            ICurrentMarket currentMarket,
            IBookmarksService bookmarksService,
            ICartService cartService,
            IContentCacheKeyCreator contentCacheKeyCreator,
            IContentLoader contentLoader,
            IDatabaseMode databaseMode)
        {
            _localizationService = localizationService;
            _customerService = customerService;
            _cartViewModelFactory = cartViewModelFactory;
            _urlResolver = urlResolver;
            _marketService = marketService;
            _currentMarket = currentMarket;
            _bookmarksService = bookmarksService;
            _cartService = cartService;
            _contentCacheKeyCreator = contentCacheKeyCreator;
            _contentLoader = contentLoader;
            _databaseMode = databaseMode;
        }

        public virtual THeaderViewModel CreateHeaderViewModel<THeaderViewModel>(IContent content, CmsHomePage home)
            where THeaderViewModel : HeaderViewModel, new()
        {
            var commerceHomePage = home as CommerceHomePage;
            if (commerceHomePage == null)
            {
                return null;
            }

            var contact = _customerService.GetCurrentContact();
            var isBookmarked = IsBookmarked(content);
            var viewModel = CreateViewModel<CommerceHeaderViewModel>(content, commerceHomePage, contact, isBookmarked);
            AddCommerceComponents(contact, viewModel);
            AddAnonymousComponents(commerceHomePage, viewModel);
            AddMarketViewModel(content, viewModel);
            AddMyAccountMenu(commerceHomePage, viewModel);
            return viewModel as THeaderViewModel;
        }

        public virtual void AddMyAccountMenu<THomePage, THeaderViewModel>(THomePage homePage,
            THeaderViewModel viewModel)
            where THeaderViewModel : HeaderViewModel, new()
            where THomePage : CmsHomePage
        {
            var commerceHomePage = homePage as CommerceHomePage;
            if (commerceHomePage == null)
            {
                return;
            }

            if (HttpContext.Current != null && !HttpContext.Current.Request.IsAuthenticated)
            {
                viewModel.UserLinks = new LinkItemCollection();
                return;
            }

            var menuItems = new LinkItemCollection();
            var filter = new FilterContentForVisitor();
            var contact = _customerService.GetCurrentContact();

            if (contact != null && contact.FoundationOrganization != null)
            {
                var orgLink = new LinkItem
                {
                    Href = _urlResolver.GetUrl(commerceHomePage.OrganizationMainPage),
                    Text = _localizationService.GetString("My Organization", "My Organization"),
                    Title = _localizationService.GetString("My Organization", "My Organization")
                };

                menuItems.Add(orgLink);
            }

            foreach (var linkItem in commerceHomePage.MyAccountCommerceMenu ?? new LinkItemCollection())
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

        protected virtual T CreateViewModel<T>(IContent currentContent, CommerceHomePage homePage, Customer.FoundationContact contact, bool isBookmarked)
            where T : CommerceHeaderViewModel, new()
        {
            var menuItems = new List<MenuItemViewModel>();
            var homeLanguage = homePage.Language.DisplayName;
            menuItems = homePage.MainMenu?.FilteredItems.Select(x =>
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
                        var keyDependency = new List<string>();
                        keyDependency.Add(_contentCacheKeyCreator.CreateCommonCacheKey(homePage.ContentLink)); // If The HomePage updates menu (remove MenuItems)
                        keyDependency.Add(_contentCacheKeyCreator.CreateCommonCacheKey(x.ContentLink));

                        var eviction = new CacheEvictionPolicy(TimeSpan.FromDays(1), CacheTimeoutType.Sliding, keyDependency);
                        CacheManager.Insert(x.ContentLink.ID + homeLanguage + ":" + Constant.CacheKeys.MenuItems, menuItem, eviction);
                    }

                    return menuItem;
                }
            }).ToList();

            return new T
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
                    ResetPasswordPage = homePage.ResetPasswordPage
                },
                RegisterAccountViewModel = new RegisterAccountViewModel
                {
                    Address = new AddressModel()
                },
            };
        }

        protected virtual void AddMarketViewModel(IContent currentContent, CommerceHeaderViewModel viewModel)
        {
            var currentMarket = _currentMarket.GetCurrentMarket();

            if (CacheManager.Get(Constant.CacheKeys.MarketViewModel + "-" + currentMarket.MarketId.Value) is MarketViewModel marketsViewModel)
            {
                viewModel.Markets = marketsViewModel;
            }
            else
            {
                var markets = _marketService.GetAllMarkets().Where(x => x.IsEnabled).OrderBy(x => x.MarketName)
                    .Select(x => new MarketItem
                    {
                        Selected = false,
                        Text = x.MarketName,
                        Value = x.MarketId.Value,
                        FlagUrl = GetFlagUrl(x.MarketId)
                    });
                marketsViewModel = new MarketViewModel
                {
                    Markets = markets,
                    MarketId = currentMarket.MarketId.Value,
                    CurrentMarket = new MarketItem
                    {
                        Selected = false,
                        Text = currentMarket.MarketName,
                        Value = currentMarket.MarketId.Value,
                        FlagUrl = GetFlagUrl(currentMarket.MarketId)
                    },
                    ContentLink = currentContent?.ContentLink ?? ContentReference.EmptyReference
                };
                viewModel.Markets = marketsViewModel;

                CacheManager.Insert(Constant.CacheKeys.MarketViewModel + "-" + currentMarket.MarketId.Value, marketsViewModel, new CacheEvictionPolicy(TimeSpan.FromHours(1), CacheTimeoutType.Sliding));
            }
        }

        protected virtual void AddCommerceComponents(Customer.FoundationContact contact, CommerceHeaderViewModel viewModel)
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

        protected virtual void AddAnonymousComponents(CommerceHomePage homePage, CommerceHeaderViewModel viewModel)
        {
            if (HttpContext.Current != null && !HttpContext.Current.Request.IsAuthenticated)
            {
                viewModel.LoginViewModel = new LoginViewModel
                {
                    ResetPasswordPage = homePage.ResetPasswordPage
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

        protected virtual string GetFlagUrl(MarketId marketId)
        {
            if (marketId == new MarketId("FR"))
            {
                return $"{FlagLocation}fr.svg";
            }

            if (marketId == new MarketId("AUS"))
            {
                return $"{FlagLocation}au.svg";
            }

            if (marketId == new MarketId("BRA"))
            {
                return $"{FlagLocation}br.svg";
            }

            if (marketId == new MarketId("CAN"))
            {
                return $"{FlagLocation}ca.svg";
            }

            if (marketId == new MarketId("CHL"))
            {
                return $"{FlagLocation}cl.svg";
            }

            if (marketId == new MarketId("DEFAULT"))
            {
                return $"{FlagLocation}us.svg";
            }

            if (marketId == new MarketId("DEU"))
            {
                return $"{FlagLocation}de.svg";
            }

            if (marketId == new MarketId("ESP"))
            {
                return $"{FlagLocation}es.svg";
            }

            if (marketId == new MarketId("JPN"))
            {
                return $"{FlagLocation}jp.svg";
            }

            if (marketId == new MarketId("NLD"))
            {
                return $"{FlagLocation}nl.svg";
            }

            if (marketId == new MarketId("NOR"))
            {
                return $"{FlagLocation}no.svg";
            }

            if (marketId == new MarketId("SAU"))
            {
                return $"{FlagLocation}sa.svg";
            }

            if (marketId == new MarketId("SWE"))
            {
                return $"{FlagLocation}se.svg";
            }

            if (marketId == new MarketId("UK"))
            {
                return $"{FlagLocation}gb.svg";
            }

            return marketId == new MarketId("US") ? $"{FlagLocation}us.svg" : "";
        }
    }
}
