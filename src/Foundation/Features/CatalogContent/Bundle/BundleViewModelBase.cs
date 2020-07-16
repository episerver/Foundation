using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using System.Collections.Generic;

namespace Foundation.Features.CatalogContent.Bundle
{
    public abstract class BundleViewModelBase<TBundle> : EntryViewModelBase<TBundle> where TBundle : BundleContent
    {
        protected BundleViewModelBase()
        {
        }

        protected BundleViewModelBase(TBundle fashionBundle) : base(fashionBundle)
        {
            Bundle = fashionBundle;
        }

        public TBundle Bundle { get; set; }
        public IEnumerable<EntryContentBase> Entries { get; set; }
        public IEnumerable<EntryRelation> EntriesRelation { get; set; }
    }
}
