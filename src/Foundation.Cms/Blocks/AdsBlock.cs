using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "Ads Block",
        GUID = "fcd91a0b-403f-439b-a9a1-e0c832662996",
        Description = "Simple Rich Text Block in Ads",
        GroupName = CmsGroupNames.Content)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-03.png")]
    public class AdsBlock : FoundationBlockData
    {
        [CultureSpecific]
        [Display(Name = "Main body")]
        public virtual XhtmlString AdsBody { get; set; }
    }
}
