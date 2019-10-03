using EPiServer.Find.ClientConventions;
using EPiServer.Find.Cms;
using EPiServer.Find.Cms.Conventions;
using EPiServer.Find.Framework;
using EPiServer.Find.UnifiedSearch;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Foundation.Cms.Media;
using Foundation.Find.Cms.Facets;
using Foundation.Find.Cms.Models.Pages;
using Foundation.Find.Cms.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Find.Cms
{
    [ModuleDependency(typeof(Foundation.Cms.Initialize))]
    public class Initialize : IConfigurableModule
    {
        void IConfigurableModule.ConfigureContainer(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddSingleton<IFacetRegistry>(new FacetRegistry(new List<FacetDefinition>()));
            services.AddSingleton<ICmsSearchService, CmsSearchService>();
            services.AddSingleton<IModelBinderProvider, FindCmsModelBinderProvider>();
        }

        void IInitializableModule.Initialize(InitializationEngine context)
        {
            SearchClient.Instance.Conventions.UnifiedSearchRegistry
            .ForInstanceOf<LocationListPage>()
            .ProjectImageUriFrom(
                page => new Uri(context.Locate.Advanced.GetInstance<UrlResolver>().GetUrl(page.PageImage), UriKind.Relative));

            SearchClient.Instance.Conventions.ForInstancesOf<LocationItemPage>().IncludeField(dp => dp.TagString());

            //ModelBinderProviders.BinderProviders.Insert(0, new FindCmsModelBinderProvider());
        }

        void IInitializableModule.Uninitialize(InitializationEngine context)
        {
        }
    }
}