using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using System.IO;

namespace Foundation.Features.Media
{
    [TemplateDescriptor(TemplateTypeCategory = TemplateTypeCategories.MvcPartialComponent, Inherited = true)]
    public class MediaController : AsyncPartialContentComponent<MediaData>
    {
        private readonly UrlResolver _urlResolver;
        private readonly IContextModeResolver _contextModeResolver;

        public MediaController(UrlResolver urlResolver, IContextModeResolver contextModeResolver)
        {
            _urlResolver = urlResolver;
            _contextModeResolver = contextModeResolver;
        }

        protected override async Task<IViewComponentResult> InvokeComponentAsync(MediaData currentContent)
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

                    if (_contextModeResolver.CurrentMode == ContextMode.Edit)
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
                    return await Task.FromResult(View("~/Features/Media/VideoFile.cshtml", videoViewModel));
                case ImageMediaData image:
                    var imageViewModel = new ImageMediaDataViewModel
                    {
                        Name = image.Name,
                        Description = image.Description,
                        ImageAlignment = image.ImageAlignment,
                        PaddingStyles = image.PaddingStyles,
                        AltText = image.AltText,
                        Title = image.Title
                    };

                    if (_contextModeResolver.CurrentMode == ContextMode.Edit)
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

                    return await Task.FromResult(View("~/Features/Media/ImageMedia.cshtml", imageViewModel));
                case FoundationPdfFile pdfFile:
                    var pdfViewModel = new FoundationPdfFileViewModel
                    {
                        Name = pdfFile.Name,
                        Title = pdfFile.Title,
                        Description = pdfFile.Description,
                        Height = pdfFile.Height,
                        DisplayAsPreview = pdfFile.DisplayAsPreview,
                        ShowDescription = pdfFile.ShowDescription,
                        ShowIcon = pdfFile.ShowIcon
                    };

                    if (_contextModeResolver.CurrentMode == ContextMode.Edit)
                    {
                        pdfViewModel.PdfLink = _urlResolver.GetUrl(pdfFile.ContentLink, null, new VirtualPathArguments { ContextMode = ContextMode.Default });
                    }
                    else
                    {
                        pdfViewModel.PdfLink = _urlResolver.GetUrl(pdfFile.ContentLink);
                    }

                    return await Task.FromResult(View("~/Features/Media/PdfFile.cshtml", pdfViewModel));
                case StandardFile standardFile:
                    var standardFileViewModel = new StandardFileViewModel
                    {
                        Name = standardFile.Name,
                        Title = standardFile.Title,
                        Description = standardFile.Description,
                        ShowDescription = standardFile.ShowDescription,
                        ShowIcon = standardFile.ShowIcon,
                        FileExtension = Path.GetExtension(standardFile.Name)
                    };

                    if (_contextModeResolver.CurrentMode == ContextMode.Edit)
                    {
                        standardFileViewModel.FileLink = _urlResolver.GetUrl(standardFile.ContentLink, null, new VirtualPathArguments { ContextMode = ContextMode.Default });
                    }
                    else
                    {
                        standardFileViewModel.FileLink = _urlResolver.GetUrl(standardFile.ContentLink);
                    }

                    return await Task.FromResult(View("~/Features/Media/StandardFile.cshtml", standardFileViewModel));
                default:
                    return await Task.FromResult(View("~/Features/Media/Index.cshtml", currentContent.GetType().BaseType.Name));
            }
        }
    }
}