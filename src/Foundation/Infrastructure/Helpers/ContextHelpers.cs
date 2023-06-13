namespace Foundation.Infrastructure.Helpers
{
    public static class ContextHelpers
    {
        private static IHttpContextAccessor _httpContextAccessor;
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private static Uri GetAbsoluteUri()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = request.Scheme;
            uriBuilder.Host = request.Host.Host;
            uriBuilder.Path = request.Path.ToString();
            uriBuilder.Query = request.QueryString.ToString();
            return uriBuilder.Uri;
        }

        // Similar methods for Url/AbsolutePath which internally call GetAbsoluteUri
        public static string GetAbsoluteUrl() { return GetAbsoluteUri().ToString(); }
        public static string GetAbsolutePath() { return GetAbsoluteUri().AbsolutePath; }
    }
}
