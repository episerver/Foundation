using Foundation.Features.MyOrganization.SubOrganization;

namespace Foundation.Features.MyOrganization.Organization
{
    public class OrganizationPageViewModel : ContentViewModel<OrganizationPage>
    {
        public OrganizationModel Organization { get; set; }
        public SubOrganizationModel NewSubOrganization { get; set; }
        public ContentReference SubOrganizationPage { get; set; }
        public bool IsAdmin { get; set; }
    }
}
