using EPiServer.SpecializedProperties;
using Foundation.Features.Header;
using Foundation.Features.Home;
namespace Foundation.Features.MyOrganization
{
    public class B2BNavigationViewModel
    {
        public ContentReference CurrentContentLink { get; set; }
        public Guid CurrentContentGuid { get; set; }
        public HomePage StartPage { get; set; }
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
