using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc.Html;
using ImageProcessor.Web.Episerver;
using System.Web.Mvc;

namespace Foundation.Helpers
{
    public static class ImageUrlHelpers
    {
        /// <summary>
        /// Render a resized image URL with webp support to change format if supported
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="contentLink"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static string WebPFallbackImageUrl(this UrlHelper urlHelper, ContentReference contentLink, int? width = null, int? height = null)
        {
            var imageUrl = new UrlBuilder(urlHelper.ContentUrl(contentLink));
            imageUrl = imageUrl.Resize(width, height, ImageProcessor.Imaging.ResizeMode.Pad, ImageProcessor.Imaging.AnchorPosition.Center, true);
            if (WebPHelper.SupportsWebP(urlHelper.RequestContext.HttpContext.Request))
            {
                imageUrl.QueryCollection.Add("format", "webp");
            }
            return imageUrl.ToString();
        }
    }
}