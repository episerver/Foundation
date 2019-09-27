using EPiServer.Security;
using EPiServer.Shell.Navigation;
using System.Collections.Generic;

namespace Foundation.Cms
{
    [MenuProvider]
    public class CmsMenuProvider : IMenuProvider
    {
        public IEnumerable<MenuItem> GetMenuItems()
        {
            return new MenuItem[]
            {
                new SectionMenuItem("Extensions", "/global/foundation")
                {
                    IsAvailable = (_) => PrincipalInfo.CurrentPrincipal.IsInRole("CommerceAdmins"),
                    SortIndex = 6000
                }
            };
        }
    }
}
