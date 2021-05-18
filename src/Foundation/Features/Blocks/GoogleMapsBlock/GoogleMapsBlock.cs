using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blocks.GoogleMapsBlock
{
    [ContentType(DisplayName = "Google Maps Block",
        GUID = "8fc31051-6d22-4445-b92d-7c394267fa49",
        Description = "Display Google Maps",
        GroupName = GroupNames.SocialMedia)]
    [ImageUrl("/icons/cms/blocks/map.png")]
    public class GoogleMapsBlock : FoundationBlockData
    {
        [Required]
        [Display(Name = "API Key")]
        public virtual string ApiKey { get; set; }

        [Display(Name = "Search term")]
        public virtual string SearchTerm { get; set; }

        public virtual double Height { get; set; }
    }
}