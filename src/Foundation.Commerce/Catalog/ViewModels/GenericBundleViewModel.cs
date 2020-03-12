using Foundation.Commerce.Models.Catalog;

namespace Foundation.Commerce.Catalog.ViewModels
{
    public class GenericBundleViewModel : BundleViewModelBase<GenericBundle>
    {
        public GenericBundleViewModel()
        {
        }

        public GenericBundleViewModel(GenericBundle fashionBundle) : base(fashionBundle)
        {
        }
    }
}
