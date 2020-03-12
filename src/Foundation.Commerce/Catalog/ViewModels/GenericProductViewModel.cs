using Foundation.Commerce.Models.Catalog;

namespace Foundation.Commerce.Catalog.ViewModels
{
    public class GenericProductViewModel : ProductViewModelBase<GenericProduct, GenericVariant>
    {
        public GenericProductViewModel()
        {
        }

        public GenericProductViewModel(GenericProduct fashionProduct) : base(fashionProduct)
        {
        }
    }
}