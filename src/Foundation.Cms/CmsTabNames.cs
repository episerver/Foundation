using EPiServer.DataAnnotations;
using EPiServer.Security;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms
{
    [GroupDefinitions]
    public static class CmsTabNames
    {
        #region Groupnames for Tabs

        [Display(Order = 10)]
        public const string Default = "Default";

        [Display(Order = 20)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string MetaData = "Metadata";

        [Display(Name = "Block padding", Order = 30)]
        public const string BlockPadding = "BlockPadding";

        [Display(Order = 40)]
        public const string Teaser = "Teaser";

        [Display(Name = "Blog list", Order = 50)]
        public const string BlogList = "BlogList";

        [Display(Order = 60)]
        public const string Review = "Review";

        [Display(Order = 70)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Styles = "Styles";

        [Display(Order = 80)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Scripts = "Scripts";

        [Display(Order = 90)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Header = "Header";

        [Display(Order = 100)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Menu = "Menu";

        [Display(Order = 110)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Footer = "Footer";

        [Display(Order = 120)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Archives = "Archives";

        [Display(Order = 130)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Tags = "Tags";

        #endregion

        #region Groupnames for Content Types

        [Display(Order = 510)]
        public const string Content = "Content";

        [Display(Order = 530)]
        public const string Account = "Account";

        [Display(Order = 540)]
        public const string Blog = "Blog";

        [Display(Name = "Calendar event", Order = 550)]
        public const string CalendarEvent = "CalendarEvent";

        [Display(Order = 570)]
        public const string Forms= "Forms";

        [Display(Order = 580)]
        public const string Multimedia = "Multimedia";

        [Display(Order = 600)]
        public const string SocialMedia = "Social media";

        [Display(Order = 620)]
        public const string Syndication = "Syndication";

        #endregion
    }
}