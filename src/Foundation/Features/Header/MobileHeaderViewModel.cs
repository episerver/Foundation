using EPiServer.SpecializedProperties;
using Foundation.Features.Home;

namespace Foundation.Features.Header
{
    public class MobileHeaderViewModel
    {
        public MegaMenuModel MenuModel { get; set; }

        public LinkItemCollection Pages { get; set; }

        public HomePage StartPage { get; set; }
    }
}
