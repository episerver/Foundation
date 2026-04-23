using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Infrastructure.Display
{
    public static class DisplayExtensions
    {
        public static void AddDisplay(this IServiceCollection services)
        {
            services.Configure<DisplayOptions>(displayOption =>
            {
                displayOption.Add("screen", "/displayoptions/screen", "screen-width-block", "", "epi-icon__layout--screen");
                displayOption.Add("full", "/displayoptions/full", "col-12", "", "epi-icon__layout--full");
                displayOption.Add("threequarter", "/displayoptions/threequarter", "col-lg-9 col-md-6 col-sm-12 col-12 displaymode-three-quarters", "", "epi-icon__layout--three-quarters");
                displayOption.Add("wide", "/displayoptions/wide", "col-lg-8 col-md-6 col-sm-12 col-12 displaymode-two-thirds", "", "epi-icon__layout--two-thirds");
                displayOption.Add("half", "/displayoptions/half", "col-lg-6 col-md-6 col-sm-12 col-12 displaymode-half", "", "epi-icon__layout--half");
                displayOption.Add("narrow", "/displayoptions/narrow", "col-lg-4 col-md-6 col-sm-12 col-12 displaymode-one-third", "", "epi-icon__layout--one-third");
                displayOption.Add("one-quarter", "/displayoptions/one-quarter", "col-lg-3 col-md-6 col-sm-12 col-12 displaymode-one-quarter", "", "epi-icon__layout--one-quarter");
                displayOption.Add("one-sixth", "/displayoptions/one-sixth", "col-lg-2 col-md-4 col-sm-12 col-12 displaymode-one-sixth", "", "epi-icon__layout--one-sixth");
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
