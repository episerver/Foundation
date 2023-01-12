using EPiServer.Shell.Security;
using Foundation.Infrastructure.Commerce.Customer;
using Mediachase.BusinessFoundation.Data;
using Mediachase.BusinessFoundation.Data.Business;
using Mediachase.BusinessFoundation.Data.Meta.Management;
using Mediachase.BusinessFoundation.Data.Modules;
using Mediachase.Commerce.Core.RecentReferenceHistory;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Shared;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Xml.Linq;

namespace Foundation.Infrastructure.Commerce.Install.Steps
{
    public class AddCustomers : BaseInstallStep
    {
        private readonly UIUserProvider _uIUserProvider;
        private readonly UIRoleProvider _uIRoleProvider;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AddCustomers(IContentRepository contentRepository,
            ReferenceConverter referenceConverter,
            IMarketService marketService,
            IWebHostEnvironment webHostEnvironment,
            UIUserProvider uIUserProvider,
            UIRoleProvider uIRoleProvider) : base(contentRepository, referenceConverter, marketService, webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _uIUserProvider = uIUserProvider;
            _uIRoleProvider = uIRoleProvider;
        }

        public override int Order => 8;

        public override string Description => "Adds customers to Foundation.";

        protected override void ExecuteInternal(IProgressMessenger progressMessenger)
        {
            using (var scope = DataContext.Current.MetaModel.BeginEdit(MetaClassManagerEditScope.SystemOwner, AccessLevel.System))
            {
                var manager = DataContext.Current.MetaModel;
                var changeTrackingManifest = ChangeTrackingManager.CreateModuleManifest();
                var recentReferenceManifest = RecentReferenceManager.CreateModuleManifest();
                var contactMetaClass = manager.MetaClasses[ContactEntity.ClassName];

                var demoUserMenu = MetaEnum.Create("DemoUserMenu", "Show in Demo User Menu", false);
                MetaEnum.AddItem(demoUserMenu, 1, "Never", 1);
                MetaEnum.AddItem(demoUserMenu, 2, "Always", 2);
                MetaEnum.AddItem(demoUserMenu, 3, "Commerce Only", 3);

                using (var builder = new MetaFieldBuilder(contactMetaClass))
                {
                    builder.CreateEnumField("ShowInDemoUserMenu", "{Customer:DemoUserMenu}", "DemoUserMenu", true, "1", false);
                    builder.CreateText("DemoUserTitle", "{Customer:DemoUserTitle}", true, 100, false);
                    builder.CreateText("DemoUserDescription", "{Customer:DemoUserDescription}", true, 500, false);
                    builder.CreateInteger("DemoSortOrder", "{Customer:DemoSortOrder}", true, 0);
                    builder.SaveChanges();
                }

                var giftCardClass = manager.CreateMetaClass("GiftCard", "{Customer:GiftCard}", "{Customer:GiftCard}", "cls_GiftCard", PrimaryKeyIdValueType.Guid);
                ModuleManager.Activate(giftCardClass, changeTrackingManifest);
                using (var builder = new MetaFieldBuilder(giftCardClass))
                {
                    builder.CreateText("GiftCardName", "{Customer:GiftCardName}", false, 100, false);
                    builder.CreateCurrency("InitialAmount", "{Customer:InitialAmount}", true, 0, true);
                    builder.CreateCurrency("RemainBalance", "{Customer:RemainBalance}", true, 0, true);
                    builder.CreateText("RedemptionCode", "{Customer:RedemptionCode}", true, 100, false);
                    builder.CreateCheckBoxBoolean("IsActive", "{Customer:IsActive}", true, true, "{Customer:IsActive}");
                    giftCardClass.Fields[MetaClassManager.GetPrimaryKeyName(giftCardClass.Name)].FriendlyName = "{GlobalMetaInfo:PrimaryKeyId}";
                    builder.CreateReference("Contact", "{Customer:CreditCard_mf_Contact}", true, "Contact", false);
                    builder.SaveChanges();
                }

                giftCardClass.AddPermissions();
                scope.SaveChanges();
            }

            using (var stream = new FileStream(Path.Combine(_webHostEnvironment.ContentRootPath, @"App_Data", @"Customers.xml"), FileMode.Open))
            {
                ProcessCustomers(stream);
                ProcessOrganizations(stream);
            }
        }

