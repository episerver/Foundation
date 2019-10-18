using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(GUID = "C5B623C6-8930-4F97-98F2-0E0B6965DEDF", DisplayName = "Tag Cloud Block", GroupName = CmsGroupNames.Blog, AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/blocks/cms-icon-block-18.png")]
    public class TagCloudBlock : BlockData
    {
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content)]
        public virtual string Heading { get; set; }

        [Display(Name = "Blog tag link page", GroupName = SystemTabNames.Content)]
        public virtual ContentReference BlogTagLinkPage { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            Heading = "Tags";
        }
    }
}