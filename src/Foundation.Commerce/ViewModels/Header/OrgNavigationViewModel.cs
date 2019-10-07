using EPiServer.Core;
using Foundation.Commerce.Customer.ViewModels;

namespace Foundation.Commerce.ViewModels.Header
{
    public class OrgNavigationViewModel
    {
        public OrganizationModel Organization { get; set; }
        public OrganizationModel CurrentOrganization { get; set; }

        public ContentReference OrganizationPage { get; set; }

        public ContentReference SubOrganizationPage { get; set; }
    }
}