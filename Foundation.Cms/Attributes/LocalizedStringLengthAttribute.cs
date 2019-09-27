using EPiServer.Framework.Localization;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Attributes
{
    public class LocalizedStringLengthAttribute : StringLengthAttribute
    {
        private readonly string _translationPath;

        public LocalizedStringLengthAttribute(string translationPath, int maximumLength)
            : base(maximumLength) => _translationPath = translationPath;

        public LocalizedStringLengthAttribute(string translationPath, int minimumLength, int maximumLength)
            : base(maximumLength)
        {
            _translationPath = translationPath;
            MinimumLength = minimumLength;
        }

        public override string FormatErrorMessage(string name)
        {
            ErrorMessage = LocalizationService.Current.GetString(_translationPath);
            return base.FormatErrorMessage(name);
        }
    }
}