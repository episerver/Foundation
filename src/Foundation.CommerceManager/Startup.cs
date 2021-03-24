using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.ServiceLocation;
using Foundation.CommerceManager;
using Mediachase.Data.Provider;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

[assembly: OwinStartup(typeof(Startup))]
namespace Foundation.CommerceManager
{
    public class Startup
    {
        private readonly IConnectionStringHandler _connectionStringHandler;

        public Startup() : this(ServiceLocator.Current.GetInstance<IConnectionStringHandler>())
        {
            // Parameterless constructor required by OWIN.
        }

        public Startup(IConnectionStringHandler connectionStringHandler) => _connectionStringHandler = connectionStringHandler;

        public void Configuration(IAppBuilder app)
        {
            app.AddCmsAspNetIdentity<SiteUser>(new ApplicationOptions
            {
                ConnectionStringName = _connectionStringHandler.Commerce.Name
            });

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider.
            // Configure the sign in cookie.
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Apps/Shell/Pages/Login.aspx"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager<SiteUser>, SiteUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => manager.GenerateUserIdentityAsync(user)),
                    OnApplyRedirect = (context => redirectMe(context))
                }
            });
        }

        private void redirectMe(CookieApplyRedirectContext context)
        {
            var redirect = context.RedirectUri;
            if (context.Request.Headers.ContainsKey("X-Forwarded-Proto") && context.Request.Headers["X-Forwarded-Proto"].ToString() != null)
            {
                redirect = redirect.Replace("http://", context.Request.Headers["X-Forwarded-Proto"].ToString() + "://");
            }
            context.Response.Redirect(redirect);
        }
    }
}