using EPiServer;
using EPiServer.Core;
using EPiServer.Editor;
using EPiServer.Framework.Cache;
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

namespace Foundation.Cms.ViewModels.Header
{
    public class HeaderViewModelFactory : IHeaderViewModelFactory
    {
        private const string MenuCacheKey = "MenuItemsCacheKey";

        private readonly IUrlResolver _urlResolver;
        private readonly IContentCacheKeyCreator _contentCacheKeyCreator;
        private readonly IContentLoader _contentLoader;

        public HeaderViewModelFactory(IUrlResolver urlResolver, IContentCacheKeyCreator contentCacheKeyCreator, IContentLoader contentLoader)
        {
            _urlResolver = urlResolver;
            _contentCacheKeyCreator = contentCacheKeyCreator;
            _contentLoader = contentLoader;
        }

        public THeaderViewModel CreateHeaderViewModel<THeaderViewModel>(IContent currentContent, CmsHomePage homePage)
            where THeaderViewModel : HeaderViewModel, new()
        {
            var menuItems = new List<MenuItemViewModel>();
            menuItems = homePage.MainMenu?.FilteredItems.Select(x =>
            {
                var itemCached = CacheManager.Get(x.ContentLink.ID + MenuCacheKey) as MenuItemViewModel;
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
                        CacheManager.Insert(x.ContentLink.ID + MenuCacheKey, menuItem, eviction);
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
                MobileNavigation = homePage.MobileNavigationPages,
            };
        }
    }
}
