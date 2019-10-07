using EPiServer.Core;
using EPiServer.DataAnnotations;
using Foundation.Cms.Blocks;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce.Models.Blocks
{
    [ContentType(DisplayName = "Elevated Role Block",
        GUID = "DD114EBB-2027-4B81-816E-3B228D121DD8",
        Description = "Elevated Role Block that uses access rights for read",
        GroupName = "Content")]
    [ImageUrl("~/assets/icons/cms/pages/elected.png")]
    public class ElevatedRoleBlock : FoundationBlockData
    {
        [Display(Name = "Main Content Area")]
        public virtual ContentArea MainContentArea { get; set; }

        [Display(Name = "Main Body")]
        public virtual XhtmlString Body { get; set; }


    }
}