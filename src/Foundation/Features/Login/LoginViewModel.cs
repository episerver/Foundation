using EPiServer.Core;
using Foundation.Cms.Attributes;

namespace Foundation.Features.Login
{
    public class LoginViewModel
    {
        [LocalizedDisplay("/Login/Form/Label/Email")]
        [LocalizedRequired("/Login/Form/Empty/Email")]
        [LocalizedEmail("/Login/Form/Error/InvalidEmail")]
        public string Email { get; set; }

        public ContentReference ResetPasswordPage { get; set; }

        [LocalizedDisplay("/Login/Form/Label/Password")]
        [LocalizedRequired("/Login/Form/Empty/Password")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        [LocalizedDisplay("/Login/Form/Label/RememberMe")]
        public bool RememberMe { get; set; }
    }
}