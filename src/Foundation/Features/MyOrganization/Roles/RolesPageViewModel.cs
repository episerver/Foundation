using Foundation.Cms.ViewModels;
using Foundation.Commerce.Models.Pages;
using System.Collections.Generic;

namespace Foundation.Features.MyOrganization.Roles
{
    public class RolesPageViewModel : ContentViewModel<RolesPage>
    {
        public List<RoleViewModel> Roles { get; set; }

        public RolesPageViewModel(RolesPage currentPage) : base(currentPage) 
        {
            Roles = new List<RoleViewModel>();
        }
    }
}