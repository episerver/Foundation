using Foundation.Features.Login;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Infrastructure.Cms;
using Foundation.Infrastructure.Cms.Users;
using Foundation.Infrastructure.Commerce.Customer.Services;
using Foundation.Infrastructure.Personalization;
using System.Web;

namespace Foundation.Features.Api
{
    public class PublicApiController : Controller
    {
        private readonly IContentLoader _contentLoader;
        private readonly IAddressBookService _addressBookService;
        private readonly LocalizationService _localizationService;
        private readonly ICustomerService _customerService;
        //private readonly ICampaignService _campaignService;
        private readonly IUrlResolver _urlResolver;
        private readonly ICmsTrackingService _cmsTrackingService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PublicApiController(LocalizationService localizationService,
            IContentLoader contentLoader,
            IAddressBookService addressBookService,
            ICustomerService customerService,
            //ICampaignService campaignService,
            IUrlResolver urlResolver,
            ICmsTrackingService cmsTrackingService,
            IHttpContextAccessor httpContextAccessor)
        {
            _localizationService = localizationService;
            _contentLoader = contentLoader;
            _customerService = customerService;
            _addressBookService = addressBookService;
            //_campaignService = campaignService;
            _urlResolver = urlResolver;
            _cmsTrackingService = cmsTrackingService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        new public async Task<IActionResult> SignOut()
        {
            await _customerService.SignOutAsync();
            TrackingCookieManager.SetTrackingCookie(Guid.NewGuid().ToString());
            return Redirect("~/");
        }

        [HttpGet]
        public async Task<IActionResult> Login(string userName, string returnUrl)
        {
            await _customerService.SignOutAsync();
            var user = _customerService.UserManager().FindByEmailAsync(userName).GetAwaiter().GetResult();
            if (user == null)
            {
                return new EmptyResult();
            }

            await _customerService.SignInManager().SignInAsync(user.UserName, "Episerver123!", returnUrl);

            //set tracking cookie
            TrackingCookieManager.SetTrackingCookie(user.Id);

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
                IsApproved = true
            };

            var registration = await _customerService.CreateUser(user);

            if (registration.IdentityResult.Succeeded)
            {
                if (registration.FoundationContact != null)
                {
                    _addressBookService.Save(viewModel.Address, registration.FoundationContact);
                }
                TrackingCookieManager.SetTrackingCookie(registration.FoundationContact.UserId);

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
            var returnUrl = GetSafeReturnUrl(Request.GetTypedHeaders().Referer);

            if (!ModelState.IsValid)
            {
                return View("~/Features/Login/Index.cshtml", Url.GetUserViewModel(returnUrl));
            }
            var user = await _customerService.GetSiteUserAsync(viewModel.Email);
            if (user != null)
            {
                var result = await _customerService.SignInManager().SignInAsync(user.UserName, viewModel.Password, returnUrl);
                if (!result)
                {
                    ModelState.AddModelError("LoginViewModel.Password", _localizationService.GetString("/Login/Form/Error/WrongPasswordOrEmail", "You have entered wrong username or password"));
                    return View("~/Features/Login/Index.cshtml", Url.GetUserViewModel(returnUrl));
                }

                return Redirect(returnUrl);
            }

            ModelState.AddModelError("LoginViewModel.Password", _localizationService.GetString("/Login/Form/Error/WrongPasswordOrEmail", "You have entered wrong username or password"));
            return View("~/Features/Login/Index.cshtml", Url.GetUserViewModel(returnUrl));
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLogin(string provider, string returnUrl) =>
        //    // Request a redirect to the external login provider
        //    new ChallengeResult(provider, Url.Action("ExternalLoginCallback", new { returnUrl }));

        //[AllowAnonymous]
        //public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //{
        //    var loginInfo = await _customerService.GetExternalLoginInfoAsync();

        //    if (loginInfo == null)
        //    {
        //        return Redirect("/");
        //    }

        //    // Sign in the user with this external login provider if the user already has a login
        //    var result = await _customerService.SignInManager().ExternalSignInAsync(loginInfo, isPersistent: false);

        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(returnUrl);

        //        case SignInStatus.LockedOut:
        //            return RedirectToAction("Lockout", "Login");

        //        case SignInStatus.RequiresVerification:
        //            return RedirectToAction("SendCode", "Login", new { ReturnUrl = returnUrl, RememberMe = false });
        //        default:
        //            // If the user does not have an account, then prompt the user to create an account
        //            ViewBag.ReturnUrl = returnUrl;
        //            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;

        //            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { ReturnUrl = returnUrl });
        //    }
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel viewModel)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Index", "Manage");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        var socialLoginDetails = await _customerService.GetExternalLoginInfoAsync();
        //        if (socialLoginDetails == null)
        //        {
        //            return View("ExternalLoginFailure");
        //        }

        //        var eMail = socialLoginDetails.ExternalIdentity.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Email))?.Value;
        //        var names = socialLoginDetails.ExternalIdentity.Name.Split(' ');
        //        var firstName = names[0];
        //        var lastName = names.Length > 1 ? names[1] : string.Empty;

        //        var user = new SiteUser
        //        {
        //            UserName = eMail,
        //            Email = eMail,
        //            FirstName = firstName,
        //            LastName = lastName,
        //            RegistrationSource = "Social login",
        //            NewsLetter = viewModel.Newsletter,
        //            IsApproved = true
        //        };

        //        var result = await _customerService.CreateUser(user);
        //        if (result.IdentityResult.Succeeded)
        //        {
        //            var identityResult = await _customerService.UserManager().AddLoginAsync(user.Id, socialLoginDetails.Login);
        //            if (identityResult.Succeeded)
        //            {
        //                await _customerService.SignInManager().SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //                return RedirectToLocal(viewModel.ReturnUrl);
        //            }
        //        }

        //        AddErrors(result.IdentityResult.Errors);
        //    }

        //    return View(viewModel);
        //}

        [HttpPost]
        public ActionResult TrackHeroBlock(string blockId, string blockName, string pageName)
        {
            _cmsTrackingService.HeroBlockClicked(_httpContextAccessor.HttpContext, blockId, blockName, pageName);
            return new ContentResult()
            {
                Content = blockName
            };
        }

        [HttpPost]
        public ActionResult TrackVideoBlock(string blockId, string blockName, string pageName)
        {
            _cmsTrackingService.VideoBlockViewed(_httpContextAccessor.HttpContext, blockId, blockName, pageName);
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