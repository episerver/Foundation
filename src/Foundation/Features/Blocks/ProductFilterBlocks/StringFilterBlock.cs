using EPiServer.Commerce.Catalog.ContentTypes;
using System.Reflection;

namespace Foundation.Features.Blocks.ProductFilterBlocks
{
    [ContentType(DisplayName = "String Filter Block",
        GUID = "efcb0aef-5427-49bb-ab1b-2b429a2f2cc3",
        Description = "Filter product search blocks by field values",
        GroupName = "Commerce")]
    [ImageUrl("/icons/cms/pages/CMS-icon-page-14.png")]
    public class StringFilterBlock : FilterBaseBlock
    {
        [CultureSpecific]
        [Display(Name = "Value", Description = "The value to filter search results on", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string FieldValue { get; set; }

        public override Func<EntryContentBase, bool> GetPredicate()
        {
            if (string.IsNullOrEmpty(FieldName) || string.IsNullOrEmpty(FieldValue))
                return null;

            return entry =>
            {
                var prop = entry.GetType().GetProperty(FieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                var val = prop?.GetValue(entry)?.ToString();
                return val != null && val.Contains(FieldValue, StringComparison.OrdinalIgnoreCase);
            };
        }
    }
}
