using EPiServer.Personalization.Commerce.Tracking;
using Foundation.Social.ViewModels;
using System.Collections.Generic;

namespace Foundation.Demo.Interfaces
{
    public interface IDemoEntryViewModelBase
    {
        ReviewsViewModel Reviews { get; set; }
        IEnumerable<Recommendation> AlternativeProducts { get; set; }
        IEnumerable<Recommendation> CrossSellProducts { get; set; }
    }
}
