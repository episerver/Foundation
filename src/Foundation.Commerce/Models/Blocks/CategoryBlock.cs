using EPiServer.Commerce;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using Foundation.Cms.Blocks;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce.Models.Blocks
{
    [ContentType(DisplayName = "Category Block",
        GUID = "24496187-a0f4-4eac-9b02-787cae620fed",
        Description = "Category block",
        GroupName = CommerceGroupNames.Commerce)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-30.png")]
    public class CategoryBlock : FoundationBlockData
    {
        public virtual string Heading { get; set; }

        [UIHint(UIHint.CatalogContent)]
        public virtual ContentReference Catalog { get; set; }
    }
}