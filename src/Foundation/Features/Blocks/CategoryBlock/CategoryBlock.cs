namespace Foundation.Features.Blocks.CategoryBlock
{
    [ContentType(DisplayName = "Category Block",
        GUID = "24496187-a0f4-4eac-9b02-787cae620fed",
        Description = "Category block",
        GroupName = GroupNames.Commerce)]
    [ImageUrl("/icons/cms/blocks/CMS-icon-block-30.png")]
    public class CategoryBlock : FoundationBlockData
    {
        public virtual string Heading { get; set; }

        [UIHint(EPiServer.Commerce.UIHint.CatalogContent)]
        public virtual ContentReference Catalog { get; set; }
    }
}