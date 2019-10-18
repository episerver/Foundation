using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "Blog Archive Block", GUID = "73F610A5-D705-4BCA-960A-3CA03F312D30", GroupName = CmsGroupNames.Blog, AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/blocks/cms-icon-block-19.png")]
    public class BlogArchiveBlock : FoundationBlockData
    {
        public virtual string Heading { get; set; }

        [Display(Name = "Blog start")]
        public virtual ContentReference BlogStart { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            Heading = "Archive";
        }
    }
}