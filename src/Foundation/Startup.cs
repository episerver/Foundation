using Advanced.CMS.AdvancedReviews;
using EPiServer.Authorization;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Cms.UI.VisitorGroups;
using EPiServer.Personalization.VisitorGroups;
// CMS 13 removed: EPiServer.Cms.TinyMce.SpellChecker no longer exists in CMS 13.
// using EPiServer.Cms.TinyMce.SpellChecker;
// EPiServer.ContentApi.Cms removed: EPiServer.ContentDeliveryApi.* requires EPiServer.CMS.UI.Core < 13.0.0
// using EPiServer.ContentApi.Cms;
// using EPiServer.ContentApi.Cms.Internal;
// using EPiServer.ContentApi.Commerce;
// using EPiServer.ContentDefinitionsApi;
// using EPiServer.ContentManagementApi;
using EPiServer.Data;
using EPiServer.DependencyInjection;
// EPiServer.OpenIDConnect removed: no CMS 13 version. Replaced by EPiServer.OptimizelyIdentity.
// using EPiServer.OpenIDConnect;
// EPiServer.ServiceApi removed: EPiServer.ServiceApi.Commerce requires EPiServer.Commerce.Core < 15.0.0
// using EPiServer.ServiceApi;
using EPiServer.Shell.Modules;
using Foundation.Features.Checkout.Payments;
using Foundation.Infrastructure.Cms.ModelBinders;
using Foundation.Infrastructure.Cms.Users;
using Foundation.Infrastructure.Display;
using Mediachase.Commerce.Anonymous;
using Mediachase.Commerce.Orders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
// AdaptiveImages removed: no CMS 13 version (requires EPiServer.CMS < 13.0.0).
// using AdaptiveImages.Initialization;
// using AdaptiveImages.Unsplash;
using EPiServer.Cms.Shell.UI.Configurations;
using Optimizely.Cms.DependencyInjection;
using Optimizely.Cms.OpalChat;
using Optimizely.Cms.Opal.Tools;
using Optimizely.Opal.Tools;
// Optimizely.Cms.Preview1 removed: preview functionality absorbed into CMS 13 main package.
// Phase 4: Optimizely Graph + Content Manager
using Optimizely.Graph.DependencyInjection;
using System.IO;

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
            // EcfSqlConnection falls back to EPiServerDB when not separately configured.
            // On DXP, only EPiServerDB is injected automatically; Commerce shares the same database.
            var ecfConnStr = _configuration.GetConnectionString("EcfSqlConnection");
            if (string.IsNullOrEmpty(ecfConnStr))
                ecfConnStr = _configuration.GetConnectionString("EPiServerDB");
            services.Configure<DataAccessOptions>(options => options.ConnectionStrings.Add(new ConnectionStringOptions
            {
                Name = "EcfSqlConnection",
                ConnectionString = ecfConnStr
            }));
            // CMS 13: On DXP, Opti ID is mandatory — do NOT call AddCmsAspNetIdentity on non-Development.
            // AddOptimizelyIdentity is registered below in the non-Development block.
            if (_webHostingEnvironment.IsDevelopment())
            {
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
            }
            else
            {
                // AddCmsAspNetIdentity is not called on DXP (Opti ID is used instead).
                // CustomerService requires ServiceAccessor<ApplicationSignInManager<SiteUser>> and
                // ServiceAccessor<ApplicationUserManager<SiteUser>> — register null-returning stubs
                // so the service can be constructed. Callers must guard for null on non-dev.
                services.AddSingleton<ServiceAccessor<ApplicationSignInManager<SiteUser>>>(_ => () => null);
                services.AddSingleton<ServiceAccessor<ApplicationUserManager<SiteUser>>>(_ => () => null);
            }

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

            services.AddCms();
            // CMS 13: On-Page Editing (OPE) is disabled by default — Visual Builder is the new default.
            // Foundation uses traditional MVC rendering, so OPE must be explicitly re-enabled so that
            // the preview iframe receives content-change notifications from the editor and updates.
            services.Configure<CmsFeatureOptions>(o => o.SectionsVisibility.OnPageEditing = true);
            if (!_webHostingEnvironment.IsDevelopment())
            {
                services.AddCmsCloudPlatformSupport(_configuration);
            }
            services.AddVisitorGroupsCore();
            services.AddVisitorGroupsMvc();
            services.AddVisitorGroupsUI();
            services.AddCommerce();
            // Commerce's CampaignVisitorGroupFilter requires IVisitorGroupRoleRepository.
            // AddVisitorGroupsUI() is UI-only and does not register the runtime service.
            // TryAdd placed after AddVisitorGroupsUI() so the real implementation wins if it is ever registered.
            services.TryAddSingleton<IVisitorGroupRoleRepository, NoOpVisitorGroupRoleRepository>();
            // Forms 6.0.0 (CMS 13): requires explicit AddForms() call for DI registration.
            services.AddForms();
            // EPiServer.Find removed: no CMS 13 version. Graph replacement in Phase 4.
            // services.AddFind();
            // EPiServer.Social.Framework removed (user decision + deprecated)
            services.AddDisplay();
            // ContentInstaller installs demo data and requires UIUserProvider/UISignInManager
            // (registered by AddCmsAspNetIdentity). On DXP, Opti ID is used and these types
            // are not available, so only register ContentInstaller in development.
            if (_webHostingEnvironment.IsDevelopment())
            {
                services.TryAddEnumerable(Microsoft.Extensions.DependencyInjection.ServiceDescriptor.Singleton(typeof(IFirstRequestInitializer), typeof(ContentInstaller)));
            }
            services.AddDetection();
            services.AddTinyMceConfiguration();
            // CMS 13 removed: AddTinyMceSpellChecker no longer exists in CMS 13.
            // services.AddTinyMceSpellChecker();

            //site specific
            services.AddEmbeddedLocalization<Startup>();
            services.Configure<OrderOptions>(o => o.DisableOrderDataLocalization = true);

            // Content Delivery API removed: EPiServer.ContentDeliveryApi.* requires EPiServer.CMS.UI.Core < 13.0.0
            // Phase 4 will add Optimizely Graph / Content Graph API.
            // services.ConfigureContentApiOptions(o =>
            // {
            //     o.EnablePreviewFeatures = true;
            //     o.IncludeEmptyContentProperties = true;
            //     o.FlattenPropertyModel = false;
            //     o.IncludeMasterLanguage = false;
            // });
            // services.AddContentDeliveryApi()
            //     .WithFriendlyUrl()
            //     .WithSiteBasedCors();
            // services.AddContentDeliveryApi(OpenIDConnectOptionsDefaults.AuthenticationScheme, options => {
            //     options.SiteDefinitionApiEnabled = true;
            // })
            //    .WithFriendlyUrl()
            //    .WithSiteBasedCors();
            // services.AddContentSearchApi(o =>
            // {
            //     o.MaximumSearchResults = 100;
            // });
            // services.AddFormsApi();
            // services.AddCommerceApi<SiteUser>(OpenIDConnectOptionsDefaults.AuthenticationScheme, o =>
            // {
            //     o.DisableScopeValidation = true;
            // });
            // services.AddContentDefinitionsApi(options =>
            // {
            //     options.DisableScopeValidation = true;
            // });
            // services.AddContentManagementApi(OpenIDConnectOptionsDefaults.AuthenticationScheme, options =>
            // {
            //     options.DisableScopeValidation = true;
            // });
            // EPiServer.ServiceApi.Commerce removed: requires EPiServer.Commerce.Core < 15.0.0
            // services.AddServiceApiAuthorization(OpenIDConnectOptionsDefaults.AuthenticationScheme);

            // CMS 13: EPiServer.OpenIDConnect.UI replaced by EPiServer.OptimizelyIdentity.
            // Local dev uses AddCmsAspNetIdentity (above). On DXP, Opti ID is mandatory.
            if (_webHostingEnvironment.IsDevelopment())
            {
                // Allow the quick editor to be embedded into CMP via an iframe (ASP.NET Identity cookie).
                services.ConfigureApplicationCookie(options =>
                {
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                });
            }
            else
            {
                services.AddOptimizelyIdentity();
            }

            // services.ConfigureContentDeliveryApiSerializer(settings => settings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore);

            // Geta.NotFoundHandler removed: no CMS 13 version. Custom 404 solution to be implemented in Phase 3.
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

            // Phase 4: Optimizely Graph — replaces EPiServer.Find.
            // Credentials are read automatically from Optimizely:ContentGraph in appsettings.json.
            services.AddContentGraph(_ => { });
            // Graph query client — allows injecting IGraphContentClient into services (e.g. SearchService).
            services.AddGraphContentClient();
            // Content Manager UI (requires Graph to be registered first).
            services.AddContentManager();

            // Add AdvancedReviews
            services.AddAdvancedReviews();
            // Opal Chat — registered unconditionally. OpalChatOptionsConfigurer resolves
            // the InstanceId from the DXP platform context at runtime, not from appsettings,
            // so guarding on a config key always returned false and prevented registration.
            // On local dev the widget is simply hidden (no platform context = no InstanceId).
            services.AddOpalChat();

            // Opal Tools
            services.AddCmsOpalTools();

            // Geta.Optimizely.Categories removed: no CMS 13 version.
            // UNRVLD.ODP.VisitorGroups removed: no CMS 13 version (< 13.0.0 constraint).

            // Add Welcome DAM
            // services.AddDAMUi(); // Removed: DAM UI package not compatible with CMS 13.

            // MaxMind geolocation removed: EPiServer.Personalization.MaxMindGeolocation has no CMS 13 version.

            // URLs for files in contentassets folder shows friendly URL
            services.Configure<RoutingOptions>(o =>
            {
                o.ContentAssetsBasePath = ContentAssetsBasePath.ContentOwner;
            });

            // Optimizely.Labs.MarketingAutomationIntegration.ODP removed: no CMS 13 version.
            // EPiServer.Marketing.Automation.Forms removed: ExactTarget dependency removed.

            // EPiServer.Marketing.Testing (A/B Testing) removed: explicitly requires CMS < 13.0.0.

            // EPiServer.Labs.ContentManager removed: superseded by built-in CMS 13 Content Manager (added in Phase 4).
            // Advanced.CMS.GroupingHeader removed: no CMS 13 version.
            // Advanced.CMS.BulkEdit removed: no CMS 13 version.
            // EPiServer.Labs.ProjectEnhancements removed: no CMS 13 version.

            // TinymceDamPicker removed: no CMS 13 version.

            // CMS 13 removed: EPiServer.Shell.Telemetry namespace removed.
            // services.Configure<EPiServer.Shell.Telemetry.TelemetryOptions>(o => { o.Enabled = false; } );
            // AdaptiveImages removed: no CMS 13 version.
            // services.AddAdaptiveImages().AddUnsplash();

            // Optimizely.Cms.Cmp.Publishing removed: no CMS 13 version.
            // services.AddCmsCmpPublishing();

            // Allow the quick editor to be embedded into CMP via an iframe
            services.AddAntiforgery(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });


            // CMS 13 removed: CmsServiceOptions and AddDevelopmentSigningCredentials moved/removed.
            // if (_webHostingEnvironment.IsDevelopment())
            // {
            //     services.Configure<CmsServiceOptions>(o => {
            //         o.AddDevelopmentSigningCredentials();
            //     });
            // }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use((context, next) => { context.Request.Scheme = "https"; return next(context); });
            // Geta.NotFoundHandler removed — app.UseNotFoundHandler() removed.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Geta.Optimizely.Categories removed — app.UseGetaCategories() and app.UseGetaCategoriesFind() removed.

            app.UseAnonymousId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();
            // Optimizely.Cms.Cmp.Publishing removed: app.UseCmsCmpPublishingPreviewLinks() removed.
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAnonymousCartMerging();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "Default", pattern: "{controller}/{action}/{id?}");
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapContent();
                endpoints.MapOpalTools("/opal/");
            });
        }
    }
}
