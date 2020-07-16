using EPiServer.Commerce.Marketing;
using EPiServer.DataAbstraction;
using EPiServer.Framework.Web;
using EPiServer.Web.Mvc;
using Foundation.Features.Shared;

namespace Foundation.Infrastructure.Display
{
    public class ViewTemplateModelRegistrator : IViewTemplateModelRegistrator
    {
        public const string FoundationFolder = "~/Features/Shared/Views/";

        public void Register(TemplateModelCollection viewTemplateModelRegistrator)
        {
            viewTemplateModelRegistrator.Add(typeof(FoundationPageData), new TemplateModel
            {
                Name = "PartialPage",
                Inherit = true,
                AvailableWithoutTag = true,
                TemplateTypeCategory = TemplateTypeCategories.MvcPartialView,
                Path = $"{FoundationFolder}_Page.cshtml"
            });

            viewTemplateModelRegistrator.Add(typeof(PromotionData), new TemplateModel
            {
                Name = "PartialPromotion",
                Inherit = true,
                AvailableWithoutTag = true,
                TemplateTypeCategory = TemplateTypeCategories.MvcPartialView,
                Path = $"{FoundationFolder}_Promotion.cshtml"
            });
        }
    }
}