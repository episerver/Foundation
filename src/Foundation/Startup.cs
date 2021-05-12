using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Data;
using EPiServer.DependencyInjection;
using EPiServer.Framework.Web.Resources;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Foundation.Infrastructure;
using Foundation.Infrastructure.Cms.ModelBinders;
using Foundation.Infrastructure.Commerce.Markets;
using Foundation.Infrastructure.Display;
using Mediachase.Commerce;
using Mediachase.Commerce.Orders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
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
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(_webHostingEnvironment.ContentRootPath, "App_Data"));
            services.Configure<DataAccessOptions>(o =>
            {
                o.ConnectionStrings.Add(new ConnectionStringOptions
                {
                    ConnectionString = _configuration.GetConnectionString("EcfSqlConnection"),
                    Name = "EcfSqlConnection"
                });
            });

            services.AddCmsAspNetIdentity<ApplicationUser>(o =>
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

            services.Configure<JsonOptions>(o =>
            {
                o.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            services.AddMvc(o => o.Conventions.Add(new FeatureConvention()))
                .AddRazorOptions(ro => ro.ConfigureFeatureFolders());

            services.AddCms();
            services.AddDisplay();
            services.AddTinyMce();
            services.AddFindUI(_configuration);
            services.TryAddEnumerable(Microsoft.Extensions.DependencyInjection.ServiceDescriptor.Singleton(typeof(IFirstRequestInitializer), typeof(ContentInstaller)));
            services.AddDetection();

            //Commerce
            services.AddCommerce();
            services.AddTinyMce();

            //site specific
            services.Configure<IISServerOptions>(options => options.AllowSynchronousIO = true);
            services.AddSingleton<ICurrentMarket, CurrentMarket>();
            services.AddEmbeddedLocalization<Startup>();
            services.Configure<MvcOptions>(o =>
            {
                o.ModelBinderProviders.Insert(0, new ModelBinderProvider());
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/util/Login";
                options.ExpireTimeSpan = new TimeSpan(0, 20, 0);
                options.SlidingExpiration = true;
            });

            services.Configure<OrderOptions>(o =>
            {
                o.DisableOrderDataLocalization = true;
            });
            services.AddHttpContextAccessor();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //HttpContextHelper.Initialize(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void ConfigureMvcOptions(MvcOptions mvcOptions)
        { 
        }
    }
}
