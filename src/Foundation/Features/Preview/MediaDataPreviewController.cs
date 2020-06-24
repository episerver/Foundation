using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.Framework.Web.Mvc;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Features.Media;
using System.Web.Mvc;

namespace Foundation.Features.Preview
{
    [TemplateDescriptor(
        Default = true,
        Inherited = true,
        TemplateTypeCategory = TemplateTypeCategories.MvcController,
        Tags = new[] { RenderingTags.Preview, RenderingTags.Edit },
        AvailableWithoutTag = false)]
    [RequireClientResources]
    public class MediaDataPreviewController : ActionControllerBase, IRenderTemplate<MediaData>
    {
        private readonly UrlResolver _urlResolver;

        public MediaDataPreviewController(UrlResolver urlResolver)
        {
            _urlResolver = urlResolver;
        }

        public ActionResult Index(IContent currentContent)
        {
            switch (currentContent)
            {
                case VideoFile videoFile:
                    var model = new VideoFileViewModel
                    {
                        VideoLink = _urlResolver.GetUrl(currentContent.ContentLink, null, new VirtualPathArguments { ContextMode = ContextMode.Default }),
                        PreviewImage = ContentReference.IsNullOrEmpty(videoFile.PreviewImage) ? string.Empty :
                            _urlResolver.GetUrl(videoFile.PreviewImage, null, new VirtualPathArguments { ContextMode = ContextMode.Default }),
                        DisplayControls = videoFile.DisplayControls,
                        Autoplay = videoFile.Autoplay,
                        Copyright = videoFile.Copyright
                    };
                    return PartialView("~/Features/Preview/VideoFilePreview.cshtml", model);
                case ImageMediaData image:
                    var imageViewModel = new ImageMediaDataViewModel
                    {
                        Name = image.Name,
                        ImageLink = _urlResolver.GetUrl(image.ContentLink, null, new VirtualPathArguments { ContextMode = ContextMode.Default }),
                        LinkToContent = ContentReference.IsNullOrEmpty(image.Link) ? string.Empty :
                           _urlResolver.GetUrl(image.Link, null, new VirtualPathArguments { ContextMode = ContextMode.Default }),
                        Description = image.Description,
                        ImageAlignment = image.ImageAlignment,
                        PaddingStyles = image.PaddingStyles
                    };
                    return PartialView("~/Features/Preview/ImageMediaPreview.cshtml", imageViewModel);
                case FoundationPdfFile pdfFile:
                    var pdfViewModel = new FoundationPdfFileViewModel
                    {
                        Height = pdfFile.Height
                    };
                    pdfViewModel.PdfLink = _urlResolver.GetUrl(pdfFile.ContentLink, null, new VirtualPathArguments { ContextMode = ContextMode.Default });
                    return PartialView("~/Features/Preview/PdfFilePreview.cshtml", pdfViewModel);
                default:
                    return PartialView("~/Features/Preview/NoPreview.cshtml", currentContent.GetType().BaseType.Name);
            }
        }
    }
}