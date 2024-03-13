using EPiServer.Events.Clients;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using Foundation.Infrastructure.Cms.ModelBinders;
using Foundation.Infrastructure.Cms.Settings;
using Foundation.Infrastructure.Cms.Users;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Infrastructure.Cms
{
    [ModuleDependency(typeof(InitializationModule))]
    public class Initialize : IConfigurableModule
    {
        public static readonly Guid SettingsRaiserId = Guid.NewGuid();
        public static readonly Guid SettingsEventId = new Guid("c3b20325-5aa8-4430-b33f-2a74c3e5b807");

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
            context.Locate.Advanced.GetInstance<IEventRegistry>().Get(SettingsEventId).Raised += context.Locate.Advanced.GetInstance<ISettingsService>().SettingsEvent_Raised;
            context.Locate.Advanced.GetInstance<ISettingsService>().InitializeSettings();
        }

        void IInitializableModule.Uninitialize(InitializationEngine context)
        {
            context.Locate.Advanced.GetInstance<IEventRegistry>().Get(SettingsEventId).Raised -= context.Locate.Advanced.GetInstance<ISettingsService>().SettingsEvent_Raised;
            context.Locate.Advanced.GetInstance<ISettingsService>().UnintializeSettings();
        }
    }
}