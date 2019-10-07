using EPiServer.Core;
using Foundation.Cms.ViewModels;
using Foundation.Commerce.Customer.ViewModels;
using Foundation.Commerce.Models.Pages;

namespace Foundation.Commerce.ViewModels
{
    public class OrganizationPageViewModel : ContentViewModel<OrganizationPage>
    {
        public OrganizationModel Organization { get; set; }
        public SubOrganizationModel NewSubOrganization { get; set; }
        public ContentReference SubOrganizationPage { get; set; }
        public bool IsAdmin { get; set; }
    }
}
