using System;
using System.Web;

namespace Foundation.Cms.Extensions
{
    public static class StringExtensions
    {
        public static bool IsLocalUrl(this string url, HttpRequestBase request)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var absoluteUri) && string.Equals(request.Url.Host,
                       absoluteUri.Host, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsNullOrEmpty(this string input) => string.IsNullOrEmpty(input);



        public static string MakeCompactString(this string str, int maxLength = 30, string subfix = "...")
        {
            string newStr = str;
            if (str.Length > maxLength)
                newStr = str.Substring(0, maxLength);

            return newStr + subfix;
        }
    }
}