using Foundation.Infrastructure.Commerce.GiftCard;

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