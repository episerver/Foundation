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
            viewModel.UseBillingAddressForShipment = updateViewModel.UseBillingAddressForShipment;
            if (!string.IsNullOrEmpty(updateViewModel.AddressId))
            {
                var isShippingAddressUpdated = updateViewModel.AddressType == AddressType.Shipping;
                var updateAddress = _addressBookService.GetAddress(updateViewModel.AddressId);
                _addressBookService.LoadAddress(updateAddress);

                if (isShippingAddressUpdated)
                {
                    viewModel.Shipments[updateViewModel.ShippingAddressIndex].Address = updateAddress;
                }
                else
                {
                    viewModel.BillingAddress = updateAddress;
                }
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