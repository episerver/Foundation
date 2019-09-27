using EPiServer.ConnectForCampaign.Core.Configuration;
using EPiServer.ConnectForCampaign.Core.Implementation;
using EPiServer.ConnectForCampaign.Services;
using EPiServer.ConnectForCampaign.Services.Implementation;

namespace Foundation.Demo.Campaign
{
    public class ExtendedRecipientListService : RecipientListService
    {
        public ExtendedRecipientListService(IServiceClientFactory serviceClientFactory, IAuthenticationService authenticationService, ICacheService cacheService, ICampaignSettings campaignConfig)
            : base(serviceClientFactory, authenticationService, cacheService, campaignConfig)
        {
        }

        protected override bool IsOptinListConnectorList(long recipientListId)
        {
            return true;
        }

    }
}