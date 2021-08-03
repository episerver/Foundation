using EPiServer.Cms.UI.Admin.ContentTypes.Internal;
using EPiServer.Cms.UI.VisitorGroups.Controllers.Internal;
using EPiServer.Core;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Shell;
using EPiServer.Web;
using System.Collections.Generic;

namespace Foundation.Infrastructure.Display
{
    public class FoundationQuickNavigatorItemProvider : IQuickNavigatorItemProvider
    {
        public IDictionary<string, QuickNavigatorMenuItem> GetMenuItems(ContentReference currentContent)
        {
            var accessor = ServiceLocator.Current.GetInstance<IPrincipalAccessor>();
            var menuItems = new Dictionary<string, QuickNavigatorMenuItem>();

            if (accessor.Principal.IsInRole("CmsAdmins") ||
                accessor.Principal.IsInRole("VisitorGroupAdmins"))
            {
                menuItems.Add("Visitor Groups",
                    new QuickNavigatorMenuItem("/shell/cms/visitorgroups/index/name",
                        Paths.ToResource(typeof(ManageVisitorGroupsController).Assembly, "ManageVisitorGroups"), null, "true", null));
            }

            if (accessor.Principal.IsInRole("CmsAdmins"))
            {
                menuItems.Add("Admin mode",
                    new QuickNavigatorMenuItem("/shell/cms/menu/admin", Paths.ToResource(typeof(ContentTypesController).Assembly, "default"), null, "true", null));
            }

            return menuItems;
        }

        public int SortOrder => int.MaxValue - 10;
    }
}