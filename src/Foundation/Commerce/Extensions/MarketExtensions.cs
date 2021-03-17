namespace Foundation.Commerce.Extensions
{
    public static class MarketExtensions
    {
        public static string MarketCodeAdapter(this string countryCode)
        {
            switch (countryCode)
            {
                case "USA":
                    return "US";
                case "GBR":
                    return "UK";
                case "ESP":
                    return "ESP";
                case "AFG":
                    return "AF";
                case "ALB":
                    return "AL";
                case "AUS":
                    return "AUS";
                case "BRA":
                    return "BRA";
                case "CAN":
                    return "CAN";
                case "CHL":
                    return "CHL";
                case "DEU":
                    return "DEU";
                case "JPN":
                    return "JPN";
                case "NLD":
                    return "NLD";
                case "NOR":
                    return "NOR";
                case "SAU":
                    return "SAU";
                case "SWE":
                    return "SWE";
                default:
                    return "US";
            }
        }
    }
}