        private void ProcessOrganizations(FileStream stream)
        {
            foreach (var xOrganization in GetXElements(stream, "Organization"))
            {
                var organization = new OrganizationPoco
                {
                    Id = xOrganization.Get("Id"),
                    Name = xOrganization.Get("Name"),
                    Users = new List<CustomerPoco>(),
                    CreditCards = new List<CreditCardPoco>(),
                    SubOrganizations = new List<OrganizationPoco>()
                };

                foreach (var xUser in xOrganization.Element("Users")?.Elements("User") ?? Enumerable.Empty<XElement>())
                {
                    var customer = new CustomerPoco
                    {
                        Email = xUser.Get("Email"),
                        FirstName = xUser.Get("FirstName"),
                        LastName = xUser.Get("LastName"),
                        Roles = xUser.GetEnumerable("Roles", ','),
                        B2BRole = xUser.Get("B2BRole"),
                        Location = xUser.Get("Location"),
                        ShowInDemoUserMenu = xUser.GetIntOrDefault("ShowInDemoUserMenu", 1),
                        DemoUserTitle = xUser.Get("DemoUserTitle"),
                        DemoUserDescription = xUser.Get("DemoUserDescription"),
                        DemoSortOrder = xUser.GetIntOrDefault("DemoSort"),
                        Addresses = new List<AddressPoco>(),
                        CreditCards = new List<CreditCardPoco>()
                    };

                    foreach (var xAddress in xUser.Element("Addresses")?.Elements("Address") ?? Enumerable.Empty<XElement>())
                    {
                        var address = new AddressPoco
                        {
                            Name = xAddress.Get("Name"),
                            Line1 = xAddress.Get("Line1"),
                            City = xAddress.Get("City"),
                            CountryCode = xAddress.Get("CountryCode"),
                            CountryName = xAddress.Get("CountryName"),
                            RegionCode = xAddress.Get("RegionCode"),
                            RegionName = xAddress.Get("RegionName"),
                            PostalCode = xAddress.Get("PostalCode")
                        };

                        customer.Addresses.Add(address);
                    }

                    organization.Users.Add(customer);
                }

                foreach (var xCreditCard in xOrganization.Element("CreditCards")?.Elements("CreditCard") ?? Enumerable.Empty<XElement>())
                {
                    var cc = new CreditCardPoco
                    {
                        Number = xCreditCard.Get("Number"),
                        CardType = xCreditCard.Get("CardType"),
                        LastFour = xCreditCard.Get("LastFour"),
                        ExpirationYear = xCreditCard.GetInt("ExpirationYear"),
                        ExpirationMonth = xCreditCard.GetInt("ExpirationMonth")
                    };

                    organization.CreditCards.Add(cc);
                }

                foreach (var xSubOrganization in xOrganization.Element("SubOrganizations")?.Elements("SubOrganization") ?? Enumerable.Empty<XElement>())
                {
                    var subOrganization = new OrganizationPoco
                    {
                        Id = xSubOrganization.Get("Id"),
                        Name = xSubOrganization.Get("Name"),
                        Users = new List<CustomerPoco>(),
                        CreditCards = new List<CreditCardPoco>(),
                        SubOrganizations = new List<OrganizationPoco>()
                    };

                    foreach (var xUser in xSubOrganization.Element("Users")?.Elements("User") ?? Enumerable.Empty<XElement>())
                    {
                        var customer = new CustomerPoco
                        {
                            Email = xUser.Get("Email"),
                            FirstName = xUser.Get("FirstName"),
                            LastName = xUser.Get("LastName"),
                            Roles = xUser.GetEnumerable("Roles", ','),
                            B2BRole = xUser.Get("B2BRole"),
                            Location = xUser.Get("Location"),
                            ShowInDemoUserMenu = xUser.GetIntOrDefault("ShowInDemoUserMenu", 1),
                            DemoUserTitle = xUser.Get("DemoUserTitle"),
                            DemoUserDescription = xUser.Get("DemoUserDescription"),
                            DemoSortOrder = xUser.GetIntOrDefault("DemoSort"),
                            Addresses = new List<AddressPoco>(),
                            CreditCards = new List<CreditCardPoco>()
                        };

                        foreach (var xAddress in xUser.Element("Addresses")?.Elements("Address") ?? Enumerable.Empty<XElement>())
                        {
                            var address = new AddressPoco
                            {
                                Name = xAddress.Get("Name"),
                                Line1 = xAddress.Get("Line1"),
                                City = xAddress.Get("City"),
                                CountryCode = xAddress.Get("CountryCode"),
                                CountryName = xAddress.Get("CountryName"),
                                RegionCode = xAddress.Get("RegionCode"),
                                RegionName = xAddress.Get("RegionName"),
                                PostalCode = xAddress.Get("PostalCode")
                            };

                            customer.Addresses.Add(address);
                        }

                        subOrganization.Users.Add(customer);
                    }

                    foreach (var xCreditCard in xSubOrganization.Element("CreditCards")?.Elements("CreditCard") ?? Enumerable.Empty<XElement>())
                    {
                        var cc = new CreditCardPoco
                        {
                            Number = xCreditCard.Get("Number"),
                            CardType = xCreditCard.Get("CardType"),
                            LastFour = xCreditCard.Get("LastFour"),
                            ExpirationYear = xCreditCard.GetInt("ExpirationYear"),
                            ExpirationMonth = xCreditCard.GetInt("ExpirationMonth")
                        };

                        subOrganization.CreditCards.Add(cc);
                    }

                    organization.SubOrganizations.Add(subOrganization);
                }

                SaveOrganization(organization);
            }
        }

