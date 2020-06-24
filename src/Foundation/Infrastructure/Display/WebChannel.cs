using EPiServer.Web;
using System.Web;

namespace Foundation.Infrastructure.Display
{
    /// <summary>
    /// Defines the 'Web' content channel
    /// </summary>
    public class WebChannel : DisplayChannel
    {
        public override string ChannelName => "Web";

        public override bool IsActive(HttpContextBase context)
        {
            return !context.Request.Browser.IsMobileDevice;
        }
    }
}