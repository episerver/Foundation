using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Commerce.Models.EditorDescriptors
{
    public static class ProductSearchSortOrder
    {
        public const string None = "None";
        public const string BestSellerByQuantity = "Best seller by quantity";
        public const string BestSellerByRevenue = "Best seller by revenue";
        public const string NewestProducts = "Newest products by date";
    }

    public class SortOrderSelectionFactory : ISelectionFactory
    {
        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "None", Value = ProductSearchSortOrder.None },
                new SelectItem { Text = "Best seller by quantity", Value = ProductSearchSortOrder.BestSellerByQuantity },
                new SelectItem { Text = "Best seller by revenue", Value = ProductSearchSortOrder.BestSellerByRevenue },
                new SelectItem { Text = "Newest products by date", Value = ProductSearchSortOrder.NewestProducts }
            };
        }
    }
}