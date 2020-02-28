using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Web.Mvc;
using Foundation.Commerce.Models.Catalog;
using Foundation.Find.Commerce;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Features.Pages.ComparisonPage
{
    public class ComparisonPageController : PageController<Commerce.Models.Pages.ComparisonPage>
    {
        private CommerceSearchService _commerceSearchService;
        private ReferenceConverter _referenceConverter;
        private IContentLoader _contentLoader;
        private ICurrentMarket _currentMarket;

        public ComparisonPageController(CommerceSearchService commerceSearchService,
            ReferenceConverter referenceConverter,
            IContentLoader contentLoader,
            ICurrentMarket currentMarket)
        {
            _commerceSearchService = commerceSearchService;
            _referenceConverter = referenceConverter;
            _contentLoader = contentLoader;
            _currentMarket = currentMarket;
        }

        public ActionResult Index(Commerce.Models.Pages.ComparisonPage currentPage, string skuCode1, string skuCode2)
        {
            if (string.IsNullOrEmpty(skuCode1))
            {
                return View("~/Features/Pages/ComparisonPage/Empty.cshtml", new ComparisonPageViewModel(currentPage));
            }

            var variantLink = _referenceConverter.GetContentLink(skuCode1);
            var variant = _contentLoader.Get<EntryContentBase>(variantLink) as GenericVariant;
            var market = _currentMarket.GetCurrentMarket();

            if (string.IsNullOrEmpty(skuCode2))
            {
                var model = new ComparisonPageViewModel(currentPage);
                model.Variant1 = new VariationCompareModel(variant, market);
                model.Variants = _commerceSearchService.GetTheSameVariants(skuCode1);
                ViewBag.Currency = market.DefaultCurrency;
                return View("~/Features/Pages/ComparisonPage/CompareList.cshtml", model);
            }
            else
            {
                var variantLink2 = _referenceConverter.GetContentLink(skuCode2);
                var variant2 = _contentLoader.Get<EntryContentBase>(variantLink2) as GenericVariant;
                var model = new ComparisonPageViewModel(currentPage, variant, variant2, market);
                if (model.CurrentContent.ComparisonProperties != null && model.CurrentContent.ComparisonProperties.Count > 0)
                {
                    var comparisonCollection = new List<ComparisonItem>();
                    foreach(var p in model.CurrentContent.ComparisonProperties)
                    {
                        var item = new ComparisonItem();
                        item.PropertyDisplayName = p.DisplayName;
                        item.ValueVariant1 = variant.Property[p.PropertyName];
                        item.ValueVariant2 = variant2.Property[p.PropertyName];
                        comparisonCollection.Add(item);
                    }
                    model.ComparisonCollection = comparisonCollection;
                }

                return View("~/Features/Pages/ComparisonPage/Index.cshtml", model);
            }
        }
    }
}