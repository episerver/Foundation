using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "Google Maps Embed Block",
        GUID = "8fc31051-6d22-4445-b92d-7c394267fa49",
        Description = "Display Google Maps Embed",
        GroupName = CmsGroupNames.SocialMedia)]
    public class GoogleMapsEmbedBlock : FoundationBlockData
    {
        [Required]
        [Display(Name = "API Key")]
        public virtual string ApiKey { get; set; }

        [Display(Name = "Search term")]
        public virtual string SearchTerm { get; set; }

        public virtual double Height { get; set; }
    }
}