using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Personalization
{
    [GroupDefinitions]
    public static class CmsPersonalizationGroupNames
    {
        [Display(Name = "Cms personalization", Order = 590)]
        public const string CmsPersonalization = "CmsPersonalization";
    }
}
