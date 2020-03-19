using Foundation.Cms.Identity;
using Foundation.Cms.Pages;
using Foundation.Cms.ViewModels.Pages;
using Foundation.Commerce.Customer;
using Foundation.Commerce.Customer.ViewModels;
using Foundation.Commerce.Order.ViewModels;
using Foundation.Commerce.ViewModels.Header;
using System.Collections.Generic;

namespace Foundation.Commerce.ViewModels
{
    public class CommerceProfilePageViewModel<TContent, THeaderViewModel> : ProfilePageViewModel<TContent, THeaderViewModel>
        where TContent : ProfilePage
        where THeaderViewModel : CommerceHeaderViewModel, new()
    {
        public CommerceProfilePageViewModel()
        {
        }

        public CommerceProfilePageViewModel(TContent profilePage) : base(profilePage)
        {
        }

        public List<OrderViewModel> Orders { get; set; }
        public IEnumerable<AddressModel> Addresses { get; set; }
        public SiteUser SiteUser { get; set; }
        public FoundationContact CustomerContact { get; set; }
        public string OrderDetailsPageUrl { get; set; }
    }
}