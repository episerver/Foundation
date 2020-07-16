namespace Foundation.Infrastructure.Display
{
    /// <summary>
    /// Defines resolution for desktop displays
    /// </summary>
    public class StandardResolution : DisplayResolutionBase
    {
        public StandardResolution() : base("/resolutions/standard", 1366, 768)
        {
        }
    }

    /// <summary>
    /// Defines resolution for a horizontal iPad
    /// </summary>
    public class IpadHorizontalResolution : DisplayResolutionBase
    {
        public IpadHorizontalResolution() : base("/resolutions/ipadhorizontal", 1024, 768)
        {
        }
    }

    /// <summary>
    /// Defines resolution for a vertical iPhone 5s
    /// </summary>
    public class IphoneVerticalResolution : DisplayResolutionBase
    {
        public IphoneVerticalResolution() : base("/resolutions/iphonevertical", 320, 568)
        {
        }
    }

    /// <summary>
    /// Defines resolution for a vertical Android handheld device
    /// </summary>
    public class AndroidVerticalResolution : DisplayResolutionBase
    {
        public AndroidVerticalResolution() : base("/resolutions/androidvertical", 480, 800)
        {
        }
    }

    /// <summary>
    /// Defines resolution for a vertical iPhone 11
    /// </summary>
    public class Iphone11Resolution : DisplayResolutionBase
    {
        public Iphone11Resolution() : base("/resolutions/iphone11", 320, 692)
        {
        }
    }

    /// <summary>
    /// Defines resolution for a vertical iPad Air
    /// </summary>
    public class IpadAirResolution : DisplayResolutionBase
    {
        public IpadAirResolution() : base("/resolutions/ipadair", 519, 692)
        {
        }
    }
}