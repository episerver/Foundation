using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace Foundation.Infrastructure.Cms.Users
{
    public class SiteUser : ApplicationUser
    {
        [NotMapped] public string FirstName { get; set; }

        [NotMapped] public string LastName { get; set; }
        [NotMapped] public DateTime? BirthDate { get; set; }

        [NotMapped] public string RegistrationSource { get; set; }

        [NotMapped] public string Password { get; set; }

        [NotMapped]
        public bool NewsLetter { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(IUserClaimsPrincipalFactory<SiteUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateAsync(this);
            var claimsIdentity = ((ClaimsIdentity)userIdentity.Identity);
            // Add custom user claims here
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, Email));

            if (!string.IsNullOrEmpty(FirstName))
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.GivenName, FirstName));
            }

            if (!string.IsNullOrEmpty(LastName))
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Surname, LastName));
            }

            return claimsIdentity;
        }
    }
}