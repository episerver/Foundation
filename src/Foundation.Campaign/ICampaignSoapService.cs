using Foundation.Campaign.Connected_Services.CouponBlock;
using Foundation.Campaign.Connected_Services.CouponCode;
using Foundation.Campaign.Connected_Services.MailId;
using Foundation.Campaign.Connected_Services.MailingReporting;
using Foundation.Campaign.Connected_Services.MailingService;
using Foundation.Campaign.Connected_Services.RecipientList;
using Foundation.Campaign.Connected_Services.SessionService;

namespace Foundation.Campaign
{
    public interface ICampaignSoapService
    {
        SessionWebserviceClient GetSessionWebserviceClient();
        MailingWebserviceClient GetMailingWebserviceClient();
        RecipientListWebserviceClient GetRecipientListWebserviceClient();
        MailIdWebserviceClient GetMailIdClient();
        CouponBlockWebserviceClient GetCouponBlockWebserviceClient();
        CouponCodeWebserviceClient GetCouponCodeWebserviceClient();
        MailingReportingWebserviceClient GetMailingReportingWebserviceClient();
    }
}