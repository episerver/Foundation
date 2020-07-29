using EPiServer.Web;
using System.Web;
using System.Web.WebPages;

namespace Foundation.Infrastructure.Display
{
    public class MobileChannel : DisplayChannel
    {
        public override string ChannelName => "Mobile";

        public override string ResolutionId => typeof(IphoneVerticalResolution).FullName;

        public override bool IsActive(HttpContextBase context) => context.GetOverriddenBrowser().IsMobileDevice;
    }
}