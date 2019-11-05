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

        public HeaderViewModelFactory(IUrlResolver urlResolver, IContentCacheKeyCreator contentCacheKeyCreator)
        {
            _urlResolver = urlResolver;
            _contentCacheKeyCreator = contentCacheKeyCreator;
        }

        public THeaderViewModel CreateHeaderViewModel<THeaderViewModel>(IContent currentContent, CmsHomePage homePage)
            where THeaderViewModel : HeaderViewModel, new()
        {
            var menuItems = new List<MenuItemViewModel>();
            var menuCached = CacheManager.Get(homePage.ContentLink.ID + MenuCacheKey) as List<MenuItemViewModel>;
            if (menuCached != null && !PageEditing.PageIsInEditMode)
            {
                menuItems = menuCached;
            }
            else
            {
                var menuItemBlocks = homePage.MainMenu?.FilteredItems.GetContentItems<IContent>();
                menuItems = menuItemBlocks?
                   .Select(x => {
                       MenuItemBlock _;
                       if (x is MenuItemBlock)
                       {
                           _ = x as MenuItemBlock;
                           return new MenuItemViewModel
                           {
                               Name = _.Name,
                               ButtonText = _.ButtonText,
                               TeaserText = _.TeaserText,
                               Uri = _.Link == null ? string.Empty : _urlResolver.GetUrl(new UrlBuilder(_.Link.ToString()), new UrlResolverArguments() { ContextMode = ContextMode.Default }),
                               ImageUrl = !ContentReference.IsNullOrEmpty(_.MenuImage) ? _urlResolver.GetUrl(_.MenuImage) : "",
                               ButtonLink = _.ButtonLink?.Host + _.ButtonLink?.PathAndQuery,
                               ChildLinks = _.ChildItems?.ToList() ?? new List<GroupLinkCollection>()
                           };
                       } else
                       {
                           return new MenuItemViewModel
                           {
                               Name = x.Name,
                               Uri = _urlResolver.GetUrl(x.ContentLink),
                               ChildLinks = new List<GroupLinkCollection>()
                           };
                       }
                       
                   }).ToList() ?? new List<MenuItemViewModel>();

                if (!PageEditing.PageIsInEditMode)
                {
                    var keyDependency = new List<string>();
                    keyDependency.Add(_contentCacheKeyCreator.CreateCommonCacheKey(homePage.ContentLink)); // If The HomePage updates menu (remove MenuItems)

                    foreach (var m in menuItemBlocks)
                    {
                        keyDependency.Add(_contentCacheKeyCreator.CreateCommonCacheKey((m as IContent).ContentLink));
                    }

                    var eviction = new CacheEvictionPolicy(TimeSpan.FromDays(1), CacheTimeoutType.Sliding, keyDependency);
                    CacheManager.Insert(homePage.ContentLink.ID + MenuCacheKey, menuItems, eviction);
                }
            }

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
