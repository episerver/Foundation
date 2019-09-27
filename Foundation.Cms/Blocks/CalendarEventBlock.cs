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
        GroupName = "Calendar Event")]
    [ImageUrl("~/assets/icons/cms/pages/calendar.png")]
    public class CalendarEventBlock : FoundationBlockData
    {
        [Required]
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Name = "View Mode", Order = 1)]
        [SelectOne(SelectionFactoryType = typeof(CalendarViewModeSelectionFactory))]
        public virtual string ViewMode { get; set; }

        [Required]
        [Display(GroupName = SystemTabNames.Content, Order = 2)]
        [AllowedTypes(typeof(FolderPage))]
        public virtual PageReference Root { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 3)]
        public virtual int Count { get; set; }

        [Display(GroupName = SystemTabNames.Content, Name = "Category Filter", Order = 4)]
        public virtual CategoryList CategoryFilter { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 5)]
        public virtual bool Recursive { get; set; }

        #region IInitializableContent

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            ViewMode = "month";
        }

        #endregion
    }
}