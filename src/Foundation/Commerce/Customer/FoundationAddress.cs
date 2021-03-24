using Mediachase.BusinessFoundation.Data;
using Mediachase.BusinessFoundation.Data.Business;
using Mediachase.Commerce.Customers;
using System;

namespace Foundation.Commerce.Customer
{
    public class FoundationAddress
    {
        public FoundationAddress(CustomerAddress customerAddress) => Address = customerAddress;

        public CustomerAddress Address { get; set; }

        public Guid AddressId
        {
            get => Address.AddressId;
            set => Address.AddressId = (PrimaryKeyId)value;
        }

        public string Name
        {
            get => Address.Name;
            set => Address.Name = value;
        }

        public string Street
        {
            get => Address.Line1;
            set => Address.Line1 = value;
        }

        public string City
        {
            get => Address.City;
            set => Address.City = value;
        }

        public string PostalCode
        {
            get => Address.PostalCode;
            set => Address.PostalCode = value;
        }

        public string CountryCode
        {
            get => Address.CountryCode;
            set => Address.CountryCode = value;
        }

        public string CountryName
        {
            get => Address.CountryName;
            set => Address.CountryName = value;
        }

        public Guid OrganizationId
        {
            get => Address.OrganizationId ?? Guid.Empty;
            set => Address.OrganizationId = (PrimaryKeyId?)value;
        }

        public void SaveChanges() => BusinessManager.Update(Address);
    }
}