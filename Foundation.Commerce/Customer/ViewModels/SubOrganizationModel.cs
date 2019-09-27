using Foundation.Cms.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Customer.ViewModels
{
    public class SubOrganizationModel : OrganizationModel
    {
        public SubOrganizationModel(FoundationOrganization organization) : base(organization)
        {
            Name = organization.Name;
            Locations = organization.Addresses != null && organization.Addresses.Any()
                ? organization.Addresses.Select(address => new B2BAddressViewModel(address)).ToList()
                : new List<B2BAddressViewModel>();
        }

        public SubOrganizationModel() => Locations = new List<B2BAddressViewModel>();

        [LocalizedDisplay("/B2B/Organization/SubOrganizationName")]
        [LocalizedRequired("/B2B/Organization/SubOrganizationNameRequired")]
        public new string Name { get; set; }

        public List<B2BAddressViewModel> Locations { get; set; }
        public IEnumerable<CountryViewModel> CountryOptions { get; set; }
    }

    public class SubFoundationOrganizationModel : FoundationOrganization
    {
        public SubFoundationOrganizationModel(FoundationOrganization organization) : base(organization.OrganizationEntity)
        {
            Name = organization.Name;
            Locations = organization.Addresses != null && organization.Addresses.Any()
                ? organization.Addresses.Select(address => new B2BAddressViewModel(address)).ToList()
                : new List<B2BAddressViewModel>();
        }

        [LocalizedDisplay("/B2B/Organization/SubOrganizationName")]
        [LocalizedRequired("/B2B/Organization/SubOrganizationNameRequired")]
        public new string Name { get; set; }

        public List<B2BAddressViewModel> Locations { get; set; }
        public IEnumerable<CountryViewModel> CountryOptions { get; set; }
    }
}