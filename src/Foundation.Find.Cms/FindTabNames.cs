using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Find.Cms
{
    [GroupDefinitions]
    public static class FindTabNames
    {
        #region Groupnames for Content Types

        [Display(Order = 100)]
        public const string Location = "Location";

        [Display(Order = 200)]
        public const string Person = "Person";
        #endregion
    }
}