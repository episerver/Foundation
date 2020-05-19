using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using EPiServer.Web.Mvc;
using Foundation.Commerce.Customer;
using Foundation.Commerce.Models.Pages;

namespace Foundation.Features.MyOrganization.Roles
{
    public class RolesController : PageController<RolesPage>
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public ActionResult Index(RolesPage currentPage)
        {
            var roleCriteria = new CompositeCriteria<RoleFilter, RoleExtension>
            {
                ExtensionFilter = FilterExpressionBuilder<RoleExtension>.Field(x => x.Group).EqualTo("[B2B]"),
                OrderBy = new List<SortInfo>
                {
                    new SortInfo(RoleSortFields.Name, true)
                }
            };
            var roles = _roleService.Get(roleCriteria).Results.ToList();
            var roleViewModels = new List<RoleViewModel>();
            var permissions = Enum.GetValues(typeof(B2BPermissions));

            // get added roles
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    roleViewModels.Add(GetRoleViewModel(role));
                }
            }

            // get default roles
            var defaultRoles = Enum.GetValues(typeof(B2BUserRoles));
            foreach(var role in defaultRoles)
            {
                roleViewModels.Add(new RoleViewModel
                {
                    Name = role.ToString(),
                    Permissions = new List<PermissionViewModel> {
                        new PermissionViewModel { Permission = B2BPermissions.Default }
                    },
                    IsDefault = true
                });
            }

            var viewModel = new RolesPageViewModel(currentPage);
            viewModel.Roles = roleViewModels;
            return View(viewModel);
        }

        public ActionResult EditRole(RolesPage currentPage, string roleId = "")
        {
            var viewModel = new EditRoleViewModel(currentPage);
            if (string.IsNullOrEmpty(roleId))
            {
                return View(viewModel);
            }

            var role = _roleService.Get<RoleExtension>(new RoleId(roleId));
            var roleModel = GetRoleViewModel(role);
            viewModel.Role = roleModel;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CreateOrUpdateRole(RolesPage currentPage, EditRoleModel role)
        {
            // create
            if (string.IsNullOrEmpty(role.RoleId))
            {
                var existedRoleName = _roleService.Get(new Criteria<RoleFilter> { Filter = { Name = role.Name } }).Results != null;
                if (existedRoleName)
                {
                    return RedirectToAction("Index");
                }

                var newRole = new Role(role.Name);
                var newRoleExtension = new RoleExtension { Group = "[B2B]", Permissions = string.Join(",", role.Permissions) };
                _roleService.Add(newRole, newRoleExtension);
                return RedirectToAction("Index");
            }

            // edit
            var updateRole = _roleService.Get(new RoleId(role.RoleId));
            var updateRoleExtension = new RoleExtension { Group = "[B2B]", Permissions = string.Join(",", role.Permissions) };
            _roleService.Update(updateRole.Id, updateRoleExtension);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteRole(string roleId)
        {
            _roleService.Remove(new RoleId(roleId));
            return RedirectToAction("Index");
        }

        private RoleViewModel GetRoleViewModel(Composite<Role, RoleExtension> role)
        {
            var splitRolePers = role.Extension.Permissions.Split(',');

            // get permission of the role
            var rolePermissions = new List<PermissionViewModel>();
            if (splitRolePers.Any())
            {
                foreach (var p in splitRolePers)
                {
                    var permission = (B2BPermissions)Enum.Parse(typeof(B2BPermissions), p);
                    var pInfo = permission.GetType().GetMember(p)[0];
                    var desAttr = pInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
                    var perDes = desAttr == null ? permission.ToString() : desAttr.Description;

                    rolePermissions.Add(new PermissionViewModel { Permission = permission, Description = perDes });
                }
            }

            return new RoleViewModel
            {
                Id = role.Data.Id,
                Name = role.Data.Name,
                Permissions = rolePermissions
            };
        }
    }
}