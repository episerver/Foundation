namespace Foundation.Features.Experiences.Elements
{
    /// <summary>
    /// Image element for Visual Builder (opti-astro: Image component).
    /// </summary>
    [ContentType(DisplayName = "Image",
        GUID = "27FDF47C-534A-4A26-834D-7A1BB3DA5586",
        Description = "An image element",
        GroupName = "Elements",
        CompositionBehaviors = new[] { "ElementEnabled" })]
    public class ImageElement : BlockData
    {
        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Display(Name = "Image", Order = 10)]
        public virtual ContentReference Image { get; set; }

        [CultureSpecific]
        [Display(Name = "Alt text", Order = 20)]
        public virtual string AltText { get; set; }
    }
}
