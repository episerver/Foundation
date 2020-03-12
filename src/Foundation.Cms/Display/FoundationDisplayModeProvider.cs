using EPiBootstrapArea;
using EPiBootstrapArea.Providers;
using System.Collections.Generic;

namespace Foundation.Cms.Display
{
    public class FoundationDisplayModeProvider : DisplayModeFallbackDefaultProvider
    {
        public override List<DisplayModeFallback> GetAll()
        {
            return new List<DisplayModeFallback>
            {
                new DisplayModeFallback
                {
                    Name = "Full width (1/1)",
                    Tag = DisplayOptionTags.FullWidth,
                    LargeScreenWidth = 12,
                    MediumScreenWidth = 12,
                    SmallScreenWidth = 12,
                    ExtraSmallScreenWidth = 12,
                    Icon = "epi-icon__layout--full"
                },
                new DisplayModeFallback
                {
                    Name = "Three quarters width (3/4)",
                    Tag = DisplayOptionTags.ThreeQuarter,
                    LargeScreenWidth = 9,
                    MediumScreenWidth = 6,
                    SmallScreenWidth = 12,
                    ExtraSmallScreenWidth = 12,
                    Icon = "epi-icon__layout--three-quarters"
                },
                new DisplayModeFallback
                {
                    Name = "Two thirds width (2/3)",
                    Tag = DisplayOptionTags.TwoThird,
                    LargeScreenWidth = 8,
                    MediumScreenWidth = 6,
                    SmallScreenWidth = 12,
                    ExtraSmallScreenWidth = 12,
                    Icon = "epi-icon__layout--two-thirds"
                },
                new DisplayModeFallback
                {
                    Name = "Half width (1/2)",
                    Tag = DisplayOptionTags.Half,
                    LargeScreenWidth = 6,
                    MediumScreenWidth = 6,
                    SmallScreenWidth = 12,
                    ExtraSmallScreenWidth = 12,
                    Icon = "epi-icon__layout--half"
                },
                new DisplayModeFallback
                {
                    Name = "One third width (1/3)",
                    Tag = DisplayOptionTags.OneThird,
                    LargeScreenWidth = 4,
                    MediumScreenWidth = 6,
                    SmallScreenWidth = 12,
                    ExtraSmallScreenWidth = 12,
                    Icon = "epi-icon__layout--one-third"
                },
                new DisplayModeFallback
                {
                    Name = "One quarter width (1/4)",
                    Tag = DisplayOptionTags.OneQuarter,
                    LargeScreenWidth = 3,
                    MediumScreenWidth = 6,
                    SmallScreenWidth = 12,
                    ExtraSmallScreenWidth = 12,
                    Icon = "epi-icon__layout--one-quarter"
                },
                new DisplayModeFallback
                {
                    Name = "One sixth width (1/6)",
                    Tag = DisplayOptionTags.OneSixth,
                    LargeScreenWidth = 2,
                    MediumScreenWidth = 4,
                    SmallScreenWidth = 6,
                    ExtraSmallScreenWidth = 12
                }
            };
        }

        public static class DisplayOptionTags
        {
            public const string FullWidth = "displaymode-full";
            public const string ThreeQuarter = "displaymode-three-quarters";
            public const string TwoThird = "displaymode-two-thirds";
            public const string Half = "displaymode-half";
            public const string OneThird = "displaymode-one-third";
            public const string OneQuarter = "epi-icon__layout--one-quarter";
            public const string OneSixth = "displaymode-one-sixth";
        }
    }
}
