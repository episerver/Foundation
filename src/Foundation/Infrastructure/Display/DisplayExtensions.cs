using EPiServer.Web;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Infrastructure.Display
{
    public static class DisplayExtensions
    {
        public static void AddDisplay(this IServiceCollection services)
        {
            services.Configure<DisplayOptions>(displayOption =>
            {
                displayOption.Add("full", "/displayoptions/full", "col-12", "", "epi-icon__layout--full");
                displayOption.Add("half", "/displayoptions/half", "col-lg-6 col-md-6 col-sm-12 col-xs-12", "", "epi-icon__layout--half");
                displayOption.Add("wide", "/displayoptions/wide", "col-lg-8 col-md-6 col-sm-12 col-xs-12", "", "epi-icon__layout--two-thirds");
                displayOption.Add("narrow", "/displayoptions/narrow", "col-lg-4 col-md-6 col-sm-12 col-xs-12", "", "epi-icon__layout--one-third");
                displayOption.Add("one-quarter", "/displayoptions/one-quarter", "col-lg-3 col-md-6 col-sm-12 col-xs-12", "", "epi-icon__layout--one-quarter");
            });

            services.AddDisplayResolutions();
        }

        private static void AddDisplayResolutions(this IServiceCollection services)
        {
            services.AddSingleton<StandardResolution>();
            services.AddSingleton<IpadHorizontalResolution>();
            services.AddSingleton<IphoneVerticalResolution>();
            services.AddSingleton<AndroidVerticalResolution>();
        }
    }
}
