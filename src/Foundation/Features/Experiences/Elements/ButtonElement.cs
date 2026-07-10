namespace Foundation.Features.Experiences.Elements
{
    /// <summary>
    /// Button / call-to-action element for Visual Builder (opti-astro: Button component).
    /// Style variants are provided by the "DefaultButton" display template seeded in
    /// <see cref="DisplayTemplatesInit"/>.
    /// </summary>
    [ContentType(DisplayName = "Button",
        GUID = "59865234-4CE5-4857-A686-2B36CBB98706",
        Description = "A clickable button element for calls-to-action",
        GroupName = "Elements",
        CompositionBehaviors = new[] { "ElementEnabled" })]
    public class ButtonElement : BlockData
    {
        [CultureSpecific]
        [Display(Name = "Label", Order = 10)]
        public virtual string Label { get; set; }

        [Display(Name = "Link", Order = 20)]
        public virtual Url Link { get; set; }
    }
}
