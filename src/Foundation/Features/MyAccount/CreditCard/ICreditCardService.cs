using System.Collections.Generic;

namespace Foundation.Features.MyAccount.CreditCard
{
    /// <summary>
    /// All action on credit card data
    /// </summary>
    public interface ICreditCardService
    {
        IList<CreditCardModel> List(bool isOrganization = false, bool isUsingToPurchase = false);

        bool IsValid(string creditCardId, out string errorMessage);

        bool IsReadyToUse(string creditCardId);

        void Save(CreditCardModel creditCardModel);

        void Delete(string creditCardId);

        void LoadCreditCard(CreditCardModel creditCardModel);

        Mediachase.Commerce.Customers.CreditCard GetCreditCard(string creditCardId);

        void MapToCreditCard(CreditCardModel creditCardModel, ref Mediachase.Commerce.Customers.CreditCard creditCard);

        void MapToModel(Mediachase.Commerce.Customers.CreditCard creditCard, ref CreditCardModel creditCardModel);
    }
}
