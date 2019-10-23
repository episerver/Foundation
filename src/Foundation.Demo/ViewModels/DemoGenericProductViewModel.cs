using EPiServer.Personalization.Commerce.Tracking;
using Foundation.Commerce.Catalog.ViewModels;
using Foundation.Demo.Interfaces;
using Foundation.Social.ViewModels;
using System.Collections.Generic;

namespace Foundation.Demo.ViewModels
{
    public class DemoGenericProductViewModel : GenericProductViewModel, IDemoEntryViewModelBase
    {
        public ReviewsViewModel Reviews { get; set; }
        public IEnumerable<Recommendation> AlternativeProducts { get; set; }
        public IEnumerable<Recommendation> CrossSellProducts { get; set; }
    }
}
