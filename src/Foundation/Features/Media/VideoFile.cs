using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Labs.ContentManager.Cards;
using EPiServer.Labs.ContentManager.Dashboard;
using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Media
{
    [ContentType(DisplayName = "Video File",
        GUID = "8a9d9d4b-cd4b-40e8-a777-414cfbda7770",
        Description = "Used for video file types such as mp4, flv, webm")]
    [MediaDescriptor(ExtensionString = "mp4,flv,webm")]
    public class VideoFile : VideoData, IDashboardItem
    {
        [UIHint(UIHint.Image)]
        [Display(Name = "Preview image", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual ContentReference PreviewImage { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string Copyright { get; set; }

        [Display(Name = "Display controls", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual bool DisplayControls { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 40)]
        public virtual bool Autoplay { get; set; }

        public void SetItem(ItemModel itemModel)
        {
            itemModel.Description = Copyright;
            itemModel.Image = PreviewImage;
        }
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            Autoplay = false;
            DisplayControls = true;
        }
    }
}