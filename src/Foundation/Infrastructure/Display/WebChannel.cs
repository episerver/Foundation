using EPiServer.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Wangkanai.Detection;

namespace Foundation.Infrastructure.Display
{
    /// <summary>
    /// Defines the 'Web' content channel
    /// </summary>
    public class WebChannel : DisplayChannel
    {
        public override string ChannelName => "Web";

        public override bool IsActive(HttpContext context)
        {
            var detection = context.RequestServices.GetRequiredService<IDetection>();
            return detection.Device.Type == DeviceType.Desktop;
        }
    }
}