using Mediachase.BusinessFoundation.Data;
using Mediachase.BusinessFoundation.Data.Business;

namespace Foundation.Commerce.Order.Payments
{
    public static class GiftCardManager
    {
        public const string GiftCardMetaClass = "GiftCard";
        public const string ContactMetaClass = "Contact";
        public const string GiftCardNameField = "GiftCardName";
        public const string ContactIdField = "ContactId";
        public const string InitialAmountField = "InitialAmount";
        public const string RemainBalanceField = "RemainBalance";
        public const string IsActiveField = "IsActive";
        public const string RedemptionCodeField = "RedemptionCode";

        public static EntityObject[] GetAllGiftCards() => BusinessManager.List(GiftCardMetaClass, new FilterElement[0]);

        public static EntityObject[] GetCustomerGiftCards(PrimaryKeyId contactId)
        {
            return BusinessManager.List(GiftCardMetaClass, new[]
            {
                FilterElement.EqualElement(ContactIdField, contactId)
            });
        }

        public static PrimaryKeyId CreateGiftCard(string giftCardName, PrimaryKeyId contactId, decimal initialAmount,
            decimal remainBalance, string redemptionCode, bool isActive)
        {
            var giftCard = BusinessManager.InitializeEntity(GiftCardMetaClass);
            giftCard[GiftCardNameField] = giftCardName;
            giftCard[ContactIdField] = contactId;
            giftCard[InitialAmountField] = initialAmount;
            giftCard[RemainBalanceField] = remainBalance;
            giftCard[IsActiveField] = isActive;
            giftCard[RedemptionCodeField] = redemptionCode;
            var giftCardId = BusinessManager.Create(giftCard);
            return giftCardId;
        }

        public static void UpdateGiftCard(PrimaryKeyId giftCardId, string giftCardName, PrimaryKeyId contactId,
            decimal initialAmount, decimal remainBalance, string redemptionCode, bool isActive)
        {
            var giftCard = BusinessManager.Load(GiftCardMetaClass, giftCardId);
            giftCard[GiftCardNameField] = giftCardName;
            giftCard[ContactIdField] = contactId;
            giftCard[InitialAmountField] = initialAmount;
            giftCard[RemainBalanceField] = remainBalance;
            giftCard[IsActiveField] = isActive;
            giftCard[RedemptionCodeField] = redemptionCode;
            BusinessManager.Update(giftCard);
        }

        public static void DeleteGiftCard(PrimaryKeyId giftCardId)
        {
            var giftCard = BusinessManager.Load(GiftCardMetaClass, giftCardId);
            BusinessManager.Delete(giftCard);
        }
    }
}