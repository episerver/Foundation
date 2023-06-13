using Mediachase.Commerce.Inventory;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Shared;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Foundation.Infrastructure.Commerce.Install.Steps
{
    public class AddWarehouses : BaseInstallStep
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public AddWarehouses(IContentRepository contentRepository,
            ReferenceConverter referenceConverter,
            IMarketService marketService,
            IWarehouseRepository warehouseRepository,
            IWebHostEnvironment webHostEnvironment) : base(contentRepository, referenceConverter, marketService, webHostEnvironment)
        {
            _warehouseRepository = warehouseRepository;
        }

        public override int Order => 3;

        public override string Description => "Adds warehouses to Foundation.";

        protected override void ExecuteInternal(IProgressMessenger progressMessenger)
        {
            using (var stream = new FileStream(Path.Combine(WebHostEnvironment.ContentRootPath, @"App_Data", @"warehouses.xml"), FileMode.Open))
            {
                foreach (var xWarehouse in GetXElements(stream, "Warehouse"))
                {
                    var contactInfo = new WarehouseContactInformation
                    {
                        FirstName = xWarehouse.Get("FirstName"),
                        LastName = xWarehouse.Get("LastName"),
                        Organization = xWarehouse.Get("Organization"),
                        Line1 = xWarehouse.Get("Line1"),
                        Line2 = xWarehouse.Get("Line2"),
                        City = xWarehouse.Get("City"),
                        State = xWarehouse.Get("State"),
                        CountryCode = xWarehouse.Get("CountryCode"),
                        CountryName = xWarehouse.Get("CountryName"),
                        PostalCode = xWarehouse.Get("PostalCode"),
                        RegionCode = xWarehouse.Get("RegionCode"),
                        RegionName = xWarehouse.Get("RegionName"),
                        DaytimePhoneNumber = xWarehouse.Get("DaytimePhoneNumber"),
                        EveningPhoneNumber = xWarehouse.Get("EveningPhoneNumber"),
                        FaxNumber = xWarehouse.Get("FaxNumber"),
                        Email = xWarehouse.Get("Email")
                    };

                    var warehouse = new Warehouse
                    {
                        Created = DateTime.UtcNow,
                        Modified = DateTime.UtcNow,
                        CreatorId = "admin@example.com",
                        ModifierId = "admin@example.com",
                        ContactInformation = contactInfo,
                        Name = xWarehouse.GetStringOrEmpty("Name"),
                        IsActive = xWarehouse.GetBoolOrDefault("IsActive"),
                        IsPrimary = xWarehouse.GetBoolOrDefault("IsPrimary"),
                        SortOrder = xWarehouse.GetIntOrDefault("SortOrder"),
                        Code = xWarehouse.Get("Code"),
                        IsFulfillmentCenter = xWarehouse.GetBoolOrDefault("IsFulfillmentCenter"),
                        IsPickupLocation = xWarehouse.GetBoolOrDefault("IsPickupLocation"),
                        IsDeliveryLocation = xWarehouse.GetBoolOrDefault("IsDeliveryLocation"),
                    };

                    var existingWarehouse = _warehouseRepository.Get(warehouse.Code);
                    if (existingWarehouse != null)
                    {
                        warehouse.WarehouseId = existingWarehouse.WarehouseId;
                        warehouse.Created = existingWarehouse.Created;
                        warehouse.CreatorId = existingWarehouse.CreatorId;
                    }

                    _warehouseRepository.Save(warehouse);
                }
            }
        }
    }
}
