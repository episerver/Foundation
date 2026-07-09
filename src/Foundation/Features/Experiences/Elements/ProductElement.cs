using EPiServer.Commerce.Catalog.ContentTypes;

namespace Foundation.Features.Experiences.Elements
{
    /// <summary>
    /// Commerce product element for Visual Builder: lets editors drop a catalog entry
    /// (product/variant/bundle/package) into an experience. The reference is indexed
    /// into Optimizely Graph and the headless frontend resolves and renders the
    /// referenced product as a card.
    /// </summary>
    [ContentType(DisplayName = "Product",
        GUID = "5DB068EB-4B41-436D-A6C0-07991DC53117",
        Description = "A commerce catalog product element",
        GroupName = "Elements",
        CompositionBehaviors = new[] { "ElementEnabled" })]
    public class ProductElement : BlockData
    {
        [AllowedTypes(typeof(EntryContentBase))]
        [Display(Name = "Product", Order = 10)]
        public virtual ContentReference Product { get; set; }

        [CultureSpecific]
        [Display(Name = "Custom title (optional)", Order = 20)]
        public virtual string CustomTitle { get; set; }
    }
}
