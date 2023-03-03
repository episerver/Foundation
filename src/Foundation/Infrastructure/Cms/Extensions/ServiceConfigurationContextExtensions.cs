using EPiServer.Cms.TinyMce.Core;
using EPiServer.ServiceLocation;
using Microsoft.Extensions.DependencyInjection;
using Foundation.Features.Settings;
using Foundation.Infrastructure.Cms.Settings;
using System;
using System.Collections.Generic;
using EPiServer.Core;
using EPiServer;
using System.Linq;

namespace Foundation.Infrastructure.Cms.Extensions
{
    public static class ServiceConfigurationContextExtensions
    {
        private static readonly Lazy<ISettingsService> _settingsService =
            new Lazy<ISettingsService>(() => ServiceLocator.Current.GetInstance<ISettingsService>());

        private static readonly IContentLoader _contentLoader;

        public static void AddTinyMceConfiguration(this IServiceCollection services)
        {
            services.Configure<TinyMceConfiguration>(config =>
            {
                var settings = _settingsService.Value.GetSiteSettings<FontSettings>();

                List<string> FontFormats = new List<string>();
                FontFormats.Add("Andale Mono=andale mono,times");
                FontFormats.Add("Arial=arial,helvetica,sans-serif");
                FontFormats.Add("Arial Black=arial black,avant garde");
                FontFormats.Add("Book Antiqua=book antiqua,palatino");
                FontFormats.Add("Comic Sans MS=comic sans ms,sans-serif");
                FontFormats.Add("Courier New=courier new,courier");
                FontFormats.Add("Georgia=georgia,palatino");
                FontFormats.Add("Helvetica=helvetica");
                FontFormats.Add("Impact=impact,chicago");
                FontFormats.Add("Symbol=symbol");
                FontFormats.Add("Tahoma=tahoma,arial,helvetica,sans-serif");
                FontFormats.Add("Terminal=terminal,monaco");
                FontFormats.Add("Times New Roman=times new roman,times");
                FontFormats.Add("Trebuchet MS=trebuchet ms,geneva");
                FontFormats.Add("Verdana=verdana,geneva");
                FontFormats.Add("Webdings=webdings");
                FontFormats.Add("Wingdings=wingdings,zapf dingbats");

                var fontImports = "";

                if (settings != null && settings.FontFields != null)
                {
                    for (var i = 0; i < settings.FontFields.Count; i++)
                    {
                        if (settings.FontFields[i].EnableFont)
                        {
                            FontFormats.Add(string.Format("{0}={0}", settings.FontFields[i].FontName));

                            fontImports += string.Format("{0}", settings.FontFields[i].FontImport);

                        }
                    }
                }
                if (settings != null && settings.CustomFonts != null)
                {
                    for (var i = 0; i < settings.CustomFonts.Count; i++)
                    {
                        if (settings.CustomFonts[i].EnableFont)
                        {

                            FontFormats.Add(string.Format("{0}={0}", settings.CustomFonts[i].FontName));
                            fontImports += string.Format("@import url('{0}');", settings.CustomFonts[i].FontFile[0].GetMappedHref());
                        }
                    }
                }

                FontFormats.Sort();
                var fonts = string.Join(";", FontFormats);


                config.Default()
                    .AddPlugin("media wordcount anchor code searchreplace")
                    .Toolbar("blocks fontfamily fontsize | epi-personalized-content epi-link anchor numlist bullist indent outdent bold italic underline code",
                        "alignleft aligncenter alignright alignjustify | image epi-image-editor media | epi-dnd-processor | forecolor backcolor | removeformat | searchreplace fullscreen")
                    .AddSetting("image_caption", true)
                    .AddSetting("image_advtab", true)
                    .AddSetting("resize", "both")
                    .AddSetting("height", 400)
                    .AddSetting("font_family_formats", fonts)
                    .AddSetting("content_style", fontImports); ;

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