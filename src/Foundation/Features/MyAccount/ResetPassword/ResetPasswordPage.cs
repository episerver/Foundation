using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;

namespace Foundation.Features.MyAccount.ResetPassword
{
    [ContentType(DisplayName = "Reset Password Page",
        GUID = "05834347-8f4f-48ec-a74c-c46278654a92",
        Description = "Page for allowing users to reset their passwords. The page must also be set in the StartPage's ResetPasswordPage property.",
        GroupName = GroupNames.Account,
        AvailableInEditMode = false)]
    [ImageUrl("/icons/cms/pages/CMS-icon-page-09.png")]
    public class ResetPasswordPage : FoundationPageData
    {
    }
}