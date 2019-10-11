using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Social
{
    [GroupDefinitions]
    public static class SocialTabNames
    {
        #region Groupnames for Content Types

        [Display(Order = 610)]
        public const string Social = "Social";

        #endregion
    }
}
