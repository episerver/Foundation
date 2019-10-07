using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "YouTube Block",
        GUID = "67429E0D-9365-407C-8A49-69489382BBDC",
        Description = "Display YouTube video",
        GroupName = "Multimedia")]
    [ImageUrl("~/assets/icons/cms/blocks/video.png")]
    public class YouTubeBlock : FoundationBlockData
    {
        [Editable(true)]
        [Display(
            Name = "YouTube Link",
            Description = "URL link to YouTube video",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        [Required]
        public virtual string YouTubeLink
        {
            get
            {
                var linkName = this["YouTubeLink"] as string;
                if (string.IsNullOrEmpty(linkName))
                {
                    return null;
                }

                if (!linkName.Contains("youtube") || !linkName.Contains("/watch?v=") && !linkName.Contains("/v/") &&
                    !linkName.Contains("/embed/"))
                {
                    return null;
                }

                if (linkName.Contains("/watch?v="))
                {
                    linkName = linkName.Replace("/watch?v=", "/embed/");
                }
                else if (linkName.Contains("/v/"))
                {
                    linkName = linkName.Replace("/watch?v=", "/embed/");
                }

                return linkName;
            }
            set => this["YouTubeLink"] = value;
        }

        [Editable(true)]
        [Display(
            Name = "Heading",
            Description = "Heading for the video",
            GroupName = SystemTabNames.Content,
            Order = 2)]
        [CultureSpecific]
        public virtual string Heading { get; set; }

        [Editable(true)]
        [Display(
            Name = "Video Text",
            Description = "Descriptive text for the video",
            GroupName = SystemTabNames.Content,
            Order = 3)]
        [CultureSpecific]
        public virtual XhtmlString VideoText { get; set; }

        [Editable(false)] public bool HasVideo => !string.IsNullOrEmpty(YouTubeLink);

        [Editable(false)]
        public bool HasHeadingText => !string.IsNullOrEmpty(Heading) || VideoText != null && !VideoText.IsEmpty;
    }
}