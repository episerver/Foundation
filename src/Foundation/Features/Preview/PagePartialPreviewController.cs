using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web.Mvc;
using EPiServer.Web.Mvc;
using System.Web.Mvc;

namespace Foundation.Features.Preview
{
    [TemplateDescriptor(
        Inherited = true,
        Tags = new[] { PartialViewDisplayChannel.PartialViewDisplayChannelName },
        AvailableWithoutTag = false)]
    [VisitorGroupImpersonation]
    [RequireClientResources]
    public class PagePartialPreviewController : PageController<PageData>
    {
        private readonly PreviewControllerHelper _previewControllerHelper;

        public PagePartialPreviewController(PreviewControllerHelper previewControllerHelper)
        {
            _previewControllerHelper = previewControllerHelper;
        }

        public ActionResult Index(IContent currentContent) => _previewControllerHelper.RenderResult(currentContent);
    }
}