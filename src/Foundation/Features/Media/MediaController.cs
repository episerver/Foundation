using EPiServer.Core;
using EPiServer.Editor;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Cms.Media;
using Foundation.Cms.ViewModels.Media;
using System.Web.Mvc;

namespace Foundation.Features.Media
{
    public class MediaController : PartialContentController<MediaData>
    {
        private readonly UrlResolver _urlResolver;

        public MediaController(UrlResolver urlResolver)
        {
            _urlResolver = urlResolver;
        }

        public override ActionResult Index(MediaData currentContent)
        {
            switch (currentContent)
            {
                case VideoFile videoFile:
                    var model = new VideoFileViewModel
                    {
                        DisplayControls = videoFile.DisplayControls,
                        Autoplay = videoFile.Autoplay,
                        Copyright = videoFile.Copyright
                    };

                    if (PageEditing.PageIsInEditMode)
                    {
                        model.VideoLink = _urlResolver.GetUrl(videoFile.ContentLink, null, new VirtualPathArguments { ContextMode = ContextMode.Default });
                        model.PreviewImage = ContentReference.IsNullOrEmpty(videoFile.PreviewImage) ? string.Empty :
                           _urlResolver.GetUrl(videoFile.PreviewImage, null, new VirtualPathArguments { ContextMode = ContextMode.Default });
                    }
                    else
                    {
                        model.VideoLink = _urlResolver.GetUrl(videoFile.ContentLink);
                        model.PreviewImage = ContentReference.IsNullOrEmpty(videoFile.PreviewImage) ? string.Empty : _urlResolver.GetUrl(videoFile.PreviewImage);
                    }
                   
                    return PartialView("~/Features/Media/VideoFile.cshtml", model);
                case ImageMediaData _:
                    return PartialView("~/Features/Media/ImageMedia.cshtml", currentContent);
                case FoundationPdfFile _:
                    return PartialView("~/Features/Media/PdfFile.cshtml", currentContent as FoundationPdfFile);
                default:
                    return PartialView("~/Features/Media/Index.cshtml", currentContent.GetType().BaseType.Name);
            }
        }
    }
}