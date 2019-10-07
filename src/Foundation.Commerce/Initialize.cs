using EPiServer;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Foundation.Cms.ViewModels.Header;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Markets;
using Foundation.Commerce.Markets.ViewModels;
using Foundation.Commerce.ViewModels.Header;
using Mediachase.Commerce;
using System;
using System.Web.Mvc;

namespace Foundation.Commerce
{
    [ModuleDependency(typeof(Cms.Initialize))]
    public class Initialize : IConfigurableModule
    {
        void IConfigurableModule.ConfigureContainer(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddSingleton<ICurrentMarket, CurrentMarket>();
            services.AddSingleton<IBookmarksService, BookmarksService>();
            services.AddSingleton<IHeaderViewModelFactory, CommerceHeaderViewModelFactory>();
            services.AddSingleton<IModelBinderProvider, ModelBinderProvider>();
        }

        void IInitializableModule.Initialize(InitializationEngine context)
        {
            MarketEvent.ChangeMarket += ChangeMarket;
        }

        private void ChangeMarket(object o, EventArgs e)
        {
            var market = o as IMarket;
            if (market != null)
            {
                var marketCache = CacheManager.Get(Constant.CacheKeys.MarketViewModel) as MarketViewModel;
                if (marketCache.MarketId != market.MarketId)
                {
                    CacheManager.Remove(Constant.CacheKeys.MarketViewModel);
                }
            }
        }

        void IInitializableModule.Uninitialize(InitializationEngine context)
        {
        }
    }
}