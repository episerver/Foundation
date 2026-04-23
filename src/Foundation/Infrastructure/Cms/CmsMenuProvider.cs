using EPiServer.Security;
using EPiServer.Shell;

namespace Foundation.Infrastructure.Cms
{
    [MenuProvider]
    public class CmsMenuProvider : IMenuProvider
    {
        private const string MainMenuPath = MenuPaths.Global + "/extensions";

        public IEnumerable<MenuItem> GetMenuItems()
        {
            var menuItems = new List<MenuItem>();

            menuItems.Add(new SectionMenuItem("Extensions", MainMenuPath)
            {
                IsAvailable = (_) => PrincipalInfo.CurrentPrincipal.IsInRole("CommerceAdmins"),
                SortIndex = 6000
            });

            menuItems.Add(new UrlMenuItem("Bulk Update", MainMenuPath + "/bulkupdate", "/episerver/foundation/bulkupdate")
            {
                SortIndex = 100,
            });

            menuItems.Add(new FoundationAdminMenuItem("Coupons", MainMenuPath + "/coupons", "/episerver/foundation/promotions")
            {
                SortIndex = 200,
                Paths = new[] { "foundation/promotions", "foundation/editPromotionCoupons" }
            });

            menuItems.Add(new UrlMenuItem("Comments Manager", MainMenuPath + "/commentsmanager", "/episerver/foundation/moderation")
            {
                SortIndex = 200,
            });

            return menuItems;
        }
    }

    public class FoundationAdminMenuItem : UrlMenuItem
    {
        public IEnumerable<string> Paths { get; set; }

        public FoundationAdminMenuItem(string text, string path, string url) : base(text, path, url)
        {
        }

        public override bool IsSelected(HttpContext requestContext)
        {
            Validate.RequiredParameter("requestContext", requestContext);
            var requestUrl = requestContext.Request != null ? requestContext.Request.Path.Value.Trim('/') : null;
            return Paths.Any(x =>  requestUrl.Contains(x));
        }
    }
}
