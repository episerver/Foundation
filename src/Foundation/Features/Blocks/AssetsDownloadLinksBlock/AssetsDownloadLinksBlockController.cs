using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Framework.Blobs;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Commerce.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Blocks.AssetsDownloadLinksBlock
{
    [TemplateDescriptor(Default = true)]
    public class AssetsDownloadLinksBlockController : BlockController<AssetsDownloadLinksBlock>
    {
        private readonly IContentLoader _contentLoader;
        private readonly UrlResolver _urlResolver;

        public AssetsDownloadLinksBlockController(IContentLoader contentLoader, UrlResolver urlResolver)
        {
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
        }

        public override ActionResult Index(AssetsDownloadLinksBlock currentBlock)
        {
            var model = new AssetsDownloadLinksBlockViewModel(currentBlock);
            var rootContent = _contentLoader.Get<IContent>(currentBlock.RootContent);
            if (rootContent != null)
            {
                var assets = new List<MediaData>();
                if (rootContent is ContentFolder)
                {
                    assets = _contentLoader.GetChildren<MediaData>(rootContent.ContentLink).OrderByDescending(x => x.StartPublish).ToList();
                }

                if (rootContent is IAssetContainer assetContainer)
                {
                    assets = assetContainer.GetAssetsMediaData(_contentLoader, currentBlock.GroupName)
                        .OrderByDescending(x => x.StartPublish).ToList();
                }

                if (currentBlock.Count > 0)
                {
                    assets = assets.Take(currentBlock.Count).ToList();
                }

                model.Assets = assets;
            }

            return PartialView("~/Features/Blocks/AssetsDownloadLinksBlock/AssetsDownloadLinksBlock.cshtml", model);
        }

        public void Download(int contentLinkId)
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

                        HttpContext.Response.ContentType = "application/octet-stream";
                        HttpContext.Response.AddHeader("content-disposition", "attachment;filename=" + Path.GetFileName(downloadFileName));
                        HttpContext.Response.TransmitFile(blob.FilePath);
                    }
                }
            }
        }
    }
}
