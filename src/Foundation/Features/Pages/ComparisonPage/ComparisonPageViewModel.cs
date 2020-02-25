using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using Foundation.Cms.ViewModels;
using Foundation.Commerce.Catalog;
using Foundation.Commerce.Extensions;
using Foundation.Commerce.Models.Catalog;
using Foundation.Find.Commerce.ViewModels;
using Foundation.Social.ViewModels;
using Mediachase.Commerce;
using System.Collections.Generic;

namespace Foundation.Features.Pages.ComparisonPage
{
    public class ComparisonPageViewModel : ContentViewModel<Commerce.Models.Pages.ComparisonPage>
    {
        public IEnumerable<VariationModel> Variants { get; set; }
        public VariationCompareModel Variant1 { get; set; }
        public VariationCompareModel Variant2 { get; set; }
        public ComparisonPageViewModel(Commerce.Models.Pages.ComparisonPage currentPage) : base(currentPage) => Variants = new List<VariationModel>();
        public ComparisonPageViewModel(Commerce.Models.Pages.ComparisonPage currentPage, GenericVariant variant1, GenericVariant variant2, IMarket market) : this(currentPage)
        {
            Variant1 = new VariationCompareModel(variant1, market);
            Variant2 = new VariationCompareModel(variant2, market);
        }
    }

    public class VariationCompareModel
    {
        public GenericVariant Variant { get; set; }
        public ReviewsViewModel Review { get; set; }
        public Money Price { get; set; }
        public string ImageUrl { get; set; }

        public VariationCompareModel(GenericVariant variant, IMarket market)
        {
            Variant = variant;
            ImageUrl = (variant as IAssetContainer).GetDefaultAsset<MediaData>();
            Price = PriceCalculationService.GetSalePrice(variant.Code, market.MarketId, market.DefaultCurrency).UnitPrice;
        }
    }
}
