using EPiServer.VisualBuilder;

namespace Foundation.Features.Experiences
{
    /// <summary>
    /// Visual Builder section. Section types get SectionEnabled composition behavior
    /// automatically (ContentType.CompositionBehaviors defaults it for the Section base
    /// type) and carry the built-in grid <see cref="SectionData.Layout"/> that provides
    /// rows and columns — those are structural nodes, not content types.
    /// </summary>
    [ContentType(DisplayName = "Blank Section",
        GUID = "B9197E82-4DC8-486F-9A36-8562F928DF0A",
        Description = "A blank grid section for Visual Builder experiences",
        GroupName = "Experiences")]
    public class BlankSection : SectionData
    {
    }
}
