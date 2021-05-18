using Castle.Core.Internal;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Framework.Localization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Foundation.Infrastructure.Cms.Users
{
    public class UserService : IUserService
    {
        private readonly LocalizationService _localizationService;

        public UserService(ApplicationSignInManager<SiteUser> signinManager,
            ApplicationUserManager<SiteUser> userManager,
            LocalizationService localizationService)
        {
            SignInManager = signinManager;
            _localizationService = localizationService;
            UserManager = userManager;
        }

        public virtual ApplicationUserManager<SiteUser> UserManager { get; }
        public virtual ApplicationSignInManager<SiteUser> SignInManager { get; }

        public Guid CurrentContactId => throw new NotImplementedException();

        public virtual async Task<SiteUser> GetSiteUser(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            return await UserManager.FindByEmailAsync(email);
        }

        public virtual async Task<SiteUser> GetSiteUserAsync(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            return await UserManager.FindByNameAsync(email);
        }

        public virtual async Task<ExternalLoginInfo> GetExternalLoginInfoAsync() => await SignInManager.GetExternalLoginInfoAsync();

        public virtual async Task<IdentityResult> CreateUserAsync(SiteUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (user.Password.IsNullOrEmpty())
            {
                throw new MissingFieldException("Password");
            }

            if (user.Email.IsNullOrEmpty())
            {
                throw new MissingFieldException("Email");
            }

            var result = new IdentityResult();
            if (await UserManager.FindByEmailAsync(user.Email) != null)
            {
                result = IdentityResult.Failed(new IdentityError { Description = _localizationService.GetString("/Registration/Form/Error/UsedEmail", "This email address is already used") });
            }
            else
            {
                result = await UserManager.CreateAsync(user, user.Password);

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, false);
                }
            }

            return result;
        }

        public virtual async Task SignOut()
        {
            await SignInManager.SignOutAsync();
            TrackingCookieManager.SetTrackingCookie(Guid.NewGuid().ToString());
        }
    }
}