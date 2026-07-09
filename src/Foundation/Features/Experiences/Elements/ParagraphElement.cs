namespace Foundation.Features.Experiences.Elements
{
    /// <summary>
    /// Rich text element for Visual Builder (opti-astro: Paragraph/RichText).
    /// </summary>
    [ContentType(DisplayName = "Paragraph",
        GUID = "838E4ACA-A17E-4638-AA51-FC6E8CA654BC",
        Description = "A rich text element",
        GroupName = "Elements",
        CompositionBehaviors = new[] { "ElementEnabled" })]
    public class ParagraphElement : BlockData
    {
        [CultureSpecific]
        [Display(Name = "Text", Order = 10)]
        public virtual XhtmlString Text { get; set; }
    }
}
