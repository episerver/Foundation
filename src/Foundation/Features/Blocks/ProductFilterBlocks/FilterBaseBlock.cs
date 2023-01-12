using EPiServer.Find.Api.Querying;

namespace Foundation.Features.Blocks.ProductFilterBlocks
{
    public abstract class FilterBaseBlock : BlockData
    {
        public abstract Filter GetFilter();

        [CultureSpecific]
        [Display(Name = "Name", Description = "Name of field in index", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string FieldName { get; set; }
    }
}