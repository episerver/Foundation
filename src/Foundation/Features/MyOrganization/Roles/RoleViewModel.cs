using EPiServer.Social.Groups.Core;
using Foundation.Commerce.Customer;
using System.Collections.Generic;

namespace Foundation.Features.MyOrganization.Roles
{
    public class RoleViewModel
    {
        public RoleId Id { get; set; }
        public string Name { get; set; }
        public List<PermissionViewModel> Permissions { get; set; }
        public bool IsDefault { get; set; }

        public RoleViewModel()
        {
            Permissions = new List<PermissionViewModel>();
            IsDefault = false;
        }
    }

    public class PermissionViewModel
    {
        public B2BPermissions Permission { get; set; }
        public string Description { get; set; }
    }
}