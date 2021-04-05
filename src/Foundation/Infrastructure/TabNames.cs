using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Security;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Infrastructure
{
    [GroupDefinitions]
    public static class TabNames
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

        [Display(Name = "Search settings", Order = 65)]
        public const string SearchSettings = "SearchSettings";

        [Display(Order = 70)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Menu = "Menu";

        [Display(Name = "Site labels", Order = 75)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string SiteLabels = "SiteLabels";

        [Display(Order = 76)]
        public const string Manufacturer = "Manufacturer";

        [Display(Name = "Site structure", Order = 77)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string SiteStructure = "SiteStructure";

        [Display(Name = "Mail templates", Order = 78)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string MailTemplates = "MailTemplates";

        [Display(Order = 80)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Archives = "Archives";

        [Display(Order = 90)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Tags = "Tags";

        [Display(Order = 100)]
        public const string Location = "Location";

        [Display(Order = 200)]
        public const string Person = "Person";

        [Display(Order = 250)]
        public const string Teaser = "Teaser";

        [Display(Order = 260)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string MetaData = "Metadata";

        [Display(Name = "Custom settings", Order = 265)]
        public const string CustomSettings = "CustomSettings";

        [Display(Order = 270)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Styles = "Styles";

        [Display(Order = 280)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Scripts = "Scripts";

        [Display(Name = "Text", Order = 281)]
        public const string Text = "Text";

        [Display(Name = "Background", Order = 283)]
        public const string Background = "Background";

        [Display(Name = "Border", Order = 284)]
        public const string Border = "Border";

        [Display(Name = "Colors", Order = 285)]
        public const string Colors = "Colors";

        [Display(Name = "Image", Order = 286)]
        public const string Image = "Image";

        [Display(Name = "Block styling", Order = 287)]
        public const string BlockStyling = "BlockStyling";

        [Display(Name = "Button", Order = 287)]
        public const string Button = "Button";

        [Display(Name = "Settings", Order = 290)]
        public const string Settings = SystemTabNames.Settings;
    }
}