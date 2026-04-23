using Foundation.Features.MyOrganization.Organization;
using Foundation.Infrastructure.Commerce.Customer;
using Foundation.Infrastructure.Commerce.Customer.Services;
using Mediachase.BusinessFoundation.Data;
using Mediachase.BusinessFoundation.Data.Business;
using Mediachase.Commerce.Customers;

namespace Foundation.Features.MyAccount.CreditCard
{
    /// <summary>
    /// All action on credit card data.
    /// Commerce 15 removed: CreditCard API (GetContactCreditCards, GetOrganizationCreditCards,
    /// CreditCard.CreateInstance, CreditCard.Delete, CreditCardEntity, ContactId, OrganizationId,
    /// PrimaryKeyId, CardType, LastFourDigits) removed. Methods stubbed to return empty/default.
    /// </summary>
    public class CreditCardService : ICreditCardService
    {
        private readonly CustomerContext _customerContext;
        private readonly IOrganizationService _organizationService;
        private readonly ICustomerService _customerService;
        private readonly LocalizationService _localizationService;

        public CreditCardService(IOrganizationService organizationService,
            ICustomerService customerService,
            LocalizationService localizationService
        )
        {
            _customerContext = CustomerContext.Current;
            _organizationService = organizationService;
            _customerService = customerService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// Check credit card is valid for edit/delete.
        /// Commerce 15 removed: always returns true (stub).
        /// </summary>
        public bool IsValid(string creditCardId, out string errorMessage)
        {
            // Commerce 15 removed: CreditCard API removed. Stub returns true.
            errorMessage = null;
            return true;
        }

        /// <summary>
        /// Check credit card is valid to use.
        /// Commerce 15 removed: always returns false (stub).
        /// </summary>
        public bool IsReadyToUse(string creditCardId)
        {
            // Commerce 15 removed: CreditCard API removed. Stub returns false.
            return false;
        }

        /// <summary>
        /// Delete a credit card.
        /// Commerce 15 removed: no-op stub.
        /// </summary>
        public void Delete(string creditCardId)
        {
            // Commerce 15 removed: CreditCard.Delete removed. No-op stub.
        }

        /// <summary>
        /// Save credit card.
        /// Commerce 15 removed: no-op stub.
        /// </summary>
        public void Save(CreditCardModel creditCardModel)
        {
            // Commerce 15 removed: CreditCard save API removed. No-op stub.
        }

        /// <summary>
        /// List all credit cards available for user or organization.
        /// Commerce 15 removed: returns empty list (stub).
        /// </summary>
        public IList<CreditCardModel> List(bool isOrganization = false, bool isUsingToPurchase = false)
        {
            // Commerce 15 removed: GetContactCreditCards/GetOrganizationCreditCards removed. Returns empty.
            return new List<CreditCardModel>();
        }

        /// <summary>
        /// Load data for a credit card.
        /// Commerce 15 removed: no-op stub.
        /// </summary>
        public void LoadCreditCard(CreditCardModel creditCardModel)
        {
            // Commerce 15 removed: CreditCard API removed. No-op stub.
        }

        /// <summary>
        /// Map credit card view model to credit card of commerce core.
        /// Commerce 15 removed: no-op stub.
        /// </summary>
        public void MapToCreditCard(CreditCardModel creditCardModel, ref Mediachase.Commerce.Customers.CreditCard creditCard)
        {
            // Commerce 15 removed: CreditCard properties removed. No-op stub.
        }

        /// <summary>
        /// Map credit card of commerce core to credit card view model.
        /// Commerce 15 removed: no-op stub.
        /// </summary>
        public void MapToModel(Mediachase.Commerce.Customers.CreditCard creditCard, ref CreditCardModel creditCardModel)
        {
            // Commerce 15 removed: CreditCard properties removed. No-op stub.
        }

        /// <summary>
        /// Get credit card by id.
        /// Commerce 15 removed: returns null (stub).
        /// </summary>
        public Mediachase.Commerce.Customers.CreditCard GetCreditCard(string creditCardId)
        {
            // Commerce 15 removed: CreditCardEntity/BusinessManager list API removed. Returns null.
            return null;
        }
    }
}
