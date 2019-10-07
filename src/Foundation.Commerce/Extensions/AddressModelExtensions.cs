using Foundation.Commerce.Customer.ViewModels;

namespace Foundation.Commerce.Extensions
{
    public static class AddressModelExtensions
    {
        public static bool IsEqual(this AddressModel address,
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