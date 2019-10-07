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
    [ContentType(DisplayName = "Hero Block", GUID = "8bdfac81-3dbd-43b9-a092-522bd67ee8b3",
        Description = "Image Block with Overlay for text", GroupName = "Content")]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-23.png")]
    public class HeroBlock : FoundationBlockData
    {
        [CultureSpecific]
        [UIHint(UIHint.Image)]
        public virtual ContentReference BackgroundImage { get; set; }

        [CultureSpecific]
        [Display(Name = "Background Video")]
        [UIHint(UIHint.Video)]
        public virtual ContentReference MainBackgroundVideo { get; set; }

        [Display(Name = "Callout")]
        [UIHint("HeroBlockCallout")]
        public virtual HeroBlockCallout Callout { get; set; }

        public virtual Url Link { get; set; }
    }

    [ContentType(DisplayName = "Hero Block Callout", GUID = "7A3C9E9E-8612-4722-B795-2A93CB54A476",
        AvailableInEditMode = false)]
    public class HeroBlockCallout : BlockData
    {
        public virtual bool ShowBackgroundColor { get; set; }

        public virtual string BackgroundColor { get; set; }

        [Range(0, 1.0, ErrorMessage = "Opacity only allows value between 0 and 1")]
        public virtual double Opacity { get; set; }

        [CultureSpecific]
        public virtual XhtmlString CalloutContent { get; set; }

        [Display(Name = "Callout text color", Description = "Sets text color of callout content", GroupName = SystemTabNames.Content)]
        [SelectOne(SelectionFactoryType = typeof(HeroBlockTextColorSelectionFactory))]
        public virtual string CalloutTextColor { get; set; }

        [SelectOne(SelectionFactoryType = typeof(CalloutContentAlignmentSelectionFactory))]
        public virtual string CalloutContentAlignment { get; set; }

        [Display(Name = "Padding Top")]
        public virtual int PaddingTop { get; set; }

        [Display(Name = "Padding Right")]
        public virtual int PaddingRight { get; set; }

        [Display(Name = "Padding Bottom")]
        public virtual int PaddingBottom { get; set; }

        [Display(Name = "Padding Left")]
        public virtual int PaddingLeft { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            Opacity = 1;
            PaddingTop = 15;
            PaddingRight = 60;
            PaddingBottom = 15;
            PaddingLeft = 60;
            CalloutTextColor = ColorThemes.None;
        }
    }
}