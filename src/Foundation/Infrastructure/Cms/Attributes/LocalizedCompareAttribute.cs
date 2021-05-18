using EPiServer.Framework.Localization;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Infrastructure.Cms.Attributes
{
    public class LocalizedCompareAttribute : CompareAttribute
    {
        private readonly string _translationPath;

        public LocalizedCompareAttribute(string otherProperty, string translationPath)
            : base(otherProperty) => _translationPath = translationPath;

        public override string FormatErrorMessage(string name)
        {
            ErrorMessage = LocalizationService.Current.GetString(_translationPath);
            return base.FormatErrorMessage(name);
        }
    }
}