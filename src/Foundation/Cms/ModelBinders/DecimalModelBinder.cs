using System;
using System.Globalization;
using System.Web.Mvc;

namespace Foundation.Cms.ModelBinders
{
    public class DecimalModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext,
                                         ModelBindingContext bindingContext)
        {
            object result = null;

            var modelName = bindingContext.ModelName;
            var attemptedValue =
                bindingContext.ValueProvider.GetValue(modelName).AttemptedValue;

            // Depending on CultureInfo, the NumberDecimalSeparator can be "," or "."
            // Both "." and "," should be accepted, but aren't.
            var wantedSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
            var alternateSeparator = (wantedSeparator == "," ? "." : ",");

            if (attemptedValue.IndexOf(wantedSeparator) == -1
                && attemptedValue.IndexOf(alternateSeparator) != -1)
            {
                attemptedValue =
                    attemptedValue.Replace(alternateSeparator, wantedSeparator);
            }

            if (bindingContext.ModelMetadata.IsNullableValueType
                && string.IsNullOrWhiteSpace(attemptedValue))
            {
                return null;
            }

            try
            {
                result = decimal.Parse(attemptedValue, NumberStyles.Any);
            }
            catch (FormatException e)
            {
                bindingContext.ModelState.AddModelError(modelName, e);
            }

            return result;
        }
    }
}
