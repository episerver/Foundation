using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Order;
using EPiServer.Globalization;
using Foundation.Commerce.Markets;
using Foundation.Features.Checkout.ViewModels;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Infrastructure;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Orders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.Checkout.Services
{
    public class ShipmentViewModelFactory
    {
        private readonly IContentLoader _contentLoader;
        private readonly IShippingService _shippingService;
        private readonly LanguageService _languageService;
        private readonly ReferenceConverter _referenceConverter;
        private readonly IAddressBookService _addressBookService;
        private readonly CartItemViewModelFactory _cartItemViewModelFactory;
        private readonly LanguageResolver _languageResolver;
        private readonly IMarketService _marketService;
        private ShippingMethodInfoModel _instorePickup;

        public ShipmentViewModelFactory(
            IContentLoader contentLoader,
            IShippingService shippingService,
            LanguageService languageService,
            ReferenceConverter referenceConverter,
            IAddressBookService addressBookService,
            CartItemViewModelFactory cartItemViewModelFactory,
            LanguageResolver languageResolver,
            IMarketService marketService)
        {
            _contentLoader = contentLoader;
            _shippingService = shippingService;
            _languageService = languageService;
            _referenceConverter = referenceConverter;
            _addressBookService = addressBookService;
            _cartItemViewModelFactory = cartItemViewModelFactory;
            _languageResolver = languageResolver;
            _marketService = marketService;
        }

        public virtual ShippingMethodInfoModel InStorePickupInfoModel => _instorePickup ?? (_instorePickup = _shippingService.GetInstorePickupModel());

        public virtual IEnumerable<ShipmentViewModel> CreateShipmentsViewModel(ICart cart)
        {
            var preferredCulture = _languageResolver.GetPreferredCulture();
            foreach (var shipment in cart.GetFirstForm().Shipments)
            {
                var shipmentModel = new ShipmentViewModel
                {
                    ShipmentId = shipment.ShipmentId,
                    CartItems = new List<CartItemViewModel>(),
                    Address = _addressBookService.ConvertToModel(shipment.ShippingAddress),
                    ShippingMethods = CreateShippingMethodViewModels(cart.MarketId, cart.Currency, shipment)
                };

                var currentShippingMethod = shipmentModel.ShippingMethods.FirstOrDefault();
                if (shipment.ShippingMethodId != Guid.Empty)
                {
                    currentShippingMethod = shipmentModel.ShippingMethods.FirstOrDefault(x => x.Id == shipment.ShippingMethodId);
                }
                else
                {
                    currentShippingMethod = shipmentModel.ShippingMethods.FirstOrDefault();
                }

                shipmentModel.ShippingMethodId = currentShippingMethod?.Id ?? shipment.ShippingMethodId;
                shipmentModel.CurrentShippingMethodName = currentShippingMethod?.DisplayName ?? "In store pickup";
                shipmentModel.CurrentShippingMethodPrice = currentShippingMethod?.Price ?? new Money(0, cart.Currency);
                shipmentModel.WarehouseCode = shipment.WarehouseCode;

                var entries = _contentLoader.GetItems(shipment.LineItems.Select(x => _referenceConverter.GetContentLink(x.Code)),
                    preferredCulture).OfType<EntryContentBase>();

                foreach (var lineItem in shipment.LineItems)
                {
                    var entry = entries.FirstOrDefault(x => x.Code == lineItem.Code);
                    if (entry == null)
                    {
                        //Entry was deleted, skip processing.
                        continue;
                    }

                    shipmentModel.CartItems.Add(_cartItemViewModelFactory.CreateCartItemViewModel(cart, lineItem, entry));
                }

                yield return shipmentModel;
            }
        }

        private IEnumerable<ShippingMethodViewModel> CreateShippingMethodViewModels(MarketId marketId, Currency currency, IShipment shipment)
        {
            var shippingRates = GetShippingRates(marketId, currency, shipment);

            if (shipment.LineItems.Count(o => o.IsVirtualVariant()) == shipment.LineItems.Count)
            {
                shippingRates = shippingRates.Where(o => o.Money == 0);
            }
            else
            {
                shippingRates = shippingRates.Where(o => o.Money > 0);
            }

            var models = shippingRates.Select(r => new ShippingMethodViewModel { Id = r.Id, DisplayName = r.Name, Price = r.Money })
                .ToList();

            if (shipment.ShippingMethodId == InStorePickupInfoModel.MethodId)
            {
                models.Insert(0, new ShippingMethodViewModel
                {
                    Id = InStorePickupInfoModel.MethodId,
                    DisplayName = $"In store pickup - ({shipment.ShippingAddress.Line1} , {shipment.ShippingAddress.City} , {shipment.ShippingAddress.RegionName})",
                    Price = new Money(0m, currency),
                    IsInstorePickup = true
                });
            }

            return models;
        }

        public IEnumerable<ShippingRate> GetShippingRates(MarketId marketId, Currency currency, IShipment shipment)
        {
            var methods = _shippingService.GetShippingMethodsByMarket(marketId.Value, false)
                .Where(x => x.MethodId != InStorePickupInfoModel.MethodId);
            var currentLanguage = _languageService.GetCurrentLanguage().TwoLetterISOLanguageName;

            return methods.Where(shippingMethodRow => currentLanguage.Equals(shippingMethodRow.LanguageId, StringComparison.OrdinalIgnoreCase)
                && string.Equals(currency, shippingMethodRow.Currency, StringComparison.OrdinalIgnoreCase))
                .OrderBy(shippingMethodRow => shippingMethodRow.Ordering)
                .Select(shippingMethodRow => _shippingService.GetRate(shipment, shippingMethodRow, _marketService.GetMarket(marketId)));
        }
    }
}