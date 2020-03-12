using EPiServer.Core;
using EPiServer.SpecializedProperties;
using Foundation.Commerce.Models.Pages;
using Foundation.Commerce.Order.ViewModels;
using System;

namespace Foundation.Commerce.ViewModels.Header
{
    public class NavigationViewModel
    {
        public ContentReference CurrentContentLink { get; set; }
        public Guid CurrentContentGuid { get; set; }
        public CommerceHomePage StartPage { get; set; }
        public LinkItemCollection UserLinks { get; set; }
        public MiniCartViewModel MiniCart { get; set; }
        public MiniWishlistViewModel WishListMiniCart { get; set; }
        public MiniCartViewModel SharedMiniCart { get; set; }
        public string Name { get; set; }
        public bool ShowCommerceControls { get; set; }
        public bool ShowSharedCart { get; set; }
        public PageData StorePage { get; set; }
        public LinkItemCollection RestrictedMenu { get; set; }
        public bool HasOrganization { get; set; }
        public bool IsBookmarked { get; set; }
    }
}