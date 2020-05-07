using EPiServer.Core;
using EPiServer.Editor;
using EPiServer.Events.ChangeNotification.Implementation;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.Framework.Web.Mvc;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Cms.Media;
using Foundation.Cms.ViewModels.Media;
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
    public class VideoFilePreviewController : ActionControllerBase, IRenderTemplate<VideoFile>
    {
        private readonly UrlResolver _urlResolver;

        public VideoFilePreviewController(UrlResolver urlResolver)
        {
            _urlResolver = urlResolver;
        }

        public ActionResult Index(IContent currentContent)
        {
            var videoFile = currentContent as VideoFile;
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
        }
    }
}