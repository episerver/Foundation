using EPiServer.Commerce.Order;
using Mediachase.BusinessFoundation.Data;
using Mediachase.BusinessFoundation.Data.Business;
using Mediachase.Commerce;
using Mediachase.Commerce.Shared;
using System;

namespace Foundation.Commerce.GiftCard
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

        public static EntityObject[] GetAllGiftCards() => BusinessManager.List(GiftCardMetaClass, Array.Empty<FilterElement>());

        public static EntityObject[] GetCustomerGiftCards(PrimaryKeyId contactId)
        {
            return BusinessManager.List(GiftCardMetaClass, new[]
            {
                FilterElement.EqualElement(ContactIdField, contactId)
            });
        }

        public static EntityObject GetGiftCardById(PrimaryKeyId giftCardId) => BusinessManager.Load(GiftCardMetaClass, giftCardId);

        public static PrimaryKeyId CreateGiftCard(
            string giftCardName,
            PrimaryKeyId contactId,
            decimal initialAmount,
            decimal remainBalance,
            string redemptionCode,
            bool isActive)
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

        public static bool PurchaseByGiftCard(IPayment payment, Currency currency)
        {
            var giftCard = BusinessManager.Load(GiftCardMetaClass, new PrimaryKeyId(new Guid(payment.Properties["GiftCardId"].ToString())));
            var priceInUSD = decimal.Round(CurrencyFormatter.ConvertCurrency(new Money(payment.Amount, currency), Currency.USD));
            if (priceInUSD > (decimal)giftCard[RemainBalanceField])
            {
                return false;
            }
            else
            {
                giftCard[RemainBalanceField] = (decimal)giftCard[RemainBalanceField] - priceInUSD;
                if ((decimal)giftCard[RemainBalanceField] <= 0)
                {
                    giftCard[IsActiveField] = false;
                }

                BusinessManager.Update(giftCard);
                return true;
            }
        }
    }
}