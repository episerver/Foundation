using EPiBootstrapArea;
using EPiBootstrapArea.Providers;
using System.Collections.Generic;

namespace Foundation.Infrastructure.Display
{
    public class FoundationDisplayModeProvider : DisplayModeFallbackDefaultProvider
    {
        public override List<DisplayModeFallback> GetAll()
        {
            return new List<DisplayModeFallback>
            {
                new DisplayModeFallback
                {
                    Name = "Screen width",
                    Tag = "displaymode-screen",
                    LargeScreenWidth = 12,
                    MediumScreenWidth = 12,
                    SmallScreenWidth = 12,
                    ExtraSmallScreenWidth = 12,
                },
                new DisplayModeFallback
                {
                    Name = "Full width (1/1)",
                    Tag = "displaymode-full",
                    LargeScreenWidth = 12,
                    MediumScreenWidth = 12,
                    SmallScreenWidth = 12,
                    ExtraSmallScreenWidth = 12,
                    Icon = "epi-icon__layout--full"
                },
                new DisplayModeFallback
                {
                    Name = "Three quarters width (3/4)",
                    Tag = "displaymode-three-quarters",
                    LargeScreenWidth = 9,
                    MediumScreenWidth = 6,
                    SmallScreenWidth = 12,
                    ExtraSmallScreenWidth = 12,
                    Icon = "epi-icon__layout--three-quarters"
                },
                new DisplayModeFallback
                {
                    Name = "Two thirds width (2/3)",
                    Tag = "displaymode-two-thirds",
                    LargeScreenWidth = 8,
                    MediumScreenWidth = 6,
                    SmallScreenWidth = 12,
                    ExtraSmallScreenWidth = 12,
                    Icon = "epi-icon__layout--two-thirds"
                },
                new DisplayModeFallback
                {
                    Name = "Half width (1/2)",
                    Tag = "displaymode-half",
                    LargeScreenWidth = 6,
                    MediumScreenWidth = 6,
                    SmallScreenWidth = 12,
                    ExtraSmallScreenWidth = 12,
                    Icon = "epi-icon__layout--half"
                },
                new DisplayModeFallback
                {
                    Name = "One third width (1/3)",
                    Tag = "displaymode-one-third",
                    LargeScreenWidth = 4,
                    MediumScreenWidth = 6,
                    SmallScreenWidth = 12,
                    ExtraSmallScreenWidth = 12,
                    Icon = "epi-icon__layout--one-third"
                },
                new DisplayModeFallback
                {
                    Name = "One quarter width (1/4)",
                    Tag = "displaymode-one-quarter",
                    LargeScreenWidth = 3,
                    MediumScreenWidth = 6,
                    SmallScreenWidth = 12,
                    ExtraSmallScreenWidth = 12,
                    Icon = "epi-icon__layout--one-quarter"
                },
                new DisplayModeFallback
                {
                    Name = "One sixth width (1/6)",
                    Tag = "displaymode-one-sixth",
                    LargeScreenWidth = 2,
                    MediumScreenWidth = 4,
                    SmallScreenWidth = 12,
                    ExtraSmallScreenWidth = 12
                }
            };
        }
    }
}
