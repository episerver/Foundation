﻿namespace Foundation.Infrastructure.Cms.Attributes
{
    public class LocalizedRegularExpressionAttribute : RegularExpressionAttribute
    {
        private readonly string _name;

        public LocalizedRegularExpressionAttribute(string pattern, string name)
            : base(pattern) => _name = name;

        public override string FormatErrorMessage(string name)
        {
            ErrorMessage = LocalizationService.Current.GetString(_name);
            return base.FormatErrorMessage(name);
        }
    }
}