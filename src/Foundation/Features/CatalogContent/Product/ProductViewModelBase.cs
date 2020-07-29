using EPiServer.Commerce.Catalog.ContentTypes;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Features.CatalogContent.Product
{
    public abstract class ProductViewModelBase<TProduct, TVariant> : EntryViewModelBase<TProduct>
        where TProduct : ProductContent
        where TVariant : VariationContent
    {
        protected ProductViewModelBase()
        {
        }

        protected ProductViewModelBase(TProduct product) : base(product)
        {
            Product = product;
        }

        public TProduct Product { get; set; }
        public TVariant Variant { get; set; }
        public IList<SelectListItem> Colors { get; set; }
        public IList<SelectListItem> Sizes { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public List<VariantViewModel> Variants { get; set; }
        public string WishlistLabel { get; set; }
    }
}