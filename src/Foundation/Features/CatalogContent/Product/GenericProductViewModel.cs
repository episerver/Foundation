using EPiServer.Personalization.Commerce.Tracking;
using Foundation.Features.CatalogContent.Variation;
using Foundation.Social.ViewModels;
using System.Collections.Generic;

namespace Foundation.Features.CatalogContent.Product
{
    public class GenericProductViewModel : ProductViewModelBase<GenericProduct, GenericVariant>, IEntryViewModelBase
    {
        public GenericProductViewModel()
        {
        }

        public GenericProductViewModel(GenericProduct fashionProduct) : base(fashionProduct)
        {
        }

        public ReviewsViewModel Reviews { get; set; }
        public IEnumerable<Recommendation> AlternativeProducts { get; set; }
        public IEnumerable<Recommendation> CrossSellProducts { get; set; }
    }
}