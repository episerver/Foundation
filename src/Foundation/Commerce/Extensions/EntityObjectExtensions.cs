using Mediachase.BusinessFoundation.Data.Business;
using System;

namespace Foundation.Commerce.Extensions
{
    public static class EntityObjectExtensions
    {
        public static string GetStringValue(this EntityObject item, string fieldName) => item.GetStringValue(fieldName, string.Empty);

        public static string GetStringValue(this EntityObject item, string fieldName, string defaultValue) => item[fieldName] != null ? item[fieldName].ToString() : defaultValue;

        public static DateTime GetDateTimeValue(this EntityObject item, string fieldName) => item.GetDateTimeValue(fieldName, DateTime.MinValue);

        public static DateTime GetDateTimeValue(this EntityObject item, string fieldName, DateTime defaultValue)
        {
            if (item[fieldName] == null)
            {
                return defaultValue;
            }

            return DateTime.TryParse(item[fieldName].ToString(), out var retVal) ? retVal : defaultValue;
        }

        public static int GetIntegerValue(this EntityObject item, string fieldName) => item.GetIntegerValue(fieldName, 0);

        public static int GetIntegerValue(this EntityObject item, string fieldName, int defaultValue)
        {
            if (item[fieldName] == null)
            {
                return defaultValue;
            }

            return int.TryParse(item[fieldName].ToString(), out var retVal) ? retVal : defaultValue;
        }

        public static float GetFloatValue(this EntityObject item, string fieldName) => item.GetFloatValue(fieldName, 0);

        public static float GetFloatValue(this EntityObject item, string fieldName, float defaultValue)
        {
            if (item[fieldName] == null)
            {
                return defaultValue;
            }

            return float.TryParse(item[fieldName].ToString(), out var retVal) ? retVal : defaultValue;
        }

        public static decimal GetDecimalValue(this EntityObject item, string fieldName) => item.GetDecimalValue(fieldName, 0);

        public static decimal GetDecimalValue(this EntityObject item, string fieldName, decimal defaultValue)
        {
            if (item[fieldName] == null)
            {
                return defaultValue;
            }

            return decimal.TryParse(item[fieldName].ToString(), out var retVal) ? retVal : defaultValue;
        }

        public static bool GetBoolValue(this EntityObject item, string fieldName) => item.GetBoolValue(fieldName, false);

        public static bool GetBoolValue(this EntityObject item, string fieldName, bool defaultValue)
        {
            if (item[fieldName] == null)
            {
                return defaultValue;
            }

            return bool.TryParse(item[fieldName].ToString(), out var retVal) ? retVal : defaultValue;
        }

        public static Guid GetGuidValue(this EntityObject item, string fieldName) => item.GetGuidValue(fieldName, Guid.Empty);

        public static Guid GetGuidValue(this EntityObject item, string fieldName, Guid defaultValue)
        {
            if (item[fieldName] == null)
            {
                return defaultValue;
            }

            return Guid.TryParse(item[fieldName].ToString(), out var retVal) ? retVal : defaultValue;
        }
    }
}