        private void SaveOrganization(OrganizationPoco organization)
        {
            var org = Organization.CreateInstance();

            if (!organization.Id.IsNullOrEmpty())
            {
                org.PrimaryKeyId = new PrimaryKeyId(new Guid(organization.Id));
            }

            org.Name = organization.Name;
            org.OrganizationType = "Organization";
            org.SaveChanges();

            MapCreditCardsFromOrgToOrganization(organization.CreditCards, org);
            foreach (var user in organization.Users)
            {
                SaveCustomer(user, org.PrimaryKeyId.Value);
            }
            org.SaveChanges();

            foreach (var subOrganization in organization.SubOrganizations)
            {
                var subOrg = Organization.CreateInstance();

                if (!subOrganization.Id.IsNullOrEmpty())
                {
                    subOrg.PrimaryKeyId = new PrimaryKeyId(new Guid(subOrganization.Id));
                    subOrg.ParentId = org.PrimaryKeyId;
                }

                subOrg.Name = subOrganization.Name;
                subOrg.OrganizationType = "Organization Unit";
                subOrg.SaveChanges();

                MapCreditCardsFromOrgToOrganization(subOrganization.CreditCards, subOrg);
                foreach (var user in subOrganization.Users)
                {
                    SaveCustomer(user, subOrg.PrimaryKeyId.Value);
                }
                subOrg.SaveChanges();
            }
        }

