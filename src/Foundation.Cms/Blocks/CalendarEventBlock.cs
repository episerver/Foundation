using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Foundation.Cms.EditorDescriptors;
using Foundation.Cms.Pages;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(GUID = "D5148C01-DFB0-4E57-8399-6CEEBF48F38E",
        DisplayName = "Calendar Event Block",
        Description = "Display list of events on calendar",
        GroupName = CmsGroupNames.CalendarEvent)]
    [ImageUrl("~/assets/icons/cms/pages/calendar.png")]
    public class CalendarEventBlock : FoundationBlockData
    {
        [Required]
        [CultureSpecific]
        [SelectOne(SelectionFactoryType = typeof(CalendarViewModeSelectionFactory))]
        [Display(Name = "View mode", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string ViewMode { get; set; }

        [Required]
        [AllowedTypes(typeof(FolderPage))]
        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual PageReference Root { get; set; }

        [Display(Name = "Number of events", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual int Count { get; set; }

        [Display(Name = "Category filter", GroupName = SystemTabNames.Content, Order = 40)]
        public virtual CategoryList CategoryFilter { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 50)]
        public virtual bool Recursive { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            ViewMode = "month";
        }
    }
}