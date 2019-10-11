using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Personalization
{
    [GroupDefinitions]
    public static class CmsPersonalizationTabNames 
    {
        #region Groupnames for Content Types

        [Display(Name = "Cms personalization", Order = 590)]
        public const string CmsPersonalization = "CmsPersonalization";

        #endregion
    }
}
