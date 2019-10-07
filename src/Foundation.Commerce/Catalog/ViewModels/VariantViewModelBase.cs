using EPiServer.Commerce.Catalog.ContentTypes;
using System.Collections.Generic;

namespace Foundation.Commerce.Catalog.ViewModels
{
    public abstract class VariantViewModelBase<TVariant> : EntryViewModelBase<TVariant> where TVariant : VariationContent
    {
        protected VariantViewModelBase()
        {
        }

        protected VariantViewModelBase(TVariant genericVariant) : base(genericVariant)
        {
            Variant = genericVariant;
        }

        public TVariant Variant { get; set; }
        public IEnumerable<EntryContentBase> Entries { get; set; }
    }
}
