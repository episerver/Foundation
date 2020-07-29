using Foundation.Cms.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.MyAccount.AddressBook
{
    public class AddressModel
    {
        public AddressModel()
        {
            CountryRegion = new CountryRegionViewModel();
        }

        public string AddressId { get; set; }

        [LocalizedRequired("/Shared/Address/Form/Empty/Name")]
        [LocalizedDisplay("/Shared/Address/Form/Label/Name")]
        public string Name { get; set; }

        [LocalizedDisplay("/Shared/Address/Form/Label/FirstName")]
        [LocalizedRequired("/Shared/Address/Form/Empty/FirstName")]
        public string FirstName { get; set; }

        [LocalizedDisplay("/Shared/Address/Form/Label/LastName")]
        [LocalizedRequired("/Shared/Address/Form/Empty/LastName")]
        public string LastName { get; set; }

        public string CountryName { get; set; }

        [LocalizedDisplay("/Shared/Address/Form/Label/CountryCode")]
        [LocalizedRequired("/Shared/Address/Form/Empty/CountryCode")]
        public string CountryCode { get; set; }

        public IEnumerable<CountryViewModel> CountryOptions { get; set; }

        [LocalizedDisplay("/Shared/Address/Form/Label/City")]
        [LocalizedRequired("/Shared/Address/Form/Empty/City")]
        public string City { get; set; }

        [LocalizedDisplay("/Shared/Address/Form/Label/PostalCode")]
        [LocalizedRequired("/Shared/Address/Form/Empty/PostalCode")]
        public string PostalCode { get; set; }

        [LocalizedDisplay("/Shared/Address/Form/Label/Line1")]
        [LocalizedRequired("/Shared/Address/Form/Empty/Line1")]
        public string Line1 { get; set; }

        [LocalizedDisplay("/Shared/Address/Form/Label/Line2")]
        public string Line2 { get; set; }

        [UIHint("AddressRegion")]
        public CountryRegionViewModel CountryRegion { get; set; }

        [LocalizedDisplay("/Shared/Address/Form/Label/Email")]
        [LocalizedEmail("/Shared/Address/Form/Error/InvalidEmail")]
        public string Email { get; set; }

        [LocalizedDisplay("/Shared/Address/Form/Label/ShippingAddress")]
        public bool ShippingDefault { get; set; }

        [LocalizedDisplay("/Shared/Address/Form/Label/BillingAddress")]
        public bool BillingDefault { get; set; }

        [LocalizedDisplay("/Shared/Address/Form/Label/DaytimePhoneNumber")]
        public string DaytimePhoneNumber { get; set; }

        [LocalizedDisplay("/Shared/Address/Form/Label/Organization")]
        public string Organization { get; set; }

        public string ErrorMessage { get; set; }

        public string MultipleAddressLabel => Line1 + ", " + City + ", " + CountryRegion.Region + ", " + PostalCode;
    }
}