using EPiServer.Personalization.Commerce.Tracking;
using Foundation.Social.ViewModels;
using System.Collections.Generic;

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

        public ReviewsViewModel Reviews { get; set; }
        public IEnumerable<Recommendation> AlternativeProducts { get; set; }
        public IEnumerable<Recommendation> CrossSellProducts { get; set; }
    }
}
