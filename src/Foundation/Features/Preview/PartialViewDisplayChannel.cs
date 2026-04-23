namespace Foundation.Features.Preview
{
    public class PartialViewDisplayChannel : DisplayChannel
    {
        public const string PartialViewDisplayChannelName = "Partial View Preview";

        public override string ChannelName => PartialViewDisplayChannelName;

        public override bool IsActive(HttpContext context) => false;
    }
}