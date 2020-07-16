using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Order;
using EPiServer.Logging;
using EPiServer.Security;
using EPiServer.Web;
using Foundation.Commerce;
using Foundation.Commerce.Extensions;
using Foundation.Commerce.Markets;
using Foundation.Features.CatalogContent.Services;
using Foundation.Features.Checkout.ViewModels;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.MyOrganization.Organization;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Inventory;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Foundation.Features.Checkout.Services
{
    public class CartService : ICartService
    {
        private readonly IProductService _productService;
        private readonly IOrderGroupFactory _orderGroupFactory;
        private readonly CustomerContext _customerContext;
        private readonly IPlacedPriceProcessor _placedPriceProcessor;
        private readonly IInventoryProcessor _inventoryProcessor;
        private readonly ILineItemValidator _lineItemValidator;
        private readonly IPromotionEngine _promotionEngine;
        private readonly IOrderRepository _orderRepository;
        private readonly IAddressBookService _addressBookService;
        private readonly ICurrentMarket _currentMarket;
        private readonly ICurrencyService _currencyService;
        private readonly ReferenceConverter _referenceConverter;
        private readonly IContentLoader _contentLoader;
        private readonly IRelationRepository _relationRepository;
        private readonly ShippingService _shippingManagerFacade;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly ILineItemCalculator _lineItemCalculator;
        private readonly IPromotionService _promotionService;
        private ShippingMethodInfoModel _instorePickup;
        private readonly IOrganizationService _organizationService;

        public CartService(
            IProductService productService,
            IOrderGroupFactory orderGroupFactory,
            IPlacedPriceProcessor placedPriceProcessor,
            IInventoryProcessor inventoryProcessor,
            ILineItemValidator lineItemValidator,
            IOrderRepository orderRepository,
            IPromotionEngine promotionEngine,
            IAddressBookService addressBookService,
            ICurrentMarket currentMarket,
            ICurrencyService currencyService,
            ReferenceConverter referenceConverter,
            IContentLoader contentLoader,
            IRelationRepository relationRepository,
            ShippingService shippingManagerFacade,
            IWarehouseRepository warehouseRepository,
            ILineItemCalculator lineItemCalculator,
            IPromotionService promotionService, IOrganizationService organizationService)
        {
            _productService = productService;
            _orderGroupFactory = orderGroupFactory;
            _customerContext = CustomerContext.Current;
            _placedPriceProcessor = placedPriceProcessor;
            _inventoryProcessor = inventoryProcessor;
            _lineItemValidator = lineItemValidator;
            _promotionEngine = promotionEngine;
            _orderRepository = orderRepository;
            _addressBookService = addressBookService;
            _currentMarket = currentMarket;
            _currencyService = currencyService;
            _referenceConverter = referenceConverter;
            _contentLoader = contentLoader;
            _relationRepository = relationRepository;
            _shippingManagerFacade = shippingManagerFacade;
            _warehouseRepository = warehouseRepository;
            _lineItemCalculator = lineItemCalculator;
            _promotionService = promotionService;
            _organizationService = organizationService;
        }

        public ShippingMethodInfoModel InStorePickupInfoModel => _instorePickup ?? (_instorePickup = _shippingManagerFacade.GetInstorePickupModel());

        public Dictionary<ILineItem, List<ValidationIssue>> ChangeCartItem(ICart cart, int shipmentId, string code, decimal quantity, string size, string newSize)
        {
            _ = new Dictionary<ILineItem, List<ValidationIssue>>();
            Dictionary<ILineItem, List<ValidationIssue>> validationIssues;
            if (quantity > 0)
            {
                if (size == newSize)
                {
                    validationIssues = ChangeQuantity(cart, shipmentId, code, quantity);
                }
                else
                {
                    var newCode = _productService.GetSiblingVariantCodeBySize(code, newSize);
                    validationIssues = UpdateLineItemSku(cart, shipmentId, code, newCode, quantity);
                }
            }
            else
            {
                validationIssues = RemoveLineItem(cart, shipmentId, code);
            }

            return validationIssues;
        }

        public string DefaultCartName => "Default" + SiteDefinition.Current.StartPage.ID;

        public string DefaultWishListName => "WishList" + SiteDefinition.Current.StartPage.ID;

        public string DefaultSharedCartName => "Shared" + SiteDefinition.Current.StartPage.ID;

        public string DefaultOrderPadName => "OrderPad" + SiteDefinition.Current.StartPage.ID;

        public void RecreateLineItemsBasedOnShipments(ICart cart, IEnumerable<CartItemViewModel> cartItems, IEnumerable<AddressModel> addresses)
        {
            var form = cart.GetFirstForm();
            var items = cartItems
                .GroupBy(x => new { x.AddressId, x.Code, x.DisplayName, x.IsGift })
                .Select(x => new
                {
                    x.Key.Code,
                    x.Key.DisplayName,
                    x.Key.AddressId,
                    Quantity = x.Count(),
                    x.Key.IsGift
                });

            foreach (var shipment in form.Shipments)
            {
                shipment.LineItems.Clear();
            }

            form.Shipments.Clear();

            foreach (var address in addresses)
            {
                var shipment = cart.CreateShipment(_orderGroupFactory);
                form.Shipments.Add(shipment);
                shipment.ShippingAddress = _addressBookService.ConvertToAddress(address, cart);

                foreach (var item in items.Where(x => x.AddressId == address.AddressId))
                {
                    var lineItem = cart.CreateLineItem(item.Code, _orderGroupFactory);
                    lineItem.DisplayName = item.DisplayName;
                    lineItem.IsGift = item.IsGift;
                    lineItem.Quantity = item.Quantity;
                    shipment.LineItems.Add(lineItem);
                }
            }

            ValidateCart(cart);
        }

        public void MergeShipments(ICart cart)
        {
            if (cart == null || !cart.GetAllLineItems().Any())
            {
                return;
            }

            var form = cart.GetFirstForm();
            var keptShipment = cart.GetFirstShipment();
            var removedShipments = form.Shipments.Skip(1).ToList();
            var movedLineItems = removedShipments.SelectMany(x => x.LineItems).ToList();
            removedShipments.ForEach(x => x.LineItems.Clear());
            removedShipments.ForEach(x => cart.GetFirstForm().Shipments.Remove(x));

            foreach (var item in movedLineItems)
            {
                var existingLineItem = keptShipment.LineItems.SingleOrDefault(x => x.Code == item.Code);
                if (existingLineItem != null)
                {
                    existingLineItem.Quantity += item.Quantity;
                    continue;
                }

                keptShipment.LineItems.Add(item);
            }

            ValidateCart(cart);
        }

        public AddToCartResult AddToCart(ICart cart, string code, decimal quantity, string deliveryMethod, string warehouseCode)
        {
            var contentLink = _referenceConverter.GetContentLink(code);
            var entryContent = _contentLoader.Get<EntryContentBase>(contentLink);
            return AddToCart(cart, entryContent, quantity, deliveryMethod, warehouseCode);
        }

        public AddToCartResult AddToCart(ICart cart, EntryContentBase entryContent, decimal quantity, string deliveryMethod, string warehouseCode)
        {
            var result = new AddToCartResult();
            var contact = PrincipalInfo.CurrentPrincipal.GetCustomerContact();

            if (contact?.OwnerId != null)
            {
                var org = cart.GetString("OwnerOrg");
                if (string.IsNullOrEmpty(org))
                {
                    cart.Properties["OwnerOrg"] = contact.OwnerId.Value.ToString().ToLower();
                }
            }

            IWarehouse warehouse = null;

            if (deliveryMethod.Equals("instore") && !string.IsNullOrEmpty(warehouseCode))
            {
                warehouse = _warehouseRepository.Get(warehouseCode);
            }

            if (entryContent is BundleContent)
            {
                foreach (var relation in _relationRepository.GetChildren<BundleEntry>(entryContent.ContentLink))
                {
                    var entry = _contentLoader.Get<EntryContentBase>(relation.Child);
                    var recursiveResult = AddToCart(cart, entry, (relation.Quantity ?? 1) * quantity, deliveryMethod, warehouseCode);
                    if (recursiveResult.EntriesAddedToCart)
                    {
                        result.EntriesAddedToCart = true;
                    }

                    foreach (var message in recursiveResult.ValidationMessages)
                    {
                        result.ValidationMessages.Add(message);
                    }
                }

                return result;
            }

            var form = cart.GetFirstForm();
            if (form == null)
            {
                form = _orderGroupFactory.CreateOrderForm(cart);
                form.Name = cart.Name;
                cart.Forms.Add(form);
            }

            var shipment = cart.GetFirstForm().Shipments.FirstOrDefault(x => string.IsNullOrEmpty(warehouseCode) || (x.WarehouseCode == warehouseCode && x.ShippingMethodId == InStorePickupInfoModel.MethodId));
            if (warehouse != null)
            {
                if (shipment != null && !shipment.LineItems.Any())
                {
                    shipment.WarehouseCode = warehouseCode;
                    shipment.ShippingMethodId = InStorePickupInfoModel.MethodId;
                    shipment.ShippingAddress = GetOrderAddressFromWarehosue(cart, warehouse);
                }
                else
                {
                    shipment = form.Shipments.FirstOrDefault(x => !string.IsNullOrEmpty(x.WarehouseCode) && x.WarehouseCode.Equals(warehouse.Code));
                    if (shipment == null)
                    {
                        if (cart.GetFirstShipment().LineItems.Count > 0)
                        {
                            shipment = _orderGroupFactory.CreateShipment(cart);
                        }
                        else
                        {
                            shipment = cart.GetFirstShipment();
                        }

                        shipment.WarehouseCode = warehouseCode;
                        shipment.ShippingMethodId = InStorePickupInfoModel.MethodId;
                        shipment.ShippingAddress = GetOrderAddressFromWarehosue(cart, warehouse);

                        if (cart.GetFirstShipment().LineItems.Count > 0)
                        {
                            cart.GetFirstForm().Shipments.Add(shipment);
                        }
                    }
                }
            }

            if (shipment == null)
            {
                if (cart.GetFirstShipment().LineItems.Count > 0)
                {
                    shipment = _orderGroupFactory.CreateShipment(cart);
                    cart.GetFirstForm().Shipments.Add(shipment);
                }
                else
                {
                    shipment = cart.GetFirstShipment();
                }
            }

            var lineItem = shipment.LineItems.FirstOrDefault(x => x.Code == entryContent.Code);
            decimal originalLineItemQuantity = 0;

            if (lineItem == null)
            {
                lineItem = cart.CreateLineItem(entryContent.Code, _orderGroupFactory);
                lineItem.DisplayName = entryContent.DisplayName;
                lineItem.Quantity = quantity;
                cart.AddLineItem(shipment, lineItem);
            }
            else
            {
                originalLineItemQuantity = lineItem.Quantity;
                cart.UpdateLineItemQuantity(shipment, lineItem, lineItem.Quantity + quantity);
            }

            var validationIssues = ValidateCart(cart);
            var newLineItem = shipment.LineItems.FirstOrDefault(x => x.Code == entryContent.Code);
            var isAdded = (newLineItem != null ? newLineItem.Quantity : 0) - originalLineItemQuantity > 0;

            AddValidationMessagesToResult(result, lineItem, validationIssues, isAdded);

            return result;
        }

        public void SetCartCurrency(ICart cart, Currency currency)
        {
            if (currency.IsEmpty || currency == cart.Currency)
            {
                return;
            }

            cart.Currency = currency;
            foreach (var lineItem in cart.GetAllLineItems())
            {
                // If there is an item which has no price in the new currency, a NullReference exception will be thrown.
                // Mixing currencies in cart is not allowed.
                // It's up to site's managers to ensure that all items have prices in allowed currency.
                lineItem.PlacedPrice = PriceCalculationService.GetSalePrice(lineItem.Code, cart.MarketId, currency).UnitPrice.Amount;
            }

            ValidateCart(cart);
        }

        public Dictionary<ILineItem, List<ValidationIssue>> ValidateCart(ICart cart)
        {
            var validationIssues = new Dictionary<ILineItem, List<ValidationIssue>>();
            if (cart.Name.Equals(DefaultWishListName))
            {
                cart.UpdatePlacedPriceOrRemoveLineItems(_customerContext.GetContactById(cart.CustomerId), (item, issue) => validationIssues.AddValidationIssues(item, issue), _placedPriceProcessor);
                return validationIssues;
            }

            cart.ValidateOrRemoveLineItems((item, issue) => validationIssues.AddValidationIssues(item, issue), _lineItemValidator);
            cart.UpdatePlacedPriceOrRemoveLineItems(_customerContext.GetContactById(cart.CustomerId), (item, issue) => validationIssues.AddValidationIssues(item, issue), _placedPriceProcessor);
            cart.UpdateInventoryOrRemoveLineItems((item, issue) => validationIssues.AddValidationIssues(item, issue), _inventoryProcessor);

            cart.ApplyDiscounts(_promotionEngine, new PromotionEngineSettings());

            return validationIssues;
        }

        public Dictionary<ILineItem, List<ValidationIssue>> RequestInventory(ICart cart)
        {
            var validationIssues = new Dictionary<ILineItem, List<ValidationIssue>>();
            cart.AdjustInventoryOrRemoveLineItems((item, issue) => validationIssues.AddValidationIssues(item, issue), _inventoryProcessor);
            return validationIssues;
        }

        public CartWithValidationIssues LoadCart(string name, bool validate) => LoadCart(name, _customerContext.CurrentContactId.ToString(), validate);

        public CartWithValidationIssues LoadCart(string name, string contactId, bool validate)
        {
            var validationIssues = new Dictionary<ILineItem, List<ValidationIssue>>();
            var cart = !string.IsNullOrEmpty(contactId) ? _orderRepository.LoadOrCreateCart<ICart>(new Guid(contactId), name, _currentMarket) : null;
            if (cart != null)
            {
                SetCartCurrency(cart, _currencyService.GetCurrentCurrency());
                if (validate)
                {
                    validationIssues = ValidateCart(cart);
                    if (validationIssues.Any())
                    {
                        _orderRepository.Save(cart);
                    }
                }
            }

            return new CartWithValidationIssues
            {
                Cart = cart,
                ValidationIssues = validationIssues
            };
        }

        public ICart LoadOrCreateCart(string name) => LoadOrCreateCart(name, _customerContext.CurrentContactId.ToString());

        public ICart LoadOrCreateCart(string name, string contactId)
        {
            if (string.IsNullOrEmpty(contactId))
            {
                return null;
            }
            else
            {
                var cart = _orderRepository.LoadOrCreateCart<ICart>(new Guid(contactId), name, _currentMarket);
                if (cart != null)
                {
                    SetCartCurrency(cart, _currencyService.GetCurrentCurrency());
                }

                return cart;
            }
        }

        public bool AddCouponCode(ICart cart, string couponCode)
        {
            var couponCodes = cart.GetFirstForm().CouponCodes;
            if (couponCodes.Any(c => c.Equals(couponCode, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            couponCodes.Add(couponCode);
            var rewardDescriptions = cart.ApplyDiscounts(_promotionEngine, new PromotionEngineSettings());
            var appliedCoupons = rewardDescriptions
                .Where(r => r.AppliedCoupon != null)
                .Select(r => r.AppliedCoupon);

            var couponApplied = appliedCoupons.Any(c => c.Equals(couponCode, StringComparison.OrdinalIgnoreCase));
            if (!couponApplied)
            {
                couponCodes.Remove(couponCode);
            }

            return couponApplied;
        }

        public void RemoveCouponCode(ICart cart, string couponCode)
        {
            cart.GetFirstForm().CouponCodes.Remove(couponCode);
            cart.ApplyDiscounts(_promotionEngine, new PromotionEngineSettings());
        }

        public Dictionary<ILineItem, List<ValidationIssue>> ChangeQuantity(ICart cart, int shipmentId, string code, decimal quantity)
        {
            if (quantity == 0)
            {
                return RemoveLineItem(cart, shipmentId, code);
            }
            else
            {
                var shipment = cart.GetFirstForm().Shipments.First(s => s.ShipmentId == shipmentId || shipmentId == 0);
                var lineItem = shipment.LineItems.FirstOrDefault(x => x.Code == code);
                if (lineItem == null)
                {
                    throw new InvalidOperationException($"No lineitem with matching code '{code}' for shipment id {shipmentId}");
                }

                cart.UpdateLineItemQuantity(shipment, lineItem, quantity);
            }

            return ValidateCart(cart);
        }

        private Dictionary<ILineItem, List<ValidationIssue>> RemoveLineItem(ICart cart, int shipmentId, string code)
        {
            if (cart.GetFirstForm().Shipments.Any())
            {
                //gets  the shipment for shipment id or for wish list shipment id as a parameter is always equal zero( wish list).
                var shipment = cart.GetFirstForm().Shipments.FirstOrDefault(s => s.ShipmentId == shipmentId || shipmentId == 0);
                if (shipment == null)
                {
                    throw new InvalidOperationException($"No shipment with matching id {shipmentId}");
                }

                var lineItem = shipment.LineItems.FirstOrDefault(l => l.Code == code);
                if (lineItem != null)
                {
                    shipment.LineItems.Remove(lineItem);
                }

                if (!shipment.LineItems.Any())
                {
                    cart.GetFirstForm().Shipments.Remove(shipment);
                }

                if (!cart.GetFirstForm().GetAllLineItems().Any())
                {
                    _orderRepository.Delete(cart.OrderLink);
                }
            }

            return ValidateCart(cart);
        }

        private static void AddValidationMessagesToResult(AddToCartResult result, ILineItem lineItem, Dictionary<ILineItem, List<ValidationIssue>> validationIssues, bool isHasAddedItem)
        {
            foreach (var validationIssue in validationIssues)
            {
                var warning = new StringBuilder();
                warning.Append(string.Format("Line Item with code {0} ", lineItem.Code));
                validationIssue.Value.Aggregate(warning, (current, issue) => current.Append(issue).Append(", "));

                result.ValidationMessages.Add(warning.ToString().TrimEnd(',', ' '));
            }

            if (!validationIssues.HasItemBeenRemoved(lineItem) && isHasAddedItem)
            {
                result.EntriesAddedToCart = true;
            }
        }

        private Dictionary<ILineItem, List<ValidationIssue>> UpdateLineItemSku(ICart cart, int shipmentId, string oldCode, string newCode, decimal quantity)
        {
            RemoveLineItem(cart, shipmentId, oldCode);

            //merge same sku's
            var newLineItem = GetFirstLineItem(cart, newCode);
            if (newLineItem != null)
            {
                var shipment = cart.GetFirstForm().Shipments.First(s => s.ShipmentId == shipmentId || shipmentId == 0);
                cart.UpdateLineItemQuantity(shipment, newLineItem, newLineItem.Quantity + quantity);
            }
            else
            {
                newLineItem = cart.CreateLineItem(newCode, _orderGroupFactory);
                newLineItem.Quantity = quantity;
                cart.AddLineItem(newLineItem, _orderGroupFactory);

                var price = PriceCalculationService.GetSalePrice(newCode, cart.MarketId, _currentMarket.GetCurrentMarket().DefaultCurrency);
                if (price != null)
                {
                    newLineItem.PlacedPrice = price.UnitPrice.Amount;
                }
            }

            return ValidateCart(cart);
        }

        public Money? GetDiscountedPrice(ICart cart, ILineItem lineItem)
        {
            var marketId = _currentMarket.GetCurrentMarket().MarketId;
            var currency = _currencyService.GetCurrentCurrency();
            if (cart.Name.Equals(DefaultWishListName))
            {
                var discountedPrice = _promotionService.GetDiscountPrice(new CatalogKey(lineItem.Code), marketId, currency);
                return discountedPrice?.UnitPrice;
            }

            return lineItem.GetDiscountedPrice(cart.Currency, _lineItemCalculator);
        }

        public ICart LoadWishListCardByCustomerId(Guid currentContactId)
        {
            var cart = _orderRepository.LoadCart<ICart>(currentContactId, DefaultWishListName, _currentMarket);
            if (cart == null)
            {
                return cart;
            }

            SetCartCurrency(cart, _currencyService.GetCurrentCurrency());

            var validationIssues = ValidateCart(cart);
            // After validate, if there is any change in cart, saving cart.
            if (validationIssues.Any())
            {
                _orderRepository.Save(cart);
            }

            return cart;
        }

        public ICart LoadOrganizationCardByCustomerId(Guid currentContactId)
        {
            var cart = _orderRepository.LoadCart<ICart>(currentContactId, DefaultWishListName, _currentMarket);
            if (cart == null)
            {
                return cart;
            }

            SetCartCurrency(cart, _currencyService.GetCurrentCurrency());

            var validationIssues = ValidateCart(cart);
            // After validate, if there is any change in cart, saving cart.
            if (validationIssues.Any())
            {
                _orderRepository.Save(cart);
            }

            return cart;
        }

        private ILineItem GetFirstLineItem(IOrderGroup cart, string code) => cart.GetAllLineItems().FirstOrDefault(x => x.Code == code);

        private IOrderAddress GetOrderAddressFromWarehosue(ICart cart, IWarehouse warehouse)
        {
            var address = _orderGroupFactory.CreateOrderAddress(cart);
            address.Id = warehouse.Code;
            address.City = warehouse.ContactInformation.City;
            address.CountryCode = warehouse.ContactInformation.CountryCode;
            address.CountryName = warehouse.ContactInformation.CountryName;
            address.DaytimePhoneNumber = warehouse.ContactInformation.DaytimePhoneNumber;
            address.Email = warehouse.ContactInformation.Email;
            address.EveningPhoneNumber = warehouse.ContactInformation.EveningPhoneNumber;
            address.FaxNumber = warehouse.ContactInformation.FaxNumber;
            address.FirstName = warehouse.ContactInformation.FirstName;
            address.LastName = warehouse.ContactInformation.LastName;
            address.Line1 = warehouse.ContactInformation.Line1;
            address.Line2 = warehouse.ContactInformation.Line2;
            address.Organization = warehouse.ContactInformation.Organization;
            address.PostalCode = warehouse.ContactInformation.PostalCode;
            address.RegionName = warehouse.ContactInformation.RegionName;
            address.RegionCode = warehouse.ContactInformation.RegionCode;
            return address;
        }

        public ICart LoadSharedCardByCustomerId(Guid currentContactId)
        {
            var cart = _orderRepository.LoadCart<ICart>(currentContactId, DefaultSharedCartName, _currentMarket);
            if (cart == null)
            {
                return cart;
            }

            SetCartCurrency(cart, _currencyService.GetCurrentCurrency());

            var validationIssues = ValidateCart(cart);
            // After validate, if there is any change in cart, saving cart.
            if (validationIssues.Any())
            {
                _orderRepository.Save(cart);
            }

            return cart;
        }

        public ICart CreateNewCart()
        {
            return _orderRepository.LoadOrCreateCart<ICart>(PrincipalInfo.CurrentPrincipal.GetContactId(),
                DefaultCartName);
        }

        public void DeleteCart(ICart cart) => _orderRepository.Delete(cart.OrderLink);

        public void RemoveQuoteNumber(ICart cart)
        {
            if (cart == null || cart.GetAllLineItems().Any())
            {
                return;
            }

            if (cart.Properties["ParentOrderGroupId"] == null)
            {
                return;
            }

            cart.Properties["ParentOrderGroupId"] = 0;
            _orderRepository.Save(cart);
        }

        public bool PlaceCartForQuote(ICart cart)
        {
            var quoteResult = true;
            try
            {
                foreach (var lineItem in cart.GetFirstForm().GetAllLineItems())
                {
                    lineItem.Properties[Constant.Quote.PreQuotePrice] = lineItem.PlacedPrice;
                }

                var orderReference = _orderRepository.SaveAsPurchaseOrder(cart);
                var purchaseOrder = _orderRepository.Load<PurchaseOrder>(orderReference.OrderGroupId);
                if (purchaseOrder != null)
                {
                    _ = int.TryParse(ConfigurationManager.AppSettings[Constant.Quote.QuoteExpireDate], out var quoteExpireDays);
                    purchaseOrder[Constant.Quote.QuoteExpireDate] =
                        string.IsNullOrEmpty(ConfigurationManager.AppSettings[Constant.Quote.QuoteExpireDate])
                            ? DateTime.Now.AddDays(30)
                            : DateTime.Now.AddDays(quoteExpireDays);

                    purchaseOrder[Constant.Quote.PreQuoteTotal] = purchaseOrder.Total;
                    purchaseOrder[Constant.Quote.QuoteStatus] = Constant.Quote.RequestQuotation;
                    purchaseOrder.Status = OrderStatus.OnHold.ToString();
                    if (string.IsNullOrEmpty(purchaseOrder[Constant.Customer.CustomerFullName]?.ToString()))
                    {
                        if (CustomerContext.Current != null && CustomerContext.Current.CurrentContact != null)
                        {
                            var contact = CustomerContext.Current.CurrentContact;
                            purchaseOrder[Constant.Customer.CustomerFullName] = contact.FullName;
                            purchaseOrder[Constant.Customer.CustomerEmailAddress] = contact.Email;
                            var org = _organizationService.GetCurrentFoundationOrganization();
                            if (org != null)
                            {
                                purchaseOrder[Constant.Customer.CurrentCustomerOrganization] = org.Name;
                            }
                        }
                    }
                }

                _orderRepository.Save(purchaseOrder);
            }
            catch (Exception ex)
            {
                quoteResult = false;
                LogManager.GetLogger(GetType()).Error("Failed to process request quote request.", ex);
            }

            return quoteResult;
        }

        public int PlaceCartForQuoteById(int orderId, Guid userId)
        {
            PurchaseOrder purchaseOrder = null;
            try
            {
                var referedOrder = _orderRepository.Load<IPurchaseOrder>(orderId);
                var cart = _orderRepository.LoadOrCreateCart<ICart>(userId, "RequstQuoteFromOrder");
                foreach (var lineItem in referedOrder.GetFirstForm().GetAllLineItems())
                {
                    var newLineItem = lineItem;
                    newLineItem.Properties[Constant.Quote.PreQuotePrice] = lineItem.PlacedPrice;
                    cart.AddLineItem(newLineItem);
                }

                cart.Currency = referedOrder.Currency;
                cart.MarketId = referedOrder.MarketId;

                var orderReference = _orderRepository.SaveAsPurchaseOrder(cart);
                purchaseOrder = _orderRepository.Load<PurchaseOrder>(orderReference.OrderGroupId);
                if (purchaseOrder != null)
                {
                    _ = int.TryParse(ConfigurationManager.AppSettings[Constant.Quote.QuoteExpireDate], out var quoteExpireDays);
                    purchaseOrder[Constant.Quote.QuoteExpireDate] =
                        string.IsNullOrEmpty(ConfigurationManager.AppSettings[Constant.Quote.QuoteExpireDate])
                            ? DateTime.Now.AddDays(30)
                            : DateTime.Now.AddDays(quoteExpireDays);

                    purchaseOrder[Constant.Quote.PreQuoteTotal] = purchaseOrder.Total;
                    purchaseOrder[Constant.Quote.QuoteStatus] = Constant.Quote.RequestQuotation;
                    purchaseOrder.Status = OrderStatus.OnHold.ToString();
                    if (string.IsNullOrEmpty(purchaseOrder[Constant.Customer.CustomerFullName]?.ToString()))
                    {
                        if (CustomerContext.Current != null && CustomerContext.Current.CurrentContact != null)
                        {
                            var contact = CustomerContext.Current.CurrentContact;
                            purchaseOrder[Constant.Customer.CustomerFullName] = contact.FullName;
                            purchaseOrder[Constant.Customer.CustomerEmailAddress] = contact.Email;
                            var org = _organizationService.GetCurrentFoundationOrganization();
                            if (org != null)
                            {
                                purchaseOrder[Constant.Customer.CurrentCustomerOrganization] = org.Name;
                            }
                        }
                    }
                }

                purchaseOrder.AcceptChanges();
                _orderRepository.Delete(cart.OrderLink);
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(GetType()).Error("Failed to process request quote request.", ex);
            }

            return purchaseOrder?.Id ?? 0;
        }

        public ICart PlaceOrderToCart(IPurchaseOrder purchaseOrder, ICart cart)
        {
            var returnCart = cart;
            var lineItems = purchaseOrder.GetAllLineItems();
            foreach (var lineItem in lineItems)
            {
                cart.AddLineItem(lineItem);
                lineItem.IsInventoryAllocated = false;
            }

            return returnCart;
        }

        public AddToCartResult SeparateShipment(ICart cart, string code, int quantity, int fromShipmentId, int toShipmentId, string deliveryMethodId, string warehouseCode)
        {
            var contentLink = _referenceConverter.GetContentLink(code);
            var entryContent = _contentLoader.Get<EntryContentBase>(contentLink);

            ChangeQuantity(cart, fromShipmentId, code, quantity - 1);
            return AddItemToShipment(cart, entryContent, 1, toShipmentId, deliveryMethodId, warehouseCode);
        }

        private AddToCartResult AddItemToShipment(ICart cart, EntryContentBase entryContent, decimal quantity, int shipmentId, string deliveryMethodId, string warehouseCode)
        {
            var result = new AddToCartResult();

            IWarehouse warehouse = null;

            if (!string.IsNullOrEmpty(warehouseCode))
            {
                warehouse = _warehouseRepository.Get(warehouseCode);
            }

            if (entryContent is BundleContent)
            {
                foreach (var relation in _relationRepository.GetChildren<BundleEntry>(entryContent.ContentLink))
                {
                    var entry = _contentLoader.Get<EntryContentBase>(relation.Child);
                    var recursiveResult = AddItemToShipment(cart, entry, (relation.Quantity ?? 1) * quantity, shipmentId, deliveryMethodId, warehouseCode);
                    if (recursiveResult.EntriesAddedToCart)
                    {
                        result.EntriesAddedToCart = true;
                    }

                    foreach (var message in recursiveResult.ValidationMessages)
                    {
                        result.ValidationMessages.Add(message);
                    }
                }

                return result;
            }

            var form = cart.GetFirstForm();
            if (form == null)
            {
                form = _orderGroupFactory.CreateOrderForm(cart);
                form.Name = cart.Name;
                cart.Forms.Add(form);
            }

            var shipment = form.Shipments.FirstOrDefault(x => x.ShipmentId == shipmentId);
            if (shipment == null)
            {
                shipment = _orderGroupFactory.CreateShipment(cart);
                shipment.WarehouseCode = warehouseCode;
                shipment.ShippingMethodId = new Guid(deliveryMethodId);
                shipment.ShippingAddress = GetOrderAddressFromWarehosue(cart, warehouse);
                cart.GetFirstForm().Shipments.Add(shipment);
            }

            var lineItem = shipment.LineItems.FirstOrDefault(x => x.Code == entryContent.Code);
            decimal originalLineItemQuantity = 0;
            if (lineItem == null)
            {
                lineItem = cart.CreateLineItem(entryContent.Code, _orderGroupFactory);
                lineItem.DisplayName = entryContent.DisplayName;
                lineItem.Quantity = quantity;
                cart.AddLineItem(shipment, lineItem);
            }
            else
            {
                originalLineItemQuantity = lineItem.Quantity;
                cart.UpdateLineItemQuantity(shipment, lineItem, lineItem.Quantity + quantity);
            }

            var validationIssues = ValidateCart(cart);
            var newLineItem = shipment.LineItems.FirstOrDefault(x => x.Code == entryContent.Code);
            var isAdded = (newLineItem != null ? newLineItem.Quantity : 0) - originalLineItemQuantity > 0;

            AddValidationMessagesToResult(result, lineItem, validationIssues, isAdded);

            return result;
        }
    }
}