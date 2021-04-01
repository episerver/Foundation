using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Find;

namespace Foundation.Features.Blocks.ProductFilterBlocks
{
    [ContentType(DisplayName = "Exists Filter Block",
        GUID = "E93C9A50-4B62-4116-8E56-1DF84AB93EF7",
        Description = "Filter product that has a value for the given field",
        GroupName = "Commerce")]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-14.png")]
    public class ExistsFilterBlock : FilterBaseBlock
    {
        public override Filter GetFilter()
        {
            if (string.IsNullOrEmpty(FieldName))
            {
                return null;
            }
            var fullFieldName = SearchClient.Instance.GetFullFieldName(FieldName);
            return new ExistsFilter(fullFieldName);
        }
    }
}