using EPiServer.Web;
using System.Web;

namespace Foundation.Features.Preview
{
    public class PartialViewDisplayChannel : DisplayChannel
    {
        public const string PartialViewDisplayChannelName = "Partial View Preview";

        public override string ChannelName => PartialViewDisplayChannelName;

        public override bool IsActive(HttpContextBase context) => false;
    }
}