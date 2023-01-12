using Castle.Core.Internal;
using EPiServer.Cms.UI.AspNetIdentity;
using Foundation.Features.MyOrganization;
using Foundation.Infrastructure.Cms.Users;
using Mediachase.Commerce.Customers;
using Microsoft.AspNetCore.Identity;

namespace Foundation.Infrastructure.Commerce.Customer.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerContext _customerContext;
        private readonly LocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerService(ServiceAccessor<ApplicationSignInManager<SiteUser>> signinManager,
            ServiceAccessor<ApplicationUserManager<SiteUser>> userManager,
            IHttpContextAccessor httpContextAccessor,
            LocalizationService localizationService)
        {
            _customerContext = CustomerContext.Current;
            _httpContextAccessor = httpContextAccessor;
            _localizationService = localizationService;
            UserManager = userManager;
            SignInManager = signinManager;
        }

        public virtual ServiceAccessor<ApplicationUserManager<SiteUser>> UserManager { get; }
        public virtual ServiceAccessor<ApplicationSignInManager<SiteUser>> SignInManager { get; }
        public virtual Guid CurrentContactId => _customerContext.CurrentContactId;

        public virtual void CreateContact(FoundationContact contact, string contactId)
        {
            contact.ContactId = Guid.Parse(contactId);
            contact.UserId = contact.Email;
            contact.UserLocationId = contact.UserRole != B2BUserRoles.Admin.ToString() ? contact.UserLocationId : "";
            contact.SaveChanges();

            if (contact.UserRole == B2BUserRoles.Admin.ToString())
            {
                AddContactToOrganization(contact);
            }
            else
            {
                AddContactToOrganization(contact, contact.FoundationOrganization.OrganizationId.ToString());
            }
        }

        public virtual void EditContact(FoundationContact model) => UpdateContact(model.ContactId.ToString(), model.UserRole, model.UserLocationId);

        public virtual void RemoveContactFromOrganization(string id)
        {
            var contact = GetContactById(id);
            contact.FoundationOrganization = new FoundationOrganization(new Mediachase.Commerce.Customers.Organization());
            contact.SaveChanges();
        }

        public virtual void AddContactToOrganization(FoundationContact contact, string organizationId = null)
        {
            if (organizationId.IsNullOrEmpty())
            {
                var org = GetCurrentOrganization();
                if (org != null)
                {
                    contact.FoundationOrganization = org;
                    contact.SaveChanges();
                }
            }
            else
            {
                var organization = _customerContext.GetOrganizationById(organizationId);
                if (organization != null)
                {
                    contact.FoundationOrganization = new FoundationOrganization(organization);
                    contact.SaveChanges();
                }
            }
        }

        public virtual void UpdateContact(string contactId, string userRole, string location = null)
        {
            var contact = GetContactById(contactId);
            contact.UserRole = userRole;
            contact.UserLocationId = location;
            contact.SaveChanges();
        }

        public virtual bool CanSeeOrganizationNav()
        {
            var contact = GetCurrentContact();
            if (contact == null)
            {
                return false;
            }

            var currentRole = contact.B2BUserRole;
            return currentRole == B2BUserRoles.Admin || currentRole == B2BUserRoles.Approver;
        }

        public virtual bool HasOrganization(string contactId)
        {
            var contact = GetContactById(contactId);
            return contact.FoundationOrganization != null;
        }

        public virtual FoundationContact GetContactByEmail(string email)
        {
            var contact = _customerContext.GetContacts(0, 1000)
                .FirstOrDefault(user => user.Email == email);
            return contact == null ? null : new FoundationContact(contact);
        }

        public virtual FoundationContact GetCurrentContact()
        {
            var contact = _customerContext.CurrentContact;
            if (contact == null)
            {
                return null;
            }

            return new FoundationContact(contact);
        }

        public virtual FoundationContact GetContactById(string contactId)
        {
            if (string.IsNullOrEmpty(contactId))
            {
                return null;
            }

            var contact = _customerContext.GetContactById(new Guid(contactId));
            return contact != null ? new FoundationContact(contact) : null;
        }

        public virtual List<FoundationContact> GetContactsForOrganization(FoundationOrganization organization = null)
        {
            if (organization == null)
            {
                organization = GetCurrentOrganization();
            }

            if (organization == null)
            {
                return new List<FoundationContact>();
            }

            return _customerContext.GetCustomerContactsInOrganization(organization.OrganizationEntity)
                .Select(_ => new FoundationContact(_))
                .ToList();
        }

        public virtual void AddContactToOrganization(FoundationOrganization organization, FoundationContact contact, B2BUserRoles userRole)
        {
            contact.FoundationOrganization = organization;
            contact.UserRole = userRole.ToString();
            contact.SaveChanges();
        }

        public virtual List<FoundationContact> GetContacts()
        {
            return _customerContext.GetContacts(0, 1000)
                .Select(c => new FoundationContact(c))
                .ToList();
        }

        public virtual async Task<SiteUser> GetSiteUserAsync(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            return await UserManager().FindByNameAsync(email);
        }

        public virtual async Task<ExternalLoginInfo> GetExternalLoginInfoAsync() => await SignInManager().GetExternalLoginInfoAsync();

        public virtual async Task<IdentityContactResult> CreateUser(SiteUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (user.Password.IsNullOrEmpty())
            {
                throw new MissingFieldException("Password");
            }

            if (user.Email.IsNullOrEmpty())
            {
                throw new MissingFieldException("Email");
            }

            var result = new IdentityContactResult();
            if ((await UserManager().FindByEmailAsync(user.Email)) != null)
            {
                result.IdentityResult = IdentityResult.Failed(new IdentityError { Description = _localizationService.GetString("/Registration/Form/Error/UsedEmail", "This email address is already used") });
            }
            else
            {
                result.IdentityResult = await UserManager().CreateAsync(user, user.Password);

                if (result.IdentityResult.Succeeded)
                {
                    var identity = await SignInManager().GenerateUserIdentityAsync(user);
                    await SignInManager().SignInWithClaimsAsync(user, true, identity.Claims);

                    result.FoundationContact = CreateFoundationContact(user);
                }
            }

            return result;
        }

        public virtual async Task SignOutAsync()
        {
            await SignInManager().SignOutAsync();
            //TrackingCookieManager.SetTrackingCookie(Guid.NewGuid().ToString());
        }

        private void SetPreferredAddresses(CustomerContact contact)
        {
            var changed = false;

            var publicAddress = contact.ContactAddresses.FirstOrDefault(a => a.AddressType == CustomerAddressTypeEnum.Public);
            var preferredBillingAddress = contact.ContactAddresses.FirstOrDefault(a => a.AddressType == CustomerAddressTypeEnum.Billing);
            var preferredShippingAddress = contact.ContactAddresses.FirstOrDefault(a => a.AddressType == CustomerAddressTypeEnum.Shipping);

            if (publicAddress != null)
            {
                contact.PreferredShippingAddress = contact.PreferredBillingAddress = publicAddress;
                changed = true;
            }

            if (preferredBillingAddress != null)
            {
                contact.PreferredBillingAddress = preferredBillingAddress;
                changed = true;
            }

            if (preferredShippingAddress != null)
            {
                contact.PreferredShippingAddress = preferredShippingAddress;
                changed = true;
            }

            if (changed)
            {
                contact.SaveChanges();
            }
        }

        private FoundationOrganization GetCurrentOrganization()
        {
            var contact = GetCurrentContact();
            if (contact != null)
            {
                return contact.FoundationOrganization;
            }

            return null;
        }

        private FoundationContact CreateFoundationContact(SiteUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var contact = FoundationContact.New();
            if (!user.FirstName.IsNullOrEmpty() || !user.LastName.IsNullOrEmpty())
            {
                contact.FullName = string.Format("{0} {1}", user.FirstName, user.LastName);
            }

            contact.FirstName = user.FirstName;
            contact.LastName = user.LastName;
            contact.Email = user.Email;
            contact.UserId = user.Email;
            contact.RegistrationSource = user.RegistrationSource;

            //if (user.Addresses != null && user.Addresses.Any())
            //{
            //    foreach (var address in user.Addresses)
            //    {
            //        contact.Contact.AddContactAddress(address);
            //    }
            //}

            contact.SaveChanges();

            SetPreferredAddresses(contact.Contact);

            return contact;
        }

        public ContactViewModel GetCurrentContactViewModel()
        {
            var currentContact = GetCurrentContact();
            return currentContact?.Contact != null ? new ContactViewModel(currentContact) : new ContactViewModel();
        }
    }
}