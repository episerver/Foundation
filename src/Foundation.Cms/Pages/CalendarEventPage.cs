using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Pages
{
    [ContentType(DisplayName = "Calendar Event Page",
        GUID = "f086fd08-4e54-4eb9-8367-c45630415226",
        GroupName = CmsGroupNames.Calendar,
        Description = "Used to define an Event")]
    [ImageUrl("~/assets/icons/cms/pages/calendar.png")]
    public class CalendarEventPage : FoundationPageData
    {
        [CultureSpecific]
        [Display(Name = "Start date", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual DateTime EventStartDate { get; set; }

        [CultureSpecific]
        [Display(Name = "End date", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual DateTime EventEndDate { get; set; }

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 30)]
        public virtual string Location { get; set; }
    }
}