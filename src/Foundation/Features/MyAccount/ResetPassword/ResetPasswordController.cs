using EPiServer;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using Foundation.Cms.Attributes;
using Foundation.Cms.Identity;
using Foundation.Cms.Pages;
using Foundation.Cms.ViewModels;
using Foundation.Cms.ViewModels.Pages;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Mail;
using Foundation.Commerce.Models.Pages;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Foundation.Features.MyAccount.ResetPassword
{
    public class ResetPasswordController : IdentityControllerBase<ResetPasswordPage>
    {
        private readonly IContentLoader _contentLoader;
        private readonly IMailService _mailService;
        private readonly LocalizationService _localizationService;

        public ResetPasswordController(ApplicationSignInManager<SiteUser> signinManager,
            ApplicationUserManager<SiteUser> userManager,
            ICustomerService customerService,
            IContentLoader contentLoader,
            IMailService mailService,
            LocalizationService localizationService)

            : base(signinManager, userManager, customerService)
        {
            _contentLoader = contentLoader;
            _mailService = mailService;
            _localizationService = localizationService;
        }

        [AllowAnonymous]
        public ActionResult Index(ResetPasswordPage currentPage)
        {
            var viewModel = new ForgotPasswordViewModel(currentPage);
            return View("ForgotPassword", viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model, string language)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var startPage = _contentLoader.Get<CommerceHomePage>(ContentReference.StartPage);
            //var body = _mailService.GetHtmlBodyForMail(startPage.ResetPasswordMail, new NameValueCollection(), language);
            var mailPage = _contentLoader.Get<MailBasePage>(startPage.ResetPasswordMail);
            var body = mailPage.MainBody.ToHtmlString();
            var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var url = Url.Action("ResetPassword", "ResetPassword", new { userId = user.Id, code = HttpUtility.UrlEncode(code), language }, Request.Url.Scheme);

            body = body.Replace("[MailUrl]",
                string.Format("{0}<a href=\"{1}\">{2}</a>", _localizationService.GetString("/ResetPassword/Mail/Text"), url, _localizationService.GetString("/ResetPassword/Mail/Link"))
            );

            _mailService.Send(mailPage.Subject, body, user.Email);

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            var homePage = _contentLoader.Get<PageData>(ContentReference.StartPage) as CommerceHomePage;
            var model = ContentViewModel.Create(homePage);
            return View("ForgotPasswordConfirmation", model);
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(ResetPasswordPage currentPage, string code)
        {
            var viewModel = new ResetPasswordViewModel(currentPage) { Code = code };
            return code == null ? View("Error") : View("ResetPassword", viewModel);
        }

        [HttpPost]
        [AllowDBWrite]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await UserManager.ResetPasswordAsync(user.Id, HttpUtility.UrlDecode(model.Code), model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            AddErrors(result.Errors);

            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            var homePage = _contentLoader.Get<PageData>(ContentReference.StartPage) as CommerceHomePage;
            var model = ContentViewModel.Create(homePage);
            return View("ResetPasswordConfirmation", model);
        }
    }
}