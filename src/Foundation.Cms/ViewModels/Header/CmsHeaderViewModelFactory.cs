using EPiServer;
using EPiServer.Core;
using EPiServer.Data;
using EPiServer.Editor;
using EPiServer.Filters;
using EPiServer.Framework.Cache;
using EPiServer.Framework.Localization;
using EPiServer.Security;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Foundation.Cms.Blocks;
using Foundation.Cms.Extensions;
using Foundation.Cms.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundation.Cms.ViewModels.Header
{
    public class CmsHeaderViewModelFactory : IHeaderViewModelFactory
    {
        private const string MenuCacheKey = "MenuItemsCacheKey";
        private readonly IUrlResolver _urlResolver;
        private readonly IContentCacheKeyCreator _contentCacheKeyCreator;
        private readonly IContentLoader _contentLoader;
        private readonly LocalizationService _localizationService;
        private readonly IDatabaseMode _databaseMode;

        public CmsHeaderViewModelFactory(IUrlResolver urlResolver,
            IContentCacheKeyCreator contentCacheKeyCreator,
            IContentLoader contentLoader,
            LocalizationService localizationService,
            IDatabaseMode databaseMode)
        {
            _urlResolver = urlResolver;
            _contentCacheKeyCreator = contentCacheKeyCreator;
            _contentLoader = contentLoader;
            _localizationService = localizationService;
            _databaseMode = databaseMode;
        }

        public THeaderViewModel CreateHeaderViewModel<THeaderViewModel>(IContent currentContent, CmsHomePage homePage)
            where THeaderViewModel : HeaderViewModel, new()
        {
            var menuItems = new List<MenuItemViewModel>();
            var homeLanguage = homePage.Language.DisplayName;
            menuItems = homePage.MainMenu?.FilteredItems.Select(x =>
            {
                var itemCached = CacheManager.Get(x.ContentLink.ID + homeLanguage + ":" + MenuCacheKey) as MenuItemViewModel;
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
                        CacheManager.Insert(x.ContentLink.ID + homeLanguage + ":" + MenuCacheKey, menuItem, eviction);
                    }
                    return menuItem;
                }
            }).ToList();

            return new THeaderViewModel
            {
                HomePage = homePage,
                CurrentContentLink = currentContent?.ContentLink,
                CurrentContentGuid = currentContent?.ContentGuid ?? Guid.Empty,
                UserLinks = new LinkItemCollection(),
                Name = PrincipalInfo.Current.Name,
                MenuItems = menuItems,
                IsReadonlyMode = _databaseMode.DatabaseMode == DatabaseMode.ReadOnly
            };
        }

        public virtual void AddMyAccountMenu<THomePage, THeaderViewModel>(THomePage homePage,
            THeaderViewModel viewModel)
            where THeaderViewModel : HeaderViewModel, new()
            where THomePage : CmsHomePage
        {
            if (HttpContext.Current != null && !HttpContext.Current.Request.IsAuthenticated)
            {
                viewModel.UserLinks = new LinkItemCollection();
                return;
            }

            var menuItems = new LinkItemCollection();
            var filter = new FilterContentForVisitor();

            foreach (var linkItem in homePage.MyAccountCmsMenu ?? new LinkItemCollection())
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
    }
}
