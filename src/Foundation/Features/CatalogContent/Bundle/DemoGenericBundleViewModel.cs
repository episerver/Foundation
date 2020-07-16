using EPiServer.Personalization.Commerce.Tracking;
using Foundation.Social.ViewModels;
using System.Collections.Generic;

namespace Foundation.Features.CatalogContent.Bundle
{
    public class DemoGenericBundleViewModel : GenericBundleViewModel, IEntryViewModelBase
    {
        public ReviewsViewModel Reviews { get; set; }
        public IEnumerable<Recommendation> AlternativeProducts { get; set; }
        public IEnumerable<Recommendation> CrossSellProducts { get; set; }
    }
}
