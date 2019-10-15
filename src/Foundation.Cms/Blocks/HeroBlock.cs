using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Foundation.Cms.EditorDescriptors;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "Hero Block",
        GUID = "8bdfac81-3dbd-43b9-a092-522bd67ee8b3",
        Description = "Image Block with Overlay for text", 
        GroupName = CmsGroupNames.Content)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-23.png")]
    public class HeroBlock : FoundationBlockData
    {
        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Display(Name = "Image", Order = 10)]
        public virtual ContentReference BackgroundImage { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Video)]
        [Display(Name = "Video", Order = 20)]
        public virtual ContentReference MainBackgroundVideo { get; set; }

        [Display(Order = 30)]
        public virtual Url Link { get; set; }

        [UIHint("HeroBlockCallout")]
        [Display(Name = "Hero block callout", GroupName = SystemTabNames.Content, Order = 40)]
        public virtual HeroBlockCallout Callout { get; set; }
    }

    [ContentType(DisplayName = "Hero Block Callout", GUID = "7A3C9E9E-8612-4722-B795-2A93CB54A476", AvailableInEditMode = false)]
    public class HeroBlockCallout : BlockData
    {
        [CultureSpecific]
        [Display(Name = "Text", Order = 10)]
        public virtual XhtmlString CalloutContent { get; set; }

        [SelectOne(SelectionFactoryType = typeof(CalloutContentAlignmentSelectionFactory))]
        [Display(Name = "Text placement", Order = 20)]
        public virtual string CalloutContentAlignment { get; set; }

        [SelectOne(SelectionFactoryType = typeof(HeroBlockTextColorSelectionFactory))]
        [Display(Name = "Callout text color", Description = "Sets text color of callout content", Order = 30)]
        public virtual string CalloutTextColor { get; set; }

        [Display(Name = "Background color", Order = 40)]
        public virtual string BackgroundColor { get; set; }

        [Range(0, 1.0, ErrorMessage = "Opacity only allows value between 0 and 1")]
        [Display(Order = 50)]
        public virtual double Opacity { get; set; }

        [Display(Name = "Padding top", Order = 51)]
        public virtual int PaddingTop { get; set; }

        [Display(Name = "Padding right", Order = 52)]
        public virtual int PaddingRight { get; set; }

        [Display(Name = "Padding bottom", Order = 53)]
        public virtual int PaddingBottom { get; set; }

        [Display(Name = "Padding left", Order = 54)]
        public virtual int PaddingLeft { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            Opacity = 0.5;
            BackgroundColor = "white";
            PaddingTop = 15;
            PaddingRight = 60;
            PaddingBottom = 15;
            PaddingLeft = 60;
            CalloutTextColor = ColorThemes.None;
            CalloutContentAlignment = "None";
        }
    }
}