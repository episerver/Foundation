using EPiServer.Shell;
using EPiServer.Shell.ViewComposition;

namespace Foundation.Test
{
    [Component]
    public class CommentsPaneNavigationComponent : ComponentDefinitionBase
    {
        public CommentsPaneNavigationComponent()
            : base("epi-cms/component/MainNavigationComponent")
        {
            Categories = new[] { "content" };
            Title = "Comments";
            SortOrder = 1000;
            PlugInAreas = new[] { PlugInArea.AssetsDefaultGroup };
            Settings.Add(new Setting("repositoryKey", CommentsPaneDescriptor.RepositoryKey));

        }
    }
}