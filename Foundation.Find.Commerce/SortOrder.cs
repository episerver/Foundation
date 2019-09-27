using System.Web.Helpers;

namespace Foundation.Find.Commerce
{
    public class SortOrder
    {
        public ProductSortOrder Name { get; set; }
        public string Key { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}
