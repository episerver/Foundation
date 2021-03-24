using System.Collections.Generic;

namespace Foundation.Commerce.GiftCard
{
    public interface IGiftCardService
    {
        List<GiftCard> GetAllGiftCards();
        List<GiftCard> GetCustomerGiftCards(string contactId);
        string CreateGiftCard(GiftCard giftCard);
        string UpdateGiftCard(GiftCard giftCard);
        string DeleteGiftCard(string giftCardId);
        GiftCard GetGiftCard(string giftCardId);
    }
}
