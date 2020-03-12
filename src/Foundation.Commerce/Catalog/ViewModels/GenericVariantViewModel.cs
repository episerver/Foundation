using Foundation.Commerce.Models.Catalog;

namespace Foundation.Commerce.Catalog.ViewModels
{
    public class GenericVariantViewModel : EntryViewModelBase<GenericVariant>
    {
        public GenericVariantViewModel()
        {
        }

        public GenericVariantViewModel(GenericVariant variantBase) : base(variantBase)
        {
        }
    }
}
