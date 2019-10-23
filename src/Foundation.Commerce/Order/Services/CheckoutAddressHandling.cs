using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Order.ViewModels;
using System;
using System.Linq;

namespace Foundation.Commerce.Order.Services
{
    public class CheckoutAddressHandling
    {
        private readonly IAddressBookService _addressBookService;

        public CheckoutAddressHandling(IAddressBookService addressBookService) => _addressBookService = addressBookService;

        public virtual void UpdateAuthenticatedUserAddresses(CheckoutViewModel viewModel)
        {
            LoadBillingAddressFromAddressBook(viewModel);
            LoadShippingAddressesFromAddressBook(viewModel);
            if (viewModel.UseBillingAddressForShipment)
            {
                viewModel.Shipments.Single().Address = viewModel.BillingAddress;
            }
        }

        public virtual void UpdateAnonymousUserAddresses(CheckoutViewModel viewModel)
        {
            SetDefaultBillingAddressName(viewModel);

            if (viewModel.UseBillingAddressForShipment)
            {
                SetDefaultShippingAddressesNames(viewModel);
                viewModel.Shipments.Single().Address = viewModel.BillingAddress;
            }
        }

        public virtual void ChangeAddress(CheckoutViewModel viewModel, UpdateAddressViewModel updateViewModel)
        {
            var isShippingAddressUpdated = updateViewModel.ShippingAddressIndex > -1;

            var updatedAddress = isShippingAddressUpdated ?
                updateViewModel.Shipments[updateViewModel.ShippingAddressIndex].Address :
                updateViewModel.BillingAddress;

            if (updatedAddress.AddressId != null)
            {
                _addressBookService.LoadAddress(updatedAddress);
            }

            _addressBookService.LoadCountriesAndRegionsForAddress(updatedAddress);

            viewModel.UseBillingAddressForShipment = updateViewModel.UseBillingAddressForShipment;
            viewModel.BillingAddress = updateViewModel.BillingAddress;

            if (isShippingAddressUpdated)
            {
                _addressBookService.LoadAddress(viewModel.BillingAddress);
                _addressBookService.LoadCountriesAndRegionsForAddress(viewModel.BillingAddress);
                _addressBookService.LoadAddress(updatedAddress);
                viewModel.Shipments[updateViewModel.ShippingAddressIndex].Address = updatedAddress;
            }
            else
            {
                for (var i = 0; i < viewModel.Shipments.Count; i++)
                {
                    viewModel.Shipments[i].Address = updateViewModel.Shipments[i].Address;
                }
            }

            foreach (var shipment in viewModel.Shipments)
            {
                _addressBookService.LoadCountriesAndRegionsForAddress(shipment.Address);
            }
        }

        private void SetDefaultBillingAddressName(CheckoutViewModel viewModel)
        {
            if (Guid.TryParse(viewModel.BillingAddress.Name, out var guid))
            {
                viewModel.BillingAddress.Name = "Billing address (" + viewModel.BillingAddress.Line1 + ")";
            }
        }

        private void SetDefaultShippingAddressesNames(CheckoutViewModel viewModel)
        {
            foreach (var address in viewModel.Shipments.Select(x => x.Address))
            {
                if (Guid.TryParse(address.Name, out var guid))
                {
                    address.Name = "Shipping address (" + address.Line1 + ")";
                }
            }
        }

        private void LoadBillingAddressFromAddressBook(CheckoutViewModel checkoutViewModel) => _addressBookService.LoadAddress(checkoutViewModel.BillingAddress);

        private void LoadShippingAddressesFromAddressBook(CheckoutViewModel checkoutViewModel)
        {
            foreach (var shipment in checkoutViewModel.Shipments)
            {
                _addressBookService.LoadAddress(shipment.Address);
            }
        }
    }
}