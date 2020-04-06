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
        [SelectOne(SelectionFactoryType = typeof(BlockRatioSelectionFactory))]
        [Display(Name = "Block ratio (width-height)", Order = 5)]
        public virtual string BlockRatio { get; set; }

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
        [Display(Name = "Callout", GroupName = SystemTabNames.Content, Order = 40)]
        public virtual HeroBlockCallout Callout { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            BlockOpacity = 1;
            BlockRatio = "10-5";
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

        [ClientEditor(ClientEditingClass = "dijit/ColorPalette")]
        [Display(Name = "Text color", Description = "Sets text color of callout content", Order = 30)]
        public virtual string CalloutTextColor { get; set; }

        [SelectOne(SelectionFactoryType = typeof(BackgroundColorSelectionFactory))]
        [Display(Name = "Background color", Order = 40)]
        public virtual string BackgroundColor { get; set; }

        [Range(0, 1.0, ErrorMessage = "Opacity only allows value between 0 and 1")]
        [Display(Name = "Callout opacity (0 to 1)", Order = 50)]
        public virtual double CalloutOpacity { get; set; }

        [SelectOne(SelectionFactoryType = typeof(CalloutPositionSelectionFactory))]
        [Display(Name = "Callout position", Order = 55)]
        public virtual string CalloutPosition { get; set; }

        [SelectOne(SelectionFactoryType = typeof(PaddingSelectionFactory))]
        [Display(Name = "Padding", Order = 60)]
        public virtual string Padding { get; set; }

        [SelectOne(SelectionFactoryType = typeof(MarginSelectionFactory))]
        [Display(Name = "Margin", Order = 65)]
        public virtual string Margin { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            Padding = "p-1";
            Margin = "m-0";
            BackgroundColor = "transparent";
            CalloutOpacity = 1;
            CalloutPosition = "flex-middle";
            CalloutContentAlignment = "left";
            CalloutTextColor = "black";
        }
    }
}