using Foundation.Cms.Attributes;

namespace Foundation.Features.Api
{
    public class ExternalLoginConfirmationViewModel
    {
        [LocalizedRequired("/Registration/Form/Empty/Address")]
        [LocalizedDisplay("/Registration/Form/Label/Address")]
        public string Address { get; set; }

        [LocalizedRequired("/Registration/Form/Empty/Country")]
        [LocalizedDisplay("/Registration/Form/Label/Country")]
        public string Country { get; set; }

        [LocalizedRequired("/Registration/Form/Empty/City")]
        [LocalizedDisplay("/Registration/Form/Label/City")]
        public string City { get; set; }

        [LocalizedRequired("/Registration/Form/Empty/PostalCode")]
        [LocalizedDisplay("/Registration/Form/Label/PostalCode")]
        public string PostalCode { get; set; }

        public bool Newsletter { get; set; }

        public string ReturnUrl { get; set; }
    }
}