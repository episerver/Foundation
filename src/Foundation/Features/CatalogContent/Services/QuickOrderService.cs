using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using Foundation.Features.MyOrganization.QuickOrderBlock;
using Mediachase.Commerce.InventoryService;
using System.Linq;

namespace Foundation.Features.CatalogContent.Services
{
    public interface IQuickOrderService
    {
        string ValidateProduct(ContentReference variationReference, decimal quantity, string code);
        QuickOrderProductViewModel GetProductByCode(ContentReference productReference);
        decimal GetTotalInventoryByEntry(string code);
    }

    public class QuickOrderService : IQuickOrderService
    {
        private readonly IContentLoader _contentLoader;
        private readonly IInventoryService _inventoryService;

        public QuickOrderService(IContentLoader contentLoader, IInventoryService inventoryService)
        {
            _contentLoader = contentLoader;
            _inventoryService = inventoryService;
        }

        public string ValidateProduct(ContentReference variationReference, decimal quantity, string code)
        {
            if (ContentReference.IsNullOrEmpty(variationReference))
            {
                return $"The product with SKU {code} does not exist.";
            }

            var variantContent = _contentLoader.Get<VariationContent>(variationReference);
            var maxQuantity = GetTotalInventoryByEntry(variantContent.Code);
            if (quantity > maxQuantity)
            {
                return $"Quantity ordered is bigger than in stock quantity for the product with SKU {code}.";
            }

            return null;
        }

        public QuickOrderProductViewModel GetProductByCode(ContentReference productReference)
        {
            var product = new QuickOrderProductViewModel();
            if (!ContentReference.IsNullOrEmpty(productReference))
            {
                var variantContent = _contentLoader.Get<VariationContent>(productReference);
                product.ProductName = variantContent.Name;
                product.Sku = variantContent.Code;
                product.UnitPrice = variantContent.GetDefaultPrice() != null
                    ? variantContent.GetDefaultPrice().UnitPrice.Amount
                    : 0;
            }

            return product;
        }

        public decimal GetTotalInventoryByEntry(string code) => _inventoryService.QueryByEntry(new[] { code }).Sum(x => x.PurchaseAvailableQuantity);
    }
}