using EPiServer.Commerce.Catalog.ContentTypes;
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
    public class CatalogPartialPreviewController : ContentController<CatalogContentBase>
    {
        private readonly PreviewControllerHelper _previewControllerHelper;

        public CatalogPartialPreviewController(PreviewControllerHelper previewControllerHelper)
        {
            _previewControllerHelper = previewControllerHelper;
        }

        public ActionResult Index(IContent currentContent) => _previewControllerHelper.RenderResult(currentContent);
    }
}