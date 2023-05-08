namespace Foundation.Features.Blocks.ContainerBlock
{
    [ContentType(DisplayName = "Container Block",
        GUID = "8bdfac81-1dbd-43b9-a012-522bd67ee8b3",
        Description = "Allow to style individual blocks, as well as groups of blocks",
        GroupName = GroupNames.Content)]
    [ImageUrl("/icons/cms/blocks/CMS-icon-block-04.png")]
    public class ContainerBlock : FoundationBlockData
    {
        [Display(Name = "Main content area")]
        [AllowedTypes(new[] { typeof(IContentData) })]
        public virtual ContentArea MainContentArea { get; set; }

        [Display(Name = "CSS class")]
        public virtual string CssClass { get; set; }
    }
}