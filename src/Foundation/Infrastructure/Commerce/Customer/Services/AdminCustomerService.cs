using EPiServer.Commerce.UI.Admin.Shared.Models;
using EPiServer.Framework.Localization;
using EPiServer.Security;
using EPiServer.Shell.Security;
using Mediachase.BusinessFoundation.Data.Business;
using Mediachase.BusinessFoundation.Data.Meta.Management;
using Mediachase.BusinessFoundation.Data.Meta;
using Mediachase.BusinessFoundation.Data;
using Mediachase.Commerce.Customers.Request;
using Mediachase.Commerce.Customers;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using EPiServer.Commerce.UI.Admin.Customers.Internal;
using Mediachase.Commerce.Security;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Infrastructure.Commerce.Customer.Services
{
    internal class AdminCustomerService : EPiServer.Commerce.UI.Admin.Customers.Internal.ICustomerService
    {
        private readonly CustomerOptions _customerOptions;
        private static readonly ILogger _log = new LoggerFactory().CreateLogger<CustomerService>();
        private readonly LocalizationService _localizationService;
        private readonly UIUserProvider _uiUserProvider;

        public AdminCustomerService(IOptions<CustomerOptions> customerOptions,
            LocalizationService localizationService,
            UIUserProvider uIUserProvider)
        {
            _customerOptions = customerOptions.Value;
            _localizationService = localizationService;
            _uiUserProvider = uIUserProvider;
        }

        #region Contacts
        public ContactViewModel GetContactById(Guid contactId)
        {
            var contact = (CustomerContact)BusinessManager.Load(CustomerContact.ClassName, new PrimaryKeyId(contactId));
            var result = contact.ToContactViewModel();
            result.PreferredShippingAddressId = result.PreferredShippingAddressId == null ? Guid.Empty : result.PreferredShippingAddressId;
            result.PreferredBillingAddressId = result.PreferredBillingAddressId == null ? Guid.Empty : result.PreferredBillingAddressId;

            result.ContactNotes = ListContactNotes(contactId);
            var noValue = new AddressViewModel
            {
                Name = "[ No value ]",
                PrimaryKeyId = Guid.Empty
            };
            result.Addresses = GetAddressesInContact(contactId);
            result.Addresses = result.Addresses.Prepend(noValue);
            return result;
        }

        public ContactEntity AddOrUpdateContact(ContactViewModel contactViewModel)
        {
            var isContactUpdate = contactViewModel.PrimaryKeyId.HasValue && contactViewModel.PrimaryKeyId != Guid.Empty;
            var contactId = isContactUpdate ? contactViewModel.PrimaryKeyId.Value : Guid.Empty;
            if (ContactExists(contactId, contactViewModel.Email))
            {
                var message = string.Format(_localizationService.GetString("/episerver.commerce.ui.admin/contact/duplicate_email"), contactViewModel.Email);
                throw new Exception(message);
            }

            ContactEntity contact;
            if (isContactUpdate)
            {
                var allAddresses = GetAddressesInContact(contactId);
                var sameAddress = contactViewModel.PreferredShippingAddressId == contactViewModel.PreferredBillingAddressId;
                contactViewModel.Addresses = contactViewModel.Addresses.Where(x => x.PrimaryKeyId != Guid.Empty);
                contactViewModel.PreferredShippingAddressId = contactViewModel.PreferredShippingAddressId == Guid.Empty ? null : contactViewModel.PreferredShippingAddressId;
                contactViewModel.PreferredBillingAddressId = contactViewModel.PreferredBillingAddressId == Guid.Empty ? null : contactViewModel.PreferredBillingAddressId;

                //If select a new address as Shippping Address, save it firstly, then update Contact
                if (contactViewModel.PreferredShippingAddressId != null && !allAddresses.Any(x => x.PrimaryKeyId == contactViewModel.PreferredShippingAddressId))
                {
                    var shippingAddress = contactViewModel.Addresses.FirstOrDefault(x => x.PrimaryKeyId == contactViewModel.PreferredShippingAddressId);
                    if (shippingAddress != null)
                    {
                        shippingAddress.PrimaryKeyId = null;
                        var newId = AddOrUpdateAddress(shippingAddress);
                        if (sameAddress)
                        {
                            contactViewModel.PreferredShippingAddressId = contactViewModel.PreferredBillingAddressId = Guid.Parse(newId);
                        }
                        else
                        {
                            contactViewModel.PreferredShippingAddressId = Guid.Parse(newId);
                        }
                    }
                }

                //If select a new address as Billing Address, save it firstly, then update Contact
                if (contactViewModel.PreferredBillingAddressId != null && !sameAddress && !allAddresses.Any(x => x.PrimaryKeyId == contactViewModel.PreferredBillingAddressId))
                {
                    var billingAddress = contactViewModel.Addresses.FirstOrDefault(x => x.PrimaryKeyId == contactViewModel.PreferredBillingAddressId);
                    if (billingAddress != null)
                    {
                        billingAddress.PrimaryKeyId = null;
                        var newId = AddOrUpdateAddress(billingAddress);
                        contactViewModel.PreferredBillingAddressId = Guid.Parse(newId);
                    }
                }

                contact = contactViewModel.ToContactEntity((CustomerContact)BusinessManager.Load(ContactEntity.ClassName, new PrimaryKeyId(contactViewModel.PrimaryKeyId.Value)));
                BusinessManager.Update(contact);
                contactViewModel.Addresses = contactViewModel.Addresses.Where(x => x.PrimaryKeyId != null);

                //Update contact notes
                var allContactNotes = ListContactNotes(contactId);
                if (contactViewModel.ContactNotes.Any())
                {
                    var deleteContactNotes = allContactNotes.Where(x => !contactViewModel.ContactNotes.Any(z => z.ContactNoteId == x.ContactNoteId));

                    //Delete
                    if (deleteContactNotes.Any())
                    {
                        var contactNoteIds = deleteContactNotes.Select(x => x.ContactNoteId ?? Guid.Empty).ToArray();
                        DeleteContactNotes(contactNoteIds);
                    }
                    //Add and update
                    foreach (var note in contactViewModel.ContactNotes)
                    {
                        var isNew = allContactNotes.All(x => x.ContactNoteId != note.ContactNoteId);
                        if (isNew)
                        {
                            note.ContactNoteId = null;
                        }
                        AddOrUpdateContactNote(note);
                    }
                }
                else
                {
                    if (allContactNotes != null && allContactNotes.Any())
                    {
                        var contactNoteIds = allContactNotes.Select(x => x.ContactNoteId ?? Guid.Empty).ToArray();
                        DeleteContactNotes(contactNoteIds);
                    }
                }

                //Update address
                if (contactViewModel.Addresses.Any())
                {
                    UpdateAddressesToOrganization(contactViewModel.Addresses, allAddresses);
                }
            }
            else
            {
                contact = contactViewModel.ToContactEntity(BusinessManager.InitializeEntity<ContactEntity>(ContactEntity.ClassName));
                contactId = BusinessManager.Create(contact);
            }
            return contact;
        }

        public void DeleteContact(Guid contactId)
        {
            var contact = CustomerContext.Current.GetContactById(contactId);
            contact.DeleteWithAllDependents();
        }

        public ContactListViewModel ListContacts(int startIndex, int recordsToRetrieve)
        {
            return new ContactListViewModel
            {
                Contacts = BusinessManager.List(ContactEntity.ClassName, new FilterElement[0], new SortingElement[0], startIndex, recordsToRetrieve)
                .OfType<CustomerContact>()
                .Select(x => x.ToContactViewModel()),
                PagingInfo = new PagingInfo
                {
                    StartRow = startIndex,
                    RowsPerPage = recordsToRetrieve,
                    TotalRowCount = GetTotalContacts(),
                    SearchInput = "",
                },
            };
        }

        [Obsolete]
        public ContactListViewModel SearchContacts(string query, int startIndex, int recordsToRetrieve)
        {
            var contacts = GetContactsByPattern(query, startIndex, recordsToRetrieve, out var totalCount)
                .Select(x => x.ToContactViewModel());

            return new ContactListViewModel
            {
                PagingInfo = new PagingInfo
                {
                    StartRow = startIndex,
                    RowsPerPage = recordsToRetrieve,
                    TotalRowCount = totalCount,
                    SearchInput = string.IsNullOrEmpty(query) ? "" : query,
                },
                Contacts = contacts
            };
        }

        private static bool ContactExists(Guid contactId, string contactEmail)
        {
            var contacts = CustomerContext.Current.GetContactsByPattern(contactEmail);
            return contactId == Guid.Empty
                ? contacts.Any(c => c.Email.Equals(contactEmail, StringComparison.OrdinalIgnoreCase))
                : contacts.Any(c => c.PrimaryKeyId != contactId && c.Email.Equals(contactEmail, StringComparison.OrdinalIgnoreCase));
        }

        private void UpdateAddressesToOrganization(IEnumerable<AddressViewModel> addresses, IEnumerable<AddressViewModel> allAddresses)
        {
            if (addresses.Any())
            {
                var deleteAddresses = allAddresses.Where(x => !addresses.Any(z => z.PrimaryKeyId != null && z.PrimaryKeyId == x.PrimaryKeyId));
                //Delete
                if (deleteAddresses.Any())
                {
                    DeleteAddresses(deleteAddresses);
                }

                //Add and update
                foreach (var address in addresses)
                {
                    var isNew = allAddresses.All(x => address.PrimaryKeyId != null && x.PrimaryKeyId != address.PrimaryKeyId);
                    if (isNew)
                    {
                        address.PrimaryKeyId = null;
                    }
                    AddOrUpdateAddress(address);
                }
            }
            else
            {
                DeleteAddresses(allAddresses);
            }
        }

        private void DeleteAddresses(IEnumerable<AddressViewModel> addresses)
        {
            if (addresses != null && addresses.Any())
            {
                var addressIds = addresses.Select(x => x.PrimaryKeyId ?? Guid.Empty).ToList();
                foreach (var id in addressIds)
                {
                    DeleteAddress(id);
                }
            }
        }

        private IEnumerable<CustomerContact> GetContactsByPattern(string pattern, int startIndex, int recordsToRetrieve, out int totalCount)
        {
            var cacheKey = CustomersCache.CreateCacheKey(ContactEntity.ClassName, string.Empty, $"GetContactsByPattern:{startIndex}:{recordsToRetrieve}", pattern);

            var contacts = CustomersCache.ReadThrough(cacheKey, null, _customerOptions.Cache.ContactCollectionCacheExpiration,
                () =>
                {
                    if (!string.IsNullOrEmpty(pattern))
                    {
                        var textPropertyFilters = CustomerContact.TextProperties.Select(x => new FilterElement(x, FilterElementType.Contains, pattern));
                        var orBlock = new OrBlockFilterElement(textPropertyFilters.ToArray());
                        var filter = new FilterElement[] { orBlock };
                        return BusinessManager.List(ContactEntity.ClassName, filter, Array.Empty<SortingElement>(), startIndex, recordsToRetrieve)
                        .OfType<CustomerContact>()
                        .ToList();
                    }
                    else
                    {
                        return BusinessManager.List(ContactEntity.ClassName, new FilterElement[0], new SortingElement[0], startIndex, recordsToRetrieve)
                        .OfType<CustomerContact>()
                        .ToList();
                    }
                });


            totalCount = GetTotalContacts(string.IsNullOrEmpty(pattern) ? null :
                 new FilterElement[]
                 {
                    new OrBlockFilterElement(CustomerContact.TextProperties.Select(x => new FilterElement(x, FilterElementType.Contains, pattern)).ToArray())
                    {
                        Source = pattern
                    }
                 });
            return contacts;
        }
        #endregion

        #region Contact notes
        private const string ContactNoteMetaClass = "ContactNote";
        private const string CreatedField = "Created";
        private const string ModifiedField = "Modified";
        private const string NoteTitleField = "NoteTitle";
        private const string NoteContentField = "NoteContent";
        private const string ContactIdField = "ContactId";
        private const string CreatorIdField = "CreatorId";
        private const string ModifierIdField = "ModifierId";

        public IEnumerable<ContactNoteViewModel> ListContactNotes(Guid contactId)
        {
            var contactNotes = BusinessManager.List(ContactNoteMetaClass, new[]
            {
                FilterElement.EqualElement(ContactIdField, contactId)
            });

            foreach (var contactNote in contactNotes.OrderBy(x => x.Properties[CreatedField].Value))
            {
                yield return new ContactNoteViewModel
                {
                    ContactNoteId = new Guid(contactNote.PrimaryKeyId.ToString()),
                    Created = DateTime.Parse(contactNote[CreatedField].ToString()).ToLocalTime(),
                    Modified = DateTime.Parse(contactNote[ModifiedField].ToString()).ToLocalTime(),
                    NoteTitle = contactNote[NoteTitleField].ToString(),
                    NoteContent = contactNote[NoteContentField].ToString(),
                    ContactId = new Guid(contactNote[ContactIdField].ToString()),
                    ContactName = GetContactName((Guid)contactNote[CreatorIdField])
                };
            }
        }

        public void AddOrUpdateContactNote(ContactNoteViewModel contactNoteViewModel)
        {
            if (contactNoteViewModel.ContactNoteId == null)
            {
                var userId = PrincipalInfo.CurrentPrincipal.GetContactId();
                var contactNote = BusinessManager.InitializeEntity(ContactNoteMetaClass);
                contactNote.Properties[CreatedField].Value = DateTime.UtcNow;
                contactNote.Properties[ModifiedField].Value = DateTime.UtcNow;
                contactNote.Properties[NoteTitleField].Value = contactNoteViewModel.NoteTitle;
                contactNote.Properties[NoteContentField].Value = contactNoteViewModel.NoteContent;
                contactNote.Properties[ContactIdField].Value = contactNoteViewModel.ContactId;
                contactNote.Properties[CreatorIdField].Value = userId;
                contactNote.Properties[ModifierIdField].Value = userId;
                BusinessManager.Create(contactNote);
            }
            else
            {
                var contactNote = BusinessManager.Load(ContactNoteMetaClass, new PrimaryKeyId(contactNoteViewModel.ContactNoteId.Value));
                contactNote.Properties[ModifiedField].Value = DateTime.UtcNow;
                contactNote.Properties[NoteTitleField].Value = contactNoteViewModel.NoteTitle;
                contactNote.Properties[NoteContentField].Value = contactNoteViewModel.NoteContent;
                contactNote.Properties[ContactIdField].Value = contactNoteViewModel.ContactId;
                BusinessManager.Update(contactNote);
            }
        }

        public void DeleteContactNotes(Guid[] contactNoteIds)
        {
            foreach (var item in contactNoteIds)
            {
                var contactNote = BusinessManager.Load(ContactNoteMetaClass, new PrimaryKeyId(item));
                BusinessManager.Delete(contactNote);
            }
        }
        #endregion

        #region Customer groups

        public IEnumerable<EnumViewModel> GetEnumsByType(string type)
        {
            var customerGroupList = DataContext.Current.MetaModel.RegisteredTypes[type]?.EnumItems ?? new MetaEnumItem[0];
            for (int i = 0; i < customerGroupList.Length; i++)
            {
                yield return new EnumViewModel
                {
                    Id = customerGroupList[i].Handle,
                    Name = customerGroupList[i].Name,
                    Type = type,
                    OrderId = customerGroupList[i].OrderId
                };
            }
        }

        public void AddOrUpdateEnum(EnumViewModel enumViewModel)
        {
            var metaFieldType = DataContext.Current.MetaModel.RegisteredTypes[enumViewModel.Type];
            if (enumViewModel.Id > 0)
            {
                MetaEnum.UpdateItem(metaFieldType, enumViewModel.Id, enumViewModel.Name, enumViewModel.OrderId);
            }
            else
            {
                MetaEnum.AddItem(metaFieldType, enumViewModel.Name, enumViewModel.OrderId);
            }
        }

        public void DeleteEnum(int id, string type)
        {
            MetaEnum.RemoveItem(DataContext.Current.MetaModel.RegisteredTypes[type], id);
        }

        #endregion

        #region Addresses
        public IEnumerable<AddressViewModel> GetAddressesInContact(Guid contactId)
        {
            var customerContact = CustomerContext.Current.GetContactById(contactId);
            return customerContact.ContactAddresses.Select(a => a.ToAddressViewModel());
        }

        public IEnumerable<AddressViewModel> GetAddressesInOrganization(string orgId)
        {
            var organization = CustomerContext.Current.GetOrganizationById(orgId);
            return organization.Addresses.Select(a => a.ToAddressViewModel());
        }

        public AddressViewModel GetAddressById(Guid addressId)
        {
            return ((CustomerAddress)BusinessManager.Load(CustomerAddress.ClassName, new PrimaryKeyId(addressId))).ToAddressViewModel();
        }

        public string AddOrUpdateAddress(AddressViewModel addressViewModel)
        {
            AddressEntity address;
            var primaryKeyId = "";
            if (addressViewModel.PrimaryKeyId != null)
            {
                address = addressViewModel.ToAddressEntity((AddressEntity)BusinessManager
                    .Load(AddressEntity.ClassName, new PrimaryKeyId(addressViewModel.PrimaryKeyId.Value)));
                BusinessManager.Update(address);
            }
            else
            {
                address = addressViewModel.ToAddressEntity(BusinessManager.InitializeEntity<AddressEntity>(AddressEntity.ClassName));
                primaryKeyId = BusinessManager.Create(address).ToString();
            }

            return primaryKeyId;
        }

        public void DeleteAddress(Guid id)
        {
            FilterElementCollection filters;
            var primaryKeyId = new PrimaryKeyId(id);
            using (var tran = DataContext.Current.BeginTransaction())
            {
                // Remove reference in PreferredBillingAddressId field at Contact Entity
                filters = new FilterElementCollection();
                filters.Add(FilterElement.EqualElement(ContactEntity.FieldPreferredBillingAddressId, primaryKeyId));
                foreach (ContactEntity contact in BusinessManager.List(ContactEntity.ClassName, filters.ToArray()))
                {
                    contact.PreferredBillingAddressId = null;
                    BusinessManager.Update(contact);
                }

                // Remove reference in PreferredShippingAddressId field at Contact Entity
                filters = new FilterElementCollection();
                filters.Add(FilterElement.EqualElement(ContactEntity.FieldPreferredShippingAddressId, primaryKeyId));
                foreach (ContactEntity contact in BusinessManager.List(ContactEntity.ClassName, filters.ToArray()))
                {
                    contact.PreferredShippingAddressId = null;
                    BusinessManager.Update(contact);
                }

                BusinessManager.Delete(CustomerAddress.ClassName, primaryKeyId);
                tran.Commit();
            }
        }
        #endregion

        #region Organizations
        public IEnumerable<OrganizationViewModel> SearchOrganizationsByPattern(string pattern)
        {
            var filter = new FilterElement[0];

            if (!string.IsNullOrEmpty(pattern))
            {
                var textPropertyFilters = Organization.TextProperties.Select(x => new FilterElement(x, FilterElementType.Contains, pattern));
                var orBlock = new OrBlockFilterElement(textPropertyFilters.ToArray());
                filter = new FilterElement[] { orBlock };
            }

            return BusinessManager.List(Organization.ClassName, filter).OfType<Organization>().Select(o => o.ToOrganizationViewModel()).ToList();
        }

        public OrganizationViewModel GetOrganizationById(Guid orgId)
        {
            var org = (BusinessManager.Load(Organization.ClassName, new PrimaryKeyId(orgId)) as Organization).ToOrganizationViewModel();
            org.Children = GetChildOrganizations(org.PrimaryKeyId.Value).ToList();
            org.Contacts = GetContactsInOrganization(orgId);
            org.Addresses = GetAddressesInOrganization(orgId.ToString());
            return org;
        }

        public OrganizationEntity AddOrUpdateOrganization(OrganizationViewModel model)
        {
            OrganizationEntity org;
            var primaryKeyId = model.PrimaryKeyId ?? Guid.Empty;
            if (model.PrimaryKeyId != null)
            {
                var entity = (OrganizationEntity)BusinessManager.Load(OrganizationEntity.ClassName, new PrimaryKeyId(model.PrimaryKeyId.Value));
                var descendants = GetChildOrganizations((Guid)entity.PrimaryKeyId.Value).ToList();
                if (entity.ParentId != model.ParentId && model.ParentId != Guid.Empty)
                {
                    if (descendants.Any(x => x.PrimaryKeyId == model.ParentId))
                    {
                        throw new Exception(_localizationService.GetString("/episerver.commerce.ui.admin/organization/parent_circular_reference"));
                    }
                }

                org = model.ToOrganizationEntity(entity);
                BusinessManager.Update(org);

                //Add-Update-Delete addresses.
                var allAddresses = GetAddressesInOrganization(primaryKeyId.ToString());
                UpdateAddressesToOrganization(model.Addresses, allAddresses);

                //Add contact to organization.
                if (model.Contacts.Any())
                {
                    var lstContacts = GetContactsInOrganization(primaryKeyId);
                    var newContactIds = model.Contacts.Where(x => !lstContacts.Any(z => z.PrimaryKeyId == x.PrimaryKeyId)).Select(t => t.PrimaryKeyId ?? Guid.Empty).ToList();
                    if (newContactIds.Any())
                    {
                        AddContactsToOrganization(newContactIds, primaryKeyId);
                    }
                }

                //Add children to organization.
                if (model.Children.Any())
                {
                    var newOrgIds = model.Children.Where(x => !descendants.Any(z => z.PrimaryKeyId == x.PrimaryKeyId)).Select(t => t.PrimaryKeyId ?? Guid.Empty).ToList();
                    if (newOrgIds.Any())
                    {
                        AddChildOrganizationsToOrganization(newOrgIds, primaryKeyId);
                    }
                }
            }
            else
            {
                org = model.ToOrganizationEntity(BusinessManager.InitializeEntity<OrganizationEntity>(OrganizationEntity.ClassName));
                BusinessManager.Create(org);
            }
            return org;
        }

        public void DeleteOrganization(Guid orgId, eRelatedEntityDeleteMode mode)
        {
            var request = new DeleteEntityWithDependsRequest(OrganizationEntity.ClassName, new PrimaryKeyId(orgId), mode);
            try
            {
                BusinessManager.Execute(request);
            }
            // if we caught ObjectNotFoundException during deletion we suppose it was a bunch deletion and this exception shouldn't be propagated
            catch (ObjectNotFoundException)
            {
                _log.LogInformation($"Can't delete the Organization with id={orgId} because the entity doesn't exist. It might've been a bunch of deletions.");
            }
        }

        public IEnumerable<ContactViewModel> GetContactsInOrganization(Guid orgId)
        {
            var organization = BusinessManager.Load(OrganizationEntity.ClassName, new PrimaryKeyId(orgId)) as Organization;
            foreach (var item in organization.Contacts)
            {
                yield return item.ToContactViewModel();
            };
        }

        public void AddContactsToOrganization(List<Guid> contactIds, Guid orgId)
        {
            foreach (var id in contactIds)
            {
                var contact = CustomerContext.Current.GetContactById(id);
                contact.OwnerId = new PrimaryKeyId(orgId);
                contact.SaveChanges();
            }
        }

        public IEnumerable<OrganizationViewModel> GetChildOrganizations(Guid orgId)
        {
            var parentOrganization = BusinessManager.Load(OrganizationEntity.ClassName, new PrimaryKeyId(orgId)) as Organization;
            foreach (var item in parentOrganization.ChildOrganizations)
            {
                yield return item.ToOrganizationViewModel();

                foreach (var org in GetChildOrganizations(item.PrimaryKeyId.Value))
                {
                    yield return org;
                }
            };
        }

        public void AddChildOrganizationsToOrganization(List<Guid> organizationIds, Guid parentOrganizationId)
        {
            var childrens = new List<OrganizationViewModel>();
            organizationIds.ForEach(x => childrens.AddRange(GetChildOrganizations(x)));

            if (childrens.Any(x => x.PrimaryKeyId == parentOrganizationId))
            {
                throw new Exception(_localizationService.GetString("/episerver.commerce.ui.admin/organization/children_circular_reference"));
            }

            foreach (var id in organizationIds)
            {
                var organization = BusinessManager.Load(OrganizationEntity.ClassName, new PrimaryKeyId(id)) as Organization;
                organization.ParentId = new PrimaryKeyId(parentOrganizationId);
                organization.SaveChanges();
            }
        }

        #endregion

        #region Extended Properties
        public IEnumerable<ExtendedPropertyViewModel> GetExtendedPropertiesByClassName(string className)
        {
            var extendedProperties = new List<ExtendedPropertyViewModel>();
            var metaFields = DataContext.Current.GetMetaClass(className)?.Fields.OfType<MetaField>().ToList();
            List<string> properties = null;
            switch (className)
            {
                case "Organization":
                    properties = new List<string>() {
                        "OrganizationId",
                        "Created", "Modified",
                        "CreatorId",
                        "ModifierId",
                        "Name",
                        "Description",
                        "PrimaryContactId",
                        "PrimaryContact",
                        "OrganizationType",
                        "OrgCustomerGroup",
                        "BusinessCategory",
                        "ParentId",
                        "Parent"
                    };
                    break;
                case "Contact":
                    properties = new List<string>() {
                        "ContactId",
                        "Created",
                        "Modified",
                        "CreatorId",
                        "ModifierId",
                        "FullName",
                        "LastName",
                        "FirstName",
                        "MiddleName",
                        "Password",
                        "Email",
                        "BirthDate",
                        "LastOrder",
                        "CustomerGroup",
                        "Code",
                        "PreferredLanguage",
                        "PreferredCurrency",
                        "RegistrationSource",
                        "OwnerId",
                        "Owner",
                        "PreferredShippingAddressId",
                        "PreferredShippingAddress",
                        "PreferredBillingAddressId",
                        "PreferredBillingAddress",
                        "UserId",
                        "AcceptMarketingEmail",
                        "ConsentUpdated"
                    };
                    break;
                case "Address":
                    properties = new List<string>() {
                         "AddressId",
                         "Created",
                         "Modified",
                         "CreatorId",
                         "ModifierId",
                         "Name",
                         "ApplicationId",
                         "LastName",
                         "FirstName",
                         "OrganizationName",
                         "Line1",
                         "Line2",
                         "City",
                         "State",
                         "CountryCode",
                         "CountryName",
                         "PostalCode",
                         "RegionCode",
                         "RegionName",
                         "DaytimePhoneNumber",
                         "EveningPhoneNumber",
                         "Email",
                         "IsDefault",
                         "AddressType",
                         "ContactId",
                         "Contact",
                         "OrganizationId",
                         "Organization",
                    };
                    break;
                default:
                    break;
            }

            if (properties != null && metaFields != null)
            {
                foreach (var field in metaFields)
                {
                    if (!properties.Any(x => x.Equals(field.Name)))
                    {
                        extendedProperties.Add(new ExtendedPropertyViewModel
                        {
                            Name = field.Name,
                            FriendlyName = field.FriendlyName,
                            DataType = field.GetMetaType().McDataType,
                            Value = "",
                            IsNullable = field.IsNullable
                        });
                    }
                }
            }
            return extendedProperties;
        }
        #endregion

        #region Customer account
        public async Task<CustomerAccountViewModel> GetCustomerAccountByContactIdAsync(Guid contactId)
        {
            var result = new CustomerAccountViewModel();
            var contact = (CustomerContact)BusinessManager.Load(CustomerContact.ClassName, new PrimaryKeyId(contactId));
            var userKey = new MapUserKey(new[] { new ConvertStringUserKey() }).ToUserKey(contact.UserId);
            var userId = userKey != null ? userKey.ToString() : contact.UserId;
            var user = await _uiUserProvider.GetUserAsync(userId);
            if (user != null)
            {
                result.Username = user.Username;
                result.Email = user.Email;
                result.Approved = user.IsApproved;
            }
            return result;
        }

        public async Task<CreateUserResult> CreateCustomerAccountAsync(CustomerAccountViewModel user)
        {
            if (!string.IsNullOrEmpty(user.Username))
            {
                var account = await _uiUserProvider.GetUserAsync(user.Username);
                if (account != null && !string.IsNullOrEmpty(account.Username))
                {
                    throw new ValidationException(string.Format("Username {0} is existed.", account.Username));
                }
            }
            var response = await _uiUserProvider.CreateUserAsync(user.Username, user.Password, user.Email, null, null, user.Approved);
            return response;
        }

        #endregion

        #region Helper
        private int GetTotalContacts(FilterElement[] filterElements = null)
        {
            var cacheKey = CustomersCache.CreateCacheKey(ContactEntity.ClassName, string.Empty, "GetTotalCount",
                filterElements == null ? "All" : string.Join(":", filterElements.Select(x => x.Source)));

            var count = CustomersCache.Get(cacheKey);
            if (count != null)
            {
                return (int)count;
            }

            var executedCount = MetaObject.GetTotalCount(DataContext.Current.MetaModel.MetaClasses[ContactEntity.ClassName],
                    filterElements ?? new FilterElement[0]);
            CustomersCache.Insert(cacheKey, executedCount,
                _customerOptions.Cache.ContactCollectionCacheExpiration);
            return executedCount;
        }

        private static string GetContactName(Guid contactId)
        {
            var contact = CustomerContext.Current.GetContactById(contactId);
            if (contact != null)
            {
                var userName = contact.FullName ?? contact.Email;
                if (!string.IsNullOrEmpty(userName))
                {
                    return userName;
                }
            }

            return contactId.ToString();
        }
        #endregion
    }

    internal static class ContactExtensions
    {
        public static ContactViewModel ToContactViewModel(this CustomerContact customerContact)
        {
            var userKey = new MapUserKey(new[] { new ConvertStringUserKey() }).ToUserKey(customerContact.UserId);

            var model = new ContactViewModel
            {
                AcceptMarketingEmail = customerContact.AcceptMarketingEmail,
                BirthDate = customerContact.BirthDate,
                ConsentUpdated = customerContact.ConsentUpdated,
                Created = customerContact.Created,
                CreatorId = customerContact.CreatorId,
                CustomerGroupId = ((ContactEntity)customerContact).CustomerGroup,
                CustomerGroup = customerContact.CustomerGroup,
                Email = customerContact.Email,
                FirstName = customerContact.FirstName,
                FullName = customerContact.FullName,
                LastName = customerContact.LastName,
                LastOrder = customerContact.LastOrder,
                MiddleName = customerContact.MiddleName,
                Modified = customerContact.Modified,
                ModifierId = customerContact.ModifierId,
                OwnerId = customerContact.OwnerId,
                PreferredBillingAddressId = customerContact.PreferredBillingAddressId,
                PreferredCurrencyId = customerContact.PreferredCurrency,
                PreferredLanguageId = customerContact.PreferredLanguage,
                PreferredShippingAddressId = customerContact.PreferredShippingAddressId,
                PrimaryKeyId = customerContact.PrimaryKeyId.Value,
                RegistrationSource = customerContact.RegistrationSource,
                UserId = userKey != null ? userKey.ToString() : customerContact.UserId,
                ExtendedProperties = new List<ExtendedPropertyViewModel>()
            };

            var metaFields = DataContext.Current.GetMetaClass(customerContact.MetaClassName)?.Fields.OfType<MetaField>().ToList() ?? new List<MetaField>();
            if (metaFields.Count == 0)
            {
                return model;
            }

            foreach (var prop in customerContact.ExtendedProperties)
            {
                var field = metaFields.SingleOrDefault(x => x.Name.Equals(prop.Name));
                if (field == null)
                {
                    continue;
                }

                model.ExtendedProperties.Add(new ExtendedPropertyViewModel
                {
                    Name = field.Name,
                    FriendlyName = field.FriendlyName,
                    DataType = field.GetMetaType().McDataType,
                    Value = prop.Value,
                    IsNullable = field.IsNullable
                });
            }

            return model;
        }

        public static ContactEntity ToContactEntity(this ContactViewModel contactViewModel, ContactEntity currentContact)
        {
            currentContact.Email = contactViewModel.Email;
            currentContact.FirstName = contactViewModel.FirstName;
            currentContact.FullName = contactViewModel.FullName;
            currentContact.LastName = contactViewModel.LastName;
            currentContact.MiddleName = contactViewModel.MiddleName;
            currentContact.OwnerId = contactViewModel.OwnerId != null ? new PrimaryKeyId(contactViewModel.OwnerId.Value) : (PrimaryKeyId?)null;
            currentContact.UserId = !string.IsNullOrEmpty(contactViewModel.UserId) ? new MapUserKey().ToTypedString(contactViewModel.UserId) : "";

            currentContact.PreferredBillingAddressId = contactViewModel.PreferredBillingAddressId != null ?
                                                       new PrimaryKeyId(contactViewModel.PreferredBillingAddressId.Value) :
                                                       (PrimaryKeyId?)null;

            currentContact.PreferredShippingAddressId = contactViewModel.PreferredShippingAddressId != null ?
                                                        new PrimaryKeyId(contactViewModel.PreferredShippingAddressId.Value) :
                                                        (PrimaryKeyId?)null;
            currentContact.CustomerGroup = contactViewModel.CustomerGroupId;
            currentContact.PreferredCurrency = contactViewModel.PreferredCurrencyId;
            currentContact.PreferredLanguage = contactViewModel.PreferredLanguageId;
            currentContact.RegistrationSource = contactViewModel.RegistrationSource;

            if (contactViewModel.PrimaryKeyId == Guid.Empty)
            {
                contactViewModel.Created = DateTime.UtcNow;
                contactViewModel.Modified = DateTime.UtcNow;
            }
            else
            {
                contactViewModel.Modified = DateTime.UtcNow;
            }

            var metaFields = DataContext.Current.GetMetaClass(ContactEntity.ClassName)?.Fields.OfType<MetaField>().ToList() ?? new List<MetaField>();
            if (metaFields.Count == 0)
            {
                return currentContact;
            }
            if (contactViewModel.ExtendedProperties != null)
            {
                foreach (var prop in contactViewModel.ExtendedProperties)
                {
                    var field = metaFields.SingleOrDefault(x => x.Name.Equals(prop.Name));
                    if (field == null)
                    {
                        continue;
                    }

                    if (prop.Value != null)
                    {
                        if (prop.DataType == McDataType.Boolean)
                        {
                            currentContact.Properties.Add(new EntityObjectProperty(prop.Name, bool.Parse(prop.Value.ToString())));
                        }
                        else if (prop.DataType == McDataType.Integer)
                        {
                            currentContact.Properties.Add(new EntityObjectProperty(prop.Name, Int32.Parse(prop.Value.ToString())));
                        }
                        else if (prop.DataType == McDataType.Decimal || prop.DataType == McDataType.Currency)
                        {
                            currentContact.Properties.Add(new EntityObjectProperty(prop.Name, Decimal.Parse(prop.Value.ToString())));
                        }
                        else if (prop.DataType == McDataType.Guid)
                        {
                            currentContact.Properties.Add(new EntityObjectProperty(prop.Name, new Guid(prop.Value.ToString())));
                        }
                        else if (prop.DataType == McDataType.String)
                        {
                            currentContact.Properties.Add(new EntityObjectProperty(prop.Name, prop.Value.ToString()));
                        }
                        else if (prop.DataType == McDataType.DateTime)
                        {
                            if (DateTime.TryParse(prop.Value.ToString(), out var dateTime))
                            {
                                currentContact.Properties.Add(new EntityObjectProperty(prop.Name, dateTime));
                            }
                        }
                    }
                    else
                    {
                        currentContact.Properties.Add(new EntityObjectProperty(prop.Name, null));
                    }
                }
            }

            return currentContact;
        }

        public static AddressViewModel ToAddressViewModel(this AddressEntity addressEntity)
        {
            var model = new AddressViewModel
            {
                AddressType = addressEntity.AddressType,
                City = addressEntity.City,
                ContactId = addressEntity.ContactId,
                OrganizationId = addressEntity.OrganizationId,
                CountryCode = addressEntity.CountryCode,
                CountryName = addressEntity.CountryName,
                DaytimePhoneNumber = addressEntity.DaytimePhoneNumber,
                Email = addressEntity.Email,
                EveningPhoneNumber = addressEntity.EveningPhoneNumber,
                FirstName = addressEntity.FirstName,
                LastName = addressEntity.LastName,
                Line1 = addressEntity.Line1,
                Line2 = addressEntity.Line2,
                Name = addressEntity.Name,
                PostalCode = addressEntity.PostalCode,
                PrimaryKeyId = addressEntity.PrimaryKeyId.Value,
                RegionCode = addressEntity.RegionCode,
                State = addressEntity.State,
                ExtendedProperties = new List<ExtendedPropertyViewModel>()
            };

            var metaFields = DataContext.Current.GetMetaClass(addressEntity.MetaClassName)?.Fields.OfType<MetaField>().ToList() ?? new List<MetaField>();
            if (metaFields.Count == 0)
            {
                return model;
            }

            foreach (var prop in addressEntity.ExtendedProperties)
            {
                var field = metaFields.SingleOrDefault(x => x.Name.Equals(prop.Name));
                if (field == null)
                {
                    continue;
                }

                model.ExtendedProperties.Add(new ExtendedPropertyViewModel
                {
                    Name = field.Name,
                    FriendlyName = field.FriendlyName,
                    DataType = field.GetMetaType().McDataType,
                    Value = prop.Value,
                    IsNullable = field.IsNullable
                });
            }

            return model;
        }

        public static AddressEntity ToAddressEntity(this AddressViewModel addressViewModel, AddressEntity addressEntity)
        {
            addressEntity.AddressType = addressViewModel.AddressType;
            addressEntity.City = addressViewModel.City;
            addressEntity.CountryCode = addressViewModel.CountryCode;
            addressEntity.CountryName = addressViewModel.CountryName;
            addressEntity.DaytimePhoneNumber = addressViewModel.DaytimePhoneNumber;
            addressEntity.Email = addressViewModel.Email;
            addressEntity.EveningPhoneNumber = addressViewModel.EveningPhoneNumber;
            addressEntity.FirstName = addressViewModel.FirstName;
            addressEntity.LastName = addressViewModel.LastName;
            addressEntity.Line1 = addressViewModel.Line1;
            addressEntity.Line2 = addressViewModel.Line2;
            addressEntity.Name = addressViewModel.Name;
            addressEntity.PostalCode = addressViewModel.PostalCode;
            addressEntity.RegionCode = addressViewModel.RegionCode;
            addressEntity.State = addressViewModel.State;
            addressEntity.ContactId = addressViewModel.ContactId != null ?
                                                       new PrimaryKeyId(addressViewModel.ContactId.Value) :
                                                       (PrimaryKeyId?)null;
            addressEntity.OrganizationId = addressViewModel.OrganizationId != null ?
                                                       new PrimaryKeyId(addressViewModel.OrganizationId.Value) :
                                                       (PrimaryKeyId?)null;

            var metaFields = DataContext.Current.GetMetaClass(AddressEntity.ClassName)?.Fields.OfType<MetaField>().ToList() ?? new List<MetaField>();
            if (metaFields.Count == 0)
            {
                return addressEntity;
            }

            foreach (var prop in addressViewModel.ExtendedProperties)
            {
                var field = metaFields.SingleOrDefault(x => x.Name.Equals(prop.Name));
                if (field == null)
                {
                    continue;
                }

                if (prop.Value != null)
                {
                    if (prop.DataType == McDataType.Boolean)
                    {
                        addressEntity.Properties[prop.Name].Value = bool.Parse(prop.Value.ToString());
                    }
                    else if (prop.DataType == McDataType.Integer)
                    {
                        addressEntity.Properties[prop.Name].Value = Int32.Parse(prop.Value.ToString());
                    }
                    else if (prop.DataType == McDataType.Decimal || prop.DataType == McDataType.Currency)
                    {
                        addressEntity.Properties[prop.Name].Value = Decimal.Parse(prop.Value.ToString());
                    }
                    else if (prop.DataType == McDataType.Guid)
                    {
                        addressEntity.Properties[prop.Name].Value = new Guid(prop.Value.ToString());
                    }
                    else if (prop.DataType == McDataType.String)
                    {
                        addressEntity.Properties[prop.Name].Value = prop.Value.ToString();
                    }
                    else if (prop.DataType == McDataType.DateTime)
                    {
                        if (DateTime.TryParse(prop.Value.ToString(), out var dateTime))
                        {
                            addressEntity.Properties[prop.Name].Value = dateTime;
                        }
                    }
                }
                else
                {
                    addressEntity.Properties[prop.Name].Value = null;
                }
            }

            return addressEntity;
        }

        public static OrganizationViewModel ToOrganizationViewModel(this Organization organization)
        {
            var model = new OrganizationViewModel
            {
                Name = organization.Name,
                Description = organization.Description,
                PrimaryKeyId = organization.PrimaryKeyId.Value,
                OrganizationTypeId = ((OrganizationEntity)organization).OrganizationType,
                OrganizationType = organization.OrganizationType,
                OrganizationCustomerGroupId = ((OrganizationEntity)organization).OrgCustomerGroup,
                OrganizationCustomerGroup = organization.OrgCustomerGroup,
                BusinessCategoryId = ((OrganizationEntity)organization).BusinessCategory,
                BusinessCategory = organization.BusinessCategory,
                ParentId = organization.ParentId,
                ExtendedProperties = new List<ExtendedPropertyViewModel>()
            };

            var metaFields = DataContext.Current.GetMetaClass(organization.MetaClassName)?.Fields.OfType<MetaField>().ToList() ?? new List<MetaField>();
            if (metaFields.Count == 0)
            {
                return model;
            }

            foreach (var prop in organization.ExtendedProperties)
            {
                var field = metaFields.SingleOrDefault(x => x.Name.Equals(prop.Name));
                if (field == null)
                {
                    continue;
                }

                model.ExtendedProperties.Add(new ExtendedPropertyViewModel
                {
                    Name = field.Name,
                    FriendlyName = field.FriendlyName,
                    DataType = field.GetMetaType().McDataType,
                    Value = prop.Value,
                    IsNullable = field.IsNullable
                });
            }

            return model;
        }

        public static OrganizationEntity ToOrganizationEntity(this OrganizationViewModel organizationModel, OrganizationEntity currentOrganization)
        {
            currentOrganization.Name = organizationModel.Name;
            currentOrganization.Description = organizationModel.Description;
            currentOrganization.OrganizationType = organizationModel.OrganizationTypeId;
            currentOrganization.BusinessCategory = organizationModel.BusinessCategoryId;
            currentOrganization.OrgCustomerGroup = organizationModel.OrganizationCustomerGroupId;
            currentOrganization.ParentId = organizationModel.ParentId != null && organizationModel.ParentId.Value != Guid.Empty ?
                                                       new PrimaryKeyId(organizationModel.ParentId.Value) :
                                                       null;
            currentOrganization.PrimaryKeyId = organizationModel.PrimaryKeyId != null ?
                                                       new PrimaryKeyId(organizationModel.PrimaryKeyId.Value) :
                                                       null;

            var metaFields = DataContext.Current.GetMetaClass(OrganizationEntity.ClassName)?.Fields.OfType<MetaField>().ToList() ?? new List<MetaField>();
            if (metaFields.Count == 0)
            {
                return currentOrganization;
            }

            foreach (var prop in organizationModel.ExtendedProperties)
            {
                var field = metaFields.SingleOrDefault(x => x.Name.Equals(prop.Name));
                if (field == null)
                {
                    continue;
                }
                if (prop.Value != null)
                {
                    if (field.GetMetaType().McDataType == McDataType.Boolean)
                    {
                        currentOrganization.Properties.Add(new EntityObjectProperty(prop.Name, bool.Parse(prop.Value.ToString())));
                    }
                    else if (field.GetMetaType().McDataType == McDataType.Integer)
                    {
                        currentOrganization.Properties.Add(new EntityObjectProperty(prop.Name, Int32.Parse(prop.Value.ToString())));
                    }
                    else if (field.GetMetaType().McDataType == McDataType.Decimal || field.GetMetaType().McDataType == McDataType.Currency)
                    {
                        currentOrganization.Properties.Add(new EntityObjectProperty(prop.Name, Decimal.Parse(prop.Value.ToString())));
                    }
                    else if (field.GetMetaType().McDataType == McDataType.Guid)
                    {
                        currentOrganization.Properties.Add(new EntityObjectProperty(prop.Name, new Guid(prop.Value.ToString())));
                    }
                    else if (field.GetMetaType().McDataType == McDataType.String)
                    {
                        currentOrganization.Properties.Add(new EntityObjectProperty(prop.Name, prop.Value.ToString()));
                    }
                    else if (field.GetMetaType().McDataType == McDataType.DateTime)
                    {
                        if (DateTime.TryParse(prop.Value.ToString(), out var dateTime))
                        {
                            currentOrganization.Properties.Add(new EntityObjectProperty(prop.Name, dateTime));
                        }
                    }
                }
                else
                {
                    currentOrganization.Properties.Add(new EntityObjectProperty(prop.Name, null));
                }
            }

            return currentOrganization;
        }
    }
}
