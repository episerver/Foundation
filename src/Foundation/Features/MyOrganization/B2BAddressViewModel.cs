using Foundation.Cms.Attributes;
using Foundation.Commerce.Customer;
using Foundation.Features.MyAccount.AddressBook;
using System;
using System.Collections.Generic;

namespace Foundation.Features.MyOrganization
{
    public class B2BAddressViewModel
    {
        public B2BAddressViewModel(FoundationAddress address)
        {
            AddressId = address.AddressId;
            Name = address.Name;
            Street = address.Street;
            City = address.City;
            PostalCode = address.PostalCode;
            CountryCode = address.CountryCode;
            CountryName = address.CountryName;
        }

        public B2BAddressViewModel()
        {
        }

        public Guid AddressId { get; set; }

        [LocalizedRequired("/Shared/Address/Form/Empty/Name")]
        [LocalizedDisplay("/Shared/Address/Form/Label/Name")]
        public string Name { get; set; }

        [LocalizedDisplay("/Shared/Address/Form/Label/Line1")]
        [LocalizedRequired("/Shared/Address/Form/Empty/Line1")]
        public string Street { get; set; }

        [LocalizedDisplay("/Shared/Address/Form/Label/City")]
        [LocalizedRequired("/Shared/Address/Form/Empty/City")]
        public string City { get; set; }

        [LocalizedDisplay("/Shared/Address/Form/Label/PostalCode")]
        [LocalizedRequired("/Shared/Address/Form/Empty/PostalCode")]
        public string PostalCode { get; set; }

        [LocalizedDisplay("/Shared/Address/Form/Label/CountryCode")]
        [LocalizedRequired("/Shared/Address/Form/Empty/CountryCode")]
        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        public IEnumerable<CountryViewModel> CountryOptions { get; set; }

        public string AddressString => Street + " " + City + " " + PostalCode + " " + CountryName;
    }
}