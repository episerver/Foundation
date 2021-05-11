using EPiServer;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Orders.ImportExport;
using Mediachase.Commerce.Shared;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Foundation.Infrastructure.Commerce.Install.Steps
{
    public class AddTaxes : BaseInstallStep
    {
        private readonly TaxImportExport _taxImportExport;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AddTaxes(IContentRepository contentRepository,
            ReferenceConverter referenceConverter,
            IMarketService marketService,
            TaxImportExport taxImportExport,
            IWebHostEnvironment webHostEnvironment) : base(contentRepository, referenceConverter, marketService)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public override int Order => 4;

        public override string Description => "Adds taxes to Foundation.";

        protected override void ExecuteInternal(IProgressMessenger progressMessenger) => _taxImportExport.Import(Path.Combine(_webHostEnvironment.ContentRootPath, @"App_Data\Taxes.csv"), ',');
    }
}
