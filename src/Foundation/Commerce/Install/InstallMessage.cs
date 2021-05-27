using System;
using System.Globalization;

namespace Foundation.Commerce.Install
{
    public class InstallMessage
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public DateTime TimeStamp { get; set; }

        public string ToHtmlString()
        {
            return string.Format(CultureInfo.CurrentUICulture, "<span class=\"{0}\">{1}: {2}</span>",
                Error ? "epi-danger" : string.Empty,
                TimeStamp.ToString("T"),
                Message);
        }
    }
}
