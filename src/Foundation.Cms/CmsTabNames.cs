using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Security;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms
{
    [GroupDefinitions]
    public static class CmsTabNames
    {
        [Display(Order = 10)]
        public const string Default = "Default";

        [Display(Name = "Blog list", Order = 30)]
        public const string BlogList = "BlogList";

        [Display(Order = 40)]
        public const string Review = "Review";

        [Display(Order = 50)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Header = "Header";

        [Display(Order = 60)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Footer = "Footer";

        [Display(Order = 70)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Menu = "Menu";

        [Display(Order = 80)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Archives = "Archives";

        [Display(Order = 90)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Tags = "Tags";

        [Display(Order = 250)]
        public const string Teaser = "Teaser";

        [Display(Order = 260)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string MetaData = "Metadata";

        [Display(Name = "Custom Settings", Order = 265)]
        public const string CustomSettings = "CustomSettings";

        [Display(Order = 270)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Styles = "Styles";

        [Display(Order = 280)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Scripts = "Scripts";

        [Display(Name = "Block styling", Order = 285)]
        public const string BlockStyling = "BlockStyling";

        [Display(Name = "Settings", Order = 290)]
        public const string Settings = SystemTabNames.Settings;
    }
}