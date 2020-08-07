using EPiServer.Core;

namespace Foundation.Features.Header
{
    public class HeaderLogoViewModel
    {
        public bool LargeHeaderMenu { get; set; }
        public string HeaderMenuStyle { get; set; }
        public ContentReference SiteLogo { get; set; }
    }
}