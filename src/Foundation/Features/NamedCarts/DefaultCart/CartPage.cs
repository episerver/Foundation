using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.NamedCarts.DefaultCart
{
    [ContentType(DisplayName = "Cart Page",
        GUID = "4d32f8b1-7651-49db-88e2-cdcbec8ed11c",
        Description = "Page for managing cart",
        GroupName = GroupNames.Commerce,
        AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-08.png")]
    public class CartPage : FoundationPageData
    {
        [CultureSpecific]
        [Display(Name = "Bottom content area", GroupName = SystemTabNames.Content, Order = 300)]
        public virtual ContentArea BottomContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Show Recommendations", Order = 50, Description = "This will determine whether or not to show recommendations.")]
        public virtual bool ShowRecommendations { get; set; }

        public override void SetDefaultValues(ContentType contentType) => ShowRecommendations = true;
    }
}