using EPiServer.Commerce.Catalog.ContentTypes;
using System.Reflection;

namespace Foundation.Features.Blocks.ProductFilterBlocks
{
    [ContentType(DisplayName = "Exists Filter Block",
        GUID = "E93C9A50-4B62-4116-8E56-1DF84AB93EF7",
        Description = "Filter products that have a value for the given field",
        GroupName = "Commerce")]
    [ImageUrl("/icons/cms/pages/CMS-icon-page-14.png")]
    public class ExistsFilterBlock : FilterBaseBlock
    {
        public override Func<EntryContentBase, bool> GetPredicate()
        {
            if (string.IsNullOrEmpty(FieldName))
                return null;

            return entry =>
            {
                var prop = entry.GetType().GetProperty(FieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                var val = prop?.GetValue(entry);
                return val != null && !string.IsNullOrEmpty(val.ToString());
            };
        }
    }
}
