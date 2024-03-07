using EPiServer.Find.Api.Querying;
using EPiServer.Find.Api.Querying.Filters;

namespace Foundation.Features.Blocks.ProductFilterBlocks
{
    [ContentType(DisplayName = "String Filter Block",
        GUID = "efcb0aef-5427-49bb-ab1b-2b429a2f2cc3",
        Description = "Filter product search blocks by field values",
        GroupName = "Commerce")]
    [ImageUrl("/icons/cms/pages/CMS-icon-page-14.png")]
    public class StringFilterBlock : FilterBaseBlock
    {
        [CultureSpecific(true)]
        [Display(Name = "Value", Description = "The value to filter search results on", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string FieldValue { get; set; }

        public override Filter GetFilter()
        {
            if (!string.IsNullOrEmpty(FieldName) && !string.IsNullOrEmpty(FieldValue))
            {
                return new TermFilter($"{FieldName}$$string", FieldFilterValue.Create(FieldValue));
            }
            return null;
        }
    }
}