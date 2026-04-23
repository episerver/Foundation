using Foundation.Features.Checkout.Services;
using Foundation.Features.MyAccount.AddressBook;

namespace Foundation.Features.Checkout.ViewModels
{
    public class MultiShipmentViewModelFactory
    {
        private readonly LocalizationService _localizationService;
        private readonly IAddressBookService _addressBookService;
        private readonly UrlResolver _urlResolver;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ShipmentViewModelFactory _shipmentViewModelFactory;

        public MultiShipmentViewModelFactory(
            LocalizationService localizationService,
            IAddressBookService addressBookService,
            UrlResolver urlResolver,
            IHttpContextAccessor httpContextAccessor,
            ShipmentViewModelFactory shipmentViewModelFactory)
        {
            _localizationService = localizationService;
            _addressBookService = addressBookService;
            _urlResolver = urlResolver;
            _httpContextAccessor = httpContextAccessor;
            _shipmentViewModelFactory = shipmentViewModelFactory;
        }

        //public virtual MultiShipmentViewModel CreateMultiShipmentViewModel(ICart cart, MultiShipmentPage multiShipmentPage, bool isAuthenticated)
        //{
        //    var viewModel = new MultiShipmentViewModel(multiShipmentPage)
        //    {
        //        AvailableAddresses = GetAvailableShippingAddresses(cart),
        //        CartItems = cart != null ? FlattenCartItems(_shipmentViewModelFactory.CreateShipmentsViewModel(cart).SelectMany(x => x.CartItems)) : new CartItemViewModel[0],
        //        ReferrerUrl = GetReferrerUrl()
        //    };

        //    if (!isAuthenticated)
        //    {
        //        UpdateShippingAddressesForAnonymous(viewModel);
        //    }

        //    return viewModel;
        //}

        public virtual MultiAddressViewModel CreateMultiShipmentViewModel(ICart cart, CheckoutPage checkoutPage, bool isAuthenticated)
        {
            var viewModel = new MultiAddressViewModel(checkoutPage)
            {
                AvailableAddresses = GetAvailableShippingAddresses(cart),
                CartItems = cart != null ? FlattenCartItems(_shipmentViewModelFactory.CreateShipmentsViewModel(cart).SelectMany(x => x.CartItems)) : Array.Empty<CartItemViewModel>(),
                ReferrerUrl = GetReferrerUrl(),
            };

            if (!isAuthenticated && !viewModel.AvailableAddresses.Any())
            {
                UpdateShippingAddressesForAnonymous(viewModel);
            }

            return viewModel;
        }

        private IList<AddressModel> GetAvailableShippingAddresses(ICart cart)
        {
            var addresses = _addressBookService.List();
            foreach (var address in addresses.Where(x => string.IsNullOrEmpty(x.Name)))
            {
                address.Name = _localizationService.GetString("/Shared/Address/DefaultAddressName");
            }

            if (cart != null)
            {
                foreach (var shipment in cart.GetFirstForm().Shipments)
                {
                    if (shipment.ShippingAddress == null)
                    {
                        continue;
                    }

                    var shipmentAddress = _addressBookService.ConvertToModel(shipment.ShippingAddress);
                    var savedAddress = addresses.FirstOrDefault(x => IsEqual(x, shipmentAddress));
                    if (savedAddress != null)
                    {
                        continue;
                    }

                    if (addresses.Any(x => x.AddressId.Equals(shipmentAddress.AddressId)))
                    {
                        shipmentAddress.AddressId = shipmentAddress.Name = Guid.NewGuid().ToString();
                    }

                    addresses.Add(shipmentAddress);
                }
            }

            return addresses;
        }

        //private void UpdateShippingAddressesForAnonymous(MultiShipmentViewModel viewModel)
        //{
        //    foreach (var item in viewModel.CartItems)
        //    {
        //        var anonymousShippingAddress = new AddressModel
        //        {
        //            AddressId = Guid.NewGuid().ToString(),
        //            Name = Guid.NewGuid().ToString(),
        //            CountryCode = "USA"
        //        };

        //        item.AddressId = anonymousShippingAddress.AddressId;
        //        _addressBookService.LoadCountriesAndRegionsForAddress(anonymousShippingAddress);
        //        viewModel.AvailableAddresses.Add(anonymousShippingAddress);
        //    }
        //}

        private void UpdateShippingAddressesForAnonymous(MultiAddressViewModel viewModel)
        {
            foreach (var item in viewModel.CartItems)
            {
                var anonymousShippingAddress = new AddressModel
                {
                    AddressId = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString(),
                    CountryCode = "USA"
                };

                item.AddressId = anonymousShippingAddress.AddressId;
                _addressBookService.LoadCountriesAndRegionsForAddress(anonymousShippingAddress);
                viewModel.AvailableAddresses.Add(anonymousShippingAddress);
            }
        }

        private string GetReferrerUrl()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var urlReferer = httpContext.Request.Headers["UrlReferrer"].ToString();
            var hostUrlReferer = string.IsNullOrEmpty(urlReferer) ? "" : new Uri(urlReferer).Host;
            if (urlReferer != null && hostUrlReferer.Equals(httpContext.Request.Host.Host.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return urlReferer;
            }

            return _urlResolver.GetUrl(ContentReference.StartPage);
        }

        private CartItemViewModel[] FlattenCartItems(IEnumerable<CartItemViewModel> cartItems)
        {
            var list = new List<CartItemViewModel>();

            foreach (var item in cartItems)
            {
                for (var i = 0; i < item.Quantity; i++)
                {
                    list.Add(new CartItemViewModel
                    {
                        Quantity = 1,
                        AvailableSizes = item.AvailableSizes,
                        Brand = item.Brand,
                        DisplayName = item.DisplayName,
                        Code = item.Code,
                        ImageUrl = item.ImageUrl,
                        IsAvailable = item.IsAvailable,
                        PlacedPrice = item.PlacedPrice,
                        AddressId = item.AddressId,
                        Url = item.Url,
                        Entry = item.Entry,
                        DiscountedUnitPrice = item.DiscountedUnitPrice,
                        DiscountedPrice = item.DiscountedPrice,
                        IsGift = item.IsGift
                    });
                }
            }

            return list.ToArray();
        }

        public bool IsEqual(AddressModel address,
           AddressModel compareAddressViewModel)
        {
            return address.FirstName == compareAddressViewModel.FirstName &&
                   address.LastName == compareAddressViewModel.LastName &&
                   address.Line1 == compareAddressViewModel.Line1 &&
                   address.Line2 == compareAddressViewModel.Line2 &&
                   address.Organization == compareAddressViewModel.Organization &&
                   address.PostalCode == compareAddressViewModel.PostalCode &&
                   address.City == compareAddressViewModel.City &&
                   address.CountryCode == compareAddressViewModel.CountryCode &&
                   address.CountryRegion.Region == compareAddressViewModel.CountryRegion.Region;
        }
    }
}