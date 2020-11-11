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
            var variantGroups = model.Variant.VariantOptions.GroupBy(x => x.ParentGroupName);
            var market = _currentMarket.Service.GetCurrentMarket();
            var currency = _currencyservice.Service.GetCurrentCurrency();

            if (variantGroups != null)
            {
                var groups = new List<VariantGroupModel>();

                foreach (var group in variantGroups)
                {
                    var groupModel = new VariantGroupModel();

                    if (string.IsNullOrEmpty(group.Key))
                    {
                        var groupByGroups = group.ToList().GroupBy(x => x.GroupName);
                        foreach (var g in groupByGroups)
                        {
                            groupModel.GroupName = g.Key;

                            foreach (var v in g)
                            {
                                var variantModel = new VariantOptionModel();
                                var defaultPriceModel = v.Prices.FirstOrDefault(x => x.Currency == currency.CurrencyCode);
                                var defaultPrice = defaultPriceModel != null ? new Money(defaultPriceModel.Amount, currency) : new Money(0, currency);
                                variantModel.Name = v.Name;
                                variantModel.Code = v.Code;
                                variantModel.ImageUrl = !ContentReference.IsNullOrEmpty(v.Image) ? _url.Service.GetUrl(v.Image) : "";
                                variantModel.DefaultPrice = defaultPrice;
                                variantModel.DiscountedPrice = defaultPrice;

                                groupModel.Variants.Add(variantModel);
                            }

                            groups.Add(groupModel);
                        }
                    }
                    else
                    {
                        groupModel.GroupName = group.Key;

                        var groupByGroups = group.ToList().GroupBy(x => x.GroupName);
                        foreach (var g in groupByGroups)
                        {
                            var subGroupModel = new VariantGroupModel();
                            subGroupModel.GroupName = g.Key;

                            foreach (var v in g)
                            {
                                var variantModel = new VariantOptionModel();
                                var defaultPriceModel = v.Prices.FirstOrDefault(x => x.Currency == currency.CurrencyCode);
                                var defaultPrice = defaultPriceModel != null ? new Money(defaultPriceModel.Amount, currency) : new Money(0, currency);
                                variantModel.Name = v.Name;
                                variantModel.Code = v.Code;
                                variantModel.ImageUrl = !ContentReference.IsNullOrEmpty(v.Image) ? _url.Service.GetUrl(v.Image) : "";
                                variantModel.DefaultPrice = defaultPrice;
                                variantModel.DiscountedPrice = defaultPrice;

                                subGroupModel.Variants.Add(variantModel);
                            }

                            groupModel.SubGroups.Add(subGroupModel);
                        }

                        groups.Add(groupModel);
                    }
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