using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Find.Cms
{
    [GroupDefinitions]
    public static class FindTabs
    {
        [Display(Name = "Location", Order = 8)]
        public const string Location = "Location";
    }
}