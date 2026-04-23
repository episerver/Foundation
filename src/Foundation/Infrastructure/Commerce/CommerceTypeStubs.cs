// Commerce 15 removed types: Mediachase.Commerce.Customers.CreditCard and
// EPiServer.Commerce.Order.ICreditCardPayment. Stubs provided here so the
// credit-card feature compiles; functionality will be restored when Commerce
// 15 GA ships these types or they are replaced with a newer approach.

using Mediachase.BusinessFoundation.Data;

namespace Mediachase.Commerce.Customers
{
    public class CreditCard
    {
        public enum eCreditCardType
        {
            MasterCard = 0,
            Visa = 1,
            AMEX = 2,
            Discover = 3,
            JCB = 4,
            DinersClub = 5,
        }

        public string CreditCardId { get; set; }
        public string CreditCardName { get; set; }
        public string CreditCardNumber { get; set; }
        public string SecurityCode { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }
        public eCreditCardType CreditType { get; set; }
        public string CustomerId { get; set; }

        // Commerce 15 removed: Additional stub properties needed by CreditCardService.
        public int CardType { get; set; }
        public string LastFourDigits { get; set; }
        public PrimaryKeyId? ContactId { get; set; }
        public PrimaryKeyId? OrganizationId { get; set; }
        public PrimaryKeyId? PrimaryKeyId { get; set; }

        // Commerce 15 removed: Factory method stub.
        public static CreditCard CreateInstance() => new CreditCard();

        // Commerce 15 removed: Delete stub.
        public static void Delete(PrimaryKeyId id) { /* Commerce 15 removed */ }

        // Commerce 15 removed: AddCreditCard stub (was on CustomerContact).
        // Moved to CustomerContact extension below.
    }
}

// Commerce 15 removed: CustomerContact.AddCreditCard and ContactCreditCards removed.
namespace Mediachase.Commerce.Customers
{
    public static class CustomerContactCreditCardExtensions
    {
        public static void AddCreditCard(this CustomerContact contact, CreditCard creditCard)
        {
            // Commerce 15 removed: AddCreditCard removed from CustomerContact.
        }

        public static System.Collections.Generic.IEnumerable<CreditCard> ContactCreditCards(this CustomerContact contact)
        {
            // Commerce 15 removed: ContactCreditCards removed from CustomerContact.
            return System.Linq.Enumerable.Empty<CreditCard>();
        }
    }
}

// Commerce 15 removed: CustomerContext.GetContactCreditCards and GetOrganizationCreditCards removed.
namespace Mediachase.Commerce.Customers
{
    public static class CustomerContextCreditCardExtensions
    {
        public static System.Collections.Generic.IEnumerable<CreditCard> GetContactCreditCards(this CustomerContext context, CustomerContact contact)
        {
            // Commerce 15 removed: GetContactCreditCards removed.
            return System.Linq.Enumerable.Empty<CreditCard>();
        }

        public static System.Collections.Generic.IEnumerable<CreditCard> GetOrganizationCreditCards(this CustomerContext context, object organization)
        {
            // Commerce 15 removed: GetOrganizationCreditCards removed.
            return System.Linq.Enumerable.Empty<CreditCard>();
        }
    }
}

namespace EPiServer.Commerce.Order
{
    public interface ICreditCardPayment
    {
        string CardType { get; set; }
        string CustomerName { get; set; }
        string CreditCardNumber { get; set; }
        int ExpirationMonth { get; set; }
        int ExpirationYear { get; set; }
        string CreditCardSecurityCode { get; set; }
    }
}
