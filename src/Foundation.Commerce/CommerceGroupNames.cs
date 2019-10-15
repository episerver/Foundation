using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce
{
    [GroupDefinitions]
    public static class CommerceGroupNames
    {
        [Display(Order = 520)]
        public const string Commerce = "Commerce";
    }
}
