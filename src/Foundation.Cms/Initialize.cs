using EPiBootstrapArea;
using EPiBootstrapArea.Initialization;
using EPiServer;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Core;
using EPiServer.Editor;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using EPiServer.Web.PageExtensions;
using EPiServer.Web.Routing;
using Foundation.Cms.Display;
using Foundation.Cms.Extensions;
using Foundation.Cms.Identity;
using Foundation.Cms.Media;
using Foundation.Cms.ModelBinders;
using Foundation.Cms.Pages;
using Foundation.Cms.SchemaMarkup;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Web;
using System.Web.Mvc;

namespace Foundation.Cms
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule), typeof(SetupBootstrapRenderer))]
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
            _services.AddSingleton<IDisplayModeFallbackProvider, FoundationDisplayModeProvider>();
            _services.AddTransient<IsInEditModeAccessor>(locator => () => PageEditing.PageIsInEditMode);
            _services.AddSingleton<ServiceAccessor<IContentRouteHelper>>(locator => locator.GetInstance<IContentRouteHelper>);
            _services.AddTransient<IModelBinderProvider, ModelBinderProvider>();
            _services.AddSingleton<CookieService>();
            _services.AddSingleton<BlogTagFactory>();
            _services.AddTransient<IQuickNavigatorItemProvider, FoundationQuickNavigatorItemProvider>();
            _services.AddTransient<IViewTemplateModelRegistrator, ViewTemplateModelRegistrator>();
            _services.AddSingleton<ISchemaDataMapper<BlogItemPage>, BlogItemPageSchemaMapper>();
            _services.AddSingleton<ISchemaDataMapper<CmsHomePage>, CmsHomePageSchemaMapper>();
        }

        void IInitializableModule.Initialize(InitializationEngine context)
        {
            context.InitComplete += ContextOnInitComplete;
        }

        void IInitializableModule.Uninitialize(InitializationEngine context)
        {
            context.InitComplete -= ContextOnInitComplete;
        }

        private void ContextOnInitComplete(object sender, EventArgs eventArgs)
        {
            _services.AddTransient<ContentAreaRenderer, FoundationContentAreaRenderer>();
        }
    }
}