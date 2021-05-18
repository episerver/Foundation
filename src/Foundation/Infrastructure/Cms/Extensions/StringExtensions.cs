using Microsoft.AspNetCore.Http;
using System;

namespace Foundation.Infrastructure.Cms.Extensions
{
    public static class StringExtensions
    {
        public static bool IsLocalUrl(this string url, HttpRequest request)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var absoluteUri) && string.Equals(request.Host.Host,
                       absoluteUri.Host, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsNullOrEmpty(this string input) => string.IsNullOrEmpty(input);

        public static bool IsEmpty(this string input) => input.Equals("");

        public static string MakeCompactString(this string str, int maxLength = 30, string suffix = "...")
        {
            var newStr = string.IsNullOrEmpty(str) ? string.Empty : str;
            var strLength = string.IsNullOrEmpty(str) ? 0 : str.Length;
            if (strLength > maxLength)
                newStr = str?.Substring(0, maxLength);

            return newStr + suffix;
        }
    }
}