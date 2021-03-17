using EPiServer.Commerce.Order;
using Foundation.Cms.Extensions;
using Foundation.Commerce.Customer;
using Foundation.Commerce.Customer.Services;
using Foundation.Features.Checkout.ViewModels;
using Foundation.Features.MyOrganization;
using Mediachase.BusinessFoundation.Data;
using Mediachase.BusinessFoundation.Data.Business;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Orders.Dto;
using Mediachase.Commerce.Orders.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.MyAccount.AddressBook
{
    public class AddressBookService : IAddressBookService
    {
        private readonly ICustomerService _customerService;
        private readonly IOrderGroupFactory _orderGroupFactory;

        public AddressBookService(IOrderGroupFactory orderGroupFactory,
            ICustomerService customerService)
        {
            _customerService = customerService;
            _orderGroupFactory = orderGroupFactory;
        }

        public void MapToModel(CustomerAddress customerAddress, AddressModel addressModel)
        {
            var contact = _customerService.GetCurrentContact();

            addressModel.Line1 = customerAddress.Line1;
            addressModel.Line2 = customerAddress.Line2;
            addressModel.City = customerAddress.City;
            addressModel.CountryName = customerAddress.CountryName;
            addressModel.CountryCode = customerAddress.CountryCode;
            addressModel.Email = customerAddress.Email;
            addressModel.FirstName = customerAddress.FirstName;
            addressModel.LastName = customerAddress.LastName;
            addressModel.PostalCode = customerAddress.PostalCode;
            addressModel.AddressId = customerAddress.AddressId.ToString();
            addressModel.Name = customerAddress.Name;
            addressModel.DaytimePhoneNumber = customerAddress.DaytimePhoneNumber;

            addressModel.CountryRegion = new CountryRegionViewModel
            {
                Region = customerAddress.RegionName ?? customerAddress.RegionCode ?? customerAddress.State
            };

            addressModel.ShippingDefault = contact?.Contact.PreferredShippingAddress != null
                                           && customerAddress.Name == contact.Contact.PreferredShippingAddress.Name;

            addressModel.BillingDefault = contact?.Contact.PreferredBillingAddress != null
                                          && customerAddress.Name == contact.Contact.PreferredBillingAddress.Name;
        }

        public void MapToAddress(AddressModel addressModel, IOrderAddress orderAddress)
        {
            orderAddress.Id = addressModel.Name;
            orderAddress.City = addressModel.City;
            orderAddress.CountryCode = addressModel.CountryCode;
            orderAddress.FirstName = addressModel.FirstName;
            orderAddress.LastName = addressModel.LastName;
            orderAddress.Line1 = addressModel.Line1;
            orderAddress.Line2 = addressModel.Line2;
            orderAddress.DaytimePhoneNumber = addressModel.DaytimePhoneNumber;
            orderAddress.PostalCode = addressModel.PostalCode;
            orderAddress.RegionName = addressModel.CountryRegion.Region;
            orderAddress.RegionCode = addressModel.CountryRegion.Region;
            orderAddress.Email = addressModel.Email;
            orderAddress.Organization = addressModel.Organization;

            orderAddress.CountryName = CountryManager.GetCountries().Country
                .Where(x => x.Code == addressModel.CountryCode)
                .Select(x => x.Name)
                .FirstOrDefault();
        }

        public void MapToAddress(AddressModel addressModel, CustomerAddress customerAddress)
        {
            customerAddress.Name = addressModel.Name;
            customerAddress.City = addressModel.City;
            customerAddress.CountryCode = addressModel.CountryCode;
            customerAddress.Email = addressModel.Email;
            customerAddress.FirstName = addressModel.FirstName;
            customerAddress.LastName = addressModel.LastName;
            customerAddress.Line1 = addressModel.Line1;
            customerAddress.Line2 = addressModel.Line2;
            customerAddress.DaytimePhoneNumber = addressModel.DaytimePhoneNumber;
            customerAddress.PostalCode = addressModel.PostalCode;
            customerAddress.RegionName = addressModel.CountryRegion.Region;
            customerAddress.RegionCode = addressModel.CountryRegion.Region;

            customerAddress.CountryName = CountryManager.GetCountries().Country
                .Where(x => x.Code == addressModel.CountryCode)
                .Select(x => x.Name)
                .FirstOrDefault();

            // Commerce Manager expects State to be set for addresses in order management. Set it to be same as
            // RegionName to avoid issues.
            customerAddress.State = addressModel.CountryRegion.Region;

            customerAddress.AddressType =
                CustomerAddressTypeEnum.Public |
                (addressModel.ShippingDefault ? CustomerAddressTypeEnum.Shipping : 0) |
                (addressModel.BillingDefault ? CustomerAddressTypeEnum.Billing : 0);
        }

        public IOrderAddress ConvertToAddress(AddressModel addressModel, IOrderGroup orderGroup)
        {
            var address = orderGroup.CreateOrderAddress(_orderGroupFactory, addressModel.Name);
            MapToAddress(addressModel, address);
            return address;
        }

        public AddressModel ConvertToModel(IOrderAddress orderAddress)
        {
            var address = new AddressModel();
            if (orderAddress != null)
            {
                MapToModel(orderAddress, address);
            }

            return address;
        }

        public IList<AddressModel> MergeAnonymousShippingAddresses(IList<AddressModel> addresses,
            IEnumerable<CartItemViewModel> cartItems)
        {
            var mergedAddresses = new List<AddressModel>(addresses);

            for (var index = addresses.Count - 1; index >= 0; index--)
            {
                var currentAddress = addresses[index];

                foreach (var address in mergedAddresses.Where(x => x != currentAddress))
                {
                    if (address.FirstName == currentAddress.FirstName &&
                        address.LastName == currentAddress.LastName &&
                        address.Line1 == currentAddress.Line1 &&
                        address.Line2 == currentAddress.Line2 &&
                        address.Organization == currentAddress.Organization &&
                        address.PostalCode == currentAddress.PostalCode &&
                        address.City == currentAddress.City &&
                        address.CountryCode == currentAddress.CountryCode &&
                        address.CountryRegion.Region == currentAddress.CountryRegion.Region)
                    {
                        foreach (var item in cartItems.Where(x => x.AddressId == currentAddress.AddressId))
                        {
                            item.AddressId = address.AddressId;
                        }

                        mergedAddresses.Remove(currentAddress);
                        break;
                    }
                }
            }

            return mergedAddresses;
        }

        public AddressCollectionViewModel GetAddressBookViewModel(AddressBookPage addressBookPage)
        {
            return new AddressCollectionViewModel(addressBookPage)
            {
                CurrentContent = addressBookPage,
                Addresses = _customerService.GetCurrentContact()?
                    .Contact?
                    .ContactAddresses
                    .Select(ConvertAddress)
                    ?? Enumerable.Empty<AddressModel>()
            };
        }

        public bool CanSave(AddressModel addressModel)
        {
            return !_customerService.GetCurrentContact()?
                .Contact?
                .ContactAddresses
                .Any(x => x.Name.Equals(addressModel.Name, StringComparison.InvariantCultureIgnoreCase) && x.Name != addressModel.AddressId)
                ?? false;
        }

        public void Save(AddressModel addressModel, FoundationContact contact = null)
        {
            FoundationContact currentContact;
            if (contact != null)
            {
                currentContact = contact;
            }
            else
            {
                currentContact = _customerService.GetCurrentContact();
            }

            if (currentContact == null)
            {
                return;
            }

            var customerAddress = CreateOrUpdateCustomerAddress(currentContact.Contact, addressModel);

            if (addressModel.BillingDefault)
            {
                currentContact.Contact.PreferredBillingAddress = customerAddress;
            }
            else if (currentContact.Contact.PreferredBillingAddress != null &&
                     currentContact.Contact.PreferredBillingAddress.Name.Equals(addressModel.AddressId))
            {
                currentContact.Contact.PreferredBillingAddressId = null;
            }

            if (addressModel.ShippingDefault)
            {
                currentContact.Contact.PreferredShippingAddress = customerAddress;
            }
            else if (currentContact.Contact.PreferredShippingAddress != null &&
                     currentContact.Contact.PreferredShippingAddress.Name.Equals(addressModel.AddressId))
            {
                currentContact.Contact.PreferredShippingAddressId = null;
            }

            currentContact.SaveChanges();
            addressModel.AddressId = customerAddress.AddressId.ToString();
        }

        public void Delete(string addressId)
        {
            var currentContact = _customerService.GetCurrentContact();
            if (currentContact == null)
            {
                return;
            }

            var customerAddress = GetAddress(currentContact.Contact, addressId);
            if (customerAddress == null)
            {
                return;
            }

            if (currentContact.Contact.PreferredBillingAddressId == customerAddress.PrimaryKeyId ||
                currentContact.Contact.PreferredShippingAddressId == customerAddress.PrimaryKeyId)
            {
                currentContact.Contact.PreferredBillingAddressId =
                    currentContact.Contact.PreferredBillingAddressId == customerAddress.PrimaryKeyId
                        ? null
                        : currentContact.Contact.PreferredBillingAddressId;
                currentContact.Contact.PreferredShippingAddressId =
                    currentContact.Contact.PreferredShippingAddressId == customerAddress.PrimaryKeyId
                        ? null
                        : currentContact.Contact.PreferredShippingAddressId;
                currentContact.SaveChanges();
            }

            currentContact.Contact.DeleteContactAddress(customerAddress);
            currentContact.SaveChanges();
        }

        public void SetPreferredBillingAddress(string addressId)
        {
            var currentContact = _customerService.GetCurrentContact();
            if (currentContact == null)
            {
                return;
            }

            var customerAddress = GetAddress(currentContact.Contact, addressId);
            if (customerAddress == null)
            {
                return;
            }

            currentContact.Contact.PreferredBillingAddress = customerAddress;
            currentContact.SaveChanges();
        }

        public void SetPreferredShippingAddress(string addressId)
        {
            var currentContact = _customerService.GetCurrentContact();
            if (currentContact == null)
            {
                return;
            }

            var customerAddress = GetAddress(currentContact.Contact, addressId);
            if (customerAddress == null)
            {
                return;
            }

            currentContact.Contact.PreferredShippingAddress = customerAddress;
            currentContact.SaveChanges();
        }

        public CustomerAddress GetPreferredBillingAddress() => _customerService.GetCurrentContact()?.Contact?.PreferredBillingAddress;

        public void LoadAddress(AddressModel addressModel)
        {
            addressModel.CountryOptions = GetAllCountries();

            var currentContact = _customerService.GetCurrentContact();
            if (currentContact != null)
            {
                if (!string.IsNullOrEmpty(addressModel.AddressId))
                {
                    var existingCustomerAddress = GetAddress(currentContact.Contact, addressModel.AddressId);

                    if (existingCustomerAddress != null)
                    {
                        MapToModel(existingCustomerAddress, addressModel);
                    }
                }
            }

            var countryCode = addressModel.CountryCode;
            if (countryCode.IsNullOrEmpty() && addressModel.CountryOptions.Any())
            {
                countryCode = addressModel.CountryOptions.First().Code;
            }

            if (!string.IsNullOrEmpty(countryCode))
            {
                if (addressModel.CountryRegion == null)
                {
                    addressModel.CountryRegion = new CountryRegionViewModel();
                }

                addressModel.CountryRegion.RegionOptions = GetRegionsByCountryCode(countryCode);
            }
        }

        public IList<AddressModel> List()
        {
            var currentContact = _customerService.GetCurrentContact();
            if (currentContact == null)
            {
                return new List<AddressModel>();
            }

            return currentContact.Contact.ContactAddresses.Select(customerAddress => new AddressModel
            {
                AddressId = customerAddress.Name,
                Name = customerAddress.Name,
                FirstName = customerAddress.FirstName,
                LastName = customerAddress.LastName,
                Line1 = customerAddress.Line1,
                Line2 = customerAddress.Line2,
                PostalCode = customerAddress.PostalCode,
                City = customerAddress.City,
                CountryCode = customerAddress.CountryCode,
                CountryName = customerAddress.CountryName,
                CountryRegion = new CountryRegionViewModel
                {
                    Region = customerAddress.RegionName ?? customerAddress.RegionCode ?? customerAddress.State
                },
                Email = customerAddress.Email,
                ShippingDefault = currentContact.Contact.PreferredShippingAddress != null
                                    && customerAddress.AddressId == currentContact.Contact.PreferredShippingAddressId,
                BillingDefault = currentContact.Contact.PreferredBillingAddress != null
                                    && customerAddress.AddressId == currentContact.Contact.PreferredBillingAddressId
            }).ToList();
        }

        public IEnumerable<string> GetRegionsByCountryCode(string countryCode)
        {
            var country = CountryManager.GetCountry(countryCode, false)?.Country?.FirstOrDefault();
            return country != null ? GetRegionsForCountry(country) : Enumerable.Empty<string>();
        }

        public void LoadCountriesAndRegionsForAddress(AddressModel addressModel)
        {
            addressModel.CountryOptions = GetAllCountries();

            // Try get the address country first by country code, then by name, else use the first in list as final fallback.
            var selectedCountry = (GetCountryByCode(addressModel) ??
                                   GetCountryByName(addressModel)) ??
                                  addressModel.CountryOptions.FirstOrDefault();

            addressModel.CountryRegion.RegionOptions = selectedCountry != null
                ? GetRegionsByCountryCode(selectedCountry.Code)
                : Enumerable.Empty<string>();
        }

        public bool UseBillingAddressForShipment()
        {
            var customer = _customerService.GetCurrentContact();
            if (customer == null)
            {
                return false;
            }

            return customer.Contact.PreferredShippingAddressId.HasValue &&
                   customer.Contact.PreferredShippingAddressId == customer.Contact.PreferredBillingAddressId;
        }

        public void MapToModel(IOrderAddress orderAddress, AddressModel addressModel)
        {
            if (orderAddress == null)
            {
                return;
            }

            addressModel.AddressId = orderAddress.Id;
            addressModel.Name = orderAddress.Id;
            addressModel.Line1 = orderAddress.Line1;
            addressModel.Line2 = orderAddress.Line2;
            addressModel.City = orderAddress.City;
            addressModel.CountryName = orderAddress.CountryName;
            addressModel.CountryCode = orderAddress.CountryCode;
            addressModel.Email = orderAddress.Email;
            addressModel.FirstName = orderAddress.FirstName;
            addressModel.LastName = orderAddress.LastName;
            addressModel.PostalCode = orderAddress.PostalCode;
            addressModel.Organization = orderAddress.Organization;
            addressModel.CountryRegion = new CountryRegionViewModel
            {
                Region = orderAddress.RegionName ?? orderAddress.RegionCode
            };
            addressModel.DaytimePhoneNumber = orderAddress.DaytimePhoneNumber;
        }

        public void UpdateOrganizationAddress(FoundationOrganization organization, B2BAddressViewModel addressModel)
        {
            var address = GetOrganizationAddress(organization.OrganizationEntity, addressModel.AddressId) ??
                          CreateAddress();

            address.OrganizationId = organization.OrganizationId;
            address.Name = addressModel.Name;
            address.Street = addressModel.Street;
            address.City = addressModel.City;
            address.PostalCode = addressModel.PostalCode;
            address.CountryCode = addressModel.CountryCode;
            address.CountryName = GetCountryNameByCode(addressModel.CountryCode);

            address.SaveChanges();
        }

        public IEnumerable<CountryViewModel> GetAllCountries()
        {
            var countries = GetCountries();
            return countries.Country.Select(x => new CountryViewModel { Code = x.Code, Name = x.Name });
        }

        public string GetCountryNameByCode(string code)
        {
            var countryOptions =
                GetCountries().Country.Select(x => new CountryViewModel { Code = x.Code, Name = x.Name });
            var selectedCountry = countryOptions.FirstOrDefault(x => x.Code == code);
            return selectedCountry?.Name;
        }

        public void DeleteAddress(string organizationId, string addressId)
        {
            var organization = GetFoundationOrganizationById(organizationId);
            if (organization == null)
            {
                return;
            }

            var address = GetOrganizationAddress(organization.OrganizationEntity, new Guid(addressId));
            address?.Address?.Delete();
        }

        public AddressModel GetAddress(string addressId)
        {
            var currentContact = _customerService.GetCurrentContact();
            var model = new AddressModel();
            if (currentContact != null)
            {
                var address = currentContact.Contact.ContactAddresses.FirstOrDefault(x => x.Name == addressId);
                MapToModel(address, model);
            }

            return model;
        }

        private FoundationAddress CreateAddress()
        {
            var address = new FoundationAddress(CustomerAddress.CreateInstance());
            address.AddressId = BusinessManager.Create(address.Address);
            return address;
        }

        private FoundationAddress GetOrganizationAddress(Organization organization, Guid addressId)
        {
            var organizationAddresses = CustomerContext.Current.GetAddressesInOrganization(organization);
            var organizationAddress = organizationAddresses.FirstOrDefault(address => address.AddressId == addressId);
            return organizationAddress != null ? new FoundationAddress(organizationAddress) : null;
        }

        private CountryDto GetCountries() => CountryManager.GetCountries();

        private CountryViewModel GetCountryByCode(AddressModel addressModel)
        {
            var selectedCountry = addressModel.CountryOptions.FirstOrDefault(x => x.Code == addressModel.CountryCode);
            if (selectedCountry != null)
            {
                addressModel.CountryName = selectedCountry.Name;
            }

            return selectedCountry;
        }

        private CountryViewModel GetCountryByName(AddressModel addressModel)
        {
            var selectedCountry = addressModel.CountryOptions.FirstOrDefault(x => x.Name == addressModel.CountryName);
            if (selectedCountry != null)
            {
                addressModel.CountryCode = selectedCountry.Code;
            }

            return selectedCountry;
        }

        private IEnumerable<string> GetRegionsForCountry(CountryDto.CountryRow country)
        {
            return country == null
                ? Enumerable.Empty<string>()
                : country.GetStateProvinceRows().Select(x => x.Name).ToList();
        }

        private CustomerAddress CreateOrUpdateCustomerAddress(CustomerContact contact, AddressModel addressModel)
        {
            var customerAddress = GetAddress(contact, addressModel.AddressId);
            var isNew = customerAddress == null;
            IEnumerable<PrimaryKeyId> existingId = contact.ContactAddresses.Select(a => a.AddressId).ToList();
            if (isNew)
            {
                customerAddress = CustomerAddress.CreateInstance();
            }

            MapToAddress(addressModel, customerAddress);

            if (isNew)
            {
                contact.AddContactAddress(customerAddress);
            }
            else
            {
                contact.UpdateContactAddress(customerAddress);
            }

            contact.SaveChanges();
            if (isNew)
            {
                customerAddress.AddressId = contact.ContactAddresses
                    .Where(a => !existingId.Contains(a.AddressId))
                    .Select(a => a.AddressId)
                    .Single();
                addressModel.AddressId = customerAddress.Name;
            }

            return customerAddress;
        }

        public AddressModel ConvertAddress(CustomerAddress customerAddress)
        {
            AddressModel addressModel = null;

            if (customerAddress != null)
            {
                addressModel = new AddressModel();
                MapToModel(customerAddress, addressModel);
            }

            return addressModel;
        }

        private CustomerAddress GetAddress(CustomerContact contact, string addressId) => contact.ContactAddresses.FirstOrDefault(x => x.Name == addressId || x.AddressId.ToString() == addressId);

        private FoundationOrganization GetFoundationOrganizationById(string organizationId)
        {
            if (string.IsNullOrEmpty(organizationId))
            {
                return null;
            }

            var organization = CustomerContext.Current.GetOrganizationById(organizationId);
            return organization != null ? new FoundationOrganization(organization) : null;
        }
    }
}