using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Labs.ContentManager.Cards;
using EPiServer.Labs.ContentManager.Dashboard;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Foundation.Features.Shared;
using Foundation.Features.Shared.SelectionFactories;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blocks.HeroBlock
{
    [ContentType(DisplayName = "Hero Block",
        GUID = "8bdfac81-3dbd-43b9-a092-522bd67ee8b3",
        Description = "Image block with overlay for text",
        GroupName = GroupNames.Content)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-22.png")]
    public class HeroBlock : FoundationBlockData, IDashboardItem
    {
        [Searchable(false)]
        [SelectOne(SelectionFactoryType = typeof(BlockRatioSelectionFactory))]
        [Display(Name = "Block ratio (width:height)", Order = 5)]
        public virtual string BlockRatio { get; set; }

        [UIHint(UIHint.Image)]
        [Display(Name = "Image", Order = 10)]
        public virtual ContentReference BackgroundImage { get; set; }

        [UIHint(UIHint.Video)]
        [Display(Name = "Video", Order = 20)]
        public virtual ContentReference MainBackgroundVideo { get; set; }

        [Display(Order = 30)]
        public virtual Url Link { get; set; }

        [UIHint("HeroBlockCallout")]
        [Display(Name = "Callout", GroupName = SystemTabNames.Content, Order = 40)]
        public virtual HeroBlockCallout Callout { get; set; }

        public void SetItem(ItemModel itemModel)
        {
            itemModel.Description = Callout?.CalloutContent?.ToHtmlString();
            itemModel.Image = BackgroundImage;
        }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            BlockOpacity = 1;
            BlockRatio = "2:1";
        }
    }

    [ContentType(DisplayName = "Hero Block Callout", GUID = "7A3C9E9E-8612-4722-B795-2A93CB54A476", AvailableInEditMode = false)]
    public class HeroBlockCallout : BlockData
    {
        [CultureSpecific]
        [Display(Name = "Text", Order = 10)]
        public virtual XhtmlString CalloutContent { get; set; }

        [Searchable(false)]
        [SelectOne(SelectionFactoryType = typeof(CalloutContentAlignmentSelectionFactory))]
        [Display(Name = "Text placement", Order = 20)]
        public virtual string CalloutContentAlignment { get; set; }

        [Searchable(false)]
        [ClientEditor(ClientEditingClass = "foundation/editors/ColorPicker")]
        [Display(Name = "Text color", Description = "Sets text color of callout content", Order = 30)]
        public virtual string CalloutTextColor { get; set; }

        [Searchable(false)]
        [ClientEditor(ClientEditingClass = "foundation/editors/ColorPicker")]
        [Display(Name = "Background color", Order = 40)]
        public virtual string BackgroundColor { get; set; }

        [Range(0, 1.0, ErrorMessage = "Opacity only allows value between 0 and 1")]
        [Display(Name = "Callout opacity (0 to 1)", Order = 50)]
        public virtual double CalloutOpacity { get; set; }

        [Searchable(false)]
        [SelectOne(SelectionFactoryType = typeof(CalloutPositionSelectionFactory))]
        [Display(Name = "Callout position", Order = 55)]
        public virtual string CalloutPosition { get; set; }

        [Searchable(false)]
        [SelectOne(SelectionFactoryType = typeof(PaddingSelectionFactory))]
        [Display(Name = "Padding", Order = 60)]
        public virtual string Padding { get; set; }

        [Searchable(false)]
        [SelectOne(SelectionFactoryType = typeof(MarginSelectionFactory))]
        [Display(Name = "Margin", Order = 65)]
        public virtual string Margin { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            Padding = "p-1";
            Margin = "m-0";
            BackgroundColor = "#00000000";
            CalloutOpacity = 1;
            CalloutPosition = "center";
            CalloutContentAlignment = "left";
            CalloutTextColor = "#000000ff";
        }
    }
}