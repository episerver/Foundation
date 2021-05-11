using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.Framework.Web.Mvc;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using Foundation.Features.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq;

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
        private readonly IContentLoader _contentLoader;
        private readonly TemplateResolver _templateResolver;
        private readonly DisplayOptions _displayOptions;

        public PagePartialPreviewController(IContentLoader contentLoader, TemplateResolver templateResolver, DisplayOptions displayOptions)
        {
            _contentLoader = contentLoader;
            _templateResolver = templateResolver;
            _displayOptions = displayOptions;
        }

        public ActionResult Index(IContent currentContent)
        {
            //As the layout requires a page for title etc we "borrow" the start page
            var startPage = _contentLoader.Get<HomePage>(ContentReference.StartPage);

            var model = new PreviewModel(startPage, currentContent);

            var supportedDisplayOptions = _displayOptions
                .Select(x => new
                {
                    x.Tag,
                    x.Name,
                    Supported = SupportsTag(currentContent, x.Tag)
                }).ToList();

            if (!supportedDisplayOptions.Any(x => x.Supported))
            {
                return new ViewResult
                {
                    ViewName = "~/Features/Preview/Index.cshtml",
                    ViewData = { Model = model }
                };
            }
            foreach (var displayOption in supportedDisplayOptions)
            {
                var contentArea = new ContentArea();
                contentArea.Items.Add(new ContentAreaItem
                {
                    ContentLink = currentContent.ContentLink
                });
                var areaModel = new PreviewModel.PreviewArea
                {
                    Supported = displayOption.Supported,
                    AreaTag = displayOption.Tag,
                    AreaName = displayOption.Name,
                    ContentArea = contentArea
                };

                model.Areas.Add(areaModel);
            }

            return new ViewResult
            {
                ViewName = "~/Features/Preview/Index.cshtml",
                ViewData = new ViewDataDictionary<PreviewModel>(ViewData, model)
            };
        }

        private bool SupportsTag(IContent content, string tag)
        {
            var templateModel = _templateResolver.Resolve(HttpContext,
                content.GetOriginalType(),
                content,
                TemplateTypeCategories.MvcPartial,
                tag);

            return templateModel != null;
        }
    }
}