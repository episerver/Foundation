using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Foundation.Commerce.Markets;
using Mediachase.Commerce;
using System.Web.Mvc;

namespace Foundation.Features.CatalogContent.Product
{
    [TemplateDescriptor(Inherited = true)]
    public class ProductPartialController : PartialContentController<EntryContentBase>
    {
        private readonly ICurrentMarket _currentMarket;
        private readonly ICurrencyService _currencyService;

        public ProductPartialController(ICurrentMarket currentMarket,
            ICurrencyService currencyService)
        {
            _currentMarket = currentMarket;
            _currencyService = currencyService;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public override ActionResult Index(EntryContentBase currentContent)
        {
            var productTileViewModel = currentContent.GetProductTileViewModel(_currentMarket.GetCurrentMarket(), _currencyService.GetCurrentCurrency());
            return PartialView("_Product", productTileViewModel);
        }
    }
}