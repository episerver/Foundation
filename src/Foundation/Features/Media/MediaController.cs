using EPiServer.Core;
using EPiServer.Editor;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
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
                    var videoViewModel = new VideoFileViewModel
                    {
                        DisplayControls = videoFile.DisplayControls,
                        Autoplay = videoFile.Autoplay,
                        Copyright = videoFile.Copyright
                    };

                    if (PageEditing.PageIsInEditMode)
                    {
                        videoViewModel.VideoLink = _urlResolver.GetUrl(videoFile.ContentLink, null, new VirtualPathArguments { ContextMode = ContextMode.Default });
                        videoViewModel.PreviewImage = ContentReference.IsNullOrEmpty(videoFile.PreviewImage) ? string.Empty :
                           _urlResolver.GetUrl(videoFile.PreviewImage, null, new VirtualPathArguments { ContextMode = ContextMode.Default });
                    }
                    else
                    {
                        videoViewModel.VideoLink = _urlResolver.GetUrl(videoFile.ContentLink);
                        videoViewModel.PreviewImage = ContentReference.IsNullOrEmpty(videoFile.PreviewImage) ? string.Empty : _urlResolver.GetUrl(videoFile.PreviewImage);
                    }
                    return PartialView("~/Features/Media/VideoFile.cshtml", videoViewModel);
                case ImageMediaData image:
                    var imageViewModel = new ImageMediaDataViewModel
                    {
                        Name = image.Name,
                        Description = image.Description,
                        ImageAlignment = image.ImageAlignment,
                        PaddingStyles = image.PaddingStyles
                    };

                    if (PageEditing.PageIsInEditMode)
                    {
                        imageViewModel.ImageLink = _urlResolver.GetUrl(image.ContentLink, null, new VirtualPathArguments { ContextMode = ContextMode.Default });
                        imageViewModel.LinkToContent = ContentReference.IsNullOrEmpty(image.Link) ? string.Empty :
                           _urlResolver.GetUrl(image.Link, null, new VirtualPathArguments { ContextMode = ContextMode.Default });
                    }
                    else
                    {
                        imageViewModel.ImageLink = _urlResolver.GetUrl(image.ContentLink);
                        imageViewModel.LinkToContent = ContentReference.IsNullOrEmpty(image.Link) ? string.Empty : _urlResolver.GetUrl(image.Link);
                    }

                    return PartialView("~/Features/Media/ImageMedia.cshtml", imageViewModel);
                case FoundationPdfFile pdfFile:
                    var pdfViewModel = new FoundationPdfFileViewModel
                    {
                        Height = pdfFile.Height
                    };

                    if (PageEditing.PageIsInEditMode)
                    {
                        pdfViewModel.PdfLink = _urlResolver.GetUrl(pdfFile.ContentLink, null, new VirtualPathArguments { ContextMode = ContextMode.Default });
                    }
                    else
                    {
                        pdfViewModel.PdfLink = _urlResolver.GetUrl(pdfFile.ContentLink);
                    }

                    return PartialView("~/Features/Media/PdfFile.cshtml", pdfViewModel);
                default:
                    return PartialView("~/Features/Media/Index.cshtml", currentContent.GetType().BaseType.Name);
            }
        }
    }
}