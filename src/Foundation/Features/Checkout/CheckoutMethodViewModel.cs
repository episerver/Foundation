using Foundation.Features.Login;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.Shared;

namespace Foundation.Features.Checkout
{
    public class CheckoutMethodViewModel : ContentViewModel<CheckoutPage>
    {
        public LoginViewModel LoginViewModel { get; set; }
        public RegisterAccountViewModel RegisterAccountViewModel { get; set; }

        public CheckoutMethodViewModel() : base()
        {
            LoginViewModel = new LoginViewModel();
            RegisterAccountViewModel = new RegisterAccountViewModel
            {
                Address = new AddressModel()
            };
        }

        public CheckoutMethodViewModel(CheckoutPage currentPage, string returnUrl = "/") : base(currentPage)
        {
            LoginViewModel = new LoginViewModel();
            RegisterAccountViewModel = new RegisterAccountViewModel
            {
                Address = new AddressModel()
            };
            LoginViewModel.ReturnUrl = returnUrl;
            CurrentContent = currentPage;
        }
    }
}