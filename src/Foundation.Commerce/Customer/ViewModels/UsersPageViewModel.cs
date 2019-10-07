using Foundation.Cms.ViewModels;
using Foundation.Commerce.Models.Pages;
using System.Collections.Generic;

namespace Foundation.Commerce.Customer.ViewModels
{
    public class UsersPageViewModel : ContentViewModel<UsersPage>
    {
        public List<FoundationContact> Users { get; set; }
        public FoundationContact Contact { get; set; }
        public List<FoundationOrganization> Organizations { get; set; }
        public SubFoundationOrganizationModel SubOrganization { get; set; }
    }
}
