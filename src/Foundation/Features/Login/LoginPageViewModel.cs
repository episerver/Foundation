using Foundation.Features.Shared;

namespace Foundation.Features.Login;

public class LoginPageViewModel : ContentViewModel<LoginPage>
{
    public string Logo { get; set; }
    public string Title { get; set; }
    public string ResetPasswordUrl { get; set; }
    public LoginViewModel LoginViewModel { get; set; } 

    public LoginPageViewModel(LoginPage currentContent)
    {
        CurrentContent = currentContent;
    }
}