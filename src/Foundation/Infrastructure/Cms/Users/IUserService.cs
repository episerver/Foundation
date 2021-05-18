using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Foundation.Infrastructure.Cms.Users
{
    public interface IUserService
    {
        ApplicationUserManager<SiteUser> UserManager { get; }
        ApplicationSignInManager<SiteUser> SignInManager { get; }
        Guid CurrentContactId { get; }
        Task<SiteUser> GetSiteUserAsync(string email);
        Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
        Task<IdentityResult> CreateUserAsync(SiteUser user);
        Task SignOut();
    }
}
