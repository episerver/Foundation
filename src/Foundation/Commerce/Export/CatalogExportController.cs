using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Framework.Blobs;
using EPiServer.Logging;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Catalog.ImportExport;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace Foundation.Commerce.Export
{
    /// <summary>
    /// Download a catalog.
    /// </summary>
    public class CatalogExportController : ApiController
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

        /// <summary>
        /// Direct download catalog export for admins.
        /// </summary>
        /// <param name="catalogName">Name of catalog to be exported.</param>
        /// <returns>
        /// Catalog.zip if successful else HttpResponseMessage containing error.
        /// </returns>
        [HttpGet]
        [Authorize(Roles = "CommerceAdmins")]
        [Route(DownloadRoute)]
        public HttpResponseMessage Index(string catalogName)
        {
            var catalogs = _contentLoader.GetChildren<CatalogContent>(_referenceConverter.GetRootLink());
            var catalog = catalogs.First(x => x.Name.Equals(catalogName, StringComparison.OrdinalIgnoreCase));
            if (catalog != null)
            {
                return GetFile(catalog.Name);
            }

            return new HttpResponseMessage
            { Content = new StringContent($"There is no catalog with name {catalogName}.") };
        }

        private HttpResponseMessage GetFile(string catalogName)
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

            var response = new HttpResponseMessage
            {
                Content = new PushStreamContent(async (outputStream, content, context) =>
                {
                    var fileStream = blob.OpenRead();

                    await fileStream.CopyToAsync(outputStream)
                        .ContinueWith(task =>
                        {
                            fileStream.Close();
                            outputStream.Close();

                            if (task.IsFaulted)
                            {
                                _logger.Error($"Catalog download failed", task.Exception);
                                return;
                            }

                            _logger.Information($"Feed download completed.");
                        }, TaskScheduler.FromCurrentSynchronizationContext());
                }, new MediaTypeHeaderValue("application/zip"))
            };
            return response;
        }
    }
}