        private void ProcessCustomers(FileStream stream)
        {
            foreach (var xCustomer in GetXElements(stream, "Customer"))
            {
                var customer = new CustomerPoco
                {
                    Email = xCustomer.Get("Email"),
                    FirstName = xCustomer.Get("FirstName"),
                    LastName = xCustomer.Get("LastName"),
                    Roles = xCustomer.GetEnumerable("Roles", ','),
                    ShowInDemoUserMenu = xCustomer.GetIntOrDefault("ShowInDemoUserMenu", 1),
                    DemoUserTitle = xCustomer.Get("DemoUserTitle"),
                    Location = xCustomer.Get("Location"),
                    DemoUserDescription = xCustomer.Get("DemoUserDescription"),
                    DemoSortOrder = xCustomer.GetIntOrDefault("DemoSort"),
                    Addresses = new List<AddressPoco>(),
                    CreditCards = new List<CreditCardPoco>()
                };

                foreach (var xAddress in xCustomer.Element("Addresses")?.Elements("Address") ?? Enumerable.Empty<XElement>())
                {
                    var address = new AddressPoco
                    {
                        Name = xAddress.Get("Name"),
                        Line1 = xAddress.Get("Line1"),
                        City = xAddress.Get("City"),
                        CountryCode = xAddress.Get("CountryCode"),
                        CountryName = xAddress.Get("CountryName"),
                        RegionCode = xAddress.Get("RegionCode"),
                        RegionName = xAddress.Get("RegionName"),
                        PostalCode = xAddress.Get("PostalCode")
                    };

                    customer.Addresses.Add(address);
                }

                foreach (var xCreditCard in xCustomer.Element("CreditCards")?.Elements("CreditCard") ?? Enumerable.Empty<XElement>())
                {
                    var cc = new CreditCardPoco
                    {
                        Number = xCreditCard.Get("Number"),
                        CardType = xCreditCard.Get("CardType"),
                        LastFour = xCreditCard.Get("LastFour"),
                        ExpirationYear = xCreditCard.GetInt("ExpirationYear"),
                        ExpirationMonth = xCreditCard.GetInt("ExpirationMonth")
                    };

                    customer.CreditCards.Add(cc);
                }

                SaveCustomer(customer, PrimaryKeyId.Empty);
            }
        }

        private void SaveCustomer(CustomerPoco customer, PrimaryKeyId orgId)
        {
            var user = _uIUserProvider.GetUserAsync(customer.Email)
                .GetAwaiter()
                .GetResult();

            if (user != null)
            {
                return;
            }

            CreateUser(customer.Email, customer.Email, customer.Roles)
                .GetAwaiter()
                .GetResult();

            FoundationContact foundationContact;
            var contact = CustomerContext.GetContactByUserId($"String:{customer.Email}");
            if (contact == null)
            {
                foundationContact = FoundationContact.New();
                foundationContact.UserId = customer.Email;
                foundationContact.Email = customer.Email;
            }
            else
            {
                foundationContact = new FoundationContact(contact);
            }

            foundationContact.FirstName = customer.FirstName;
            foundationContact.LastName = customer.LastName;
            foundationContact.FullName = $"{foundationContact.FirstName} {foundationContact.LastName}";
            foundationContact.RegistrationSource = "Imported customer";
            foundationContact.AcceptMarketingEmail = true;
            foundationContact.ConsentUpdated = DateTime.UtcNow;
            foundationContact.UserRole = customer.B2BRole;
            foundationContact.UserLocationId = customer.Location;
            foundationContact.DemoUserTitle = customer.DemoUserTitle;
            foundationContact.DemoUserDescription = customer.DemoUserDescription;
            foundationContact.ShowInDemoUserMenu = customer.ShowInDemoUserMenu == 0 ? 1 : customer.ShowInDemoUserMenu;
            foundationContact.DemoSortOrder = customer.DemoSortOrder;

            if (orgId != PrimaryKeyId.Empty)
            {
                foundationContact.Contact.OwnerId = orgId;
            }
            foundationContact.SaveChanges();

            MapAddressesFromCustomerToContact(customer, foundationContact.Contact);
            MapCreditCardsFromCustomerToContact(customer.CreditCards, foundationContact.Contact);
            foundationContact.SaveChanges();
        }

