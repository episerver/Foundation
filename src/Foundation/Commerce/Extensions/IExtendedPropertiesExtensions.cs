using EPiServer.Commerce.Storage;
using System;

namespace Foundation.Commerce.Extensions
{
    public static class ExtendedPropertiesExtensions
    {
        public static string GetString(this IExtendedProperties extendedProperties, string fieldName) => DefaultIfNull(extendedProperties.Properties[fieldName], string.Empty);

        public static bool GetBool(this IExtendedProperties extendedProperties, string fieldName) => DefaultIfNull(extendedProperties.Properties[fieldName], false);

        public static Guid GetGuid(this IExtendedProperties extendedProperties, string fieldName) => DefaultIfNull(extendedProperties.Properties[fieldName], Guid.Empty);

        public static int GetInt32(this IExtendedProperties extendedProperties, string fieldName) => DefaultIfNull(extendedProperties.Properties[fieldName], default(int));

        public static DateTime GetDateTime(this IExtendedProperties extendedProperties, string fieldName) => DefaultIfNull(extendedProperties.Properties[fieldName], DateTime.MaxValue);

        public static decimal GetDecimal(this IExtendedProperties extendedProperties, string fieldName) => DefaultIfNull(extendedProperties.Properties[fieldName], default(decimal));

        private static T DefaultIfNull<T>(object val, T defaultValue) => val == null || val == DBNull.Value ? defaultValue : (T)val;
    }
}