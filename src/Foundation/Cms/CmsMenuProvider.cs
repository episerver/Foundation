using EPiServer.Security;
using EPiServer.Shell.Navigation;
using System.Collections.Generic;

namespace Foundation.Cms
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

            menuItems.Add(new UrlMenuItem("Bulk Update", MainMenuPath + "/bulkupdate", "/bulkupdate")
            {
                SortIndex = 100,
            });

            menuItems.Add(new UrlMenuItem("Coupons", MainMenuPath + "/coupons", "/singleusecoupon")
            {
                SortIndex = 200,
            });

            menuItems.Add(new UrlMenuItem("Gift cards", MainMenuPath + "/giftcards", "/giftcardmanager")
            {
                SortIndex = 300,
            });

            menuItems.Add(new UrlMenuItem("Moderation", MainMenuPath + "/moderation", "/moderation")
            {
                SortIndex = 400,
            });

            return menuItems;
        }
    }
}
