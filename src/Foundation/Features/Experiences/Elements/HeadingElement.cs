namespace Foundation.Features.Experiences.Elements
{
    /// <summary>
    /// Heading element for Visual Builder (inspired by opti-astro's Heading component).
    /// Style options (size, alignment, transform) are provided by the "DefaultHeading"
    /// display template seeded in <see cref="DisplayTemplatesInit"/>.
    /// </summary>
    [ContentType(DisplayName = "Heading",
        GUID = "FDA16953-75A0-4C2B-BDB4-D7EDFB6B860B",
        Description = "A heading element",
        GroupName = "Elements",
        CompositionBehaviors = new[] { "ElementEnabled" })]
    public class HeadingElement : BlockData
    {
        [CultureSpecific]
        [Display(Name = "Heading text", Order = 10)]
        public virtual string Heading { get; set; }
    }
}
