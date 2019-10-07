using System;

namespace Foundation.Commerce.Markets
{
    public static class MarketEvent
    {
        public static event EventHandler ChangeMarket;
        public static void OnChangeMarket(object o, EventArgs e) => ChangeMarket?.Invoke(o, e);
    }
}
