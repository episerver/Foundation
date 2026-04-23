namespace Foundation.Infrastructure.Cms.Attributes
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
            // .NET 10: setting ErrorMessage then calling base throws conflict; return directly.
            return LocalizationService.Current.GetString(_translationPath) ?? name + " exceeds allowed length.";
        }
    }
}