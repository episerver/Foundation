// CMS 13: IBlobFactory removed from EPiServer.Framework. Using MemoryStream for catalog export.
using EPiServer.Logging;
using Mediachase.Commerce.Catalog.ImportExport;
using System.IO;
using System.IO.Compression;

namespace Foundation.Features.Api
{

    [ApiController]
    [Route("[controller]")]
    public class CatalogExportController : ControllerBase
    {
        private readonly CatalogImportExport _importExport;
        private readonly IContentLoader _contentLoader;
        private readonly ReferenceConverter _referenceConverter;
        internal const string DownloadRoute = "episerverapi/catalogs/";

        private static readonly ILogger _logger = LogManager.GetLogger(typeof(CatalogExportController));

        public CatalogExportController(CatalogImportExport importExport,
            IContentLoader contentLoader,
            ReferenceConverter referenceConverter)
        {
            _importExport = importExport;
            _contentLoader = contentLoader;
            _referenceConverter = referenceConverter;
            _importExport.IsModelsAvailable = true;
        }

        // GET: CatalogExportController
        [HttpGet]
        [Authorize(Roles = "CommerceAdmins")]
        [Route(DownloadRoute)]
        public ActionResult Index(string catalogName)
        {
            var catalogs = _contentLoader.GetChildren<EPiServer.Commerce.Catalog.ContentTypes.CatalogContent>(_referenceConverter.GetRootLink());
            var catalog = catalogs.FirstOrDefault(x => x.Name.Equals(catalogName, StringComparison.OrdinalIgnoreCase));
            if (catalog != null)
            {
                return Ok(GetFile(catalog.Name));
            }

            return Ok(string.Format("{0} not found", catalogName));
        }

        private Task GetFile(string catalogName)
        {
            // CMS 13: IBlobFactory removed. Export directly to MemoryStream.
            var memoryStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
            {
                var entry = zipArchive.CreateEntry("catalog.xml");
                using (var entryStream = entry.Open())
                {
                    _importExport.Export(catalogName, entryStream, Path.GetTempPath());
                }
            }
            memoryStream.Position = 0;
            HttpContext.Response.ContentType = "application/zip";
            return memoryStream.CopyToAsync(HttpContext.Response.Body);
        }

        //[HttpGet]
        //[Route("streaming")]
        //public async Task GetStreaming()
        //{
        //    const string filePath = @"C:\Users\mike\Downloads\dotnet-sdk-3.1.201-win-x64.exe";
        //    this.Response.StatusCode = 200;
        //    this.Response.Headers.Add(HeaderNames.ContentDisposition, $"attachment; filename=\"{Path.GetFileName(filePath)}\"");
        //    this.Response.Headers.Add(HeaderNames.ContentType, "application/octet-stream");
        //    var inputStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        //    var outputStream = this.Response.Body;
        //    const int bufferSize = 1 << 10;
        //    var buffer = new byte[bufferSize];
        //    while (true)
        //    {
        //        var bytesRead = await inputStream.ReadAsync(buffer, 0, bufferSize);
        //        if (bytesRead == 0) break;
        //        await outputStream.WriteAsync(buffer, 0, bytesRead);
        //    }
        //    await outputStream.FlushAsync();
        //}
    }
}
