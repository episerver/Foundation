using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Infrastructure.Personalization
{
    [ModuleDependency(typeof(Cms.Initialize))]
    public class Initialize : IConfigurableModule
    {
        void IConfigurableModule.ConfigureContainer(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddTransient<ICmsTrackingService, CmsTrackingService>();
            services.AddTransient<ICommerceTrackingService, CommerceTrackingService>();
            services.AddTransient<TrackingDataFactory>();
        }

        void IInitializableModule.Initialize(InitializationEngine context)
        {
        }

        void IInitializableModule.Uninitialize(InitializationEngine context)
        {
        }
    }
}