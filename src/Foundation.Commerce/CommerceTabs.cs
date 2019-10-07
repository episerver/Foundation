using EPiServer.DataAnnotations;
using EPiServer.Security;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce
{
    [GroupDefinitions]
    public static class CommerceTabs
    {
        [Display(Name = "Search Settings", Order = 7)]
        public const string SearchSettings = "Search Settings";

        [Display(Order = 16)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Labels = "Site Labels";

        [Display(Order = 19)]
        public const string Manufacturer = "Manufacturer";

        [Display(Order = 21)]
        public const string Commerce = "Commerce";

        [Display(Order = 100)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string SiteStructure = "Site structure";

        [Display(Order = 110)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string MailTemplates = "Mail templates";
    }
}