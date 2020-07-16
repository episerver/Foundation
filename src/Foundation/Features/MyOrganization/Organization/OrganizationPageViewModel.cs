using EPiServer.Core;
using Foundation.Features.MyOrganization.SubOrganization;
using Foundation.Features.Shared;

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
