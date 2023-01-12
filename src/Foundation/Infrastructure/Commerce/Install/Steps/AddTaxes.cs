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

        public AddTaxes(IContentRepository contentRepository,
            ReferenceConverter referenceConverter,
            IMarketService marketService,
            TaxImportExport taxImportExport,
            IWebHostEnvironment webHostEnvironment) : base(contentRepository, referenceConverter, marketService, webHostEnvironment)
        {
            _taxImportExport = taxImportExport;
        }

        public override int Order => 4;

        public override string Description => "Adds taxes to Foundation.";

        protected override void ExecuteInternal(IProgressMessenger progressMessenger) => 
            _taxImportExport.Import(Path.Combine(WebHostEnvironment.ContentRootPath, @"App_Data", @"Taxes.csv"), ',');
    }
}
