using EPiServer.Security;
using EPiServer.Shell;
using EPiServer.Shell.Navigation;
using System.Collections.Generic;

namespace Foundation.Demo
{
    [MenuProvider]
    public class DemoMenuProvider : IMenuProvider
    {
        public IEnumerable<MenuItem> GetMenuItems()
        {
            var dashboard = new UrlMenuItem("Configuration",
                "/global/foundation/configuration",
                $"{Paths.ToResource("Foundation.Demo", "foundationconfiguration")}/index")
            {
                SortIndex = 0,
                IsAvailable = (_) => PrincipalInfo.CurrentPrincipal.IsInRole("CommerceAdmins")
            };

            var powerbi = new DropDownMenuItem("Power Bi", "/global/foundation/powerbi")
            {
                IsAvailable = (request) => PrincipalInfo.CurrentPrincipal.IsInRole("CommerceAdmins"),
                SortIndex = 400,
                Alignment = MenuItemAlignment.Right
            };

            var ga = new UrlMenuItem("Google Analytics",
                "/global/foundation/powerbi/google",
                $"{Paths.ToResource("Foundation.Demo", "bireports")}/index/google")
            {
                SortIndex = 13,
                IsAvailable = (request) => PrincipalInfo.CurrentPrincipal.IsInRole("CommerceAdmins")
            };

            var advance = new UrlMenuItem("Marketing",
                "/global/foundation/powerbi/marketing",
                $"{Paths.ToResource("Foundation.Demo", "bireports")}/index/marketing")
            {
                SortIndex = 11,
                IsAvailable = (request) => PrincipalInfo.CurrentPrincipal.IsInRole("CommerceAdmins")
            };

            var perform = new UrlMenuItem("Commerce",
                "/global/foundation/powerbi/commerce",
                $"{Paths.ToResource("Foundation.Demo", "bireports")}/index/commerce")
            {
                SortIndex = 10,
                IsAvailable = (request) => PrincipalInfo.CurrentPrincipal.IsInRole("CommerceAdmins")
            };

            var segment = new UrlMenuItem("Segments",
                "/global/foundation/powerbi/commerce",
                $"{Paths.ToResource("Foundation.Demo", "bireports")}/index/segments")
            {
                SortIndex = 12,
                IsAvailable = (request) => PrincipalInfo.CurrentPrincipal.IsInRole("CommerceAdmins")
            };

            return new MenuItem[]
            {
                dashboard,
                powerbi,
                ga,
                advance,
                perform,
                segment
            };
        }
    }
}
