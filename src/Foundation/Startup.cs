using Advanced.CMS.AdvancedReviews;
using EPiServer.Authorization;
using EPiServer.ContentApi.Cms;
using EPiServer.ContentApi.Cms.Internal;
using EPiServer.ContentDefinitionsApi;
using EPiServer.ContentManagementApi;
using EPiServer.Data;
using EPiServer.Framework.Web.Resources;
using EPiServer.Labs.BlockEnhancements;
using EPiServer.OpenIDConnect;
using EPiServer.ServiceLocation;
using EPiServer.Shell.Modules;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Foundation.Features.Checkout.Payments;
using Foundation.Infrastructure;
using Foundation.Infrastructure.Cms.Extensions;
using Foundation.Infrastructure.Cms.ModelBinders;
using Foundation.Infrastructure.Cms.Users;
using Foundation.Infrastructure.Display;
using Geta.NotFoundHandler.Infrastructure.Configuration;
using Geta.NotFoundHandler.Infrastructure.Initialization;
using Geta.NotFoundHandler.Optimizely.Infrastructure.Configuration;
using Geta.Optimizely.Categories.Configuration;
using Geta.Optimizely.Categories.Find.Infrastructure.Initialization;
using Geta.Optimizely.Categories.Infrastructure.Initialization;
using Mediachase.Commerce.Anonymous;
using Mediachase.Commerce.Orders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using UNRVLD.ODP.VisitorGroups.Initilization;

namespace Foundation
{
    public class Startup
    {
        private readonly IWebHostEnvironment _webHostingEnvironment;
        private readonly IConfiguration _configuration;

        public Startup(IWebHostEnvironment webHostingEnvironment, IConfiguration configuration)
        {
            _webHostingEnvironment = webHostingEnvironment;
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DataAccessOptions>(options => options.ConnectionStrings.Add(new ConnectionStringOptions
            {
                Name = "EcfSqlConnection",
                ConnectionString = _configuration.GetConnectionString("EcfSqlConnection")
            }));
            services.AddCmsAspNetIdentity<SiteUser>(o =>
            {
                if (string.IsNullOrEmpty(o.ConnectionStringOptions?.ConnectionString))
                {
                    o.ConnectionStringOptions = new ConnectionStringOptions
                    {
                        Name = "EcfSqlConnection",
                        ConnectionString = _configuration.GetConnectionString("EcfSqlConnection")
                    };
                }
            });

            //UI
            if (_webHostingEnvironment.IsDevelopment())
            {
                services.Configure<ClientResourceOptions>(uiOptions =>
                {
                    uiOptions.Debug = true;
                });
            }

            services.AddMvc(o =>
            {
                o.Conventions.Add(new FeatureConvention());
                o.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
                o.ModelBinderProviders.Insert(0, new PaymentModelBinderProvider());
            })
            .AddRazorOptions(ro => ro.ViewLocationExpanders.Add(new FeatureViewLocationExpander()));

            services.AddCommerce();
            services.AddFind();
            services.AddSocialFramework();
            services.AddDisplay();
            services.TryAddEnumerable(Microsoft.Extensions.DependencyInjection.ServiceDescriptor.Singleton(typeof(IFirstRequestInitializer), typeof(ContentInstaller)));
            services.AddDetection();
            services.AddTinyMceConfiguration();

            //site specific
            services.AddEmbeddedLocalization<Startup>();
            services.Configure<OrderOptions>(o => o.DisableOrderDataLocalization = true);

            services.ConfigureContentApiOptions(o =>
            {
                o.EnablePreviewFeatures = true;
                o.IncludeEmptyContentProperties = true;
                o.FlattenPropertyModel = false;
                o.IncludeMasterLanguage = false; 
                
            });

            // Content Delivery API
            services.AddContentDeliveryApi()
                .WithFriendlyUrl()
                .WithSiteBasedCors();
            services.AddContentDeliveryApi(OpenIDConnectOptionsDefaults.AuthenticationScheme, options => {
                options.SiteDefinitionApiEnabled = true;
            })
               .WithFriendlyUrl()
               .WithSiteBasedCors();

            // Content Delivery Search API
            services.AddContentSearchApi(o =>
            {
                o.MaximumSearchResults = 100;
            });

            // Content Definitions API
            services.AddContentDefinitionsApi(options =>
            {
                // Accept anonymous calls
                options.DisableScopeValidation = true;
            });

            // Content Management
            services.AddContentManagementApi(OpenIDConnectOptionsDefaults.AuthenticationScheme, options =>
            {
                // Accept anonymous calls
                options.DisableScopeValidation = true;
            });

            services.AddOpenIDConnect<SiteUser>(options =>
            {
                //options.RequireHttps = !_webHostingEnvironment.IsDevelopment();
                var application = new OpenIDConnectApplication()
                {
                    ClientId = "postman-client",
                    ClientSecret = "postman",
                    Scopes =
                    {
                        ContentDeliveryApiOptionsDefaults.Scope,
                        ContentManagementApiOptionsDefaults.Scope,
                        ContentDefinitionsApiOptionsDefaults.Scope,
                    }
                };

                // Using Postman for testing purpose.
                // The authorization code is sent to postman after successful authentication.
                application.RedirectUris.Add(new Uri("https://oauth.pstmn.io/v1/callback"));
                options.Applications.Add(application);
                options.AllowResourceOwnerPasswordFlow = true;
            });
            
            services.AddOpenIDConnectUI();

            services.ConfigureContentDeliveryApiSerializer(settings => settings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore);

            services.AddNotFoundHandler(o => o.UseSqlServer(_configuration.GetConnectionString("EPiServerDB")), policy => policy.RequireRole(Roles.CmsAdmins));
            services.AddOptimizelyNotFoundHandler();
            services.Configure<ProtectedModuleOptions>(x =>
            {
                if (!x.Items.Any(x => x.Name.Equals("Foundation")))
                {
                    x.Items.Add(new ModuleDetails
                    {
                        Name = "Foundation"
                    });
                }
            });
            // Don't camelCase Json output -- leave property names unchanged
            //services.AddControllers()
            //    .AddJsonOptions(options =>
            //    {
            //        options.JsonSerializerOptions.PropertyNamingPolicy = null;
            //    });

            // Add BlockEnhancements
            services.AddBlockEnhancements();
            services.Configure<BlockEnhancementsOptions>(options =>
            {
                //var blockEnhancements = new BlockEnhancementsOptions
                options.LocalContentFeatureEnabled = false;
                options.HideForThisFolder = false;
                options.AllowQuickEditOnSharedBlocks = true;
                options.PublishPageWithBlocks = true;
            });

            // Add AdvancedReviews
            services.AddAdvancedReviews();
            services.AddGetaCategories();
            services.AddODPVisitorGroups();

            // Add Welcome DAM
            services.AddDAMUi();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseNotFoundHandler();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGetaCategories();
            app.UseGetaCategoriesFind();

            app.UseAnonymousId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "Default", pattern: "{controller}/{action}/{id?}");
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapContent();
            });
        }
    }
}
