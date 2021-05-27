using EPiServer.Commerce.Order;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Customers;

namespace Foundation.Commerce.Extensions
{
    public static class CustomerAddressExtensions
    {
        private static readonly Injected<IOrderGroupFactory> Factory = default;

        public static IOrderAddress ConvertToOrderAddress(this CustomerAddress address, IOrderGroup order)
        {
            var newAddress = Factory.Service.CreateOrderAddress(order);
            newAddress.City = address.City;
            newAddress.CountryCode = address.CountryCode;
            newAddress.CountryName = address.CountryName;
            newAddress.DaytimePhoneNumber = address.DaytimePhoneNumber;
            newAddress.Email = address.Email;
            newAddress.EveningPhoneNumber = address.EveningPhoneNumber;
            newAddress.FirstName = address.FirstName;
            newAddress.LastName = address.LastName;
            newAddress.Line1 = address.Line1;
            newAddress.Line2 = address.Line2;
            newAddress.Id = address.Name;
            newAddress.PostalCode = address.PostalCode;
            newAddress.RegionName = address.RegionName;
            newAddress.RegionCode = address.RegionCode;
            return newAddress;
        }
    }
}