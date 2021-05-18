using EPiServer.Personalization.Commerce.Tracking;
using System.Collections.Generic;

namespace Foundation.Features.CatalogContent
{
    public interface IEntryViewModelBase
    {
        //ReviewsViewModel Reviews { get; set; }
        IEnumerable<Recommendation> AlternativeProducts { get; set; }
        IEnumerable<Recommendation> CrossSellProducts { get; set; }
    }
}