using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Foundation.Infrastructure.Commerce.Markets;
using Mediachase.Commerce;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Foundation.Features.CatalogContent.Product
{
    [TemplateDescriptor(Inherited = true)]
    public class ProductPartialContentComponent : AsyncPartialContentComponent<EntryContentBase>
    {
        private readonly ICurrentMarket _currentMarket;
        private readonly ICurrencyService _currencyService;

        public ProductPartialContentComponent(ICurrentMarket currentMarket,
            ICurrencyService currencyService)
        {
            _currentMarket = currentMarket;
            _currencyService = currencyService;
        }

        [AcceptVerbs(new string[] { "GET", "POST" })]
        protected override async Task<IViewComponentResult> InvokeComponentAsync(EntryContentBase currentContent)
        {
            var productTileViewModel = currentContent.GetProductTileViewModel(_currentMarket.GetCurrentMarket(), _currencyService.GetCurrentCurrency());
            return await Task.FromResult(View("/Features/Shared/Views/_Product.cshtml", productTileViewModel));
        }
    }
}