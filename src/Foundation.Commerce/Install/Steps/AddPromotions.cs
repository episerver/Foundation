using EPiServer;
using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Marketing.Promotions;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using EPiServer.Web;
using Foundation.Cms.Extensions;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Orders.Managers;
using Mediachase.Commerce.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Xml.Linq;

namespace Foundation.Commerce.Install.Steps
{
    public class AddPromotions : BaseInstallStep
    {
        public AddPromotions(IContentRepository contentRepository,
            ReferenceConverter referenceConverter,
            IMarketService marketService) : base(contentRepository, referenceConverter, marketService)
        {
        }

        public override int Order => 7;

        public override string Description => "Adds promotions to Foundation.";

        protected override void ExecuteInternal(IProgressMessenger progressMessenger) => ConfigureMarketing();

        private void ConfigureMarketing()
        {
            //ImportEpiserverData(null);
            using (var stream = new FileStream(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"App_Data\promotions.xml"), FileMode.Open))
            {
                foreach (var xCampaign in GetXElements(stream, "Campaign"))
                {
                    var campaignLink = CreateCampaigns(xCampaign);
                    var xPromotions = xCampaign.Element(XName.Get("Promotions"));
                    foreach (var xPromotion in xPromotions.Elements())
                    {
                        CreatePromotion(campaignLink, xPromotion);
                    }
                }
            }
        }

        private ContentReference CreateCampaigns(XElement xCampaign)
        {
            var campaign = ContentRepository.GetDefault<SalesCampaign>(SalesCampaignFolder.CampaignRoot);
            campaign.Name = xCampaign.Get("Name") ?? "Foundation";
            campaign.Created = DateTime.UtcNow;
            campaign.IsActive = string.IsNullOrEmpty(xCampaign.Get("IsActive")) ? true : bool.Parse(xCampaign.Get("IsActive"));
            campaign.ValidFrom = string.IsNullOrEmpty(xCampaign.Get("StartDate")) ? DateTime.Today : DateTime.Parse(xCampaign.Get("StartDate"));
            campaign.ValidUntil = string.IsNullOrEmpty(xCampaign.Get("EndDate")) ? DateTime.Today.AddYears(1) : DateTime.Parse(xCampaign.Get("EndDate"));
            return ContentRepository.Save(campaign, SaveAction.Publish, AccessLevel.NoAccess);
        }

        private void CreatePromotion(ContentReference campaignLink, XElement xPromotion)
        {
            var promotionProperties = PromotionProperties.Create(xPromotion, ReferenceConverter);
            switch (promotionProperties.Type)
            {
                case "BuyQuantityGetItemDiscount":
                    CreateBuyFromMenShoesGetDiscountPromotion(campaignLink, promotionProperties);
                    break;
                case "SpendAmountGetOrderDiscount":
                    CreateSpendAmountGetDiscountPromotion(campaignLink, promotionProperties);
                    break;
                case "BuyQuantityGetShippingDiscount":
                    CreateBuyFromWomenShoesGetShippingDiscountPromotion(campaignLink, promotionProperties);
                    break;
                default:
                    break;
            }
        }

        private void CreateBuyFromMenShoesGetDiscountPromotion(ContentReference campaignLink, PromotionProperties promotionProperties)
        {
            var promotion = ContentRepository.GetDefault<BuyQuantityGetItemDiscount>(campaignLink);
            promotion.IsActive = promotionProperties.IsActive;
            promotion.Name = promotionProperties.Name ?? "20 % off Mens Shoes";
            promotion.Condition.Items = promotionProperties.ConditionContentLinks;
            promotion.Condition.RequiredQuantity = promotionProperties.ConditionRequiredQuantity;
            promotion.DiscountTarget.Items = promotionProperties.TargetContentLinks;
            promotion.Discount.UseAmounts = promotionProperties.IsDiscountUseAmount;
            promotion.Discount.Percentage = promotionProperties.DiscountPercentage;
            promotion.Banner = GetAssetUrl(promotionProperties.Banner);
            ContentRepository.Save(promotion, SaveAction.Publish, AccessLevel.NoAccess);
        }

