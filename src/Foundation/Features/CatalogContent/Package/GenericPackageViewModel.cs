namespace Foundation.Features.CatalogContent.Package
{
    public class GenericPackageViewModel : PackageViewModelBase<GenericPackage>, IEntryViewModelBase
    {
        public GenericPackageViewModel()
        {
        }

        public GenericPackageViewModel(GenericPackage fashionPackage) : base(fashionPackage)
        {
        }

        // AlternativeProducts and CrossSellProducts removed: EPiServer.Personalization.Commerce has no CMS 13 version.
    }
}
