using Foundation.Commerce.Customer;
using Foundation.Commerce.Extensions;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.MyOrganization.SubOrganization;
using Mediachase.Commerce.Customers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.MyOrganization.Organization
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IAddressBookService _addressBookService;
        private readonly CustomerContext _customerContext;

        public OrganizationService(IAddressBookService addressBookService)
        {
            _addressBookService = addressBookService;
            _customerContext = CustomerContext.Current;
        }

        public OrganizationModel GetOrganizationModel(FoundationOrganization organization = null)
        {
            if (organization == null)
            {
                organization = GetCurrentFoundationOrganization();
            }

            if (organization == null)
            {
                return null;
            }

            if (organization.ParentOrganizationId == Guid.Empty)
            {
                return new OrganizationModel(organization);
            }

            var parentOrganization = GetFoundationOrganizationById(organization.ParentOrganizationId.ToString());
            return new OrganizationModel(organization)
            {
                ParentOrganization = new OrganizationModel(parentOrganization),
                ParentOrganizationId = parentOrganization.OrganizationId
            };
        }

        public SubOrganizationModel GetSubOrganizationById(string subOrganizationId)
        {
            var subOrganization = GetFoundationOrganizationById(subOrganizationId);
            if (subOrganization == null)
            {
                return null;
            }

            if (subOrganization.ParentOrganizationId == Guid.Empty)
            {
                return new SubOrganizationModel(subOrganization);
            }

            var parentOrganization = GetFoundationOrganizationById(subOrganization.ParentOrganizationId.ToString());
            return new SubOrganizationModel(subOrganization)
            {
                ParentOrganization = new OrganizationModel(parentOrganization),
                ParentOrganizationId = parentOrganization.OrganizationId
            };
        }

        public SubFoundationOrganizationModel GetSubFoundationOrganizationById(string subOrganizationId)
        {
            var subOrganization = GetFoundationOrganizationById(subOrganizationId);
            if (subOrganization == null)
            {
                return null;
            }

            if (subOrganization.ParentOrganizationId == Guid.Empty)
            {
                return new SubFoundationOrganizationModel(subOrganization);
            }

            var parentOrganization = GetFoundationOrganizationById(subOrganization.ParentOrganizationId.ToString());
            return new SubFoundationOrganizationModel(subOrganization)
            {
                ParentOrganization = parentOrganization,
                ParentOrganizationId = parentOrganization.OrganizationId
            };
        }

        public void UpdateSubOrganization(SubOrganizationModel subOrganizationModel)
        {
            var organization = GetFoundationOrganizationById(subOrganizationModel.OrganizationId.ToString());
            organization.Name = subOrganizationModel.Name;
            organization.SaveChanges();
            foreach (var location in subOrganizationModel.Locations)
            {
                _addressBookService.UpdateOrganizationAddress(organization, location);
            }
        }

        public void CreateOrganization(OrganizationModel organizationInfo)
        {
            var organization = FoundationOrganization.New();
            organization.Name = organizationInfo.Name;
            organization.SaveChanges();

            var contact = GetCurrentContact();
            if (contact != null)
            {
                AddContactToOrganization(organization, contact, B2BUserRoles.Admin);
            }

            _addressBookService.UpdateOrganizationAddress(organization, organizationInfo.Address);
        }

        public void UpdateOrganization(OrganizationModel organizationInfo)
        {
            var organization = GetFoundationOrganizationById(organizationInfo.OrganizationId.ToString());
            organization.Name = organizationInfo.Name;
            organization.SaveChanges();
            _addressBookService.UpdateOrganizationAddress(organization, organizationInfo.Address);
        }

        public void CreateSubOrganization(SubOrganizationModel newSubOrganization)
        {
            var currentOrganization = GetCurrentFoundationOrganization();
            if (currentOrganization == null)
            {
                return;
            }

            var organization = FoundationOrganization.New();
            organization.Name = newSubOrganization.Name;
            organization.ParentOrganizationId = currentOrganization.OrganizationId;
            organization.SaveChanges();

            foreach (var location in newSubOrganization.Locations)
            {
                _addressBookService.UpdateOrganizationAddress(organization, location);
            }
        }

        public string GetUserCurrentOrganizationLocation()
        {
            var currentOrganization = GetCurrentFoundationOrganization();
            if (currentOrganization?.Addresses.FirstOrDefault() == null)
            {
                return string.Empty;
            }

            return currentOrganization.Addresses.First().CountryCode.MarketCodeAdapter();
        }

        public OrganizationModel GetOrganizationModel(Guid id) => new OrganizationModel(GetFoundationOrganizationById(id.ToString()));

        public List<OrganizationModel> GetOrganizationModels()
        {
            return GetOrganizations()
                .Select(x => new OrganizationModel(x))
                .ToList();
        }

        public FoundationOrganization GetCurrentFoundationOrganization() => GetCurrentContact()?.FoundationOrganization;

        public FoundationOrganization GetFoundationOrganizationById(string organizationId)
        {
            if (string.IsNullOrEmpty(organizationId))
            {
                return null;
            }

            var organization = _customerContext.GetOrganizationById(organizationId);
            return organization != null ? new FoundationOrganization(organization) : null;
        }

        public List<FoundationOrganization> GetOrganizations()
        {
            return CustomerContext.Current.GetOrganizations().Where(x => !x.ParentId.HasValue)
                .Select(x => new FoundationOrganization(x))
                .ToList();
        }

        private void AddContactToOrganization(FoundationOrganization organization, FoundationContact contact, B2BUserRoles userRole)
        {
            contact.FoundationOrganization = organization;
            contact.UserRole = userRole.ToString();
            contact.SaveChanges();
        }

        private FoundationContact GetCurrentContact()
        {
            var contact = _customerContext.CurrentContact;
            if (contact == null)
            {
                return null;
            }

            return new FoundationContact(contact);
        }
    }
}