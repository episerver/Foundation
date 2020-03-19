using EPiServer.Core;
using EPiServer.Web.Mvc;
using Foundation.Cms.Media;
using System.Web.Mvc;

namespace Foundation.Features.Media
{
    public class MediaController : PartialContentController<MediaData>
    {
        public override ActionResult Index(MediaData currentContent)
        {
            if (currentContent is VideoFile)
            {
                return PartialView("~/Features/Media/VideoFile.cshtml", currentContent);
            }
            else if (currentContent is ImageMediaData)
            {
                return PartialView("~/Features/Media/ImageMedia.cshtml", currentContent);
            }
            else if (currentContent is FoundationPdfFile)
            {
                return PartialView("~/Features/Media/PdfFile.cshtml", currentContent as FoundationPdfFile);
            }
            else
            {
                return PartialView("~/Features/Media/Index.cshtml", currentContent.GetType().BaseType.Name);
            }
        }
    }
}