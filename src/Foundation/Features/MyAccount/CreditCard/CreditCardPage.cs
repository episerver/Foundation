using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Features.Shared.Descriptors;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.MyAccount.CreditCard
{
    [ContentType(DisplayName = "Credit Card Page",
        GUID = "adad362c-4f73-4592-abb9-093f6e7bb7c6",
        Description = "Manage credit cards",
        AvailableInEditMode = false,
        GroupName = GroupNames.Commerce)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-14.png")]
    public class CreditCardPage : FoundationPageData, IDisableOPE
    {
        [Display(GroupName = SystemTabNames.Content, Order = 200)]
        [CultureSpecific]
        public virtual bool B2B { get; set; }
    }
}