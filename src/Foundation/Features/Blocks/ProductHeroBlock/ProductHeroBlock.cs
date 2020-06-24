using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Foundation.Cms.Attributes;
using Foundation.Features.Shared;
using Foundation.Features.Shared.SelectionFactories;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blocks.ProductHeroBlock
{
    [ContentType(DisplayName = "Product Hero Block",
        GUID = "6b43692b-6abd-49b1-b5f2-48ffbb8e626a",
        Description = "Product here block",
        GroupName = GroupNames.Commerce)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-23.png")]
    public class ProductHeroBlock : FoundationBlockData
    {
        [SelectOne(SelectionFactoryType = typeof(ProductHeroBlockLayoutSelectionFactory))]
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Layout { get; set; }

        [UIHint("ProductHeroBlockCallout")]
        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual ProductHeroBlockCallout Callout { get; set; }

        [UIHint("ProductHeroBlockImage")]
        [Display(GroupName = SystemTabNames.Content, Order = 30)]
        public virtual ProductHeroBlockImage Image { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            Layout = "CalloutLeft";
        }
    }

    [ContentType(DisplayName = "Product Hero Block Callout", GUID = "8C80C82F-6D92-4998-B541-08E12DAA28EC", AvailableInEditMode = false)]
    public class ProductHeroBlockCallout : BlockData
    {
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual XhtmlString Text { get; set; }

        [SelectOne(SelectionFactoryType = typeof(PaddingSelectionFactory))]
        [Display(Name = "Padding", Order = 1)]
        public virtual string Padding { get; set; }

        [SelectOne(SelectionFactoryType = typeof(MarginSelectionFactory))]
        [Display(Name = "Margin", Order = 2)]
        public virtual string Margin { get; set; }

        [SelectOne(SelectionFactoryType = typeof(BackgroundColorSelectionFactory))]
        [Display(Name = "Background Color", Order = 3)]
        public virtual string BackgroundColor { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            Padding = "p-1";
            Margin = "m-1";
            BackgroundColor = "transparent";
        }
    }

    [ContentType(DisplayName = "Product Hero Block Image", GUID = "F0E7CC46-524D-4237-9A5F-9410238006E4", AvailableInEditMode = false)]
    public class ProductHeroBlockImage : BlockData
    {
        [MaxElements(1)]
        [CultureSpecific]
        [AllowedTypes(new[] { typeof(EntryContentBase) })]
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual ContentArea Product { get; set; }

        [Display(Name = "Image width", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual int ImageWidth { get; set; }

        [Display(Name = "Image height", GroupName = SystemTabNames.Content, Order = 21)]
        public virtual int ImageHeight { get; set; }

        [SelectOne(SelectionFactoryType = typeof(ProductHeroBlockImagePositionSelectionFactory))]
        [Display(Name = "Image position", GroupName = SystemTabNames.Content, Order = 30,
            Description = "Set image position in the image section to the left, center, right or set a certain position using paddings")]
        public virtual string ImagePosition { get; set; }

        [Display(Name = "Padding top", Order = 40)]
        public virtual int PaddingTop { get; set; }

        [Display(Name = "Padding right", Order = 41)]
        public virtual int PaddingRight { get; set; }

        [Display(Name = "Padding bottom", Order = 42)]
        public virtual int PaddingBottom { get; set; }

        [Display(Name = "Padding left", Order = 43)]
        public virtual int PaddingLeft { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            ImageWidth = 0;
            ImageHeight = 0;
            ImagePosition = "ImageCenter";
            PaddingTop = 0;
            PaddingRight = 0;
            PaddingBottom = 0;
            PaddingLeft = 0;
        }
    }
}