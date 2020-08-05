using EPiServer.Cms.TinyMce.Core;
using EPiServer.ServiceLocation;
using Foundation.Cms.DependencyInjection;
using Newtonsoft.Json;
using System.Web.Http;
using System.Web.Mvc;

namespace Foundation.Cms.Extensions
{
    public static class ServiceConfigurationContextExtensions
    {
        public static void ConfigureFoundationCms(this ServiceConfigurationContext context)
        {
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(context.StructureMap()));
            GlobalConfiguration.Configure(config =>
            {
                config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.LocalOnly;
                config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings();
                config.Formatters.XmlFormatter.UseXmlSerializer = true;
                config.DependencyResolver = new StructureMapResolver(context.StructureMap());
                config.MapHttpAttributeRoutes();
            });

            context.Services.Configure<TinyMceConfiguration>(config =>
            {
                config.Default()
                    .AddPlugin("media wordcount anchor code textcolor colorpicker")
                    .Toolbar("formatselect | epi-personalized-content epi-link anchor numlist bullist indent outdent bold italic underline alignleft aligncenter alignright | image epi-image-editor media code | epi-dnd-processor | removeformat | fullscreen | forecolor backcolor | icons")
                    .AddSetting("image_caption", true)
                    .AddSetting("image_advtab", true);

                config.Default()
                    .AddExternalPlugin("icons", "/ClientResources/Scripts/fontawesomeicons.js")
                    .AddSetting("extended_valid_elements", "i[class], span")
                    .ContentCss(new[] { "/node_modules/@fontawesome/fontawesome-free/css/all.min.css",
                        "https://fonts.googleapis.com/css?family=Roboto:100,100i,300,300i,400,400i,500,500i,700,700i,900,900i",
                        "/ClientResources/Styles/TinyMCE.css" });
            });
        }
    }
}