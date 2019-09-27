using System.Web;

namespace Foundation.Helpers
{
    public static class WebPHelper
    {
        /// <summary>
        /// Does the requesting browser support WebP
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        public static bool SupportsWebP(HttpRequestBase httpRequest)
        {
            try
            {
                var acceptHeader = httpRequest.Headers["ACCEPT"];
                return acceptHeader.Contains("image/webp");
            }
            catch
            {
                return false;
            }
        }
    }
}