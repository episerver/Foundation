using Foundation.Cms.ViewModels;
using Foundation.Commerce.Models.Pages;
using System.Collections.Generic;

namespace Foundation.Commerce.Customer.ViewModels
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