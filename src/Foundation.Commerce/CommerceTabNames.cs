using EPiServer.DataAnnotations;
using EPiServer.Security;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce
{
    [GroupDefinitions]
    public static class CommerceTabNames
    {
        #region Groupnames for Tabs

        [Display(Name = "Search settings", Order = 65)]
        public const string SearchSettings = "SearchSettings";

        [Display(Name = "Site labels", Order = 66)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string SiteLabels = "SiteLabels";

        [Display(Order = 67)]
        public const string Manufacturer = "Manufacturer";

        [Display(Name = "Site structure", Order = 140)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string SiteStructure = "SiteStructure";

        [Display(Name = "Mail templates", Order = 150)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string MailTemplates = "MailTemplates";

        #endregion

        #region Groupnames for Content Types

        [Display(Order = 520)]
        public const string Commerce = "Commerce";

        #endregion
    }
}