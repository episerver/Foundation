using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using Foundation.Cms;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Inventory;
using Mediachase.Commerce.InventoryService;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.Stores
{
    public interface IStoreService
    {
        List<StoreItemViewModel> GetEntryStoresViewModels(string entryCode);
        List<StoreItemViewModel> GetAllStoreViewModels();
        StoreItemViewModel GetCurrentStoreViewModel();
        bool SetCurrentStore(string storeCode);
    }

    public class StoreService : IStoreService
    {
        private const string StoreCookie = "CurrentStore";
        private readonly IInventoryService _inventoryService;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IContentLoader _contentLoader;
        private readonly ReferenceConverter _referenceConverter;
        private readonly ICurrentMarket _currentMarket;
        private readonly IRelationRepository _relationRepository;
        private readonly CookieService _cookieService;

        public StoreService(IInventoryService inventoryService,
            IWarehouseRepository warehouseRepository,
            IContentLoader contentLoader,
            ReferenceConverter referenceConverter,
            ICurrentMarket currentMarket,
            IRelationRepository relationRepository,
            CookieService cookieService)
        {
            _inventoryService = inventoryService;
            _warehouseRepository = warehouseRepository;
            _contentLoader = contentLoader;
            _referenceConverter = referenceConverter;
            _currentMarket = currentMarket;
            _relationRepository = relationRepository;
            _cookieService = cookieService;
        }

        public List<StoreItemViewModel> GetEntryStoresViewModels(string entryCode)
        {
            var entry = _contentLoader.Get<EntryContentBase>(_referenceConverter.GetContentLink(entryCode, CatalogContentType.CatalogEntry));
            if (entry == null)
            {
                return new List<StoreItemViewModel>();
            }

            var warehouses = _warehouseRepository.List().Where(x => x.IsActive && x.IsPickupLocation);
            var codes = new List<string>();
            if (entry.ClassTypeId.Equals("Variation") || entry.ClassTypeId.Equals("Package"))
            {
                codes.Add(entryCode);
            }
            else if (entry.ClassTypeId.Equals("Product"))
            {
                var product = entry as ProductContent;
                if (product != null)
                {
                    codes.AddRange(product.GetVariants(_relationRepository).Select(x => _referenceConverter.GetCode(x)));
                }
            }

            if (!codes.Any())
            {
                return new List<StoreItemViewModel>();
            }

            var records = _inventoryService.QueryByEntry(codes);

            if (!records.Any())
            {
                return new List<StoreItemViewModel>();
            }

            var currentMarket = _currentMarket.GetCurrentMarket();

            return warehouses
                .Where(x => records.Any(y => y.WarehouseCode.Equals(x.Code) &&
                                             !string.IsNullOrEmpty(x.ContactInformation.CountryCode) && currentMarket.Countries.Any(z => x.ContactInformation.CountryCode.Equals(z))))
                .Select(x => GetWarehoseViewModel(x, records.FirstOrDefault(y => y.WarehouseCode.Equals(x.Code))))
                .ToList();
        }

        public List<StoreItemViewModel> GetAllStoreViewModels()
        {
            var warehouses = _warehouseRepository.List().Where(x => x.IsActive && x.IsPickupLocation);
            var currentMarket = _currentMarket.GetCurrentMarket();
            return warehouses.Where(x => !string.IsNullOrEmpty(x.ContactInformation.CountryCode) &&
                                         currentMarket.Countries.Any(z => x.ContactInformation.CountryCode.Equals(z)))
                .Select(x => GetWarehoseViewModel(x, null))
                .ToList();
        }

        public StoreItemViewModel GetCurrentStoreViewModel()
        {
            return TryGetStoreViewModel(_cookieService.Get(StoreCookie), out var storeViewModel) ?
                storeViewModel :
                GetDefaultStoreViewModel();
        }

        public bool SetCurrentStore(string storeCode)
        {
            if (!TryGetStoreViewModel(storeCode, out _))
            {
                return false;
            }

            _cookieService.Set(StoreCookie, storeCode);

            return true;
        }

        private static StoreItemViewModel GetWarehoseViewModel(IWarehouse warehouse, InventoryRecord record)
        {
            return new StoreItemViewModel
            {
                Code = warehouse.Code,
                City = warehouse.ContactInformation.City,
                CountryCode = warehouse.ContactInformation.CountryCode,
                CountryName = warehouse.ContactInformation.CountryName,
                IsFulfillmentCenter = warehouse.IsFulfillmentCenter,
                IsPickupLocation = warehouse.IsPickupLocation,
                Line1 = warehouse.ContactInformation.Line1,
                Line2 = warehouse.ContactInformation.Line2,
                Name = warehouse.Name,
                RegionCode = !string.IsNullOrEmpty(warehouse.ContactInformation.RegionCode) ?
                    warehouse.ContactInformation.RegionCode : warehouse.ContactInformation.PostalCode,
                RegionName = warehouse.ContactInformation.RegionName,
                Inventory = record?.PurchaseAvailableQuantity ?? 0
            };
        }

        private bool TryGetStoreViewModel(string storeCode, out StoreItemViewModel storeViewModel)
        {
            var result = GetAllStoreViewModels()
                .FirstOrDefault(x => x.Code == storeCode);

            if (result != null)
            {
                storeViewModel = result;
                return true;
            }

            storeViewModel = null;
            return false;
        }

        private StoreItemViewModel GetDefaultStoreViewModel()
        {
            return GetAllStoreViewModels().
                FirstOrDefault();
        }
    }
}
