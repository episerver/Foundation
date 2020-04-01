using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Personalization
{
    [ContentType(DisplayName = "Visitor Intelligence Tracking Block",
        GUID = "E9F5009A-6794-4AC3-9311-10C95BFB1DE6",
        Description = "A block that allows you to set a tracking event name and description to insert this information into Visitor Intelligence",
        GroupName = CmsPersonalizationGroupNames.CmsPersonalization)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-17.png")]
    public class TrackingBlock : BlockData
    {
        [Required]
        [RegularExpression("([a-z]+[A-Z]+\\w+)+",
            ErrorMessage = "The tracking event name must be in lower camel case (Examples: epiPageView)")]
        [Display(Name = "Tracking event name")]
        public virtual string TrackingEventName { get; set; }

        [Required]
        [UIHint(UIHint.Textarea)]
        [Display(Name = "Description")]
        public virtual string Description { get; set; }
    }
}