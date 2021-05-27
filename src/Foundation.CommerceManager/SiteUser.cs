using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNet.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Foundation.CommerceManager
{
    public class SiteUser : ApplicationUser
    {
        [NotMapped] public string FirstName { get; set; }

        [NotMapped] public string LastName { get; set; }
        [NotMapped] public DateTime? BirthDate { get; set; }

        [NotMapped] public string RegistrationSource { get; set; }

        [NotMapped] public string Password { get; set; }

        public bool NewsLetter { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<SiteUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            userIdentity.AddClaim(new Claim(ClaimTypes.Email, Email));

            if (!string.IsNullOrEmpty(FirstName))
            {
                userIdentity.AddClaim(new Claim(ClaimTypes.GivenName, FirstName));
            }

            if (!string.IsNullOrEmpty(LastName))
            {
                userIdentity.AddClaim(new Claim(ClaimTypes.Surname, LastName));
            }

            return userIdentity;
        }
    }
}