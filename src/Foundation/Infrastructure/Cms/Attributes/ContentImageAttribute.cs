using EPiServer.DataAnnotations;

namespace Foundation.Infrastructure.Cms.Attributes
{
    public class ContentImageAttribute : ImageUrlAttribute
    {
        public ContentImageAttribute() : base("/Content/ContentIcons/default.png")
        {
        }

        public ContentImageAttribute(string path) : base(path.Contains('/') ? path : "~/Content/ContentIcons/" + path)
        {
        }
    }
}