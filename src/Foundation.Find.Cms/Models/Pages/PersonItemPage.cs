using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Labs.ContentManager.Cards;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Foundation.Cms.EditorDescriptors;
using Foundation.Cms.Pages;
using Foundation.Find.Cms.Facets.Config;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Find.Cms.Models.Pages
{
    [ContentType(DisplayName = "Person Item Page", 
        GUID = "b5af511b-96c9-4ad7-828f-254924542430", 
        Description = "Used to show info of specific person",
        GroupName = FindTabNames.Person)]
    [AvailableContentTypes(Availability.Specific, Exclude = new[] { typeof(PageData) })]
    [ImageUrl("~/assets/icons/cms/blocks/contact.png")]
    public class PersonItemPage : FoundationPageData
    {
        [CultureSpecific]
        [Display(Name = "Job title", GroupName = SystemTabNames.Content, Order = 1)]
        public virtual string JobTitle { get; set; }

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 2)]
        [SelectOne(SelectionFactoryType = typeof(LocationsSelectionFactory))]
        public virtual string Location { get; set; }

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 3)]
        [SelectOne(SelectionFactoryType = typeof(SectorsSelectionFactory))]
        public virtual string Sector { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 4)]
        public virtual string Phone { get; set; }

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 5)]
        public virtual string Email { get; set; }

        [UIHint(UIHint.Image)]
        [Display(Name = "Person image", GroupName = SystemTabNames.Content, Order = 6)]
        public virtual ContentReference Image { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        [Display(GroupName = SystemTabNames.Content, Order = 7)]
        public virtual XhtmlString About { get; set; }

        public override void SetItem(ItemModel itemModel)
        {
            itemModel.Description = About?.ToHtmlString();
            itemModel.Image = Image;
        }
    }
}