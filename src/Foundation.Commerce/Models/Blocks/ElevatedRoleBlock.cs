using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Cms.Blocks;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce.Models.Blocks
{
    [ContentType(DisplayName = "Elevated Role Block",
        GUID = "DD114EBB-2027-4B81-816E-3B228D121DD8",
        Description = "Elevated Role Block that uses access rights for read")]
    [ImageUrl("~/assets/icons/cms/pages/elected.png")]
    public class ElevatedRoleBlock : FoundationBlockData
    {
        [Display(Name = "Main content area")]
        public virtual ContentArea MainContentArea { get; set; }

        [Display(Name = "Main body")]
        public virtual XhtmlString Body { get; set; }
    }
}