        private async Task CreateUser(string username, string email, IEnumerable<string> roles)
        {
            var result = await _uIUserProvider.CreateUserAsync(username, "Episerver123!", email, null, null, true);
            if (result.Status == UIUserCreateStatus.Success)
            {
                foreach (var role in roles)
                {
                    var exists = await _uIRoleProvider.RoleExistsAsync(role);
                    if (!exists)
                    {
                        await _uIRoleProvider.CreateRoleAsync(role);
                    }
                }

                await _uIRoleProvider.AddUserToRolesAsync(result.User.Username, roles);
            }
        }

        private static void MapAddressesFromCustomerToContact(CustomerPoco customer, CustomerContact contact)
        {
            foreach (var importedAddress in customer.Addresses)
            {
                var address = CustomerAddress.CreateInstance();

                address.Name = importedAddress.Name;
                address.City = importedAddress.City;
                address.CountryCode = importedAddress.CountryCode;
                address.CountryName = importedAddress.CountryName;
                address.FirstName = customer.FirstName;
                address.LastName = customer.LastName;
                address.Line1 = importedAddress.Line1;
                address.RegionCode = importedAddress.RegionCode;
                address.RegionName = importedAddress.RegionName;
                address.AddressType = CustomerAddressTypeEnum.Public | CustomerAddressTypeEnum.Shipping | CustomerAddressTypeEnum.Billing;

                contact.AddContactAddress(address);
            }
        }

        private static void MapCreditCardsFromCustomerToContact(List<CreditCardPoco> cards, CustomerContact contact)
        {
            foreach (var cc in cards)
            {
                var creditCard = CreditCard.CreateInstance();

                creditCard.CreditCardNumber = cc.Number;
                creditCard.CardType = 1;
                creditCard.LastFourDigits = cc.LastFour;
                creditCard.ExpirationMonth = cc.ExpirationMonth;
                creditCard.ExpirationYear = cc.ExpirationYear;
                contact.AddCreditCard(creditCard);
            }
        }

        private static void MapCreditCardsFromOrgToOrganization(List<CreditCardPoco> cards, Organization org)
        {
            foreach (var cc in cards)
            {
                var creditCard = CreditCard.CreateInstance();

                creditCard.CreditCardNumber = cc.Number;
                creditCard.CardType = 1;
                creditCard.LastFourDigits = cc.LastFour;
                creditCard.ExpirationMonth = cc.ExpirationMonth;
                creditCard.ExpirationYear = cc.ExpirationYear;
                creditCard.OrganizationId = org.PrimaryKeyId;
                BusinessManager.Create(creditCard);
            }
        }

        private class CustomerPoco
        {
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Location { get; set; }
            public IEnumerable<string> Roles { get; set; }
            public string B2BRole { get; set; }
            public string DemoUserTitle { get; set; }
            public string DemoUserDescription { get; set; }
            public int ShowInDemoUserMenu { get; set; }
            public int DemoSortOrder { get; set; }
            public List<AddressPoco> Addresses { get; set; }
            public List<CreditCardPoco> CreditCards { get; set; }
        }

        private class AddressPoco
        {
            public string Name { get; set; }
            public string Line1 { get; set; }
            public string City { get; set; }
            public string CountryCode { get; set; }
            public string CountryName { get; set; }
            public string RegionCode { get; set; }
            public string RegionName { get; set; }
            public string PostalCode { get; set; }
        }

        private class CreditCardPoco
        {
            public string Number { get; set; }
            public string CardType { get; set; }
            public string LastFour { get; set; }
            public int ExpirationYear { get; set; }
            public int ExpirationMonth { get; set; }
        }

        private class OrganizationPoco
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public List<CustomerPoco> Users { get; set; }
            public List<OrganizationPoco> SubOrganizations { get; set; }
            public List<CreditCardPoco> CreditCards { get; set; }
        }
    }
}
