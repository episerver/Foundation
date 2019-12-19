using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Order.ViewModels;
using System;

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
        }

        public virtual void UpdateAnonymousUserAddresses(CheckoutViewModel viewModel)
        {
            SetDefaultBillingAddressName(viewModel);
        }

        public virtual void ChangeAddress(CheckoutViewModel viewModel, UpdateAddressViewModel updateViewModel)
        {
            viewModel.UseShippingingAddressForBilling = updateViewModel.UseBillingAddressForShipment;
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