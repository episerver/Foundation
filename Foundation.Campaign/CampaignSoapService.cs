using Foundation.Campaign.Connected_Services.CouponBlock;
using Foundation.Campaign.Connected_Services.CouponCode;
using Foundation.Campaign.Connected_Services.MailId;
using Foundation.Campaign.Connected_Services.MailingReporting;
using Foundation.Campaign.Connected_Services.MailingService;
using Foundation.Campaign.Connected_Services.RecipientList;
using Foundation.Campaign.Connected_Services.SessionService;
using System.Configuration;

namespace Foundation.Campaign
{
    public class CampaignSoapService : ICampaignSoapService
    {
        private readonly string _configClientid;
        private readonly string _configPassword;
        private readonly string _configUsername;

        public CampaignSoapService()
        {
            _configUsername = ConfigurationManager.AppSettings["campaign:Username"];
            _configPassword = ConfigurationManager.AppSettings["campaign:Password"];
            _configClientid = ConfigurationManager.AppSettings["campaign:Clientid"];
        }

        public SessionWebserviceClient GetSessionWebserviceClient() => new SessionWebserviceClient();

        public MailingWebserviceClient GetMailingWebserviceClient() => new MailingWebserviceClient();

        public RecipientListWebserviceClient GetRecipientListWebserviceClient() => new RecipientListWebserviceClient();

        public MailIdWebserviceClient GetMailIdClient() => new MailIdWebserviceClient();

        public CouponBlockWebserviceClient GetCouponBlockWebserviceClient() => new CouponBlockWebserviceClient();

        public CouponCodeWebserviceClient GetCouponCodeWebserviceClient() => new CouponCodeWebserviceClient();

        public MailingReportingWebserviceClient GetMailingReportingWebserviceClient() => new MailingReportingWebserviceClient();
    }
}