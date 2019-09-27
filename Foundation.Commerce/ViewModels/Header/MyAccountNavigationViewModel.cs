using EPiServer.Core;
using EPiServer.SpecializedProperties;
using Foundation.Commerce.Customer.ViewModels;

namespace Foundation.Commerce.ViewModels.Header
{
    public enum MyAccountPageType
    {
        Link,
        Organization,
    }

    public class MyAccountNavigationViewModel
    {
        public OrganizationModel Organization { get; set; }
        public OrganizationModel CurrentOrganization { get; set; }
        public ContentReference OrganizationPage { get; set; }
        public ContentReference SubOrganizationPage { get; set; }
        public MyAccountPageType CurrentPageType { get; set; }
        public string CurrentPageText { get; set; }
        public LinkItemCollection MenuItemCollection { get; set; }
    }
}