using Foundation.Commerce.Models.Catalog;

namespace Foundation.Commerce.Catalog.ViewModels
{
    public class GenericPackageViewModel : PackageViewModelBase<GenericPackage>
    {
        public GenericPackageViewModel()
        {
        }

        public GenericPackageViewModel(GenericPackage fashionPackage) : base(fashionPackage)
        {
        }
    }
}
