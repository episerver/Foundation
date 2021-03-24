using EPiServer.Commerce.Order;
using Mediachase.Commerce.Orders;
using System;

namespace Foundation.Commerce.Extensions
{
    public static class OrderGroupExtensions
    {
        private static T DefaultIfNull<T>(object val, T defaultValue) => val == null || val == DBNull.Value ? defaultValue : (T)val;

        #region OrderGroup extensions

        public static bool IsQuoteCart(this OrderGroup orderGroup) => orderGroup is Cart && orderGroup.GetParentOrderId() != 0;

        public static int GetParentOrderId(this OrderGroup orderGroup) => orderGroup.GetIntegerValue(Constant.Quote.ParentOrderGroupId);

        public static int GetIntegerValue(this OrderGroup orderGroup, string fieldName) => orderGroup.GetIntegerValue(fieldName, 0);

        public static int GetIntegerValue(this OrderGroup orderGroup, string fieldName, int defaultValue)
        {
            if (orderGroup[fieldName] == null)
            {
                return defaultValue;
            }

            return int.TryParse(orderGroup[fieldName].ToString(), out var retVal) ? retVal : defaultValue;
        }

        public static string GetStringValue(this OrderGroup orderGroup, string fieldName) => DefaultIfNull(orderGroup[fieldName], string.Empty);

        #endregion

        #region ICart extensions

        public static bool IsQuoteCart(this ICart orderGroup) => orderGroup.GetParentOrderId() != 0;

        public static int GetParentOrderId(this ICart orderGroup) => orderGroup.GetIntegerValue(Constant.Quote.ParentOrderGroupId);

        public static int GetIntegerValue(this ICart orderGroup, string fieldName) => orderGroup.GetIntegerValue(fieldName, 0);

        public static int GetIntegerValue(this ICart orderGroup, string fieldName, int defaultValue)
        {
            if (orderGroup.Properties[fieldName] == null)
            {
                return defaultValue;
            }

            return int.TryParse(orderGroup.Properties[fieldName].ToString(), out var retVal) ? retVal : defaultValue;
        }

        #endregion
    }
}