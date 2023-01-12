using EPiServer.Personalization.Commerce.Tracking;

namespace Foundation.Features.CatalogContent
{
    public interface IEntryViewModelBase
    {
        //ReviewsViewModel Reviews { get; set; }
        IEnumerable<Recommendation> AlternativeProducts { get; set; }
        IEnumerable<Recommendation> CrossSellProducts { get; set; }
    }
}