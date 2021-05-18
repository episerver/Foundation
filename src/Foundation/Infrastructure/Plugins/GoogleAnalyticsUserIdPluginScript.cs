using EPiServer.GoogleAnalytics.Web.Tracking;
using EPiServer.Security;
using Mediachase.Commerce.Security;
using AlloyTemplates;

namespace Foundation.Infrastructure.Plugins
{
    public class GoogleAnalyticsUserIdPluginScript : IPluginScript
    {
        public string GetScript()
        {
            if (HttpContextHelper.Current.User.Identity.IsAuthenticated)
            {
                return string.Format("ga('set', 'userId', '{0}');", HttpContextHelper.Current.User.Identity.Name);
            }

            return string.Format("ga('set', 'userId', '{0}');", PrincipalInfo.CurrentPrincipal.GetContactId());
        }
    }
}