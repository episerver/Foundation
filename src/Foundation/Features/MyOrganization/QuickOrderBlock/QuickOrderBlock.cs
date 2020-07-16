using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Security;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using Mediachase.Commerce.Security;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.MyOrganization.QuickOrderBlock
{
    [ContentType(DisplayName = "Quick Order Block",
        GUID = "003076FD-659C-485E-9480-254A447CC809",
        Description = "Used to quick order a list of products",
        GroupName = GroupNames.Commerce)]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-14.png")]
    public class QuickOrderBlock : FoundationBlockData
    {
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 5)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(Name = "Main body", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual XhtmlString MainBody { get; set; }

        [Ignore]
        [Display(Name = "Product list", Order = 20)]
        public List<QuickOrderProductViewModel> ProductsList { get; set; }

        [Ignore]
        [Display(Name = "Returned messages", Order = 30)]
        public List<string> ReturnedMessages { get; set; }

        [Ignore]
        public bool HasOrganization
        {
            get
            {
                var contact = PrincipalInfo.CurrentPrincipal.GetCustomerContact();
                return contact?.OwnerId != null;
            }
        }
    }
}