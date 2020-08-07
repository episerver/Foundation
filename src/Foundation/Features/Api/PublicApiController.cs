using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.Web.Routing;
using Foundation.Cms;
using Foundation.Cms.Extensions;
using Foundation.Cms.Identity;
using Foundation.Commerce.Customer.Services;
using Foundation.Features.Login;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Infrastructure.Services;
using Foundation.Personalization;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Foundation.Features.Api
{
    public class PublicApiController : Controller
    {
        private readonly IContentLoader _contentLoader;
        private readonly IAddressBookService _addressBookService;
        private readonly LocalizationService _localizationService;
        private readonly ICustomerService _customerService;
        private readonly ICampaignService _campaignService;
        private readonly IUrlResolver _urlResolver;
        private readonly ICmsTrackingService _cmsTrackingService;
        private readonly HttpContextBase _httpContextBase;

        public PublicApiController(LocalizationService localizationService,
            IContentLoader contentLoader,
            IAddressBookService addressBookService,
            ICustomerService customerService,
            ICampaignService campaignService,
            IUrlResolver urlResolver,
            ICmsTrackingService cmsTrackingService,
            HttpContextBase httpContextBase)
        {
            _localizationService = localizationService;
            _contentLoader = contentLoader;
            _customerService = customerService;
            _addressBookService = addressBookService;
            _campaignService = campaignService;
            _urlResolver = urlResolver;
            _cmsTrackingService = cmsTrackingService;
            _httpContextBase = httpContextBase;
        }

        [HttpGet]
        public ActionResult SignOut()
        {
            _customerService.SignOut();
            TrackingCookieManager.SetTrackingCookie(Guid.NewGuid().ToString());
            return RedirectToAction("Index", new { node = ContentReference.StartPage });
        }

        [HttpGet]
        public ActionResult Login(string userName, string returnUrl)
        {
            _customerService.SignOut();
            var user = _customerService.UserManager().FindByEmailAsync(userName).GetAwaiter().GetResult();
            if (user == null)
            {
                return new EmptyResult();
            }

            _customerService.SignInManager().SignIn(user, true, true);

            //set tracking cookie
            //TrackingCookieManager.SetTrackingCookie(_pageTrackingService.GetUserTrackingId(userName));

            return Redirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterAccount(RegisterAccountViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    errors = ModelState.Keys
                        .SelectMany(k => ModelState[k].Errors)
                        .Select(m => m.ErrorMessage).ToArray()
                });
            }

            viewModel.Address.BillingDefault = true;
            viewModel.Address.ShippingDefault = true;
            viewModel.Address.Email = viewModel.Email;

            var user = new SiteUser
            {
                UserName = viewModel.Email,
                Email = viewModel.Email,
                Password = viewModel.Password,
                FirstName = viewModel.Address.FirstName,
                LastName = viewModel.Address.LastName,
                RegistrationSource = "Registration page",
                NewsLetter = viewModel.Newsletter,
                IsApproved = true
            };

            var registration = await _customerService.CreateUser(user);

            if (registration.IdentityResult.Succeeded)
            {
                if (registration.FoundationContact != null)
                {
                    _addressBookService.Save(viewModel.Address, registration.FoundationContact);
                }

                return new EmptyResult();
            }
            else
            {
                return Json(new
                {
                    success = false,
                    errors = registration.IdentityResult.Errors
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> InternalLogin(LoginViewModel viewModel)
        {
            var returnUrl = GetSafeReturnUrl(Request.UrlReferrer);

            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    errors = ModelState.Keys
                        .SelectMany(k => ModelState[k].Errors)
                        .Select(m => m.ErrorMessage).ToArray()
                });
            }
            var user = _customerService.GetSiteUser(viewModel.Email);
            if (user != null)
            {
                var result = await _customerService.SignInManager().PasswordSignInAsync(user.UserName, viewModel.Password, viewModel.RememberMe, shouldLockout: true);
                switch (result)
                {
                    case SignInStatus.Success:
                        _campaignService.UpdateLastLoginDate(viewModel.Email);
                        break;

                    case SignInStatus.LockedOut:
                        throw new Exception("Account is locked out.");

                    default:
                        ModelState.AddModelError("LoginViewModel.Password", _localizationService.GetString("/Login/Form/Error/WrongPasswordOrEmail", "You have entered wrong username or password"));
                        return Json(new
                        {
                            success = false,
                            errors = ModelState.Keys
                             .SelectMany(k => ModelState[k].Errors)
                             .Select(m => m.ErrorMessage).ToArray()
                        });
                }

                return Json(new
                {
                    success = true,
                    returnUrl = viewModel.ReturnUrl
                });
            }

            ModelState.AddModelError("LoginViewModel.Password", _localizationService.GetString("/Login/Form/Error/WrongPasswordOrEmail", "You have entered wrong username or password"));
            return Json(new
            {
                success = false,
                errors = ModelState.Keys
                 .SelectMany(k => ModelState[k].Errors)
                 .Select(m => m.ErrorMessage).ToArray()
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl) =>
            // Request a redirect to the external login provider
            new ChallengeResult(provider, Url.Action("ExternalLoginCallback", new { returnUrl }));

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await _customerService.GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                return Redirect("/");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await _customerService.SignInManager().ExternalSignInAsync(loginInfo, isPersistent: false);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:
                    return RedirectToAction("Lockout", "Login");

                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", "Login", new { ReturnUrl = returnUrl, RememberMe = false });
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;

                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { ReturnUrl = returnUrl });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel viewModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var socialLoginDetails = await _customerService.GetExternalLoginInfoAsync();
                if (socialLoginDetails == null)
                {
                    return View("ExternalLoginFailure");
                }

                var eMail = socialLoginDetails.ExternalIdentity.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Email))?.Value;
                var names = socialLoginDetails.ExternalIdentity.Name.Split(' ');
                var firstName = names[0];
                var lastName = names.Length > 1 ? names[1] : string.Empty;

                var user = new SiteUser
                {
                    UserName = eMail,
                    Email = eMail,
                    FirstName = firstName,
                    LastName = lastName,
                    RegistrationSource = "Social login",
                    NewsLetter = viewModel.Newsletter,
                    IsApproved = true
                };

                var result = await _customerService.CreateUser(user);
                if (result.IdentityResult.Succeeded)
                {
                    var identityResult = await _customerService.UserManager().AddLoginAsync(user.Id, socialLoginDetails.Login);
                    if (identityResult.Succeeded)
                    {
                        await _customerService.SignInManager().SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(viewModel.ReturnUrl);
                    }
                }

                AddErrors(result.IdentityResult.Errors);
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult TrackHeroBlock(string blockId, string blockName, string pageName)
        {
            _cmsTrackingService.HeroBlockClicked(_httpContextBase, blockId, blockName, pageName);
            return new ContentResult()
            {
                Content = blockName
            };
        }

        [HttpPost]
        public ActionResult TrackVideoBlock(string blockId, string blockName, string pageName)
        {
            _cmsTrackingService.VideoBlockViewed(_httpContextBase, blockId, blockName, pageName);
            return new ContentResult()
            {
                Content = blockName
            };
        }

        private void AddErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (returnUrl.IsLocalUrl(Request))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", new { node = ContentReference.StartPage });
        }

        private string GetSafeReturnUrl(Uri referrer)
        {
            //Make sure we only return to relative url.
            var returnUrl = HttpUtility.ParseQueryString(referrer.Query)["returnUrl"];
            if (string.IsNullOrEmpty(returnUrl))
            {
                return "/";
            }

            if (Uri.TryCreate(returnUrl, UriKind.Absolute, out var uri))
            {
                return uri.PathAndQuery;
            }
            return returnUrl;
        }

    }
}