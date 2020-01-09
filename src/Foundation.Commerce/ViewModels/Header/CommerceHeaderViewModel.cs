using EPiServer.Core;
using EPiServer.SpecializedProperties;
using Foundation.Cms.ViewModels.Header;
using Foundation.Commerce.Customer.ViewModels;
using Foundation.Commerce.Markets.ViewModels;
using Foundation.Commerce.Models.Pages;
using Foundation.Commerce.Order.ViewModels;

namespace Foundation.Commerce.ViewModels.Header
{
    public class CommerceHeaderViewModel : HeaderViewModel
    {
        public MiniCartViewModel MiniCart { get; set; }
        public MiniWishlistViewModel WishListMiniCart { get; set; }
        public MiniCartViewModel SharedMiniCart { get; set; }
        public LoginViewModel LoginViewModel { get; set; }
        public RegisterAccountViewModel RegisterAccountViewModel { get; set; }
        public bool ShowSharedCart { get; set; }
        public PageData StorePage { get; set; }
        public LinkItemCollection RestrictedMenu { get; set; }
        public bool HasOrganization { get; set; }
        public MarketViewModel Markets { get; set; }
        public bool IsBookmarked { get; set; }

        public CommerceHomePage CommerceHomePage => HomePage as CommerceHomePage;
    }
}