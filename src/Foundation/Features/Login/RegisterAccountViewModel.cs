using Foundation.Cms.Attributes;
using Foundation.Features.MyAccount.AddressBook;

namespace Foundation.Features.Login
{
    public class RegisterAccountViewModel
    {
        public AddressModel Address { get; set; }

        [LocalizedDisplay("/Registration/Form/Label/Email")]
        [LocalizedRequired("/Registration/Form/Empty/Email")]
        [LocalizedEmail("/Registration/Form/Error/InvalidEmail")]
        public string Email { get; set; }

        [LocalizedDisplay("/Registration/Form/Label/Password")]
        [LocalizedRequired("/Registration/Form/Empty/Password")]
        [LocalizedStringLength("/Registration/Form/Error/PasswordLength2", 5, 100)]
        public string Password { get; set; }

        [LocalizedDisplay("/Registration/Form/Label/Password2")]
        [LocalizedRequired("/Registration/Form/Empty/Password2")]
        [LocalizedCompare("Password", "/Registration/Form/Error/PasswordMatch")]
        [LocalizedStringLength("/Registration/Form/Error/PasswordLength2", 5, 100)]
        public string Password2 { get; set; }

        public bool Newsletter { get; set; }

        public string ErrorMessage { get; set; }
    }
}