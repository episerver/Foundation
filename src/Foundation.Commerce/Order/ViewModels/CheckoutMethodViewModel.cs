using Foundation.Cms.ViewModels;
using Foundation.Commerce.Customer.ViewModels;
using Foundation.Commerce.Models.Pages;

namespace Foundation.Commerce.Order.ViewModels
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