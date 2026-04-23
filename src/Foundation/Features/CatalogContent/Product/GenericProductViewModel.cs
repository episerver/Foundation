using Foundation.Features.CatalogContent.Variation;

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

        // AlternativeProducts and CrossSellProducts removed: EPiServer.Personalization.Commerce has no CMS 13 version.
    }
}
