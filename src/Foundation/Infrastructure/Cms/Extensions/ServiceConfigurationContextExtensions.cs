using EPiServer.Cms.TinyMce.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Infrastructure.Cms.Extensions
{
    public static class ServiceConfigurationContextExtensions
    {
        public static void AddTinyMceConfiguration(this IServiceCollection services)
        {
            services.Configure<TinyMceConfiguration>(config =>
            {
                config.Default()
                    .AddPlugin("media wordcount anchor code")
                    .Toolbar("blocks | epi-personalized-content epi-link anchor numlist bullist indent outdent bold italic underline | code",
                        "alignleft aligncenter alignright alignjustify | image epi-image-editor media | epi-dnd-processor | forecolor backcolor | removeformat | searchreplace fullscreen")
                    .AddSetting("image_caption", true)
                    .AddSetting("image_advtab", true)
                    .AddSetting("resize", "both")
                    .AddSetting("height", 400);

                config.Default()
                    .AddEpiserverSupport()
                    //.AddExternalPlugin("icons", "/ClientResources/Scripts/fontawesomeicons.js")
                    .AddSetting("extended_valid_elements", "i[class], span");
                    //.ContentCss(new[] { "/ClientResources/Styles/fontawesome.min.css",
                    //    "https://fonts.googleapis.com/css?family=Roboto:100,100i,300,300i,400,400i,500,500i,700,700i,900,900i",
                    //    "/ClientResources/Styles/TinyMCE.css" });
            });
        }
    }
}