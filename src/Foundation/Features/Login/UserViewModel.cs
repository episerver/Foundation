using Foundation.Features.MyAccount.AddressBook;

namespace Foundation.Features.Login
{
    public class UserViewModel
    {
        public string Logo { get; set; }
        public string Title { get; set; }
        public string ResetPasswordUrl { get; set; }
        public string ReturnUrl { get; set; }
        public LoginViewModel LoginViewModel { get; set; }
        public RegisterAccountViewModel RegisterAccountViewModel { get; set; }

        public UserViewModel()
        {
            LoginViewModel = new LoginViewModel();
            RegisterAccountViewModel = new RegisterAccountViewModel
            {
                Address = new AddressModel()
            };
        }
    }
}