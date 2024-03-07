using EPiServer.Framework.Blobs;
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
        private readonly IBlobFactory _blobFactory;
        private readonly IContentLoader _contentLoader;
        private readonly ReferenceConverter _referenceConverter;
        internal const string DownloadRoute = "episerverapi/catalogs/";
        private static readonly Guid _blobContainerIdentifier = Guid.Parse("119AD01E-ECD1-4781-898B-6DEC356FC8D8");

        private static readonly ILogger _logger = LogManager.GetLogger(typeof(CatalogExportController));


        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogExportController"/> class.
        /// </summary>
        /// <param name="importExport">Catalog import export</param>
        /// <param name="blobFactory">The blob factory.</param>
        /// <param name="contentLoader">The content loader.</param>
        /// <param name="referenceConverter"></param>
        public CatalogExportController(CatalogImportExport importExport,
            IBlobFactory blobFactory,
            IContentLoader contentLoader,
            ReferenceConverter referenceConverter)
        {
            _importExport = importExport;
            _blobFactory = blobFactory;
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
            var catalogs = _contentLoader.GetChildren<EPiServer.Commerce.Catalog.ContentTypes.CatalogContent >(_referenceConverter.GetRootLink());
            var catalog = catalogs.FirstOrDefault(x => x.Name.Equals(catalogName, StringComparison.OrdinalIgnoreCase));
            if (catalog != null)
            {
                return Ok(GetFile(catalog.Name));
            }

            return Ok(string.Format("{0} not found", catalogName)) ; 
            
        }

        private Task GetFile(string catalogName)
        {
            var container = Blob.GetContainerIdentifier(_blobContainerIdentifier);
            var blob = _blobFactory.CreateBlob(container, ".zip");
            using (var stream = blob.OpenWrite())
            {
                using (var zipArchive = new ZipArchive(stream, ZipArchiveMode.Create, false))
                {
                    var entry = zipArchive.CreateEntry("catalog.xml");

                    using (var entryStream = entry.Open())
                    {
                        _importExport.Export(catalogName, entryStream, Path.GetTempPath());
                    }
                }
            }


            HttpContext.Response.ContentType = "application/zip";
            var sourceStream = blob.OpenRead();// get the source stream
            return sourceStream.CopyToAsync(HttpContext.Response.Body);

            
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
