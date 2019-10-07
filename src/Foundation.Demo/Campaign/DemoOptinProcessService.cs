using EPiServer.ConnectForCampaign.Core.Configuration;
using EPiServer.ConnectForCampaign.Core.Implementation;
using EPiServer.ConnectForCampaign.Services;
using EPiServer.ConnectForCampaign.Services.Implementation;
using System.Collections.Generic;

namespace Foundation.Demo.Campaign
{
    public class DemoOptinProcessService : OptinProcessService
    {
        public DemoOptinProcessService(IServiceClientFactory serviceClientFactory, ICacheService cacheService, ICampaignSettings campaignConfig, IAuthenticationService authenticationService)
            : base(serviceClientFactory, cacheService, campaignConfig, authenticationService)
        {
        }

        protected override string GetOptinProcessName(long optinProcessId)
        {
            return optinProcessId == 0 ? "DEMO ONLY - No double opt in" : base.GetOptinProcessName(optinProcessId);
        }

        protected override IEnumerable<long> GetAllOptinProcessIds()
        {
            var results = base.GetAllOptinProcessIds();

            (results as List<long>)?.Add(0);

            return results;
        }

        protected override string GetOptinProcessType(long optinProcessId)
        {
            return optinProcessId == 0 ? "double" : base.GetOptinProcessType(optinProcessId);
        }
    }
}