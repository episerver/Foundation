using EPiServer.Framework.Localization;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Attributes
{
    public class LocalizedRequiredAttribute : RequiredAttribute
    {
        private readonly string _translationPath;

        public LocalizedRequiredAttribute(string translationPath) => _translationPath = translationPath;

        public override string FormatErrorMessage(string name)
        {
            ErrorMessage = LocalizationService.Current.GetString(_translationPath);
            return base.FormatErrorMessage(name);
        }
    }
}