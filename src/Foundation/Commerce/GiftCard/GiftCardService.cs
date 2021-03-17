using Foundation.Commerce.Extensions;
using Mediachase.BusinessFoundation.Data;
using Mediachase.Commerce.Customers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.GiftCard
{
    public class GiftCardService : IGiftCardService
    {
        public List<GiftCard> GetAllGiftCards()
        {
            return GiftCardManager.GetAllGiftCards().Select(giftCard => new GiftCard()
            {
                GiftCardId = giftCard.PrimaryKeyId.ToString(),
                ContactId = giftCard[GiftCardManager.ContactIdField].ToString(),
                GiftCardName = giftCard[GiftCardManager.GiftCardNameField].ToString(),
                ContactName = CustomerContext.Current.GetContactById(
                    giftCard.GetGuidValue(GiftCardManager.ContactIdField)
                    ).FullName,
                InitialAmount = (decimal)giftCard[GiftCardManager.InitialAmountField],
                RemainBalance = (decimal)giftCard[GiftCardManager.RemainBalanceField],
                RedemptionCode = giftCard[GiftCardManager.RedemptionCodeField]?.ToString() ?? "",
                IsActive = (bool)giftCard[GiftCardManager.IsActiveField]
            }).ToList();
        }

        public List<GiftCard> GetCustomerGiftCards(string contactId)
        {
            return GiftCardManager.GetCustomerGiftCards(new PrimaryKeyId(new Guid(contactId))).Select(giftCard => new GiftCard()
            {
                GiftCardId = giftCard.PrimaryKeyId.ToString(),
                GiftCardName = giftCard[GiftCardManager.GiftCardNameField].ToString(),
                InitialAmount = (decimal)giftCard[GiftCardManager.InitialAmountField],
                RemainBalance = (decimal)giftCard[GiftCardManager.RemainBalanceField],
                IsActive = (bool)giftCard[GiftCardManager.IsActiveField],
                RedemptionCode = giftCard[GiftCardManager.RedemptionCodeField]?.ToString() ?? "",
            }).ToList();
        }

        public GiftCard GetGiftCard(string giftCardId)
        {
            var giftCardObject = GiftCardManager.GetGiftCardById(new PrimaryKeyId(new Guid(giftCardId)));
            return new GiftCard()
            {
                GiftCardId = giftCardObject.PrimaryKeyId.ToString(),
                GiftCardName = giftCardObject[GiftCardManager.GiftCardNameField].ToString(),
                InitialAmount = (decimal)giftCardObject[GiftCardManager.InitialAmountField],
                RemainBalance = (decimal)giftCardObject[GiftCardManager.RemainBalanceField],
                IsActive = (bool)giftCardObject[GiftCardManager.IsActiveField],
                RedemptionCode = giftCardObject[GiftCardManager.RedemptionCodeField]?.ToString() ?? "",
            };
        }

        public string CreateGiftCard(GiftCard giftCard)
        {
            try
            {
                var contactId = new PrimaryKeyId(new Guid(giftCard.ContactId));
                var giftCardId = GiftCardManager.CreateGiftCard(giftCard.GiftCardName, contactId, giftCard.InitialAmount, giftCard.RemainBalance, giftCard.RedemptionCode, giftCard.IsActive);
                return giftCardId.ToString();
            }
            catch (Exception)
            {
                return "Gift card FAILED to add";
            }
        }

        public string UpdateGiftCard(GiftCard giftCard)
        {
            try
            {
                var giftCardId = new PrimaryKeyId(new Guid(giftCard.GiftCardId));
                var contactId = new PrimaryKeyId(new Guid(giftCard.ContactId));
                GiftCardManager.UpdateGiftCard(giftCardId, giftCard.GiftCardName, contactId, giftCard.InitialAmount, giftCard.RemainBalance, giftCard.RedemptionCode, giftCard.IsActive);
                return "Gift card UPDATED";
            }
            catch
            {
                return "Gift card FAILED to update";
            }
        }

        public string DeleteGiftCard(string giftCardId)
        {
            try
            {
                GiftCardManager.DeleteGiftCard(new PrimaryKeyId(new Guid(giftCardId)));
                return "Gift card DELETED";
            }
            catch
            {
                return "Gift card FAILED to delete";
            }
        }
    }
}