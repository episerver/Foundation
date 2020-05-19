using System.Collections.Generic;

namespace Foundation.Features.MyOrganization.Roles
{
    public class EditRoleModel
    {
        public string RoleId { get; set; }
        public string Name { get; set; }
        public List<string> Permissions { get; set; }
    }
}