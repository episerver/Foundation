using EPiServer.Core;
using System.Collections.Generic;

namespace Foundation.Features.Header.Market
{
    public class MarketViewModel
    {
        public IEnumerable<MarketItem> Markets { get; set; }
        public string MarketId { get; set; }
        public MarketItem CurrentMarket { get; set; }
        public ContentReference ContentLink { get; set; }
    }

    public class MarketItem
    {
        public string FlagUrl { get; set; }
        public bool Selected { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
    }
}