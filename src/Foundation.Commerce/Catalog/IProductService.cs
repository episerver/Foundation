using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using Foundation.Commerce.Catalog.ViewModels;
using System.Collections.Generic;

namespace Foundation.Commerce.Catalog
{
    public interface IProductService
    {
        ProductTileViewModel GetProductTileViewModel(EntryContentBase entry);
        IEnumerable<ProductTileViewModel> GetProductTileViewModels(IEnumerable<ContentReference> entryLinks);
        string GetSiblingVariantCodeBySize(string siblingCode, string size);
        IEnumerable<VariationContent> GetVariants(ProductContent currentContent);
    }
}