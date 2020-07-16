using EPiServer.Framework.Localization;
using Foundation.Commerce.Customer;
using Foundation.Commerce.Customer.Services;
using Foundation.Features.MyOrganization.Organization;
using Foundation.Infrastructure;
using Mediachase.BusinessFoundation.Data;
using Mediachase.BusinessFoundation.Data.Business;
using Mediachase.Commerce.Customers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.MyAccount.CreditCard
{
    /// <summary>
    /// All action on credit card data
    /// </summary>
    public class CreditCardService : ICreditCardService
    {
        private readonly CustomerContext _customerContext;
        private readonly IOrganizationService _organizationService;
        private readonly CustomerService _customerService;
        private readonly LocalizationService _localizationService;

        public CreditCardService(IOrganizationService organizationService,
            CustomerService customerService,
            LocalizationService localizationService
        )
        {
            _customerContext = CustomerContext.Current;
            _organizationService = organizationService;
            _customerService = customerService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// Check credit card is valid for edit/delete
        /// </summary>
        /// <param name="creditCardId">Credit card id</param>
        /// <param name="errorMessage">Error message when credit card id is not valid</param>
        public bool IsValid(string creditCardId, out string errorMessage)
        {
            errorMessage = null;

            //AddNew
            if (string.IsNullOrEmpty(creditCardId))
            {
                return true;
            }

            //Delete, Edit
            var currentCreditCard = GetCreditCard(creditCardId);
            var currentUser = _customerService.GetCurrentContactViewModel();

            if (currentCreditCard != null)
            {
                if (currentCreditCard.ContactId == currentUser.ContactId)
                {
                    return true;
                }
                else if (currentUser.IsAdmin)
                {
                    var currentOrganization = _organizationService.GetOrganizationModel();
                    if (IsValidOrganizationCard(currentCreditCard, currentOrganization))
                    {
                        return true;
                    }
                }
            }

            errorMessage = _localizationService.GetString(
                "/CreditCard/ValidationErrors/InvalidCreditCard",
                "The credit card is not available or you don't have permission to use it");

            return false;
        }

        /// <summary>
        /// Check credit card of organization is valid for edit/delete
        /// </summary>
        /// <param name="creditCard"></param>
        private bool IsValidOrganizationCard(Mediachase.Commerce.Customers.CreditCard creditCard, OrganizationModel organization)
        {
            if (creditCard.OrganizationId == organization.OrganizationId)
            {
                return true;
            }
            else
            {
                var isValid = false;

                foreach (var subOrganization in organization.SubOrganizations)
                {
                    if (IsValidOrganizationCard(creditCard, subOrganization))
                    {
                        isValid = true;
                        break;
                    }
                }

                return isValid;
            }
        }

        /// <summary>
        /// Check credit card is valid to use
        /// </summary>
        /// <param name="creditCardId">Credit card id</param>
        public bool IsReadyToUse(string creditCardId)
        {
            if (string.IsNullOrEmpty(creditCardId))
            {
                return false;
            }

            var curCreditCard = GetCreditCard(creditCardId);
            if (curCreditCard == null)
            {
                return false;
            }
            else
            {
                var currentUser = _customerService.GetCurrentContactViewModel();
                if (curCreditCard.ContactId == currentUser.ContactId)
                {
                    return true;
                }
                else
                {
                    var currentOrganization = _organizationService.GetCurrentFoundationOrganization();
                    if (currentOrganization != null && curCreditCard.OrganizationId == currentOrganization.OrganizationId)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Delete a credit card
        /// </summary>
        /// <param name="creditCardId">Credit card id</param>
        public void Delete(string creditCardId)
        {
            if (IsValid(creditCardId, out _))
            {
                try
                {
                    Mediachase.Commerce.Customers.CreditCard.Delete(PrimaryKeyId.Parse(creditCardId));
                }
                catch
                {
                    //do nothing
                }
            }
        }

        /// <summary>
        /// Save credit card
        /// </summary>
        /// <param name="creditCardModel">Model of credit card</param>
        public void Save(CreditCardModel creditCardModel)
        {
            if (IsValid(creditCardModel.CreditCardId, out _))
            {
                var creditCard = GetCreditCard(creditCardModel.CreditCardId);
                var isNew = creditCard == null;

                if (isNew)
                {
                    creditCard = Mediachase.Commerce.Customers.CreditCard.CreateInstance();
                }

                MapToCreditCard(creditCardModel, ref creditCard);

                if (isNew)
                {
                    //Create CC for user
                    if (creditCard.OrganizationId == null)
                    {
                        creditCard.ContactId = PrimaryKeyId.Parse(_customerService.GetCurrentContactViewModel().ContactId.ToString());
                    }

                    BusinessManager.Create(creditCard);
                }
                else
                {
                    BusinessManager.Update(creditCard);
                }
            }
        }

        /// <summary>
        /// List all credit card that avaiable for user or organization
        /// </summary>
        /// <param name="isOrganization">List for Organization or not</param>
        /// <param name="isUsingToPurchase">List credit card to manage or to purchase
        /// In case manager: user only see own credit card or organization's card depend on setting isOrganization
        /// In case purchase: user can use own credit card and card of organization that user is belong
        /// </param>
        public IList<CreditCardModel> List(bool isOrganization = false, bool isUsingToPurchase = false)
        {
            var currentContact = _customerContext.CurrentContact;
            var creditCards = new List<CreditCardModel>();

            //Get credit card of current contact
            if (currentContact != null && !isOrganization)
            {
                AddRangeCreditCard(currentContact, null, creditCards, currentContact.ContactCreditCards);
            }

            if (isUsingToPurchase || isOrganization)
            {
                //Get credit card of all organization that current customer belong
                var currentOrganization = _organizationService.GetCurrentFoundationOrganization();
                GetCreditCardOrganization(currentOrganization, !isUsingToPurchase, creditCards);
            }

            return creditCards;
        }

        /// <summary>
        /// Get all credit card of current organization and its sub organization
        /// </summary>
        /// <param name="organization">Organization that need to get credit card from</param>
        private void GetCreditCardOrganization(FoundationOrganization organization, bool recursive, List<CreditCardModel> list)
        {
            if (organization != null)
            {
                var orgCards = _customerContext.GetOrganizationCreditCards(organization.OrganizationEntity);
                AddRangeCreditCard(null, new OrganizationModel(organization), list, orgCards);

                if (organization.SubOrganizations.Count > 0 && recursive)
                {
                    foreach (var subOrg in organization.SubOrganizations)
                    {
                        GetCreditCardOrganization(subOrg, recursive, list);
                    }
                }
            }
        }

        /// <summary>
        /// Load data for a credit card
        /// </summary>
        /// <param name="creditCardModel">Model of credit card</param>
        public void LoadCreditCard(CreditCardModel creditCardModel)
        {
            var creditCard = GetCreditCard(creditCardModel.CreditCardId);
            if (creditCard != null)
            {
                MapToModel(creditCard, ref creditCardModel);
            }
        }

        /// <summary>
        /// Map credit card view model to credit card of commerce core
        /// </summary>
        /// <param name="creditCardModel">Source credit card</param>
        /// <param name="creditCard">Target credit card</param>
        public void MapToCreditCard(CreditCardModel creditCardModel, ref Mediachase.Commerce.Customers.CreditCard creditCard)
        {
            creditCard.CardType = (int)creditCardModel.CreditCardType;
            creditCard.CreditCardNumber = creditCardModel.CreditCardNumber;
            creditCard.LastFourDigits =
                creditCardModel.CreditCardNumber.Substring(creditCardModel.CreditCardNumber.Length - 4);
            creditCard.SecurityCode = creditCardModel.CreditCardSecurityCode;
            creditCard.ExpirationMonth = creditCardModel.ExpirationMonth;
            creditCard.ExpirationYear = creditCardModel.ExpirationYear;
            if (creditCardModel.CurrentContact != null)
            {
                creditCard.ContactId = creditCardModel.CurrentContact.PrimaryKeyId;
            }
            else if (!string.IsNullOrEmpty(creditCardModel.OrganizationId))
            {
                creditCard.OrganizationId =
                    PrimaryKeyId.Parse(creditCardModel.OrganizationId);
            }

            if (!string.IsNullOrEmpty(creditCardModel.CreditCardId))
            {
                creditCard.PrimaryKeyId = PrimaryKeyId.Parse(creditCardModel.CreditCardId);
            }
        }

        /// <summary>
        /// Map credit card of commerce core to credit card view model
        /// </summary>
        /// <param name="creditCard">Source credit card</param>
        /// <param name="creditCardModel">Target credit card</param>
        public void MapToModel(Mediachase.Commerce.Customers.CreditCard creditCard, ref CreditCardModel creditCardModel)
        {
            creditCardModel.CreditCardType = (Mediachase.Commerce.Customers.CreditCard.eCreditCardType)creditCard.CardType;
            creditCardModel.CreditCardNumber = creditCard.CreditCardNumber;
            creditCardModel.CreditCardSecurityCode = creditCard.SecurityCode;
            creditCardModel.ExpirationMonth = creditCard.ExpirationMonth;
            creditCardModel.ExpirationYear = creditCard.ExpirationYear;
            creditCardModel.CreditCardId = creditCard.PrimaryKeyId.ToString();

            if (creditCard.OrganizationId != null)
            {
                creditCardModel.Organization = _organizationService.GetOrganizationModel((Guid)creditCard.OrganizationId);
            }
            else if (creditCard.ContactId != null)
            {
                creditCardModel.CurrentContact = _customerContext.GetContactById(new Guid(creditCard.ContactId.ToString()));
            }
        }

        /// <summary>
        /// Get credit card by id
        /// </summary>
        /// <param name="creditCardId">Credit card id</param>
        public Mediachase.Commerce.Customers.CreditCard GetCreditCard(string creditCardId)
        {
            if (string.IsNullOrEmpty(creditCardId))
            {
                return null;
            }

            return Enumerable.OfType<Mediachase.Commerce.Customers.CreditCard>(BusinessManager.List(
                CreditCardEntity.ClassName,
                new FilterElement[1] { new FilterElement("CreditCardId", FilterElementType.Equal, new Guid(creditCardId)) }))
                .FirstOrDefault();
        }

        /// <summary>
        /// Append a list of credit card to current credit card
        /// </summary>
        /// <param name="customerContact"></param>
        /// <param name="organization"></param>
        /// <param name="currentListCards"></param>
        /// <param name="appendListCreditCards"></param>
        private void AddRangeCreditCard(CustomerContact customerContact, OrganizationModel organization, List<CreditCardModel> currentListCards, IEnumerable<Mediachase.Commerce.Customers.CreditCard> appendListCreditCards)
        {
            currentListCards.AddRange(appendListCreditCards.Select(x => new CreditCardModel
            {
                CreditCardNumber = x.CreditCardNumber,
                CreditCardType = (Mediachase.Commerce.Customers.CreditCard.eCreditCardType)x.CardType,
                CreditCardSecurityCode = x.SecurityCode,
                ExpirationMonth = x.ExpirationMonth,
                ExpirationYear = x.ExpirationYear,
                CreditCardId = x.PrimaryKeyId.ToString(),
                CurrentContact = customerContact,
                Organization = organization
            }));
        }
    }
}