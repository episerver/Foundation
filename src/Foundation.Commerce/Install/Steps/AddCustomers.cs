using EPiServer;
using EPiServer.Cms.UI.AspNetIdentity;
using Foundation.Cms.Extensions;
using Foundation.Cms.Identity;
using Foundation.Commerce.Customer;
using Mediachase.BusinessFoundation.Core;
using Mediachase.BusinessFoundation.Data;
using Mediachase.BusinessFoundation.Data.Business;
using Mediachase.BusinessFoundation.Data.Meta.Management;
using Mediachase.BusinessFoundation.Data.Modules;
using Mediachase.BusinessFoundation.MetaForm;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Core.RecentReferenceHistory;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Shared;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Foundation.Commerce.Install.Steps
{
    public class AddCustomers : BaseInstallStep
    {
        private readonly XmlSerializer _listViewProfileXmlSerializer = new XmlSerializer(typeof(ListViewProfile), new Type[]{
            typeof(int[]),
            typeof(double[]),
            typeof(decimal[]),
            typeof(PrimaryKeyId[]),
            typeof(string[]),
            typeof(DateTime[]),
            typeof(object[])
        });

        //No OWIN Context yet so we cannot use Constructor injection for these two
        private readonly ApplicationUserManager<SiteUser> _userManager =
            new ApplicationUserManager<SiteUser>(new UserStore<SiteUser>(new ApplicationDbContext<SiteUser>("EcfSqlConnection")));

        private readonly ApplicationRoleManager<SiteUser> _roleManager =
            new ApplicationRoleManager<SiteUser>(new RoleStore<IdentityRole>(new ApplicationDbContext<SiteUser>("EcfSqlConnection")));

        public AddCustomers(IContentRepository contentRepository,
            ReferenceConverter referenceConverter,
            IMarketService marketService) : base(contentRepository, referenceConverter, marketService)
        {
        }

        public override int Order => 8;

        public override string Description => "Adds customers to Foundation.";

        protected override void ExecuteInternal(IProgressMessenger progressMessenger)
        {
            using (var scope = DataContext.Current.MetaModel.BeginEdit(MetaClassManagerEditScope.SystemOwner, Mediachase.BusinessFoundation.Data.Meta.Management.AccessLevel.System))
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

                AddMetaFieldToForms(contactMetaClass, contactMetaClass.Fields["ShowInDemoUserMenu"]);
                AddMetaFieldToForms(contactMetaClass, contactMetaClass.Fields["DemoUserTitle"]);
                AddMetaFieldToForms(contactMetaClass, contactMetaClass.Fields["DemoUserDescription"]);
                AddMetaFieldToForms(contactMetaClass, contactMetaClass.Fields["DemoSortOrder"]);

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
                    var contactReference = builder.CreateReference("Contact", "{Customer:CreditCard_mf_Contact}", true, "Contact", false);
                    contactReference.Attributes.Add(McDataTypeAttribute.ReferenceDisplayBlock, "InfoBlock");
                    contactReference.Attributes.Add(McDataTypeAttribute.ReferenceDisplayText, "{Customer:GiftCard}");
                    contactReference.Attributes.Add(McDataTypeAttribute.ReferenceDisplayOrder, "10000");
                    builder.SaveChanges();
                }
                giftCardClass.AddPermissions();

                AddMetaFieldToForms(giftCardClass, giftCardClass.Fields["GiftCardName"]);
                AddMetaFieldToForms(giftCardClass, giftCardClass.Fields["InitialAmount"]);
                AddMetaFieldToForms(giftCardClass, giftCardClass.Fields["RemainBalance"]);
                AddMetaFieldToForms(giftCardClass, giftCardClass.Fields["RedemptionCode"]);
                AddMetaFieldToForms(giftCardClass, giftCardClass.Fields["IsActive"]);
                AddMetaFieldToForms(giftCardClass, giftCardClass.Fields["ContactId"]);

                scope.SaveChanges();
            }

            var contactProfile = @"<ListViewProfile xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                    <Id>{54a649a9-302f-48bd-b657-11ca3604fda9}</Id>
                    <Name>{Customer:AllContacts}</Name>
                    <IsSystem>true</IsSystem>
                    <IsPublic>true</IsPublic>
                    <FieldSetName>Contacts</FieldSetName>
                   <FieldSet>
                    <string>FullName</string>
                    <string>Email</string>
                    <string>LastOrder</string>
                    <string>NumberOfOrders</string>
                    <string>NumberOfReviews</string>
                    <string>Points</string>
                    <string>ShowInDemoUserMenu</string>
                    <string>DemoUserTitle</string>
                  </FieldSet>
                  <GroupByFieldName />
                  <Filters />
                  <Sorting />
                  <ColumnsUI>
                    <Column field=""FullName"" width=""150"" />
                    <Column field=""Email"" width=""150"" />
                    <Column field=""LastOrder"" width=""150"" />
                    <Column field=""NumberOfOrders"" width=""150"" />
                    <Column field=""NumberOfReviews"" width=""150"" />
                    <Column field=""Points"" width=""150"" />
                    <Column field=""ShowInDemoUserMenu"" width=""150"" />
                    <Column field=""DemoUserTitle"" width=""150"" />
                  </ColumnsUI>
                </ListViewProfile>";

            var contactviewProfile = (ListViewProfile)_listViewProfileXmlSerializer.Deserialize(new StringReader(contactProfile));
            ListViewProfile.SaveSystemProfile("Contact", "EntityList", Guid.NewGuid(), contactviewProfile);

            using (var stream = new FileStream(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"App_Data\Customers.xml"), FileMode.Open))
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
            var user = _userManager.FindByEmailAsync(customer.Email)
                .GetAwaiter()
                .GetResult();

            if (user == null)
            {
                user = new SiteUser
                {
                    CreationDate = DateTime.UtcNow,
                    Username = customer.Email,
                    Email = customer.Email,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    IsApproved = true
                };

                var result = _userManager.CreateAsync(user, "Episerver123!")
                .GetAwaiter()
                .GetResult();

                if (!result.Succeeded)
                {
                    return;
                }
            }

            foreach (var role in customer.Roles)
            {
                if (!_roleManager.RoleExistsAsync(role)
                    .GetAwaiter()
                    .GetResult())
                {
                    var createdRole = new IdentityRole(role);

                    var roleResult = _roleManager.CreateAsync(createdRole)
                        .GetAwaiter()
                        .GetResult();

                    if (!roleResult.Succeeded)
                    {
                        continue;
                    }
                    _userManager.AddToRoleAsync(user.Id, role)
                        .GetAwaiter()
                        .GetResult();
                }
            }

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

        private static void AddMetaFieldToForms(MetaClass metaClass, MetaField newField)
        {
            var formNames = new string[] { FormController.BaseFormType, FormController.GeneralViewFormType, FormController.ShortViewFormType, FormController.CreateFormType };
            foreach (var formName in formNames)
            {
                FormController.AddMetaPrimitive(metaClass.Name, formName, newField.Name);
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
