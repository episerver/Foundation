// CMS 13: ContentTypeController is internal in CMS 13. Use Assembly.GetExecutingAssembly() instead.
// using EPiServer.Cms.UI.Admin.ContentTypes.Internal;
// EPiServer.Cms.UI.VisitorGroups.Controllers.Internal removed in CMS 13. Visitor groups menu item disabled.
// using EPiServer.Cms.UI.VisitorGroups.Controllers.Internal;
namespace Foundation.Infrastructure.Display
{
    // CMS 13: IQuickNavigatorItemProvider custom items removed.
    // QuickNavigatorMenuItem internally resolves the imageUrl via Paths.ToResource, which requires
    // the calling assembly to be a registered Shell module. Foundation is not registered as one.
    // The built-in CMS quick navigator already provides Edit/Preview/Admin links.
    public class FoundationQuickNavigatorItemProvider : IQuickNavigatorItemProvider
    {
        public IDictionary<string, QuickNavigatorMenuItem> GetMenuItems(ContentReference currentContent)
            => new Dictionary<string, QuickNavigatorMenuItem>();

        public int SortOrder => int.MaxValue - 10;
    }
}