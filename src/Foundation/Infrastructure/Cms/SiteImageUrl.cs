using EPiServer.DataAnnotations;

namespace Foundation.Infrastructure.Cms
{
    /// <summary>
    /// Attribute to set the default thumbnail for site page and block types
    /// </summary>
    public class SiteImageUrl : ImageUrlAttribute
    {
        /// <summary>
        /// The parameterless constructor will initialize a SiteImageUrl attribute with a default thumbnail
        /// </summary>
        public SiteImageUrl() : base("/icons/gfx/page-type-thumbnail.png")
        {

        }

        public SiteImageUrl(string path) : base(path)
        {

        }
    }
}
