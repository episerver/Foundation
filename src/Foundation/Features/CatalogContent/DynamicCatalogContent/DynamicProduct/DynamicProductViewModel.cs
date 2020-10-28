using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Marketing.Testing.Core.DataClass;
using EPiServer.Personalization.Commerce.Tracking;
using EPiServer.ServiceLocation;
using Foundation.Commerce.Markets;
using Foundation.Features.CatalogContent.DynamicCatalogContent.DynamicVariation;
using Foundation.Features.CatalogContent.Product;
using Foundation.Features.CatalogContent.Services;
using Foundation.Features.CatalogContent.Variation;
using Foundation.Social.ViewModels;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Pricing;
using System;
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
        private static readonly Injected<IPromotionService> _promotionService;
        private static readonly Injected<IPriceService> _priceService;
        private static readonly Injected<IContentLoader> _contentLoader;
        private static readonly Injected<ICurrentMarket> _currentMarket;
        private static readonly Injected<ICurrencyService> _currencyservice;

        public static void GenerateVariantGroup(this DynamicProductViewModel model)
        {
            var variantGroups = model.Variant.VariantsOptions.GroupBy(x => x.GroupName);
            var market = _currentMarket.Service.GetCurrentMarket();
            var currency = _currencyservice.Service.GetCurrentCurrency();

            if (variantGroups != null)
            {
                var groups = new List<VariantGroupModel>();
                foreach (var group in variantGroups)
                {
                    var groupModel = new VariantGroupModel();
                    groupModel.GroupName = group.Key;

                    foreach (var v in group)
                    {
                        var variantModel = new VariantOptionModel();
                        var defaultPrice = GetDefaultPrice(model.Variant.Code, market, currency);
                        variantModel.Variant = _contentLoader.Service.Get<VariationContent>(v.Variant) as GenericVariant;
                        variantModel.DefaultPrice = defaultPrice?.UnitPrice ?? new Money(0, currency);
                        variantModel.DiscountedPrice = GetDiscountPrice(defaultPrice, market, currency);

                        groupModel.Variants.Add(variantModel);
                    }

                    groups.Add(groupModel);
                }


                model.GroupVariants = groups;
            }
        }

        private static IPriceValue GetDefaultPrice(string entryCode, IMarket market, Currency currency)
        {
            return _priceService.Service.GetDefaultPrice(
                market.MarketId,
                DateTime.Now,
                new CatalogKey(entryCode),
                currency);
        }

        private static Money? GetDiscountPrice(IPriceValue defaultPrice, IMarket market, Currency currency)
        {
            if (defaultPrice == null)
            {
                return null;
            }

            return _promotionService.Service.GetDiscountPrice(defaultPrice.CatalogKey, market.MarketId, currency).UnitPrice;
        }

    }


    public class VariantGroupModel
    {
        public VariantGroupModel()
        {
            Variants = new List<VariantOptionModel>();
        }
        public string GroupName { get; set; }
        public List<VariantOptionModel> Variants { get; set; }
    }

    public class VariantOptionModel
    {
        public GenericVariant Variant { get; set; }
        public Money DefaultPrice { get; set; }
        public Money? DiscountedPrice { get; set; }
    }
}