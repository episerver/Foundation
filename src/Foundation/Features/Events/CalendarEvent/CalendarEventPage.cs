using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Events.CalendarEvent
{
    [ContentType(DisplayName = "Calendar Event Page",
        GUID = "f086fd08-4e54-4eb9-8367-c45630415226",
        GroupName = GroupNames.Calendar,
        Description = "Used to define an Event")]
    [ImageUrl("/icons/cms/pages/calendar.png")]
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