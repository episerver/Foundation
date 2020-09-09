using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blocks.YouTubeBlock
{
    [ContentType(DisplayName = "YouTube Block",
        GUID = "67429E0D-9365-407C-8A49-69489382BBDC",
        Description = "Display YouTube video",
        GroupName = GroupNames.Content)]
    [ImageUrl("~/assets/icons/cms/blocks/video.png")]
    public class YouTubeBlock : FoundationBlockData
    {
        [Required]
        [Editable(true)]
        [Display(Name = "YouTube link", Description = "URL link to YouTube video", GroupName = SystemTabNames.Content, Order = 10)]
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
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string Heading { get; set; }

        [Editable(true)]
        [CultureSpecific]
        [Display(Name = "Main body", Description = "Descriptive text for the video", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual XhtmlString MainBody { get; set; }

        [Editable(false)]
        public bool HasVideo => !string.IsNullOrEmpty(YouTubeLink);

        [Editable(false)]
        public bool HasHeadingText => !string.IsNullOrEmpty(Heading) || MainBody != null && !MainBody.IsEmpty;
    }
}