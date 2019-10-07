using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.Web.Mvc;
using Foundation.Cms.Media;
using System.Web.Mvc;

namespace Foundation.Features.Media
{
    [TemplateDescriptor(TemplateTypeCategory = TemplateTypeCategories.MvcPartialController, Inherited = true)]
    public class MediaController : PartialContentController<ImageMediaData>
    {
        public override ActionResult Index(ImageMediaData currentContent)
        {
            return PartialView(currentContent);
        }
    }
}