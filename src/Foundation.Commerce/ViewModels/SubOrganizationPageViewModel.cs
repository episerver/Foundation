using Foundation.Cms.ViewModels;
using Foundation.Commerce.Customer.ViewModels;
using Foundation.Commerce.Models.Pages;

namespace Foundation.Commerce.ViewModels
{
    public class SubOrganizationPageViewModel : ContentViewModel<SubOrganizationPage>
    {
        public SubOrganizationModel SubOrganizationModel { get; set; }
        public bool IsAdmin { get; set; }
    }
}
