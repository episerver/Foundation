using Mediachase.Commerce.Customers;

namespace Foundation.Commerce.Customer.Services
{
    public class LoyaltyService : ILoyaltyService
    {
        public void AddNumberOfOrders()
        {
            var currentContact = CustomerContext.Current.CurrentContact;
            if (currentContact != null)
            {
                var contact = new FoundationContact(currentContact);
                contact.NumberOfOrders += 1;
                contact.Points += 10;
                contact.Tier = SetTier(contact.Points);
                contact.SaveChanges();
            }
        }

        public void AddNumberOfReviews()
        {
            var currentContact = CustomerContext.Current.CurrentContact;
            if (currentContact != null)
            {
                var contact = new FoundationContact(currentContact);
                contact.NumberOfReviews += 1;
                contact.Points += 1;
                contact.Tier = SetTier(contact.Points);
                contact.SaveChanges();
            }
        }

        private string SetTier(int points)
        {
            if (points <= 100)
            {
                return "Classic";
            }

            if (points <= 200)
            {
                return "Bronze";
            }

            if (points <= 500)
            {
                return "Silver";
            }

            if (points <= 1000)
            {
                return "Gold";
            }

            if (points <= 2000)
            {
                return "Platinum";
            }

            return "Diamond";
        }
    }
}