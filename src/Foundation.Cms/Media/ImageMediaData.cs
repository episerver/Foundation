using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.Blobs;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Media
{
    [ContentType(GUID = "20644be7-3ca1-4f84-b893-ee021b73ce6c")]
    [MediaDescriptor(ExtensionString = "jpg,jpeg,jpe,ico,gif,bmp,png,svg,webp")]
    public class ImageMediaData : ImageData
    {
        [Editable(false)]
        [ImageDescriptor(Width = 256, Height = 256)]
        public virtual Blob LargeThumbnail { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Alternate text",
            Description = "Description of the image",
            GroupName = SystemTabNames.Content,
            Order = 50)]
        public virtual string Description { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Link",
            Description = "Link to content",
            GroupName = SystemTabNames.Content,
            Order = 60)]
        [UIHint("allcontent")]
        public virtual ContentReference Link { get; set; }

        [Display(
            Order = 10)]
        public virtual string Title { get; set; }

        [Display(
            Order = 20)]
        public virtual string AltText { get; set; }

        public virtual string Copyright { get; set; }

        [Editable(false)]
        public virtual string FileSize { get; set; }

        [Display(
            Order = 30)]
        public virtual string CreditsText { get; set; }

        [Display(
            Order = 40)]
        public virtual Url CreditsLink { get; set; }

        public virtual string AccentColor { get; set; }

        public virtual string Caption { get; set; }

        public virtual string ClipArtType { get; set; }

        public virtual string DominantColorBackground { get; set; }

        public virtual string DominantColorForeground { get; set; }

        public virtual IList<string> DominantColors { get; set; }

        public virtual IList<string> ImageCategories { get; set; }

        public virtual bool IsAdultContent { get; set; }

        public virtual bool IsBwImg { get; set; }

        public virtual bool IsRacyContent { get; set; }

        public virtual string LineDrawingType { get; set; }

        public virtual IList<string> Tags { get; set; }
    }
}