using Foundation.Cms.ViewModels;
using Foundation.Commerce.Models.Pages;
using System.Collections.Generic;

namespace Foundation.Commerce.Customer.ViewModels
{
    public class GiftCardViewModel : ContentViewModel<GiftCardPage>
    {
        public GiftCardViewModel(GiftCardPage currentPage) : base(currentPage)
        {
        }

        public List<GiftCard> GiftCardList { get; set; }
    }
}