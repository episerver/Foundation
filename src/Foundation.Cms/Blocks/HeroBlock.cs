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
        Description = "Image block with overlay for text",
        GroupName = CmsGroupNames.Content)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-22.png")]
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

        [CultureSpecific]
        [Range(0, 1.0, ErrorMessage = "Opacity only allows value between 0 and 1")]
        [Display(Name = "Block opacity (0 to 1)", Order = 25)]
        public virtual double BlockOpacity { get; set; }

        [Display(Order = 30)]
        public virtual Url Link { get; set; }

        [UIHint("HeroBlockCallout")]
        [Display(Name = "Callout", GroupName = SystemTabNames.Content, Order = 40)]
        public virtual HeroBlockCallout Callout { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            BlockOpacity = 0;
        }
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
        [Display(Name = "Text color", Description = "Sets text color of callout content", Order = 30)]
        public virtual string CalloutTextColor { get; set; }

        [Display(Name = "Background color", Order = 40)]
        public virtual string BackgroundColor { get; set; }

        [Range(0, 1.0, ErrorMessage = "Opacity only allows value between 0 and 1")]
        [Display(Name = "Callout opacity (0 to 1)", Order = 50)]
        public virtual double CalloutOpacity { get; set; }

        [SelectOne(SelectionFactoryType = typeof(CalloutPositionSelectionFactory))]
        [Display(Name = "Callout position", Order = 55)]
        public virtual string CalloutPosition { get; set; }

        [SelectOne(SelectionFactoryType = typeof(FoundationBlockDataPaddingUnitSelectionFactory))]
        [Display(Name = "Padding unit", Order = 60)]
        public virtual string PaddingUnit { get; set; }

        [Display(Name = "Padding top", Order = 61)]
        public virtual int PaddingTop { get; set; }

        [Display(Name = "Padding right", Order = 62)]
        public virtual int PaddingRight { get; set; }

        [Display(Name = "Padding bottom", Order = 63)]
        public virtual int PaddingBottom { get; set; }

        [Display(Name = "Padding left", Order = 64)]
        public virtual int PaddingLeft { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            PaddingUnit = "%";
            PaddingTop = 2;
            PaddingRight = 2;
            PaddingBottom = 2;
            PaddingLeft = 2;
            CalloutOpacity = 0.5;
            BackgroundColor = "white";
            CalloutPosition = "flex-middle";
            CalloutContentAlignment = "left";
            CalloutTextColor = ColorThemes.None;
        }
    }
}