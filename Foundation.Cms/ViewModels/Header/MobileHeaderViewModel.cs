using EPiServer.SpecializedProperties;
using Foundation.Cms.Pages;

namespace Foundation.Cms.ViewModels.Header
{
    public class MobileHeaderViewModel
    {
        public MegaMenuModel MenuModel { get; set; }

        public LinkItemCollection Pages { get; set; }

        public CmsHomePage StartPage { get; set; }
    }
}
