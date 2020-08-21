using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "My Block",
        GUID = "8497c00c-1989-4fc9-938b-d0445a00a284",
        Description = "My first block",
        GroupName = CmsGroupNames.Content,
        AvailableInEditMode = true)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-26.png")]
    public class MyBlock : FoundationBlockData
    {
        [CultureSpecific]
        [Display(Name = "Header", Order = 10, GroupName = SystemTabNames.Content)]
        public virtual string Header { get; set; }

        [Display(Name = "Description", Order = 20, GroupName = SystemTabNames.Content)]
        public virtual XhtmlString Description { get; set; }
    }
}
