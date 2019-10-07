using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Security;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms
{
    [GroupDefinitions]
    public static class CmsTabs
    {
        [Display(Order = 0)]
        public const string Content = SystemTabNames.Content;

        [Display(Name = "Default", Order = 1)]
        public const string Default = "Default";

        [Display(Name = "Block Padding", Order = 2)]
        public const string BlockPadding = "Block Padding";

        [Display(Name = "Metadata", Order = 3)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string MetaData = "Metadata";

        [Display(Name = "Page Settings", Order = 4)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string PageSettings = "Page Settings";

        [Display(Name = "Site Settings", Order = 6)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string SiteSettings = "Site Settings";

        [Display(Name = "Teaser", Order = 10)]
        public const string Teaser = "Teaser";

        [Display(Name = "Review", Order = 11)]
        public const string Review = "Review";

        [Display(Name = "Styles", Order = 12)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Styles = "Styles";

        [Display(Name = "Scripts", Order = 13)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Scripts = "Scripts";

        [Display(Name = "Menu", Order = 14)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Menu = "Menu";

        [Display(Name = "Footer", Order = 15)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Footer = "Footer";

        [Display(Order = 17)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Archive = "Archives";

        [Display(Order = 18)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Tags = "Tags";

        [Display(Order = 20)]
        public const string Blog = "Blog";

        [Display(Order = 21)]
        public const string Account = "Account";

        [Display(Order = 996)]
        public const string Social = "Social";
    }
}