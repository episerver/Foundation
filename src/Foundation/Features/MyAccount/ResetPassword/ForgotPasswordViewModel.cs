using Foundation.Cms.Attributes;
using Foundation.Features.Shared;

namespace Foundation.Features.MyAccount.ResetPassword
{
    public class ForgotPasswordViewModel : ContentViewModel<ResetPasswordPage>
    {
        public ForgotPasswordViewModel(ResetPasswordPage resetPasswordPage) : base(resetPasswordPage)
        {
        }

        public ForgotPasswordViewModel() { }

        [LocalizedDisplay("/ResetPassword/Form/Label/Email")]
        [LocalizedRequired("/ResetPassword/Form/Empty/Email")]
        [LocalizedEmail("/ResetPassword/Form/Error/InvalidEmail")]
        public string Email { get; set; }
    }
}