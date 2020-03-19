using Foundation.Commerce.Customer.ViewModels;
using Mediachase.Commerce.Customers;
using System.Collections.Generic;

namespace Foundation.Commerce.Customer.Services
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

        CreditCard GetCreditCard(string creditCardId);

        void MapToCreditCard(CreditCardModel creditCardModel, ref CreditCard creditCard);

        void MapToModel(CreditCard creditCard, ref CreditCardModel creditCardModel);
    }
}
