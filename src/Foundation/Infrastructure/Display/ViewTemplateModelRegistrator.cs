using EPiServer.Framework.Web;

namespace Foundation.Infrastructure.Display
{
    [ServiceConfiguration(typeof(IViewTemplateModelRegistrator))]
    public class ViewTemplateModelRegistrator : IViewTemplateModelRegistrator
    {
        public static void OnTemplateResolved(object sender, TemplateResolverEventArgs args)
        {

        }

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