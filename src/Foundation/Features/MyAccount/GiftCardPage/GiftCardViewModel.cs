using Foundation.Commerce.GiftCard;
using Foundation.Features.Shared;
using System.Collections.Generic;

namespace Foundation.Features.MyAccount.GiftCardPage
{
    public class GiftCardViewModel : ContentViewModel<GiftCardPage>
    {
        public GiftCardViewModel(GiftCardPage currentPage) : base(currentPage)
        {
        }

        public List<GiftCard> GiftCardList { get; set; }
    }
}