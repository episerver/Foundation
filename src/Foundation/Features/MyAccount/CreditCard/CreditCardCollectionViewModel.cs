using Foundation.Commerce.Customer;
using Foundation.Features.Shared;
using System.Collections.Generic;

namespace Foundation.Features.MyAccount.CreditCard
{
    /// <summary>
    /// Represent for all credit cards of user or an organization
    /// </summary>
    public class CreditCardCollectionViewModel : ContentViewModel<CreditCardPage>
    {
        public CreditCardCollectionViewModel(CreditCardPage currentPage) : base(currentPage)
        {
        }

        public IEnumerable<CreditCardModel> CreditCards { get; set; }
        public FoundationContact CurrentContact { get; set; }
        public bool IsB2B { get; set; }
    }
}