        private void CreateSpendAmountGetDiscountPromotion(ContentReference campaignLink, PromotionProperties promotionProperties)
        {
            var promotion = ContentRepository.GetDefault<SpendAmountGetOrderDiscount>(campaignLink);
            promotion.IsActive = promotionProperties.IsActive;
            promotion.Name = promotionProperties.Name ?? "$50 off Order over $500";
            promotion.Condition.Amounts = promotionProperties.ConditionAmounts;
            promotion.Discount.UseAmounts = promotionProperties.IsDiscountUseAmount;
            promotion.Discount.Amounts = promotionProperties.DiscountAmounts;
            promotion.Banner = GetAssetUrl(promotionProperties.Banner);
            ContentRepository.Save(promotion, SaveAction.Publish, AccessLevel.NoAccess);
        }

        private void CreateBuyFromWomenShoesGetShippingDiscountPromotion(ContentReference campaignLink, PromotionProperties promotionProperties)
        {
            var promotion = ContentRepository.GetDefault<BuyQuantityGetShippingDiscount>(campaignLink);
            promotion.IsActive = promotionProperties.IsActive;
            promotion.Name = promotionProperties.Name ?? "$10 off shipping from Women's Shoes";
            promotion.Condition.Items = promotionProperties.ConditionContentLinks;
            promotion.ShippingMethods = GetShippingMethodIds();
            promotion.Condition.RequiredQuantity = promotionProperties.ConditionRequiredQuantity;
            promotion.Discount.UseAmounts = promotionProperties.IsDiscountUseAmount;
            promotion.Discount.Amounts = promotionProperties.DiscountAmounts;
            promotion.Banner = GetAssetUrl(promotionProperties.Banner);
            ContentRepository.Save(promotion, SaveAction.Publish, AccessLevel.NoAccess);
        }

        private IList<Guid> GetShippingMethodIds()
        {
            var shippingMethods = new List<Guid>();
            var enabledMarkets = MarketService.GetAllMarkets().Where(x => x.IsEnabled).ToList();
            foreach (var language in enabledMarkets.SelectMany(x => x.Languages).Distinct())
            {
                var languageId = language.TwoLetterISOLanguageName;
                var dto = ShippingManager.GetShippingMethods(languageId);
                foreach (var shippingMethodRow in dto.ShippingMethod)
                {
                    shippingMethods.Add(shippingMethodRow.ShippingMethodId);
                }
            }

            return shippingMethods;
        }

        private ContentReference GetAssetUrl(string assetPath)
        {
            if (assetPath == null)
            {
                return null;
            }

            var slugs = assetPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var pathDepth = slugs.Length;
            if (pathDepth < 1)
            {
                return null;
            }

            var currentFolder = SiteDefinition.Current.SiteAssetsRoot;
            foreach (var folderName in slugs.Take(pathDepth - 1))
            {
                currentFolder = GetChildContentByName<ContentFolder>(currentFolder, folderName);
                if (currentFolder == null)
                {
                    return null;
                }
            }

            return GetChildContentByName<MediaData>(currentFolder, slugs.Last());
        }

