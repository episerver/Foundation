using EPiServer.Framework.Blobs;
using System.IO;

namespace Foundation.Features.Blocks.AssetsDownloadLinksBlock
{
    [ApiController]
    [Route("[controller]")]
    public class AssetsDownloadLinksBlockController : ControllerBase
    {
        private readonly IContentLoader _contentLoader;

        public AssetsDownloadLinksBlockController(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        [HttpGet("Download/{contentLinkId}")]
        public IActionResult Download(int contentLinkId)
        {
            var mediaData = _contentLoader.Get<IContent>(new ContentReference(contentLinkId)) as MediaData;
            if (mediaData != null)
            {
                var downloadFile = mediaData;
                if (downloadFile != null)
                {
                    var blob = downloadFile.BinaryData as FileBlob;
                    if (blob != null)
                    {
                        var routeSegment = downloadFile.RouteSegment;
                        var extension = Path.GetExtension(blob.FilePath) ?? "";
                        var downloadFileName = routeSegment.EndsWith(extension) ? routeSegment : routeSegment + extension;

                        HttpContext.Response.Headers.Add("content-disposition", "attachment;filename=" + Path.GetFileName(downloadFileName));
                        return File(System.IO.File.ReadAllBytes(blob.FilePath), "application/octet-stream");
                    }
                }
            }
            return null;
        }
    }
}
