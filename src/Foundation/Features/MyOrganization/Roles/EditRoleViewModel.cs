using Foundation.Cms.ViewModels;
using Foundation.Commerce.Customer;
using Foundation.Commerce.Models.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Foundation.Features.MyOrganization.Roles
{
    public class EditRoleViewModel : ContentViewModel<RolesPage>
    {
        public RoleViewModel Role { get; set; }
        public List<PermissionViewModel> Permissions { get; set; }

        public EditRoleViewModel(RolesPage currentPage) : base(currentPage)
        {
            Role = new RoleViewModel();
            Permissions = new List<PermissionViewModel>();

            var permissions = Enum.GetValues(typeof(B2BPermissions));
            foreach (var p in permissions)
            {
                var permission = ((B2BPermissions)p);
                if (permission == B2BPermissions.Default) continue;

                var pInfo = permission.GetType().GetMember(p.ToString())[0];
                var desAttr = pInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
                var perDes = desAttr == null ? permission.ToString() : desAttr.Description;

                Permissions.Add(new PermissionViewModel { Permission = permission, Description = perDes });
            }
        }
    }
}