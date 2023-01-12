using EPiServer.Cms.UI.AspNetIdentity;
using Foundation.Infrastructure.Cms.Users;
using Foundation.Infrastructure.Commerce.Customer.Services;

namespace Foundation.Features.MyAccount
{
    /// <summary>
    /// Base class for controllers related to ASP.NET Identity. This controller can be used both for
    /// pages and blocks.
    /// </summary>
    /// <typeparam name="T">The contextual IContent related to the current page or block.</typeparam>
    [AuthorizeContent]
    [VisitorGroupImpersonation]
    public abstract class IdentityControllerBase<T> : ActionControllerBase, IRenderTemplate<T> where T : IContentData
    {
        protected IdentityControllerBase(ApplicationSignInManager<SiteUser> applicationSignInManager, ApplicationUserManager<SiteUser> applicationUserManager, ICustomerService customerService)
        {
            SignInManager = applicationSignInManager;
            UserManager = applicationUserManager;
            CustomerService = customerService;
        }

        public ICustomerService CustomerService { get; }

        public ApplicationSignInManager<SiteUser> SignInManager { get; }

        public ApplicationUserManager<SiteUser> UserManager { get; }

        /// <summary>
        /// Redirects the request to the original URL.
        /// </summary>
        /// <param name="returnUrl">The URL to be redirected to.</param>
        /// <returns>The ActionResult of the URL if it is within the current application, else it
        /// redirects to the web application start page.</returns>
        public ActionResult RedirectToLocal(string returnUrl)
        {
            if (returnUrl.IsLocalUrl(Request))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", new { node = ContentReference.StartPage });
        }

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await CustomerService.SignOutAsync();
            return RedirectToAction("Index", new { node = ContentReference.StartPage });
        }

        public void AddErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        private bool _disposed;
        protected override void Dispose(bool disposing)
        {
            if (!disposing || _disposed)
            {
                return;
            }

            if (UserManager != null)
            {
                UserManager.Dispose();
            }

            //if (SignInManager != null)
            //{
            //    SignInManager.Dispose();
            //}

            base.Dispose(disposing);

            _disposed = true;
        }
    }
}