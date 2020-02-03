using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.Blobs;
using EPiServer.Framework.DataAnnotations;
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
        [Display(Name = "Large thumbnail", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual Blob LargeThumbnail { get; set; }

        [Editable(false)]
        [Display(Name = "File size", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string FileSize { get; set; }

        [Display(Name = "Accent color", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual string AccentColor { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 40)]
        public virtual string Caption { get; set; }

        [Display(Name = "Clip art type", GroupName = SystemTabNames.Content, Order = 50)]
        public virtual string ClipArtType { get; set; }

        [Display(Name = "Dominant color background", GroupName = SystemTabNames.Content, Order = 60)]
        public virtual string DominantColorBackground { get; set; }

        [Display(Name = "Dominant color foreground", GroupName = SystemTabNames.Content, Order = 70)]
        public virtual string DominantColorForeground { get; set; }

        [Display(Name = "Dominant colors", GroupName = SystemTabNames.Content, Order = 80)]
        public virtual IList<string> DominantColors { get; set; }

        [Display(Name = "Image categories", GroupName = SystemTabNames.Content, Order = 90)]
        public virtual IList<string> ImageCategories { get; set; }

        [Display(Name = "Is adult content", GroupName = SystemTabNames.Content, Order = 100)]
        public virtual bool IsAdultContent { get; set; }

        [Display(Name = "Is black & white image", GroupName = SystemTabNames.Content, Order = 110)]
        public virtual bool IsBwImg { get; set; }

        [Display(Name = "Is racy content", GroupName = SystemTabNames.Content, Order = 120)]
        public virtual bool IsRacyContent { get; set; }

        [Display(Name = "Line drawing type", GroupName = SystemTabNames.Content, Order = 130)]
        public virtual string LineDrawingType { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 140)]
        public virtual IList<string> Tags { get; set; }

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 150)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(Description = "Description of the image", GroupName = SystemTabNames.Content, Order = 160)]
        public virtual string Description { get; set; }

        [CultureSpecific]
        [Display(Name = "Alternate text", GroupName = SystemTabNames.Content, Order = 170)]
        public virtual string AltText { get; set; }

        [CultureSpecific]
        [Display(Name = "Credits text", GroupName = SystemTabNames.Content, Order = 180)]
        public virtual string CreditsText { get; set; }

        [CultureSpecific]
        [Display(Name = "Credits link", GroupName = SystemTabNames.Content, Order = 190)]
        public virtual Url CreditsLink { get; set; }

        [CultureSpecific]
        [UIHint("allcontent")]
        [Display(Description = "Link to content", GroupName = SystemTabNames.Content, Order = 200)]
        public virtual ContentReference Link { get; set; }

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 210)]
        public virtual string Copyright { get; set; }
    }
}