using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Foundation.Cms.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

namespace Foundation.Cms.Extensions
{
    public static class AppBuilderExtensions
    {
        public static void ConfigureAuthentication(this IAppBuilder app, string commerceConectionStringName)
        {
            app.AddCmsAspNetIdentity<SiteUser>(new ApplicationOptions
            {
                ConnectionStringName = commerceConectionStringName
            });

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider.
            // Configure the sign in cookie.
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/util/Login.aspx"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity =
                        SecurityStampValidator.OnValidateIdentity<ApplicationUserManager<SiteUser>, SiteUser>(
                            TimeSpan.FromMinutes(30),
                            (manager, user) => manager.GenerateUserIdentityAsync(user)),
                    OnApplyRedirect = context => context.Response.Redirect(context.RedirectUri),
                    OnResponseSignOut = context =>
                        context.Response.Redirect(UrlResolver.Current.GetUrl(ContentReference.StartPage))
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
        }
    }
}