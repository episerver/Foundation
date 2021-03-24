using EPiServer;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Orders.ImportExport;
using Mediachase.Commerce.Shared;
using System.IO;
using System.Web.Hosting;

namespace Foundation.Commerce.Install.Steps
{
    public class AddTaxes : BaseInstallStep
    {
        private readonly TaxImportExport _taxImportExport;

        public AddTaxes(IContentRepository contentRepository,
            ReferenceConverter referenceConverter,
            IMarketService marketService,
            TaxImportExport taxImportExport) : base(contentRepository, referenceConverter, marketService) => _taxImportExport = taxImportExport;

        public override int Order => 4;

        public override string Description => "Adds taxes to Foundation.";

        protected override void ExecuteInternal(IProgressMessenger progressMessenger) => _taxImportExport.Import(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"App_Data\Taxes.csv"), ',');
    }
}
