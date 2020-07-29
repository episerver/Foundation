using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Editor;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Foundation.Cms.Extensions;
using Foundation.Cms.Identity;
using Foundation.Cms.ModelBinders;
using Foundation.Cms.Settings;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Web;
using System.Web.Mvc;

namespace Foundation.Cms
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class Initialize : IConfigurableModule
    {
        private IServiceConfigurationProvider _services;

        void IConfigurableModule.ConfigureContainer(ServiceConfigurationContext context)
        {
            _services = context.Services;
            _services.AddTransient(_ => HttpContext.Current.GetOwinContext());
            _services.AddTransient(locator => locator.GetInstance<IOwinContext>().GetUserManager<ApplicationUserManager<SiteUser>>()).AddServiceAccessor();
            _services.AddTransient(locator => locator.GetInstance<IOwinContext>().Authentication).AddServiceAccessor();
            _services.AddTransient(locator => locator.GetInstance<IOwinContext>().Get<ApplicationSignInManager<SiteUser>>()).AddServiceAccessor();
            _services.AddSingleton<ISettingsService, SettingsService>();
            _services.AddTransient<IsInEditModeAccessor>(locator => () => PageEditing.PageIsInEditMode);
            _services.AddSingleton<ServiceAccessor<IContentRouteHelper>>(locator => locator.GetInstance<IContentRouteHelper>);
            _services.AddTransient<IModelBinderProvider, ModelBinderProvider>();
            _services.AddSingleton<CookieService>();
        }

        void IInitializableModule.Initialize(InitializationEngine context) => context.InitComplete += (sender, eventArgs) => context.Locate.Advanced.GetInstance<ISettingsService>().InitializeSettings();

        void IInitializableModule.Uninitialize(InitializationEngine context)
        {
        }
    }
}