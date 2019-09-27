using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Cms.Pages;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "New Products Page",
        GUID = "3ce903a3-3d48-4fe3-92f5-14b5e6f393b5",
        Description = "Show the top new products by sorted by the creation date",
        GroupName = CommerceTabs.Commerce)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-21.png")]
    public class NewProductsPage : FoundationPageData
    {
        [Display(Name = "Number of products")]
        public virtual int NumberOfProducts { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            NumberOfProducts = 12;
        }
    }
}