using EPiServer;
using EPiServer.Enterprise;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Orders.ImportExport;
using Mediachase.Commerce.Shared;
using System.IO;
using System.Web.Hosting;

namespace Foundation.Demo.Install.Steps
{
    [ServiceConfiguration(ServiceType = typeof(IInstallStep), Lifecycle = ServiceInstanceScope.Singleton)]
    public class AddTaxes : BaseInstallStep
    {
        private readonly TaxImportExport _taxImportExport;

        public AddTaxes(IContentRepository contentRepository,
            ServiceAccessor<IDataImporter> dataImporter,
            ReferenceConverter referenceConverter,
            IMarketService marketService,
            TaxImportExport taxImportExport) : base(contentRepository, dataImporter, referenceConverter, marketService) => _taxImportExport = taxImportExport;

        public override int Order => 4;

        public override string Description => "Adds taxes to Foundation.";

        protected override void ExecuteInternal(IProgressMessenger progressMessenger) => _taxImportExport.Import(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"App_Data\Taxes.csv"), ',');
    }
}
