using Foundation.Features.MyOrganization.Organization;

namespace Foundation.Features.Header
{
    public class OrgNavigationViewModel
    {
        public OrganizationModel Organization { get; set; }
        public OrganizationModel CurrentOrganization { get; set; }
        public ContentReference OrganizationPage { get; set; }
        public ContentReference SubOrganizationPage { get; set; }
    }
}