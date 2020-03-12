using EPiServer.DataAbstraction;
using EPiServer.Framework.Web;
using EPiServer.Web.Mvc;
using Foundation.Cms.Blocks;
using Foundation.Cms.Pages;
using static Foundation.Cms.Display.FoundationDisplayModeProvider;

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

            viewTemplateModelRegistrator.Add(typeof(FullWidthContainerBlock), new TemplateModel
            {
                Name = "FullWidthContainerBlockNonDisplay",
                Inherit = false,
                AvailableWithoutTag = false,
                Tags  = new[] { DisplayOptionTags.OneSixth,
                    DisplayOptionTags.OneQuarter,
                    DisplayOptionTags.OneThird,
                    DisplayOptionTags.Half,
                    DisplayOptionTags.TwoThird,
                    DisplayOptionTags.ThreeQuarter },
                TemplateTypeCategory = TemplateTypeCategories.MvcPartialView,
                Path = $"{FoundationFolder}DisplayOptionUnavailable.cshtml"
            });
        }
    }
}