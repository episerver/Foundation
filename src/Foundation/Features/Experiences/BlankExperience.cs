using EPiServer.VisualBuilder;

namespace Foundation.Features.Experiences
{
    /// <summary>
    /// Visual Builder experience. CMS 13 ships the <see cref="ExperienceData"/> base type
    /// but no concrete creatable experience — this is the code-first equivalent of the
    /// SaaS "Blank Experience". Editors compose sections, rows, columns and elements in
    /// Visual Builder; the composition is indexed into Optimizely Graph and rendered by
    /// the headless Next.js site.
    /// </summary>
    [ContentType(DisplayName = "Blank Experience",
        GUID = "49F095E6-10FF-4D38-8B54-AEB2F0E08C69",
        Description = "A blank Visual Builder experience for the headless site",
        GroupName = "Experiences")]
    public class BlankExperience : ExperienceData
    {
    }
}
