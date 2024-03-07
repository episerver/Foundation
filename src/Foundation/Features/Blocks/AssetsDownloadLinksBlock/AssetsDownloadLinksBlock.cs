namespace Foundation.Features.Blocks.AssetsDownloadLinksBlock
{
    [ContentType(DisplayName = "Assets Download Links Block",
        GUID = "F8C78C8A-9EB8-4171-8A0B-8CA4B190DE3E",
        Description = "Blocks to show links for assets to download",
        GroupName = GroupNames.Content)]
    [ImageUrl("/icons/cms/blocks/CMS-icon-block-32.png")]
    public class AssetsDownloadLinksBlock : FoundationBlockData
    {
        [Display(
            Name = "Root content",
            Description = "Root content can be a folder or catalog content",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        [Required]
        [AllowedTypes(new[] { typeof(ContentFolder), typeof(CatalogContentBase) })]
        [UIHint(EPiServer.Commerce.UIHint.AllContent)]
        public virtual ContentReference RootContent { get; set; }

        [Display(
            Name = "Number of results",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual int Count { get; set; }

        [Display(
            Name = "Group name of assets",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual string GroupName { get; set; }
    }
}