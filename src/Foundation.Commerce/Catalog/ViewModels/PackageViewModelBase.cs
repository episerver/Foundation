using EPiServer.Commerce.Catalog.ContentTypes;
using System.Collections.Generic;

namespace Foundation.Commerce.Catalog.ViewModels
{
    public abstract class PackageViewModelBase<TPackage> : EntryViewModelBase<TPackage> where TPackage : PackageContent
    {
        protected PackageViewModelBase()
        {
        }

        protected PackageViewModelBase(TPackage package) : base(package)
        {
            Package = package;
        }

        public TPackage Package { get; set; }
        public IEnumerable<CatalogContentBase> Entries { get; set; }
    }
}
