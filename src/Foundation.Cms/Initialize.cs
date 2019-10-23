using EPiBootstrapArea;
using EPiBootstrapArea.Initialization;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Editor;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using EPiServer.Web.PageExtensions;
using EPiServer.Web.Routing;
using Foundation.Cms.Display;
using Foundation.Cms.Identity;
using Foundation.Cms.ModelBinders;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Web;
using System.Web.Mvc;

namespace Foundation.Cms
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule), typeof(SetupBootstrapRenderer))]
    public class Initialize : IConfigurableModule
    {
        void IConfigurableModule.ConfigureContainer(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddTransient(_ => HttpContext.Current.GetOwinContext());
            services.AddTransient(locator => locator.GetInstance<IOwinContext>().GetUserManager<ApplicationUserManager<SiteUser>>()).AddServiceAccessor();
            services.AddTransient(locator => locator.GetInstance<IOwinContext>().Authentication).AddServiceAccessor();
            services.AddTransient(locator => locator.GetInstance<IOwinContext>().Get<ApplicationSignInManager<SiteUser>>()).AddServiceAccessor();
            services.AddSingleton<IDisplayModeFallbackProvider, FoundationDisplayModeProvider>();
            services.AddTransient<IsInEditModeAccessor>(locator => () => PageEditing.PageIsInEditMode);
            services.AddSingleton<ServiceAccessor<IContentRouteHelper>>(locator => locator.GetInstance<IContentRouteHelper>);
            services.AddTransient<IModelBinderProvider, ModelBinderProvider>();
            services.AddSingleton<CookieService>();
            services.AddSingleton<BlogTagFactory>();
            services.AddTransient<IQuickNavigatorItemProvider, FoundationQuickNavigatorItemProvider>();
            services.AddTransient<IViewTemplateModelRegistrator, ViewTemplateModelRegistrator>();
        }

        void IInitializableModule.Initialize(InitializationEngine context)
        {
        }

        void IInitializableModule.Uninitialize(InitializationEngine context)
        {
        }
    }
}