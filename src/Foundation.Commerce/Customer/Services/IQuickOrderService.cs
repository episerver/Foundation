using EPiServer.Core;
using Foundation.Commerce.Catalog.ViewModels;

namespace Foundation.Commerce.Customer.Services
{
    public interface IQuickOrderService
    {
        string ValidateProduct(ContentReference variationReference, decimal quantity, string code);
        ProductViewModel GetProductByCode(ContentReference productReference);
        decimal GetTotalInventoryByEntry(string code);
    }
}