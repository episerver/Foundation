using EPiServer.DataAbstraction;
using EPiServer.Framework.Web;
using EPiServer.Web.Mvc;
using Foundation.Cms.Pages;

namespace Foundation.Cms.Display
{
    public class ViewTemplateModelRegistrator : IViewTemplateModelRegistrator
    {
        public const string FoundationFolder = "~/Features/Shared/Foundation/";

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
        }
    }
}