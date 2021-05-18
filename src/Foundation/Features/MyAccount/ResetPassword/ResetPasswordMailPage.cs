using EPiServer.DataAnnotations;
using Foundation.Infrastructure;

namespace Foundation.Features.MyAccount.ResetPassword
{
    [ContentType(DisplayName = "Reset Password Mail Page",
        GUID = "73bc5587-eef3-4844-be9d-0c90d081e2e4",
        Description = "The reset password template mail page.",
        GroupName = GroupNames.Account,
        AvailableInEditMode = false)]
    [ImageUrl("/icons/cms/pages/CMS-icon-page-26.png")]
    public class ResetPasswordMailPage : MailBasePage
    {
    }
}