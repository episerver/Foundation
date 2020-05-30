using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Foundation.Commerce.Extensions;
using Foundation.Commerce.Markets;
using Foundation.Demo.Models;
using Mediachase.Commerce;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.CatalogContent.Product
{
    [TemplateDescriptor(Inherited = true)]
    public class ProductPartialController : PartialContentController<EntryContentBase>
    {
        private readonly ICurrentMarket _currentMarket;
        private readonly ICurrencyService _currencyService;
        private readonly IContentLoader _contentLoader;

        public ProductPartialController(ICurrentMarket currentMarket, ICurrencyService currencyService, IContentLoader contentLoader)
        {
            _currentMarket = currentMarket;
            _currencyService = currencyService;
            _contentLoader = contentLoader;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public override ActionResult Index(EntryContentBase currentContent)
        {
            var productTileViewModel = currentContent.GetProductTileViewModel(_currentMarket.GetCurrentMarket(), _currencyService.GetCurrentCurrency());
            var startPage = _contentLoader.Get<DemoHomePage>(ContentReference.StartPage);
            if (startPage?.ShowProductRatingsOnListings ?? default(bool))
            {
                var code = currentContent.Code;
                if (currentContent is VariationContent)
                {
                    var parentLink = currentContent.GetParentProducts().FirstOrDefault();
                    if (!ContentReference.IsNullOrEmpty(parentLink))
                    {
                        var product = (currentContent as VariationContent).GetParentProducts().FirstOrDefault();
                        code = _contentLoader.Get<EntryContentBase>(product)?.Code;
                    }
                }
                //productTileViewModel.ReviewStatistics = _reviewService.GetRatings(new[] { code }).FirstOrDefault();
            }
            return PartialView("_Product", productTileViewModel);
        }
    }
}