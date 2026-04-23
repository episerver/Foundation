using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using Foundation.Infrastructure.Cms.ModelBinders;
using Foundation.Infrastructure.Cms.Settings;
using Foundation.Infrastructure.Cms.Users;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Infrastructure.Cms
{
    [ModuleDependency(typeof(InitializationModule))]//, typeof(SetupBootstrapRenderer))]
    public class Initialize : IConfigurableModule
    {
        void IConfigurableModule.ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<IsInEditModeAccessor>(locator => () => locator.GetInstance<IContextModeResolver>().CurrentMode.EditOrPreview());
            context.Services.AddSingleton<ServiceAccessor<IContentRouteHelper>>(locator => locator.GetInstance<IContentRouteHelper>);
            context.Services.AddTransient<IModelBinderProvider, DecimalModelBinderProvider>();
            context.Services.AddSingleton<IUserService, UserService>();
            context.Services.AddTransient<ICookieService, CookieService>();
            context.Services.AddSingleton<ISettingsService, SettingsService>();
        }

        void IInitializableModule.Initialize(InitializationEngine context)
        {
            // CMS 13: InitializeSettings previously called only from ContentInstaller (first-request initializer).
            // In CMS 13, tblContentSource is empty so the first-request approach may not load settings in time.
            // Call InitializeSettings here so the settings cache is ready before any request hits the site.
            var settingsService = context.Locate.Advanced.GetInstance<ISettingsService>();
            settingsService.InitializeSettings();
        }

        void IInitializableModule.Uninitialize(InitializationEngine context)
        {
        }
    }
}