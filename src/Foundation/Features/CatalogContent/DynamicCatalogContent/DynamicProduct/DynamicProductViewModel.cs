using EPiServer.Core;
using EPiServer.Personalization.Commerce.Tracking;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Foundation.Commerce.Markets;
using Foundation.Features.CatalogContent.DynamicCatalogContent.DynamicVariation;
using Foundation.Features.CatalogContent.Product;
using Foundation.Social.ViewModels;
using Mediachase.Commerce;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.CatalogContent.DynamicCatalogContent.DynamicProduct
{
    public class DynamicProductViewModel : ProductViewModelBase<DynamicProduct, DynamicVariant>, IEntryViewModelBase
    {
        public DynamicProductViewModel()
        {
            GroupVariants = new List<VariantGroupModel>();
        }

        public DynamicProductViewModel(DynamicProduct currentContent) : base(currentContent)
        {
            GroupVariants = new List<VariantGroupModel>();
        }

        public ReviewsViewModel Reviews { get; set; }
        public IEnumerable<Recommendation> AlternativeProducts { get; set; }
        public IEnumerable<Recommendation> CrossSellProducts { get; set; }
        public List<VariantGroupModel> GroupVariants { get; set; }
    }

    public static class DynamicProductViewModelExtensions
    {
        private static readonly Injected<UrlResolver> _url;
        private static readonly Injected<ICurrentMarket> _currentMarket;
        private static readonly Injected<ICurrencyService> _currencyservice;

        public static void GenerateVariantGroup(this DynamicProductViewModel model)
        {
            var variantGroups = model.Variant.VariantOptions?.GroupBy(x => x.GroupName);
            var market = _currentMarket.Service.GetCurrentMarket();
            var currency = _currencyservice.Service.GetCurrentCurrency();

            if (variantGroups != null)
            {
                var groups = new List<VariantGroupModel>();
                foreach (var group in variantGroups)
                {
                    var groupModel = new VariantGroupModel();
                    groupModel.GroupName = group.Key;

                    var subGroups = group.ToList().GroupBy(x => x.SubgroupName);

                    foreach (var g in subGroups)
                    {
                        if (string.IsNullOrEmpty(g.Key))
                        {
                            foreach (var v in g)
                            {
                                var optionModel = new VariantOptionModel();
                                var defaultPriceModel = v.Prices.FirstOrDefault(x => x.Currency == currency.CurrencyCode);
                                var defaultPrice = defaultPriceModel != null ? new Money(defaultPriceModel.Amount, currency) : new Money(0, currency);
                                optionModel.Name = v.Name;
                                optionModel.Code = v.Code;
                                optionModel.ImageUrl = !ContentReference.IsNullOrEmpty(v.Image) ? _url.Service.GetUrl(v.Image) : "";
                                optionModel.DefaultPrice = defaultPrice;
                                optionModel.DiscountedPrice = defaultPrice;

                                groupModel.Variants.Add(optionModel);
                            }
                        }
                        else
                        {
                            var subGroupModel = new VariantGroupModel();
                            subGroupModel.GroupName = g.Key;

                            foreach (var v in g)
                            {
                                var optionModel = new VariantOptionModel();
                                var defaultPriceModel = v.Prices.FirstOrDefault(x => x.Currency == currency.CurrencyCode);
                                var defaultPrice = defaultPriceModel != null ? new Money(defaultPriceModel.Amount, currency) : new Money(0, currency);
                                optionModel.Name = v.Name;
                                optionModel.Code = v.Code;
                                optionModel.ImageUrl = !ContentReference.IsNullOrEmpty(v.Image) ? _url.Service.GetUrl(v.Image) : "";
                                optionModel.DefaultPrice = defaultPrice;
                                optionModel.DiscountedPrice = defaultPrice;

                                subGroupModel.Variants.Add(optionModel);
                            }

                            groupModel.SubGroups.Add(subGroupModel);
                        }
                    }

                    groups.Add(groupModel);
                }
                model.GroupVariants = groups;
            }
        }
    }
    public class VariantGroupModel
    {
        public VariantGroupModel()
        {
            Variants = new List<VariantOptionModel>();
            SubGroups = new List<VariantGroupModel>();
        }

        public string GroupName { get; set; }
        public List<VariantOptionModel> Variants { get; set; }
        public List<VariantGroupModel> SubGroups { get; set; }
    }

    public class VariantOptionModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public Money DefaultPrice { get; set; }
        public Money? DiscountedPrice { get; set; }
    }
}