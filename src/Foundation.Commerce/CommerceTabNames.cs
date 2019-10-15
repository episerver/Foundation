using EPiServer.DataAnnotations;
using EPiServer.Security;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce
{
    [GroupDefinitions]
    public static class CommerceTabNames
    {
        [Display(Name = "Search settings", Order = 65)]
        public const string SearchSettings = "SearchSettings";

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
    }
}