namespace Foundation.Infrastructure.Cms.Attributes
{
    public class LocalizedRequiredAttribute : RequiredAttribute
    {
        private readonly string _translationPath;

        public LocalizedRequiredAttribute(string translationPath) => _translationPath = translationPath;

        public override string FormatErrorMessage(string name)
        {
            // .NET 10: ValidationAttribute base classes now use a DefaultMessageFactory (Func<string>),
            // so setting this.ErrorMessage and then calling base.FormatErrorMessage throws
            // "Either ErrorMessageString or ErrorMessageResourceName must be set, but not both."
            // Return the localized string directly without mutating attribute state.
            return LocalizationService.Current.GetString(_translationPath) ?? name + " is required.";
        }
    }
}