using EPiServer;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using Foundation.Cms.Extensions;
using Foundation.Demo.Install;
using Foundation.Demo.Install.Steps;
using Foundation.Demo.ProfileStore;
using System;
using System.Web;

namespace Foundation.Demo
{
    [ModuleDependency(typeof(Personalization.Initialize), typeof(Find.Initialize), typeof(Social.Initialize))]
    public class Initialize : IConfigurableModule
    {
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(Initialize));
        private readonly ImageHelper _imageHelper = new ImageHelper();

        void IConfigurableModule.ConfigureContainer(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddTransient(_ => HttpContext.Current.GetOwinContext());
            services.AddTransient<ContentExportProcessor>();
            services.AddSingleton<IInstallService, InstallService>();
            services.AddSingleton<IInstallStep, AddCurrencies>();
            services.AddSingleton<IInstallStep, AddCustomers>();
            services.AddSingleton<IInstallStep, AddMarkets>();
            services.AddSingleton<IInstallStep, AddPaymentMethods>();
            services.AddSingleton<IInstallStep, AddPromotions>();
            services.AddSingleton<IInstallStep, AddShippingMethods>();
            services.AddSingleton<IInstallStep, AddTaxes>();
            services.AddSingleton<IInstallStep, AddWarehouses>();
            services.AddSingleton<IStorageService, StorageService>();
            services.AddSingleton<IProfileStoreService, ProfileStoreService>();
        }

        void IInitializableModule.Initialize(InitializationEngine context)
        {
            context.Locate.Advanced.GetInstance<IContentEvents>().SavingContent += OnSavingContent;
        }

        void IInitializableModule.Uninitialize(InitializationEngine context)
        {
        }

        private void OnSavingContent(object sender, ContentEventArgs contentEventArgs)
        {
            var contentData = contentEventArgs?.Content as IImageTagging;
            if (contentData == null || ((SaveContentEventArgs)contentEventArgs).Action != EPiServer.DataAccess.SaveAction.Publish)
            {
                return;
            }

            try
            {
                AsyncHelpers.RunSync(() => _imageHelper.TagImagesAsync(contentData));
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, e);
            }

        }
    }
}