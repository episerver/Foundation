namespace Foundation.Infrastructure.Cms.Attributes
{
    public class LocalizedRegularExpressionAttribute : RegularExpressionAttribute
    {
        private readonly string _name;

        public LocalizedRegularExpressionAttribute(string pattern, string name)
            : base(pattern) => _name = name;

        public override string FormatErrorMessage(string name)
        {
            // .NET 10: setting ErrorMessage then calling base throws conflict; return directly.
            return LocalizationService.Current.GetString(_name) ?? name + " is invalid.";
        }
    }
}