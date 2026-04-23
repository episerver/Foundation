using Mediachase.Commerce.Markets;

namespace Foundation.Infrastructure.Commerce.Models.EditorDescriptors
{
    public class CurrencySelectionFactory : ISelectionFactory
    {
        private readonly Injected<IMarketService> _marketService;

        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var markets = _marketService.Service.GetAllMarkets();
            var currencies = new List<string>();

            if (markets != null && markets.Any())
            {
                foreach (var market in markets)
                {
                    foreach (var currency in market.Currencies)
                    {
                        currencies.Add(currency.CurrencyCode);
                    }
                }
            }

            var items = currencies.Distinct().Select(x => new SelectItem { Text = x, Value = x }).OrderBy(i => i.Text).ToList();
            return items;
        }
    }
}