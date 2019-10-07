using EPiServer.Core;
using EPiServer.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(GUID = "73F610A5-D705-4BCA-960A-3CA03F312D30", DisplayName = "Blog Archive Block", GroupName = "Blog", AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/blocks/cms-icon-block-19.png")]
    public class BlogArchiveBlock : BlockData
    {
        [DefaultValue("Archive")]
        public virtual string Heading { get; set; }

        [Display(Name = "Blog Start")]
        public virtual ContentReference BlogStart { get; set; }
    }
}