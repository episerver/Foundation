﻿using EPiServer.Commerce.Catalog.Linking;

namespace Foundation.Features.CatalogContent.Package
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
        public IEnumerable<EntryRelation> EntriesRelation { get; set; }
    }
}
