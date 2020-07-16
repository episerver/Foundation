using Foundation.Features.Shared;

namespace Foundation.Features.MyOrganization.SubOrganization
{
    public class SubOrganizationPageViewModel : ContentViewModel<SubOrganizationPage>
    {
        public SubOrganizationModel SubOrganizationModel { get; set; }
        public bool IsAdmin { get; set; }
    }
}
