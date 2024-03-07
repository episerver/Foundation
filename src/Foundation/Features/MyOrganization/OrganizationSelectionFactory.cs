using Mediachase.Commerce.Customers;

namespace Foundation.Features.MyOrganization
{
    public class OrganizationSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var organizationsList = CustomerContext.Current.GetOrganizations();

            return organizationsList.Select(organization => new SelectItem
            {
                Value = organization.Name,
                Text = organization.Name
            }).ToList();
        }
    }
}