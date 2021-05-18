using Foundation.Infrastructure.Cms.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.MyAccount.ProfilePage
{
    public class AccountInformationViewModel
    {
        [LocalizedDisplay("/Shared/Address/Form/Label/FirstName")]
        [LocalizedRequired("/Shared/Address/Form/Empty/FirstName")]
        public string FirstName { get; set; }

        [LocalizedDisplay("/Shared/Address/Form/Label/LastName")]
        [LocalizedRequired("/Shared/Address/Form/Empty/LastName")]
        public string LastName { get; set; }

        [LocalizedDisplay("/AccountInformation/Form/DateOfBirth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        [LocalizedDisplay("/AccountInformation/Form/SubscribesToNewsletter")]
        public bool SubscribesToNewsletter { get; set; }
    }
}