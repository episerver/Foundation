using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Foundation.Cms.Extensions
{
    public static class XElementExtensions
    {
        public static string Get(this XElement xElement, string elementName) => (string)xElement.Element(elementName);

        public static string GetAttribute(this XElement xElement, string attributeName) => (string)xElement.Attribute(attributeName);

        public static string GetStringOrEmpty(this XElement xElement, string elementName) => (string)xElement.Element(elementName) ?? string.Empty;

        public static string GetStringOrNull(this XElement xElement, string elementName)
        {
            var element = xElement.Element(elementName);
            return element != null && !element.IsEmpty ? (string)element : null;
        }

        public static int GetInt(this XElement xElement, string elementName) => int.Parse((string)xElement.Element(elementName), CultureInfo.InvariantCulture);

        public static int GetIntOrDefault(this XElement xElement, string elementName, int defaultValue = 0)
        {
            if (int.TryParse((string)xElement.Element(elementName), NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
            {
                return value;
            }
            return defaultValue;
        }

        public static bool GetBool(this XElement xElement, string elementName) => bool.Parse((string)xElement.Element(elementName));

        public static bool GetBoolOrDefault(this XElement xElement, string elementName)
        {
            bool.TryParse((string)xElement.Element(elementName), out var value);
            return value;
        }

        public static decimal GetDecimal(this XElement xElement, string elementName) => decimal.Parse((string)xElement.Element(elementName), CultureInfo.InvariantCulture);

        public static decimal GetDecimalOrDefault(this XElement xElement, string elementName)
        {
            decimal.TryParse((string)xElement.Element(elementName), NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue);
            return parsedValue;
        }

        public static double GetDoubleOrDefault(this XElement xElement, string elementName)
        {
            double.TryParse((string)xElement.Element(elementName), NumberStyles.Float, CultureInfo.InvariantCulture, out var value);
            return value;
        }

        public static IEnumerable<string> GetEnumerable(this XElement xElement, string elementName, char seperator)
        {
            var value = (string)xElement.Element(elementName);
            if (value.IsNullOrEmpty())
            {
                return Enumerable.Empty<string>();
            }
            return value.Split(new[] { seperator }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static T GetAs<T>(this XElement xElement, string elementName) where T : new() => (T)Activator.CreateInstance(typeof(T), (string)xElement.Element(elementName));

        public static IEnumerable<XElement> GetChildren(this XElement xElement, string childElementName)
        {
            if (xElement == null || xElement.IsEmpty)
            {
                return Enumerable.Empty<XElement>();
            }
            return xElement.Elements(childElementName);
        }
    }
}
