using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Infrastructure
{
    [GroupDefinitions]
    public static class GroupNamesCustom
    {
        [Display(Order = 590)]
        public const string Odp = "ODP";
    }
}
