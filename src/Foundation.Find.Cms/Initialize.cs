using EPiServer.Find.ClientConventions;
using EPiServer.Find.Framework;
using EPiServer.Find.UnifiedSearch;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ContentQuery;
using EPiServer.Web.Routing;
using Foundation.Cms.SchemaMarkup;
using Foundation.Find.Cms.Facets;
using Foundation.Find.Cms.Facets.Config;
using Foundation.Find.Cms.Models.Pages;
using Foundation.Find.Cms.PowerSlices;
using Foundation.Find.Cms.SchemaDataMappers;
using Foundation.Find.Cms.ViewModels;
using PowerSlice;
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
            services.AddTransient<IContentQuery, LandingPagesSlice>();
            services.AddTransient<IContentSlice, LandingPagesSlice>();
            services.AddTransient<IContentQuery, StandardPagesSlice>();
            services.AddTransient<IContentSlice, StandardPagesSlice>();
            services.AddTransient<IContentQuery, BlogsSlice>();
            services.AddTransient<IContentSlice, BlogsSlice>();
            services.AddTransient<IContentQuery, BlocksSlice>();
            services.AddTransient<IContentSlice, BlocksSlice>();
            services.AddTransient<IContentQuery, MediaSlice>();
            services.AddTransient<IContentSlice, MediaSlice>();
            services.AddTransient<IContentQuery, ImagesSlice>();
            services.AddTransient<IContentSlice, ImagesSlice>();
            services.AddTransient<IContentQuery, EverythingSlice>();
            services.AddTransient<IContentSlice, EverythingSlice>();
            services.AddTransient<IContentQuery, MyContentSlice>();
            services.AddTransient<IContentSlice, MyContentSlice>();
            services.AddTransient<IContentQuery, MyPagesSlice>();
            services.AddTransient<IContentSlice, MyPagesSlice>();
            services.AddTransient<IContentQuery, UnusedMediaSlice>();
            services.AddTransient<IContentSlice, UnusedMediaSlice>();
            services.AddTransient<IContentQuery, UnusedBlocksSlice>();
            services.AddTransient<IContentSlice, UnusedBlocksSlice>();
            services.AddSingleton<IFacetConfigFactory, FacetConfigFactory>();
            services.AddSingleton<ISchemaDataMapper<LocationItemPage>, LocationItemPageSchemaDataMapper>();
        }

        void IInitializableModule.Initialize(InitializationEngine context)
        {
            SearchClient.Instance.Conventions.UnifiedSearchRegistry
            .ForInstanceOf<LocationListPage>()
            .ProjectImageUriFrom(
                page => new Uri(context.Locate.Advanced.GetInstance<UrlResolver>().GetUrl(page.PageImage), UriKind.Relative));

            SearchClient.Instance.Conventions.ForInstancesOf<LocationItemPage>().IncludeField(dp => dp.TagString());

        }

        void IInitializableModule.Uninitialize(InitializationEngine context)
        {
        }

    }
}