using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Models.EditorDescriptors
{
    public class SortOrderSelectionFactory : ISelectionFactory
    {
        public static class ProductSearchSortOrder
        {
            public const string None = "None";
            public const string BestSellerByQuantity = "Best seller by quantity";
            public const string BestSellerByRevenue = "Best seller by revenue";
            public const string NewestProducts = "Newest products by date";
        }

        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var dic = new Dictionary<string, string>()
            {
                {"None", ProductSearchSortOrder.None},
                {"Best seller by quantity", ProductSearchSortOrder.BestSellerByQuantity},
                {"Best seller by revenue", ProductSearchSortOrder.BestSellerByRevenue},
                {"Newest products by date", ProductSearchSortOrder.NewestProducts}
            };

            return dic.Select(x => new SelectItem() { Text = x.Key, Value = x.Value });
        }
    }
}