        private ContentReference GetChildContentByName<T>(ContentReference contentLink, string name) where T : IContent
        {
            var match = ContentRepository.GetChildren<T>(contentLink)
                .FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return match?.ContentLink;
        }
    }

    public class PromotionProperties
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Banner { get; set; }
        public bool IsActive { get; set; }
        public int ConditionRequiredQuantity { get; set; }
        public List<Money> ConditionAmounts { get; set; }
        public bool IsDiscountUseAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public List<Money> DiscountAmounts { get; set; }
        public List<ContentReference> ConditionContentLinks { get; set; }
        public List<ContentReference> TargetContentLinks { get; set; }

        public PromotionProperties()
        {
            ConditionAmounts = new List<Money>();
            DiscountAmounts = new List<Money>();
            ConditionContentLinks = new List<ContentReference>();
            TargetContentLinks = new List<ContentReference>();
        }

        private static List<ContentReference> GetContentLinks(XElement xElement, string parentNameElement, string childNameElement, ReferenceConverter referenceConverter)
        {
            var xCondition = xElement.Element(XName.Get(parentNameElement));
            var xConditionCodes = xCondition.Element(XName.Get(childNameElement));
            var conditionContentLinks = new List<ContentReference>();
            if (xConditionCodes.Elements() == null)
            {
                conditionContentLinks.Add(referenceConverter.GetContentLink("shoes", CatalogContentType.CatalogNode));
            }
            else
            {
                foreach (var xCode in xConditionCodes.Elements())
                {
                    conditionContentLinks.Add(referenceConverter.GetContentLink(xCode.Value));
                }
            }
            return conditionContentLinks;
        }

        public static PromotionProperties Create(XElement xPromotion, ReferenceConverter referenceConverter)
        {
            var promotion = new PromotionProperties();

            // Conditions
            var conditionContentLinks = GetContentLinks(xPromotion, "Condition", "Codes", referenceConverter);
            var xCondition = xPromotion.Element(XName.Get("Condition"));
            promotion.ConditionContentLinks = conditionContentLinks;

            var xConditionAmount = xCondition.Element(XName.Get("Amount"));
            var conditionValue = string.IsNullOrEmpty(xConditionAmount.Get("Value")) ? "0" : xConditionAmount.Get("Value");
            var conditionCurrency = string.IsNullOrEmpty(xConditionAmount.Get("Currency")) ? "USD" : xConditionAmount.Get("Currency");
            var conditionAmount = new Money(decimal.Parse(conditionValue ?? "0"), new Currency(conditionCurrency));
            var conditionRequiredQuantity = string.IsNullOrEmpty(xCondition.Get("RequiredQuanity")) ? 0 : int.Parse(xCondition.Get("RequiredQuanity") ?? "0");
            promotion.ConditionAmounts = new List<Money> { conditionAmount };
            promotion.ConditionRequiredQuantity = conditionRequiredQuantity;

            // Targets
            var targetContentLinks = GetContentLinks(xPromotion, "Target", "Codes", referenceConverter);
            promotion.TargetContentLinks = targetContentLinks;

            // Discount
            var xDiscount = xPromotion.Element(XName.Get("Discount"));
            var xDiscountAmount = xDiscount.Element(XName.Get("Amount"));
            var discountValue = string.IsNullOrEmpty(xDiscountAmount.Get("Value")) ? "0" : xDiscountAmount.Get("Value");
            var discountCurrency = string.IsNullOrEmpty(xDiscountAmount.Get("Currency")) ? "USD" : xDiscountAmount.Get("Currency");
            var discountAmount = new Money(decimal.Parse(discountValue ?? "0"), new Currency(discountCurrency));
            var discountUseAmount = bool.Parse(string.IsNullOrEmpty(xDiscount.Get("UseAmounts")) ? "False" : xDiscount.Get("UseAmounts"));
            var discountPercentage = decimal.Parse(string.IsNullOrEmpty(xDiscount.Get("Percentage")) ? "0" : xDiscount.Get("Percentage"));
            promotion.DiscountAmounts = new List<Money> { discountAmount };
            promotion.IsDiscountUseAmount = discountUseAmount;
            promotion.DiscountPercentage = discountPercentage;

            // Common promotio attributes
            var bannerPath = xPromotion.Get("Banner");
            var description = xPromotion.Get("Description");
            var isActive = bool.Parse(string.IsNullOrEmpty(xPromotion.Get("IsActive")) ? "False" : xPromotion.Get("IsActive"));
            var type = xPromotion.Get("Type");
            var name = xPromotion.Get("Name");

            promotion.Name = name;
            promotion.Description = description;
            promotion.IsActive = isActive;
            promotion.Type = type;
            promotion.Banner = bannerPath;

            return promotion;
        }
    }
}
