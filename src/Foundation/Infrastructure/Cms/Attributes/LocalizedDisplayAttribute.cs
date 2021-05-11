using EPiServer.Framework.Localization;
using System.ComponentModel;

namespace Foundation.Infrastructure.Cms.Attributes
{
    public class LocalizedDisplayAttribute : DisplayNameAttribute
    {
        public LocalizedDisplayAttribute(string displayNameKey)
            : base(displayNameKey)
        {
        }

        public override string DisplayName
        {
            get
            {
                var s = LocalizationService.Current.GetString(base.DisplayName);
                return string.IsNullOrWhiteSpace(s) ? base.DisplayName : s;
            }
        }
    }
}