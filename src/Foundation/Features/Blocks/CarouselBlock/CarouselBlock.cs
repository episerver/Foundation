using Foundation.Features.Media;

namespace Foundation.Features.Blocks.CarouselBlock
{
    [ContentType(DisplayName = "Carousel Block",
        GUID = "980ead74-1d13-45d6-9c5c-16f900269ee6",
        Description = "Allows users to create a slider using a collection of Images or Hero blocks",
        GroupName = GroupNames.Content)]
    [ImageUrl("/icons/cms/blocks/imageslider.png")]
    public class CarouselBlock : FoundationBlockData
    {
        [CultureSpecific]
        [AllowedTypes(new[] { typeof(HeroBlock.HeroBlock), typeof(ImageMediaData), typeof(MediaData), typeof(ContainerBlock.ContainerBlock) })]
        [Display(
            Name = "Carousel items",
            GroupName = "Carousel",
            Description = "List of carousel items",            
            Order = 10)]

        public virtual ContentArea CarouselItems { get; set; }

        [Display(
            Name = "Carousel Controls",
            GroupName = "Carousel",
            Description = "Carousel Controls",
            Order = 20)]
        public virtual CarouselControls CarouselControls { get; set; }
    }

    [ContentType(
        DisplayName = "Carousel Controls",
        GUID = "C2F80550-EAE7-4786-83C2-64C1C42C3583",
        AvailableInEditMode = false)]
    public class CarouselControls : BlockData
    {
        [CultureSpecific]
        [Display(
            Name = "Show controls",
            GroupName = "Carousel",
            Description = "Show carousel controls, left / right arrows so users can manually move between items",
            Order = 20)]
        public virtual bool ShowControls { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Show indicators",
            GroupName = "Carousel",
            Description = "Show carousel indicators, small bars below the carousel items to allow users to jump to any item in the carousel",
            Order = 30)]
        public virtual bool ShowIndicators { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Auto play",
            GroupName = "Carousel",
            Description = "If set the carousel will auto-play",
            Order = 30)]
        public virtual bool AutoPlay { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Fade",
            GroupName = "Carousel",
            Description = "If set, the carousely transition will fade",
            Order = 40)]
        public virtual bool Fade { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Interval in milliseconds",
            GroupName = "Carousel",
            Description = "The amount of time in milliseconds before automatically advancing to the next slide. Defaults to 5 seconds",
            Order = 50)]
        [Range(1000, 300000)] // Between 1s and 5mins
        public virtual int Interval { get; set; }

        [CultureSpecific]
        [SelectOne(SelectionFactoryType = typeof(CarouselThemeSelectionFactory))]
        [Display(
           Name = "Indicator color theme",
           GroupName = "Carousel",
           Order = 60)]
        public virtual string Theme { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            ShowControls = true;
            ShowIndicators = true;
            AutoPlay = true;
            Fade = false;
            Interval = 5000;
            Theme = "light";
        }
        public class CarouselThemeSelectionFactory : ISelectionFactory
        {
            public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
            {
                return new List<SelectItem>
                {
                    new SelectItem { Text = "Light", Value = "light" },
                    new SelectItem { Text = "Dark", Value = "dark" },
                };
            }
        }
    }
}