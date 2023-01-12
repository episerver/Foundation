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
        }

        void IInitializableModule.Uninitialize(InitializationEngine context)
        {
        }
    }
}