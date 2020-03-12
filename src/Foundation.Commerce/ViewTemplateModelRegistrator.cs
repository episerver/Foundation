using EPiServer.Commerce.Marketing;
using EPiServer.DataAbstraction;
using EPiServer.Framework.Web;
using EPiServer.Web.Mvc;

namespace Foundation.Commerce
{
    public class ViewTemplateModelRegistrator : IViewTemplateModelRegistrator
    {
        public const string FoundationFolder = "~/Features/Shared/Foundation/";

        public void Register(TemplateModelCollection viewTemplateModelRegistrator)
        {
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