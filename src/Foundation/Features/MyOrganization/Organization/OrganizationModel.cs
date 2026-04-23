using Foundation.Features.MyOrganization.Budgeting;
using Foundation.Infrastructure.Commerce;
using Foundation.Infrastructure.Commerce.Customer;
using Mediachase.Commerce.Customers;

namespace Foundation.Features.MyOrganization.Organization
{
    public class OrganizationModel
    {
        public OrganizationModel(FoundationOrganization organization)
        {
            if (organization != null)
            {
                OrganizationId = organization.OrganizationId;
                Name = organization.Name;
                Address = organization.Address != null ? new B2BAddressViewModel(organization.Address) : null;
                SubOrganizations = organization.SubOrganizations != null && organization.SubOrganizations.Any()
                    ? organization.SubOrganizations.Select(subOrg => new OrganizationModel(subOrg)).ToList()
                    : new List<OrganizationModel>();
                ParentOrganizationId = organization.ParentOrganizationId;
                var contact =
                    organization.OrganizationEntity.Contacts.FirstOrDefault(x =>
                        x.GetStringValue(Constant.Fields.UserRole) == "Admin")
                    ?? organization.OrganizationEntity.Contacts.FirstOrDefault();

                MainContact = contact;
            }
        }

        public OrganizationModel()
        {
        }

        public Guid OrganizationId { get; set; }

        [LocalizedDisplay("/B2B/Organization/OrganizationName")]
        [LocalizedRequired("/B2B/Organization/OrganizationNameRequired")]
        public string Name { get; set; }

        public B2BAddressViewModel Address { get; set; }
        public List<OrganizationModel> SubOrganizations { get; set; }
        public Guid ParentOrganizationId { get; set; }
        public OrganizationModel ParentOrganization { get; set; }
        public BudgetViewModel CurrentBudgetViewModel { get; set; }
        public CustomerContact MainContact { get; set; }
    }
}