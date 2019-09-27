using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Foundation.Cms.Blocks
{
    [ContentType(
        DisplayName = "Vimeo Video",
        GUID = "a8172c33-e087-4e68-980e-a79b0e093675",
        Description = "Displays Vimeo video",
        GroupName = "Multimedia")]
    [ImageUrl("~/assets/icons/gfx/Multimedia-thumbnail.png")]
    public class VimeoBlock : FoundationBlockData
    {
        private VimeoUrl _vimeoUrl;

        [Display(
            Name = "Vimeo Link",
            Description = "URL link to Vimeo video",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        [Required]
        [RegularExpression(@"^https?:\/\/(?:www\.)?vimeo.com\/?(?=\w+)(?:\S+)?$",
            ErrorMessage = "The Url must be a valid Vimeo video link")]
        [Searchable(false)]
        public virtual string VimeoVideoLink { get; set; }

        [Display(
            Name = "CoverImage",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        [UIHint(UIHint.Image)]
        [Searchable(false)]
        public virtual ContentReference CoverImage { get; set; }

        [ScaffoldColumn(false)]
        public virtual VimeoUrl VimeoVideo
        {
            get
            {
                var videoId = VimeoVideoLink;

                if (!string.IsNullOrEmpty(videoId))
                {
                    if (_vimeoUrl == null)
                    {
                        _vimeoUrl = new VimeoUrl(videoId);
                    }

                    return _vimeoUrl;
                }

                return null;
            }
        }

        [Display(
            Name = "Heading",
            Description = "Heading for the video",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        [CultureSpecific]
        public virtual string Heading { get; set; }

        [Display(
            Name = "Video Text",
            Description = "Descriptive text for the video",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        [CultureSpecific]
        public virtual XhtmlString VideoText { get; set; }

        [ScaffoldColumn(false)] public bool HasVideo => !string.IsNullOrEmpty(VimeoVideoLink);

        [ScaffoldColumn(false)] public bool HasCoverImage => CoverImage != null;

        [Editable(false)]
        public bool HasHeadingText => !string.IsNullOrEmpty(Heading) || VideoText != null && !VideoText.IsEmpty;
    }

    public class VimeoUrl
    {
        private const string UrlRegex = @"vimeo\.com/(\d+)";

        public VimeoUrl(string videoUrl) => GetVideoId(videoUrl);

        public string Id { get; set; }

        private void GetVideoId(string videoUrl)
        {
            var regex = new Regex(UrlRegex);

            var match = regex.Match(videoUrl);

            if (match.Success)
            {
                Id = match.Groups[1].Value;
            }
        }

        public string GetIframeUrl(bool autoPlay) => "//player.vimeo.com/video/" + Id + "?title=0&byline=0&portrait=0" + (autoPlay ? "&autoplay=1" : "");
    }
}