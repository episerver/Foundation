using EPiServer.Core.Routing.Pipeline;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce;

namespace SonDo.Infrastructure.Routing;
{
    public class MarketRoutingHelper
    {
        public const string SegmentName = "market";

        public static bool ProcessMarketSegment(
            ICurrentMarket currentMarket,
            IMarketService marketService,
            UrlResolverContext context,
            SegmentPair segmentPair)
        {
            var marketSegment = segmentPair.Next;
            var marketId = new MarketId(marketSegment.ToString());

            var market = marketService.GetMarket(marketId);
            if (market == null) return false;

            context.RouteValues[SegmentName] = marketSegment;
            context.RemainingPath = segmentPair.Remaining;

            return true;
        }

    }
}
