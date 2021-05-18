using EPiServer.Framework.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Infrastructure.Find.Facets.Config
{
    public static class EnumHelpers
    {
        public static IList<SelectListItem> GetSelectListItems<TEnum>(IList<string> selectedValues = null) where TEnum : struct, IConvertible
        {
            var list = new List<SelectListItem>();

            var values = Enum.GetValues(typeof(TEnum));

            foreach (var value in values)
            {
                list.Add(new SelectListItem
                {
                    Text = GetValueName<TEnum>(value),
                    Value = value.ToString(),
                    Selected = selectedValues != null && selectedValues.Any(x => value.ToString().Equals(x, StringComparison.InvariantCultureIgnoreCase))
                });
            }

            return list;
        }

        public static string GetValueName<TEnum>(object value) where TEnum : struct, IConvertible
        {
            if (value == null)
            {
                return string.Empty;
            }

            TEnum enumValue;

            if (value is TEnum)
            {
                enumValue = (TEnum)value;
            }
            else
            {
                Enum.TryParse(value.ToString(), true, out enumValue);
            }

            var staticEnumName = Enum.GetName(typeof(TEnum), enumValue);

            if (LocalizationService.Current.TryGetString($"/Enum/{typeof(TEnum).Name}/{staticEnumName}", out string localizedEnumName))
            {
                return localizedEnumName;
            }

            return staticEnumName;
        }
    }
}
